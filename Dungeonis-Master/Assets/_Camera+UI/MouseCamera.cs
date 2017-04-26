namespace RPG.CameraUI
{
	using System.Collections;
	using UnityEngine;

	public class MouseCamera
		: MonoBehaviour
	{
		public float XSensitivity = 0.2f;
		public float YSensitivity = 0.2f;

		public float TargetHeight = 1.7f;
		public float Distance = 5.0f;
		public float OffsetFromWall = 0.1f;

		public float MaxDistance = 20;
		public float MinDistance = 0.6f;
		public float SpeedDistance = 5;

		public float xSpeed = 200.0f;
		public float ySpeed = 200.0f;

		public int yMinLimit = -40;
		public int yMaxLimit = 80;

		public int ZoomRate = 40;

		public float RotationDampening = 3.0f;
		public float ZoomDampening = 5.0f;

		public LayerMask CollisionLayers = -1;

		public bool CameraLock = true;

		private GameObject _player;
		private GameObject _cameraArm;
		private Transform _target;
		private float _xDeg = 0.0f;
		private float _yDeg = 0.0f;
		private float _currentDistance;
		private float _desiredDistance;
		private float _correctedDistance;

		private Quaternion _cameraArmTransformCache;
		private Quaternion _cameraTransformCache;

		public void ToggleCameraLock()
		{
			CameraLock = !CameraLock;
		}

		private void Awake()
		{
			_player = GameObject.FindGameObjectWithTag("Player");
			_cameraArm = GameObject.FindGameObjectWithTag("CameraArm");
		}

		private void Start()
		{
			Vector3 angles = transform.eulerAngles;
			_xDeg = angles.x;
			_yDeg = angles.y;

			_currentDistance = Distance;
			_desiredDistance = Distance;
			_correctedDistance = Distance;

			// Make the rigid body not change rotation
			if (_player.GetComponent<Rigidbody>())
				_player.GetComponent<Rigidbody>().freezeRotation = true;
		}

		/// <summary>
		/// Camera logic on LateUpdate to only update after all character movement logic has been handled.
		/// </summary>
		private void Update()
		{
			if (CameraLock)
			{
				Cursor.lockState = CursorLockMode.Locked;
				MoveCameraWithMouse();
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
			}
		}

		private void MoveCameraWithMouse()
		{

			float x = Input.GetAxis("Mouse X") * XSensitivity;
			float y = Input.GetAxis("Mouse Y") * YSensitivity;


			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				// cache camera transforms
				_cameraArmTransformCache = _cameraArm.transform.rotation;
				_cameraTransformCache    = transform.rotation;
			}
			else if (Input.GetKeyUp(KeyCode.Mouse1))
			{
				StartCoroutine("ResetCameraPosition");
				// reset camera position to default
				//_cameraArm.transform.rotation = Quaternion.Lerp(_cameraArm.transform.rotation, _cameraArmTransformCache, 5.0f * Time.deltaTime);
				//transform.rotation  = Quaternion.Lerp(transform.rotation, _cameraTransformCache, 5.0f * Time.deltaTime);
			}


			if (Input.GetKey(KeyCode.Mouse1))
			{
				_cameraArm.transform.rotation *= Quaternion.Euler(0f, x, 0f);
				//transform.rotation            *= Quaternion.Euler(-y, 0f, 0f);
			}
			else
			{
				_player.transform.rotation *= Quaternion.Euler(0f, x, 0f);
			}

			//transform.localRotation = Quaternion.Euler(0f, x * 25, 0f);
		}

		private IEnumerator ResetCameraPosition()
		{
			while (Quaternion.Angle(_cameraArm.transform.rotation, _cameraArmTransformCache) > 0.3f)
			{
				_cameraArm.transform.rotation = Quaternion.Slerp(_cameraArm.transform.rotation, _cameraArmTransformCache, 2.0f * Time.deltaTime);

				yield return null;
			}
		}
	}
}
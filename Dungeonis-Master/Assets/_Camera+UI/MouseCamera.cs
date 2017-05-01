namespace RPG.CameraUI
{
	using System.Collections;
	using UnityEngine;

	public class MouseCamera
		: MonoBehaviour
	{
		[Header("Camera Settings")]
		public float SensitivityX = 4.0f;
		public float SensitivityY = 4.0f;
		[SerializeField] private float _minAngleY = -50.0f;
		[SerializeField] private float _maxAngleY = 50.0f;

		[Header("Camera Clipping Settings")]
		public LayerMask CollisionLayers = -1;
		public float TargetHeight = 1.7f;
		public float OffsetFromWall = 0.1f;

		[Header("Camera Zooming Settings")]
		public float MaxDistance = 20;
		public float MinDistance = 0.6f;
		public float Distance = 10.0f;
		public float ZoomRate = 40;
		public float SpeedDistance = 5.0f;
		public float ZoomDampening = 5.0f;

		public bool CameraLock = true;

		private Camera _mainCamera;
		private GameObject _cameraArm;
		private GameObject _player;
		private float _currentX = 0.0f;
		private float _currentY = 0.0f;

		private float _currentDistance;
		private float _desiredDistance;
		private float _correctedDistance;

		private void Awake()
		{
			_player = GameObject.FindGameObjectWithTag("Player");
			_cameraArm = GameObject.FindGameObjectWithTag("CameraArm");
			_mainCamera = Camera.main;

			CameraLock       = true;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void Start()
		{
			_currentDistance = Distance;
			_desiredDistance = Distance;
			_correctedDistance = Distance;

			// Make the rigid body not change rotation
			if (_player.GetComponent<Rigidbody>())
				_player.GetComponent<Rigidbody>().freezeRotation = true;
		}

		private void LateUpdate()
		{
			if (CameraLock)
			{
				// Let the mouse govern camera position
				_currentX += Input.GetAxis("Mouse X") * SensitivityX;
				_currentY -= Input.GetAxis("Mouse Y") * SensitivityY;
				_currentY = Mathf.Clamp(_currentY, _minAngleY, _maxAngleY);

				// calculate the desired distance
				_desiredDistance  -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomRate * SpeedDistance;
				_desiredDistance   = Mathf.Clamp(_desiredDistance, MinDistance, MaxDistance);
				_correctedDistance = _desiredDistance;

				// Calculate desired camera position
				Vector3 dir         = new Vector3(0f, 0f, -Distance);
				Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0f);
				Vector3 position    = _cameraArm.transform.position + rotation * dir;

				// Check for collision using the true target's desired registration point as set by user using height
				RaycastHit collisionHit;
				Vector3 trueTargetPosition = new Vector3(_cameraArm.transform.position.x, _cameraArm.transform.position.y, _cameraArm.transform.position.z);

				// If there was a collision, correct the camera position and calculate the corrected distance
				bool isCorrected = false;
				if (Physics.Linecast(trueTargetPosition, position, out collisionHit, CollisionLayers.value))
				{
					_correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - OffsetFromWall;
					isCorrected = true;
				}

				// For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
				_currentDistance = !isCorrected || _correctedDistance > _currentDistance ? Mathf.Lerp(_currentDistance, _correctedDistance, Time.deltaTime * ZoomDampening) : _correctedDistance;

				// Keep within legal limits
				_currentDistance = Mathf.Clamp(_currentDistance, MinDistance, MaxDistance);


				Vector3 dire = new Vector3(0f, 0f, -_currentDistance);

				_mainCamera.transform.position = _cameraArm.transform.position + rotation * dire;
				_mainCamera.transform.LookAt(_cameraArm.transform.position);
			}
		}

		public void ToggleCameraLock()
		{
			CameraLock = !CameraLock;
			Cursor.lockState = CameraLock ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}
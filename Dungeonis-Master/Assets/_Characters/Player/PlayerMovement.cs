namespace RPG.Characters
{
	using UnityEngine;
	using RPG.CameraUI;

	public class PlayerMovement
		: PlayerMovementBase
	{
		public float Speed = 10;

		[SerializeField] private Camera _cam;
		private MouseCamera _camComponent;
		private GameObject _camArm;
		private bool _jump;
		private Vector3 _groundNormal;

		private void Start()
		{
			if (Camera.main != null)
			{
				_cam          = Camera.main;
				_camComponent = _cam.GetComponent<MouseCamera>();
				_camArm       = GameObject.FindGameObjectWithTag("CameraArm");
			}
			else
			{
				Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftControl))
				_camComponent.ToggleCameraLock();

			if (!_jump)
				_jump = Input.GetButtonDown("Jump");
		}

		// Fixed update is called in sync with physics
		private void FixedUpdate()
		{
			if (_camComponent.CameraLock)
			{
				float horizontalAxis = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
				float verticalAxis   = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
				float mouse = Input.GetAxis("Mouse X");

				if (Input.GetKey(KeyCode.LeftShift))
				{
					horizontalAxis *= 0.5f;
					verticalAxis *= 0.5f;
				}

				Vector3 movement = verticalAxis * _cam.transform.forward + horizontalAxis * _cam.transform.right;
				Vector3 actualMovement = new Vector3(movement.x, 0f, movement.z);

				transform.Translate(actualMovement);

				if (horizontalAxis != 0 || verticalAxis != 0)
				{
					transform.LookAt(new Vector3());
				}

				//Move(movementVector, false, _jump); //Redesign PlayerMovementBase when we understand animation + blend trees
			}
		}
	}
}
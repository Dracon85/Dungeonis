namespace RPG.Characters
{
	using UnityEngine;
	using RPG.CameraUI;

	public class PlayerMovement
		: PlayerMovementBase
	{
		public float Speed = 10;

		private Camera _cam;
		private MouseCamera _camComponent;
		private bool _jump;

		private void Start()
		{
			if (Camera.main != null)
			{
				_cam = Camera.main;
				_camComponent = _cam.GetComponent<MouseCamera>();
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
				float verticalAxis = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

				if (Input.GetKey(KeyCode.LeftShift))
				{
					horizontalAxis *= 0.5f;
					verticalAxis *= 0.5f;
				}

				transform.Translate(horizontalAxis, 0, verticalAxis);
				Move(new Vector3(horizontalAxis, 0, verticalAxis), false, false);
			}
		}
	}
}
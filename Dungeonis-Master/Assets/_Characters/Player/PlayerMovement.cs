using UnityEngine;

namespace RPG.Characters{
[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement
	: MonoBehaviour
{
	public float speed = 10;

	private ThirdPersonCharacter _character;
	private Transform _cam;
	private Vector3 _move;
	private bool _jump;

	private void Start()
	{
		if (Camera.main != null)
			_cam = Camera.main.transform;
		else
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);

		// Get the third person character ( this should never be null due to require component )
		_character = GetComponent<ThirdPersonCharacter>();
	}

	private void Update()
	{
		if (!_jump)
			_jump = Input.GetButtonDown("Jump");
	}

	// Fixed update is called in sync with physics
	private void FixedUpdate()
	{
		float horizontalAxis = Input.GetAxis("Horizontal");
		float verticalAxis   = Input.GetAxis("Vertical");
		bool crouch          = Input.GetKey(KeyCode.C);     //TODO: Map this in menus

		// Calculate move direction to pass to character
		if (_cam != null)
		{
			_move = verticalAxis * _cam.forward.normalized + horizontalAxis * _cam.right.normalized;

			if (Input.GetKey(KeyCode.LeftShift))
				_move *= 0.5f;

			_character.Move(_move, crouch, _jump);
			_jump = false;
		}
	}
}
}
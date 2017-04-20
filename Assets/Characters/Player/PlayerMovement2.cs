using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement2: MonoBehaviour
{
	private ThirdPersonCharacter _character;
	private Transform _cam;
	private Vector3 _camForward;
	private Vector3 _move;
	private bool _jump;
	private Vector3 _movement;
	public float speed = 10;

	private void Start()
	{
		// get the transform of the main camera
		if (Camera.main != null)
			_cam = Camera.main.transform;
		else
		{
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		// get the third person character ( this should never be null due to require component )
		_character       = GetComponent<ThirdPersonCharacter>();
	}


	private void Update()
	{
		if (!_jump)
			_jump = Input.GetButtonDown("Jump");
	}

	// Fixed update is called in sync with physics
	private void FixedUpdate()
	{
		// read inputs

		float horizontalAxis = Input.GetAxis("Horizontal");
		float verticalAxis   = Input.GetAxis("Vertical");
		bool crouch          = Input.GetKey(KeyCode.C);     //TODO: Map this in menus

		// calculate move direction to pass to character
		if (_cam != null)
		{
			_move = verticalAxis* _cam.forward.normalized + horizontalAxis* _cam.right.normalized;

			// walk speed multiplier
			if (Input.GetKey(KeyCode.LeftShift))
				_move *= 0.5f;

			// pass all parameters to the character control script
			_character.Move(_move, crouch, _jump);
			_jump = false;
		}
	}
}
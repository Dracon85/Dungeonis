namespace RPG.Characters
{
	using UnityEngine;

	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Animator))]
	public class PlayerMovementBase
		: MonoBehaviour
	{
		[SerializeField] private bool _isGrounded;
		[SerializeField] private float _turnAmount;
		[SerializeField] private float _forwardAmount;
		[SerializeField] private bool _crouching;
		[SerializeField] private float _moveSpeedMultiplier = 1f;
		[SerializeField] private float _animSpeedMultiplier = 1f;
		[SerializeField] private float _runCycleLegOffset = 0.2f;

		[SerializeField] private float _movingTurnSpeed = 360;
		[SerializeField] private float _stationaryTurnSpeed = 180;
		[SerializeField] private float _jumpPower = 12f;
		[Range(1f, 4f)] [SerializeField] private float _gravityMultiplier = 2f;
		[SerializeField] private float _groundCheckDistance = 0.5f;

		[SerializeField]  private Animator _animator;
		[SerializeField]  private Rigidbody _rigidbody;
		[SerializeField] private CapsuleCollider _capsule;
		private const float HALF = 0.5f;
		private float _origGroundCheckDistance;
		private Vector3 _groundNormal;
		private float _capsuleHeight;
		private Vector3 _capsuleCenter;

		public void Move(Vector3 move, bool crouch, bool jump)
		{
			if (move.magnitude > 1f)
				move.Normalize();

			CheckGroundStatus();

			_turnAmount    = Mathf.Atan2(move.x, move.z);
			_forwardAmount = move.magnitude;

			Vector3 projectedMove = Vector3.ProjectOnPlane(move, _groundNormal);
			_turnAmount = Mathf.Atan2(projectedMove.x, projectedMove.z);



			/*if (_isGrounded)
				HandleGroundedMovement(crouch, jump);
			else
				HandleAirborneMovement();*/

			//ScaleCapsuleForCrouching(crouch);
			UpdateAnimator(move);
		}

		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (_isGrounded && Time.deltaTime > 0)
			{
				Vector3 v = (_animator.deltaPosition * _moveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = _rigidbody.velocity.y;
				_rigidbody.velocity = v;
			}
		}

		private void Start()
		{
			_animator                = GetComponent<Animator>();
			_rigidbody               = GetComponent<Rigidbody>();
			_capsule                 = GetComponent<CapsuleCollider>();
			_capsuleHeight           = _capsule.height;
			_capsuleCenter           = _capsule.center;
			_rigidbody.constraints   = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			_origGroundCheckDistance = _groundCheckDistance;
		}

		private void ScaleCapsuleForCrouching(bool crouch)
		{
			if (_isGrounded && crouch)
			{
				if (_crouching) return;
				_capsule.height = _capsule.height / 2f;
				_capsule.center = _capsule.center / 2f;
				_crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(_rigidbody.position + Vector3.up * _capsule.radius * HALF, Vector3.up);
				float crouchRayLength = _capsuleHeight - _capsule.radius * HALF;
				if (Physics.SphereCast(crouchRay, _capsule.radius * HALF, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					_crouching = true;
					return;
				}
				_capsule.height = _capsuleHeight;
				_capsule.center = _capsuleCenter;
				_crouching = false;
			}
		}

		private void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!_crouching)
			{
				Ray crouchRay = new Ray(_rigidbody.position + Vector3.up * _capsule.radius * HALF, Vector3.up);
				float crouchRayLength = _capsuleHeight - _capsule.radius * HALF;
				if (Physics.SphereCast(crouchRay, _capsule.radius * HALF, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					_crouching = true;
				}
			}
		}

		private void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			_animator.SetFloat("Forward", _forwardAmount, 0.1f, Time.deltaTime);
			_animator.SetFloat("Turn", _turnAmount, 0.1f, Time.deltaTime);
			_animator.SetBool("Crouch", _crouching);
			_animator.SetBool("OnGround", _isGrounded);

			if (!_isGrounded)
				_animator.SetFloat("Jump", _rigidbody.velocity.y);

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle = Mathf.Repeat(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime + _runCycleLegOffset, 1);
			float jumpLeg  = (runCycle < HALF ? 1 : -1) * _forwardAmount;

			if (_isGrounded)
				_animator.SetFloat("JumpLeg", jumpLeg);

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (_isGrounded && move.magnitude > 0)
				_animator.speed = _animSpeedMultiplier;
			else
				_animator.speed = 1;
		}

		private void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * _gravityMultiplier) - Physics.gravity;
			_rigidbody.AddForce(extraGravityForce);

			_groundCheckDistance = _rigidbody.velocity.y < 0 ? _origGroundCheckDistance : 0.01f;
		}

		private void HandleGroundedMovement(bool crouch, bool jump)
		{
			if (jump && !crouch && _animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				_rigidbody.velocity       = new Vector3(_rigidbody.velocity.x, _jumpPower, _rigidbody.velocity.z);
				_isGrounded               = false;
				_animator.applyRootMotion = false;
				_groundCheckDistance      = 0.1f;
			}
		}

		private void CheckGroundStatus()
		{
			RaycastHit hitInfo;

			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, _groundCheckDistance))
			{
				_groundNormal             = hitInfo.normal;
				_isGrounded               = true;
				_animator.applyRootMotion = true;
			}
			else
			{
				_isGrounded               = false;
				_groundNormal             = Vector3.up;
				_animator.applyRootMotion = false;
			}
		}
	}
}

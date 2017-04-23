using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoWtypeCamera
	: MonoBehaviour
{
	public float TargetHeight   = 1.7f;
	public float Distance       = 5.0f;
	public float OffsetFromWall = 0.1f;

	public float MaxDistance   = 20;
	public float MinDistance   = 0.6f;
	public float SpeedDistance = 5;

	public float xSpeed = 200.0f;
	public float ySpeed = 200.0f;

	public int yMinLimit = -40;
	public int yMaxLimit = 80;

	public int ZoomRate = 40;

	public float RotationDampening = 3.0f;
	public float ZoomDampening     = 5.0f;

	public LayerMask CollisionLayers = -1;

	private GameObject _player;
	private Transform _target;
	private float _xDeg = 0.0f;
	private float _yDeg = 0.0f;
	private float _currentDistance;
	private float _desiredDistance;
	private float _correctedDistance;

	private void Awake()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
		_target = _player.transform;

		// Set the initial camera angle so it's not facing straight down
		SetInitialCameraTransforms();
	}

	private void Start ()
	{
		Vector3 angles = transform.eulerAngles;
		_xDeg           = angles.x;
		_yDeg           = angles.y;

		_currentDistance   = Distance;
		_desiredDistance   = Distance;
		_correctedDistance = Distance;

		// Make the rigid body not change rotation
		if (gameObject.GetComponent<Rigidbody>())
			gameObject.GetComponent<Rigidbody>().freezeRotation = true;
	}

	/// <summary>
	/// Camera logic on LateUpdate to only update after all character movement logic has been handled.
	/// </summary>
	private void LateUpdate ()
	{
		Vector3 vTargetOffset;

		// Don't do anything if target is not defined
		if (!_target)
			return;

		if (GUIUtility.hotControl == 0)
		{
			if (Input.GetMouseButton(1))
			{
				// Let the mouse govern camera position
				_xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
				_yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			}
			else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
			{
				// Otherwise, ease behind the target if any of the directional keys are pressed
				float targetRotationAngle  = _target.eulerAngles.y;
				float currentRotationAngle = transform.eulerAngles.y;
				_xDeg                       = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, RotationDampening * Time.deltaTime);

			}
		}

		// calculate the desired distance
		_desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomRate * Mathf.Abs(_desiredDistance) * SpeedDistance;
		_desiredDistance  = Mathf.Clamp(_desiredDistance, MinDistance, MaxDistance);

		_yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);

		// Set camera rotation
		Quaternion rotation = Quaternion.Euler(_yDeg, _xDeg, 0);
		_correctedDistance   = _desiredDistance;

		// Calculate desired camera position
		vTargetOffset    = new Vector3 (0, -TargetHeight, 0);
		Vector3 position = _target.position - (rotation * Vector3.forward * _desiredDistance + vTargetOffset);

		// Check for collision using the true target's desired registration point as set by user using height
		RaycastHit collisionHit;
		Vector3 trueTargetPosition = new Vector3(_target.position.x, _target.position.y, _target.position.z) - vTargetOffset;

		// If there was a collision, correct the camera position and calculate the corrected distance
		bool isCorrected = false;
		if (Physics.Linecast(trueTargetPosition, position, out collisionHit, CollisionLayers.value))
		{
			// calculate the distance from the original estimated position to the collision location,
			// subtracting out a safety "offset" distance from the object we hit.  The offset will help
			// keep the camera from being right on top of the surface we hit, which usually shows up as
			// the surface geometry getting partially clipped by the camera's front clipping plane.
			_correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - OffsetFromWall;
			isCorrected = true;
		}

		// For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
		_currentDistance = !isCorrected || _correctedDistance > _currentDistance ? Mathf.Lerp (_currentDistance, _correctedDistance, Time.deltaTime * ZoomDampening) : _correctedDistance;

		// Keep within legal limits
		_currentDistance = Mathf.Clamp (_currentDistance, MinDistance, MaxDistance);

		// Recalculate position based on the new currentDistance
		position = _target.position - (rotation * Vector3.forward * _currentDistance + vTargetOffset);

		transform.rotation = rotation;
		transform.position = position;
	}

	/// <summary>
	/// Sets the initial camera transforms. TODO need to fix this for different scenes - might not be required if we keep the player gameobject/cam and pass it across scenes
	/// </summary>
	private void SetInitialCameraTransforms()
	{
		transform.position = new Vector3(-0.3f, 3.0f, -5f);
		transform.rotation = Quaternion.Euler(18.0f, 0.0f, 0.0f);
	}

	/// <summary>
	/// Clamps the angle.
	/// </summary>
	/// <param name="angle">The angle.</param>
	/// <param name="min">The minimum.</param>
	/// <param name="max">The maximum.</param>
	/// <returns>An angle clamped between 0-360</returns>
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}

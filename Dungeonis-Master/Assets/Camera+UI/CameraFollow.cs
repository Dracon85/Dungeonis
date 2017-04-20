using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow
	: MonoBehaviour
{
	private GameObject _player;
	public bool ReverseMode = false;
	public float PanSpeed   = 10.0f;

	void Start ()
	{
		_player = GameObject.FindGameObjectWithTag ("Player");
	}

	private void Update()
	{
		if (ReverseMode)
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * PanSpeed);
		else
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 360, 0), Time.deltaTime * PanSpeed);
	}

	void LateUpdate()
	{
		if (!ReverseMode)
		{
			transform.position = _player.transform.position;
			//transform.rotation = Player.transform.rotation; // This is causing the veering to the left. Let me know how you want the camera to act.
		}
		else
			transform.position = _player.transform.position;

		if (Input.GetKeyDown(KeyCode.Q))
			ReverseMode = !ReverseMode;
	}
}

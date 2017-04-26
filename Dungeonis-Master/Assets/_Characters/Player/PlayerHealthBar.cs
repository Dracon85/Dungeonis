﻿namespace RPG.Characters
{
	using UnityEngine;
	using UnityEngine.UI;

	[RequireComponent(typeof(RawImage))]
	public class PlayerHealthBar
		: MonoBehaviour
	{
		RawImage healthBarRawImage;
		Player player;

		// Use this for initialization
		void Start()
		{
			player            = FindObjectOfType<Player>();
			healthBarRawImage = GetComponent<RawImage>();
		}

		// Update is called once per frame
		void Update()
		{
			float xValue             = -(player.HealthAsPercentage / 2f) - 0.5f;
			healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
		}
	}
}

﻿namespace RPG.Characters
{
	using UnityEngine;
	using RPG.UtilScripts;

	public class DestroyHPPowerup
		: MonoBehaviour
	{
		private Player player;

		void Start()
		{
			player = FindObjectOfType<Player>();
		}

		void OnCollisionEnter(Collision collision)
		{
			//layer number of enemy in project settings
			if (collision.gameObject.layer == (int)Layer.Player)
			{
				player.MaxHealthPoints += Random.Range(15, 25);
				player.SavePlayer();
				Destroy(gameObject);
			}
		}
	}
}

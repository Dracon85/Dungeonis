﻿namespace RPG.Characters
{
	using UnityEngine;
	using RPG.UtilScripts;

	public class DestroyMagAtk
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
				player.MagicAttackPower += Random.Range(2, 8);
				player.SavePlayer();
				Destroy(gameObject);
			}
		}
	}
}

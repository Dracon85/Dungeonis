using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO consider rewiring...
using RPG.UtilScripts;
using RPG.Characters;

namespace RPG.Characters{
public class DestroyHPPowerup
		: MonoBehaviour{
		private Player player;

		void Start(){
			player              = GameObject.FindObjectOfType<Player>();
		}

	void OnCollisionEnter(Collision collision)
	{
			//layer number of enemy in project settings
			if (collision.gameObject.layer == 11){
				player.localPlayerData._maxHealthPoints += Random.Range(10,25);
				player.SavePlayer ();
				Object.Destroy (gameObject);
				}
			}
	}
}

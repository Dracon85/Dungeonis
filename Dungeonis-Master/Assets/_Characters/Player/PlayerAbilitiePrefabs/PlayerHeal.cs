using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UtilScripts;
using RPG.Weapons;
using RPG.Characters;

public class PlayerHeal : MonoBehaviour {

	private Player player;
	private Projectile script;
	private Transform target;
	public float speed=10f;

	void Start()
	{
		script = GetComponent<Projectile>(); 
		script.enabled = !script.enabled;
		player = FindObjectOfType<Player>();
		target = player.transform;
	}
	void Update(){
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		if (player.CurrentHealthPoints==player.MaxHealthPoints){
			Object.Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		//layer number of enemy in project settings
			if (collision.gameObject.layer == 11&&player.CurrentHealthPoints!=player.MaxHealthPoints)
		{
				if (player.CurrentHealthPoints<player.MaxHealthPoints){
			player.CurrentHealthPoints += player.MagicAttackPower;
			Object.Destroy (gameObject);
				} 	
	}
}
}
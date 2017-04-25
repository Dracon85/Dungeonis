using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO consider rewiring...
using RPG.UtilScripts;

namespace RPG.Weapons{
public class Projectile
	: MonoBehaviour
{

	[SerializeField] float projectileSpeed;
	//inspectable to see who shot the projectile when paused
	[SerializeField] GameObject shooter;

	public float projectileDuration=5f;
	float damageCaused;

	public void SetShooter(GameObject shooter){
		this.shooter = shooter;
	}

	void Update()
	{
		Object.Destroy(gameObject, projectileDuration);
	}

	public void SetDamage(float damage)
	{
		damageCaused = damage;
	}

	public float GetDefaultLaunchSpeed(){
		return projectileSpeed;
	}

	void OnCollisionEnter(Collision collision)
	{
		Component damageableComponent = collision.gameObject.GetComponent (typeof(IDamageable));
		if (damageableComponent)
			(damageableComponent as IDamageable).TakeDamage (damageCaused);
		}
	}
}

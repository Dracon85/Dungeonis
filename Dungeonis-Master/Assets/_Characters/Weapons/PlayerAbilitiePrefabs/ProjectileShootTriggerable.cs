using UnityEngine;
using System.Collections;
using RPG.Weapons;

namespace RPG.Characters{
public class ProjectileShootTriggerable : MonoBehaviour {

	[HideInInspector]public Rigidbody projectile;                          // Rigidbody variable to hold a reference to our projectile prefab
	public Transform bulletSpawn;                           // Transform variable to hold the location where we will spawn our projectile
	[HideInInspector] public float projectileForce = 250f;                  // Float variable to hold the amount of force which we will apply to launch our projectiles
	private Transform _cam;
	private Player player;

	void Start () {
			player              = GameObject.FindObjectOfType<Player>();
		if (Camera.main != null)
			_cam = Camera.main.transform;
		else
		{
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}		
	}

	public void Launch()
	{
		//Instantiate a copy of our projectile and store it in a new rigidbody variable called clonedBullet
		Rigidbody clonedBullet = Instantiate(projectile, bulletSpawn.position, Quaternion.identity) as Rigidbody;
		Projectile projectileComponent = clonedBullet.GetComponent<Projectile>();
		
		//use damage set on player in inspector and shoot at mouse position
		var damagePerShot=player.localPlayerData._playerMagicAtkPower*projectileComponent.SpellDamageModifier;
		projectileComponent.SetDamage(damagePerShot);

		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100f)) {
				Debug.DrawLine (_cam.position, hit.point);
			}
		clonedBullet.transform.LookAt(hit.point); 

		//Add force to the instantiated bullet, pushing it forward away from the bulletSpawn location, using projectile force for how hard to push it away
		clonedBullet.GetComponent<Rigidbody>().velocity=clonedBullet.transform.forward*projectileForce;
//		clonedBullet.AddForce(bulletSpawn.transform.forward * projectileForce);
	}
}
}
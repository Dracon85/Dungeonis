namespace RPG.Characters
{
	using UnityEngine;
	using RPG.Weapons;

	public class ProjectileShootTriggerable
		: MonoBehaviour
	{
		[HideInInspector] public float projectileForce = 250f;	// Float variable to hold the amount of force which we will apply to launch our projectiles
		[HideInInspector] public Rigidbody projectile;          // Rigidbody variable to hold a reference to our projectile prefab
		public Transform bulletSpawn;							// Transform variable to hold the location where we will spawn our projectile
		private Transform _cam;
		public Player player;

		private void Start()
		{

			if (Camera.main != null)
				_cam = Camera.main.transform;
			else
				Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
		}

		public void Launch()
		{
			//Instantiate a copy of our projectile and store it in a new rigidbody variable called clonedBullet
			Rigidbody clonedBullet         = Instantiate(projectile, bulletSpawn.position, Quaternion.identity) as Rigidbody;
			Projectile projectileComponent = clonedBullet.GetComponent<Projectile>();

			//use damage set on player in inspector and shoot at mouse position
			float damagePerShot = player.MagicAttackPower * projectileComponent.SpellDamageModifier;
			projectileComponent.SetDamage(damagePerShot);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100f))
				Debug.DrawLine(_cam.position, hit.point);

			clonedBullet.transform.LookAt(hit.point);

//			Add force to the instantiated bullet, pushing it forward away from the bulletSpawn location, using projectile force for how hard to push it away
			clonedBullet.GetComponent<Rigidbody>().velocity = _cam.forward * projectileForce;
//			clonedBullet.AddForce(bulletSpawn.transform.forward * projectileForce);
		}
	}
}
namespace RPG.Weapons
{
	using UnityEngine;
	//TODO consider rewiring...
	using RPG.UtilScripts;

	public class Projectile
		: MonoBehaviour
	{
		[SerializeField] private float _projectileSpeed;
		[SerializeField] private float _projectileDuration = 5f;
		[SerializeField] private GameObject _shooter;
		private float _damageCaused;

		public void SetShooter(GameObject shooter)
		{
			_shooter = shooter;
		}

		public void SetDamage(float damage)
		{
			_damageCaused = damage;
		}

		public float GetDefaultLaunchSpeed()
		{
			return _projectileSpeed;
		}

		private void Update()
		{
			Destroy(gameObject, _projectileDuration);
		}

		private void OnCollisionEnter(Collision collision)
		{
			Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));

			if (damageableComponent)
				(damageableComponent as IDamageable).TakeDamage(_damageCaused);
		}
	}
}

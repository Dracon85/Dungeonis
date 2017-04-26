namespace RPG.Characters
{
	using UnityEngine;
	using UnityStandardAssets.Characters.ThirdPerson;
	//TODO consider rewiring...
	using RPG.UtilScripts;
	using RPG.Weapons;

	public class Enemy
		: CharacterBase
	{
		[SerializeField] private float _noticeRange = 5f;
		[SerializeField] private float damagePerShot = 5f;
		[SerializeField] private Vector3 aimOffset = new Vector3(0, 1f, 0); //target adjustment for projectile sort of a hack fix
		[SerializeField] private GameObject projectileToUse;
		[SerializeField] private GameObject projectileSocket;

		private AICharacterControl aiCharControl = null;
		private GameObject player = null;
		private bool isAttacking = false;

		public override void TakeDamage(float damage)
		{
			CurrentHealthPoints = Mathf.Clamp(CurrentHealthPoints - damage, 0f, MaxHealthPoints);
			//enemies now chase when damaged. more hits =farther chase
			_noticeRange += 20;

			if (CurrentHealthPoints <= 0)
				Destroy(gameObject);
		}

		private void Awake()
		{
			MaxHealthPoints = 100f;
			AttackRange = 2f;
			ShotDelay = 1f;
		}

		private void Start()
		{
			player              = GameObject.FindGameObjectWithTag("Player");
			aiCharControl       = GetComponent<AICharacterControl>();
			CurrentHealthPoints = MaxHealthPoints;
		}

		private void LateUpdate()
		{
			if (IsTargetInRange(player, _noticeRange))
				aiCharControl.SetTarget(player.transform);
			else
				aiCharControl.SetTarget(transform);


			if (IsTargetInRange(player, AttackRange) && !isAttacking)
			{
				isAttacking = true;
				InvokeRepeating("FireProjectile", 0f, ShotDelay); //TODO switch to coroutines
			}

			if (!IsTargetInRange(player, AttackRange))
			{
				isAttacking = false;
				CancelInvoke();
			}
		}
		//separate out character firing logic into a new class
		void FireProjectile()
		{
			GameObject newProjectile       = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
			Projectile projectileComponent = newProjectile.GetComponent<Projectile>();

			projectileComponent.SetDamage(damagePerShot);
			projectileComponent.SetShooter(gameObject);

			Vector3 unitVectorToPlayer                       = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
			float projectileSpeed                            = projectileComponent.GetDefaultLaunchSpeed();
			newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
		}

		private void OnDrawGizmos()
		{
			//draw aggroRadius
			Gizmos.color = new Color(0, 0, 255, .5f);
			Gizmos.DrawWireSphere(transform.position, _noticeRange);
			Gizmos.color = new Color(255, 0, 0, .5f);
			Gizmos.DrawWireSphere(transform.position, AttackRange);
		}
	}
}

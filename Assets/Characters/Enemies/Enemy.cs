using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy
	: MonoBehaviour, IDamageable
{
	[SerializeField]float maxHealthPoints=100f;
	[SerializeField]float blueAggroRadius=5f;
	[SerializeField]float redAttackRange=2f;
	[SerializeField]float damagePerShot=5f;
	[SerializeField]float shotDelay=1f;
	[SerializeField]Vector3 aimOffset = new Vector3(0,1f,0); //target adjustment for projectile sort of a hack fix
	[SerializeField]GameObject projectileToUse;
	[SerializeField]GameObject projectileSocket;

	float currentHealthPoints;

	AICharacterControl aiCharControl = null;
	GameObject player                = null;
	bool isAttacking                 = false;

	public void TakeDamage(float damage)
	{
		currentHealthPoints = Mathf.Clamp (currentHealthPoints-damage, 0f, maxHealthPoints);

		if (currentHealthPoints <= 0)
			Destroy (gameObject);
	}

	public float healthAsPercentage
	{
		get
		{
			return currentHealthPoints/maxHealthPoints;
		}
	}

	void Start ()
	{
		player              = GameObject.FindGameObjectWithTag ("Player");
		aiCharControl       = GetComponent<AICharacterControl>();
		currentHealthPoints = maxHealthPoints;
	}

	void LateUpdate ()
	{
		float distanceToPlayer = Vector3.Distance (player.transform.position, transform.position);

		if (distanceToPlayer <= blueAggroRadius)
			aiCharControl.SetTarget(player.transform);
		else
			aiCharControl.SetTarget(transform);


		if (distanceToPlayer <= redAttackRange&&!isAttacking)
		{
			isAttacking = true;
			InvokeRepeating ("FireProjectile", 0f, shotDelay); //TODO switch to coroutines
		}

		if (distanceToPlayer >= redAttackRange)
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
		projectileComponent.SetShooter (gameObject);

		Vector3 unitVectorToPlayer                       = (player.transform.position+aimOffset - projectileSocket.transform.position).normalized;
		float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
		newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer*projectileSpeed;
	}

	void OnDrawGizmos()
	{
		//draw aggroRadius
		Gizmos.color= new Color(0,0,255,.5f);
		Gizmos.DrawWireSphere (transform.position,blueAggroRadius);
		Gizmos.color= new Color(255,0,0,.5f);
		Gizmos.DrawWireSphere (transform.position,redAttackRange);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player
	: MonoBehaviour,IDamageable
{

	[SerializeField]float maxHealthPoints    = 100f;
	[SerializeField]float playerAtkPower     = 10f;
	[SerializeField]float minTimeBetweenHits = 0.5f;
	[SerializeField]float maxAttackRange     = 2f;
	[SerializeField] int enemyLayer          = 9;
	//set weapon in use and location for it to spawn
	[SerializeField] Weapon weaponInUse;

	GameObject respawnPoint;
	float currentHealthPoints;
	float lastHitTime = 0f;
	CameraRaycaster cameraRaycaster;

	void Start()
	{
		cameraRaycaster                            = FindObjectOfType<CameraRaycaster>();
		cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
		currentHealthPoints                        = maxHealthPoints;
		PutWeaponInHand ();
	}

	private void PutWeaponInHand ()
	{
		var weaponPrefab = weaponInUse.GetWeaponPrefab();
		GameObject dominantHand = RequestDominantHand ();
		var weapon = Instantiate (weaponPrefab, dominantHand.transform);
		weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
		weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
		//TODO move weapon to correct place and child to hand
	}
	//handle DominantHand attachment script
	private GameObject RequestDominantHand (){
		var dominantHands = GetComponentsInChildren<DominantHand>();
		int numberOfDominantHands = dominantHands.Length;
		Assert.IsFalse(numberOfDominantHands<=0,"No Dominant Hand Found, on Player Please Add one to Bone"); 
		Assert.IsFalse(numberOfDominantHands>1,"Multiple Dominant Hand scripts on Player Please use only one");
		return dominantHands[0].gameObject; 
	}

	//mouse click attack if within range...TODO consider using weapon collider to do damage to enemies
	void OnMouseClick (RaycastHit raycastHit, int layerHit)
	{
		if (layerHit == enemyLayer)
		{
			GameObject enemy = raycastHit.collider.gameObject;

			if((enemy.transform.position-transform.position).magnitude > maxAttackRange)
				return;

			var enemyComponent = enemy.GetComponent<Enemy> ();

			if (Time.time-lastHitTime>minTimeBetweenHits)
			{
				enemyComponent.TakeDamage(playerAtkPower);
				lastHitTime = Time.time;
			}
		}
	}

	public float healthAsPercentage
	{
		get
		{
			return currentHealthPoints/maxHealthPoints;
		}
	}

	public void TakeDamage(float damage)
	{
		currentHealthPoints     = Mathf.Clamp(currentHealthPoints-damage, 0f, maxHealthPoints);
		respawnPoint            = GameObject.FindGameObjectWithTag("PlayerRespawn");

		if (currentHealthPoints <= 0)
		{
			transform.position  = respawnPoint.transform.position;
			currentHealthPoints = maxHealthPoints;
		}
	}
}

﻿namespace RPG.Characters
{
	using UnityEngine;
	using UnityEngine.Assertions;
	//TODO consider rewiring...
	using RPG.CameraUI;
	using RPG.Weapons;

	public class Player
		: CharacterBase
	{
		[SerializeField] private int _enemyLayer = 9;
		[SerializeField] private GameObject _respawnPoint;
		[SerializeField] private Weapon _weaponInUse;
		[SerializeField] AnimatorOverrideController animatorOverrideController;

		private Animator animator;
		private float _lastHitTime = 0f;
		private CameraRaycaster _cameraRaycaster;

		private void Awake()
		{
			_respawnPoint = GameObject.FindGameObjectWithTag("PlayerRespawn");
			transform.position = _respawnPoint.transform.position;
}

		private void Start()
		{
			_cameraRaycaster = FindObjectOfType<CameraRaycaster>();
			RegisterForMouseClick();
			LoadPlayer();
			PutWeaponInHand();
			OverrideAnimatorController();
		}

		/// <summary>
		/// Applies damage taken to player.
		/// </summary>
		/// <param name="damage">The damage.</param>
		public override void TakeDamage(float damage)
		{
			CurrentHealthPoints = Mathf.Clamp(CurrentHealthPoints - damage, 0f, MaxHealthPoints);
			SavePlayer ();
			if (IsCharacterDead())
			{
				print ("player died");
				decreaseStats ();
				transform.position = _respawnPoint.transform.position;
				CurrentHealthPoints = MaxHealthPoints;
			}
		}
		//decrease stats when dead up to a certain point
		private void decreaseStats(){
			if (MaxHealthPoints>150){
				MaxHealthPoints -= Random.Range(10, 15);
			}
			if(PhysicalAttackPower>25){
				PhysicalAttackPower -= Random.Range(2, 10);
			}
			if (MagicAttackPower>25){
				MagicAttackPower -= Random.Range(2, 8);
			}
		}

		/// <summary>
		/// Registers for mouse click.
		/// </summary>
		private void RegisterForMouseClick()
		{
			_cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
		}

		/// <summary>
		/// Saves the player.
		/// </summary>
		public void SavePlayer()
		{
			GlobalControl.Instance.SavePlayer(this);
		}

		/// <summary>
		/// Loads the player.
		/// </summary>
		public void LoadPlayer()
		{
			MaxHealthPoints     = GlobalControl.Instance.MaxHealthPoints;
			ShotDelay           = GlobalControl.Instance.ShotDelay;
			CurrentHealthPoints = GlobalControl.Instance.CurrentHealthPoints;
			AttackRange         = GlobalControl.Instance.AttackRange;
			PhysicalAttackPower = GlobalControl.Instance.PhysicalAttackPower;
			MagicAttackPower 	= GlobalControl.Instance.MagicAttackPower;
			ExperiencePoints    = GlobalControl.Instance.ExperiencePoints;
		}

		/// <summary>
		/// Overrides the animator controller.
		/// </summary>
		private void OverrideAnimatorController()
		{
			animator                                     = GetComponent<Animator>();
			animator.runtimeAnimatorController           = animatorOverrideController;
			animatorOverrideController["DEFAULT ATTACK"] = _weaponInUse.GetAttackAnimClip(); //TODO remove const/string ref
		}

		/// <summary>
		/// Puts the weapon in hand.
		/// </summary>
		private void PutWeaponInHand()
		{
			GameObject weaponPrefab        = _weaponInUse.GetWeaponPrefab();
			GameObject dominantHand        = RequestDominantHand();
			GameObject weapon              = Instantiate(weaponPrefab, dominantHand.transform);
			weapon.transform.localPosition = _weaponInUse.gripTransform.localPosition;
			weapon.transform.localRotation = _weaponInUse.gripTransform.localRotation;
			//TODO move weapon to correct place and child to hand
		}

		/// <summary>
		/// Requests the dominant hand. Handle DominantHand attachment script
		/// </summary>
		/// <returns>The Dominant Hand gameObject</returns>
		private GameObject RequestDominantHand()
		{
			DominantHand[] dominantHands          = GetComponentsInChildren<DominantHand>();
			int numberOfDominantHands             = dominantHands.Length;
			Assert.IsFalse(numberOfDominantHands <= 0, "No Dominant Hand Found, on Player Please Add one to Bone");
			Assert.IsFalse(numberOfDominantHands > 1, "Multiple Dominant Hand scripts on Player Please use only one");
			return dominantHands[0].gameObject;
		}

		/// <summary>
		/// Called when [mouse click]. Attacks enemies if within range. TODO consider using weapon collider to do damage to enemies
		/// </summary>
		/// <param name="raycastHit">The raycast hit.</param>
		/// <param name="layerHit">The layer hit.</param>
		private void OnMouseClick(RaycastHit raycastHit, int layerHit)
		{
			if (layerHit.Equals(_enemyLayer))
			{
				GameObject enemy = raycastHit.collider.gameObject;

				if (IsTargetInRange(enemy, _weaponInUse.GetMaxAttackRange()))
					AttackTarget(enemy);
			}
		}

		/// <summary>
		/// Attacks the target.
		/// </summary>
		/// <param name="target">The target.</param>
		private void AttackTarget(GameObject target)
		{
			Enemy enemyComponent = target.GetComponent<Enemy>();

			if ((Time.time - _lastHitTime) > _weaponInUse.GetMinTimeBetweenHits())
			{
				animator.SetTrigger("Attack"); //TODO make const
				enemyComponent.TakeDamage(PhysicalAttackPower);
				_lastHitTime = Time.time;
			}
		}
	}
}


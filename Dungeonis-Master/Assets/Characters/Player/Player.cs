using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player
	: MonoBehaviour, IDamageable
{
	[SerializeField] private float _maxHealthPoints    = 100f;
	[SerializeField] private float _playerAtkPower     = 10f;
	[SerializeField] private float _minTimeBetweenHits = 0.5f;
	[SerializeField] private float _maxAttackRange     = 2f;
	[SerializeField] private int _enemyLayer           = 9;
	[SerializeField] private GameObject _respawnPoint;
	[SerializeField] private Weapon _weaponInUse;

	private float _currentHealthPoints;
	private float _lastHitTime = 0f;
	private CameraRaycaster _cameraRaycaster;

	public float healthAsPercentage
	{
		get
		{
			return _currentHealthPoints / _maxHealthPoints;
		}
	}

	/// <summary>
	/// Applies damage taken to player.
	/// </summary>
	/// <param name="damage">The damage.</param>
	public void TakeDamage(float damage)
	{
		_currentHealthPoints = Mathf.Clamp(_currentHealthPoints - damage, 0f, _maxHealthPoints);

		if (IsPlayerDead())
		{
			transform.position   = _respawnPoint.transform.position;
			_currentHealthPoints = _maxHealthPoints;
		}
	}

	private void Awake()
	{
		_respawnPoint      = GameObject.FindGameObjectWithTag("PlayerRespawn");
		transform.position = _respawnPoint.transform.position;
	}

	private void Start()
	{
		_cameraRaycaster                            = FindObjectOfType<CameraRaycaster>();
		_cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
		_currentHealthPoints                        = _maxHealthPoints;
		PutWeaponInHand();
	}

	/// <summary>
	/// Determines whether the player is dead.
	/// </summary>
	/// <returns>
	/// <c>true</c> if player is dead; otherwise, <c>false</c>.
	/// </returns>
	private bool IsPlayerDead()
	{
		return _currentHealthPoints <= 0;
	}

	/// <summary>
	/// Puts the weapon in hand.
	/// </summary>
	private void PutWeaponInHand()
	{
		var weaponPrefab               = _weaponInUse.GetWeaponPrefab();
		GameObject dominantHand        = RequestDominantHand();
		var weapon                     = Instantiate(weaponPrefab, dominantHand.transform);
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
		var dominantHands                     = GetComponentsInChildren<DominantHand>();
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
	private void OnMouseClick (RaycastHit raycastHit, int layerHit)
	{
		if (layerHit.Equals(_enemyLayer))
		{
			GameObject enemy = raycastHit.collider.gameObject;

			if((enemy.transform.position-transform.position).magnitude > _maxAttackRange)
				return;

			var enemyComponent = enemy.GetComponent<Enemy>();

			if ((Time.time - _lastHitTime) > _minTimeBetweenHits)
			{
				enemyComponent.TakeDamage(_playerAtkPower);
				_lastHitTime = Time.time;
			}
		}
	}
}

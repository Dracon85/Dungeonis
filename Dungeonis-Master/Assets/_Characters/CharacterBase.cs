namespace RPG.Characters
{
	using UnityEngine;
	using RPG.UtilScripts;

	public class CharacterBase
		: MonoBehaviour, IDamageable
	{
		public float MaxHealthPoints;
		public float AttackPower;
		public float ShotDelay;

		[SerializeField] protected float AttackRange;
		[SerializeField] protected float CurrentHealthPoints;

		/// <summary>
		/// Gets the health as percentage.
		/// </summary>
		/// <value>
		/// The health as percentage.
		/// </value>
		public float HealthAsPercentage
		{
			get
			{
				return CurrentHealthPoints / MaxHealthPoints;
			}
		}

		/// <summary>
		/// Gets the attack range.
		/// </summary>
		/// <returns></returns>
		public float GetAttackRange()
		{
			return AttackRange;
		}

		/// <summary>
		/// Applies damage to character.
		/// </summary>
		/// <param name="damage">The damage.</param>
		public virtual void TakeDamage(float damage)
		{
			CurrentHealthPoints = Mathf.Clamp(CurrentHealthPoints - damage, 0f, MaxHealthPoints);
		}

		/// <summary>
		/// Determines whether the character is dead.
		/// </summary>
		/// <returns>
		/// <c>true</c> if character is dead; otherwise, <c>false</c>.
		/// </returns>
		public bool IsCharacterDead()
		{
			return CurrentHealthPoints <= 0;
		}

		/// <summary>
		/// Determines whether the target is in range.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>
		///   <c>true</c> if target is in range; otherwise, <c>false</c>.
		/// </returns>
		public bool IsTargetInRange(GameObject target, float maxRange)
		{
			return (target.transform.position - transform.position).magnitude <= maxRange;
		}
	}
}
namespace RPG.Weapons
{
	using UnityEngine;

	[CreateAssetMenu(menuName = ("RPG/Weapon"))]
	public class Weapon
		: ScriptableObject
	{
		public Transform gripTransform;

		[SerializeField] private GameObject weaponPrefab;
		[SerializeField] private AnimationClip attackAnimation;
		[SerializeField] private float _minTimeBetweenHits = 0.5f;
		[SerializeField] private float _maxAttackRange = 2f;

		public float GetMinTimeBetweenHits()
		{
			return _minTimeBetweenHits;
		}

		public float GetMaxAttackRange()
		{
			return _maxAttackRange;
		}

		public GameObject GetWeaponPrefab()
		{
			return weaponPrefab;
		}

		public AnimationClip GetAttackAnimClip()
		{
			RemoveAnimationEvents();
			return attackAnimation;
		}

		//so that asset packs with animation events no longer cause crashes
		void RemoveAnimationEvents()
		{
			attackAnimation.events = new AnimationEvent[0];
		}
	}
}
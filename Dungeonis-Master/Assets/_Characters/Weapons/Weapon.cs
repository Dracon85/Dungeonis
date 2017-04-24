using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons{
[CreateAssetMenu(menuName=("RPG/Weapon"))]
public class Weapon : ScriptableObject {

	public Transform gripTransform;
	[SerializeField] GameObject weaponPrefab;
	[SerializeField] AnimationClip attackAnimation;
	[SerializeField] float _minTimeBetweenHits = 0.5f;
	[SerializeField] float _maxAttackRange     = 2f;

		public float GetMinTimeBetweenHits(){
			return _minTimeBetweenHits;
		}
		public float GetMaxAttackRange(){
			return _maxAttackRange;
		}

	public GameObject GetWeaponPrefab(){
		return weaponPrefab;
	}
		public AnimationClip GetAttackAnimClip(){
			RemoveAnimationEvents();
			return attackAnimation;
		}
		//so that asset packs with animation events no longer cause crashes
		void RemoveAnimationEvents ()
		{
			attackAnimation.events = new AnimationEvent[0];
		}
}
}

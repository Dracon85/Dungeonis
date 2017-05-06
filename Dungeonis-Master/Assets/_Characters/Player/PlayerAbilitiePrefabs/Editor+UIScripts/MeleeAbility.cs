using UnityEngine;
using System.Collections;
using RPG.Weapons;
using RPG.Characters;

[CreateAssetMenu (menuName = "Abilities/MeleeAbility")]
public class MeleeAbility : Ability {

	public float projectileForce = 10f;
	public Rigidbody projectile;
	public float MartialDmgModifier=1;

	private MeleeProjectileTriggerable launcher;

	public override void Initialize(GameObject obj)
	{
		launcher = obj.GetComponent<MeleeProjectileTriggerable> ();
		launcher.projectileForce = projectileForce;
		launcher.projectile = projectile;
		launcher.MartialDmgModifier = MartialDmgModifier;
	}

	public override void TriggerAbility()
	{
		launcher.Launch ();
	}

}
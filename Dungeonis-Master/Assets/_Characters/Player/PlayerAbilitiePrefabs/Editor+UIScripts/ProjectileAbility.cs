using UnityEngine;
using System.Collections;
using RPG.Weapons;
using RPG.Characters;

[CreateAssetMenu (menuName = "Abilities/ProjectileAbility")]
public class ProjectileAbility : Ability {

	public float projectileForce = 500f;
	public Rigidbody projectile;
	public float SpellDmgModifier=1;

	private ProjectileShootTriggerable launcher;

	public override void Initialize(GameObject obj)
	{
		launcher = obj.GetComponent<ProjectileShootTriggerable> ();
		launcher.projectileForce = projectileForce;
		launcher.projectile = projectile;
		launcher.SpellDmgModifier = SpellDmgModifier;
	}

	public override void TriggerAbility()
	{
		launcher.Launch ();
	}

}
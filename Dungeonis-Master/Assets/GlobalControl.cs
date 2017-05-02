namespace RPG.Characters
{
	using UnityEngine;
	using RPG.UtilScripts;

	public class GlobalControl
		: CharacterStatistics
	{
		public static GlobalControl Instance;

		private void Awake()
		{
			if (Instance == null)
			{
				DontDestroyOnLoad(transform.root.gameObject);
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		/// <summary>
		/// Saves the player.
		/// </summary>
		/// <param name="player">The player.</param>
		public void SavePlayer(Player player)
		{
			MaxHealthPoints     = player.MaxHealthPoints;
			ShotDelay           = player.ShotDelay;
			CurrentHealthPoints = player.CurrentHealthPoints;
			AttackRange         = player.AttackRange;
			PhysicalAttackPower = player.PhysicalAttackPower;
			MagicAttackPower 	= player.MagicAttackPower;
			ExperiencePoints    = player.ExperiencePoints;
		}
}
}

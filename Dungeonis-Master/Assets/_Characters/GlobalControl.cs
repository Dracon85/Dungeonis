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
				SavePlayer();
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
			AttackPower         = player.AttackPower;
			ShotDelay           = player.ShotDelay;
			CurrentHealthPoints = player.CurrentHealthPoints;
			AttackRange         = player.AttackRange;
			ExperiencePoints    = player.ExperiencePoints;
		}

		private void SavePlayer()
		{
			MaxHealthPoints     = 100f;
			AttackPower         = 10f;
			AttackRange         = 5f;
			ShotDelay           = 0.5f;
			CurrentHealthPoints = MaxHealthPoints;
			ExperiencePoints    = 0;
			MagicAttackPower    = 5f;
			PhysicalAttackPower = 5f;
		}
	}
}

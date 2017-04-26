namespace LevelManager
{
	using UnityEngine;
	using RPG.UtilScripts;

	public class GlobalControl
		: MonoBehaviour
	{
		public static GlobalControl Instance;
		public float HealthPoints;
		public float AttackPower;
		public float ExperiencePoints;

		private void Awake()
		{
			if (Instance.IsNull())
			{
				DontDestroyOnLoad(gameObject);
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}

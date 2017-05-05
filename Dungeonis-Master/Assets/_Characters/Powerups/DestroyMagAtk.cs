namespace RPG.Characters
{
	using UnityEngine;
	using RPG.UtilScripts;

	public class DestroyMagAtk
		: MonoBehaviour
	{
		private Player player;

		void Start()
		{
			player = FindObjectOfType<Player>();
		}

		void OnCollisionEnter(Collision collision)
		{
			//layer number of enemy in project settings
			if (collision.gameObject.layer == (int)Layer.Player)
			{
				player.MagicAttackPower += Random.Range(6, 15);
				player.SavePlayer();
				Destroy(gameObject);
			}
		}
	}
}

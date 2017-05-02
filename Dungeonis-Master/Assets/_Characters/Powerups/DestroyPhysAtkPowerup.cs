namespace RPG.Characters
{
	using UnityEngine;
	using RPG.UtilScripts;

	public class DestroyPhysAtkPowerup
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
				player.PhysicalAttackPower += Random.Range(5, 25);
				player.SavePlayer();
				Destroy(gameObject);
			}
		}
	}
}

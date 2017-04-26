namespace RPG.Characters
{
	using UnityEngine;
	using UnityEngine.UI;

	[RequireComponent(typeof(RawImage))]
	public class PlayerHealthBar
		: MonoBehaviour
	{
		RawImage healthBarRawImage;
		Player player;

		// Use this for initialization
		void Start()
		{
			player            = FindObjectOfType<Player>();
			healthBarRawImage = GetComponent<RawImage>();
		}

	// Update is called once per frame
	void Update()
	{
		float xValue             = -(player.healthAsPercentage / 2f) - 0.5f;
		var currentHealth = player.localPlayerData._currentHealthPoints;
		var maxHealth = player.localPlayerData._maxHealthPoints;
		var physAtk = player.localPlayerData._playerAtkPower;
		var magAtk = player.localPlayerData._playerMagicAtkPower;
			string max = (maxHealth.ToString ());
			string current=(currentHealth.ToString());
			healthDisplay.text =string.Format(@"{0}/{1}",max,current);
			string physATK=(physAtk.ToString ());
			string magATK=(magAtk.ToString ());
			attackDisplay.text = ("Mag ATK= " + magATK);
			MagicDisplay.text = ("Phys ATK= " + physATK);
		healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
	}
}
}

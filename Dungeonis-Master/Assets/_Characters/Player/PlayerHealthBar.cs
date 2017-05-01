namespace RPG.Characters
{
	using UnityEngine;
	using UnityEngine.UI;

	[RequireComponent(typeof(RawImage))]
	public class PlayerHealthBar
		: MonoBehaviour
	{
		public Text healthDisplay;
		public Text attackDisplay;
		public Text magicDisplay;

		private RawImage healthBarRawImage;
		private Player player;

		private void Start()
		{
			player = FindObjectOfType<Player>();
			healthBarRawImage = GetComponent<RawImage>();
		}

		private void Update()
		{
			float xValue             = -(player.HealthAsPercentage / 2f) - 0.5f;
			string currentHealth     = player.CurrentHealthPoints.ToString();
			string maxHealth         = player.MaxHealthPoints.ToString();
			string magicAttack       = player.MagicAttackPower.ToString();
			string physicalAttack    = player.PhysicalAttackPower.ToString();

			healthDisplay.text       = string.Format("{0}/{1}", currentHealth, maxHealth);
			attackDisplay.text       = string.Format("Mag ATK= {0}", magicAttack);
			magicDisplay.text        = string.Format("Phys ATK= {0}", physicalAttack);
			healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
		}
	}
}

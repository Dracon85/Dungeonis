namespace RPG.LevelManager
{
	using UnityEngine;
	using RPG.Characters;

	public class LevelTrigger
		: MonoBehaviour
	{
		public SceneIndexes LoadSceneName;
		private GameObject _player;
		private Player _playerComponent;


		void Start()
		{
			_player          = GameObject.FindGameObjectWithTag("Player");
			_playerComponent = _player.GetComponent<Player>();
		}

		private void OnTriggerEnter(Collider collider)
		{
			_playerComponent.SavePlayer();
			LoadingScreenManager.LoadScene((int)LoadSceneName);
		}
	}
}
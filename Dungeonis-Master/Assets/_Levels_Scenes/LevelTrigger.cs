namespace LevelManager
{
	using UnityEngine;

public class LevelTrigger
	: MonoBehaviour
{
	public SceneIndexes LoadSceneName;
	private Player player;

		void Start ()
		{
			player              = GameObject.FindObjectOfType<Player>();
		}
	
	private void OnTriggerEnter(Collider collider)
	{
		player.SavePlayer ();
		LoadingScreenManager.LoadScene((int)LoadSceneName);
	}
}

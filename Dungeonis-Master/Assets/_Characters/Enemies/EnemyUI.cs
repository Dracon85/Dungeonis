// Add a UI Socket transform to your enemy
// Attack this script to the socket
// Link to a canvas prefab that contains NPC UI
namespace RPG.Characters
{
	using UnityEngine;

	public class EnemyUI
		: MonoBehaviour
	{
		// Works around Unity 5.5's lack of nested prefabs
		[Tooltip("The UI canvas prefab")]
		[SerializeField] private GameObject enemyCanvasPrefab = null;

		private Camera cameraToLookAt;

		private void Start()
		{
			cameraToLookAt = Camera.main;
			Instantiate(enemyCanvasPrefab, transform.position, Quaternion.identity, transform);
		}

		private void LateUpdate()
		{
			transform.LookAt(cameraToLookAt.transform);
		}
	}
}
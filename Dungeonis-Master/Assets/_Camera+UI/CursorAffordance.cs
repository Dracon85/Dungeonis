namespace RPG.CameraUI
{
	using UnityEngine;

	[RequireComponent(typeof(CameraRaycaster))]
	public class CursorAffordance
		: MonoBehaviour
	{

		[SerializeField] private Texture2D walkCursor    = null;
		[SerializeField] private Texture2D attackCursor  = null;
		[SerializeField] private Texture2D unknownCursor = null;
		[SerializeField] private Vector2 cursorHotspot   = new Vector2(0, 0);
		//TODO solve fight between Serialize and const
		[SerializeField] private const int walkableLayerNumber = 8;
		[SerializeField] private const int enemyLayerNumber    = 9;

		private CameraRaycaster cameraRaycaster;

		private void Start()
		{
			cameraRaycaster                             = GetComponent<CameraRaycaster>();
			cameraRaycaster.notifyLayerChangeObservers += OnLayerChange;
		}

		private void OnLayerChange(int newLayer)
		{
			switch (newLayer)
			{
				case walkableLayerNumber:
					Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
					break;
				case enemyLayerNumber:
					Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
					break;
				default:
					Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
					return;
			}
		}
	}
}

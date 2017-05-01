namespace RPG.UtilScripts
{
	using System.Collections.Generic;
	using UnityEngine;

	public class UnlockDoor
		: MonoBehaviour
	{
		// could drag your key gameobjects into the inspector to set them
		public List<GameObject> KeysToDoorRemaining = new List<GameObject>();

		// when a key is destroyed, call a method on the door passing in the gameobject that was destroyed
		public void KeyUsed(GameObject keyGameObject)
		{
			KeysToDoorRemaining.Remove(keyGameObject);

			if (KeysToDoorRemaining.Count <= 0)
				Destroy(gameObject);
		}

		private void Start()
		{
			// so for each gameobject that you specify as a key, we add a script component to it
			// and that component will say, once then object is destroyed
			// call the KeyUsed method and remove that item from the list
			foreach (GameObject DoorUnlockers in KeysToDoorRemaining)
			{
				DoorUnlockers.AddComponent<KeyObject>();
				DoorUnlockers.GetComponent<KeyObject>().doorThisIsFor = this;
			}
		}
	}
}
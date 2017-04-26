namespace RPG.UtilScripts
{
	using UnityEngine;

	public class KeyObject
		: MonoBehaviour
	{
		public UnlockDoor doorThisIsFor;

		void OnDestroy()
		{
			doorThisIsFor.KeyUsed(this.gameObject);
		}
	}
}

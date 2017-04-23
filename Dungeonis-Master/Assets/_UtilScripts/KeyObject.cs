using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UtilScripts{
public class KeyObject: MonoBehaviour
{
	public UnlockDoor doorThisIsFor;

	void OnDestroy()
	{
		doorThisIsFor.KeyUsed(this.gameObject);
	}
}
}

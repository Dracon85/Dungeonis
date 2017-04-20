using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject: MonoBehaviour
{
	public UnlockDoor doorThisIsFor;

	void OnDestroy()
	{
		doorThisIsFor.KeyUsed(this.gameObject);
	}
}

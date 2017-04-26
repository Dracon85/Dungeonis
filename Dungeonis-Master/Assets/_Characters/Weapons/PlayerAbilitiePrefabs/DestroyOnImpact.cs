using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO consider rewiring...
using RPG.UtilScripts;

namespace RPG.Weapons{
public class DestroyOnImpact
		: MonoBehaviour{

	void OnCollisionEnter(Collision collision)
	{
			//layer number of enemy in project settings
			if (collision.gameObject.layer == 9){
					Object.Destroy (gameObject);
				}
			}
	}
}

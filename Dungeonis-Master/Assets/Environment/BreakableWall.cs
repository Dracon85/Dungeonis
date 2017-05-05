using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UtilScripts{
	public class BreakableWall : MonoBehaviour, IDamageable {

	// Use this for initialization
	public float MaxHealthPoints;
	public float CurrentHealthPoints;
	public Color colourPicker = new Color (0.5f, 0.5f, 0.5f);
	private float alphaMinus;


	void Start () {
		CurrentHealthPoints = MaxHealthPoints;
		this.gameObject.GetComponent<MeshRenderer> ();		
	}

	public void TakeDamage(float damage)
	{
		//object gets more transparent as health decreases
		alphaMinus = CurrentHealthPoints / MaxHealthPoints;
		colourPicker.a = alphaMinus;
		this.gameObject.GetComponent<MeshRenderer> ().material.SetColor ("_Color", colourPicker);
			//code to destroy object when health reaches 0
		CurrentHealthPoints = Mathf.Clamp(CurrentHealthPoints - damage, 0f, MaxHealthPoints);	
			if (CurrentHealthPoints <= 0)
				Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour {

	//tempoary till we get abilities set up properly. TODO apply to an ability array size of 10
	[SerializeField]GameObject projectileSocket;
	[SerializeField]GameObject projectileToUse;
	[SerializeField]float damagePerShot=5f;
	[SerializeField]float attackDelay = 0.5f;
	float lastHitTime = 0f;
	private Transform _cam;

	// Use this for initialization
	void Start () {
		if (Camera.main != null)
			_cam = Camera.main.transform;
		else
		{
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}		
	}

	// Update is called once per frame
	void Update () {
		//temp fire logic TODO make real ability array
		if (Input.GetButtonDown ("Fire1")) {
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100f)){
				Debug.DrawLine (_cam.position, hit.point);
				// cache oneSpawn object in spawnPt, if not cached yet
				if (Time.time-lastHitTime>attackDelay){//add attack delay
					if (!projectileSocket) projectileSocket = GameObject.Find("oneSpawn");

					GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
					//set last hit time
					lastHitTime = Time.time;
					Projectile projectileComponent = newProjectile.GetComponent<Projectile>();

					projectileComponent.SetDamage(damagePerShot);
					projectileComponent.SetShooter (gameObject);

					float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();

					// turn the projectile to hit.point
					newProjectile.transform.LookAt(hit.point); 
					newProjectile.GetComponent<Rigidbody>().velocity = newProjectile.transform.forward*projectileSpeed;
				}		
			}

		}
	}
}

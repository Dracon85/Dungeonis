namespace RPG.Characters
{
	using UnityEngine;
	using RPG.Weapons;

	public class PlayerAttacks
		: MonoBehaviour
	{
		//tempoary till we get abilities set up properly. TODO apply to an ability array size of 10
		[SerializeField] private GameObject _projectileSocket;
		[SerializeField] private GameObject _projectileToUse;
		[SerializeField] private float _damagePerShot = 5f;
		[SerializeField] private float _attackDelay = 0.5f;
		[SerializeField] private LayerMask _layerMask;
		private float _lastHitTime = 0f;
		private GameObject _player;
		private Player _playerComponent;

		// Use this for initialization
		void Start ()
		{
			_player          = GameObject.FindGameObjectWithTag("Player");
			_playerComponent = _player.GetComponent<Player>();
		}

		// TODO Refactor this once we get new attack types
		void Update()
		{
			if (Input.GetButtonDown("Fire1"))
			{
				RaycastHit hit;
				Ray ray = new Ray(new Vector3(_player.transform.position.x, 0.50f, _player.transform.position.z), _player.transform.forward);

				if (Physics.Raycast(ray, out hit, _playerComponent.GetAttackRange(), _layerMask))
				{
					Debug.DrawLine(new Vector3(_player.transform.position.x, 0.50f, _player.transform.position.z), hit.point);

					// cache oneSpawn object in spawnPt, if not cached yet
					//add attack delay
					if (Time.time - _lastHitTime > _attackDelay)
					{
						if (!_projectileSocket)
							_projectileSocket = GameObject.Find("ShotOrigin");

						GameObject newProjectile = Instantiate(_projectileToUse, _projectileSocket.transform.position, Quaternion.identity);
						_lastHitTime = Time.time;
						Projectile projectileComponent = newProjectile.GetComponent<Projectile>();

						projectileComponent.SetDamage(_damagePerShot);
						projectileComponent.SetShooter(gameObject);

						newProjectile.transform.LookAt(hit.point);
						newProjectile.GetComponent<Rigidbody>().velocity = newProjectile.transform.forward * projectileComponent.GetDefaultLaunchSpeed();
					}
				}
			}
		}
	}
}

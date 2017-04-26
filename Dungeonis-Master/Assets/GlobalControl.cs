using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using RPG.Characters;

public class GlobalControl : MonoBehaviour {

	public static GlobalControl Instance;
	public PlayerStatistics savedPlayerData = new PlayerStatistics ();
	public GameObject Player;

		void Awake ()   
		{
			if (Instance == null)
			{
				DontDestroyOnLoad(gameObject);
				Instance = this;
			}
			else if (Instance != this)
			{
				Destroy (gameObject);
			}
		}
	}
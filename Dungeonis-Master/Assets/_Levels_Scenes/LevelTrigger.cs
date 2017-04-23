﻿using UnityEngine;

namespace LevelManager{
public class LevelTrigger
	: MonoBehaviour
{
	public SceneIndexes LoadSceneName;

	private void OnTriggerEnter(Collider collider)
	{
		LoadingScreenManager.LoadScene((int)LoadSceneName);
	}
}
}
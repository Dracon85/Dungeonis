﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

namespace RPG.CameraUI{
public class CameraRaycaster
	: MonoBehaviour
{
	// INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
	[SerializeField] int[] layerPriorities;

	float maxRaycastDepth         = 100f;	// Hard coded value
	int topPriorityLayerLastFrame = -1;		// So get ? from start with Default layer terrain

	// Setup delegates for broadcasting layer changes to other classes
	public delegate void OnCursorLayerChange(int newLayer);			// Declare new delegate type
	public event OnCursorLayerChange notifyLayerChangeObservers;	// Instantiate an observer set

	public delegate void OnClickPriorityLayer(RaycastHit raycastHit, int layerHit); // Declare new delegate type
	public event OnClickPriorityLayer notifyMouseClickObservers;					// Instantiate an observer set

	// Fix mouse click bug TODO somehow use this with third player char control?
	[SerializeField] const int walkableLayerNumber=8;
	[SerializeField] const int enemyLayerNumber=9;

	void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
	{
		switch (layerHit)
		{
			case enemyLayerNumber:
				GameObject enemy = raycastHit.collider.gameObject;
				Debug.Log(enemy);
				break;
			default:
				Debug.Log("UnspecifiedAction for clicking on this in CameraRaycaster");
				return;
		}
	}
	void Start()
	{
		notifyMouseClickObservers += ProcessMouseClick;
	}

	//end TODO
	void Update()
	{
		// Check if pointer is over an interactable UI element
		if (EventSystem.current.IsPointerOverGameObject())
		{
			NotifyObserersIfLayerChanged (5);
			return; // Stop looking for other objects
		}

		// Raycast to max depth, every frame as things can move under mouse
		Ray ray                  = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);

		RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);

		if (!priorityHit.HasValue) // if hit no priority object
		{
			NotifyObserersIfLayerChanged(0); // broadcast default layer
			return;
		}

		// Notify delegates of layer change
		int layerHit = priorityHit.Value.collider.gameObject.layer;
		NotifyObserersIfLayerChanged(layerHit);

		// Notify delegates of highest priority game object under mouse when clicked
		if (Input.GetMouseButton(0))
		{
			notifyMouseClickObservers (priorityHit.Value, layerHit);
		}
	}

	void NotifyObserersIfLayerChanged(int newLayer)
	{
		if (newLayer != topPriorityLayerLastFrame)
		{
			topPriorityLayerLastFrame = newLayer;
			notifyLayerChangeObservers(newLayer);
		}
	}

	RaycastHit? FindTopPriorityHit (RaycastHit[] raycastHits)
	{
		// Form list of layer numbers hit
		List<int> layersOfHitColliders = new List<int> ();

		foreach (RaycastHit hit in raycastHits)
		{
			layersOfHitColliders.Add(hit.collider.gameObject.layer);
		}

		// Step through layers in order of priority looking for a gameobject with that layer
		foreach (int layer in layerPriorities)
		{
			foreach (RaycastHit hit in raycastHits)
			{
				if (hit.collider.gameObject.layer.Equals(layer))
					return hit; // stop looking
			}
		}

		return null; // because cannot use GameObject? nullable
	}
}
}
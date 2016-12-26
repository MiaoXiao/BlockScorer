using UnityEngine;

interface IInteractable
{
	string OnActivate();								// Visually activate object and return pickup name
	void OnDeactivate();							// Visually deactivate object
	void OnInteract(GameObject actor);		// Acting on the item
    GameObject GetGameObject();             // Returns name of gameobject
}
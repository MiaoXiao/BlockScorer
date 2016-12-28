using UnityEngine;

interface IInteractable
{
	string OnActivate();								// Visually activate object and return pickup name
	void OnDeactivate();							// Visually deactivate object
	bool OnInteract(GameObject actor);		// Acting on the item, return true if picking up the item
    GameObject GetGameObject();             // Returns name of gameobject
}
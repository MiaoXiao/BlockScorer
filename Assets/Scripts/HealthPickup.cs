using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour, IInteractable
{
	public string healthName = "Health Kit";
	public ItemAttributes attributes;
	
	public Renderer activateRenderer;
	public Color normalColor = Color.white;
	public Color activeColor = Color.green;
	
	public string OnActivate()
	{
		if(activateRenderer != null)
			activateRenderer.material.color = activeColor;
		
		return healthName;
	}
	
	public void OnDeactivate()
	{
		if(activateRenderer != null)
			activateRenderer.material.color = normalColor;
	}
	
	public void OnInteract(GameObject actor)
	{
		Destroy(gameObject);
	}
}

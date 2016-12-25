using UnityEngine;
using System.Collections;

public class PlayerInteractController : MonoBehaviour
{
	public PlayerInput input;
	public UIController uiController;
	private IInteractable currentInteractionTarget { get; set; }
	
	private void Start()
	{
		if(input != null)
		{
			input.onActivate += OnActivate;
		}
	}
	
	private void OnDestroy()
	{
		if(input != null)
		{
			input.onActivate += OnActivate;
		}
	}
	
	private bool OnActivate()
	{
		if(currentInteractionTarget == null)
			return false;
		
		currentInteractionTarget.OnInteract(this.gameObject);
		DeactivateInteraction();
		
		return true;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		IInteractable interactComponent = other.GetComponent<IInteractable>();
		if(interactComponent == null)
			return;
		
		if(currentInteractionTarget != null)
			currentInteractionTarget.OnDeactivate();
		
		currentInteractionTarget = interactComponent;
		ActivateInteraction();
	}
	
	private void OnTriggerExit(Collider other)
	{
		if(currentInteractionTarget == null)
			return;
		
		IInteractable interactComponent = other.GetComponent<IInteractable>();
		if(interactComponent == null)
			return;
		
		if(currentInteractionTarget == interactComponent)
		{
			currentInteractionTarget.OnDeactivate();
			DeactivateInteraction();
		}
	}
	
	private void ActivateInteraction()
	{
		string pickupName = currentInteractionTarget.OnActivate();
		if(uiController != null)
			uiController.ActivateTargetName(pickupName);
	}
	
	private void DeactivateInteraction()
	{
		currentInteractionTarget = null;
		if(uiController != null)
			uiController.DeactivateTargetName();
	}
}

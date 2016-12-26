using UnityEngine;
using System.Collections;

public class PlayerInteractController : MonoBehaviour
{
    public PlayerInput input;
	public UIController uiController;
	private IInteractable currentInteractionTarget { get; set; }

    /// <summary>
    /// Is the player currently grabbing something?
    /// </summary>
    private GameObject grabbedObject = null;

    /// <summary>
    /// Set to true when looking at an interactable object
    /// </summary>
    private bool mouseOvered = false;

    //References
    private Camera MC;
    private GameController GC;

    private void Awake()
    {
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

	private void Start()
	{
		if(input != null)
		{
			input.onActivate += OnActivate;
            input.onLaunch += ActivateLaunch;
		}
    }

    private void Update()
    {
        if (GC.GamePaused)
            return;

        RaycastHit hit;
        Ray ray = MC.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            IInteractable interactComponent = hit.transform.GetComponent<IInteractable>();
            if (interactComponent != null)
            {

                ActivateInteraction();
                mouseOvered = true;
            }
            else
            {
                DeactivateInteraction();
                mouseOvered = false;
                grabbedObject = null;
            }
        }
    }

    private void OnDestroy()
    {
        if (input != null)
        {
            input.onActivate -= OnActivate;
            input.onLaunch -= ActivateLaunch;
        }
    }

	private bool OnActivate()
	{
		if(currentInteractionTarget == null)
			return false;
		
		currentInteractionTarget.OnInteract(this.gameObject);
		
		return true;
	}

    private void OnTriggerStay(Collider other)
    {
        if (!mouseOvered)
            return;

        IInteractable interactComponent = other.GetComponent<IInteractable>();
        if (interactComponent == null)
            return;

        if (currentInteractionTarget != null)
            currentInteractionTarget.OnDeactivate();

        currentInteractionTarget = interactComponent;
        
	}
	
	private void OnTriggerExit(Collider other)
	{
		
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
        if (currentInteractionTarget == null)
            return;

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

    /// <summary>
    /// Launch grabbed object
    /// </summary>
    private void ActivateLaunch(float throw_strength)
    {
        if (currentInteractionTarget == null)
            return;

        CratePickUp crate = currentInteractionTarget.GetGameObject().GetComponent<CratePickUp>();

        crate.ReleaseObject();
        crate.RB.AddForce(MC.transform.forward * throw_strength, ForceMode.Impulse);

        //Debug.Log("throwing object");
    }
}

using UnityEngine;
using System.Collections;

public class PlayerInteractController : MonoBehaviour
{
    public PlayerInput input;

    /// <summary>
    /// Object that is applicible to be picked up
    /// </summary>
	private IInteractable currentInteractionTarget { get; set; }


    /// <summary>
    /// Interactable Object we are looking at
    /// </summary>
    private GameObject MouseOveredObject = null;

    /// <summary>
    /// Interactable Object we are currently grabbing
    /// </summary>
    private GameObject GrabbedObject = null;


    //References
    private Camera MC;
    private GameController GC;
    private UIController UC;

    private void Awake()
    {
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();
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
                MouseOveredObject = hit.transform.gameObject;
            }
            else
            {
                MouseOveredObject = null;
                DeactivateInteraction();
            }
        }
        else
        {
            MouseOveredObject = null;
            DeactivateInteraction();
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
		
		bool grabbing = currentInteractionTarget.OnInteract(this.gameObject);
        if (grabbing)
            GrabbedObject = currentInteractionTarget.GetGameObject();
        else
            GrabbedObject = null;

		return true;
	}

    /// <summary>
    /// Allow player to pick up interactable object, if within range and player is looking at it
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        IInteractable interactComponent = other.GetComponent<IInteractable>();

        if (MouseOveredObject == null || interactComponent == null)
            return;

        if (interactComponent.GetGameObject() != MouseOveredObject)
            return;

        if (currentInteractionTarget != null)
            currentInteractionTarget.OnDeactivate();

        currentInteractionTarget = interactComponent;

        ActivateInteraction();

    }

    private void OnTriggerExit(Collider other)
    {
        if (MouseOveredObject)
            DeactivateInteraction();
    }

	private void ActivateInteraction()
	{
        if (currentInteractionTarget == null)
            return;

        UC.cursorImage.color = Color.green;

        string pickupName = currentInteractionTarget.OnActivate();
		if(UC != null)
            UC.ActivateTargetName(pickupName);
	}
	
	private void DeactivateInteraction()
	{
        UC.cursorImage.color = Color.white;

        currentInteractionTarget = null;
		if(UC != null)
            UC.DeactivateTargetName();
    }

    /// <summary>
    /// Launch grabbed object, or if not grabbing anything, the current interaction target
    /// </summary>
    private void ActivateLaunch(float throw_strength)
    {
        if (currentInteractionTarget == null)
            return;

        CratePickUp crate;
        if (GrabbedObject != null)
            crate = GrabbedObject.GetComponent<CratePickUp>();
        else
            crate = currentInteractionTarget.GetGameObject().GetComponent<CratePickUp>();

        crate.ReleaseObject();
        crate.RB.AddForce(MC.transform.forward * throw_strength, ForceMode.Impulse);

        //Debug.Log("throwing object");
    }
}

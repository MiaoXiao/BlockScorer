using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePickUp : MonoBehaviour, IInteractable
{
    public string crateName = "Crate";
    public ItemAttributes attributes;

    public Renderer activateRenderer;
    public Color normalColor = Color.white;
    public Color activeColor = Color.green;

    private bool isGrabbed = false;

    public string OnActivate()
    {
        if (activateRenderer != null)
            activateRenderer.material.color = activeColor;

        return crateName;
    }

    public void OnDeactivate()
    {
        if (activateRenderer != null)
            activateRenderer.material.color = normalColor;
    }

    public void OnInteract(GameObject actor)
    {
        if (isGrabbed)
            ReleaseObject(actor);
        else
            GrabObject(actor);

        isGrabbed = !isGrabbed;
    }

    /// <summary>
    /// Grab the object and place in front of player
    /// </summary>
    public void GrabObject(GameObject actor)
    {

    }

    /// <summary>
    /// Release the grabbed object
    /// </summary>
    public void ReleaseObject(GameObject actor)
    {

    }
}

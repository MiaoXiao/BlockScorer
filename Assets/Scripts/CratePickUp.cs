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

    //References
    private Rigidbody CrateRB;
    private GameObject Player;
    private Camera MainCamera;

    /// <summary>
    /// Distance crate is carried in front of player
    /// </summary>
    public float holdDistance = 1f;

    /// <summary>
    /// Set to true when crate is picked up
    /// </summary>
    private bool isGrabbed = false;

    private void Awake()
    {
        CrateRB = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

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
        isGrabbed = !isGrabbed;
        if (!isGrabbed)
            ReleaseObject(actor);
        else
            CrateRB.useGravity = false;
    }

    private void FixedUpdate()
    {
        if (isGrabbed)
        {
            Debug.Log("grabbing");
            CrateRB.MovePosition(MainCamera.transform.position + MainCamera.transform.forward * holdDistance * Time.fixedDeltaTime);
            //Debug.Log(MainCamera.transform.position + " " + MainCamera.transform.forward + " " + holdDistance);
        }
    }

    /// <summary>
    /// Release the grabbed object
    /// </summary>
    public void ReleaseObject(GameObject actor)
    {
        Debug.Log("release object");
        CrateRB.useGravity = true;
    }
}

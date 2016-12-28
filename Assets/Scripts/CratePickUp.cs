using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePickUp : MonoBehaviour, IInteractable
{
    public string crateName = "Normal Crate";
    //public CrateType type;

    public Renderer activateRenderer;
    public Color normalColor = Color.white;
    public Color activeColor = Color.green;

    /// <summary>
    /// Number of points added to the score when this crate is scored.
    /// Also the amount of time added to the clock when scored.
    /// </summary>
    public int Points = 10;

    //References
    public Rigidbody RB;
    private GameObject Player;
    private Camera MainCamera;

    /// <summary>
    /// Distance crate is carried in front of player
    /// </summary>
    public float holdDistance = 1f;

    /// <summary>
    /// Set to true when crate is picked up
    /// </summary>
    public bool isGrabbed = false;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
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
        if (isGrabbed)
            ReleaseObject();
        else
            GrabObject();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void FixedUpdate()
    {
        if (isGrabbed)
        {
            RB.MovePosition(MainCamera.transform.position + MainCamera.transform.forward * holdDistance * Time.fixedDeltaTime);
            //Debug.Log(MainCamera.transform.position + " " + MainCamera.transform.forward + " " + holdDistance);
        }
    }

    /// <summary>
    /// Grab the object
    /// </summary>
    public void GrabObject()
    {
        RB.useGravity = false;
        isGrabbed = true;
    }

    /// <summary>
    /// Release the grabbed object
    /// </summary>
    public void ReleaseObject()
    {
        //Debug.Log("release object");
        RB.useGravity = true;
        isGrabbed = false;
    }
}

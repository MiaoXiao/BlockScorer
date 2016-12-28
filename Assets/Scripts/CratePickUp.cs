using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePickUp : MonoBehaviour, IInteractable
{
    public string crateName = "Crate";
    //public CrateType type;

    public Renderer activateRenderer;
    public Color normalColor = Color.white;
    private Color activeColor = Color.green;

    /// <summary>
    /// Number of points added to the score when this crate is scored.
    /// </summary>
    public int PointsGained = 10;

    /// <summary>
    /// Number of points lost when this crate is missed
    /// </summary>
    public int PointsLost = 0;

    /// <summary>
    /// Amount of time added to the clock when scored.
    /// </summary>
    public int TimeGained = 10;

    /// <summary>
    /// Amount of time lost when missing this crate
    /// </summary>
    public int TimeLost = 10;

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

        activateRenderer.material.color = normalColor;

        activeColor = normalColor;
        activeColor.a = 0.5f;
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

    public bool OnInteract(GameObject actor)
    {
        if (isGrabbed)
        {
            ReleaseObject();
            return false;
        }

        GrabObject();
        return true;
        
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
        RB.velocity = Vector3.zero;
    }

    /// <summary>
    /// Release the grabbed object
    /// </summary>
    public void ReleaseObject()
    {
        //Debug.Log("release object");
        RB.useGravity = true;
        isGrabbed = false;
        RB.velocity = Vector3.zero;
    }
}

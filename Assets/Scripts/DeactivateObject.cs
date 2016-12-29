using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : MonoBehaviour
{
    [SerializeField]
    private float DeactivateInSeconds = 10f;

	// Use this for initialization
	void Start ()
    {
        Invoke("Deactivate", DeactivateInSeconds);	
	}
	
    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

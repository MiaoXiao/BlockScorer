using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractable
{
	public string doorName = "Door";
	
	public Renderer activateRenderer;
	public Color normalColor = Color.white;
	public Color activeColor = Color.green;
	
	public Collider doorCollider;
	public Transform hinge;
	public float doorOpenSpeed = 3f;
	public float doorCloseSpeed = 3f;
	
	private bool isDoorOpen { get; set; }
	
	public string OnActivate()
	{
		if(activateRenderer != null)
			activateRenderer.material.color = activeColor;
		
		return doorName;
	}
	
	public void OnDeactivate()
	{
		if(activateRenderer != null)
			activateRenderer.material.color = normalColor;
	
		if(isDoorOpen)
			CloseDoor();
	}
	
	public bool OnInteract(GameObject actor)
	{
		if(isDoorOpen)
			return true;
		
		OpenDoor();
        return false;
	}
	
    public GameObject GetGameObject()
    {
        return gameObject;
    }

	private void OpenDoor()
	{
		isDoorOpen = true;
		doorCollider.enabled = false;
		StartCoroutine("RoutineOpenDoor");
	}
	
	private void CloseDoor()
	{
		isDoorOpen = false;
		doorCollider.enabled = true;
		StartCoroutine("RoutineCloseDoor");
	}
	
	private IEnumerator RoutineOpenDoor()
	{
		while(hinge.localEulerAngles.y < 90f)
		{
			hinge.localEulerAngles = new Vector3(0f, Mathf.LerpAngle(hinge.localEulerAngles.y, 90f, doorOpenSpeed * Time.deltaTime), 0f);
			
			if(Mathf.Abs(hinge.localEulerAngles.y - 90f) < 1f)
				hinge.localEulerAngles = new Vector3(0, 90f, 0f);
			
			yield return null;
		}
	}
	
	private IEnumerator RoutineCloseDoor()
	{
		while(hinge.localEulerAngles.y > 0f)
		{
			hinge.localEulerAngles = new Vector3(0f, Mathf.LerpAngle(hinge.localEulerAngles.y, 0f, doorCloseSpeed * Time.deltaTime), 0f);
			
			if(Mathf.Abs(hinge.localEulerAngles.y - 0f) < 1f)
				hinge.localEulerAngles = Vector3.zero;
			
			yield return null;
		}
	}
}

using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public delegate void OnMove(bool isRunning, Vector2 direction);
	public OnMove onMove;
	
	public delegate void OnJump();
	public OnJump onJump;
	
	public delegate bool OnActivate();
	public OnActivate onActivate;
	
	public float inputThreshold = 0.05f;
	
	public bool isRunning { get; private set; }
	public Vector2 movementVector { get; private set; }

    /// <summary>
    /// Reference to game controller
    /// </summary>
    private GameController GC;

    /// <summary>
    /// Reference to main camera
    /// </summary>
    private GameObject MC; 

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        MC = GameObject.FindGameObjectWithTag("MainCamera");
    }

	private void Update()
	{
        if (GC.GamePaused)
            return;

        GetMovementInput();
		GetJumpInput();
		GetActivateInput();
    }
	
	private void GetMovementInput()
	{
		isRunning = Input.GetKey(KeyCode.LeftShift);
		
		movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if(movementVector.sqrMagnitude < inputThreshold)
			return;
		
		if(onMove != null)
			onMove(isRunning, movementVector);
	}
	
	private void GetJumpInput()
	{
		if(!Input.GetButtonDown("Jump"))
			return;
		
		if(onJump != null)
			onJump();
	}
	
	private void GetActivateInput()
	{
		if(!Input.GetKeyDown(KeyCode.E))
			return;
		
		if(onActivate == null)
			return;
		
		foreach(OnActivate del in onActivate.GetInvocationList())
		{
			if(del.Invoke())
				break;
		}
	}
}

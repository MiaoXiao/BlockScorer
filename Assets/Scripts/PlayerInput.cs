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

    public delegate void OnLaunch(float throw_strength);
    public OnLaunch onLaunch;

    public float inputThreshold = 0.05f;
	
	public bool isRunning { get; private set; }
	public Vector2 movementVector { get; private set; }

    /// <summary>
    /// Time it will take to fully charge a throw
    /// </summary>
    public float maxChargeTime = 3f;

    /// <summary>
    /// Maximum amount of strength from a fully charged throw
    /// </summary>
    public float maxThrowStrength = 75f;

    /// <summary>
    /// Current throw charge
    /// </summary>
    private float currentChargeTime = 0f;

    /// <summary>
    /// Reference to game controller
    /// </summary>
    private GameController GC;

    /// <summary>
    /// Reference to main camera
    /// </summary>
    private GameObject MC;

    /// <summary>
    /// Reference to ui controller
    /// </summary>
    private UIController UC;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        MC = GameObject.FindGameObjectWithTag("MainCamera");
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();
    }

	private void Update()
	{
        if (GC.GamePaused)
            return;

        GetMovementInput();
		GetJumpInput();
		GetActivateInput();
        GetLaunchInput();
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

    private void GetLaunchInput()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (onLaunch == null)
            return;

        StartCoroutine("DetermineThrowStrength");

    }

    /// <summary>
    /// Depending on how long the player is charging, launch the grabbed object
    /// </summary>
    /// <returns></returns>
    IEnumerator DetermineThrowStrength()
    {
        UC.SetActiveThrowMeter(true);

        float throw_strength = 0f;
        while (!Input.GetMouseButtonUp(0))
        {
            float norm_value = currentChargeTime / maxChargeTime;
            if (norm_value < 1)
            {
                currentChargeTime += Time.deltaTime;
                throw_strength = norm_value * maxThrowStrength;
                UC.throwCharge = UC.UpdateThrowCharge;
                UC.throwCharge(norm_value);
            }
            else
            {
                UC.throwCharge(1f);
            }

            yield return null;
        }

        onLaunch(throw_strength);
        currentChargeTime = 0f;

        UC.SetActiveThrowMeter(false);
    }
}

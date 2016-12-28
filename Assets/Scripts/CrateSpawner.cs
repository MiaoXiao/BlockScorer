using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public List<CrateDistribution> StageList = new List<CrateDistribution>();

    [SerializeField]
    private Vector2 SpawnArea;

    private int _CurrentStage = -1;
    public int CurrentStage
    {
        get
        {
            return _CurrentStage;
        }
        set
        {
            if (value == _CurrentStage || value < 0 || value >= StageList.Count)
                return;

            _CurrentStage = value;

            nextStage(_CurrentStage);
        }
    }

    /// <summary>
    /// Moving on to a new stage
    /// </summary>
    public delegate void NextStage(int stage);
    public NextStage nextStage;

    private int CurrNumbCrates = 0;

    private ObjectPooler[] AllObjPools;

    private GameController GC;
    private UIController UC;

    private void Awake()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        UC = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIController>();

        AllObjPools = GetComponents<ObjectPooler>();
    }

    private void Start()
    {
        GC.crateEvaluated += CheckStageDone;
        nextStage += GoToNextStage;
        nextStage += CheckEnvionment;
        CurrentStage = 0;
    }

    /// <summary>
    /// Go to the specified stage
    /// </summary>
    private void GoToNextStage(int stage)
    {
        Debug.Log("going to stage " + stage);
        SpawnObjects(StageList[CurrentStage]);
    }

    /// <summary>
    /// Decrease number of current crates or go to the next stage if there are 0 crates;
    /// </summary>
    private void CheckStageDone()
    {
        CurrNumbCrates--;
        if (CurrNumbCrates <= 0)
        {
            CurrentStage++;
        }
        Debug.Log("numb crates is now " + CurrNumbCrates);
    }

    /// <summary>
    /// Enable eniornment hazards if necessary
    /// </summary>
    private void CheckEnvionment(int stage)
    {
        
    }

    /// <summary>
    /// Begin spawning new crates at random locations within the spawn area.
    /// CrateDistribution contains information about which crates should appear, and how many total crates there are
    /// </summary>
    private void SpawnObjects(CrateDistribution cp)
    {
        for (int i = 0; i < AllObjPools.Length; ++i)
        {
            for (int j = 0; j < cp.RetrieveCrateAmount(i); ++j)
            {
                float rand_x = Random.Range(-SpawnArea.x, SpawnArea.x);
                float rand_y = Random.Range(-SpawnArea.y, SpawnArea.y);

                GameObject crate = AllObjPools[i].RetrieveCopy();
                crate.transform.position = new Vector3(rand_x, transform.position.y, rand_y);
            }
        }
        CurrNumbCrates = cp.Total;
    }
}

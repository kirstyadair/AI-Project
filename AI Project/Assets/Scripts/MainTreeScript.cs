using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentSubtree
{
    DRIVENORMALLY,
    SIREN_NO_CORRECTIONS,
    SIREN_WITH_CORRECTIONS,
    CORRECTIONS_REQUIRED,
    PREPARING
}

public enum State
{
    SUCCESSFUL,
    RUNNING,
    FAILED
}

public class MainTreeScript : MonoBehaviour
{
    public CurrentSubtree state;
    DriveNormallyScript driveNormally;



    private void Start()
    {
        state = CurrentSubtree.PREPARING;
        driveNormally = GetComponent<DriveNormallyScript>();
    }



    void Update()
    {
        if (state == CurrentSubtree.DRIVENORMALLY || state == CurrentSubtree.PREPARING)
        {
            if (driveNormally.StartTree() == State.SUCCESSFUL) Debug.Log("Tree finished");
        }
    }
}


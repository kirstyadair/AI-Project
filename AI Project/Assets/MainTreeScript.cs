using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentSubtree
{
    DRIVENORMALLY,
    SIREN_NO_CORRECTIONS,
    SIREN_WITH_CORRECTIONS,
    CORRECTIONS_REQUIRED
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
    [SerializeField] DriveNormallyScript driveNormally;

    void Update()
    {
        if (state == CurrentSubtree.DRIVENORMALLY)
        {
            if (driveNormally.StartTree() == State.SUCCESSFUL) Debug.Log("Tree finished");
        }
    }
}


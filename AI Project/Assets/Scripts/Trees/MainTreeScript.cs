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
    SirenNoCorrectionsScript siren1Script;
    [HideInInspector]
    public BoxCollider collider;


    private void OnEnable()
    {
        PoliceCarScript.OnSirenSounded += SirenEnabled;
    }


    private void Start()
    {
        driveNormally = GetComponent<DriveNormallyScript>();
        siren1Script = GetComponent<SirenNoCorrectionsScript>();
        collider = this.gameObject.GetComponent<BoxCollider>();
    }



    void Update()
    {
        if (state == CurrentSubtree.DRIVENORMALLY)
        {
            if (driveNormally.StartTree() == State.SUCCESSFUL) Debug.Log("Tree finished");
        }
        else if (state == CurrentSubtree.SIREN_NO_CORRECTIONS)
        {
            siren1Script.StartTree();
        }
    }



    void SirenEnabled()
    {
        state = CurrentSubtree.SIREN_NO_CORRECTIONS;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveNormallyScript : MonoBehaviour
{
    CarScript car;
    
    bool returnToMainTree = false;
    bool switchingLanes = false;

    private void Start()
    {
        car = GetComponent<CarScript>();
    }

    /// <summary>
    /// Call this when switching back to the DriveNormally state
    /// </summary>
    public void ResetTree()
    {
        returnToMainTree = false;
        switchingLanes = false;
        StartTree();
    }

    public State StartTree()
    {
        if (!switchingLanes)
        {
            car.SteerToNextPoint();
        }
        else
        {
            //switchingLanes();
        }

        if (!returnToMainTree) return State.RUNNING;
        else return State.SUCCESSFUL;
    }
}

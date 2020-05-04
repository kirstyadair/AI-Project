using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenNoCorrectionsScript : MonoBehaviour
{
    CarScript car;
    MainTreeScript mainTree;

    private void OnEnable()
    {
        PoliceCarScript.OnSirenDisabled += SirenOff;
    }

    private void Start()
    {
        car = GetComponent<CarScript>();
        mainTree = GetComponent<MainTreeScript>();
    }

    public void StartTree()
    {
        car.leftLaneSpeed = 0.05f;
        car.rightLaneSpeed = 0.02f;
        mainTree.state = CurrentSubtree.DRIVENORMALLY;
    }

    void SirenOff()
    {
        car.leftLaneSpeed = 0.04f;
        car.rightLaneSpeed = car.heldRightLaneSpeed;
        mainTree.state = CurrentSubtree.DRIVENORMALLY;
    }
}

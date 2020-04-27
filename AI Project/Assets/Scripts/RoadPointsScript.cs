using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPointsScript : MonoBehaviour
{
    [SerializeField] GameObject leftLanePoints;
    [SerializeField] GameObject rightLanePoints;

    public Transform[] GetPoints(bool isLeftLane)
    {
        if (isLeftLane)
        {
            return leftLanePoints.GetComponentsInChildren<Transform>();
        }
        else
        {
            return rightLanePoints.GetComponentsInChildren<Transform>();
        }
    }
}

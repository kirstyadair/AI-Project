using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPointsScript : MonoBehaviour
{
    [SerializeField] GameObject leftLanePoints;
    [SerializeField] GameObject rightLanePoints;
    public Transform[] leftPoints;
    public Transform[] rightPoints;

    void Start()
    {
        leftPoints = leftLanePoints.GetComponentsInChildren<Transform>();
        rightPoints = rightLanePoints.GetComponentsInChildren<Transform>();
    }
}

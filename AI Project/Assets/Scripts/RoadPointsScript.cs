using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPointsScript : MonoBehaviour
{
    [SerializeField] GameObject leftLanePoints;
    [SerializeField] GameObject rightLanePoints;
    public List<Transform> leftPoints = new List<Transform>();
    public List<Transform> rightPoints = new List<Transform>();



    void Start()
    {
        for (int i = 0; i < leftLanePoints.transform.childCount; i++)
        {
            leftPoints.Add(leftLanePoints.transform.GetChild(i));
        }
        for (int i = 0; i < rightLanePoints.transform.childCount; i++)
        {
            rightPoints.Add(rightLanePoints.transform.GetChild(i));
        }
    }
}

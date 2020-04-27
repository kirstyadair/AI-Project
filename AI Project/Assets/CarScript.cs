using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public bool isInLeftLane = false;
    GameObject currentRoadPiece;
    Transform[] leftPoints;
    Transform[] rightPoints;
    int currentPointNumber = 0;

    public void SteerToNextPoint()
    {
        Seek(currentPointNumber);
    }

    public void Seek(int pointNumber)
    {
        // Seek code here

        // Add to currentPointNumber once within certain radius of point
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            currentRoadPiece = collision.gameObject;
            leftPoints = collision.gameObject.GetComponent<RoadPointsScript>().leftPoints;
            rightPoints = collision.gameObject.GetComponent<RoadPointsScript>().rightPoints;
            currentPointNumber = 0;
        }
    }
}

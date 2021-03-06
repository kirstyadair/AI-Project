﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public bool isInLeftLane = false;
    GameObject currentRoadPiece;
    RoadPointsScript[] leftPoints;
    RoadPointsScript[] rightPoints;
    public int currentPointNumber = 0;
    public float speed;
    // Used to reset the right lane speed
    [HideInInspector]
    public float heldRightLaneSpeed;
    public float rightLaneSpeed;
    public float leftLaneSpeed;
    [SerializeField] int routeIndex;
    Vector3 desiredVelocity;
    Rigidbody rigidbody;
    MainScript main;
    MainTreeScript mainTree;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        main = GameObject.Find("GameController").GetComponent<MainScript>();
        mainTree = GetComponent<MainTreeScript>();
        leftPoints = main.routes[routeIndex].leftPoints;
        rightPoints = main.routes[routeIndex].rightPoints;
        heldRightLaneSpeed = rightLaneSpeed;
    }



    public void SteerToNextPoint()
    {
        Vector3 pointPosition;
        if (isInLeftLane) pointPosition = leftPoints[currentPointNumber].position;
        else
        {
            pointPosition = rightPoints[currentPointNumber].position;
        }
        Seek(pointPosition);
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(4, 0.5f, 0.5f));
    }



    /// <summary>
    /// Seek any given point, and rotate towards the point
    /// </summary>
    /// <param name="targetPosition">The position to be seeked</param>
    public void Seek(Vector3 targetPosition)
    {
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(desiredVelocity), 0.05f);
        transform.position += desiredVelocity;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            int previousPoint = currentPointNumber;
            currentPointNumber++;
            if ((currentPointNumber >= rightPoints.Length && !isInLeftLane) || (currentPointNumber >= leftPoints.Length && isInLeftLane)) currentPointNumber = 0;

   
        }
    }



    public Vector3 PredictFuturePosition(float timeInFuture)
    {
        float counter = 0;
        int closestPoint = 0;
        int futurePoint = currentPointNumber;
        
        for (int i = 1; i < 20; i++)
        {
            if (isInLeftLane)
            {
                futurePoint++;
                if (futurePoint >= leftPoints.Length) futurePoint -= leftPoints.Length;
                counter += leftPoints[futurePoint].timeToNextPoint / (speed * 100);

                if (counter > timeInFuture)
                {
                    if (futurePoint == 0) closestPoint = leftPoints.Length - 1;
                    else closestPoint = futurePoint - 1;
                    
                    break;
                    
                }
            }
            else
            {
                futurePoint++;
                if (futurePoint >= rightPoints.Length) futurePoint -= rightPoints.Length;
                counter += rightPoints[futurePoint].timeToNextPoint / (speed * 100);

                if (counter > timeInFuture)
                {
                    if (futurePoint == 0) closestPoint = rightPoints.Length - 1;
                    else closestPoint = futurePoint - 1;

                    break;

                }
            }
        }
        if (isInLeftLane) return leftPoints[closestPoint].position;
        else return rightPoints[closestPoint].position;
    }



    /// <summary>
    /// Predict the time it will take to reach a given point from the current point
    /// </summary>
    /// <param name="pointNumber">Which point in the future?</param>
    /// <param name="inLeftLane">Is the car in the left lane?</param>
    public float PredictTimeToPoint(int pointNumber, bool inLeftLane)
    {
        if (!inLeftLane)
        {
            int point = currentPointNumber;
            float timeTaken = 0;
            do
            {
                // Find the time from this point to the next point
                timeTaken += rightPoints[point].timeToNextPoint / (speed * 100);
                point++;
                if (point >= rightPoints.Length)
                {
                    point = 0;
                }

            } while (point != pointNumber);

            return timeTaken;
        }
        else
        {
            int point = currentPointNumber;
            float timeTaken = 0;
            do
            {
                // Find the time from this point to the next point
                timeTaken += leftPoints[point].timeToNextPoint / (speed * 100);
                point++;
                if (point >= leftPoints.Length)
                {
                    point = 0;
                }

            } while (point != pointNumber);

            return timeTaken;
        }
    }



    public void SwitchingLanes()
    {
        if (!isInLeftLane)
        {
            isInLeftLane = true;
            speed = leftLaneSpeed;
        }
        else
        {
            isInLeftLane = false;
            speed = rightLaneSpeed;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public bool isInLeftLane = false;
    GameObject currentRoadPiece;
    RoadPointsScript[] leftPoints;
    RoadPointsScript[] rightPoints;
    public int currentPointNumber = 0;
    [SerializeField] float speed;
    [SerializeField] int routeIndex;
    Vector3 desiredVelocity;
    Rigidbody rigidbody;
    MainScript main;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        main = GameObject.Find("GameController").GetComponent<MainScript>();
        leftPoints = main.routes[routeIndex].leftPoints;
        rightPoints = main.routes[routeIndex].rightPoints;
    }



    public void SteerToNextPoint()
    {
        Vector3 pointPosition;
        if (isInLeftLane) pointPosition = leftPoints[currentPointNumber].position;
        else pointPosition = rightPoints[currentPointNumber].position;
        Seek(pointPosition);
    }



    private void OnDrawGizmos()
    {
        
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

            if (isInLeftLane)
            {
                leftPoints[previousPoint].StopRecording();
                leftPoints[currentPointNumber].StartRecording();
            }
            else
            {
                rightPoints[previousPoint].StopRecording();
                rightPoints[currentPointNumber].StartRecording();
            }
        }
    }
}

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
    public float speed;
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

        if (isInLeftLane) leftPoints[0].StartRecording();
        else rightPoints[0].StartRecording();
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
        Gizmos.DrawWireSphere(transform.position, 1);
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
                if (!leftPoints[currentPointNumber].hasRecorded)
                {
                    leftPoints[previousPoint].StopRecording();
                    leftPoints[currentPointNumber].StartRecording();
                }
                else
                {
                    leftPoints[previousPoint].StopRecording();
                    mainTree.state = CurrentSubtree.DRIVENORMALLY;
                }
            }
            else
            {
                if (!rightPoints[currentPointNumber].hasRecorded)
                {
                    rightPoints[previousPoint].StopRecording();
                    rightPoints[currentPointNumber].StartRecording();
                }
                else
                {
                    rightPoints[previousPoint].StopRecording();
                    mainTree.state = CurrentSubtree.DRIVENORMALLY;
                }
            }
   
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
                counter += leftPoints[futurePoint].timeToNextPoint;

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
                counter += rightPoints[futurePoint].timeToNextPoint;

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



    public void SwitchingLanes()
    {
        if (!isInLeftLane) isInLeftLane = true;
        else isInLeftLane = false;        
    }
}

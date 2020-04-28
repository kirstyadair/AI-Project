using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public bool isInLeftLane = false;
    GameObject currentRoadPiece;
    public List<Transform> leftPoints;
    public List<Transform> rightPoints;
    public int currentPointNumber = 0;
    [SerializeField] float speed;
    Vector3 desiredVelocity;
    Vector3 target;

    public void SteerToNextPoint()
    {
        Vector3 pointPosition;
        if (isInLeftLane) pointPosition = leftPoints[currentPointNumber].position;
        else pointPosition = rightPoints[currentPointNumber].position;
        Seek(pointPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(target, new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.1f, 0.1f));
    }

    public void Seek(Vector3 targetPosition)
    {
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * speed;
        target = targetPosition;
        transform.position += desiredVelocity;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPointNumber++;
            Debug.Log("increased point number to " + currentPointNumber);
        }
    }

    void OnTriggerEnter(Collider collision)
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

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
    Rigidbody rigidbody;



    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }



    public void SteerToNextPoint()
    {
        Vector3 pointPosition;
        if (isInLeftLane) pointPosition = leftPoints[currentPointNumber].position;
        else pointPosition = rightPoints[currentPointNumber].position;
        Seek(pointPosition);
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
            
            currentPointNumber++;
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

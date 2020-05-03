using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarScript : MonoBehaviour
{
    RoadPointsScript[] leftPoints;
    MainScript main;
    [SerializeField] int routeIndex = 0;
    [SerializeField] int currentPointNumber;
    Vector3 desiredVelocity;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("GameController").GetComponent<MainScript>();
        leftPoints = main.routes[routeIndex].leftPoints;
    }

    void Update()
    {
        SteerToNextPoint();
    }

    public void SteerToNextPoint()
    {
        Vector3 pointPosition;
        pointPosition = leftPoints[currentPointNumber].position;
        Seek(pointPosition);
    }

    public void Seek(Vector3 targetPosition)
    {
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(desiredVelocity), 0.05f);
        transform.position += desiredVelocity;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            int previousPoint = currentPointNumber;
            currentPointNumber++;
            if (currentPointNumber >= leftPoints.Length) currentPointNumber = 0;
        }
    }
}

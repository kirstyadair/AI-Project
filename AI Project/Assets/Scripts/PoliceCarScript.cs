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
    public static event SirenSounded OnSirenSounded;
    public delegate void SirenSounded();
    public static event SirenDisabled OnSirenDisabled;
    public delegate void SirenDisabled();
    AudioSource siren;
    bool sirenOn = false;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("GameController").GetComponent<MainScript>();
        leftPoints = main.routes[routeIndex].leftPoints;
        siren = GetComponent<AudioSource>();
    }

    void Update()
    {
        SteerToNextPoint();
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (sirenOn)
            {
                OnSirenDisabled?.Invoke();
                siren.Stop();
                sirenOn = false;
            }
            else
            {
                OnSirenSounded?.Invoke();
                siren.Play();
                sirenOn = true;
            }
        }
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

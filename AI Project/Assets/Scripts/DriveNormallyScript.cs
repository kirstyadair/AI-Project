using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveNormallyScript : MonoBehaviour
{
    [SerializeField] GameObject car;
    GameObject currentRoadPiece;

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road")) currentRoadPiece = collision.gameObject;
    }
}

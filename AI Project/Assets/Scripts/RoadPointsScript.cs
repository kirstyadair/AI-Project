using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPointsScript : MonoBehaviour
{
    public Vector3 position;
    public float timeToNextPoint = 0;



    private void Start()
    {
        position = transform.position;
    }
}

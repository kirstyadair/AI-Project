using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPointsScript : MonoBehaviour
{
    public Vector3 position;
    public float timeToNextPoint = 0;
    bool recording = false;



    private void Start()
    {
        position = transform.position;
    }




    private void Update()
    {
        if (recording) timeToNextPoint += Time.deltaTime;
    }



    public void StartRecording()
    {
        recording = true;
        timeToNextPoint = 0;
    }


    public void StopRecording()
    {
        recording = false;
    }
}

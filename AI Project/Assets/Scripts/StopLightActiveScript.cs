using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLightActiveScript : MonoBehaviour
{
    [SerializeField] bool showDebugs;
    CarScript car;

    private void Start()
    {
        car = GetComponent<CarScript>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveNormallyScript : MonoBehaviour
{
    CarScript car;
    /// <summary>
    /// Stores all cars EXCLUDING this car
    /// </summary>
    List<CarScript> cars = new List<CarScript>();
    
    bool returnToMainTree = false;
    bool switchingLanes = false;



    private void Start()
    {
        car = GetComponent<CarScript>();
        GameObject[] allCars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject car in allCars)
        {
            if (car != this.gameObject)
            {
                cars.Add(car.GetComponent<CarScript>());
            }
        }
    }



    /// <summary>
    /// Call this when switching back to the DriveNormally state
    /// </summary>
    public void ResetTree()
    {
        returnToMainTree = false;
        switchingLanes = false;
        StartTree();
    }



    public State StartTree()
    {
        CheckForCollisions();

        if (!switchingLanes)
        {
            car.SteerToNextPoint();
        }
        else
        {
            car.SwitchingLanes();
            switchingLanes = false;
        }

        if (!returnToMainTree) return State.RUNNING;
        else return State.SUCCESSFUL;
    }



    void CheckForCollisions()
    {
        foreach (CarScript otherCar in cars)
        {
            if ((car.isInLeftLane && otherCar.isInLeftLane) || (!car.isInLeftLane && !otherCar.isInLeftLane))
            {
                if (car.PredictFuturePosition(2) == otherCar.PredictFuturePosition(2) && (car.speed > otherCar.speed))
                {
                    switchingLanes = true;
                }
            }
        }
    }
}

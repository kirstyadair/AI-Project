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
    MainTreeScript mainTree;
    bool returnToMainTree = false;
    bool switchingLanes = false;
    Vector3 pointOfCollision;
    bool pointOfCollisionPassed;



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
        mainTree = GetComponent<MainTreeScript>();
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
        if (mainTree.state != CurrentSubtree.PREPARING) CheckForCollisions();

        if (!switchingLanes)
        {
            car.SteerToNextPoint();
            if (car.isInLeftLane)
            {
                Debug.Log("A");
                if (CheckIfPastPointOfCollision())
                {
                    Debug.Log("C");
                    if (CheckRadius() == State.SUCCESSFUL)
                    {
                        Debug.Log("returning to right lane");
                        car.isInLeftLane = false;
                    }
                }
                
            }
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
                    pointOfCollision = car.PredictFuturePosition(2);
                    pointOfCollisionPassed = false;
                }
            }
        }
    }



    State CheckRadius()
    {
        // Check the radius
        Collider[] collisions = Physics.OverlapSphere(transform.position, 1);
        Debug.Log("D");
        if (collisions.Length <= 1)
        {
            Debug.Log("E");
            return State.SUCCESSFUL;
        }
        else
        {
            foreach (Collider collision in collisions)
            {
                Debug.Log(collision.name);
            }
        }

        return State.RUNNING;
    }



    bool CheckIfPastPointOfCollision()
    {
        if (Vector3.Distance(transform.position, pointOfCollision) < 1f)
        {
            Debug.Log("B");
            return true;
        }
        else return false;
    }
}

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
        // Check future positions for collisions
        CheckForCollisions();

        // If not currently switching to the left lane
        if (!switchingLanes)
        {
            // Steer to the next point in the route
            car.SteerToNextPoint();
            if (car.isInLeftLane)
            {
                // Check if future point of collision has been passed, if one exists
                if (CheckIfPastPointOfCollision())
                {
                    // Check the radius for other cars
                    if (CheckRadius() == State.SUCCESSFUL)
                    {
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
        Collider[] collisions = Physics.OverlapSphere(transform.position, 2);
        if (collisions.Length > 1)
        {
            foreach (Collider collision in collisions)
            {
                CarScript collidingCar = collision.GetComponent<CarScript>();
                if (collision == mainTree.collider) continue;
                if (!collidingCar.isInLeftLane && !car.isInLeftLane)
                {
                    if (collidingCar.rightLaneSpeed < car.rightLaneSpeed) switchingLanes = true;
                }
            }
        }

        foreach (CarScript otherCar in cars)
        {
            if (!car.isInLeftLane && !otherCar.isInLeftLane)
            {
                if (car.PredictFuturePosition(2) == otherCar.PredictFuturePosition(2) && (car.speed > otherCar.speed))
                {
                    switchingLanes = true;
                    pointOfCollision = car.PredictFuturePosition(2);
                    pointOfCollisionPassed = false;
                }
            }
            else if (car.isInLeftLane && otherCar.isInLeftLane)
            {
                if (car.PredictFuturePosition(2) == otherCar.PredictFuturePosition(2) && (car.rightLaneSpeed > otherCar.rightLaneSpeed))
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
        Collider[] collisions = Physics.OverlapSphere(transform.position, 2);
        if (collisions.Length <= 1)
        {
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
        if (pointOfCollision == Vector3.zero) return true;
        if (Vector3.Distance(transform.position, pointOfCollision) < 1f)
        {
            pointOfCollision = Vector3.zero;
            return true;
        }
        else return false;
    }
}

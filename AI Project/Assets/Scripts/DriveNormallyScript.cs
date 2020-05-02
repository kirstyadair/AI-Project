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
                car.speed = car.leftLaneSpeed;
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
        int surroundingsCheck = CheckSurroundings();
        FutureCollision futureCollision = FutureCollisionCheck();

        if (futureCollision.isCollision)
        {
            pointOfCollision = futureCollision.collisionLocation;

            if (surroundingsCheck == 0)
            {
                car.isInLeftLane = true;
            }
            else if (surroundingsCheck == 1)
            {
                car.isInLeftLane = true;
            }
            else if (surroundingsCheck == 2)
            {
                if (car.isInLeftLane) car.speed = futureCollision.carScript.leftLaneSpeed;
                else car.speed = futureCollision.carScript.rightLaneSpeed;
            }
            if (surroundingsCheck == 3)
            {
                if (car.isInLeftLane) car.speed = futureCollision.carScript.leftLaneSpeed;
                else car.speed = futureCollision.carScript.rightLaneSpeed;
            }
        }
    }



    FutureCollision FutureCollisionCheck()
    {
        FutureCollision collision = new FutureCollision();
        collision.isCollision = false;

        foreach (CarScript otherCar in cars)
        {
            if (!car.isInLeftLane && !otherCar.isInLeftLane)
            {
                if (car.PredictFuturePosition(2) == otherCar.PredictFuturePosition(2) && (car.speed > otherCar.speed))
                {
                    collision.isCollision = true;
                    collision.carScript = otherCar;
                    collision.collisionLocation = car.PredictFuturePosition(2);
                }
            }
            else if (car.isInLeftLane && otherCar.isInLeftLane)
            {
                if (car.PredictFuturePosition(2) == otherCar.PredictFuturePosition(2) && (car.rightLaneSpeed > otherCar.rightLaneSpeed))
                {
                    collision.isCollision = true;
                    collision.carScript = otherCar;
                    collision.collisionLocation = car.PredictFuturePosition(2);
                }
            }
        }

        return collision;
    }



    int CheckSurroundings()
    {
        int carsBeside = 0;
        int carsAhead = 0;

        Collider[] collisionsBeside = Physics.OverlapBox(transform.position, new Vector3(2, 0.5f, 0.5f), transform.rotation);
        if (collisionsBeside.Length > 1)
        {
            carsBeside = collisionsBeside.Length - 1;
        }
        Collider[] collisionsAhead = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 2f), transform.rotation);
        if (collisionsAhead.Length > 1)
        {
            carsAhead = collisionsAhead.Length - 1;
        }

        if (carsBeside > 0 && carsAhead > 0) return 3;
        if (carsBeside > 0 && carsAhead == 0) return 2;
        if (carsBeside == 0 && carsAhead > 0) return 1;
        else return 0;
    }



    State CheckRadius()
    {
        // Check the radius
        Collider[] collisions = Physics.OverlapSphere(transform.position, 2);
        if (collisions.Length > 1)
        {
            int importantColliders = collisions.Length;
            foreach (Collider collider in collisions)
            {
                if (collider.GetComponent<CarScript>().isInLeftLane) importantColliders--;
            }

            if (importantColliders > 0) return State.RUNNING;
            else return State.SUCCESSFUL;
        }
        if (collisions.Length <= 1) return State.SUCCESSFUL;

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

public class FutureCollision
{
    public bool isCollision;
    public CarScript carScript;
    public Vector3 collisionLocation;
}


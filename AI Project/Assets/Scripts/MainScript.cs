using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField] Animator panelAnimator;
    [SerializeField] Text buttonText;
    [SerializeField] Text carCount;
    [SerializeField] Text[] carFields;
    public Route[] routes;
    [SerializeField] GameObject aboutPanel;
    List<MainTreeScript> cars = new List<MainTreeScript>();
    List<CarScript> carScripts = new List<CarScript>();
    public List<float> timesToPoint = new List<float>();



    private void Start()
    {
        GameObject[] allCars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject car in allCars)
        {
            cars.Add(car.GetComponent<MainTreeScript>());
            carScripts.Add(car.GetComponent<CarScript>());
        }

        for (int i = 0; i < cars.Count; i++)
        {
            carFields[i].color = cars[i].GetComponentInChildren<MeshRenderer>().materials[0].color;
        }
        carCount.text = "Number of cars: " + cars.Count;
    }



    public void OpenPanel()
    {
        if (panelAnimator.GetFloat("Speed") == 1)
        {
            panelAnimator.SetFloat("Speed", -1);
            buttonText.text = ">";
        }
        else
        {
            panelAnimator.SetFloat("Speed", 1);
            buttonText.text = "<";
        }
    }


    public void ShowAbout()
    {
        if (!aboutPanel.activeInHierarchy)
        {
            aboutPanel.SetActive(true);
        }
        else
        {
            aboutPanel.SetActive(false);
        }
    }



    private void Update()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].state == CurrentSubtree.DRIVENORMALLY) carFields[i].text = "<b>Car " + (i + 1) + ":   <color=white>Driving Normally</color></b>";
            else if (cars[i].state == CurrentSubtree.SIREN_NO_CORRECTIONS) carFields[i].text = "<b>Car " + (i + 1) + ":   <color=brown>Responding To Siren</color></b>";
            else if (cars[i].state == CurrentSubtree.SIREN_WITH_CORRECTIONS) carFields[i].text = "<b>Car " + (i + 1) + ":   <color=red>Responding To Siren + Corrections</color></b>";
            else if (cars[i].state == CurrentSubtree.CORRECTIONS_REQUIRED) carFields[i].text = "<b>Car " + (i + 1) + ":   <color=blue>Correcting</color></b>";
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ActivateRedLight();
        }
    }


    void ActivateRedLight()
    {
        timesToPoint.Clear();

        foreach (CarScript car in carScripts)
        {
            timesToPoint.Add(car.PredictTimeToPoint(19, car.isInLeftLane));
        }
        timesToPoint.Sort();
        Debug.Log(timesToPoint[0]);

        List<CarScript> carsInLeftLane = new List<CarScript>();
        List<CarScript> carsInRightLane = new List<CarScript>();

        for (int i = 0; i < carScripts.Count; i++)
        {
            if (i % 2 == 0)
            {
                carsInRightLane.Add(carScripts[i]);
            }
            else
            {
                carsInLeftLane.Add(carScripts[i]);
            }
        }
    }
}

[Serializable]
public class Route
{
    public RoadPointsScript[] leftPoints;
    public RoadPointsScript[] rightPoints;
}

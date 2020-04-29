﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField] Animator panelAnimator;
    [SerializeField] Text buttonText;
    [SerializeField] Text carCount;
    [SerializeField] Text[] carFields;
    List<MainTreeScript> cars = new List<MainTreeScript>();



    private void Start()
    {
        GameObject[] allCars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject car in allCars)
        {
            cars.Add(car.GetComponent<MainTreeScript>());
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



    private void Update()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].state == CurrentSubtree.DRIVENORMALLY) carFields[i].text = "<b>Car " + (i + 1) + ":   <color=green>Driving Normally</color></b>";
            else if (cars[i].state == CurrentSubtree.SIREN_NO_CORRECTIONS) carFields[i].text = "<b>Car " + (i + 1) + ":   <color=orange>Responding To Siren</color></b>";
            else if (cars[i].state == CurrentSubtree.SIREN_WITH_CORRECTIONS) carFields[i].text = "<b>Car " + (i + 1) + ":   <color=red>Responding To Siren + Corrections</color></b>";
            else if (cars[i].state == CurrentSubtree.CORRECTIONS_REQUIRED) carFields[i].text = "<b>Car " + (i + 1) + ":   <color=blue>Correcting</color></b>";
        }
    }
}
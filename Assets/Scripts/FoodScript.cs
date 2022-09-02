
using System;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [Header("--CanPose--")] 
    public bool canPose;

    [Header("--FoodName--")] 
    public string foodName;

    private GameObject cheese;
    public bool mixingFood;

    public List<Recipe> recipes;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Machine"))
        {
            other.gameObject.GetComponent<Machine>().canUseMachine = true;
            other.gameObject.GetComponent<Machine>().foodObj = this.gameObject;
            other.gameObject.GetComponent<Machine>().foodName = foodName;
            
// Envoie la fonction au script Machine// 
        }

        if (other.CompareTag("PickUp"))
        {
            canPose = false;
            if (other.GetComponent<FoodScript>().foodName == foodName)
            {
                mixingFood = true;
                if (Input.GetMouseButtonDown(0))
                {
                    print("qwack");
                    canPose = true;
                    Instantiate(cheese, other.transform.position, other.transform.rotation);
                    Destroy(other);
                    Destroy(this);
                }
                
            }
        }

        if (other.CompareTag("Table"))
        {
            canPose = true;
        }

    }
    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            canPose = false;
        }
        
        if (other.CompareTag("Machine"))
        {
            other.gameObject.GetComponent<Machine>().canUseMachine = false;
        }

        // if (other.CompareTag("PickUp") || other.GetComponent<FoodScript>().foodName != foodName) // erreur ici
        
        {
           // mixingFood = false;
        }
    }
}

[Serializable] public class Recipe // Class : Stocker des variables
{
    public string foodName;
    public GameObject mixingFood;
}
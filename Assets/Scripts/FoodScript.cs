
using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [Header("--CanPose--")] 
    public bool canPose;

    [Header("--FoodName--")]
    public string foodName;

    public bool canMixingFood;
    public bool checkMixingFood;

    public List<Recipe> recipes;

    private GameObject player;
    private GameObject objMixingFood;

    
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    
    void Update()
    {
        MixingFood();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Machine"))
        {
            other.gameObject.GetComponent<Machine>().canUseMachine = true;
            other.gameObject.GetComponent<Machine>().foodObj = this.gameObject;
            other.gameObject.GetComponent<Machine>().foodName = foodName;
        }
        
        if (other.CompareTag("Table"))
        {
            if (!canMixingFood)
            {
                canPose = true;
            }
        }

        if (other.CompareTag("PickUp")) // Empêche de poser l'object + Vérifie canMixingFood 
        {
            if (player.GetComponent<Player>().heldObj != gameObject) return;
            
            canPose = false;

            if (recipes.Count == 0) return;

            print(other.GetComponent<FoodScript>().foodName + "+" + recipes[0].foodToMixName);
            
            if (string.CompareOrdinal(other.GetComponent<FoodScript>().foodName, recipes[0].foodToMixName) != 0) return;

            print("Can Mixing !");
            canMixingFood = true;
            objMixingFood = other.gameObject;
        }
        if (other.CompareTag("Sender"))
        {
            if (!canMixingFood)
            {
                canPose = true;
                _Manager.instance.objOnSender = gameObject;
            }
        }
    }
    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            if (!canMixingFood) return;
            
            canMixingFood = false;
        }
        
        if (other.CompareTag("Machine"))
            
        {
            other.gameObject.GetComponent<Machine>().canUseMachine = false;
        }

        if (other.CompareTag("PickUp"))
        {
            canMixingFood = false;
        }
    }

    private void MixingFood()
    {
        if (!canMixingFood) return;

        if (!Input.GetMouseButtonDown(0)) return;

        player.gameObject.GetComponent<Player>().canGrab = true;
        player.gameObject.GetComponent<Player>().Feedback();
        Instantiate(recipes[0].mixingFood, objMixingFood.transform.position, objMixingFood.transform.rotation);
        Destroy(objMixingFood.gameObject);
        Destroy(this.gameObject);
    }
}

[Serializable] public class Recipe // Class : Stocker des variables
{
    public string recipeName;
    public string foodToMixName;
    public GameObject mixingFood;
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [HideInInspector] public bool canPose;

    [Header("------FoodName------")]
    public string foodName;

    [Header("--List of possible recipes with this ingredient--")]
    public List<Recipe> recipes;
    
    private bool canMixingFood;
    private bool checkMixingFood;
    private Player player;
    private GameObject objMixingFood;

    
    private void Start()
    {
        player = _Manager.instance.player;
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
            if (player.heldObj != gameObject) return;
            
            canPose = false;

            if (recipes.Count == 0) return;

            if (string.CompareOrdinal(other.GetComponent<FoodScript>().foodName, recipes[0].foodToMixName) != 0) return;

            print("Can Mixing !");
            canMixingFood = true;
            objMixingFood = other.gameObject;
        }
        if (other.CompareTag("Sender"))
        {
            print("sender close");
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
            if (canMixingFood) return;
            
            canMixingFood = false;
            canPose = false;
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
        _Manager.instance.audioSourceEffect.PlayOneShot(_Manager.instance.fbMixing, _Manager.instance.effectVolume); 
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
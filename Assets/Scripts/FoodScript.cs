
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [Header("--CanPose--")] 
    public bool canPose;

    [Header("--FoodName--")] 
    public string foodName;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Machine"))
        {
            other.gameObject.GetComponent<Machine>().canUseMachine = true;
            other.gameObject.GetComponent<Machine>().foodObj = this.gameObject;
            other.gameObject.GetComponent<Machine>().foodName = foodName;

            // other.gameObject.GetComponent<Machine>().CookedMachine(); // Envoie la fonction au script Machine
        }
        if (other.CompareTag("PickUp")) return; // Si il y a un object sur la table ne pose pas l'object

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
    }
}
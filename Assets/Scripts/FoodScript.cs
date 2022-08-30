
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [Header("CanPose")] 
    public bool canPose;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp") || other.CompareTag("Bread") || other.CompareTag("Cheese"))
            return; // Si il y a un object sur la table ne pose pas l'object

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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGiver : MonoBehaviour
{

    public GameObject foodObjToGive;
    public bool canUseFoodGiver;

    private GameObject heldAnchorPlayer;
    private GameObject player;
    
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player");
        heldAnchorPlayer = GameObject.Find("Held_Anchor"); 
    }

    // Update is called once per frame
    private void Update()
    {
       GiveFood();
    }

    private void GiveFood()
    {
        if (!canUseFoodGiver) return;

        if (player.gameObject.GetComponent<Player>().heldObj) return; //si le player tiens un object 
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        var newFoodObj = Instantiate(foodObjToGive, heldAnchorPlayer.transform.position, heldAnchorPlayer.transform.rotation);
        newFoodObj.transform.parent = heldAnchorPlayer.transform;
        player.gameObject.GetComponent<Player>().heldObj = newFoodObj;
        player.gameObject.GetComponent<Player>().canGrab = false;
        player.gameObject.GetComponent<Player>().Feedback();
        canUseFoodGiver = false;
    }
    
}

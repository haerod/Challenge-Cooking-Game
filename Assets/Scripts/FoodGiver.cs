using UnityEngine;

public class FoodGiver : MonoBehaviour
{

    public GameObject foodObjToGive;

    public void GiveFood()
    {
        Player player = _Manager.instance.player;
        Transform heldObject = _Manager.instance.heldAnchor ;
        
        if (player.heldObj) return; //si le player tiens un object 
        
        var newFoodObj = Instantiate(foodObjToGive, heldObject.position, heldObject.rotation);
        newFoodObj.transform.parent = heldObject;
        player.heldObj = newFoodObj;
        player.canGrab = false;
        player.Feedback();
    }
    
}

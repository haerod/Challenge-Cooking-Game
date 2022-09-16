using UnityEngine;

public class FoodGiver : MonoBehaviour
{
    [Header("-----Food Giver parameter-----")]
    [Tooltip("Prefab food to give to the player")]
    public GameObject foodObjToGive;
    [Header("-----Audio Feedback-----")]
    [Tooltip("Audio feedback when the interact with FoodGiver")]
    [SerializeField] private AudioClip audioFoodGiver;

    public void GiveFood()
    {
        Player player = _Manager.instance.player;
        Transform heldObject = _Manager.instance.heldAnchor ;
        
        if (player.heldObj) return; //si le player tiens un object 
        
        var newFoodObj = Instantiate(foodObjToGive, heldObject.position, heldObject.rotation);
        newFoodObj.transform.parent = heldObject;
        player.heldObj = newFoodObj;
        player.canGrab = false;
        _Manager.instance.audioSourceEffect.PlayOneShot(audioFoodGiver, _Manager.instance.effectVolume);
    }
    
}

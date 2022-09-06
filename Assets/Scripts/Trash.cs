using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public bool canUseTrash;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UseTrash();
    }

    void UseTrash()
    {
        if (!canUseTrash) return;

        if (!player.gameObject.GetComponent<Player>().heldObj) return; //si le player tiens un object 
        
        if (!Input.GetMouseButtonDown(0)) return;

        Destroy(player.gameObject.GetComponent<Player>().heldObj);
        player.gameObject.GetComponent<Player>().canGrab = true;
        player.gameObject.GetComponent<Player>().Feedback();
        canUseTrash = false;
    }
}

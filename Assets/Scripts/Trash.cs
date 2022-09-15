using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public void UseTrash()
    {
        Player player = _Manager.instance.player;        
        if (!player.heldObj) return; //si le player tiens un object 

        Destroy(player.heldObj);
        player.canGrab = true;
        _Manager.instance.audioSourceEffect.PlayOneShot(_Manager.instance.fbTrash, _Manager.instance.effectVolume);      
    }
}

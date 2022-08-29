using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
	[SerializeField] private GameObject Player;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private float Smoothness;

    // Update is called once per frame
    private void Update()
    {
	    if (Player == null) return; 
        transform.position = Vector3.Lerp(transform.position, Player.transform.position + Offset, Smoothness * Time.deltaTime); 
    }
}

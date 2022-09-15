using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Player : MonoBehaviour
{
    CharacterController characterController;
    
    private float speed;
    [HideInInspector] public float jumpSpeed = 8.0f;
    [HideInInspector] public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;

    private Transform heldAnchor;
    [HideInInspector] public bool canGrab = true;
    [HideInInspector] public GameObject heldObj;
    private GameObject tagObj;


    [Header("--Physics Parameters--")] 
    [HideInInspector] public List<Collider> col;
    public LayerMask interactibleMask;


    void Start()
    {
        heldAnchor = _Manager.instance.heldAnchor;
        
        characterController = GetComponent<CharacterController>();

        speed = _Manager.instance.playerSpeed;
    }

    void Update()
    {
        Mouvement();
        
        if (!Input.GetMouseButtonDown(0)) return;

        col = Physics.OverlapSphere(heldAnchor.position, .5f, interactibleMask).ToList();
        
        if (col.Count == 0) return;
        
        Interact();
    }

    private void Mouvement()
    {
        if (characterController.isGrounded && (Input.GetAxis("Horizontal")!= 0 || Input.GetAxis("Vertical") != 0))
        {
    
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
           
           float angle = Mathf.Atan2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")) * Mathf.Rad2Deg; // Fait rotate le player
           transform.rotation =Quaternion.Euler(new Vector3(0, angle, 0));
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);    
    }

    private void Interact()
    {
        if (IsContainingInteractible("Trash"))
        {
            InteractionTrash();
        }
        else if (IsContainingInteractible("Machine"))
        {
            InteractionMachineFinish();

        }
        else if (IsContainingInteractible("FoodGiver"))
        {
            InteractionFoodGiver();
        }
        else if (IsContainingInteractible("Sender"))
        {
            InteractionSender();
        }
        if (IsContainingInteractible("PickUp"))
        {
            PickUp();
        }
    }
    private void PickUp()
    {
        
        if (!heldObj) //si il est null
        {

            heldObj = tagObj;
            heldObj.transform.position = heldAnchor.position;
            heldObj.transform.parent = heldAnchor;
            canGrab = false;
            Feedback();

        }
        else // Lache l'object 
        {
            if (!heldObj.GetComponent<FoodScript>().canPose) return;
            
            heldObj.transform.parent = null;
            heldObj = null;
            canGrab = true;
            Feedback();
        }
    }
    private void InteractionTrash()
    {
            tagObj.GetComponent<Trash>().UseTrash();
    }

    private void InteractionFoodGiver()
    {
            tagObj.GetComponent<FoodGiver>().GiveFood();
    }

    private void InteractionMachineFinish()
    {
           tagObj.GetComponent<Machine>().BakeFinished();
    }
    
    private void InteractionSender()
    {

        _Manager.instance.UseSender();
    }
    
    public void Feedback()
    {

        if (canGrab)
        {
            _Manager.instance.audioSourceEffect.PlayOneShot(_Manager.instance.fbPose, _Manager.instance.effectVolume); 
        }
        else
        {
            _Manager.instance.audioSourceEffect.PlayOneShot(_Manager.instance.fbGrab, _Manager.instance.effectVolume); 
        }
    }

    private bool IsContainingInteractible(string tag)
    {
        foreach (Collider c  in col)
        {
            if (c.CompareTag(tag))
            {
                tagObj = c.gameObject; 
                return true;
            }
        }
        return false;
    }
    void OnDrawGizmos() 
    {
        
        if (!heldAnchor) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (heldAnchor.position, .5f);
    }
}
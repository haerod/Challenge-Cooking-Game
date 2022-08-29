using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    [SerializeField] private GameObject _3DObj;

    private Vector3 moveDirection = Vector3.zero;

    [Header("Pickup Settings")] 
    [SerializeField] private Transform heldAnchor;
    public bool canGrab;
    [SerializeField] private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")] 
    [SerializeField] private float pickupRange = 5.0f;
    private RaycastHit hit;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Mouvement();

        PickUp();
    }

    private void Mouvement()
    {
        if (characterController.isGrounded)
        {
    
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
            
           
           float angle = Mathf.Atan2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
           if (angle != 0)
           {
               _3DObj.transform.rotation =Quaternion.Euler(new Vector3(0, angle, 0));
           }
            
            if (Input.GetButton("Jump"))
            {
                // si on veut un jump
                //moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);    
    }

    private void PickUp()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                if (Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange))
                {
                    if (hit.transform.CompareTag("PickUp"))
                    {
                        canGrab = false;
                        heldObj = hit.transform.gameObject;
                        heldObj.transform.position = heldAnchor.position;
                        heldObj.transform.parent = heldAnchor;
                        
                    }
                }
            }
            else
            {
                heldObj.transform.parent = null;
                heldObj = null;
                canGrab = true;
            }
        }
    }
    void OnDrawGizmos() 
    {
        //Gizmos Forward
        Gizmos.color = Color.green;
        if (Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange) && hit.transform.CompareTag("PickUp") )
        {
            Gizmos.color = new Color(r: 1f, g: 0.07f, b: 0f); //red  
        }
        Gizmos.DrawLine(transform.position, heldAnchor.transform.position);
    }
}
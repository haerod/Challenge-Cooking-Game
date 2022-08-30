using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("--Player Stats--")]
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    [SerializeField] private GameObject _3DObj;

    private Vector3 moveDirection = Vector3.zero;

    [Header("--Pickup Settings--")] 
    [SerializeField] private Transform heldAnchor;
    public bool canGrab = true;
    [SerializeField] private GameObject heldObj;


    [Header("--Physics Parameters--")] 
    [SerializeField] private float pickupRange = 2.0f;
    private RaycastHit hit;
    
    

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        Feedback();
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
            
           
           float angle = Mathf.Atan2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")) * Mathf.Rad2Deg; // Fait rotate le player
           if (angle != 0) // Si y'a pas ça, le joueur retourne tout droit quand on touche plus les touches
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
                    if (hit.transform.CompareTag("PickUp")) // Attrape l'object
                    {
                        canGrab = false;
                        heldObj = hit.transform.gameObject;
                        heldObj.transform.position = heldAnchor.position;
                        heldObj.transform.parent = heldAnchor;
                        Feedback();
                        
                    }
                }
            }
            else // Lache l'object 
            {
                if (heldObj.GetComponent<FoodScript>().canPose)
                {
                    heldObj.transform.parent = null;
                    heldObj = null;
                    canGrab = true;
                    Feedback();
                }
            }
        }
    }

    private void Feedback()
    {
        GameObject ArmL = GameObject.Find("Arm_L");
        GameObject ArmR = GameObject.Find("Arm_R");
        GameObject FeedbackArmL = GameObject.Find("Feedback_Arm_R");
        GameObject FeedbackArmR = GameObject.Find("Feedback_Arm_L");

        if (canGrab)
        {
            ArmL.transform.localScale =  new Vector3(0.2f, 0.6f, 0.2f);
            ArmR.transform.localScale =  new Vector3(0.2f, 0.6f, 0.2f);
            FeedbackArmL.transform.localScale = new Vector3(0f, 0f, 0f);
            FeedbackArmR.transform.localScale = new Vector3(0f, 0f, 0f);
            
        }
        if (canGrab == false )
        {
            ArmL.transform.localScale =  new Vector3(0f, 0f, 0f);
            ArmR.transform.localScale =  new Vector3(0f, 0f, 0f);
            FeedbackArmL.transform.localScale =  new Vector3(0.2f, 0.1f, 1.5f);
            FeedbackArmR.transform.localScale =  new Vector3(0.2f, 0.1f, 1.5f);
        }
    }
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        if (Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange) && hit.transform.CompareTag("PickUp")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 1f, g: 0.07f, b: 0f); //red  
        }
        Gizmos.DrawLine(transform.position, heldAnchor.transform.position);
    }
}
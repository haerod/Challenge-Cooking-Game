using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController characterController;
    
    [Header("--Player Stats--")]
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    [SerializeField] private GameObject _3DObj;
    [SerializeField] private GameObject manager;

    private Vector3 moveDirection = Vector3.zero;

    [Header("--Interaction Settings--")] 
    private Transform heldAnchor;
    public bool canGrab = true;
    public GameObject heldObj;
    private GameObject senderInFront;


    [Header("--Physics Parameters--")] 
    [SerializeField] private float pickupRange = 2.0f;
    private RaycastHit hit;
    
    

    void Start()
    {
        heldAnchor = _Manager.instance.heldAnchor;
        
        characterController = GetComponent<CharacterController>();
        
        Feedback();
    }

    void Update()
    {
        Mouvement();
        
        if (!Physics.BoxCast(heldAnchor.position, Vector3.one/2, transform.forward,out hit, Quaternion.identity, pickupRange)) return;

        if (!Input.GetMouseButtonDown(0)) return;

        PickUp();

        InteractionMachineFinish();

        InteractionFoodGiver();
        
        InteractionTrash();

        InteractionSender();
    }

    private void Mouvement()
    {
        if (characterController.isGrounded && (Input.GetAxis("Horizontal")!= 0 || Input.GetAxis("Vertical") != 0))
        {
    
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
           
           float angle = Mathf.Atan2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")) * Mathf.Rad2Deg; // Fait rotate le player
           _3DObj.transform.rotation =Quaternion.Euler(new Vector3(0, angle, 0));
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);    
    }

    private void PickUp()
    {
        
        if (!heldObj) //si il est null
        {

            if (!hit.transform.CompareTag("PickUp")) return;
            
            heldObj = hit.transform.gameObject;
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
            if (!hit.transform.CompareTag("Trash")) return;
            
            hit.transform.gameObject.GetComponent<Trash>().UseTrash();
    }

    private void InteractionFoodGiver()
    {
            if (!hit.transform.CompareTag("FoodGiver")) return;

            hit.transform.GetComponent<FoodGiver>().GiveFood();

    }

    private void InteractionMachineFinish()
    {
            if (!hit.transform.CompareTag("Machine")) return;
            
            hit.transform.GetComponent<Machine>().BakeFinished();
    }
    
    private void InteractionSender()
    {
        print(hit.transform.name);
        if (!hit.transform.CompareTag("Sender")) return;
        print("Etape 0");

        manager.GetComponent<_Manager>().UseSender();
    }
    
    public void Feedback()
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
        else
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
        
        if (!heldAnchor) return;
        if (Physics.Raycast(transform.position, heldAnchor.position, out hit, pickupRange) && hit.transform.CompareTag("PickUp")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 1f, g: 0f, b: 0f); //Rouge = Food (PickUp) 
        }
        if (Physics.Raycast(transform.position, heldAnchor.position, out hit, pickupRange) && hit.transform.CompareTag("Machine")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 0f, g: 0.00f, b: 1f); //bleu = Machine  
        }
        if (Physics.Raycast(transform.position, heldAnchor.position, out hit, pickupRange) && hit.transform.CompareTag("FoodGiver")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 0.5f, g: 0.3f, b: 1f); //Violet = FoodGiver 
        }
        if (Physics.Raycast(transform.position, heldAnchor.position, out hit, pickupRange) && hit.transform.CompareTag("Trash")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 1f, g: 1f, b: 0f); //jaune = Trash
        }
        if (Physics.Raycast(transform.position, heldAnchor.position, out hit, pickupRange) && hit.transform.CompareTag("Sender")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = Color.magenta; //magenta = sender 
        }
        Gizmos.DrawLine(transform.position, heldAnchor.position);
    }
}
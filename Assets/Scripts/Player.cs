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
    public GameObject heldObj;
    private GameObject machineInFront;
    private GameObject foodGiverInFront;
    private GameObject trashInFront;


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

        InteractionMachineFinish();

        InteractionFoodGiver();
        
        InteractionTrash();
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
        if (!Input.GetMouseButtonDown(0)) return; 
        
        if (heldObj == null)
        {
            if (!Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange)) return;

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
        if (!trashInFront)
        {
            if (!Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange)) return;

            if (!hit.transform.CompareTag("Trash")) return;
            trashInFront = hit.transform.gameObject;
            trashInFront.GetComponent<Trash>().canUseTrash = true;
        }
        else
        {
            if (hit.transform.CompareTag("Trash")) return;
            trashInFront.GetComponent<Trash>().canUseTrash = false;
            trashInFront = null;
        }
    }

    private void InteractionFoodGiver()
    {
        if (!foodGiverInFront)
        {
            if (!Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange)) return;

            if (!hit.transform.CompareTag("FoodGiver")) return;
            foodGiverInFront = hit.transform.gameObject;
            foodGiverInFront.GetComponent<FoodGiver>().canUseFoodGiver = true;
        }
        else
        {
            if (hit.transform.CompareTag("FoodGiver")) return;
            foodGiverInFront.GetComponent<FoodGiver>().canUseFoodGiver = false;
            foodGiverInFront = null;
        }  
    }

    private void InteractionMachineFinish()
    {
        if (machineInFront == null)
        {
            if (!Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange)) return;

            if (!hit.transform.CompareTag("Machine")) return;
            machineInFront = hit.transform.gameObject;
            machineInFront.GetComponent<Machine>().canGetFood = true;
        }
        else
        {
            if (hit.transform.CompareTag("Machine")) return;
            machineInFront.GetComponent<Machine>().canGetFood = false;
            machineInFront = null;
        }  
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
        if (Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange) && hit.transform.CompareTag("PickUp")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 1f, g: 0.07f, b: 0f); //Rouge = Food (PickUp) 
        }
        if (Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange) && hit.transform.CompareTag("Machine")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 0f, g: 0.00f, b: 1f); //bleu = Machine  
        }
        if (Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange) && hit.transform.CompareTag("FoodGiver")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 0.5f, g: 0.3f, b: 1f); //Violet = FoodGiver 
        }
        if (Physics.Raycast(transform.position, heldAnchor.transform.position, out hit, pickupRange) && hit.transform.CompareTag("Trash")) // Seulement si on peut attraper l'object
        {
            Gizmos.color = new Color(r: 1f, g: 1f, b: 0f); //jaune = Trash
        }
        Gizmos.DrawLine(transform.position, heldAnchor.transform.position);
    }
}
using UnityEngine;

public class Machine : MonoBehaviour
{
    [Header("Machine Information")]
    [SerializeField] private string foodToBakeName;
    [SerializeField] private GameObject foodBaked;
    public float timeCooking;
    
    [Header("Verification")]
    public bool cookingFinish;
    public bool canUseMachine;
    public string foodName;
    public GameObject foodObj;
    private GameObject heldAnchor; // Ancre du Player
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        heldAnchor = GameObject.Find("Held_Anchor");
    }

    // Update is called once per frame
    void Update()
    {
        CookedMachine();
        
    }
    public void CookedMachine()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (!canUseMachine) return;
        if (foodName == foodToBakeName)
        {
            Destroy(foodObj);
            var newBakedObj = Instantiate(foodBaked, heldAnchor.transform.position, heldAnchor.transform.rotation);
            newBakedObj.transform.parent = heldAnchor.transform;
            player.gameObject.GetComponent<Player>().heldObj = newBakedObj;
        }
        
        
    }
}

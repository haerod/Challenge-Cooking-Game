using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    [Header("--Machine Information--")]
    [SerializeField] private string foodToBakeName;
    [SerializeField] private GameObject foodBaked;
    public float timeBaked;
    
    // Verification Var // 
    private bool canBake;
    private bool bakeFinished;
    [HideInInspector] public bool canUseMachine;
    [HideInInspector] public bool canGetFood;
    [HideInInspector] public string foodName;
    [HideInInspector] public GameObject foodObj;
    private GameObject heldAnchor; // Ancre du Player
    private Player player;
    
    //Baked Start
    private float currentTime = 0f;
    //Baked Finished
    private bool foodIsCreated;

    [Header("-----UI-----")] 
    [SerializeField] private Image TimeFill;
    [SerializeField] private GameObject timeBarUI;
    [SerializeField] private GameObject exclamationUI;
    
    void Start()
    {
        player = _Manager.instance.player;
        heldAnchor = GameObject.Find("Held_Anchor");
        timeBarUI.SetActive(false);
        exclamationUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        BakedMachine();
        BakedStart();
    }

    private void BakedMachine()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (!canUseMachine) return;
        if (foodName != foodToBakeName) return;
        
        Destroy(foodObj);
        canBake = true;
        foodIsCreated = false;
        canUseMachine = false;
        player.canGrab = true;
        player.Feedback();
        timeBarUI.SetActive(true);
    }

    void BakedStart()
    {
        if (!canBake) return; 
        
        currentTime += Time.deltaTime;
        TimeFill.fillAmount = currentTime / timeBaked;
        
        if (currentTime < timeBaked) return;

        bakeFinished = true;
        canBake = false;
        timeBarUI.SetActive(false);
        exclamationUI.SetActive(true);
        currentTime = 0;


    }
    public void BakeFinished()
    {
        if (!bakeFinished) return;
        
        if (canUseMachine) return; // Seulement si on à les mains vides

        if (foodIsCreated) return; // Passe seulement si l'objet n'a pas été créer

        foodIsCreated = true;
        var newBakedObj = Instantiate(foodBaked, heldAnchor.transform.position, heldAnchor.transform.rotation);
        newBakedObj.transform.parent = heldAnchor.transform;
        player.heldObj = newBakedObj;
        player.canGrab = false;
        player.Feedback();
        exclamationUI.SetActive(false);
        currentTime = 0;
        bakeFinished = false;
    }
}

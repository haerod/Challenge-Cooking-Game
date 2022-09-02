using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    [Header("--Machine Information--")]
    [SerializeField] private string foodToBakeName;
    [SerializeField] private GameObject foodBaked;
    public float timeBaked;
    
    [Header("-------------------Scripts-------------------")] // Verification Var // 
    private bool canBake;
    private bool bakeFinished;
    public bool canUseMachine;
    public bool canGetFood;
    [HideInInspector] public string foodName;
    [HideInInspector] public GameObject foodObj;
    private GameObject heldAnchor; // Ancre du Player
    private GameObject player;
    
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
        player = GameObject.Find("Player");
        heldAnchor = GameObject.Find("Held_Anchor");
        timeBarUI.SetActive(false);
        exclamationUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        BakedMachine();
        BakedStart();
        BakeFinished();
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
        player.gameObject.GetComponent<Player>().canGrab = true;
        player.gameObject.GetComponent<Player>().Feedback();
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
    // A corriger : Faire instantiate une seule fois, pouvoir re-intéragir avec la machine seulement si on est proche, enlever & activer la barre de temps
    void BakeFinished()
    {
        if (!bakeFinished) return;
        
        if (canUseMachine) return; // Seulement si on à les mains vides

        if (foodIsCreated) return; // Passe seulement si l'objet n'a pas été créer

        if (!canGetFood) return;
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        
        foodIsCreated = true;
        var newBakedObj = Instantiate(foodBaked, heldAnchor.transform.position, heldAnchor.transform.rotation);
        newBakedObj.transform.parent = heldAnchor.transform;
        player.gameObject.GetComponent<Player>().heldObj = newBakedObj;
        player.gameObject.GetComponent<Player>().canGrab = false;
        player.gameObject.GetComponent<Player>().Feedback();
        exclamationUI.SetActive(false);
        currentTime = 0;
        bakeFinished = false;
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    [Header("--Machine Information--")]
    [Tooltip("Exact name of the food that can be cooked by the machine")]
    [SerializeField] private string foodToBakeName;
    [Tooltip("Prefab of the cooked food")]
    [SerializeField] private GameObject foodBaked;
    [Tooltip("Time in Secondes")]
    public float timeBaked;

    
    [Header("-----Audio Feedback-----")]
    [Tooltip("Audio feedback when the machine has finished cooking")]
    [SerializeField] private AudioClip audioMachineOver;
    
    [Header("-----Don't Touch-----")] 
    [SerializeField] private Image TimeFill;
    [SerializeField] private GameObject timeBarUI;
    [SerializeField] private GameObject exclamationUI;
        
    // Verification Var // 
    private bool canBake;
    private bool bakeFinished;
    [HideInInspector] public bool canUseMachine;
    [HideInInspector] public bool canGetFood;
    [HideInInspector] public string foodName;
    [HideInInspector] public GameObject foodObj;
    private Transform heldAnchor; // Ancre du Player
    private Player player;
    
    //Baked Start
    private float currentTime = 0f;
    //Baked Finished
    private bool foodIsCreated;
    
    void Start()
    {
        player = _Manager.instance.player;
        heldAnchor = _Manager.instance.heldAnchor;
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
        _Manager.instance.audioSourceEffect.PlayOneShot(audioMachineOver, _Manager.instance.effectVolume);


    }
    public void BakeFinished()
    {
        if (!bakeFinished) return;
        
        if (canUseMachine) return; // Seulement si on à les mains vides

        if (foodIsCreated) return; // Passe seulement si l'objet n'a pas été créer

        foodIsCreated = true;
        var newBakedObj = Instantiate(foodBaked, heldAnchor.position, heldAnchor.rotation);
        newBakedObj.transform.parent = heldAnchor;
        player.heldObj = newBakedObj;
        player.canGrab = false;
        player.Feedback();
        exclamationUI.SetActive(false);
        currentTime = 0;
        bakeFinished = false;
    }
}

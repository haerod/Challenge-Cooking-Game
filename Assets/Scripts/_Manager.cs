using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _Manager : MonoBehaviour
{
    public static _Manager instance;
    
    [Header("-----Player Stat----")] 
    [Tooltip("6 is the default speed")]
    [Range(0, 12)]
    public float playerSpeed = 6.0f;
    
    [Header("--Timer Setting--")]
    [Tooltip("Time in secondes")]
    public float timeRemaining = 10;
    
    [Header("-----UI-----")]
    [Tooltip("Default screen image after a plates is completed")]
    public Sprite defaultImg;

    [Header("-----Audio----")] 

    public AudioClip musicInGame;
    public AudioClip fbGrab;
    public AudioClip fbPose;
    public AudioClip fbMixing;
    public AudioClip fbTrash;
    public AudioClip fbSenderFalse;
    public AudioClip fbSenderTrue;
    public AudioClip fbWin;
    public AudioClip fbLoose;
    
    [Header("-----Audio Volume----")] 
    [Range(0, 1)]
    public float effectVolume = 0.5f;
    [Range(0, 1)]
    public float musicVolume = 0.5f;
    
    [Header("-----Lists of all the plates to win----")] 
    public List<ListPlat> listOfPlats;
    
    [Header("-----Don't Touch----")] 
    [Tooltip("Game Object where the text is displayed")]
    public Text timeText;
    [HideInInspector] 
    public GameObject objOnSender;
    public Player player;
    public Transform heldAnchor;
    public Animator blackScreen;
    public AudioSource audioSourceEffect;
    public AudioSource audioSourceMusic;
    private bool win;
    private bool loose;
    private bool timerIsRunning = false;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Deux versions de _Manager, en supprimer une", gameObject);
        }
    }

    private void Start()
    {
        timerIsRunning = true;
        
        SetUpUI();
        
        Cursor.visible = false;
        
        audioSourceMusic.PlayOneShot(musicInGame, musicVolume);
    }
    
    void Update()
    {
        TimerRunning();
        
        LooseBackMenu();
        WinBackMenu();
    }

    void TimerRunning()
    {
        if (!timerIsRunning) return;
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            audioSourceEffect.PlayOneShot(fbLoose, effectVolume); 

            loose = true;
            timeRemaining = 0;
            timerIsRunning = false;
            blackScreen.SetBool("looseAnim", true);
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    public void UseSender()
    {
        print("Etape1");
        if (!player.heldObj) return; //si le player tiens un object 
        
        player.canGrab = true;
        
        foreach (var allFoodName in listOfPlats) // Verification de si l'objet posés est l'un de ceux dans la listes des plats
        {
            Debug.Log(allFoodName.foodName + " & " + objOnSender.GetComponent<FoodScript>().foodName);
            if (allFoodName.foodName == objOnSender.GetComponent<FoodScript>().foodName)
            {
                print("Food Send !");
                Destroy(objOnSender);
                allFoodName.foodImgUI.transform.GetChild(0).GetComponent<Image>().sprite = defaultImg;
                listOfPlats.Remove(allFoodName);
                SendPlat();
            }
        }
        audioSourceEffect.PlayOneShot(fbSenderFalse, effectVolume);
        Destroy(objOnSender);
    }

    void SetUpUI()
    {
        if (listOfPlats.Count == 0) // Minimum 1 Plat
        {
            Debug.LogError("Il faut au minimum <color=#FF0000><b>un plat</b></color> pour lancer le jeu", gameObject);
            return;
        }
        
        foreach (ListPlat currentListOfPlats in listOfPlats) // SetUP des Images & verifs aucun paramètres est vide
        {
            if (string.IsNullOrEmpty(currentListOfPlats.foodName) || !currentListOfPlats.foodSprite || !currentListOfPlats.foodImgUI)
            {
                ErrorEmptyCase();
                return;

            }
            currentListOfPlats.foodImgUI.transform.GetChild(0).GetComponent<Image>().sprite = currentListOfPlats.foodSprite;
        }
    }

    void ErrorEmptyCase()
    {
        Debug.LogError("La case <color=#FF0000><b>food Name </b></color>, <color=#FF0000><b>food sprite </b></color> ou <color=#FF0000><b>food Img UI </b></color>de l'object <b>_Manager</b> est vide, cela ne peut fonctionner que si ces trois conditions sont rempli", gameObject);
    }
    
    void SendPlat()
    {
        if (listOfPlats.Count != 0)
        {
            audioSourceEffect.PlayOneShot(fbSenderTrue, effectVolume); 
        }
        if (listOfPlats.Count != 0) return;
        
        Debug.Log("GG !");
        timerIsRunning = false;
        win = true;
        audioSourceEffect.PlayOneShot(fbWin, effectVolume); 
        blackScreen.SetBool("winAnim", true);
    }

    void WinBackMenu()
    {
        if (!win) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            blackScreen.SetBool("winAnimOver", true);
            audioSourceEffect.PlayOneShot(fbWin, effectVolume); 
            if (blackScreen.GetCurrentAnimatorStateInfo(0).IsName("Black_Screen_Win_Over"))
            {
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    void LooseBackMenu()
    {
        if (!loose) return;
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            blackScreen.SetBool("looseAnimOver", true);
            if (blackScreen.GetCurrentAnimatorStateInfo(0).IsName("Black_Screen_Win_Over"))
            {
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}



[Serializable] public class ListPlat // Class : Stocker des variables
{
    public string foodName;
    public Sprite foodSprite;
    public Canvas foodImgUI;
}
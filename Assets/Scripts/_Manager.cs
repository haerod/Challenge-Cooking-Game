using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class _Manager : MonoBehaviour
{
    public static _Manager instance;
    
    [Header("--Timer Setting--")]
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;

    [Header("--Sender Setting--")] 

    public GameObject imgPlat1;
    public GameObject imgPlat2;
    public GameObject imgPlat3;
    public GameObject imgPlat4;
    public GameObject imgPlat5;

    private int numberOfPlatLeft;
    public GameObject objOnSender;

    public Player player;
    public Transform heldAnchor;
    
    public List<ListPlat> listOfPlats;

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
    }
    
    void Update()
    {
        TimerRunning();
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
            Debug.Log("Event Loose");
            timeRemaining = 0;
            timerIsRunning = false;
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
        player.Feedback();
        Debug.Log(listOfPlats.First().foodName);
        Debug.Log(objOnSender.GetComponent<FoodScript>().foodName);

        if (listOfPlats.First().foodName == objOnSender.GetComponent<FoodScript>().foodName)
        {
            print("GG");
        }
    }

    void SetUpUI()
    {

        if (listOfPlats.Count == 0) // Minimum 1 Plat
        {
            Debug.LogError("Il faut au minimum <color=#FF0000><b>un plat</b></color> pour lancer le jeu", gameObject);
            return;
        }
        if (listOfPlats.Count > 5) // Maximum 5 Plat
        {
            Debug.LogError("Vous avez atteint le maximum de <color=#FF0000><b>5 plats</b></color>. Supprimer les plats de trop pour continuer", gameObject);
            return;
        }
        
        if (listOfPlats.Count == 1)
        {
            if (string.IsNullOrEmpty(listOfPlats[0].foodName) || listOfPlats[0].foodSprite == null)
            {
                ErrorEmptyCase();
                return;
            }
            imgPlat1.SetActive(true);
            imgPlat1.GetComponent<Image>().sprite = listOfPlats[0].foodSprite;
            
            imgPlat2.SetActive(false);
            imgPlat3.SetActive(false);
            imgPlat4.SetActive(false);
            imgPlat5.SetActive(false);

            numberOfPlatLeft = 1;
        }

        if (listOfPlats.Count == 2)
        {
            if (string.IsNullOrEmpty(listOfPlats[1].foodName) || listOfPlats[1].foodSprite == null)
            {
                ErrorEmptyCase();
                return;
            }
            imgPlat1.SetActive(true);
            imgPlat1.GetComponent<Image>().sprite = listOfPlats[0].foodSprite;
            imgPlat2.SetActive(true);
            imgPlat2.GetComponent<Image>().sprite = listOfPlats[1].foodSprite;
            
            imgPlat3.SetActive(false);
            imgPlat4.SetActive(false);
            imgPlat5.SetActive(false);
            
            numberOfPlatLeft = 2;
        }
        
        if (listOfPlats.Count == 3)
        {
            if (string.IsNullOrEmpty(listOfPlats[2].foodName) || listOfPlats[2].foodSprite == null)
            {
                ErrorEmptyCase();
                return;
            }
            imgPlat1.SetActive(true);
            imgPlat1.GetComponent<Image>().sprite = listOfPlats[0].foodSprite;
            imgPlat2.SetActive(true);
            imgPlat2.GetComponent<Image>().sprite = listOfPlats[1].foodSprite;
            imgPlat3.SetActive(true);
            imgPlat3.GetComponent<Image>().sprite = listOfPlats[2].foodSprite;
            
            imgPlat4.SetActive(false);
            imgPlat5.SetActive(false);
            
            numberOfPlatLeft = 3;
        }
        
        if (listOfPlats.Count == 4)
        {
            if (string.IsNullOrEmpty(listOfPlats[3].foodName) || listOfPlats[3].foodSprite == null)
            {
                ErrorEmptyCase();
                return;
            }
            imgPlat1.SetActive(true);
            imgPlat1.GetComponent<Image>().sprite = listOfPlats[0].foodSprite;
            imgPlat2.SetActive(true);
            imgPlat2.GetComponent<Image>().sprite = listOfPlats[1].foodSprite;
            imgPlat3.SetActive(true);
            imgPlat3.GetComponent<Image>().sprite = listOfPlats[2].foodSprite;
            imgPlat4.SetActive(true);
            imgPlat4.GetComponent<Image>().sprite = listOfPlats[3].foodSprite;
            
            imgPlat5.SetActive(false); 
            numberOfPlatLeft = 4;
        }
        
        if (listOfPlats.Count == 5)
        {
            if (string.IsNullOrEmpty(listOfPlats[4].foodName) || listOfPlats[4].foodSprite == null)
            {
                ErrorEmptyCase();
                return;
            }
            imgPlat1.SetActive(true);
            imgPlat1.GetComponent<Image>().sprite = listOfPlats[0].foodSprite;
            imgPlat2.SetActive(true);
            imgPlat2.GetComponent<Image>().sprite = listOfPlats[1].foodSprite;
            imgPlat3.SetActive(true);
            imgPlat3.GetComponent<Image>().sprite = listOfPlats[2].foodSprite;
            imgPlat4.SetActive(true);
            imgPlat4.GetComponent<Image>().sprite = listOfPlats[3].foodSprite;
            imgPlat5.SetActive(true);
            imgPlat5.GetComponent<Image>().sprite = listOfPlats[3].foodSprite;
            
            numberOfPlatLeft = 5;
        }
        
    }

    void ErrorEmptyCase()
    {
        Debug.LogError("La case <color=#FF0000><b>food Name </b></color> ou <color=#FF0000><b>food sprite </b></color> de l'object <b>_Manager</b> est vide, cela ne peut fonctionner que si ces deux conditions sont rempli", gameObject);
    }
}

[Serializable] public class ListPlat // Class : Stocker des variables
{
    public string foodName;
    public Sprite foodSprite;
}
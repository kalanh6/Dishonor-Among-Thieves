using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerWhoWon;
    public bool playerWon;

    Button myRestartButton;
    Button myMainMenuButton;

    public int[] points = new int[4];

    GameObject[] taggedObjects = new GameObject[4];
    TextMeshProUGUI[] textsForTagged = new TextMeshProUGUI[4];

    GameObject myCanvas;
    RectTransform canvasRect;

    [SerializeField]
    float startPosition = -666;

    [SerializeField]
    float difference = 446;

    [SerializeField]
    float height = 104;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //playerWhoWon = Instance.playerWhoWon;
            //playerWon = Instance.playerWon;
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "WinLoseMenu")
        {
            FindTextInCurrentScene();
        }
        else if (SceneManager.GetActiveScene().name == "Main_Menu")
        {
            FindTextInCurrentScene();
        }
        else if (SceneManager.GetActiveScene().name == "EndlessModeEnd")
        {
            taggedObjects = GameObject.FindGameObjectsWithTag("Leaderboard");
            myCanvas = GameObject.FindGameObjectWithTag("Canvas");
            canvasRect = myCanvas.GetComponent<RectTransform>();

            for (int i = 0; i < taggedObjects.Length; i++)
            {
                textsForTagged[i] = taggedObjects[i].GetComponent<TextMeshProUGUI>();
            }
            for (int i = 0; i < textsForTagged.Length; i++)
            {




                textsForTagged[i].text = points[i].ToString();

                
                //// 1. Convert the 2D world position to a pixel screen position
                Vector2 newPosition = new Vector2((canvasRect.position.x + -666) + (i * 446), canvasRect.position.y);
                taggedObjects[i].GetComponent<RectTransform>().position = newPosition;
                //Vector2 screenPosition = Camera.main.WorldToScreenPoint(taggedObjects[i].transform.position);

                //// 2. Convert the screen position into a local point inside the Canvas RectTransform
                //Vector2 localPoint;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(
                //    ,
                //    screenPosition,
                //    Camera.main,
                //    out localPoint
                //);


                
                
                //taggedObjects[i].GetComponent<RectTransform>().position = localPoint;
                
                Debug.Log("Player " + i + " has " + points[i] + " points");
            }
        }
    }

    //private void Update()
    //{
    //    for (int i = 0; i < taggedObjects.Length; i++)
    //    {
    //        Vector2 newPosition = new Vector2((canvasRect.position.x + startPosition) + (i * difference), canvasRect.position.y);
    //        taggedObjects[i].GetComponent<RectTransform>().position = newPosition;
    //    }
    //}
    private void FindTextInCurrentScene()
    {
        if (GameObject.FindGameObjectWithTag("MainMenuButton") != null)
        {
            myMainMenuButton = GameObject.FindGameObjectWithTag("MainMenuButton").GetComponent<Button>();
        }
        myRestartButton = GameObject.FindGameObjectWithTag("RestartButton").GetComponent<Button>();
        

        if (myRestartButton != null)
        {
            // 3. Hook up your custom method to the onClick event
            myRestartButton.onClick.AddListener(Restart);
        }
        if (myMainMenuButton != null)
        {
            // 3. Hook up your custom method to the onClick event
            myMainMenuButton.onClick.AddListener(BackToMainMenu);
        }

    }

    //public void WhoWon()
    //{
    //    if (playerWon)
    //    {
    //        myImage.sprite = sprite[playerWhoWon];
    //    }
    //    else
    //    {
    //        myImage.sprite = sprite[4];
    //    }
    //}

    public void PressStart()
    {
        SceneManager.LoadScene("Game");
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void AddPoints( int pointsAdded, int whichPlayer)
    {
        points[whichPlayer] += pointsAdded;
        Debug.Log("Player " + whichPlayer + " got points");
    }
}

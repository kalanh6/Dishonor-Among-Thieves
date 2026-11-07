using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerWhoWon;
    public bool playerWon;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            playerWhoWon = Instance.playerWhoWon;
            playerWon = Instance.playerWon;
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}
    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (SceneManager.GetActiveScene().name == "WinLoseMenu")
    //    {
    //        FindTextInCurrentScene();
    //    }
    //}

    //private void FindTextInCurrentScene()
    //{
    //    GameObject textObj = GameObject.FindGameObjectWithTag("GameManagerText");
    //    if (textObj != null)
    //    {
    //        myImage = textObj.GetComponent<Image>();
    //    }
    //    //Debug.Log("hi");

    //    //WhoWon();
    //}

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
}

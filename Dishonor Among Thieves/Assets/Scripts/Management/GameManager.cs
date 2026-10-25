using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    TMP_Text winScreenText;

    //the winner
    public string thiefName;
    public bool playerWon;

    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
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
    }

    private void FindTextInCurrentScene()
    {
        GameObject textObj = GameObject.FindGameObjectWithTag("GameManagerText");
        if (textObj != null)
        {
            winScreenText = textObj.GetComponent<TMP_Text>();
        }
        Debug.Log("hi");
        Debug.Log(winScreenText != null);

        WhoWon();
    }

    public void WhoWon()
    {
        if (playerWon)
        {
            winScreenText.text = thiefName + " Wins!";
        }
        else
        {
            winScreenText.text = "Boss Wins!";
        }
    }
}

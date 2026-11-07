using UnityEngine;
using UnityEngine.UI;

public class ChangeBackgroundWin : MonoBehaviour
{

    [SerializeField]
    GameManager manager;

    //the winner
    [SerializeField]
    Sprite[] sprite = new Sprite[5];

    Image myImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.playerWon)
        {
            myImage.sprite = sprite[manager.playerWhoWon];
        }
        else
        {
            myImage.sprite = sprite[4];
        }

        
    }
}

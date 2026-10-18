using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour
{
    // ------------------------------------------------------------------------------------------------
    // I do not understand why this all can't go into player manager.
    // It uses everything from there and nothing of itself.
    // The only thing that player manager doesn;t have thats here is Boss and thats another script.
    // I want to talk about this Sunday
    // ------------------------------------------------------------------------------------------------


    // goes from 1-4 and then resets
    public int playerIndex = 0;
    [SerializeField]
    public PlayerManager playerManager;
    [SerializeField]
    public BossScript boss;

    [SerializeField]
    TMP_Text playerTurnText;

    //[SerializeField]
    //int leftSideHealth = 450;

    //[SerializeField]
    //int rightSideHealth = 1050;

    //[SerializeField]
    //int offsetValue = 1;

    public void Start()
    {
        MoveCamera();
    }

    public void LateUpdate()
    {
        MoveCamera();
    }

    // when the pass button is clicked it goes to the next person
    // (player turn order is counterclockwise with how it is currently setup)
    public void OnCLickPass()
    {
        playerManager.healthBars[playerIndex].handleRect.GetComponent<Image>().color = Color.grey;

        if (playerIndex == 3) { //change to total Players instead of 4
            boss.BossAttack();
            playerIndex = 0;
        }
        else{
            playerIndex += 1;
        }

        if (playerManager.players[playerIndex].GetComponent<PlayerScript>().health <= 0)
        {

            Vector3 moveBehind = new Vector3(playerManager.players[playerIndex].transform.position.x, playerManager.players[playerIndex].transform.position.y, 5);
            playerManager.players[playerIndex].transform.position = moveBehind;
            OnCLickPass();
        }

        //if (boss.hp <= 0)
        //{
        //    Vector3 moveBehind = new Vector3(boss.gameObject.transform.position.x, boss.gameObject.transform.position.y, 5);
        //    boss.gameObject.transform.position = moveBehind;
        //}

        playerTurnText.text = "Player " + (playerIndex + 1);

        // give playerManager the correct current player script for the next player
        playerManager.currentPlayerScript = playerManager.players[playerIndex].GetComponent<PlayerScript>();
        playerManager.players[playerIndex].GetComponent<PlayerScript>().hasMoved = false; 
        playerManager.players[playerIndex].GetComponent<PlayerScript>().hasAttacked = false;
        playerManager.healthBars[playerIndex].handleRect.GetComponent<Image>().color = playerManager.healthBars[playerIndex].fillRect.GetComponent<Image>().color;
        Debug.Log($"Current player = {playerIndex}");
    }

    private void MoveCamera()
    {
        Vector3 playerPosition = new Vector3(playerManager.players[playerIndex].transform.position.x, playerManager.players[playerIndex].transform.position.y, -10);
        Vector3 bossPosition = new Vector3(boss.gameObject.transform.position.x, boss.gameObject.transform.position.y, -10);
        //Vector3 offset = new Vector3(offsetValue, 0, 0);

        if (playerPosition.x < bossPosition.x)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, (2 * playerPosition + bossPosition) / 3.0f, 5f * Time.deltaTime);
            //for (int i = 0; i < 4; i++)
            //{
            //    playerManager.healthBars[i].transform.position = new Vector3(leftSideHealth, playerManager.healthBars[i].transform.position.y, playerManager.healthBars[i].transform.position.z);
            //}
        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, (2 * playerPosition + bossPosition) / 3.0f, 5f * Time.deltaTime);
            //for (int i = 0; i < 4; i++)
            //{
            //    playerManager.healthBars[i].transform.position = new Vector3(rightSideHealth, playerManager.healthBars[i].transform.position.y, playerManager.healthBars[i].transform.position.z);
            //}
        }



    }
}



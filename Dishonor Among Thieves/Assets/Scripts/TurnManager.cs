using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // goes from 1-4 and then resets
    public int playerIndex = 0;
    [SerializeField]
    public PlayerManager playerManager;
    [SerializeField]
    public BossScript boss;

    [SerializeField]
    TMP_Text playerTurnText;

    // when the pass button is clicked it goes to the next person
    // (player turn order is counterclockwise with how it is currently setup)
    public void OnCLickPass()
    {

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

        playerTurnText.text = "Player " + (playerIndex + 1);

        // give playerManager the correct current player script for the next player
        playerManager.currentPlayerScript = playerManager.players[playerIndex].GetComponent<PlayerScript>();
        playerManager.players[playerIndex].GetComponent<PlayerScript>().hasMoved = false; 
        playerManager.players[playerIndex].GetComponent<PlayerScript>().hasAttacked = false;
        Debug.Log($"Current player = {playerIndex}");
    }
}

using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // goes from 1-4 and then resets
    public int playerTurn = 1;
    [SerializeField]
    public PlayerManager playerManager;

    // when the pass button is clicked it goes to the next person
    // (player turn order is counterclockwise with how it is currently setup)
    public void OnCLickPass()
    {   //playerManager.players[playerTurn - 1].GetComponent<PlayerScript>().isCurrent = false; removed for move logic
        playerManager.players[playerTurn].GetComponent<PlayerScript>().isCurrent = true; 
        if(playerTurn == 4){ //change to total Players instead of 4
           playerTurn = 1;
        }
        else{
            playerTurn += 1;
        }
    }
}

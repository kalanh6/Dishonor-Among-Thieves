using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // goes from 1-4 and then resets
    public int playerTurn = 1;
    public PlayerManager playerManager;

    // when the pass button is clicked it goes to the next person
    // (player turn order is counterclockwise with how it is currently setup)
    public void OnCLickPass()
    {
        Debug.Log(playerTurn);
        if (playerTurn < playerManager.playerCount)
        {
            playerTurn++;
        }
        else
        {
            playerTurn = 1;
        }
    }
}

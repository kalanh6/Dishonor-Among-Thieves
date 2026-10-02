using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // goes from 1-4 and then resets
    public int playerIndex = 0;
    [SerializeField]
    public PlayerManager playerManager;
    [SerializeField]
    public BossScript boss;

    // when the pass button is clicked it goes to the next person
    // (player turn order is counterclockwise with how it is currently setup)
    public void OnCLickPass()
    {   playerManager.players[playerIndex].GetComponent<PlayerScript>().isCurrent = false;
        if(playerIndex == 3){ //change to total Players instead of 4
           playerIndex = 0;
           boss.isAttacking = true;
        }
        else{
            playerIndex += 1;
        }
        playerManager.players[playerIndex].GetComponent<PlayerScript>().isCurrent = true; 
        Debug.Log($"Current player = {playerIndex}");
    }
}

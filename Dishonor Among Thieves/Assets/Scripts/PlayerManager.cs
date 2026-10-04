using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public int playerCount;

    [SerializeField]
    public GameObject[] players = new GameObject[4];

    // where al players are currently (used for attacking another player in PlayerScript)
    public PlayerScript[,] playerRowColumn = new PlayerScript[9, 9];

    // to make it easier to pass to the next script
    public PlayerScript currentPlayerScript;

    int currentPlayer = 0; // goes 0-3 for 4 players to keep a normal index


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCount = players.Length;
        currentPlayerScript = players[currentPlayer].GetComponent<PlayerScript>();

        UpdatePlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // Move Function call----------------------------------------------------------
        // all done in PlayerScript but called here for the current Player
        // the move button was pressed and now waiting for where the player will move to
        if (currentPlayerScript.isTargeting && Mouse.current.leftButton.isPressed)
        {
            currentPlayerScript.MoveSelected();
        }

        // Attack Function
        // all done in PlayerScript but called here for the current Player
        // the attack button was pressed and now waiting for who the player will attack
        if (currentPlayerScript.isAttacking && Mouse.current.leftButton.isPressed)
        {
            currentPlayerScript.AttackSelected();
        }
    }
    #region OnClick functions for Move/Attack/Etc
    public void OnMoveClick()
    {
        Debug.Log(currentPlayerScript.hasMoved);
        Debug.Log(currentPlayerScript.isTargeting);
        // player hit Move button and now needs to click a square to move to
        // Moving happens in the update and MoveSelected in PlayerScript
        if (currentPlayerScript.hasMoved == false && currentPlayerScript.isTargeting == false)
        {
            currentPlayerScript.isTargeting = true;
        }
        // turn off targeting if player hit the move button again before moving
        else
        {
            currentPlayerScript.isTargeting = false;
        }
    }

    public void OnAttackClick()
    {
        Debug.Log(currentPlayerScript.hasAttacked);
        Debug.Log(currentPlayerScript.isAttacking);

        // player hit Move button and now needs to click a square to move to
        // Moving happens in the update and MoveSelected in PlayerScript
        if (currentPlayerScript.hasAttacked == false && currentPlayerScript.isAttacking == false)
        {
            currentPlayerScript.isAttacking = true;
        }
        // turn off targeting if player hit the move button again before moving
        else
        {
            currentPlayerScript.isAttacking = false;
        }
    }
    #endregion

    #region Player Rows and Columns
    private void UpdatePlayerPosition()
    {
        // set each player's position in the array to check for attacking later
        for (int i = 0; i < playerCount; i++)
        {
            PlayerScript player = players[i].GetComponent<PlayerScript>();
            playerRowColumn[player.row, player.column] = player;
        }
    }

    public void UpdateCurrentPlayer(PlayerScript player)
    {
        playerRowColumn[player.row, player.column] = player;
    }
    #endregion
    public bool CheckAdjacentPlayer(int row, int column, int damage)
    {
        Debug.Log(row + " " + column);

        for (int i = 0; i < playerCount; i++)
        {
            PlayerScript player = players[i].GetComponent<PlayerScript>();
            Debug.Log(i);
            if (player.row == row && player.column == column)
            {
                Debug.Log("hit myself");
                continue;
            }

            if ((Mathf.Abs(player.row - row) <= 1) && (Mathf.Abs(player.column - column) <= 1))
            {
                player.health--;
                Debug.Log(i + " " + player.health);
                return true;
            }
        }
        Debug.Log("returned no");
        return false;
        
    }
}

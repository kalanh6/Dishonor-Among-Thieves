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
        if (currentPlayerScript.isTargeting && Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentPlayerScript.MoveSelected();
        }

        // Attack Function
        // all done in PlayerScript but called here for the current Player
        // the attack button was pressed and now waiting for who the player will attack
        if (currentPlayerScript.isAttacking && Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentPlayerScript.AttackSelected();
        }
    }
    #region OnClick functions for Move/Attack/Etc
    public void OnMoveClick()
    {
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
        bool returnBool = false;
        int rowDiff;
        int colDiff;
        for (int i = 0; i < playerCount; i++)
        {
            PlayerScript player = players[i].GetComponent<PlayerScript>();
            if (player.row == row && player.column == column)
            {
                continue;
            }
            Debug.Log($"Player Checked: Player {i + 1} \n row: {player.row} col: {player.column}");
            Debug.Log($"Attacker At:\n row: {row} col: {column}");
            rowDiff = Mathf.Abs(player.row - row);
            colDiff = Mathf.Abs(player.column - column);
            if ((rowDiff == 1 ^ colDiff == 1) && (rowDiff == 0 ^ colDiff == 0))
            {
                Debug.Log(player.health + " hp now");
                player.health -= damage;
                Debug.Log(player.health + " hp now");
                returnBool = true;
            }
        }
        return returnBool;
        
    }
}

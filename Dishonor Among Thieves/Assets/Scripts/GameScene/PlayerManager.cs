using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int playerCount;

    [SerializeField]
    public GameObject[] players = new GameObject[4];
    int playersAlive;

    // to make it easier to pass to the next script
    public PlayerScript currentPlayerScript;

    int currentPlayer = 0; // goes 0-3 for 4 players to keep a normal index


    [SerializeField]
    public Slider[] healthBars = new Slider[4];

    [SerializeField]
    Button moveButton;

    [SerializeField]
    Button attackButton;

    //Variables from turn manager
    // goes from 1-4 and then resets
    public int playerIndex = 0;
    [SerializeField]
    public BossScript boss;

    [SerializeField]
    TMP_Text playerTurnText;

    [SerializeField]
    internal TMP_Text damageLog;

     [SerializeField]
    internal int maxMove;

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    GameObject actionBarGroup;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCount = players.Length;
        currentPlayerScript = players[currentPlayer].GetComponent<PlayerScript>();

        //from turn man
        MoveCamera();

        playersAlive = players.Length;
    }

    // from turn man
    public void LateUpdate()
    {
        MoveCamera();
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
            attackButton.interactable = !currentPlayerScript.hasAttacked;
        }
    }
    
    internal void ResetAttacked(){
        foreach(GameObject player in players){
            player.GetComponent<PlayerScript>().hasBeenAttacked = false;
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
        //Debug.Log(currentPlayerScript.hasAttacked);
        //Debug.Log(currentPlayerScript.isAttacking);

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

    public bool CheckAdjacentPlayer(int row, int column, int damage, bool isBoss)
    {
        bool returnBool = false;
        // int rowDiff;
        // int colDiff;
        for (int i = 0; i < playerCount; i++)
        {
            PlayerScript player = players[i].GetComponent<PlayerScript>();
            if (player.row == row && player.column == column)
            {
                continue;
            }
            //Debug.Log($"Player Checked: Player {i + 1} \n row: {player.row} col: {player.column}");
            //Debug.Log($"Attacker At:\n row: {row} col: {column}");
            // rowDiff = Mathf.Abs(player.row - row);
            // colDiff = Mathf.Abs(player.column - column);
            //if ((rowDiff == 1 ^ colDiff == 1) && (rowDiff == 0 ^ colDiff == 0)) no diagonal code
            // if (rowDiff <= 1 && colDiff <= 1)
            if(CheckAdjacentMath(player.row, player.column, row, column))
            {
                if(!player.hasBeenAttacked)
                {
                    Debug.Log(player.health + " hp now");
                    player.health -= damage;
                    Debug.Log(player.health + " hp now");
                    returnBool = true;
                    player.hasBeenAttacked = true;
                    if(isBoss){
                    damageLog.text = "The Boss attacked all adjacent players";
                    }
                    else if(!isBoss){
                        damageLog.text = "Player " + (playerIndex + 1) + " attacked Player " + player.playerNumber + " for " + damage + "damage.";
                    }
                    UpdateHealthBars();
                }
            }
        }
        return returnBool;
        
    }

    public bool CheckAdjacentMath(int aRow, int aCol, int bRow, int bCol){
        if(Mathf.Abs(aRow - bRow) <= 1){
            if(Mathf.Abs(aCol - bCol) <= 1){
                return true;
            }
        }
        return false;
    }
    

    public bool CheckSquare(int row, int column)
    {
        Debug.Log($"Trying to move from {currentPlayerScript.row}, {currentPlayerScript.column} to {row}, {column}");
        if(!currentPlayerScript.isAttacking){
            //Debug.Log(row + " " + column);

            for (int i = 0; i < playerCount; i++)
            {
                PlayerScript player = players[i].GetComponent<PlayerScript>();
                //Debug.Log(i);

                if ((player.row == row) && (player.column == column))
                {
                    currentPlayerScript.isTargeting = false;
                    return false;
                }
                if((Mathf.Abs(currentPlayerScript.row - row) + Mathf.Abs(currentPlayerScript.column - column)) > maxMove){
                    currentPlayerScript.isTargeting = false;
                    return false;
                }
            }
            //Debug.Log("Moved");
            moveButton.interactable = false;
            return true;
        }

        for (int i = 0; i < playerCount; i++)
        {
            PlayerScript player = players[i].GetComponent<PlayerScript>();
            //Debug.Log(i);

            if ((player.row == row) && (player.column == column))
            {
                
                return true;
            }
        }
        return false;
    }

    private void UpdateHealthBars()
    {
        int alive = players.Length;
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].value = players[i].GetComponent<PlayerScript>().health;

            if (players[i].GetComponent<PlayerScript>().health <= 0)
            {
                alive--;
            }

        }
        playersAlive = alive;
    }

    public void OnClickPass()
    {
        healthBars[playerIndex].handleRect.GetComponent<Image>().color = Color.grey;

        if (playerIndex == 3)
        { //change to total Players instead of 4
            boss.BossAttack();
            playerIndex = 0;
        }
        else
        {
            playerIndex += 1;
        }

        if (players[playerIndex].GetComponent<PlayerScript>().health <= 0)
        {
            Vector3 moveBehind = new Vector3(players[playerIndex].transform.position.x, players[playerIndex].transform.position.y, 5);
            players[playerIndex].transform.position = moveBehind;
            if (playersAlive > 0)
            {
                OnClickPass();
            }
            else
            {
                gameManager.playerWon = false;
                SceneManager.LoadScene("WinLoseMenu");
            }
        }

        if (boss.hp <= 0)
        {
            gameManager.playerWon = true;
            gameManager.thiefName = "Player " + (playerIndex);
            SceneManager.LoadScene("WinLoseMenu");
        }

        playerTurnText.text = "Player " + (playerIndex + 1);

        // give playerManager the correct current player script for the next player
        currentPlayerScript = players[playerIndex].GetComponent<PlayerScript>();
        players[playerIndex].GetComponent<PlayerScript>().hasMoved = false;
        players[playerIndex].GetComponent<PlayerScript>().hasAttacked = false;
        healthBars[playerIndex].handleRect.GetComponent<Image>().color = healthBars[playerIndex].fillRect.GetComponent<Image>().color;
        //Debug.Log($"Current player = {playerIndex}");





        moveButton.interactable = true;
        attackButton.interactable = true;
        ResetAttacked();
    }

    private void MoveCamera()
    {
        Vector3 playerPosition = new Vector3(players[playerIndex].transform.position.x, players[playerIndex].transform.position.y, -10);
        Vector3 bossPosition = new Vector3(boss.gameObject.transform.position.x, boss.gameObject.transform.position.y, -10);
        //Vector3 offset = new Vector3(offsetValue, 0, 0);

        if (playerPosition.x < bossPosition.x)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, (2 * playerPosition + bossPosition) / 3.0f, 5f * Time.deltaTime);

            actionBarGroup.transform.position = new Vector3(150, actionBarGroup.transform.position.y, actionBarGroup.transform.position.z);
        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, (2 * playerPosition + bossPosition) / 3.0f, 5f * Time.deltaTime);

            actionBarGroup.transform.position = new Vector3(1200, actionBarGroup.transform.position.y, actionBarGroup.transform.position.z);
        }
    }
}

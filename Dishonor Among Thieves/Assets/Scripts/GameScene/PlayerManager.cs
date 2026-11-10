//using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
//using Unity.VectorGraphics;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
//using static UnityEngine.Rendering.DebugUI.Table;

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

    [SerializeField]
    Button specialAttackButton;

    //Variables from turn manager
    // goes from 0-3 and then resets
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

    [SerializeField]
    Canvas canvasUI;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCount = players.Length;
        currentPlayerScript = players[currentPlayer].GetComponent<PlayerScript>();

        //from turn man
        MoveCamera();

        playersAlive = players.Length;

        //UpdateHealthBars();
        //OnClickPass();
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
        if (currentPlayerScript.isTargeting)
        {
            currentPlayerScript.HighlightSelected();
            if(Mouse.current.leftButton.wasPressedThisFrame){
                currentPlayerScript.MoveSelected();
            }
        }

        // Attack Function
        // all done in PlayerScript but called here for the current Player
        // the attack button was pressed and now waiting for who the player will attack

        if (currentPlayerScript.isAttacking)
        {
            //currentPlayerScript.HighlightSelected();

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                currentPlayerScript.AttackSelected();
                if(specialAttackButton.interactable){
                 specialAttackButton.interactable = !currentPlayerScript.hasAttacked;
                }
                attackButton.interactable = !currentPlayerScript.hasAttacked;
            }
        }

        

        

        if (currentPlayerScript.isSpecialAttacking)
        {
            // checking which special attack to do - different per class
            switch (playerIndex)
            {
                case 0:
                    currentPlayerScript.HighlightSelected();
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        currentPlayerScript.AttackSpecial();
                    }
                    break;
                case 1:
                case 3:
                    currentPlayerScript.HighlightAOE();

                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        Debug.Log("specialAttack clicked");
                        currentPlayerScript.AttackSpecialMage();
                    }
                    break;
                case 2:
                    currentPlayerScript.HighlightCharge();
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        currentPlayerScript.AttackSpecialTank();
                    }
                    break;
                //case 3:
                //    break;
                default:
                    break;
            }

            specialAttackButton.interactable = !currentPlayerScript.hasAttacked;
            attackButton.interactable = !currentPlayerScript.hasAttacked;
        }

        // change scene when boss dies
        if (boss.hp <= 0)
        {
            gameManager.AddPoints(5, playerIndex);
            gameManager.playerWon = true;
            gameManager.playerWhoWon = playerIndex;
            SceneManager.LoadScene("EndlessModeEnd");
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

            currentPlayerScript.isAttacking = false;
            currentPlayerScript.isSpecialAttacking = false;
            currentPlayerScript.AOEgroup.transform.position = new Vector3(-10, 0, -2f);
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

            currentPlayerScript.isTargeting = false;
            currentPlayerScript.isSpecialAttacking = false;
            currentPlayerScript.AOEgroup.transform.position = new Vector3(-10, 0, -2f);
        }
        // turn off targeting if player hit the move button again before moving
        else
        {
            currentPlayerScript.isAttacking = false;
        }
    }

    // will need to be fixed so clicking basic attack and special attack rgight after doesnt turn it off
    public void OnSpecialAttackClick()
    {
        // player hit Move button and now needs to click a square to move to
        // Moving happens in the update and MoveSelected in PlayerScript
        if (currentPlayerScript.hasAttacked == false && currentPlayerScript.isSpecialAttacking == false)
        {
            currentPlayerScript.isSpecialAttacking = true;

            currentPlayerScript.isTargeting = false;
            currentPlayerScript.isAttacking = false;
        }
        // turn off targeting if player hit the move button again before moving
        else
        {
            currentPlayerScript.isSpecialAttacking = false;
            currentPlayerScript.AOEgroup.transform.position = new Vector3(-10, 0, -2f);
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
                    if(!isBoss){
                        damageLog.text = "Player " + (playerIndex + 1) + " attacked Player " + player.playerNumber + " for " + damage + "damage.";
                    }
                    UpdateHealthBars();
                }
            }
        }
        if(isBoss){
        damageLog.text = "The Boss attacked all adjacent players";
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

    public bool CheckBossAttack2(int num, bool row, int damage){
        bool returnBool = false;
        for (int i = 0; i < playerCount; i++)
        {
            PlayerScript player = players[i].GetComponent<PlayerScript>();
            if(row){
                if(player.row == num){
                    player.health -= damage;
                    returnBool = true;
                }
            }

            if(!row){
                if(player.column == num){
                    player.health -= damage;
                    returnBool = true;
                }
            }
        }
        UpdateHealthBars();
         damageLog.text = "The Boss attacked all of its rows and columns";
        return returnBool;
    }

    public bool CheckSpecialAttack(int sRow, int sCol){
        int distance = 0;
        string dir = "";
        bool isVertical = false;
        bool returnBool = false;
        PlayerScript player;
        if(sRow == currentPlayerScript.row && Mathf.Abs(sCol - currentPlayerScript.column) <= 3){
            distance = sCol - currentPlayerScript.column;
        }
        //equal columns, less than 3 difference rows
        else if(sCol == currentPlayerScript.column && Mathf.Abs(sRow - currentPlayerScript.row) <= 3){
            distance = sRow - currentPlayerScript.row;
            isVertical = true;
        }

        if(isVertical){
            if(distance < 0){
                dir = "up";
            }
            else{
                dir = "down";
            }
        }

        if(!isVertical){
            if(distance < 0){
                dir = "left";
            }
            else{
                dir = "right";
            }
        }
        Debug.Log($"Special {dir} for {Mathf.Abs(distance)} squares");
        //player check
        for(int i = 0; i < playerCount; i++){
            player = players[i].GetComponent<PlayerScript>();
            if (player.row == currentPlayerScript.row && player.column == currentPlayerScript.column)
            {
                continue;
            }
            switch(dir){
                case "left":
                    if(player.column - currentPlayerScript.column >= distance && player.row == currentPlayerScript.row){
                        player.health -= 2;
                        Debug.Log($"Player {i+1} hit by special");
                        damageLog.text = "Player " + (playerIndex + 1) + " attacked Player " + (i+1) + " for " + 2 + " damage.";
                        returnBool = true;
                    }
                break;
                case "right":
                    if(player.column - currentPlayerScript.column <= distance && player.row == currentPlayerScript.row){
                        player.health -= 2;
                        Debug.Log($"Player {i+1} hit by special");
                        damageLog.text = "Player " + (playerIndex + 1) + " attacked Player " + (i+1) + " for " + 2 + " damage.";
                        returnBool = true;
                    }
                break;
                case "up":
                    if(player.row - currentPlayerScript.row >= distance && player.column == currentPlayerScript.column){
                        player.health -= 2;
                        Debug.Log($"Player {i+1} hit by special");
                        damageLog.text = "Player " + (playerIndex + 1) + " attacked Player " + (i+1) + " for " + 2 + " damage.";
                        returnBool = true;
                    }
                break;
                case "down":
                    if(player.row - currentPlayerScript.row <= distance && player.column == currentPlayerScript.column){
                        player.health -= 2;
                        Debug.Log($"Player {i+1} hit by special");
                        damageLog.text = "Player " + (playerIndex + 1) + " attacked Player " + (i+1) + " for " + 2 + " damage.";
                        returnBool = true;
                    }
                break;
                default:
                    break;
            }
        }
        UpdateHealthBars();
        //boss check
        for(int k = 0; k < boss.rows.Length; k++){
            for(int j = 0; j < boss.columns.Length; j++){
                switch(dir){
                    case "left":
                        if(boss.columns[j] - currentPlayerScript.column >= distance && boss.rows[k] == currentPlayerScript.row){
                            boss.hp -= 2; 
                            Debug.Log($"Boss hit by special");
                            returnBool = true;
                        }
                    break;
                    case "right":
                        if(boss.columns[j] - currentPlayerScript.column <= distance && boss.rows[k] == currentPlayerScript.row){
                            boss.hp -= 2; 
                            //TEST THIS
                            Debug.Log($"Boss hit by special");
                            returnBool = true;
                        }
                    break;
                    case "up":
                        if(boss.rows[k] - currentPlayerScript.row >= distance && boss.columns[j] == currentPlayerScript.column){
                            boss.hp -= 2; 
                            Debug.Log($"Boss hit by special");
                            returnBool = true;
                        }
                    break;
                    case "down":
                        if(boss.rows[k] - currentPlayerScript.row <= distance && boss.columns[j] == currentPlayerScript.column){
                            boss.hp -= 2; 
                            Debug.Log($"Boss hit by special");
                            returnBool = true;
                        }
                    break;
                    default:
                    break;
                }
            }
        }
        currentPlayerScript.specialCooldown = 3;
        return returnBool;
    }

    public bool CheckSpecialAttackMage(int sRow, int sCol, int distanceAllowed, int damage)
    {
        bool returnBool = false;
        int playerRow = currentPlayerScript.row;
        int playerCol = currentPlayerScript.column;

        // didnt hit a close enough square to self
        if (distanceAllowed - Mathf.Abs(playerCol - sCol) - Mathf.Abs(playerRow - sRow) < 0)
        {
            Debug.Log("Clicked bad square");
            return false;
        }

        Debug.Log("Clicked good square");


        // checks all players to be next to clicked square
        for (int i = 0; i < playerCount; i++)
        {
            // dont hit yourself - (could turn this part off for a bit more strategy)
            PlayerScript player = players[i].GetComponent<PlayerScript>();
            //if (i == playerIndex)
            //{
            //    continue;
            //}

            // hit a player
            if (CheckAdjacentMath(sRow, sCol, player.row, player.column))
            {
                damageLog.text = "Player " + (playerIndex + 1) + " attacked Player " + (i+1) + " for " + damage + " damage.";
                player.health -= damage;
                returnBool = true;
            }
        }

        // check for boss hit
        for (int i = 0; i < boss.rows.Length; i++)
        {
            for (int j = 0; j < boss.columns.Length; j++)
            {
                if (CheckAdjacentMath(sRow, sCol, boss.rows[i], boss.columns[i]))
                {
                    Debug.Log($"Boss Hit at {boss.hp} health");
                    boss.hp -= 1;
                    damageLog.text = "Player " + (playerIndex + 1) + " attacked the Boss for 1 damage.";
                    Debug.Log($"Boss is now at {boss.hp} health");
                    returnBool = true;
                }
            }
        }

        //Show the players what took damage
        UpdateHealthBars();
        currentPlayerScript.specialCooldown = 3;
        return returnBool;
    }
    public bool CheckSpecialAttackTank(int sRow, int sCol, int distanceAllowed, int damage)
    {
        // needs to be in a signle cardinal direction from the player
        // needs to check for walls hit/not hit
        // needs to hit players in the path it moves
        // needs to lerp the player in that direction
        // needs to check hitting boss and hit for both squares hit


        int playerRow = currentPlayerScript.row;
        int playerCol = currentPlayerScript.column;

        // checking for distance it moves
        // if too far it doesnt begin moving
        if (!(sRow == currentPlayerScript.row || sCol == currentPlayerScript.column))
        {
            Debug.Log("Clicked bad square");
            return false;
        }
        if (sRow == currentPlayerScript.row && sCol == currentPlayerScript.column)
        {
            Debug.Log("Clicked bad square");
            return false;
        }

        // good distance -> start charging
        Debug.Log("Clicked good square");

        int distance = 0;
        string dir = "";
        bool isVertical = false;
        PlayerScript player;

        if (sRow == currentPlayerScript.row && Mathf.Abs(sCol - currentPlayerScript.column) <= distanceAllowed)
        {
            distance = sCol - currentPlayerScript.column;
        }
        //equal columns, less than 3 difference rows
        else if (sCol == currentPlayerScript.column && Mathf.Abs(sRow - currentPlayerScript.row) <= distanceAllowed)
        {
            distance = sRow - currentPlayerScript.row;
            isVertical = true;
        }

        if (isVertical)
        {
            if (distance < 0)
            {
                dir = "up";

                //check if you hit a wall
                if (sRow == 0)
                {
                    currentPlayerScript.health -= damage;
                }
            }
            else
            {
                dir = "down";

                //check if you hit a wall
                if (sRow == 8)
                {
                    currentPlayerScript.health -= damage;
                }
            }
        }

        if (!isVertical)
        {
            if (distance < 0)
            {
                dir = "left";

                //check if you hit a wall
                if (sCol == 0)
                {
                    currentPlayerScript.health -= damage;
                }
            }
            else
            {
                dir = "right";

                //check if you hit a wall
                if (sCol == 8)
                {
                    currentPlayerScript.health -= damage;
                }
            }
        }

        

        // checks all players to be next to clicked square
        for (int i = 0; i < playerCount; i++)
        {
            // dont hit yourself - (could turn this part off for a bit more strategy)
            player = players[i].GetComponent<PlayerScript>();
            if (i == playerIndex)
            {
                continue;
            }

            // checking columns for players
            if (isVertical)
            {
                // only check if its possible to hit
                if (player.column == currentPlayerScript.column)
                {
                    //check up
                    if (dir == "up")
                    {
                        if (CheckSquareBetween(player.row, sRow, currentPlayerScript.row))
                        {
                            player.health -= damage;
                        }
                    }

                    //check down
                    else if (dir == "down")
                    {
                        if (CheckSquareBetween(player.row, currentPlayerScript.row, sRow))
                        {
                            player.health -= damage;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }

            //checking rows for players
            if (!isVertical)
            {
                if (player.row == currentPlayerScript.row)
                {
                    //check left
                    if (dir == "left")
                    {
                        if (CheckSquareBetween(player.column, sCol, currentPlayerScript.column))
                        {
                            player.health -= damage;
                        }
                    }

                    //check right
                    else if (dir == "right")
                    {
                        if (CheckSquareBetween(player.column, currentPlayerScript.column, sCol))
                        {
                            player.health -= damage;
                        }
                    }
                }
            }
        }

        if (isVertical)
        {
            if (currentPlayerScript.column == 4 || currentPlayerScript.column == 5)
            {
                for (int i = 0; i < boss.rows.Length; i++)
                {

                    //check up
                    if (dir == "up")
                    {
                        if (CheckSquareBetween(boss.rows[i], sRow, currentPlayerScript.row))
                        {
                            boss.hp -= damage;
                        }
                    }

                    //check down
                    else if (dir == "down")
                    {
                        if (CheckSquareBetween(boss.rows[i], currentPlayerScript.row, sRow))
                        {
                            boss.hp -= damage;
                        }
                    }
                }
            }
        }
        else
        {
            if (currentPlayerScript.row == 4 || currentPlayerScript.row == 5)
            {
                for (int i = 0; i < boss.columns.Length; i++)
                {
                    //check up
                    if (dir == "left")
                    {
                        if (CheckSquareBetween(boss.columns[i], sCol, currentPlayerScript.column))
                        {
                            boss.hp -= damage;
                        }
                    }

                    //check down
                    else if (dir == "right")
                    {
                        if (CheckSquareBetween(boss.columns[i], currentPlayerScript.column, sCol))
                        {
                            boss.hp -= damage;
                        }
                    }
                }
            }
        }

        //Show the players what took damage
        UpdateHealthBars();
        currentPlayerScript.specialCooldown = 3;
        return true;
    }

    public void CheckSpecialAttackHeal(){
        int bossGiveBack; // 1 per player. Damage each player for 2
        PlayerScript player;
        for(int i = 0; i < playerCount; i++){
            player = players[i].GetComponent<PlayerScript>();
            if(player.row == currentPlayerScript.row && player.column == currentPlayerScript.column){
            continue;
            }
            if(player.health <= 0){
                continue;
            }
            Debug.Log($"Healing boss and attacking Player {i+1}");
            player.health -= 2;
            boss.hp += 1;
        }
        currentPlayerScript.health += 2;
        UpdateHealthBars();
        currentPlayerScript.specialCooldown = 3;
    }

    public bool CheckSquare(int row, int column, bool moving)
    {
        if(moving){
            Debug.Log($"Trying to move from {currentPlayerScript.row}, {currentPlayerScript.column} to {row}, {column}");
        }
        if(currentPlayerScript.isTargeting){
            //Debug.Log(row + " " + column);

            for (int i = 0; i < playerCount; i++)
            {
                PlayerScript player = players[i].GetComponent<PlayerScript>();
                //Debug.Log(i);

                if ((player.row == row) && (player.column == column))
                {
                    if(moving){
                    currentPlayerScript.isTargeting = false;
                    }
                    return false;
                }
                if((Mathf.Abs(currentPlayerScript.row - row) + Mathf.Abs(currentPlayerScript.column - column)) > maxMove){
                    if(moving){
                    currentPlayerScript.isTargeting = false;
                    }
                    return false;
                }
            }
            //Debug.Log("Moved");
            if(moving){
                moveButton.interactable = false;
            }
            return true;
        }

        if(currentPlayerScript.isSpecialAttacking){
            Debug.Log("check square");
            if(currentPlayerScript.column == column && currentPlayerScript.row == row){
                Debug.Log("check self");
                return false;
            }
            switch (playerIndex)
            {
                case 0:
                case 2:
                    if (row == currentPlayerScript.row && Mathf.Abs(column - currentPlayerScript.column) <= 3)
                    {
                        return true;
                    }
                    //equal columns, less than 3 difference rows
                    else if (column == currentPlayerScript.column && Mathf.Abs(row - currentPlayerScript.row) <= 3)
                    {
                        return true;
                    }
                    break;
                case 1:
                case 3:
                    Debug.Log("got to player index 1/3");
                    //the 5 is distance allowed (didnt feel like moving it
                    if ((2 - Mathf.Abs(currentPlayerScript.column - column) - Mathf.Abs(currentPlayerScript.row - row)) >= 0)
                    {
                        return true;
                    }
                    return false;
                    
                //case 2:
                //    break;
                //case 3:
                //    break;
                default:
                    break;
            }
            
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
                if (playerIndex != i)
                {
                    gameManager.AddPoints(5, playerIndex);
                }
            }

        }
        playersAlive = alive;
    }

    public void OnClickPass()
    {
        healthBars[playerIndex].handleRect.GetComponent<Image>().color = Color.grey;

        if (playerIndex == 3)
        { //change to total Players instead of 4
            if(boss.adjacentAttack){
            boss.BossAttack();
            }
            else if(!boss.adjacentAttack){
            boss.BossAttack2();
            }
            
            playerIndex = 0;
            for(int i = 0; i < playerCount; i++){
                if(players[i].GetComponent<PlayerScript>().specialCooldown > 0){
                    players[i].GetComponent<PlayerScript>().specialCooldown--;
                }
            }
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
                SceneManager.LoadScene("EndlessModeEnd");
            }
        }

        //playerTurnText.text = "Player " + (playerIndex);

        // give playerManager the correct current player script for the next player
        currentPlayerScript = players[playerIndex].GetComponent<PlayerScript>();
        players[playerIndex].GetComponent<PlayerScript>().hasMoved = false;
        players[playerIndex].GetComponent<PlayerScript>().hasAttacked = false;
        healthBars[playerIndex].handleRect.GetComponent<Image>().color = healthBars[playerIndex].fillRect.GetComponent<Image>().color;
        //Debug.Log($"Current player = {playerIndex}");



        if(currentPlayerScript.specialCooldown == 0){
            specialAttackButton.interactable = true;
        }
        else if(currentPlayerScript.specialCooldown != 0){
            specialAttackButton.interactable = false;
        }
        moveButton.interactable = true;
        attackButton.interactable = true;
        ResetAttacked();
    }

    private void MoveCamera()
    {
        Vector3 playerPosition = new Vector3(players[playerIndex].transform.position.x, players[playerIndex].transform.position.y, -10);
        Vector3 bossPosition = new Vector3(boss.gameObject.transform.position.x, boss.gameObject.transform.position.y, -10);
        //Vector3 offset = new Vector3(offsetValue, 0, 0);
        
        float width = canvasUI.GetComponent<RectTransform>().rect.width;

        //left side
        if (playerPosition.x < bossPosition.x)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, (2 * playerPosition + bossPosition) / 3.0f, 5f * Time.deltaTime);

            actionBarGroup.transform.position = new Vector3(canvasUI.transform.position.x - (width), actionBarGroup.transform.position.y, actionBarGroup.transform.position.z);
        }
        //right side
        else
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, (2 * playerPosition + bossPosition) / 3.0f, 5f * Time.deltaTime);

            actionBarGroup.transform.position = new Vector3(canvasUI.transform.position.x + (width), actionBarGroup.transform.position.y, actionBarGroup.transform.position.z);
        }
    }

    /// <summary>
    /// used in a for loop of checking players' squares for rows and/or columns
    /// first used in SpecialAttackTank
    /// </summary>
    /// <param name="playerCheckProp">what the value starts at for the checked player</param>
    /// <param name="rangeBegin">where the range starts for checking against the player</param>
    /// <param name="rangeEnd">where it is ends to check the player against</param>
    /// <returns>return true if the player is in the range; returns false if the player is not in the range at all</returns>
    private bool CheckSquareBetween(int playerCheckProp, int rangeBegin, int rangeEnd)
    {
        if (playerCheckProp >= rangeBegin && playerCheckProp <= rangeEnd)
        {
            Debug.Log("checkProp, rangeBegin, rangeEnd");
            Debug.Log(playerCheckProp);
            Debug.Log(rangeBegin);
            Debug.Log(rangeEnd);

            return true;
        }
        return false;
    }
}

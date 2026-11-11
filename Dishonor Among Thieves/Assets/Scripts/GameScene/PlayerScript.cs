//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;
//using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerScript : MonoBehaviour
{
    // variables for player info
    [SerializeField]
    internal int health = 10; //make property?
    [SerializeField]
    internal int maxHealth = 10;
    [SerializeField]
    internal int playerNumber;

    [SerializeField]
    public int row;
    [SerializeField]
    public int column;

    [SerializeField]
    TileManager tileSet;

    [SerializeField]
    BossScript boss;

    [SerializeField]
    internal bool hasBeenAttacked;

    [SerializeField]
    internal int specialCooldown = 0;

    [SerializeField]
    public GameObject AOEgroup;

    [SerializeField]
    public GameObject[] chargeGroup = new GameObject[5];


    // variables for player state of turn
    public bool isAttacking = false;
    public bool hasAttacked = false;
    public bool isSpecialAttacking = false;
    public bool isTargeting = false;
    public bool hasMoved = false;

    [SerializeField]
    PlayerManager playerManager;


    // is this needed???
    void Start(){
        //assign currentSquare
        // for (int i = 0; i < tileSet.tiles.Length; i++)
        //{
        //    if (tileSet.tiles[i].position == position)
        //    {
        //        currentSquare = tiles[i];
        //    }
        //}
    }

    void Update(){
        if(health > maxHealth){
            health = maxHealth;
        }
    }
    public void AttackSpecial(){
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;

        Debug.Log("Special Attacking");
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to attack a player
            if (hit.collider.CompareTag("boardSquare"))
            {
                if(playerManager.CheckSpecialAttack(hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column)){
                    isSpecialAttacking = false;
                    hasAttacked = true;
                }
                else
                {
                    Debug.Log("SpecialAttackingNobody");
                    hasAttacked = false;
                    Debug.Log("Didnt hit a player square");
                }
            }
        }
    }
    public void AttackSpecialMage()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;

        Debug.Log("Special Attacking");
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to attack a player
            if (hit.collider.CompareTag("boardSquare"))
            {
                // Check parameters (row of square clicked on, column of square clicked on, distance allowed, damage amount)
                if (playerManager.CheckSpecialAttackMage(hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column, 2, 2))
                {
                    Debug.Log("Clicked special attack square");

                    isSpecialAttacking = false;
                    hasAttacked = true;
                    AOEgroup.transform.position = new Vector3(-10, 0, -2f);
                    hit.collider.GetComponent<TileScript>().Hover(false);
                }
                else
                {
                    Debug.Log("SpecialAttackingNobody");
                    hasAttacked = false;
                    Debug.Log("Didnt hit a player square");
                }
            }
        }
    }

    public void AttackSpecialTank()
    {
        // tank charge attack
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;

        Debug.Log("Special Attacking");
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to attack a player
            if (hit.collider.CompareTag("boardSquare"))
            {
                // Check parameters (row of square clicked on, column of square clicked on, distance allowed, damage amount)
                if (playerManager.CheckSpecialAttackTank(hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column, 5, 2))
                {
                    Vector3 nextPosition;
                    Debug.Log("Clicked special attack square");

                    isSpecialAttacking = false;
                    hasAttacked = true;
                    for (int i = 0; i < chargeGroup.Length; i++)
                    {
                        chargeGroup[i].transform.position = new Vector3(-9.6544f, 2, -2f);
                    }
                    hit.collider.GetComponent<TileScript>().Hover(false);

                    int checkRow = hit.collider.GetComponent<TileScript>().row;
                    int checkColumn = hit.collider.GetComponent<TileScript>().column;

                    if (!(playerManager.CheckSquare(hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column, true)))
                    {
                        nextPosition = hit.collider.gameObject.transform.position;
                        nextPosition.z = -4;
                        //playerScript.currentSquare = hit.collider.gameObject;
                        transform.position = nextPosition;

                        row = checkRow;
                        column = checkColumn;
                    }
                    else
                    {
                        Debug.Log("Im here moving away from player");
                        int distanceRow = row - checkRow;
                        int distanceColumn = column - checkColumn;
                        Debug.Log(distanceRow);
                        Debug.Log(distanceColumn);

                        if (distanceRow > 0)
                        {
                            nextPosition = hit.collider.gameObject.transform.position;
                            nextPosition.y += -0.6896f;
                            nextPosition.z = -4;
                            
                            transform.position = nextPosition;

                            row = checkRow + 1;
                            column = checkColumn;
                        }
                        else if (distanceRow < 0)
                        {
                            nextPosition = hit.collider.gameObject.transform.position;
                            nextPosition.y += 0.6896f;
                            nextPosition.z = -4;
                            
                            transform.position = nextPosition;

                            row = checkRow - 1;
                            column = checkColumn;
                        }
                        else if (distanceColumn > 0)
                        {
                            nextPosition = hit.collider.gameObject.transform.position;
                            nextPosition.x += 0.6896f;
                            nextPosition.z = -4;
                            
                            transform.position = nextPosition;

                            row = checkRow;
                            column = checkColumn + 1;
                        }
                        else if (distanceColumn < 0)
                        {
                            nextPosition = hit.collider.gameObject.transform.position;
                            nextPosition.y += -0.6896f;
                            nextPosition.z = -4;
                            
                            transform.position = nextPosition;

                            row = checkRow;
                            column = checkColumn - 1;
                        }

                    }
                    
                }
                else
                {
                    Debug.Log("SpecialAttackingNobody");
                    hasAttacked = false;
                    Debug.Log("Didnt hit a player square");
                }
            }
        }

    }

    public void AttackSpecialHealer(){
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;

        Debug.Log("Special Attacking With Healer");
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to attack a player
            if (hit.collider.CompareTag("boardSquare"))
            {
                if(hit.collider.GetComponent<TileScript>().isBossSquare){
                    if(playerManager.players.Length > 1){
                        playerManager.CheckSpecialAttackHeal();
                        isSpecialAttacking = false;
                        hasAttacked = true;
                    }
                    else
                    {
                        Debug.Log("SpecialAttackingNobody");
                        hasAttacked = false;
                        Debug.Log("Didnt hit a player square");
                    }
                }
            }
        }
    }

    // All functions that Player Manager will call to start the attack action for the current Player
    #region Attack
    public void AttackSelected()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;

        Debug.Log("Attacking");
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to attack a player
            if (hit.collider.CompareTag("boardSquare"))
            {
                if(hit.collider.GetComponent<TileScript>().isBossSquare){        
                    if (CheckAdjacentBoss(/*hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column*/))
                    {
                        Debug.Log("AttackingBoss");
                        isAttacking = false;
                        hasAttacked = true;
                    }
                }
                else if(playerManager.CheckAdjacentPlayer(row, column, 1, false)){
                    isAttacking = false;
                    hasAttacked = true;
                }
                else
                    {
                        Debug.Log("AttackingNobody");
                        hasAttacked = false;
                        Debug.Log("Didnt hit a player square");
                        //return false;
                    }
            }
            
        }
}

    // Here is where we want to make the new attacks
    // Make a method that is only for a specific character
    // When it is used then hasAttacked needs to be true so neither attack can work again
    // For checking players hit, use playerManager.Check adjacentplayer
    // For checking boss hit use CheckAdjacentBoss (not from player manager)(why is this different i dont know)(before we submit to magic maker, i want to fix all this code again ahhhhhhh)


    #endregion

    // Helper functions to check for certain properties needed for attack and move
    #region Tests
    private bool CheckAdjacentBoss(){
        // if(Mathf.Abs(currentSquare.GetComponent<TileScript>().row - tile.row) <= 1){
        //     if(Mathf.Abs(currentSquare.GetComponent<TileScript>().column - tile.column) <= 1){
        //         return true;
        //     }
        // }
        Debug.Log("CheckingBossAttack");
        for(int i = 0; i < boss.rows.Length; i++){
            for(int j = 0; j < boss.columns.Length; j++){
                if(playerManager.CheckAdjacentMath(row, column, boss.rows[i], boss.columns[i])){
                    Debug.Log($"Boss Hit at {boss.hp} health");
                    boss.hp -= 1;
                    playerManager.CheckDeath(4);
                    playerManager.damageLog.text = "Player " + (playerManager.playerIndex + 1) + " attacked the Boss for 1 damage.";
                    Debug.Log($"Boss is now at {boss.hp} health");
                    return true;
                }
            }
        }
        Debug.Log("Did not hit boss");
        return false;
    }

    private bool CheckRowsAndColumns(TileScript tile){
        return true;
    }
    #endregion

    // All functions that Player Manager will call to start the Move action for the current Player
    #region Move
    public void MoveSelected()
    {
        //teleports to the position given in Selected()
        

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to move the player to
            if (hit.collider.CompareTag("boardSquare"))
            {
                if (!hit.collider.GetComponent<TileScript>().isBossSquare)
                {
                    if (playerManager.CheckSquare(hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column, true))
                    {
                        Vector3 nextPosition;
                        nextPosition = hit.collider.gameObject.transform.position;
                        nextPosition.z = -4;
                        //playerScript.currentSquare = hit.collider.gameObject;
                        transform.position = nextPosition;

                        // put the player in the right array index


                        isTargeting = false;
                        hasMoved = true;
                        row = hit.collider.gameObject.GetComponent<TileScript>().row;
                        column = hit.collider.gameObject.GetComponent<TileScript>().column;
                    }
                    
                }
            }
        }
    }
    #endregion

    #region Highlights
    public void HighlightSelected()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to move the player to
            if (hit.collider.CompareTag("boardSquare"))
            {
                if (playerManager.CheckSquare(hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column, false))
                {
                    hit.collider.GetComponent<TileScript>().Hover(true);
                }
                else{
                    hit.collider.GetComponent<TileScript>().Hover(false);
                }
            }
        }
    }

    public void HighlightBasic()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to move the player to
            if (hit.collider.CompareTag("boardSquare"))
            {
                if (playerManager.CheckSquare(hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column, false))
                {
                    hit.collider.GetComponent<TileScript>().Hover(true);
                }
                else{
                    hit.collider.GetComponent<TileScript>().Hover(false);
                }
            }
        }
    }

    public void HighlightAOE()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to move the player to
            if (hit.collider.CompareTag("boardSquare"))
            {
                //Debug.Log("hit square");
                if (playerManager.CheckSquare(hit.collider.GetComponent<TileScript>().row, hit.collider.GetComponent<TileScript>().column, false) && isSpecialAttacking)
                {
                    AOEgroup.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, -4.5f);
                    hit.collider.GetComponent<TileScript>().Hover(true);
                    Debug.Log("change red position - Row: " + hit.collider.GetComponent<TileScript>().row + " Col: " + hit.collider.GetComponent<TileScript>().column);
                    Debug.Log((2 - Mathf.Abs(column - hit.collider.GetComponent<TileScript>().column) - Mathf.Abs(row - hit.collider.GetComponent<TileScript>().row)));
                }
                else
                {
                    Debug.Log("reset aoe group inside");
                    AOEgroup.transform.position = new Vector3(-10, 0, -2f);
                    hit.collider.GetComponent<TileScript>().Hover(false);
                }
            }
            else
            {
                Debug.Log("reset aoe group outside");
                AOEgroup.transform.position = new Vector3(-10, 0, -2f);
                hit.collider.GetComponent<TileScript>().Hover(false);
            }
        }
    }

    public void HighlightCharge()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to move the player to
            if (hit.collider.CompareTag("boardSquare"))
            {
                int checkRow = hit.collider.GetComponent<TileScript>().row;
                int checkColumn = hit.collider.GetComponent<TileScript>().column;
                bool isVertical = false;
                int distance = 0;
                int directionInDegrees = 0;
                float newPosition = 0;

                if (checkRow == row)
                {
                    distance = checkColumn - column;
                }
                //equal columns, less than 3 difference rows
                else if (checkColumn == column)
                {
                    distance = checkRow - row;
                    isVertical = true;
                }

                if (distance > 5)
                {
                    return;
                }

                // rotate the group in the right directions
                if (isVertical)
                {
                    if (distance < 0)
                    {
                        directionInDegrees = 90;
                    }
                    else
                    {
                        directionInDegrees = 270;
                    }
                }

                if (!isVertical)
                {
                    if (distance < 0)
                    {
                        directionInDegrees = 180;
                    }
                    else
                    {
                        directionInDegrees = 0;
                    }
                }
                for (int i = 0; i < Mathf.Abs(distance); i++)
                {
                    //Debug.Log(i);
                    switch(directionInDegrees)
                    {
                        case 0:
                            newPosition = transform.position.x + (0.6896f * (i + 1));
                            chargeGroup[i].transform.position = new Vector3(newPosition, hit.transform.position.y, -4.5f);
                            //Debug.Log("Right");
                            break;
                        case 90:
                            newPosition = transform.position.y + (0.6896f * (i + 1));
                            chargeGroup[i].transform.position = new Vector3(hit.transform.position.x, newPosition, -4.5f);
                            //Debug.Log("Up");
                            break;
                        case 180:
                            newPosition = transform.position.x - (0.6896f * (i + 1));
                            chargeGroup[i].transform.position = new Vector3(newPosition, hit.transform.position.y, -4.5f);
                            //Debug.Log("Left");
                            break;
                        case 270:
                            newPosition = transform.position.y - (0.6896f * (i + 1));
                            chargeGroup[i].transform.position = new Vector3(hit.transform.position.x, newPosition, -4.5f);
                            //Debug.Log("Down");
                            break;
                    }

                    
                }
                if (distance < 5)
                {
                    for (int i = Mathf.Abs(distance); i < 5; i++)
                    {
                        chargeGroup[i].transform.position = new Vector3(-9.6544f, 2, -2f);
                    }
                }
            }
            else
            {
                Debug.Log("reset charge group");
                AOEgroup.transform.position = new Vector3(-9.6544f, 2, -2f);
                hit.collider.GetComponent<TileScript>().Hover(false);
            }
        }
    }

    public void HighlightHeal()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to move the player to
            if (hit.collider.CompareTag("boardSquare"))
            {
                if (hit.collider.GetComponent<TileScript>().isBossSquare)
                {
                    hit.collider.GetComponent<TileScript>().Hover(true);
                }
                else{
                    hit.collider.GetComponent<TileScript>().Hover(false);
                }
            }
        }
    }
    #endregion
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // variables for player info
    [SerializeField]
    internal int health = 10; //make property?
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



    // variables for player state of turn
    public bool isAttacking = false;
    public bool hasAttacked = false;
    public bool isTargeting = false;
    public bool hasMoved = false;

    // keeping track of whos turn it is
    [SerializeField]
    TurnManager turnManager;

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
                if (playerManager.CheckAdjacentPlayer(row, column, 1) || CheckAdjacent())
                {
                    isAttacking = false;
                    hasAttacked = true;
                }
                else
                {
                    hasAttacked = false;
                    Debug.Log("Didnt hit a player square");
                }

            }
            
        }

}

    #endregion

    // Helper functions to check for certain properties needed for attack and move
    #region Tests
    private bool CheckAdjacent(){
        // if(Mathf.Abs(currentSquare.GetComponent<TileScript>().row - tile.row) <= 1){
        //     if(Mathf.Abs(currentSquare.GetComponent<TileScript>().column - tile.column) <= 1){
        //         return true;
        //     }
        // }
        Debug.Log("CheckingBossAttack");
        for(int i = 0; i < boss.rows.Length; i++){
            if(Mathf.Abs(row - boss.rows[i]) <= 1){
                 for(int j = 0; j < boss.columns.Length; j++){
                    if(Mathf.Abs(column - boss.columns[i]) <= 1){
                        Debug.Log($"Boss Hit at {boss.hp} health");
                            boss.hp -= 1;
                            Debug.Log($"Boss is now at {boss.hp} health");
                            return true;
                    }
                 }
            }
        }
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
        Vector3 nextPosition;

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to move the player to
            if (hit.collider.CompareTag("boardSquare"))
            {
                if (!hit.collider.GetComponent<TileScript>().isBossSquare)
                {
                    nextPosition = hit.collider.gameObject.transform.position;
                    nextPosition.z = -1;
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
    #endregion
}

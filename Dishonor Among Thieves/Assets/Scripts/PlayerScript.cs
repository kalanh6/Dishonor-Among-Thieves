using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    internal int health = 10; //make property?
    [SerializeField]
    internal int playerNumber;
    [SerializeField]
    internal GameObject currentSquare;
    [SerializeField]
    TileManager tileSet;
    [SerializeField]
    BossScript boss;
    [SerializeField]
    internal bool isCurrent;

    //attack variables
    private bool isAttacking;
    private bool hasAttacked;

    void Start(){
        //assign currentSquare
        // for(int i = 0; i < tileSet.tiles.Length; i++){
        //     if(tiles[i].position == position){
        //         currentSquare = tiles[i];
        //     }
        // }
    }

    //Attack Function

    // Update is called once per frame
    void Update()
    {
        if (isAttacking && Mouse.current.leftButton.isPressed)
        {
            AttackSelected();
        }
    }

    public void AttackSelected()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to attack a player
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponent<PlayerScript>().health--;

                isAttacking = false;
                hasAttacked = true;
            }
        }

}

    // make boss square array in tilemanager
    public void OnClick()
    {
        if(isCurrent){
            Debug.Log("Click!");
            for(int i = 0; i < tileSet.tiles.Length; i++){
                if(tileSet.tiles[i].GetComponent<TileScript>().isBossSquare){
                    if(CheckAdjacent(tileSet.tiles[i].GetComponent<TileScript>())){     
                            Debug.Log($"Boss Hit at {boss.hp} health");
                            boss.hp -= 1;
                            Debug.Log($"Boss is now at {boss.hp} health");
                    }
                }
            }
        }
    }

    

    private bool CheckAdjacent(TileScript tile){
        if(Mathf.Abs(currentSquare.GetComponent<TileScript>().row - tile.row) <= 1){
            if(Mathf.Abs(currentSquare.GetComponent<TileScript>().column - tile.column) <= 1){
                return true;
            }
        }
        return false;
    }

    private bool CheckRowsAndColumns(TileScript tile){
        return true;
    }
}

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

    void Start(){
        //assign currentSquare
        // for(int i = 0; i < tileSet.tiles.Length; i++){
        //     if(tiles[i].position == position){
        //         currentSquare = tiles[i];
        //     }
        // }
    }

    //Attack Function
    public void OnClick()
    {
        if(isCurrent){
            Debug.Log("Click!");
            for(int i = 0; i < tileSet.tiles.Length; i++){
                if(tileSet.tiles[i].GetComponent<TileScript>().isBossSquare){
                if(CheckAdjacent(tileSet.tiles[i].GetComponent<TileScript>())){     
                        //Debug.Log($"Boss Hit at {boss.hp} health");
                        boss.hp -= 1;
                        //Debug.Log($"Boss is now at {boss.hp} health");
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private bool CheckAdjacent(TileScript tile){
        Debug.Log($"Tile row: {tile.GetComponent<TileScript>().row} \nTile column: {tile.GetComponent<TileScript>().column}");
        if(Mathf.Abs(currentSquare.GetComponent<TileScript>().row - tile.row) <= 1){
            if(Mathf.Abs(currentSquare.GetComponent<TileScript>().column - tile.column) <= 1){
                return true;
            }
        }
        return false;
    }
}

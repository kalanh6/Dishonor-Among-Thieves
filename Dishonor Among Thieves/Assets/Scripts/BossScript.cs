using Unity.VisualScripting;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField]
    internal int hp;
    [SerializeField]
    internal bool isAttacking;
    [SerializeField]
    internal int[] rows;
    [SerializeField]
    internal int[] columns; // it was just a semi colon so idk what you wanted
    [SerializeField]
    PlayerManager playerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        //work on
        if(isAttacking){
            for(int i = 0; i < rows.Length; i++){
                for(int j = 0; j < columns.Length; j++){
                    
                    playerManager.CheckAdjacentPlayer(rows[i], columns[j], 3);
                    isAttacking = false;
                }
            }
            
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField]
    internal int hp;
    [SerializeField]
    internal int[] rows;
    [SerializeField]
    internal int[] columns; // it was just a semi colon so idk what you wanted
    [SerializeField]
    PlayerManager playerManager;
    [SerializeField]
    float lowHpTimer;
    [SerializeField]
    int lowHpThreshold = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BossAttack()
    {
        //work on
        for(int i = 0; i < rows.Length; i++){
            for(int j = 0; j < columns.Length; j++){
                    
                playerManager.CheckAdjacentPlayer(rows[i], columns[j], 3);
            }
        }
            
    }

    void Update(){
        if(hp<=lowHpThreshold){
            lowHpTimer += Time.deltaTime;
            if(lowHpTimer % 1 > .5){ //blink every half second
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else{
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}

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
    [SerializeField]
    int midHpThreshold = 10;
    [SerializeField]
    float attackTimer = 0;
    [SerializeField]
    internal bool isAttacking;
    [SerializeField]
    internal bool adjacentAttack = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BossAttack()
    {
        for(int i = 0; i < rows.Length; i++){
            for(int j = 0; j < columns.Length; j++){
                playerManager.CheckAdjacentPlayer(rows[i], columns[j], 3, true);
            }
        }
        Debug.Log("Boss Attack 1 complete");
        adjacentAttack = false;
    }

    public void BossAttack2()
    {
        Debug.Log("Boss attack 2");
        for(int i = 0; i < rows.Length; i++){
            playerManager.CheckBossAttack2(rows[i], true, 2);
        }
        for(int i = 0; i < columns.Length; i++){
            playerManager.CheckBossAttack2(columns[i], false, 2);
        }
        adjacentAttack = true;
    }

    void Update(){
        if(isAttacking){
            attackTimer += Time.deltaTime;
            if(attackTimer >= 2.0f){
                isAttacking = false;
                attackTimer = 0;
            }
        }
        if(hp<=lowHpThreshold){
            lowHpTimer += Time.deltaTime;
            if(lowHpTimer % 1 > .5){ //blink every half second
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else{
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        else if(hp<=midHpThreshold){
            lowHpTimer += Time.deltaTime;
            if(lowHpTimer % 2 > 1){ //blink every second
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else{
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}

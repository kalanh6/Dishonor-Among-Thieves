using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField]
    internal int hp;
    [SerializeField]
    internal bool isAttacking;
    [SerializeField]
    ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        //work on
        if(isAttacking){
            isAttacking = false;
        }
    }

    private bool CheckAdjacent(GameObject[] tiles){

    }
}

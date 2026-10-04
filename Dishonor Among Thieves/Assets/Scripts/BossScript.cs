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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        //work on
        if(isAttacking){
            isAttacking = false;
        }
    }

    // private bool CheckAdjacent(GameObject[] tiles){
    //     return true; // need to fix/was empty and would let me launch
    // }
}

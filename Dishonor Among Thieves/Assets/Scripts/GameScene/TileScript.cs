using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField]
    internal int row;
    [SerializeField]
    internal int column;
    [SerializeField]
    internal bool isBossSquare;
    [SerializeField]
    internal bool hovered;
    [SerializeField]
    internal bool isWhite;
    
    Vector3 tempVec;

    // [SerializeField]
    // internal float hoverTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hovered = false;
        tempVec = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // if(hovered){
            
        //     gameObject.GetComponent<SpriteRenderer>().color = Color.teal;
        //     tempVec.z = -5;
        //     gameObject.transform.position = tempVec;
        // }
        // //hovered = false;
        // if(!hovered){
        //     if(isWhite){
        //         gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        //     }
        //     else{
        //         gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        //     }
        //     tempVec.z = -1;
        //     gameObject.transform.position = tempVec;
        // }
    }
}

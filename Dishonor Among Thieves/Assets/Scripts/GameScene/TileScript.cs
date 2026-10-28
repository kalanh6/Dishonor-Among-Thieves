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

    Color tempCol;

    [SerializeField]
    internal float hoverTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hovered = false;
        tempVec = gameObject.transform.position;
        hoverTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(hovered){
            hoverTime += Time.deltaTime;
            if(hoverTime > .05f){
                hovered = false;
                if(isWhite){
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else{
                    gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                }
                tempCol = gameObject.GetComponent<SpriteRenderer>().color;
                tempCol.a = 1.0f;
                gameObject.GetComponent<SpriteRenderer>().color = tempCol;
                tempVec.z = -1;
                gameObject.transform.position = tempVec;
                hoverTime = 0;
            }
        }
    }

    internal void Hover(bool valid){
        hovered = true;
        hoverTime = 0;
        if(valid){
            gameObject.GetComponent<SpriteRenderer>().color = Color.teal;
            tempCol = gameObject.GetComponent<SpriteRenderer>().color;
            tempCol.a = 0.3f;
            gameObject.GetComponent<SpriteRenderer>().color = tempCol;
        }
        if(!valid){
            gameObject.GetComponent<SpriteRenderer>().color = Color.orange;
            tempCol = gameObject.GetComponent<SpriteRenderer>().color;
            tempCol.a = 0.3f;
            gameObject.GetComponent<SpriteRenderer>().color = tempCol;
        }
        tempVec.z = -8;
        gameObject.transform.position = tempVec;
        Debug.Log(gameObject.transform.position);
    }
}

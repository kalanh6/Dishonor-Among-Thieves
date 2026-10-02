using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    internal GameObject[] tiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        tiles = GameObject.FindGameObjectsWithTag("boardSquare");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerCount;
    public GameObject[] players;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCount = players.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

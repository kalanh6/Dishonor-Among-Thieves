using UnityEngine;

public class BackgroundChange : MonoBehaviour
{
    [SerializeField]
    PlayerManager manager;

    [SerializeField]
    Sprite[] backgrounds = new Sprite[4];

    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = backgrounds[manager.playerIndex];
    }
}

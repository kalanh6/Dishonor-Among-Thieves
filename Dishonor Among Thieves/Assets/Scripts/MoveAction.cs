using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAction : MonoBehaviour
{
    //teleports to the position given in Selected()
    Vector3 nextPosition;

    // states that each player is in based on having
    // hit the Move action button and being moved this turn
    bool isTargeting = false;
    bool hasMoved = false;

    // keeping track of whos turn it is
    [SerializeField]
    TurnManager turnManager;
    [SerializeField]
    PlayerScript playerScript;
    int myPlayerNumber;
    
    private void OnEnable()
    {
        myPlayerNumber = playerScript.playerNumber;
    }

    private void Update()
    {
        // the move button was pressed and now waiting for where the player will move to
        if (isTargeting && Mouse.current.leftButton.isPressed)
        {
            Selected();
        }
    }

    // Specific function for the button to use for onClick
    // makes the Move action selected and will then go up to Update() to call the
    // Selected() function for moving the player
    public void OnClick()
    {
        if(playerScript.isCurrent){
            playerScript.isCurrent = false;
            isTargeting = true;
        }
        // if (turnManager.playerTurn == myPlayerNumber && hasMoved == false)
        // {
        //     isTargeting = true;
        // }

        if (turnManager.playerTurn != myPlayerNumber)
        {
             hasMoved = false;
        }
    }

    // Moves the player if a board square is clicked on by the mouse
    private void Selected()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        RaycastHit hit;
        

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // mouse clicked on a square to move the player to
            if (hit.collider.CompareTag("boardSquare"))
            {
                nextPosition = hit.collider.gameObject.transform.position;
                nextPosition.z = -1;
                transform.position = nextPosition;

                // put the player in the right array index


                isTargeting = false;
                hasMoved = true;
            }
        }
    }

}

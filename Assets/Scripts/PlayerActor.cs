using System.Collections;
using UnityEngine;
using Sorcerer.GameLoop;
using Sorcerer;

public class PlayerActor : AGameLoopActor
{
    private GameManager gameManager;
    private bool isInputEnabled = false;
    private bool alreadyMovedInCurrentTurn = true;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private Entity entity { get { return gameManager.playerEntity; } }

    public override IEnumerator WaitForAction()
    {
        isInputEnabled = true;
        alreadyMovedInCurrentTurn = false;
        while (!alreadyMovedInCurrentTurn)
            yield return null;
        isInputEnabled = false;
    }

    private Vector2Int GetMovementInputVector()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        return new Vector2Int(
            x != 0 ? (int)Mathf.Sign(x) : 0, // 0 if x is 0, -1 if x < 0, 1 if x > 0
            y != 0 ? (int)Mathf.Sign(y) : 0  // 0 if y is 0, -1 if y < 0, 1 if y > 0
        );
    }

    private void FixedUpdate()
    {
        if (entity == null) // player is not ready
            return;
        if (!isInputEnabled || alreadyMovedInCurrentTurn)
            return;
        Vector2Int movementInput = GetMovementInputVector();
        if (movementInput != Vector2Int.zero && entity.AttemptToMoveBy(movementInput))
        {
            Debug.Log("moved by " + movementInput);
            alreadyMovedInCurrentTurn = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sorcerer.GameLoop;
using Sorcerer.Map;
using Sorcerer.UI;

public class PlayerActor : EntityActor
{
    private bool isInputEnabled = false;
    private bool alreadyMovedInCurrentTurn = true;

    private OutputConsole console;

    private void Awake()
    {
        console = GameObject.FindObjectOfType<OutputConsole>();
    }

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
        // player is not ready, not player's turn or player already moved
        if (entity == null || !isInputEnabled || alreadyMovedInCurrentTurn)
            return;
        Vector2Int movementInput = GetMovementInputVector();
        // no input given
        if (movementInput == Vector2Int.zero)
            return;
        ICell nextCell = entity.cell.ConnectionAt(movementInput.ToSorcererDirection());
        // invalid movement direction
        if (nextCell == null || nextCell.isBlockingMovement)
            return;
        Entity enemy = map.FirstEntityAt(nextCell.position, e => e.isBlockingMovement);
        // entities are blocking movement
        if (enemy != null)
            console.Append(entity.name + " attacked " + enemy.name);
        else
        {
            entity.position = nextCell.position;
            console.Append(entity.name + " moved " + movementInput.ToSorcererDirection().ToString());
        }
        alreadyMovedInCurrentTurn = true;
    }
}
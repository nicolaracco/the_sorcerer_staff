using System.Collections;
using UnityEngine;
using Sorcerer.GameLoop;
using Sorcerer.Map;
using Sorcerer.UI;

public class AIActor : EntityActor
{
    private OutputConsole console;

    private void Awake()
    {
        console = GameObject.FindObjectOfType<OutputConsole>();
    }

    private Vector2Int GetMovementInputVector()
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);
        return new Vector2Int(
            x != 0 ? (int)Mathf.Sign(x) : 0, // 0 if x is 0, -1 if x < 0, 1 if x > 0
            y != 0 ? (int)Mathf.Sign(y) : 0  // 0 if y is 0, -1 if y < 0, 1 if y > 0
        );
    }

    public override IEnumerator WaitForAction()
    {
        Vector2Int movementInput;
        ICell nextCell = null;
        while (nextCell == null || nextCell.isBlockingMovement)
        {
            movementInput = GetMovementInputVector();
            if (movementInput != Vector2Int.zero)
                nextCell = entity.cell.ConnectionAt(movementInput.ToSorcererDirection());
        }
        Entity enemy = map.FirstEntityAt(nextCell.position, e => e.isBlockingMovement);
        // entities are blocking movement
        if (enemy != null)
            console.Append(entity.name + " attacks " + enemy.name);
        else
            entity.position = nextCell.position;
        return null;
    }
}
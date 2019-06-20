using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The player controller handles player-specific controls
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// triggered when the player movement has been completed
    /// </summary>
    public UnityEvent OnMovementComplete;

    /// <summary>
    /// true if input is enabled, false otherwise
    /// </summary>
    private bool inputEnabled;

    /// <summary>
    /// A vector representing the current input
    /// </summary>
    /// <value></value>
    private Vector3 movementInputVector
    {
        get
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector3(
                x != 0 ? Mathf.Sign(x) : 0, // 0 if x is 0, -1 if x < 0, 1 if x > 0
                y != 0 ? Mathf.Sign(y) : 0, // 0 if y is 0, -1 if y < 0, 1 if y > 0
                0
            );
        }
    }

    public void OnGameTurnUpdated(GameTurn oldTurn, GameTurn newTurn)
    {
        if (oldTurn == GameTurn.Player)
            inputEnabled = false;
        else if (newTurn == GameTurn.Player)
            inputEnabled = true;
    }

    void FixedUpdate()
    {
        if (!inputEnabled)
            return;
        if (movementInputVector == Vector3.zero)
            return;
        transform.position += movementInputVector;
        inputEnabled = false;
        OnMovementComplete.Invoke();
    }
}

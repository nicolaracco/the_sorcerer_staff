using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The InputManager takes care of gathering any input and translates it to game events
/// </summary>
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Dispatched when a movement input is received
    /// </summary>
    public MovementEvent OnMovementAttempt;

    void FixedUpdate()
    {
        if (HandleExit())
            return;
        HandleMovement();
    }

    /// <summary>
    /// Handle exit keys. Unavailable in WebGL
    /// </summary>
    /// <returns>true if the exit key has been pressed</returns>
    private bool HandleExit()
    {
        #if !UNITY_WEBGL
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit(0);
                return true;
            }
        #endif
        return false;
    }

    /// <summary>
    /// Handle movement input
    /// </summary>
    /// <returns>true if there was some movement input</returns>
    private bool HandleMovement()
    {        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 movementInput = new Vector2(
            x != 0 ? Mathf.Sign(x) : 0, // 0 if x is 0, -1 if x < 0, 1 if x > 0
            y != 0 ? Mathf.Sign(y) : 0  // 0 if y is 0, -1 if y < 0, 1 if y > 0
        );
        if (movementInput == Vector2.zero) // do not trigger useless events
            return false;
	    OnMovementAttempt.Invoke(movementInput);
        return true;
    }
}

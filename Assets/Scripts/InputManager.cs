using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public MovementEvent OnMovementAttempt;

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 movementInput = new Vector2(
            x != 0 ? Mathf.Sign(x) : 0,
            y != 0 ? Mathf.Sign(y) : 0
        );
        if (movementInput != Vector2.zero)
            OnMovementAttempt.Invoke(movementInput);
    }
}

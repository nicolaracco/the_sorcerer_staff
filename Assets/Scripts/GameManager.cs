using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GameManager is the FSM handling the game itself.
/// At the moment it handles the current turn, the turn advancement, the player position on the 
/// screen and takes care of updating it
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Player object
    /// </summary>
    public Transform player;
    /// <summary>
    /// How much time to wait between each turn transition
    /// </summary>
    public float turnDelay = 0.1f;
    
    /// <summary>
    /// Player position on the map
    /// </summary>
    private Vector2 playerPosition = Vector2.zero;
    /// <summary>
    /// Current turn
    /// </summary>
    private GameTurn currentTurn = GameTurn.Waiting;

    void Awake()
    {
        currentTurn = GameTurn.Player;
    }

    void Update()
    {
        player.position = playerPosition;
    }

    /// <summary>
    /// Attempts to move the player.
    /// At the moment this method just checks the game is in the player turn and enqueues the
    /// execution of a coroutine to handle the player movement
    /// </summary>
    /// <param name="delta">delta to apply to the player position</param>
    public void AttemptToMovePlayerBy(Vector2 delta)
    {
        if (currentTurn != GameTurn.Player)
            return;
        currentTurn = GameTurn.Waiting;
        StartCoroutine(MovePlayerBy(delta));
    }

    /// <summary>
    /// This Coroutine takes care of updating the player position.
    /// A Coroutine is needed to wait for some time after the movement, this way we can throttle
    /// if multiple input events are registered at the same time
    /// </summary>
    /// <param name="delta">delta to apply to the player position</param>
    /// <returns></returns>
    private IEnumerator MovePlayerBy(Vector2 delta)
    {
        playerPosition += delta;
        yield return new WaitForSeconds(turnDelay);
        currentTurn = GameTurn.Player;
    }
}

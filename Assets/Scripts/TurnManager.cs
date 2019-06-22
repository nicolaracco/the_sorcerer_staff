using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The GameTurn enum represents a turn during the game
/// </summary>
public enum GameTurn
{
    Waiting, // placeholder turn for waiting
    Player // player's turn
}

/// <summary>
/// The GameTurnUpdated event is triggered when the game turn has been updated, indicating
/// the old turn and the new one
/// </summary>
[System.Serializable]
public class GameTurnUpdatedEvent : UnityEvent<GameTurn, GameTurn>
{
}

/// <summary>
/// The TurnManager is the FSM handling the game itself.
/// At the moment it handles the current turn, the turn advancement, the player position on the 
/// screen and takes care of updating it
/// </summary>
public class TurnManager : MonoBehaviour
{
    /// <summary>
    /// How much time to wait between each turn transition
    /// </summary>
    public float turnDelay = 0.1f;

    public GameTurnUpdatedEvent OnGameTurnUpdate;
    
    /// <summary>
    /// Current turn
    /// </summary>
    private GameTurn currentTurn = GameTurn.Waiting;

    public GameTurn CurrentTurn 
    { 
        get { return currentTurn; } 
        private set
        {
            if (currentTurn == value)
                return;
            GameTurn oldOne = currentTurn;
            currentTurn = value;
            OnGameTurnUpdate.Invoke(oldOne, currentTurn);
        }
    }

    void Awake()
    {
        CurrentTurn = GameTurn.Player;
    }

    /// <summary>
    /// Invoked when the player turn has been completed to move the FSM in the next state
    /// </summary>
    public void OnPlayerTurnCompleted()
    {
        CurrentTurn = GameTurn.Waiting;
        StartCoroutine(PlayerTurnComplete());
    }

    /// <summary>
    /// Waits the turn delay and move the turn back to the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerTurnComplete()
    {
        yield return new WaitForSeconds(turnDelay);
        CurrentTurn = GameTurn.Player;
    }
}

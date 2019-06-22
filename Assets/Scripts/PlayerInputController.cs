using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GameManager))]
public class PlayerInputController : MonoBehaviour
{
    public UnityEvent OnPlayerInputComplete;

    private GameManager gameManager;

    private bool isInputEnabled = false;

    /// <summary>
    /// A vector representing the current input
    /// </summary>
    /// <value></value>
    private Vector2Int movementInput
    {
        get
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector2Int(
                x != 0 ? (int)Mathf.Sign(x) : 0, // 0 if x is 0, -1 if x < 0, 1 if x > 0
                y != 0 ? (int)Mathf.Sign(y) : 0  // 0 if y is 0, -1 if y < 0, 1 if y > 0
            );
        }
    }

    private Entity entity { get { return gameManager.playerEntity; } }

    /// <summary>
    /// Triggered when the game turn changes
    /// It disables the user input if the turn is changing from a Player state
    /// It enables the user input if the turn is changing to a Player state
    /// </summary>
    /// <param name="oldTurn">old game turn</param>
    /// <param name="newTurn">new game turn</param>
    public void OnGameTurnUpdated(GameTurn oldTurn, GameTurn newTurn)
    {
        if (oldTurn == GameTurn.Player)
            isInputEnabled = false;
        else if (newTurn == GameTurn.Player)
            isInputEnabled = true;
    }

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        if (!isInputEnabled || movementInput == Vector2Int.zero)
            return;
        if (entity.AttemptToMoveBy(movementInput))
        {
            isInputEnabled = false;
            OnPlayerInputComplete.Invoke();
        }
    }
}
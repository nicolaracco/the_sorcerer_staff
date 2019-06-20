using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public float turnDelay = 0.2f;
    
    private Vector2 playerPosition = Vector2.zero;
    private GameTurn currentTurn = GameTurn.Waiting;

    void Awake()
    {
        currentTurn = GameTurn.Player;
    }

    void Update()
    {
        player.position = playerPosition;
    }

    public void AttemptToMovePlayerBy(Vector2 delta)
    {
        if (currentTurn != GameTurn.Player)
            return;
        currentTurn = GameTurn.Waiting;
        StartCoroutine(MovePlayerBy(delta));
    }

    private IEnumerator MovePlayerBy(Vector2 delta)
    {
        playerPosition += delta;
        yield return new WaitForSeconds(turnDelay);
        currentTurn = GameTurn.Player;
    }
}

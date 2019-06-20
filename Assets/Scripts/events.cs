using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// MovementEvent is dispatched for movement events and carries the position delta to apply 
/// when moving
/// </summary>
[System.Serializable]
public class MovementEvent : UnityEvent<Vector2>
{
}

/// <summary>
/// The GameTurnUpdated event is triggered when the game turn has been updated, indicating
/// the old turn and the new one
/// </summary>
[System.Serializable]
public class GameTurnUpdatedEvent : UnityEvent<GameTurn, GameTurn>
{
}
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

using UnityEngine;
using Sorcerer;

/// <summary>
/// This CameraController makes sure the player is always in the middle of the screen
/// </summary>
public class CameraController : MonoBehaviour
{
    private Transform cameraTransform;
    private GameManager gameManager;

    private Entity playerEntity { get { return gameManager.playerEntity; } }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void LateUpdate()
    {
        if (playerEntity == null)
            return;
        
        cameraTransform.position = new Vector3(
            playerEntity.position.x, playerEntity.position.y, cameraTransform.position.z
        );
    }
}
using UnityEngine;

/// <summary>
/// This CameraController makes sure the player is always in the middle of the screen
/// </summary>
public class CameraController : MonoBehaviour
{
    public Transform player;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (player == null)
            return;
        
        cameraTransform.position = new Vector3(
            player.position.x, player.position.y, cameraTransform.position.z
        );
    }
}
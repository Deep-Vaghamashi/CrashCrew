using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerCarController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 100f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                input.y += 1f;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1f;

            if (Keyboard.current.aKey.isPressed)
                input.x -= 1f;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1f;
        }

        Vector3 movement = transform.forward * input.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        float turn = input.x * turnSpeed * Time.fixedDeltaTime;
        Quaternion rotation = Quaternion.Euler(0f, turn, 0f);

        rb.MoveRotation(rb.rotation * rotation);
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        StartCoroutine(SetupCameraWhenReady());
    }

    private System.Collections.IEnumerator SetupCameraWhenReady()
    {
        while (Camera.main == null ||
               Camera.main.GetComponent<PlayerCameraFollow>() == null)
        {
            yield return null;
        }

        PlayerCameraFollow cameraFollow =
            Camera.main.GetComponent<PlayerCameraFollow>();

        cameraFollow.SetTarget(transform);
    }
}
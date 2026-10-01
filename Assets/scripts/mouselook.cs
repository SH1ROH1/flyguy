using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCamera : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxRadius = 300f;
    public float smoothTime = 0.35f;
    public float mouseDeadZone = 0.1f;

    [Header("Height")]
    public float scrollSpeed = 50f;
    public float minHeight = 100f;
    public float maxHeight = 600f;

    [Header("Return")]
    public float idleTime = 2f;
    public float returnSpeed = 5f;

    private InputAction lookAction;

    private Vector3 startLocalPosition;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private float targetHeight;
    private float idleTimer;

    void Start()
    {
        startLocalPosition = transform.localPosition;
        targetPosition = startLocalPosition;
        targetHeight = startLocalPosition.y;

        lookAction = InputSystem.actions.FindAction("Player/Look");

        if (lookAction != null)
            lookAction.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (lookAction == null)
            return;

        Vector2 look = lookAction.ReadValue<Vector2>();

        if (look.magnitude > mouseDeadZone)
        {
            idleTimer = 0f;

            Vector3 movement = new Vector3(
                -look.y,
                0f,
                look.x
            );

            targetPosition += movement * moveSpeed;

            Vector3 offset = targetPosition - startLocalPosition;

            Vector2 offsetXZ = new Vector2(
                offset.x,
                offset.z
            );

            float maxRadiusSqr = maxRadius * maxRadius;

            if (offsetXZ.sqrMagnitude > maxRadiusSqr)
            {
                offsetXZ = offsetXZ.normalized * maxRadius;

                offset.x = offsetXZ.x;
                offset.z = offsetXZ.y;

                targetPosition = startLocalPosition + offset;
            }
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTime)
            {
                targetPosition = Vector3.MoveTowards(
                    targetPosition,
                    new Vector3(
                        startLocalPosition.x,
                        targetPosition.y,
                        startLocalPosition.z
                    ),
                    returnSpeed * Time.deltaTime
                );
            }
        }

        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            if (Mathf.Abs(scroll) > 0.01f)
            {
                float direction = Mathf.Sign(scroll);

                targetHeight += direction * scrollSpeed;

                targetHeight = Mathf.Clamp(
                    targetHeight,
                    minHeight,
                    maxHeight
                );
            }
        }

        targetPosition.y = targetHeight;
    }

    void LateUpdate()
    {
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPosition,
            ref currentVelocity,
            smoothTime,
            Mathf.Infinity,
            Time.deltaTime
        );
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
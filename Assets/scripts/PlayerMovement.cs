using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Moving")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            float moveX = 0f;
            float moveZ = 0f;

            if (Keyboard.current.wKey.isPressed) moveX = -1f;
            if (Keyboard.current.sKey.isPressed) moveX = 1f;
            if (Keyboard.current.dKey.isPressed) moveZ = 1f;
            if (Keyboard.current.aKey.isPressed) moveZ = -1f;

            moveInput = new Vector2(moveX, moveZ);
        }

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        if (move.magnitude > 0.1f)
        {
            move = move.normalized;
        }
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

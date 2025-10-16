using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 Movement;
    public static bool JumpWasPressed;
    public static bool JumpIsHeld;
    public static bool JumpWasReleased;
    public static bool AttackWasPressed;
    public static bool MenuOpenInput;
    public static bool MenuCloseInput;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction menuOpenAction;

    private InputAction menuCloseAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        moveAction = PlayerInput.actions["Move"];
        jumpAction = PlayerInput.actions["Jump"];
        attackAction = PlayerInput.actions["Attack"];
        menuOpenAction = PlayerInput.actions["MenuOpen"];

        menuCloseAction = PlayerInput.actions["MenuClose"];
    }

    private void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();

        JumpWasPressed = jumpAction.WasPressedThisFrame();
        JumpIsHeld = jumpAction.IsPressed();
        JumpWasReleased = jumpAction.WasReleasedThisFrame();

        AttackWasPressed = attackAction.WasPressedThisFrame();

        MenuOpenInput = menuOpenAction.WasPressedThisFrame();
        MenuCloseInput = menuCloseAction.WasPressedThisFrame();
    }
}

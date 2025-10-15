using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]

public class MovementStats : ScriptableObject
{
    [Header("Movement")]
    [Range(0f, 1f)] public float MoveThreshold = 0.25f;
    [Range(1f, 100f)] public float MaxSpeed = 8f;
    [Range(0.25f, 50f)] public float GroundAcceleration = 5f;
    [Range(0.25f, 50f)] public float GroundDeceleration = 20f;
    [Range(0.25f, 50f)] public float AirAcceleration = 5f;
    [Range(0.25f, 50f)] public float AirDeceleration = 5f;

    [Header("Grounded/Collision Checks")]
    public LayerMask GroundLayer;
    [Range(-5f, 0f)] public float GroundingForce = -1.5f;
    public float GroundDetectionRayLength = 0.02f;
    public float HeadDetectionRayLength = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = 0.75f;
    public float WallDetectionRayLength = 0.125f;
    [Range(0.01f, 2f)] public float WallDetectionRayHeightMultiplier = 0.9f;

    [Header("Jump")]
    public float JumpHeight = 3f;
    public float TimeTillJumpApex = 0.3f;
    public float MaxFallSpeed = 26f;
    [Range(0, 5)] public int NumberOfJumpsAllowed = 2;

    [Header("Fast Fall")]
    public bool DoFastFall = true;
    [Range(0.01f, 5f)] public float FastFallMultiplier = 1.5f;
    public bool DoWallJumpFastFall = true;
    [Range(0.01f, 5f)] public float WallJumpFastFallMultiplier = 1.5f;

    [Header("Reset Jump Options")]
    public bool ResetJumpsOnWallSlide = false;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)] public float ApexThreshold = 0.97f;
    [Range(0.01f, 1f)] public float ApexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float JumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)] public float JumpCoyoteTime = 0.1f;

    [Header("Wall Slide")]
    [Min(0.01f)] public float WallSlideSpeed = 5f;
    [Range(0.25f, 50f)] public float WallSlideDecelerationSpeed = 50f;

    [Header("WallJump")]
    public Vector2 WallJumpDirection = new Vector2(-20f, 6.5f);
    [Range(0f, 1f)] public float WallJumpBufferTime = 0.125f;

    [Header("Debug")]
    public bool DebugShowIsGroundedBox;
    public bool DebugShowHeadBumpBox;
    public bool DebugShowWallHitBox;

    //Jump
    public float Gravity { get; private set; }
    public float JumpVelocity { get; private set; }

    //Wall Jump
    public float WallJumpGravity { get; private set; }
    public float WallJumpVelocity { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        //Jump
        Gravity = -(2f * JumpHeight) / Mathf.Pow(TimeTillJumpApex, 2f);
        JumpVelocity = Mathf.Abs(Gravity) * TimeTillJumpApex;

        //Wall Jump
        WallJumpGravity = -(2f * WallJumpDirection.y) / Mathf.Pow(TimeTillJumpApex, 2f);
        WallJumpVelocity = Mathf.Abs(WallJumpGravity) * TimeTillJumpApex;
    }
}

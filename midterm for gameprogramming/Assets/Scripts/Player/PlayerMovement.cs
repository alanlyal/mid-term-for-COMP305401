using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public MovementStats MoveStats;
    [SerializeField] private Collider2D bodyColl;
    [SerializeField] private Collider2D feetColl;

    private Rigidbody2D rb;

    private Animator animator;

    //movement vars
    public float HorizontalVelocity { get; private set; }
    private bool isFacingRight;

    //collision check vars
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private RaycastHit2D wallHit;
    private RaycastHit2D lastWallHit;
    private bool isGrounded;
    private bool bumpedHead;
    private bool isTouchingWall;

    //jump vars
    public float VerticalVelocity { get; private set; }
    public bool isJumping;
    public bool isFalling;
    public int numOfJumpsUsed;

    //apex vars
    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    //jump buffer vars
    private float jumpBufferTimer;

    //coyote time vars
    private float coyoteTimer;

    //wall slide vars
    private bool isWallSliding;

    //wall jump vars
    private bool isWallJumping;

    private float wallJumpBufferTimer;
    private float wallJumpCooldownTimer;

    private float wallJumpApexPoint;
    private float timePastWallJumpApexThreshold;
    private bool isPastWallJumpApexThreshold;

    private void Awake()
    {
        isFacingRight = true;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();
        LandCheck();
        WallSlideCheck(InputManager.Movement);
        WallJumpChecks();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();
        Fall();
        WallSlide();
        WallJump();

        //Grounded Movement
        if (isGrounded)
        {
            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
        }
        //Airborne Movement
        else
        {
            Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
        }

        ApplyVelocity();
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("landed", isGrounded);
        animator.SetBool("isWalking", (InputManager.Movement.x != 0));
    }

    private void ApplyVelocity()
    {
        //CLAMP FALL SPEED
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);

        rb.velocity = new Vector2(HorizontalVelocity, VerticalVelocity);
    }

    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (Mathf.Abs(moveInput.x) >= MoveStats.MoveThreshold)
        {
            TurnCheck(moveInput);

            float targetVelocity = 0f;
            targetVelocity = moveInput.x * MoveStats.MaxSpeed;

            HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else if (Mathf.Abs(moveInput.x) < MoveStats.MoveThreshold)
        {
            HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, 0f, deceleration * Time.fixedDeltaTime);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }

        else if (!isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Land/Fall

    private void LandCheck()
    {
        if (isGrounded && VerticalVelocity <= 0f)
        {
            ResetJumpValues();
            ResetWallJumpValues();
            isWallSliding = false;

            numOfJumpsUsed = 0;

            VerticalVelocity = MoveStats.GroundingForce;
        }
    }

    private void Fall()
    {
        if (!isGrounded && !isWallSliding)
        {
            if (!isFalling && VerticalVelocity < 0f)
            {
                isFalling = true;
            }

            if (!isPastApexThreshold && !isPastWallJumpApexThreshold)
            {
                VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
            }
        }
    }

    #endregion

    #region Jump

    private void ResetJumpValues()
    {
        isJumping = false;
        isFalling = false;
        isPastApexThreshold = false;
    }

    private void JumpChecks()
    {
        if (InputManager.JumpWasPressed)
        {
            jumpBufferTimer = MoveStats.JumpBufferTime;
        }

        if (InputManager.JumpWasReleased)
        {
            if (isJumping && VerticalVelocity > 0f)
            {
                if (!isPastApexThreshold)
                {
                    VerticalVelocity = 0f;
                }
            }
        }

        //INITIATE JUMP WITH JUMP BUFFERING AND COYOTE TIME
        if (jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);
        }

        //DOUBLE JUMP
        else if (jumpBufferTimer > 0f && (isJumping || isWallJumping) && !isTouchingWall && wallJumpCooldownTimer <= 0f && numOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
        {
            if (isPastApexThreshold)
            {
                isPastApexThreshold = false;
            }
            VerticalVelocity = 0f;
            InitiateJump(1);
        }

        //AIR JUMP AFTER COYOTE TIME
        else if (jumpBufferTimer > 0f && isFalling && numOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
        {
            InitiateJump(2);
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!isJumping)
        {
            isJumping = true;
        }

        if (SoundEffectManager.Instance != null)
        {
            SoundEffectManager.Play("Jump");
        }

        animator.SetTrigger("jump");

        ResetWallJumpValues();

        jumpBufferTimer = 0f;
        numOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = MoveStats.JumpVelocity;
    }

    private void Jump()
    {
        //APPLY GRAVITY WHILE JUMPING
        if (isJumping)
        {
            if (VerticalVelocity >= 0f)
            {
                //APEX CONTROLS
                apexPoint = Mathf.InverseLerp(MoveStats.JumpVelocity, 0f, VerticalVelocity);

                if (apexPoint > MoveStats.ApexThreshold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if (timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }
            }

            //GRAVITY ON DECENT
            else if (isPastApexThreshold)
            {
                if (MoveStats.DoFastFall)
                {
                    VerticalVelocity += MoveStats.Gravity * MoveStats.FastFallMultiplier * Time.fixedDeltaTime;
                }
                else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                }
            }
        }
    }

    #endregion

    #region Wall Slide

    private void WallSlideCheck(Vector2 moveInput)
    {
        if (isTouchingWall && !isGrounded && (Mathf.Abs(moveInput.x) >= MoveStats.MoveThreshold))
        {
            if (VerticalVelocity < 0f && !isWallSliding)
            {
                ResetJumpValues();
                ResetWallJumpValues();

                isWallSliding = true;

                if (MoveStats.ResetJumpsOnWallSlide)
                {
                    numOfJumpsUsed = 0;
                }
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallSlide()
    {
        if (isWallSliding)
        {
            VerticalVelocity = Mathf.Lerp(VerticalVelocity, -MoveStats.WallSlideSpeed, MoveStats.WallSlideDecelerationSpeed * Time.fixedDeltaTime);
        }
    }

    #endregion

    #region Wall Jump

    private void ResetWallJumpValues()
    {
        isWallJumping = false;
        isPastWallJumpApexThreshold = false;
    }

    private void WallJumpChecks()
    {
        if (ShouldApplyWallJumpBuffer())
        {
            wallJumpBufferTimer = MoveStats.WallJumpBufferTime;
        }

        if (InputManager.JumpWasPressed && wallJumpBufferTimer > 0f)
        {
            InitiateWallJump();
        }

        if (InputManager.JumpWasReleased)
        {
            if (isWallJumping && VerticalVelocity > 0f)
            {
                if (!isPastWallJumpApexThreshold)
                {
                    VerticalVelocity = 0f;
                }
            }
        }
    }

    private void InitiateWallJump()
    {
        if (!isWallJumping)
        {
            isWallJumping = true;
        }


        if (SoundEffectManager.Instance != null)
        {
            SoundEffectManager.Play("Jump");
        }

        animator.SetTrigger("jump");

        isWallSliding = false;
        ResetJumpValues();

        wallJumpCooldownTimer = 0.2f;

        VerticalVelocity = MoveStats.WallJumpVelocity;

        int dirMultiplier = 0;
        Vector2 hitPoint = lastWallHit.collider.ClosestPoint(bodyColl.bounds.center);

        if (hitPoint.x > transform.position.x) { dirMultiplier = -1; }
        else { dirMultiplier = 1; }

        HorizontalVelocity = Mathf.Abs(MoveStats.WallJumpDirection.x) * dirMultiplier;
    }

    private void WallJump()
    {
        //APPLY GRAVITY WHILE JUMPING
        if (isWallJumping)
        {
            if (VerticalVelocity >= 0f)
            {
                //APEX CONTROLS
                wallJumpApexPoint = Mathf.InverseLerp(MoveStats.WallJumpDirection.y, 0f, VerticalVelocity);

                if (wallJumpApexPoint > MoveStats.ApexThreshold)
                {
                    if (!isPastWallJumpApexThreshold)
                    {
                        isPastWallJumpApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastWallJumpApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if (timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }
            }

            //GRAVITY ON DECENT
            else if (isPastWallJumpApexThreshold)
            {
                if (MoveStats.DoWallJumpFastFall)
                {
                    VerticalVelocity += MoveStats.Gravity * MoveStats.WallJumpFastFallMultiplier * Time.fixedDeltaTime;
                }
                else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                }
            }
        }
    }

    private bool ShouldApplyWallJumpBuffer()
    {
        if (!isGrounded && (isTouchingWall || isWallSliding)) { return true; }
        else { return false; }
    }

    #endregion

    #region Collision Checks

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feetColl.bounds.center.x, feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);
        if (groundHit.collider != null) { isGrounded = true; }
        else { isGrounded = false; }

        #region Debug Visualization
        if (MoveStats.DebugShowIsGroundedBox)
        {
            Color rayColor;
            if (isGrounded)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(feetColl.bounds.center.x, bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);
        if (headHit.collider != null) { bumpedHead = true; }
        else { bumpedHead = false; }

        #region Debug Visualization

        if (MoveStats.DebugShowHeadBumpBox)
        {
            float headWidth = MoveStats.HeadWidth;

            Color rayColor;
            if (bumpedHead)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
        }

        #endregion
    }

    private void IsTouchingWall()
    {
        float originEndPoint = 0f;
        if (isFacingRight) { originEndPoint = bodyColl.bounds.max.x; }
        else { originEndPoint = bodyColl.bounds.min.x; }

        float adjustedHeight = bodyColl.bounds.size.y * MoveStats.WallDetectionRayHeightMultiplier;

        Vector2 boxCastOrigin = new Vector2(originEndPoint, bodyColl.bounds.center.y);
        Vector2 boxCastSize = new Vector2(MoveStats.WallDetectionRayLength, adjustedHeight);

        wallHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, transform.right, 0f, MoveStats.GroundLayer);
        if (wallHit.collider != null)
        {
            lastWallHit = wallHit;
            isTouchingWall = true;
        }
        else { isTouchingWall = false; }

        #region Debug Visualization

        if (MoveStats.DebugShowWallHitBox)
        {
            Color rayColor;
            if (isTouchingWall)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Vector2 boxBottomLeft = new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - boxCastSize.y / 2);
            Vector2 boxBottomRight = new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y - boxCastSize.y / 2);
            Vector2 boxTopLeft = new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y + boxCastSize.y / 2);
            Vector2 boxTopRight = new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y + boxCastSize.y / 2);

            Debug.DrawLine(boxBottomLeft, boxBottomRight, rayColor);
            Debug.DrawLine(boxBottomRight, boxTopRight, rayColor);
            Debug.DrawLine(boxTopRight, boxTopLeft, rayColor);
            Debug.DrawLine(boxTopLeft, boxBottomLeft, rayColor);
        }

        #endregion
    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
        IsTouchingWall();

        //CHECK FOR HEAD BUMP
        if (bumpedHead)
        {
            VerticalVelocity = Mathf.Lerp(VerticalVelocity, 0f, 0.75f);
            isJumping = false;
            isWallJumping = false;
            isPastApexThreshold = false;
        }
    }

    #endregion

    #region Timers

    private void CountTimers()
    {
        //JUMP BUFFER
        jumpBufferTimer -= Time.deltaTime;

        //JUMP COYOTE TIME
        if (!isGrounded) { coyoteTimer -= Time.deltaTime; }
        else { coyoteTimer = MoveStats.JumpCoyoteTime; }

        //WALL JUMP BUFFER
        if (!ShouldApplyWallJumpBuffer())
        {
            wallJumpBufferTimer -= Time.deltaTime;
        }

        //WALL JUMP COOLDOWN
        wallJumpCooldownTimer -= Time.deltaTime;
    }

    #endregion
}

using UnityEngine;
using UnityEngine.Events;

public class PlayerMotor : MonoBehaviour
{
    public UnityEvent onLanded = new UnityEvent();
    public UnityEvent onJumped = new UnityEvent();

	[Header("General")]
	[SerializeField]
	float maxSpeedMultiple = 2.5f;

	[SerializeField]
	[Range(0f, 1f)]
	float momentumShiftWhenTurningFactor = 0.85f;

	[Header("Grounded")]
	[SerializeField]
	float groundAccelerationRate = 150f;

	[SerializeField]
	float groundDecelerationRate = 60f;

	[SerializeField]
	float groundFrictionStrength = 1.3f;

	[SerializeField]
	float groundTargetMovementSpeed = 11f;

	[Header("Airborne")]
	[SerializeField]
	float airAccelerationRate = 120f;

	[SerializeField]
	float airDecelerationRate = 0f;

	[SerializeField]
	float airFrictionStrength = 0.9f;

	[SerializeField]
	float airTargetMovementSpeed = 10f;

	[Header("Vertical Forces")]
	[SerializeField]
	float jumpHeight = 2.3f;

	[SerializeField]
	float gravityStrength = 35f;

	[SerializeField]
	float terminalDownVelocity = -40f;

	const float GROUNDED_VELOCITY_Y = -2f;

    const float JUMP_COOLDOWN_TIME = 0.1f;

	const float SLOPE_RIDE_DISTANCE_LIMIT = 3f;

	const float SLOPE_RIDE_DOWNWARDS_FORCE_STRENGTH = 35f;

    const float FRICTIONLESS_TIME_AFTER_LANDING = 0.2f;

	const float DEADZONE_THRESHOLD = 0.2f;

	Transform           m_moveTarget;
    CharacterController m_characterController;

    GrappleHook m_grapplingHook;

    float m_groundedTimeElapsed = 0f;

	bool m_isGrounded  = true;
    bool m_isGrappling = false;

	float m_lastTimeJumpPerformed = -999f;

	float m_currentAccelerationRate;
	float m_currentDecelerationRate;
	float m_currentFrictionStrength;
	float m_currentTargetMovementSpeed;
	float m_currentMaxSpeed;

	float m_jumpVelocityY;

	Vector3 m_lastFrameForwardDir;
	Vector3 m_velocity;

	public Vector3 Velocity
	{
		get => m_velocity;
		set => m_velocity = value;
	}

    public bool IsGrounded
	{
		get => m_isGrounded;
	}

	public float GravityStrength
	{
		get => gravityStrength;
	}

	public float JumpHeight
	{
		get => jumpHeight;
		set
		{
			UpdateJumpVelocityY(value);
			jumpHeight = value;
		}
	}

	public float GetGroundedVelocityY()
	{
		return GROUNDED_VELOCITY_Y;
	}

	public void Teleport(Vector3 a_newPosition)
	{
		m_characterController.enabled = false;

		m_moveTarget.position = a_newPosition;

		m_characterController.enabled = true;
	}

	public void ResetVelocity()
	{
		m_velocity = Vector3.zero;
	}

	void UpdateJumpVelocityY(float a_newJumpHeight)
	{
		m_jumpVelocityY =
			Mathf.Sqrt(
				a_newJumpHeight * 2f * gravityStrength
			); //calculate velocity needed in order to reach desired jump height
	}

	void Awake()
	{
		m_moveTarget          = transform;
        m_characterController = GetComponent<CharacterController>();
        m_grapplingHook       = GetComponent<GrappleHook>();
	}

	void Start()
	{
		UpdateJumpVelocityY(jumpHeight);
		m_lastFrameForwardDir = m_moveTarget.forward;

		m_currentMaxSpeed = Mathf.Max(groundTargetMovementSpeed, airTargetMovementSpeed) * maxSpeedMultiple;
	}

	public void Move(in Vector2 a_inputAxes, bool a_jump)
	{
		PerformGroundCheck();

		float verticalAxis   = a_inputAxes.y;
        float horizontalAxis = a_inputAxes.x;

        Vector3 moveDirX = m_moveTarget.right   * horizontalAxis;
		Vector3 moveDirY = m_moveTarget.forward * verticalAxis;

        Vector3 constraintDisplacement = Vector3.zero;

        m_isGrappling = m_grapplingHook.IsGrappling;

        if (m_isGrappling)
        {
            m_grapplingHook.UpdateGrappling(ref constraintDisplacement, ref m_velocity, moveDirX + moveDirY);
        }
        else
        {
            UpdateCurrentMovementVariables();

            if (verticalAxis > DEADZONE_THRESHOLD)
            {
                RotateVelocityByLookChange();
            }

            ApplyFriction();

            ProcessBasicMovement(moveDirX, moveDirY);

            ApplyDeceleration(verticalAxis, horizontalAxis);
		}

        m_velocity.y -= gravityStrength * Time.deltaTime; //apply gravity

		//if the player is grounded, downwards velocity should be reset
		if (m_isGrounded && m_velocity.y < 0f && !m_isGrappling)
		{
			m_velocity.y = GROUNDED_VELOCITY_Y;
		}

		if (a_jump)
		{
			TryJump(a_inputAxes);
		}

		m_velocity.y = Mathf.Max(m_velocity.y, terminalDownVelocity);

		ApplySpeedCap();

		m_characterController.Move(m_velocity * Time.deltaTime + constraintDisplacement);

		//after standard movement stuff is done, check if the player should be glued to a slope
		PerformOnSlopeLogic();

		m_lastFrameForwardDir = m_moveTarget.forward;
	}

	void PerformGroundCheck()
    {
        bool wasGrounded = m_isGrounded;

		m_isGrounded = m_characterController.isGrounded;

        if (m_isGrounded)
        {
            if (wasGrounded)
            {
                m_groundedTimeElapsed += Time.deltaTime;
            }
            else
            {
                onLanded?.Invoke();
			}
        }
        else
        {
            m_groundedTimeElapsed = 0f;
        }
	}

	void UpdateCurrentMovementVariables()
	{
		if (m_isGrounded)
		{
			m_currentAccelerationRate = groundAccelerationRate;
			m_currentDecelerationRate = groundDecelerationRate;
			m_currentFrictionStrength = groundFrictionStrength;
			m_currentTargetMovementSpeed = groundTargetMovementSpeed;
		}
		else
		{
			m_currentAccelerationRate = airAccelerationRate;
			m_currentDecelerationRate = airDecelerationRate;
			m_currentFrictionStrength = airFrictionStrength;
			m_currentTargetMovementSpeed = airTargetMovementSpeed;
		}
	}

	void RotateVelocityByLookChange()
	{
		Quaternion deltaRotation = Quaternion.FromToRotation(m_lastFrameForwardDir, m_moveTarget.forward);

		Quaternion scaledDeltaRotation = Quaternion.Lerp(
			Quaternion.identity,
			deltaRotation,
			momentumShiftWhenTurningFactor
		);

		m_velocity = scaledDeltaRotation * m_velocity;
	}

	void ProcessBasicMovement(Vector3 a_moveDirX, Vector3 a_moveDirY)
	{
		Vector3 desiredDirection = a_moveDirX + a_moveDirY;
		desiredDirection.y = 0f;

		Vector3 currentVelocityXZ = m_velocity;
		currentVelocityXZ.y = 0f;

		float currentSpeedXZ = currentVelocityXZ.magnitude;

		if (currentSpeedXZ >= m_currentTargetMovementSpeed)
		{
			Vector3 newVelocityXZ = currentVelocityXZ + desiredDirection * m_currentAccelerationRate * Time.deltaTime;
			newVelocityXZ = Vector3.ClampMagnitude(newVelocityXZ, currentSpeedXZ);

			m_velocity.x = newVelocityXZ.x;
			m_velocity.z = newVelocityXZ.z;

			return;
		}

		m_velocity += desiredDirection * m_currentAccelerationRate * Time.deltaTime;
	}

	void ApplyDeceleration(float a_verticalAxis, float a_horizontalAxis)
	{
		float frameDecelerationAmount = m_currentDecelerationRate * Time.deltaTime;

		Vector3 localVelocity = m_moveTarget.InverseTransformDirection(m_velocity);

		//apply deceleration if the axis isn't being moved on
		if (Mathf.Abs(a_verticalAxis) <= DEADZONE_THRESHOLD)
		{
			if (Mathf.Abs(localVelocity.z) > frameDecelerationAmount)
			{
				m_velocity -= m_moveTarget.forward * Mathf.Sign(localVelocity.z) * frameDecelerationAmount;
			}
			else
			{
				m_velocity -= m_moveTarget.forward * localVelocity.z;
			}
		}

		if (Mathf.Abs(a_horizontalAxis) <= DEADZONE_THRESHOLD)
		{
			if (Mathf.Abs(localVelocity.x) > frameDecelerationAmount)
			{
				m_velocity -= m_moveTarget.right * Mathf.Sign(localVelocity.x) * frameDecelerationAmount;
			}
			else
			{
				m_velocity -= m_moveTarget.right * localVelocity.x;
			}
		}
	}

	void ApplyFriction()
	{
        if (m_isGrounded && m_groundedTimeElapsed < FRICTIONLESS_TIME_AFTER_LANDING)
        {
            return;
        }

		Vector3 newVelocityXZ = m_velocity;
		newVelocityXZ.y = 0f;

		float currentSpeed = newVelocityXZ.magnitude;
		if (!Mathf.Approximately(currentSpeed, 0f))
		{
			float frictionAmount = currentSpeed * m_currentFrictionStrength * Time.deltaTime;

			//scale the XZ velocity based on friction
			newVelocityXZ *= Mathf.Max(currentSpeed - frictionAmount, 0f) / currentSpeed;

			m_velocity = new Vector3(newVelocityXZ.x, m_velocity.y, newVelocityXZ.z);
		}
	}

	void TryJump(in Vector2 a_inputAxes)
	{
		if ((!m_isGrounded && !m_isGrappling) || m_lastTimeJumpPerformed + JUMP_COOLDOWN_TIME > Time.time)
		{
			return;
		}

        if (m_isGrappling)
        {
            m_grapplingHook.PerformGrappleJump(ref m_velocity, a_inputAxes);

            m_isGrappling = false;
        }
        else
		{
			m_velocity.y = m_jumpVelocityY;
		}

        m_lastTimeJumpPerformed = Time.time;

		onJumped?.Invoke();
	}

    //this function should be called AFTER standard movement is applied for the current update
	void PerformOnSlopeLogic()
	{
		//calculate new isGrounded since player movement was just updated
        bool wasGrounded   = m_isGrounded;
		bool newIsGrounded = m_characterController.isGrounded;

		//glue the player to the slope if they're moving down one (fixes bouncing when going down slopes)
		if (!newIsGrounded && wasGrounded && m_velocity.y < 0f)
		{
			Vector3 pointAtBottomOfPlayer =
				m_moveTarget.position - (Vector3.down * (m_characterController.height * 0.5f));

			if (Physics.Raycast(
				pointAtBottomOfPlayer,
				Vector3.down,
				out _,
				SLOPE_RIDE_DISTANCE_LIMIT))
			{
				m_characterController.Move(Vector3.down * SLOPE_RIDE_DOWNWARDS_FORCE_STRENGTH * Time.deltaTime);
			}
		}
	}

	void ApplySpeedCap()
	{
		Vector3 currentVelocityXZ = m_velocity;
		currentVelocityXZ.y = 0f;

		if (currentVelocityXZ.sqrMagnitude < m_currentMaxSpeed * m_currentMaxSpeed)
		{
			return;
		}

		currentVelocityXZ = currentVelocityXZ.normalized * m_currentMaxSpeed;

		m_velocity = new Vector3(currentVelocityXZ.x, m_velocity.y, currentVelocityXZ.z);
	}
}

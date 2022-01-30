using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrappleHook : MonoBehaviour
{
    public UnityEvent onGrappleStarted = new UnityEvent();

    [SerializeField]
    ParticleSystem grappleHitParticles = null;

    [SerializeField]
    float grappleCastRadius = 0.1f;

    [SerializeField]
    float maxLength = 20f;

    [SerializeField]
    float slackAdjustmentSpeed = 7f;

    [SerializeField]
    float grappleJumpHeight = 1.5f;

    [SerializeField]
    Transform grappleMuzzle = null;

    [SerializeField]
    LayerMask grapplableLayers;

    [SerializeField]
    LayerMask grappleBlockingLayers;

    [SerializeField]
    float grappleMoveSpeedInfluence = 1f;

    [Header("Initial Speed Boost")]
    [SerializeField]
    float speedBoostStrength = 3f;

    [SerializeField]
    float speedBoostDuration = 0.5f;

    [Header("Slow Down Over Time")]
    [SerializeField]
    float slowDownStrength = 0.5f;

    [Header("Grapple Rope")]
    [SerializeField]
    LineRenderer ropeLine = null;

    const float MIN_LENGTH            = 3f;
    const float GRAPPLE_JUMP_COOLDOWN = 0.4f;

    Transform m_playerTransform;

    bool m_isGrappling;

    float      m_currentLength;
    RaycastHit m_grappleRayHit;
    bool       m_isPossibleLatchPointValid;

    Vector3 m_tetherPoint;

    float m_grappleJumpVelocityY;

    float m_lastTimeGrappling        = Mathf.NegativeInfinity;
    float m_lastTimeGrapplingStarted = Mathf.NegativeInfinity;

    List<float> m_recentGrappleJumpTimes = new List<float>();

    float m_recentTimeThreshold = 1.5f;

    public bool IsGrappling
    {
        get => m_isGrappling;
    }

    public float LastTimeGrappling
    {
        get => m_lastTimeGrappling;
    }

    public Vector3 TetherPoint
    {
        get => m_tetherPoint;
    }

    void Awake()
    {
        m_playerTransform = FirstPersonPlayerData.Instance.BodyTransform;
    }

    void Start()
    {
        UpdateGrappleJumpHeight();
    }

    void LateUpdate()
    {
        if (!m_isGrappling)
        {
            return;
        }

        DrawRope();

        float newLength = Vector3.Distance(m_playerTransform.position, m_tetherPoint);
        if (newLength < m_currentLength && newLength > MIN_LENGTH)
        {
            m_currentLength = newLength;
        }
    }

    public void UpdateGrappleJumpHeight()
    {
        //calculate velocity needed in order to reach desired jump height
        m_grappleJumpVelocityY = Mathf.Sqrt(grappleJumpHeight * 2f * FirstPersonPlayerData.Instance.GravityStrength);
    }

    public bool HasValidGrapplePoint(Vector3 a_startPosition, Vector3 a_direction)
    {
        {
            if (Physics.Raycast(
                    a_startPosition,
                    a_direction,
                    out var grappleRayHit,
                    maxLength,
                    grapplableLayers.value + grappleBlockingLayers.value,
                    QueryTriggerInteraction.Ignore)
                && IsValidLayer(grappleRayHit.transform.gameObject.layer))
            {
                m_grappleRayHit             = grappleRayHit;
                m_isPossibleLatchPointValid = true;
                return true;
            }
        }

        {
            if (Physics.SphereCast(
                    a_startPosition,
                    grappleCastRadius,
                    a_direction,
                    out var grappleSphereHit,
                    maxLength,
                    grapplableLayers.value + grappleBlockingLayers.value,
                    QueryTriggerInteraction.Ignore)
                && IsValidLayer(grappleSphereHit.transform.gameObject.layer))
            {
                m_grappleRayHit             = grappleSphereHit;
                m_isPossibleLatchPointValid = true;
                return true;
            }
        }

        m_isPossibleLatchPointValid = false;
        return false;
    }

    bool IsValidLayer(int a_layerToTest)
    {
        return grapplableLayers == (grapplableLayers | (1 << a_layerToTest));
    }

    public void FireGrapple(Vector3 a_startPosition, Vector3 a_direction)
    {
        if (m_isGrappling)
        {
            DetachGrapple();
        }

        if (!HasValidGrapplePoint(a_startPosition, a_direction))
        {
            return;
        }

        onGrappleStarted?.Invoke();

        Vector3 playerPosition = m_playerTransform.position;

        m_currentLength = Vector3.Distance(playerPosition, m_grappleRayHit.point);
        m_currentLength = Mathf.Max(m_currentLength, MIN_LENGTH);

        m_isGrappling = true;
        m_tetherPoint = m_grappleRayHit.point;

        m_lastTimeGrapplingStarted = Time.time;

        ropeLine.enabled = true;
        DrawRope();

        var newParticles = Instantiate(grappleHitParticles, m_tetherPoint, Quaternion.LookRotation(m_grappleRayHit.normal));

        Color hitColor = m_grappleRayHit.collider.gameObject.GetComponent<Renderer>().material.GetColor("_LineColor");
        newParticles.GetComponent<Renderer>().material.SetColor("_LineColor", hitColor);
        Destroy(newParticles.gameObject, 1.5f);
    }

    public void DetachGrapple()
    {
        m_isGrappling    = false;
        ropeLine.enabled = false;
    }

    public void AddSlack()
    {
        if (m_currentLength >= maxLength)
        {
            return;
        }
        m_currentLength += slackAdjustmentSpeed * Time.deltaTime;

        if (m_currentLength > maxLength)
        {
            m_currentLength = maxLength;
        }
    }

    public void ReduceSlack()
    {
        if (m_currentLength <= MIN_LENGTH)
        {
            return;
        }
        m_currentLength -= slackAdjustmentSpeed * Time.deltaTime;

        if (m_currentLength < MIN_LENGTH)
        {
            m_currentLength = MIN_LENGTH;
        }
    }

    public void UpdateGrappling(
        ref Vector3 a_constraintDisplacement,
        ref Vector3 a_playerVelocity,
        Vector3     a_inputMoveDir)
    {
        if (!m_isGrappling)
        {
            return;
        }

        float speedToAdd = 0f;
        if (m_lastTimeGrapplingStarted + speedBoostDuration >= Time.time)
        {
            speedToAdd = speedBoostStrength * Time.deltaTime;
        }
        else
        {
            speedToAdd = -slowDownStrength * Time.deltaTime;
        }

        a_playerVelocity = a_playerVelocity.normalized * (a_playerVelocity.magnitude + speedToAdd);

        Vector3 playerPosition   = m_playerTransform.position;
        Vector3 grappleDirection = (m_tetherPoint - playerPosition);

        if (m_currentLength < grappleDirection.magnitude)
        {
            Vector3 newPlayerPosition = Vector3.Normalize(playerPosition - m_tetherPoint) * m_currentLength + m_tetherPoint;

            a_constraintDisplacement = newPlayerPosition - playerPosition;

            Vector3 newDirection = Vector3.ProjectOnPlane(a_playerVelocity, grappleDirection);

            a_playerVelocity = newDirection.normalized * a_playerVelocity.magnitude;
        }

        //player input move direction influence on grapple velocity (only for inputs perpendicular to current velocity)
        Vector3 desiredMovePerpendicular;

        Vector3 clockwise = new Vector3(a_playerVelocity.z, 0, -a_playerVelocity.x);

        if (Vector3.Dot(a_inputMoveDir, clockwise) > 0f)
        {
            desiredMovePerpendicular = clockwise;
        }
        else
        {
            //counter clockwise
            desiredMovePerpendicular = new Vector3(-a_playerVelocity.z, 0, a_playerVelocity.x);
        }

        float moveFactor = Vector3.Dot(a_inputMoveDir.normalized, desiredMovePerpendicular.normalized);
        moveFactor = Mathf.Max(0f, moveFactor);

        a_playerVelocity += desiredMovePerpendicular * moveFactor * grappleMoveSpeedInfluence * Time.deltaTime;

        m_lastTimeGrappling = Time.time;
    }

    void DrawRope()
    {
        ropeLine.SetPosition(0, grappleMuzzle.position);
        ropeLine.SetPosition(1, m_tetherPoint);
    }

    public void PerformGrappleJump(ref Vector3 a_velocity, in Vector2 a_inputAxes)
    {
        DetachGrapple();

        m_recentGrappleJumpTimes.Add(Time.time);

        for (int i = 0; i < m_recentGrappleJumpTimes.Count; i++)
        {
            if (m_recentGrappleJumpTimes[i] + m_recentTimeThreshold < Time.time)
            {
                m_recentGrappleJumpTimes.RemoveAt(i);
                i--;
            }
        }

        float jumpVelocityDivideAmount = Mathf.Max(1f, m_recentGrappleJumpTimes.Count - 2);

        if (jumpVelocityDivideAmount > 2f)
        {
            return;
        }

        if (a_velocity.y < 0f)
        {
            a_velocity.y = 0f;
        }

        if (m_recentGrappleJumpTimes.Count > 2)
        {
            a_velocity.y = m_grappleJumpVelocityY / jumpVelocityDivideAmount;
        }
        else
        {
            a_velocity.y += m_grappleJumpVelocityY / jumpVelocityDivideAmount;
        }
    }
}

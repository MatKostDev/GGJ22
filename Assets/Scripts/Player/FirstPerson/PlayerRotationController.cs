using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
	[SerializeField]
	Transform yawTransform = null;

	[SerializeField]
	Transform tiltTransform = null;
	
	[SerializeField]
	float pitchLimit = 80f;

	float m_yaw;
	float m_pitch;
	float m_roll;

	public float Yaw
	{
		get => m_yaw;
		set => m_yaw = value;
	}

	public float Pitch
	{
		get => m_pitch;
		set => m_pitch = Mathf.Clamp(value, -pitchLimit, pitchLimit);
	}

	public float Roll
	{
		get => m_roll;
		set => m_roll = value;
	}

	public Vector3 EulerRotation
	{
		get => new Vector3(m_pitch, m_yaw, m_roll);
		set
		{
			Pitch = value.x;
			Yaw   = value.y;
			Roll  = value.z;
		}
	}

	void Start()
	{
		Yaw   = yawTransform.localEulerAngles.y;
		Pitch = tiltTransform.localEulerAngles.x;
		Roll  = tiltTransform.localEulerAngles.z;
	}

	public void UpdateRotations(Vector2 a_lookAxis)
	{
		Pitch -= a_lookAxis.y;
		Yaw   += a_lookAxis.x;

		yawTransform.localRotation = Quaternion.AngleAxis(m_yaw, Vector3.up);

		tiltTransform.localRotation =
			Quaternion.AngleAxis(m_pitch, Vector3.right) * Quaternion.AngleAxis(m_roll, Vector3.forward);
	}
}

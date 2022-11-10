using UnityEngine;

public class AIStateDetectPlayer : AIState
{
	private bool m_notifyDetected;

	private bool m_notifyNotDetected;

	private float m_activeRadius;

	private float m_activeAngle;

	private float m_passiveRadius;

	private float m_checkTime;

	public AIStateDetectPlayer(bool notifyDetected, bool notifyNotDetected)
	{
		m_notifyDetected = notifyDetected;
		m_notifyNotDetected = notifyNotDetected;
		m_activeAngle = 0f;
		m_activeRadius = 0f;
		m_passiveRadius = 0f;
		m_checkTime = 0f;
	}

	public void SetActiveDetectRange(float activeRadius, float activeAngle)
	{
		m_activeRadius = activeRadius;
		m_activeAngle = activeAngle;
	}

	public void SetPassiveDetectRange(float passiveRadius)
	{
		m_passiveRadius = passiveRadius;
	}

	public override void Enter(NPC npc)
	{
		m_checkTime = 0f;
	}

	public override void Exit(NPC npc)
	{
	}

	public override string Action(NPC npc, float deltaTime)
	{
		m_checkTime -= deltaTime;
		if (m_checkTime > 0f)
		{
			return null;
		}
		if (m_notifyDetected && !m_notifyNotDetected)
		{
			m_checkTime = 2f;
		}
		if (!m_notifyDetected && m_notifyNotDetected)
		{
			m_checkTime = 0.5f;
		}
		bool flag = DetectPlayer(npc);
		if (flag && m_notifyDetected)
		{
			return "DetectPlayer.Detected";
		}
		if (!flag && m_notifyNotDetected)
		{
			return "DetectPlayer.NotDetected";
		}
		return null;
	}

	private bool DetectPlayer(NPC npc)
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player == null || player.m_dead)
		{
			return false;
		}
		Vector3 direction = player.GetTransform().position - npc.GetTransform().position;
		float sqrMagnitude = direction.sqrMagnitude;
		if (sqrMagnitude < m_passiveRadius * m_passiveRadius)
		{
			if (CanSee(npc.GetTransform().position, direction))
			{
				return true;
			}
			return false;
		}
		if (sqrMagnitude < m_activeRadius * m_activeRadius)
		{
			float num;
			for (num = Mathf.Atan2(direction.z, direction.x) * 57.29578f + npc.GetTransform().eulerAngles.y - 90f; num > 180f; num -= 360f)
			{
			}
			for (; num <= -180f; num += 360f)
			{
			}
			if (Mathf.Abs(num) < m_activeAngle / 2f)
			{
				if (CanSee(npc.GetTransform().position, direction))
				{
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private bool CanSee(Vector3 position, Vector3 direction)
	{
		Ray ray = new Ray(position + new Vector3(0f, 1.8f, 0f), direction);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, direction.magnitude, 20480))
		{
			return false;
		}
		return true;
	}
}

using UnityEngine;

public class AIStateDetectTarget : AIState
{
	private bool m_notifyDetected;

	private bool m_notifyNotDetected;

	private Vector3 m_position;

	private float m_radius;

	private float m_checkTime;

	public AIStateDetectTarget(bool notifyDetected, bool notifyNotDetected)
	{
		m_notifyDetected = notifyDetected;
		m_notifyNotDetected = notifyNotDetected;
		m_position = Vector3.zero;
		m_radius = 0f;
		m_checkTime = 0f;
	}

	public void SetDetectRange(Vector3 position, float radius)
	{
		m_position = position;
		m_radius = radius;
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
		m_checkTime = 1f;
		bool flag = DetectTarget();
		if (flag && m_notifyDetected)
		{
			return "DetectTarget.Detected";
		}
		if (!flag && m_notifyNotDetected)
		{
			return "DetectTarget.NotDetected";
		}
		return null;
	}

	private bool DetectTarget()
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player == null || player.m_dead)
		{
			return false;
		}
		float sqrMagnitude = (player.GetTransform().position - m_position).sqrMagnitude;
		if (sqrMagnitude < m_radius * m_radius)
		{
			return true;
		}
		return false;
	}
}

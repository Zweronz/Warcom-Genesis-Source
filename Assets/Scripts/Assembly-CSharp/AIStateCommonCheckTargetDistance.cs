using UnityEngine;

public class AIStateCommonCheckTargetDistance : AIState
{
	private float m_distance = -1f;

	private Vector3 m_target;

	private bool m_notifyApproaching;

	public AIStateCommonCheckTargetDistance(bool notifyApproaching)
	{
		m_notifyApproaching = notifyApproaching;
		m_target = Vector3.zero;
	}

	public void SetDistance(float distance)
	{
		m_distance = distance;
	}

	public override string Action(NPC npc, float deltaTime)
	{
		Vector3 vector = npc.GetTransform().position - m_target;
		vector.y = 0f;
		if (m_notifyApproaching)
		{
			if (m_distance > vector.magnitude)
			{
				return "TargetInDistance.Yes";
			}
		}
		else if (m_distance < vector.magnitude)
		{
			return "TargetInDistance.No";
		}
		return null;
	}

	public void SetTarget(Vector3 target)
	{
		m_target = target;
	}
}

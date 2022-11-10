using UnityEngine;

public class AIStateCommonMove : AIState
{
	private Vector3 m_target = Vector3.zero;

	private float m_moveTime;

	public override void Enter(NPC npc)
	{
		base.Enter(npc);
		m_moveTime = 0f;
		npc.m_agent.updateRotation = true;
	}

	public void SetTarget(Vector3 target)
	{
		m_target = target;
	}

	public override string Action(NPC npc, float deltaTime)
	{
		if (npc.m_agent.destination != m_target)
		{
			npc.m_agent.destination = m_target;
			if (!npc.m_agent.hasPath)
			{
				return null;
			}
		}
		Vector3 vector = npc.GetTransform().InverseTransformDirection(npc.m_agent.velocity);
		Vector2 moveDirection = new Vector2(vector.x, vector.z);
		if (moveDirection.sqrMagnitude == 0f)
		{
			npc.SetMove(false, false, Vector2.zero, 1f);
		}
		else
		{
			npc.SetMove(true, false, moveDirection, Mathf.Max(Mathf.Sqrt(moveDirection.sqrMagnitude) / npc.m_agent.speed, 0.4f));
		}
		m_moveTime += deltaTime;
		if (m_moveTime > 60f)
		{
			return "CommonMove.Complete";
		}
		if (npc.m_agent.remainingDistance < 0.1f)
		{
			return "CommonMove.Complete";
		}
		return null;
	}
}

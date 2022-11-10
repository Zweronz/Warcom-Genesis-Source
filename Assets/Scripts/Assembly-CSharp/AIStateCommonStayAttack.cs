using UnityEngine;

public class AIStateCommonStayAttack : AIState
{
	private Vector3 m_dircTarget = Vector3.zero;

	private float m_turnRotationY;

	private float m_turnRotationDelta;

	public override void Enter(NPC npc)
	{
		base.Enter(npc);
		npc.m_agent.updateRotation = false;
		npc.m_agent.destination = npc.GetTransform().position;
	}

	public override string Action(NPC npc, float deltaTime)
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player == null || player.m_dead)
		{
			return null;
		}
		m_dircTarget = player.GetTransform().position;
		UpdateStateTurn(npc, deltaTime);
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
		return null;
	}

	private void UpdateStateTurn(NPC npc, float deltaTime)
	{
		Quaternion to = Quaternion.LookRotation(m_dircTarget - npc.GetTransform().position);
		float t = Mathf.Abs(npc.m_agent.angularSpeed * deltaTime / (to.eulerAngles.y - npc.GetTransform().eulerAngles.y));
		npc.SetRotationY(Quaternion.Slerp(npc.GetTransform().rotation, to, t).eulerAngles.y);
	}
}

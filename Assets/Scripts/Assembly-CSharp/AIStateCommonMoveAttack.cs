using UnityEngine;

public class AIStateCommonMoveAttack : AIState
{
	private UnityEngine.AI.NavMeshAgent m_agent;

	private Vector3 m_moveTarget = Vector3.zero;

	private Vector3 m_dircTarget = Vector3.zero;

	private float m_moveSpeedFactor = 1f;

	private float m_turnRotationY;

	private float m_turnRotationDelta;

	public void SetMoveTarget(Vector3 target)
	{
		m_moveTarget = target;
	}

	public void SetMoveSpeedFactor(float speedFactor)
	{
		m_moveSpeedFactor = speedFactor;
	}

	public override void Enter(NPC npc)
	{
		base.Enter(npc);
		if (m_agent == null)
		{
			m_agent = npc.GetGameObject().GetComponent<UnityEngine.AI.NavMeshAgent>();
		}
		m_agent.updateRotation = false;
	}

	public override string Action(NPC npc, float deltaTime)
	{
		if (m_agent.destination != m_moveTarget)
		{
			m_agent.destination = m_moveTarget;
			if (!m_agent.hasPath)
			{
				return null;
			}
		}
		Vector3 vector = npc.GetTransform().InverseTransformDirection(m_agent.velocity);
		Vector2 moveDirection = new Vector2(vector.x, vector.z);
		if (moveDirection.sqrMagnitude == 0f)
		{
			npc.SetMove(false, false, Vector2.zero, 1f);
		}
		else
		{
			npc.SetMove(true, false, moveDirection, Mathf.Max(Mathf.Sqrt(moveDirection.sqrMagnitude) / m_agent.speed, 0.4f));
		}
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player == null || player.m_dead)
		{
			return null;
		}
		m_dircTarget = player.GetTransform().position;
		UpdateStateTurn(npc, deltaTime);
		if (m_agent.hasPath && m_agent.remainingDistance < 0.1f)
		{
			return "CommonMoveAttack.Complete";
		}
		return null;
	}

	private void UpdateStateTurn(NPC npc, float deltaTime)
	{
		Quaternion to = Quaternion.LookRotation(m_dircTarget - npc.GetTransform().position);
		float t = Mathf.Abs(m_agent.angularSpeed * deltaTime / (to.eulerAngles.y - npc.GetTransform().eulerAngles.y));
		npc.SetRotationY(Quaternion.Slerp(npc.GetTransform().rotation, to, t).eulerAngles.y);
	}
}

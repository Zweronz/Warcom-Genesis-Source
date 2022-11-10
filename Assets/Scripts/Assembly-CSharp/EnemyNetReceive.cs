using UnityEngine;

public class EnemyNetReceive
{
	private TransformInfo m_transformInfo;

	private NetPlayerStatusInfo m_nps;

	public virtual void Action(EnemyNet en, float deltaTime)
	{
		if (m_nps != null)
		{
			en.SetPitchAngle(m_nps.pitchAngle);
		}
		if (en.m_dead || !en.m_agent.enabled)
		{
			return;
		}
		if (m_transformInfo == null)
		{
			en.m_agent.destination = en.GetTransform().position;
			return;
		}
		UpdateStateTurn(en, deltaTime);
		if (en.m_agent.destination != m_transformInfo.trans.Position)
		{
			en.m_agent.destination = m_transformInfo.trans.Position;
			Vector3 vector = en.GetTransform().InverseTransformDirection(en.m_agent.velocity);
			Vector2 moveDirection = new Vector2(vector.x, vector.z);
			if (moveDirection.sqrMagnitude == 0f)
			{
				en.SetMove(false, false, Vector2.zero, 1f);
			}
			else
			{
				en.SetMove(true, false, moveDirection, Mathf.Max(Mathf.Sqrt(moveDirection.sqrMagnitude) / en.m_agent.speed, 0.8f));
			}
		}
	}

	public void SetTransformInfo(TransformInfo transformInfo)
	{
		m_transformInfo = transformInfo;
	}

	public void SetStateInfo(NetPlayerStatusInfo nps)
	{
		m_nps = nps;
	}

	private void UpdateStateTurn(EnemyNet en, float deltaTime)
	{
		float t = Mathf.Abs(en.m_agent.angularSpeed * deltaTime / (m_transformInfo.trans.Rotation.eulerAngles.y - en.GetTransform().eulerAngles.y));
		en.SetRotationY(Quaternion.Slerp(en.GetTransform().rotation, m_transformInfo.trans.Rotation, t).eulerAngles.y);
	}
}

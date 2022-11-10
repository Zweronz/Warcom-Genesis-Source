using UnityEngine;

public class AIStateCommonStay : AIState
{
	private float m_stayTimeMin;

	private float m_stayTimeMax;

	private float m_stayTimeTotal;

	private float m_stayTimeCurrent;

	public void SetStayTime(float stayTimeMin, float stayTimeMax)
	{
		m_stayTimeMin = stayTimeMin;
		m_stayTimeMax = stayTimeMax;
	}

	public override void Enter(NPC npc)
	{
		m_stayTimeTotal = Random.Range(m_stayTimeMin, m_stayTimeMax);
		m_stayTimeCurrent = 0f;
	}

	public override void Exit(NPC npc)
	{
	}

	public override string Action(NPC npc, float deltaTime)
	{
		npc.SetMove(false, false, Vector2.zero, 1f);
		m_stayTimeCurrent += deltaTime;
		if (m_stayTimeCurrent >= m_stayTimeTotal)
		{
			return "CommonStay.Complete";
		}
		return null;
	}
}

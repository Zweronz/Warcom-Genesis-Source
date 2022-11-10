using UnityEngine;

public class AIStateMoveIdleWander : AIStateContainerFSM
{
	private Vector3 m_borderCenter = Vector3.zero;

	private float m_borderRadius = -1f;

	private float m_stayTimeMin = 2f;

	private float m_stayTimeMax = 3f;

	private AIStateCommonWander m_stateWander;

	public AIStateMoveIdleWander()
	{
		m_stateWander = new AIStateCommonWander();
		m_stateWander.SetBorder(m_borderCenter, m_borderRadius);
		m_stateWander.SetStayTime(m_stayTimeMin, m_stayTimeMax);
		Add("CommonWander.Complete", m_stateWander, true);
	}

	public void SetBorder(Vector3 borderCenter, float borderRadius)
	{
		m_borderCenter = borderCenter;
		m_borderRadius = borderRadius;
		m_stateWander.SetBorder(m_borderCenter, m_borderRadius);
	}

	public void SetStayTime(float stayTimeMin, float stayTimeMax)
	{
		m_stayTimeMin = stayTimeMin;
		m_stayTimeMax = stayTimeMax;
		m_stateWander.SetStayTime(m_stayTimeMin, m_stayTimeMax);
	}
}

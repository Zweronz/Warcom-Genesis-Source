using System;
using UnityEngine;

public class AIStateCommonWander : AIStateContainerFSM
{
	private class AIStateSelectTarget : AIState
	{
		private Vector3 m_borderCenter = Vector3.zero;

		private float m_borderRadius = -1f;

		private Vector3 m_target = Vector3.zero;

		public void SetBorder(Vector3 borderCenter, float borderRadius)
		{
			m_borderCenter = borderCenter;
			m_borderRadius = borderRadius;
		}

		public Vector3 GetTarget()
		{
			return m_target;
		}

		public override string Action(NPC npc, float deltaTime)
		{
			//Discarded unreachable code: IL_00d0
			Vector3 position = npc.GetTransform().position;
			float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
			Vector3 vector = m_borderCenter + new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * UnityEngine.Random.Range(0f, m_borderRadius);
			if (new Vector2(vector.x - position.x, vector.z - position.z).sqrMagnitude < 0.25f)
			{
				return null;
			}
			try
			{
				UnityEngine.AI.NavMeshHit hit;
				if (npc.m_agent.Raycast(vector, out hit))
				{
					m_target = hit.position;
				}
				else
				{
					m_target = vector;
				}
			}
			catch (Exception)
			{
				Debug.Log("Can't Find WanderTarget!");
				return null;
			}
			return "SelectTarget.Complete";
		}
	}

	private Vector3 m_borderCenter = Vector3.zero;

	private float m_borderRadius = -1f;

	private float m_stayTimeMin;

	private float m_stayTimeMax;

	private AIStateSelectTarget m_stateSelectTarget;

	private AIStateCommonMove m_stateMove;

	private AIStateCommonStay m_stateStay;

	public AIStateCommonWander()
	{
		m_stateSelectTarget = new AIStateSelectTarget();
		m_stateSelectTarget.SetBorder(m_borderCenter, m_borderRadius);
		Add(string.Empty, m_stateSelectTarget, true);
		m_stateMove = new AIStateCommonMove();
		Add("SelectTarget.Complete", m_stateMove);
		m_stateStay = new AIStateCommonStay();
		m_stateStay.SetStayTime(m_stayTimeMin, m_stayTimeMax);
		Add("CommonMove.Complete", m_stateStay);
	}

	public void SetBorder(Vector3 borderCenter, float borderRadius)
	{
		m_borderCenter = borderCenter;
		m_borderRadius = borderRadius;
		m_stateSelectTarget.SetBorder(m_borderCenter, m_borderRadius);
	}

	public void SetStayTime(float stayTimeMin, float stayTimeMax)
	{
		m_stayTimeMin = stayTimeMin;
		m_stayTimeMax = stayTimeMax;
		m_stateStay.SetStayTime(m_stayTimeMin, m_stayTimeMax);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		if (m_currentAI == null)
		{
			return null;
		}
		string text = m_currentAI.Action(npc, deltaTime);
		if (text == null)
		{
			return null;
		}
		m_currentAI.Exit(npc);
		m_currentAI = null;
		if (m_stayTimeMax == 0f)
		{
			return "CommonWander.Complete";
		}
		switch (text)
		{
		case "SelectTarget.Complete":
		{
			Vector3 target = m_stateSelectTarget.GetTarget();
			m_stateMove.SetTarget(target);
			break;
		}
		case "CommonStay.Complete":
			return "CommonWander.Complete";
		}
		if (m_ai.ContainsKey(text))
		{
			m_currentAI = m_ai[text];
			m_currentAI.Enter(npc);
			return null;
		}
		return text;
	}
}

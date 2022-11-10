using System.Collections.Generic;

public class AIStateContainerFSM : AIState
{
	protected Dictionary<string, AIState> m_ai = new Dictionary<string, AIState>();

	protected string m_defaultAI;

	protected AIState m_currentAI;

	public void Add(string state, AIState ai, bool defaultAI)
	{
		m_ai.Add(state, ai);
		if (defaultAI)
		{
			m_defaultAI = state;
		}
	}

	public void Add(string state, AIState ai)
	{
		Add(state, ai, false);
	}

	public void Remove(string state)
	{
		m_ai.Remove(state);
		if (m_defaultAI == state)
		{
			m_defaultAI = null;
		}
	}

	public override void Enter(NPC npc)
	{
		m_currentAI = null;
		if (m_defaultAI != null && m_ai.ContainsKey(m_defaultAI))
		{
			m_currentAI = m_ai[m_defaultAI];
		}
		if (m_currentAI != null)
		{
			m_currentAI.Enter(npc);
		}
	}

	public override void Exit(NPC npc)
	{
		if (m_currentAI != null)
		{
			m_currentAI.Exit(npc);
			m_currentAI = null;
		}
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
		if (m_ai.ContainsKey(text))
		{
			m_currentAI = m_ai[text];
			m_currentAI.Enter(npc);
			return null;
		}
		return text;
	}
}

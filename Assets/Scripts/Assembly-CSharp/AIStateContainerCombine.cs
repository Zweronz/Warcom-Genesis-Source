using System.Collections.Generic;

public class AIStateContainerCombine : AIState
{
	private List<AIState> m_ai = new List<AIState>();

	public void Add(AIState ai)
	{
		m_ai.Add(ai);
	}

	public void Remove(AIState ai)
	{
		m_ai.Remove(ai);
	}

	public override void Enter(NPC npc)
	{
		for (int i = 0; i < m_ai.Count; i++)
		{
			m_ai[i].Enter(npc);
		}
	}

	public override void Exit(NPC npc)
	{
		for (int i = 0; i < m_ai.Count; i++)
		{
			m_ai[i].Exit(npc);
		}
	}

	public override string Action(NPC npc, float deltaTime)
	{
		for (int i = 0; i < m_ai.Count; i++)
		{
			string text = m_ai[i].Action(npc, deltaTime);
			if (text != null)
			{
				return text;
			}
		}
		return null;
	}
}

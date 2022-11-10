using UnityEngine;

public class AIStateMoveIdleReturn : AIStateContainerFSM
{
	private Vector3 m_target = Vector3.zero;

	private AIStateCommonMove m_stateMove;

	public AIStateMoveIdleReturn()
	{
		m_stateMove = new AIStateCommonMove();
		Add(string.Empty, m_stateMove, true);
	}

	public void SetTarget(Vector3 target)
	{
		m_target = target;
		m_stateMove.SetTarget(m_target);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		string text = base.Action(npc, deltaTime);
		if (text == "CommonMove.Complete")
		{
			return "MoveIdleReturn.Complete";
		}
		return text;
	}
}

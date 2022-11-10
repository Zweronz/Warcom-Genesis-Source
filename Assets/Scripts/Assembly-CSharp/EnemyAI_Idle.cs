using UnityEngine;

public class EnemyAI_Idle : AIStateContainerCombine
{
	private EnemyAI_IdleMove m_stateMove;

	private AIStateDetectPlayer m_stateDetect;

	private AIStateInfect m_stateInfect;

	private string m_type;

	public EnemyAI_Idle()
	{
		m_stateMove = new EnemyAI_IdleMove();
		Add(m_stateMove);
		m_stateDetect = new AIStateDetectPlayer(true, false);
		Add(m_stateDetect);
		m_stateInfect = new AIStateInfect();
		Add(m_stateInfect);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		switch (base.Action(npc, deltaTime))
		{
		case "MoveRoute.Complete":
			Debug.Log("Route.Complete");
			return "Route.Complete";
		case "DetectPlayer.Detected":
			return "Idle.Detected";
		case "Infect.Yes":
			return "Idle.Infect";
		default:
			return null;
		}
	}

	public EnemyAI_IdleMove GetStateIdleMove()
	{
		return m_stateMove;
	}

	public AIStateDetectPlayer GetStateDetect()
	{
		return m_stateDetect;
	}

	public AIStateInfect GetStateInfect()
	{
		return m_stateInfect;
	}

	public void SetMoveType(string type)
	{
		m_type = type;
	}

	public string GetMoveType()
	{
		return m_type;
	}
}

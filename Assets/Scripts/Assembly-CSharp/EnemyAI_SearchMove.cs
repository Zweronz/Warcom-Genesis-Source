public class EnemyAI_SearchMove : AIStateContainerFSM
{
	private AIStateMoveIdleChase m_stateChase;

	public EnemyAI_SearchMove()
	{
		m_stateChase = new AIStateMoveIdleChase();
		Add(string.Empty, m_stateChase, true);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		return base.Action(npc, deltaTime);
	}
}

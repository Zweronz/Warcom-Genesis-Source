public class AIStateMoveAttackDefend : AIStateContainerFSM
{
	private float m_attackMoveProbability;

	private AIStateCommonDefendAttack m_defendAttack;

	public AIStateMoveAttackDefend()
	{
		m_defendAttack = new AIStateCommonDefendAttack();
		Add(string.Empty, m_defendAttack, true);
	}

	public void SetAttackMoveProbability(float attackMoveProbability)
	{
		m_attackMoveProbability = attackMoveProbability;
	}
}

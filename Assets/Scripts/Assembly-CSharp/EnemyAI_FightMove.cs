public class EnemyAI_FightMove : AIStateContainerFSM
{
	public AIStateMoveAttackDefend m_stateMoveDefend;

	public AIStateMoveAttackChase m_stateMoveChase;

	public void InitializeModeDefend(float attackMoveProbability)
	{
		m_stateMoveDefend = new AIStateMoveAttackDefend();
		m_stateMoveDefend.SetAttackMoveProbability(attackMoveProbability);
		Add(string.Empty, m_stateMoveDefend, true);
	}

	public void InitializeModeChase(float borderRadius)
	{
		m_stateMoveChase = new AIStateMoveAttackChase();
		m_stateMoveChase.SetBorderRadius(borderRadius);
		Add(string.Empty, m_stateMoveChase, true);
	}
}

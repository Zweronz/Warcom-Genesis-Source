public class AIStateMoveAttackChase : AIStateContainerFSM
{
	private class AIStateChaseAttack : AIStateContainerCombine
	{
		private AIStateCommonChaseAttack m_chaseAttack;

		private AIStateCommonCheckTargetDistance m_checkDistance;

		public AIStateChaseAttack()
		{
			m_chaseAttack = new AIStateCommonChaseAttack();
			Add(m_chaseAttack);
			m_checkDistance = new AIStateCommonCheckTargetDistance(true);
			m_checkDistance.SetDistance(10f);
			Add(m_checkDistance);
		}

		public override string Action(NPC npc, float deltaTime)
		{
			Player player = GameManager.Instance().GetLevel().GetPlayer();
			if (player != null)
			{
				m_checkDistance.SetTarget(player.GetTransform().position);
				string text = base.Action(npc, deltaTime);
				if (text == "TargetInDistance.Yes")
				{
					return "ChaseAttack.Complete";
				}
				return null;
			}
			return "ChaseAttack.Complete";
		}
	}

	private class AIStateDefendAttack : AIStateContainerCombine
	{
		private AIStateCommonDefendAttack m_defendAttack;

		private AIStateCommonCheckTargetDistance m_checkDistance;

		public AIStateDefendAttack()
		{
			m_defendAttack = new AIStateCommonDefendAttack();
			Add(m_defendAttack);
			m_checkDistance = new AIStateCommonCheckTargetDistance(false);
			m_checkDistance.SetDistance(14f);
			Add(m_checkDistance);
		}

		public override string Action(NPC npc, float deltaTime)
		{
			Player player = GameManager.Instance().GetLevel().GetPlayer();
			if (player != null)
			{
				m_checkDistance.SetTarget(player.GetTransform().position);
				string text = base.Action(npc, deltaTime);
				if (text == "TargetInDistance.No")
				{
					return "DefendAttack.Complete";
				}
				return null;
			}
			return "DefendAttack.Complete";
		}
	}

	private float m_borderRadius = -1f;

	private AIStateChaseAttack m_chaseAttack;

	private AIStateDefendAttack m_defendAttack;

	public AIStateMoveAttackChase()
	{
		m_chaseAttack = new AIStateChaseAttack();
		Add(string.Empty, m_chaseAttack, true);
		Add("DefendAttack.Complete", m_chaseAttack, true);
		m_defendAttack = new AIStateDefendAttack();
		Add("ChaseAttack.Complete", m_defendAttack);
	}

	public void SetBorderRadius(float borderRadius)
	{
		m_borderRadius = borderRadius;
	}
}

using UnityEngine;

public class AIStateMoveIdleChase : AIStateContainerFSM
{
	private class AIStateChase : AIStateContainerCombine
	{
		private AIStateCommonTimer m_stateTimer;

		private AIStateCommonMove m_stateMove;

		public AIStateChase()
		{
			m_stateTimer = new AIStateCommonTimer();
			Add(m_stateTimer);
			m_stateMove = new AIStateCommonMove();
			Add(m_stateMove);
		}

		public void SetTime(float time)
		{
			m_stateTimer.SetTime(time);
		}

		public void SetTarget(Vector3 target)
		{
			m_stateMove.SetTarget(target);
		}

		public override string Action(NPC npc, float deltaTime)
		{
			string text = base.Action(npc, deltaTime);
			if (text == "CommonTimer.Complete")
			{
				m_stateTimer.SetTime(2f);
				Player player = GameManager.Instance().GetLevel().GetPlayer();
				if (player != null)
				{
					m_stateMove.SetTarget(player.GetTransform().position);
					return "Chase.Timeout";
				}
				return null;
			}
			if (text == "CommonMove.Complete")
			{
				return "Chase.Complete";
			}
			return text;
		}
	}

	private AIStateChase m_stateChase;

	public AIStateMoveIdleChase()
	{
		m_stateChase = new AIStateChase();
		m_stateChase.SetTime(1f);
		Add("Chase.Timeout", m_stateChase, true);
		Add("Chase.Complete", m_stateChase, true);
	}
}

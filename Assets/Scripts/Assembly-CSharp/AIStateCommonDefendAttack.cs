using System;
using UnityEngine;

public class AIStateCommonDefendAttack : AIStateContainerFSM
{
	private class AIStateDefendEvade : AIStateContainerCombine
	{
		private AIStateCommonMoveAttack m_stateMove;

		private AIStateCommonTimer m_time;

		private int m_count;

		public AIStateDefendEvade()
		{
			m_time = new AIStateCommonTimer();
			m_time.SetTime(1f);
			Add(m_time);
			m_stateMove = new AIStateCommonMoveAttack();
			Add(m_stateMove);
		}

		public override void Enter(NPC npc)
		{
			base.Enter(npc);
			m_time.SetTime(1f);
			SetMoveTarget(npc);
			m_count = 0;
		}

		public override string Action(NPC npc, float deltaTime)
		{
			string text = base.Action(npc, deltaTime);
			if (text == "CommonTimer.Complete")
			{
				m_time.SetTime(2f);
				SetMoveTarget(npc);
			}
			if (m_count >= 4)
			{
				m_count = 0;
				return "DefendEvade.Complete";
			}
			return null;
		}

		private void SetMoveTarget(NPC npc)
		{
			Vector3 moveTarget = Vector3.zero;
			int num = Mathf.Max(DataCenter.StateSingle().GetLevelInfo().weekFluctuation + DataCenter.Save().GetWeek(), 0);
			float num2 = 1f;
			num2 = ((num > 3) ? Mathf.Min((float)(num - 3) * 0.04f, 0.75f) : 0f);
			float num3 = UnityEngine.Random.Range(0f, 1f);
			if (num3 < num2)
			{
				moveTarget = ((m_count % 2 != 0) ? CalcMoveTarget(npc, -npc.GetTransform().right * 2f) : CalcMoveTarget(npc, npc.GetTransform().right * 2f));
				m_count++;
			}
			m_stateMove.SetMoveTarget(moveTarget);
		}

		private Vector3 CalcMoveTarget(NPC npc, Vector3 offset)
		{
			//Discarded unreachable code: IL_0047, IL_0062
			try
			{
				UnityEngine.AI.NavMeshHit hit;
				if (npc.m_agent.Raycast(npc.GetTransform().position + offset, out hit))
				{
					return hit.position;
				}
				return npc.GetTransform().position + offset;
			}
			catch (Exception)
			{
				Debug.Log("Can't Find MoveTarget!");
				return Vector3.zero;
			}
		}
	}

	private class AIStateDefendStay : AIStateContainerCombine
	{
		private AIStateCommonStayAttack m_stay;

		private AIStateCommonTimer m_timer;

		public AIStateDefendStay()
		{
			m_stay = new AIStateCommonStayAttack();
			Add(m_stay);
			m_timer = new AIStateCommonTimer();
			m_timer.SetTime(0.5f);
			Add(m_timer);
		}

		public override void Enter(NPC npc)
		{
			base.Enter(npc);
			m_timer.SetTime(0.5f);
		}

		public override string Action(NPC npc, float deltaTime)
		{
			string text = base.Action(npc, deltaTime);
			if (text == "CommonTimer.Complete")
			{
				m_timer.SetTime(2f);
				int num = Mathf.Max(DataCenter.StateSingle().GetLevelInfo().weekFluctuation + DataCenter.Save().GetWeek(), 0);
				float num2 = 1f;
				num2 = ((num > 3) ? Mathf.Min((float)(num - 3) * 0.04f, 0.75f) : 0f);
				float num3 = UnityEngine.Random.Range(0f, 1f);
				if (num3 < num2)
				{
					return "DefendStay.Change";
				}
				return "DefendStay.StillDefend";
			}
			return null;
		}
	}

	private AIStateDefendEvade m_defendEvade;

	private AIStateDefendStay m_defendStay;

	public AIStateCommonDefendAttack()
	{
		m_defendStay = new AIStateDefendStay();
		Add("DefendEvade.Complete", m_defendStay, true);
		Add("DefendStay.StillDefend", m_defendStay);
		m_defendEvade = new AIStateDefendEvade();
		Add("DefendStay.Change", m_defendEvade);
	}
}

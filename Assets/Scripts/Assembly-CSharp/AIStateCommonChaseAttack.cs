using System;
using UnityEngine;

public class AIStateCommonChaseAttack : AIStateContainerCombine
{
	private AIStateCommonMoveAttack m_stateMove;

	private AIStateCommonTimer m_time;

	private int m_count;

	public AIStateCommonChaseAttack()
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
	}

	public override string Action(NPC npc, float deltaTime)
	{
		string text = base.Action(npc, deltaTime);
		if (text == "CommonTimer.Complete")
		{
			m_time.SetTime(2f);
			SetMoveTarget(npc);
		}
		if (text == "CommonMoveAttack.Complete")
		{
			Player player = GameManager.Instance().GetLevel().GetPlayer();
			if (player != null)
			{
				m_stateMove.SetMoveTarget(player.GetTransform().position);
			}
		}
		return null;
	}

	private void SetMoveTarget(NPC npc)
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		Vector3 moveTarget = player.GetTransform().position;
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

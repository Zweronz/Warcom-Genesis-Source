using UnityEngine;

public class EnemyAIFSM : AIStateContainerFSM
{
	private EnemyAI_Idle m_stateIdle;

	private EnemyAI_Fight m_stateFight;

	private EnemyAI_Search m_stateSearch;

	public EnemyAIFSM()
	{
		m_stateIdle = new EnemyAI_Idle();
		Add(string.Empty, m_stateIdle, true);
		Add("GameEnd", m_stateIdle);
		Add("Search.TimeOut", m_stateIdle);
		Add("Fight.NotDetected", m_stateIdle);
		m_stateFight = new EnemyAI_Fight();
		Add("Idle.Detected", m_stateFight);
		Add("Search.Detected", m_stateFight);
		m_stateSearch = new EnemyAI_Search();
		Add("Idle.Infect", m_stateSearch);
		Add("Route.Complete", m_stateSearch);
	}

	public void Initialize(Vector3 spawnPosition, AIParam aiParam)
	{
		m_stateIdle.GetStateIdleMove().SetReturnTarget(spawnPosition);
		if (aiParam.state.Contains("IW"))
		{
			m_stateIdle.GetStateIdleMove().InitializeModeWander(spawnPosition, aiParam.wanderBorderRadius);
			m_stateIdle.SetMoveType("IW");
		}
		else if (aiParam.state.Contains("IR"))
		{
			m_stateIdle.GetStateIdleMove().InitializeModeRoute();
			m_stateIdle.SetMoveType("IR");
		}
		else if (aiParam.state.Contains("IC"))
		{
			m_stateIdle.GetStateIdleMove().InitializeModeChase();
			m_stateIdle.SetMoveType("IC");
		}
		m_stateIdle.GetStateDetect().SetActiveDetectRange(aiParam.detectActiveRadius, aiParam.detectActiveAngle);
		m_stateIdle.GetStateDetect().SetPassiveDetectRange(aiParam.detectPassiveRadius);
		if (aiParam.state.Contains("FC"))
		{
			m_stateFight.GetStateFightMove().InitializeModeChase(aiParam.chaseDistance);
			m_stateFight.SetMoveType("FC");
		}
		else if (aiParam.state.Contains("FD"))
		{
			m_stateFight.GetStateFightMove().InitializeModeDefend(aiParam.attackMoveProbability);
			m_stateFight.SetMoveType("FD");
		}
		m_stateFight.GetStateDetect().SetActiveDetectRange(aiParam.detectActiveRadius, aiParam.detectActiveAngle);
		m_stateFight.GetStateDetect().SetPassiveDetectRange(aiParam.detectPassiveRadius);
		m_stateSearch.GetStateDetect().SetActiveDetectRange(aiParam.detectActiveRadius, aiParam.detectActiveAngle);
		m_stateSearch.GetStateDetect().SetPassiveDetectRange(aiParam.detectPassiveRadius);
	}

	public void EnablePatrolling(float stayTime, bool loop, Vector3[] path)
	{
		m_stateIdle.GetStateIdleMove().SetRouteParams(stayTime, loop, path);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		if (m_currentAI == null)
		{
			return null;
		}
		string text = m_currentAI.Action(npc, deltaTime);
		if (text == null)
		{
			return null;
		}
		m_currentAI.Exit(npc);
		m_currentAI = null;
		if (text == "Fight.NotDetected" && m_stateIdle.GetMoveType() == "IC")
		{
			Player player = GameManager.Instance().GetLevel().GetPlayer();
			if (player == null)
			{
				return null;
			}
			m_stateIdle.GetStateIdleMove().SetReturnTarget(player.GetTransform().position);
		}
		if (text == "Search.Detected")
		{
		}
		if (text == "Idle.Detected")
		{
			CharacterEventManager.Instance().PostDetectPlayer(npc.GetTransform().position);
		}
		if (m_ai.ContainsKey(text))
		{
			m_currentAI = m_ai[text];
			m_currentAI.Enter(npc);
			return null;
		}
		return text;
	}
}

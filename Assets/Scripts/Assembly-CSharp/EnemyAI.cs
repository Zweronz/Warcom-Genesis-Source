using UnityEngine;

public class EnemyAI : NPC.AI
{
	private EnemyAIFSM m_AIFSM;

	public EnemyAI()
	{
		m_AIFSM = new EnemyAIFSM();
	}

	public void Initialize(Vector3 spawnPosition, AIParam aiParam)
	{
		m_AIFSM.Initialize(spawnPosition, aiParam);
	}

	public void EnablePatrolling(float stayTime, bool loop, Vector3[] path)
	{
		m_AIFSM.EnablePatrolling(stayTime, loop, path);
	}

	public override void Load(NPC npc)
	{
		m_AIFSM.Enter(npc);
	}

	public override void Unload(NPC npc)
	{
		m_AIFSM.Exit(npc);
	}

	public override void Action(NPC npc, float deltaTime)
	{
		m_AIFSM.Action(npc, deltaTime);
	}
}

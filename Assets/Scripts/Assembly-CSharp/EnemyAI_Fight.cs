public class EnemyAI_Fight : AIStateContainerCombine
{
	private EnemyAI_FightMove m_stateMove;

	private AIStateAttack m_stateAttack;

	private AIStateDetectPlayer m_stateDetect;

	private string m_type;

	public EnemyAI_Fight()
	{
		m_stateMove = new EnemyAI_FightMove();
		Add(m_stateMove);
		m_stateAttack = new AIStateAttack();
		Add(m_stateAttack);
		m_stateDetect = new AIStateDetectPlayer(false, true);
		Add(m_stateDetect);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		string text = base.Action(npc, deltaTime);
		if (text == "DetectPlayer.NotDetected")
		{
			return "Fight.NotDetected";
		}
		if (GameManager.Instance().GetCamera() != null && (GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.EndMode || GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.DeadMode))
		{
			return "GameEnd";
		}
		return null;
	}

	public EnemyAI_FightMove GetStateFightMove()
	{
		return m_stateMove;
	}

	public AIStateDetectPlayer GetStateDetect()
	{
		return m_stateDetect;
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

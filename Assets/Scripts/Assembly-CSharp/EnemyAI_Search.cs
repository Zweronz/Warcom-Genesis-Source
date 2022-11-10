public class EnemyAI_Search : AIStateContainerCombine
{
	private EnemyAI_SearchMove m_stateMove;

	private AIStateDetectPlayer m_stateDetect;

	public EnemyAI_Search()
	{
		m_stateMove = new EnemyAI_SearchMove();
		Add(m_stateMove);
		m_stateDetect = new AIStateDetectPlayer(true, false);
		Add(m_stateDetect);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		string text = base.Action(npc, deltaTime);
		if (text == "DetectPlayer.Detected")
		{
			return "Search.Detected";
		}
		if (GameManager.Instance().GetCamera() != null && (GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.EndMode || GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.DeadMode))
		{
			return "GameEnd";
		}
		return null;
	}

	public AIStateDetectPlayer GetStateDetect()
	{
		return m_stateDetect;
	}
}

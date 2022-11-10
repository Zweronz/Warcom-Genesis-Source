using UnityEngine;

public class EnemyAITest : NPC.AI
{
	private AIStateMoveIdleWander state;

	public override void Load(NPC npc)
	{
		state = new AIStateMoveIdleWander();
		state.SetBorder(new Vector3(-3f, 0f, -16f), 7f);
		state.SetStayTime(4f, 6f);
		state.Enter(npc);
	}

	public override void Unload(NPC npc)
	{
		state.Exit(npc);
		state = null;
	}

	public override void Action(NPC npc, float deltaTime)
	{
		string text = state.Action(npc, deltaTime);
		if (text != null)
		{
			Debug.Log("AI Event:" + text);
		}
	}
}

public class AIStateCommonTimer : AIState
{
	private float m_time = -1f;

	public void SetTime(float time)
	{
		m_time = time;
	}

	public override string Action(NPC npc, float deltaTime)
	{
		if (m_time < 0f)
		{
			return null;
		}
		m_time -= deltaTime;
		if (m_time > 0f)
		{
			return null;
		}
		return "CommonTimer.Complete";
	}
}

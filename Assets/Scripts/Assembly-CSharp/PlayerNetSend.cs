public class PlayerNetSend
{
	private float m_time;

	public virtual void Action(PlayerNet pn, float deltaTime)
	{
		m_time += deltaTime;
		if (m_time > 0.2f)
		{
			NetworkTransform trans = NetworkTransform.FromTransform(pn.GetTransform());
			UtilsNet.SendTransform(trans, pn.GetGameObject().name);
			NetPlayerStatusInfo netPlayerStatusInfo = new NetPlayerStatusInfo();
			netPlayerStatusInfo.pitchAngle = pn.GetPitchAngle();
			UtilsNet.SentStateInfo(netPlayerStatusInfo, pn.GetGameObject().name);
			m_time = 0f;
		}
	}
}

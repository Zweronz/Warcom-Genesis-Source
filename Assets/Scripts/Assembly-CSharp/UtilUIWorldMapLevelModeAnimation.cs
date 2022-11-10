using UnityEngine;

public class UtilUIWorldMapLevelModeAnimation : MonoBehaviour
{
	private bool m_run;

	private DampedVibration m_dampedVibration = new DampedVibration();

	private float m_delayTime;

	private float m_runTime;

	private float m_time;

	public void RunAnimation(float delayTime)
	{
		m_run = true;
		m_dampedVibration.SetParameter(0.4f, 1.8f, 12f, 0f);
		m_delayTime = delayTime;
		m_runTime = m_dampedVibration.CalculateZeroTime(3);
		m_time = 0f;
		base.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
	}

	public void Update()
	{
		if (!m_run)
		{
			return;
		}
		m_time += Mathf.Clamp(Time.deltaTime, 0f, 0.05f);
		if (!(m_time < m_delayTime))
		{
			if (m_time < m_delayTime + m_runTime)
			{
				float num = m_dampedVibration.CalculateDistance(m_time - m_delayTime);
				base.gameObject.transform.localScale = new Vector3(1f - num, 1f - num, 1f - num);
			}
			else
			{
				base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				m_run = false;
			}
		}
	}
}

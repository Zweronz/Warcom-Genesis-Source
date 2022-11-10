using UnityEngine;

public class UtilPausePrompt : MonoBehaviour
{
	private float mTime;

	private float mDelta;

	private float m_time;

	private float m_lenght;

	private bool m_play;

	private long m_startTime;

	private void Start()
	{
		mTime = Time.realtimeSinceStartup;
		m_lenght = base.gameObject.GetComponent<Animation>()["PausePromptIn"].length;
	}

	public void Play()
	{
		m_play = true;
		m_time = 0f;
		base.gameObject.GetComponent<Animation>().Play("PausePromptIn");
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (m_play)
		{
			if (m_time < m_lenght)
			{
				m_time += num;
				base.gameObject.GetComponent<Animation>()["PausePromptIn"].time = m_time;
			}
			else
			{
				m_time = 0f;
				m_play = false;
			}
		}
	}

	private float UpdateRealTimeDelta()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		mDelta = Mathf.Max(0f, realtimeSinceStartup - mTime);
		mTime = realtimeSinceStartup;
		return mDelta;
	}
}

using UnityEngine;

public class SpecialEffectAnimationContinuousGroup : MonoBehaviour
{
	private SpecialEffectAnimationContinuous[] m_group;

	public void Awake()
	{
		m_group = base.gameObject.GetComponentsInChildren<SpecialEffectAnimationContinuous>(true);
	}

	public void Play()
	{
		for (int i = 0; i < m_group.Length; i++)
		{
			if (m_group.Length > 0)
			{
				m_group[i].Play();
			}
		}
	}

	public void Stop()
	{
		for (int i = 0; i < m_group.Length; i++)
		{
			if (m_group.Length > 0)
			{
				m_group[i].Stop();
			}
		}
	}
}

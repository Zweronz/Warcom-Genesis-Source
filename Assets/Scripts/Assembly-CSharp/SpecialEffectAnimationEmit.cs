using UnityEngine;

public class SpecialEffectAnimationEmit : MonoBehaviour
{
	private float m_length;

	private bool m_play;

	private float m_time;

	public void Awake()
	{
		base.gameObject.GetComponent<Animation>().playAutomatically = false;
		m_length = base.gameObject.GetComponent<Animation>().clip.length;
		base.gameObject.SetActiveRecursively(false);
		m_play = false;
		m_time = 0f;
	}

	public void Emit()
	{
		base.gameObject.SetActiveRecursively(true);
		base.gameObject.GetComponent<Animation>().clip.wrapMode = WrapMode.Once;
		base.gameObject.GetComponent<Animation>().Play();
		m_play = true;
		m_time = 0f;
	}

	public void Update()
	{
		if (m_play)
		{
			m_time += Time.deltaTime;
			if (m_time >= m_length)
			{
				base.gameObject.SetActiveRecursively(false);
				base.gameObject.GetComponent<Animation>().Stop();
				m_play = false;
			}
		}
		else
		{
			base.gameObject.SetActiveRecursively(false);
		}
	}
}

using UnityEngine;

public class SpecialEffectParticleEmit : MonoBehaviour
{
	private ParticleEmitter[] m_emitter;

	private float m_time = 2f;

	private ParticleSystem[] m_newEmitter;

	public void Awake()
	{
		base.gameObject.SetActiveRecursively(false);
		ParticleAnimator[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleAnimator>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].autodestruct = false;
		}
		m_emitter = base.gameObject.GetComponentsInChildren<ParticleEmitter>(true);
		for (int j = 0; j < m_emitter.Length; j++)
		{
			m_emitter[j].emit = false;
		}
		m_newEmitter = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
		for (int k = 0; k < m_newEmitter.Length; k++)
		{
			m_newEmitter[k].playOnAwake = false;
			m_newEmitter[k].loop = false;
			m_newEmitter[k].enableEmission = false;
		}
	}

	public void Emit()
	{
		base.gameObject.SetActiveRecursively(true);
		for (int i = 0; i < m_emitter.Length; i++)
		{
			m_emitter[i].Emit();
		}
		for (int j = 0; j < m_newEmitter.Length; j++)
		{
			m_newEmitter[j].enableEmission = true;
			m_newEmitter[j].Play();
		}
	}

	public void Update()
	{
		if (base.gameObject.active)
		{
			m_time -= Time.deltaTime;
			if (m_time < 0f)
			{
				base.gameObject.SetActiveRecursively(false);
				m_time = 2f;
			}
		}
	}
}

using UnityEngine;

public class SpecialEffectParticleContinuous : MonoBehaviour
{
	private ParticleEmitter[] m_emitter;

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
			m_newEmitter[k].loop = true;
		}
	}

	public void StartEmit()
	{
		base.gameObject.SetActiveRecursively(true);
		for (int i = 0; i < m_emitter.Length; i++)
		{
			m_emitter[i].emit = true;
		}
		for (int j = 0; j < m_newEmitter.Length; j++)
		{
			m_newEmitter[j].Play();
		}
	}

	public void StopEmit()
	{
		base.gameObject.SetActiveRecursively(false);
		for (int i = 0; i < m_emitter.Length; i++)
		{
			m_emitter[i].emit = false;
		}
		for (int j = 0; j < m_newEmitter.Length; j++)
		{
			m_newEmitter[j].Stop();
		}
	}
}

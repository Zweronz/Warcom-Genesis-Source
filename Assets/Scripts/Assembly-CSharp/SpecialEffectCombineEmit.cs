using UnityEngine;

public class SpecialEffectCombineEmit : MonoBehaviour
{
	private SpecialEffectAnimationEmit[] m_animEmit;

	private SpecialEffectParticleEmit m_particleEmit;

	public void Awake()
	{
		m_animEmit = base.gameObject.GetComponentsInChildren<SpecialEffectAnimationEmit>(true);
		m_particleEmit = base.transform.GetComponent<SpecialEffectParticleEmit>();
	}

	public void Emit()
	{
		if (m_animEmit != null)
		{
			for (int i = 0; i < m_animEmit.Length; i++)
			{
				m_animEmit[i].Emit();
			}
		}
		if (m_particleEmit != null)
		{
			m_particleEmit.Emit();
		}
	}
}

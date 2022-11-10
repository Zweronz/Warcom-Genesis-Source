using UnityEngine;

public class SpecialEffectAnimationEmitGroup : MonoBehaviour
{
	private SpecialEffectAnimationEmit[] m_group;

	public void Awake()
	{
		m_group = base.gameObject.GetComponentsInChildren<SpecialEffectAnimationEmit>(true);
	}

	public void Emit()
	{
		for (int i = 0; i < m_group.Length; i++)
		{
			m_group[i].Emit();
		}
	}
}

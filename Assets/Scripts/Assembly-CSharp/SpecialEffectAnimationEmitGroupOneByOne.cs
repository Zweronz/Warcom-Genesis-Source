using UnityEngine;

public class SpecialEffectAnimationEmitGroupOneByOne : MonoBehaviour
{
	private int m_count;

	private SpecialEffectAnimationEmit[] m_group;

	public void Awake()
	{
		m_group = base.gameObject.GetComponentsInChildren<SpecialEffectAnimationEmit>(true);
	}

	public void Start()
	{
		for (int i = 0; i < m_group.Length; i++)
		{
			m_group[i].gameObject.SetActiveRecursively(false);
		}
	}

	public void Emit()
	{
		if (m_group.Length != 0)
		{
			int num = m_count % m_group.Length;
			m_group[num].Emit();
			m_count++;
		}
	}
}

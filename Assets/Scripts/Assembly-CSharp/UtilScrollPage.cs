using UnityEngine;

public class UtilScrollPage : MonoBehaviour
{
	public TUIScroll m_tuiScroll;

	public TUIButtonSelect[] m_btn;

	public float m_intervalDistance;

	private void Update()
	{
		for (int i = 0; i < m_btn.Length; i++)
		{
			m_btn[i].SetSelected(false);
		}
		for (int j = 0; j < m_btn.Length; j++)
		{
			if (m_intervalDistance * (float)j + 40f >= m_tuiScroll.position.x && m_tuiScroll.position.x > m_intervalDistance * (float)(j + 1) + 40f)
			{
				m_btn[j].SetSelected(true);
			}
		}
	}
}

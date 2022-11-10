using UnityEngine;

public class UtilUIScrollObject : MonoBehaviour
{
	private TUIButtonSelect[] m_buttons;

	public void OnScrollBegin()
	{
	}

	public void OnScrollMove()
	{
	}

	public void OnScrollEnd()
	{
	}

	protected void ResetButtonSelect()
	{
		if (m_buttons == null)
		{
			m_buttons = base.gameObject.GetComponentsInChildren<TUIButtonSelect>(true);
		}
		for (int i = 0; i < m_buttons.Length; i++)
		{
			m_buttons[i].Reset();
		}
	}
}

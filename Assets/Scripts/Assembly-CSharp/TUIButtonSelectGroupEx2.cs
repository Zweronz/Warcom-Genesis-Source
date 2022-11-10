public class TUIButtonSelectGroupEx2 : TUIButtonSelectGroupEx
{
	private int m_currentSelect;

	public new void Awake()
	{
		base.Awake();
		buttonSelectGroup = null;
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		buttonSelectGroup = base.gameObject.GetComponentsInChildren<TUIButtonSelect>(true);
		for (int i = 0; i < buttonSelectGroup.Length; i++)
		{
			if (buttonSelectGroup[i] == control)
			{
				m_currentSelect = i;
			}
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}

	public TUIButtonSelect GetCurrentButton()
	{
		if (buttonSelectGroup == null)
		{
			buttonSelectGroup = base.gameObject.GetComponentsInChildren<TUIButtonSelect>(true);
		}
		for (int i = 0; i < buttonSelectGroup.Length; i++)
		{
			if (buttonSelectGroup[i].IsSelected())
			{
				m_currentSelect = i;
			}
		}
		if (m_currentSelect >= buttonSelectGroup.Length)
		{
			return null;
		}
		return buttonSelectGroup[m_currentSelect];
	}
}

using UnityEngine;

public class UtilUISlider : TUIContainer
{
	private Rect m_sliderRect;

	private TUIButtonSlider m_slider;

	private TUIRect m_barRect;

	private float m_barRectWidth;

	private TUIMeshSprite m_bar;

	public void SetSliderRate(float rate)
	{
		m_barRect.rect.width = rate * m_barRectWidth;
		m_barRect.UpdateRect();
		m_bar.UpdateMesh();
		Vector3 position = m_slider.transform.position;
		position.x = m_sliderRect.x + rate * m_sliderRect.width;
		m_slider.transform.position = position;
	}

	public new void Awake()
	{
		TUIRect component = base.transform.Find("ButtonClip").gameObject.GetComponent<TUIRect>();
		component.UpdateRect();
		m_sliderRect = component.GetRect();
		m_slider = base.gameObject.GetComponentInChildren<TUIButtonSlider>();
		m_barRect = base.transform.Find("BarClip").gameObject.GetComponent<TUIRect>();
		m_barRectWidth = m_barRect.rect.width;
		m_bar = base.transform.Find("Bar").gameObject.GetComponent<TUIMeshSprite>();
		base.Awake();
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control == m_slider)
		{
			switch (eventType)
			{
			case 1:
				m_bar.frameName += "On";
				m_bar.UpdateMesh();
				break;
			case 3:
				m_barRect.rect.width = wparam * m_barRectWidth;
				m_barRect.UpdateRect();
				m_bar.UpdateMesh();
				break;
			case 2:
				m_bar.frameName = m_bar.frameName.Substring(0, m_bar.frameName.Length - 2);
				m_bar.UpdateMesh();
				break;
			}
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}

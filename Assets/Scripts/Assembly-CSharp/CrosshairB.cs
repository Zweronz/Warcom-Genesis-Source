using UnityEngine;

public class CrosshairB : Crosshair
{
	private Vector3 m_CrosshairRightUpPos = Vector3.zero;

	private Vector3 m_CrosshairRightDownPos = Vector3.zero;

	private Vector3 m_CrosshairLeftUpPos = Vector3.zero;

	private Vector3 m_CrosshairLeftDownPos = Vector3.zero;

	private GameObject m_CrosshairRightUp;

	private GameObject m_CrosshairRightDown;

	private GameObject m_CrosshairLeftDown;

	private GameObject m_CrosshairLeftUp;

	private GameObject m_Crosshair;

	public float m_accB = 1f;

	public CrosshairB()
	{
		m_type = CrosshairType.B;
		m_Crosshair = GameObject.Find("CrosshairB");
		m_CrosshairRightUp = m_Crosshair.transform.Find("Crosshair_1RightUp").gameObject;
		m_CrosshairRightDown = m_Crosshair.transform.Find("Crosshair_1RightDown").gameObject;
		m_CrosshairLeftUp = m_Crosshair.transform.Find("Crosshair_1LeftUp").gameObject;
		m_CrosshairLeftDown = m_Crosshair.transform.Find("Crosshair_1LeftDown").gameObject;
	}

	public void SetParams(CrosshairParam param, WeaponType weaponType)
	{
		m_weaponType = weaponType;
		m_shotPointBox = param.shotPointBox;
		m_smoothTime = param.time;
		m_sizeMax = param.maxChangeScale;
		m_oneShotAdd = param.oneShotAdd;
		m_CrosshairRightUpPos = new Vector3(m_shotPointBox.x / 2f, m_shotPointBox.y / 2f, 0f);
		m_CrosshairRightDownPos = new Vector3(m_shotPointBox.x / 2f, (0f - m_shotPointBox.y) / 2f, 0f);
		m_CrosshairLeftUpPos = new Vector3((0f - m_shotPointBox.x) / 2f, m_shotPointBox.y / 2f, 0f);
		m_CrosshairLeftDownPos = new Vector3((0f - m_shotPointBox.x) / 2f, (0f - m_shotPointBox.y) / 2f, 0f);
		m_CrosshairRightUp.transform.localPosition = m_CrosshairRightUpPos + new Vector3(2.5f, 2.5f, 0f);
		m_CrosshairRightDown.transform.localPosition = m_CrosshairRightDownPos + new Vector3(2.5f, -2.5f, 0f);
		m_CrosshairLeftUp.transform.localPosition = m_CrosshairLeftUpPos + new Vector3(-2.5f, 2.5f, 0f);
		m_CrosshairLeftDown.transform.localPosition = m_CrosshairLeftDownPos + new Vector3(-2.5f, -2.5f, 0f);
	}

	public override void Open()
	{
		m_Crosshair.SetActiveRecursively(true);
	}

	public override void Close()
	{
		m_Crosshair.SetActiveRecursively(false);
	}

	public override void FireUp()
	{
		if (m_weaponType == WeaponType.AssaultRifle)
		{
			m_sizeCurrent = Mathf.Clamp(m_sizeCurrent + m_oneShotAdd, m_sizeMin * m_acc * m_accB, m_sizeMax * m_acc * m_accB);
		}
		else
		{
			m_sizeCurrent = Mathf.Clamp(m_sizeCurrent + m_oneShotAdd, m_sizeMin * m_acc, m_sizeMax * m_acc);
		}
		m_CrosshairRightUp.transform.localPosition = m_CrosshairRightUpPos * m_sizeCurrent + new Vector3(2.5f, 2.5f, 0f);
		m_CrosshairRightDown.transform.localPosition = m_CrosshairRightDownPos * m_sizeCurrent + new Vector3(2.5f, -2.5f, 0f);
		m_CrosshairLeftUp.transform.localPosition = m_CrosshairLeftUpPos * m_sizeCurrent + new Vector3(-2.5f, 2.5f, 0f);
		m_CrosshairLeftDown.transform.localPosition = m_CrosshairLeftDownPos * m_sizeCurrent + new Vector3(-2.5f, -2.5f, 0f);
	}

	public override void CoolDown()
	{
		if (m_weaponType == WeaponType.AssaultRifle)
		{
			m_sizeCurrent = Mathf.SmoothDamp(m_sizeCurrent, m_sizeMin * m_acc * m_accB, ref m_animSpeed, m_smoothTime);
		}
		else
		{
			m_sizeCurrent = Mathf.SmoothDamp(m_sizeCurrent, m_sizeMin * m_acc, ref m_animSpeed, m_smoothTime);
		}
		m_CrosshairRightUp.transform.localPosition = m_CrosshairRightUpPos * m_sizeCurrent + new Vector3(2.5f, 2.5f, 0f);
		m_CrosshairRightDown.transform.localPosition = m_CrosshairRightDownPos * m_sizeCurrent + new Vector3(2.5f, -2.5f, 0f);
		m_CrosshairLeftUp.transform.localPosition = m_CrosshairLeftUpPos * m_sizeCurrent + new Vector3(-2.5f, 2.5f, 0f);
		m_CrosshairLeftDown.transform.localPosition = m_CrosshairLeftDownPos * m_sizeCurrent + new Vector3(-2.5f, -2.5f, 0f);
	}
}

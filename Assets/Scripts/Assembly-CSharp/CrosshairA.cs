using UnityEngine;

public class CrosshairA : Crosshair
{
	private Vector3 m_CrosshairUpPos = Vector3.zero;

	private Vector3 m_CrosshairDownPos = Vector3.zero;

	private Vector3 m_CrosshairRightPos = Vector3.zero;

	private Vector3 m_CrosshairLeftPos = Vector3.zero;

	private GameObject m_CrosshairUp;

	private GameObject m_CrosshairDown;

	private GameObject m_CrosshairRight;

	private GameObject m_CrosshairLeft;

	private GameObject m_Crosshair;

	public CrosshairA()
	{
		m_type = CrosshairType.A;
		m_Crosshair = GameObject.Find("CrosshairA");
		m_CrosshairUp = m_Crosshair.transform.Find("Crosshair_1Up").gameObject;
		m_CrosshairDown = m_Crosshair.transform.Find("Crosshair_1Down").gameObject;
		m_CrosshairRight = m_Crosshair.transform.Find("Crosshair_1Right").gameObject;
		m_CrosshairLeft = m_Crosshair.transform.Find("Crosshair_1Left").gameObject;
	}

	public void SetParams(CrosshairParam param, WeaponType weaponType)
	{
		m_weaponType = weaponType;
		m_shotPointBox = param.shotPointBox;
		m_smoothTime = param.time;
		m_sizeMax = param.maxChangeScale;
		m_oneShotAdd = param.oneShotAdd;
		m_CrosshairUpPos = new Vector3(0f, m_shotPointBox.y / 2f, 0f);
		m_CrosshairDownPos = new Vector3(0f, (0f - m_shotPointBox.y) / 2f, 0f);
		m_CrosshairRightPos = new Vector3(m_shotPointBox.x / 2f, 0f, 0f);
		m_CrosshairLeftPos = new Vector3((0f - m_shotPointBox.x) / 2f, 0f, 0f);
		m_CrosshairUp.transform.localPosition = m_CrosshairUpPos + new Vector3(0f, 2.5f, 0f);
		m_CrosshairDown.transform.localPosition = m_CrosshairDownPos + new Vector3(0f, -2.5f, 0f);
		m_CrosshairRight.transform.localPosition = m_CrosshairRightPos + new Vector3(2.5f, 0f, 0f);
		m_CrosshairLeft.transform.localPosition = m_CrosshairLeftPos + new Vector3(-2.5f, 0f, 0f);
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
		m_sizeCurrent = Mathf.Clamp(m_sizeCurrent + m_oneShotAdd, m_sizeMin * m_acc, m_sizeMax * m_acc);
		m_CrosshairUp.transform.localPosition = m_CrosshairUpPos * m_sizeCurrent + new Vector3(0f, 2.5f, 0f);
		m_CrosshairDown.transform.localPosition = m_CrosshairDownPos * m_sizeCurrent + new Vector3(0f, -2.5f, 0f);
		m_CrosshairRight.transform.localPosition = m_CrosshairRightPos * m_sizeCurrent + new Vector3(2.5f, 0f, 0f);
		m_CrosshairLeft.transform.localPosition = m_CrosshairLeftPos * m_sizeCurrent + new Vector3(-2.5f, 0f, 0f);
	}

	public override void CoolDown()
	{
		m_sizeCurrent = Mathf.SmoothDamp(m_sizeCurrent, m_sizeMin * m_acc, ref m_animSpeed, m_smoothTime);
		m_CrosshairUp.transform.localPosition = m_CrosshairUpPos * m_sizeCurrent + new Vector3(0f, 2.5f, 0f);
		m_CrosshairDown.transform.localPosition = m_CrosshairDownPos * m_sizeCurrent + new Vector3(0f, -2.5f, 0f);
		m_CrosshairRight.transform.localPosition = m_CrosshairRightPos * m_sizeCurrent + new Vector3(2.5f, 0f, 0f);
		m_CrosshairLeft.transform.localPosition = m_CrosshairLeftPos * m_sizeCurrent + new Vector3(-2.5f, 0f, 0f);
	}
}

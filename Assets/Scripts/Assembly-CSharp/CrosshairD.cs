using UnityEngine;

public class CrosshairD : Crosshair
{
	private Vector3 m_CrosshairPos = Vector3.zero;

	private GameObject m_Crosshair;

	private GameObject m_Crosshair_1;

	public CrosshairD()
	{
		m_type = CrosshairType.D;
		m_Crosshair = GameObject.Find("CrosshairD");
		m_Crosshair_1 = m_Crosshair.transform.Find("Crosshair_1").gameObject;
	}

	public void SetParams(CrosshairParam param, WeaponType weaponType)
	{
		m_weaponType = weaponType;
		m_Crosshair_1.transform.localPosition = m_CrosshairPos;
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
	}

	public override void CoolDown()
	{
	}
}

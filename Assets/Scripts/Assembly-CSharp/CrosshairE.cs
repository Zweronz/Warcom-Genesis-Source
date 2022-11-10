using UnityEngine;

public class CrosshairE : Crosshair
{
	private GameObject m_Crosshair;

	public CrosshairE()
	{
		m_type = CrosshairType.E;
		m_Crosshair = GameObject.Find("CrosshairE");
	}

	public void SetParams(CrosshairParam param, WeaponType weaponType)
	{
		m_weaponType = weaponType;
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

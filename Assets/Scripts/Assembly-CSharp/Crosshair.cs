using UnityEngine;

public class Crosshair
{
	public CrosshairType m_type;

	public WeaponType m_weaponType;

	public float m_smoothTime = 0.3f;

	public float m_animSpeed;

	public float m_oneShotAdd;

	public float m_sizeMin = 1f;

	public float m_sizeMax = 2f;

	public Vector2 m_shotPointBox = Vector2.zero;

	public Vector2 m_shakeRange = Vector2.zero;

	public float m_shakeSpeed;

	public float m_sizeCurrent = 1f;

	public float m_acc = 1f;

	public virtual void Open()
	{
	}

	public virtual void Close()
	{
	}

	public virtual void FireUp()
	{
	}

	public virtual void CoolDown()
	{
	}

	public float GetCurrentSize()
	{
		return m_sizeCurrent;
	}

	public Vector2 GetShotPoint()
	{
		return new Vector2(Random.Range((0f - m_shotPointBox.x) / 2f, m_shotPointBox.x / 2f), Random.Range((0f - m_shotPointBox.y) / 2f, m_shotPointBox.y / 2f)) * m_sizeCurrent;
	}
}

using UnityEngine;

public abstract class WeaponNet : WeaponBase
{
	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
	}

	protected override void OnStartFire()
	{
	}

	protected override void OnStopFire()
	{
	}

	protected override void OnEmptyFire()
	{
	}

	protected override void OnDoFire()
	{
	}

	public override void Reload()
	{
		if (m_type != 0)
		{
			if (m_ammoTotal + m_ammoCurrentAmount > m_ammoCapacity)
			{
				m_ammoTotal = m_ammoTotal - m_ammoCapacity + m_ammoCurrentAmount;
				m_ammoCurrentAmount = m_ammoCapacity;
			}
			else
			{
				m_ammoCurrentAmount = m_ammoTotal + m_ammoCurrentAmount;
				m_ammoTotal = 0;
			}
		}
		else
		{
			m_ammoCurrentAmount = m_ammoCapacity;
		}
	}

	protected override void OnActive()
	{
	}

	protected override void OnDeactive()
	{
	}
}

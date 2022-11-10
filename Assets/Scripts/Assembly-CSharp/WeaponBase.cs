using UnityEngine;

public abstract class WeaponBase : Weapon
{
	protected bool m_useAmmo = true;

	protected int m_attrDMG;

	protected int m_attrRPM;

	protected int m_attrACC;

	protected int m_ammoCapacity;

	protected int m_ammoCurrentAmount;

	protected int m_ammoTotal;

	protected bool m_fire;

	protected float m_hitTimePer;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		m_attrDMG = weaponInfo.attrDMG;
		m_attrRPM = weaponInfo.attrRPM;
		m_attrACC = weaponInfo.attrACC;
		m_ammoCapacity = weaponInfo.ammoCapacity;
		m_ammoCurrentAmount = 0;
		m_ammoTotal = weaponInfo.ammoAmount;
		m_fire = false;
	}

	public override void StartFire()
	{
		if (m_ammoCurrentAmount > 0)
		{
			m_fire = true;
			OnStartFire();
		}
		else
		{
			m_fire = false;
		}
	}

	public override void StopFire()
	{
		if (m_fire)
		{
			m_fire = false;
			OnStopFire();
		}
	}

	public override void UpdateFire()
	{
		if (m_ammoCurrentAmount > 0)
		{
			if (m_useAmmo)
			{
				m_ammoCurrentAmount--;
			}
			OnDoFire();
		}
		else
		{
			m_fire = false;
			OnEmptyFire();
			OnStopFire();
		}
	}

	public override bool NeedReload()
	{
		if (m_type == WeaponType.Knife)
		{
			return false;
		}
		return m_ammoCurrentAmount <= 0 && m_ammoTotal > 0;
	}

	public override void AddAmmoClip(int clip)
	{
		m_ammoTotal += clip * m_ammoCapacity;
	}

	public override void AddAmmo(int addAmmo)
	{
		m_ammoTotal += addAmmo;
	}

	public override void SetAmmoCapacity(float factor)
	{
		m_ammoCapacity += (int)((float)m_ammoCapacity * factor);
		m_ammoTotal += (int)((float)m_ammoTotal * factor);
		Reload();
	}

	public override float GetFireIntervalTime()
	{
		return 60f / (float)m_attrRPM;
	}

	public override void SetHitTimePer(float hitTimePer)
	{
		m_hitTimePer = hitTimePer;
	}

	public override float GetHitTimePer()
	{
		return m_hitTimePer;
	}

	public override int GetAmmoTotal()
	{
		return m_ammoTotal;
	}

	public override void SynchronizeAmmoTotal(int ammoTotal)
	{
		m_ammoTotal = ammoTotal;
	}

	public override int GetAmmoCurrentAmount()
	{
		return m_ammoCurrentAmount;
	}

	public override int GetAmmoCapacity()
	{
		return m_ammoCapacity;
	}

	public override void OpenUseAmmo()
	{
		m_useAmmo = true;
	}

	public override void CloseUseAmmo()
	{
		m_useAmmo = false;
	}

	protected abstract void OnStartFire();

	protected abstract void OnStopFire();

	protected abstract void OnDoFire();

	protected abstract void OnEmptyFire();
}

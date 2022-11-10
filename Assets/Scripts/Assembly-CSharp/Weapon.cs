using UnityEngine;

public abstract class Weapon
{
	protected GameObject m_gameObject;

	protected Transform m_transform;

	protected Transform m_characterTransform;

	protected WeaponType m_type;

	protected string m_name;

	protected string m_animation;

	public virtual void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		m_gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		m_gameObject.name = weaponInfo.name;
		m_gameObject.SetActiveRecursively(false);
		m_transform = m_gameObject.transform;
		m_type = weaponInfo.type;
		m_name = weaponInfo.name;
		m_animation = weaponInfo.animation;
	}

	public GameObject GetGameObject()
	{
		return m_gameObject;
	}

	public Transform GetTransform()
	{
		return m_transform;
	}

	public string GetName()
	{
		return m_name;
	}

	public WeaponType GetWeaponType()
	{
		return m_type;
	}

	public string GetAnimation()
	{
		return m_animation;
	}

	public void SetActive(bool active)
	{
		m_gameObject.SetActiveRecursively(active);
		if (active)
		{
			OnActive();
		}
		else
		{
			OnDeactive();
		}
	}

	public void Mount(Transform character, Transform weaponPoint)
	{
		m_transform.position = character.position;
		m_transform.rotation = character.rotation;
		m_transform.localScale = character.localScale;
		m_transform.parent = weaponPoint;
		m_characterTransform = character;
	}

	public void Unmount()
	{
		m_transform.parent = null;
	}

	public abstract void StartFire();

	public abstract void StopFire();

	public abstract void UpdateFire();

	public abstract bool NeedReload();

	public abstract void Reload();

	public abstract void AddAmmoClip(int clip);

	public abstract void AddAmmo(int addAmmo);

	public abstract int GetAmmoTotal();

	public abstract void SynchronizeAmmoTotal(int ammoTotal);

	public abstract int GetAmmoCurrentAmount();

	public abstract int GetAmmoCapacity();

	public abstract float GetFireIntervalTime();

	public abstract void SetHitTimePer(float hitTimePer);

	public abstract void SetAmmoCapacity(float factor);

	public abstract float GetHitTimePer();

	protected abstract void OnActive();

	protected abstract void OnDeactive();

	public abstract void OpenUseAmmo();

	public abstract void CloseUseAmmo();
}

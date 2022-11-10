using UnityEngine;

public class WeaponEnemyShotgun : WeaponEnemy
{
	private WeaponEffectFireEmitPool m_effectFire;

	private WeaponEffectCartridge m_effectCartridge;

	private ITAudioEvent m_evt;

	private GameObject m_firePoint;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		m_firePoint = m_gameObject.transform.Find("Bone_Weapon/FirePoint").gameObject;
		m_effectFire = new WeaponEffectFireEmitPool(m_gameObject);
		m_effectCartridge = new WeaponEffectCartridge(m_gameObject);
		GameObject original = PrefabCache.Load<GameObject>("SoundEvent/weapon_shotgun_fire");
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = m_transform;
		gameObject.transform.localPosition = Vector3.zero;
		m_evt = gameObject.GetComponent<ITAudioEvent>();
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
		base.OnDoFire();
		m_effectFire.Emit();
		m_effectCartridge.Emit();
		m_evt.Trigger();
	}
}

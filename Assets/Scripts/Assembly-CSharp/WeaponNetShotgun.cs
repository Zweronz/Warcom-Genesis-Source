using UnityEngine;

public class WeaponNetShotgun : WeaponNet
{
	private ITAudioEvent m_evt;

	private ITAudioEvent m_evt2;

	private WeaponEffectFireEmitPool m_effectFire;

	private WeaponEffectCartridge m_effectCartridge;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		m_effectFire = new WeaponEffectFireEmitPool(m_gameObject);
		m_effectCartridge = new WeaponEffectCartridge(m_gameObject);
		GameObject original = PrefabCache.Load<GameObject>("SoundEvent/weapon_shotgun_fire");
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = m_transform;
		gameObject.transform.localPosition = Vector3.zero;
		m_evt = gameObject.GetComponent<ITAudioEvent>();
		GameObject original2 = PrefabCache.Load<GameObject>("SoundEvent/weapon_shotgun_dryfire");
		GameObject gameObject2 = Object.Instantiate(original2) as GameObject;
		gameObject2.transform.parent = m_transform;
		gameObject2.transform.localPosition = Vector3.zero;
		m_evt2 = gameObject2.GetComponent<ITAudioEvent>();
	}

	protected override void OnStartFire()
	{
	}

	protected override void OnStopFire()
	{
	}

	protected override void OnEmptyFire()
	{
		m_evt2.Trigger();
	}

	protected override void OnDoFire()
	{
		base.OnDoFire();
		m_effectFire.Emit();
		m_effectCartridge.Emit();
		m_evt.Trigger();
	}
}

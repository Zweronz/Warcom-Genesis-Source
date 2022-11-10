using UnityEngine;

public class WeaponNetHandgun : WeaponNet
{
	private WeaponEffectFireEmitPool m_effectFire;

	private WeaponEffectCartridge m_effectCartridge;

	private ITAudioEvent m_evt;

	private ITAudioEvent m_evt2;

	private GameObject m_firePoint;

	private GameObject m_fireLine;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		m_firePoint = m_gameObject.transform.Find("Bone_Weapon/FirePoint").gameObject;
		m_effectFire = new WeaponEffectFireEmitPool(m_gameObject);
		m_effectCartridge = new WeaponEffectCartridge(m_gameObject);
		GameObject original = PrefabCache.Load<GameObject>("SoundEvent/weapon_handgun_fire");
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = m_transform;
		gameObject.transform.localPosition = Vector3.zero;
		m_evt = gameObject.GetComponent<ITAudioEvent>();
		GameObject original2 = PrefabCache.Load<GameObject>("SoundEvent/weapon_handgun_dryfire");
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
		Character character = CharacterStub.GetCharacter(m_characterTransform.gameObject);
		Vector3 dir = Quaternion.Euler(character.GetPitchAngle(), character.GetTransform().eulerAngles.y, 0f) * Vector3.forward;
		if (m_fireLine == null)
		{
			m_fireLine = GameObject.Find("EnemyFireLinePool");
		}
		m_fireLine.GetComponent<FireLineGroup>().SetFireLine(m_firePoint.transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f)), dir, 3f);
	}
}

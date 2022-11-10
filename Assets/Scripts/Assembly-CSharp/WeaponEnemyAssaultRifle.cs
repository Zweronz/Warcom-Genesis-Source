using UnityEngine;

public class WeaponEnemyAssaultRifle : WeaponEnemy
{
	private ITAudioEvent m_evt;

	private WeaponEffectFireEmitPool m_effectFire;

	private WeaponEffectCartridge m_effectCartridge;

	private GameObject m_firePoint;

	private GameObject m_fireLine;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		m_attrRPM = (int)((float)m_attrRPM * 0.8f);
		m_firePoint = m_gameObject.transform.Find("Bone_Weapon/FirePoint").gameObject;
		m_effectFire = new WeaponEffectFireEmitPool(m_gameObject);
		m_effectCartridge = new WeaponEffectCartridge(m_gameObject);
		GameObject original = PrefabCache.Load<GameObject>("SoundEvent/weapon_rifle_fire");
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
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		Vector3 dir = player.GetTransform().position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 1.6f), Random.Range(-0.5f, 0.5f)) - m_firePoint.transform.position;
		float lifeTime = Vector3.Distance(player.GetTransform().position, m_firePoint.transform.position) / 100f;
		if (m_fireLine == null)
		{
			m_fireLine = GameObject.Find("EnemyFireLinePool");
		}
		m_fireLine.GetComponent<FireLineGroup>().SetFireLine(m_firePoint.transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f)), dir, lifeTime);
	}
}

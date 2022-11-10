using UnityEngine;

public class WeaponEnemyRPG : WeaponEnemy
{
	private WeaponEffectFireEmitPoolEx m_effectFire;

	private ITAudioEvent m_evt;

	private GameObject m_firePoint;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		m_effectFire = new WeaponEffectFireEmitPoolEx(m_gameObject);
		m_firePoint = m_gameObject.transform.Find("Bone_Weapon/FirePoint").gameObject;
		GameObject original = PrefabCache.Load<GameObject>("SoundEvent/weapon_RPG_fire");
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = m_transform;
		gameObject.transform.localPosition = Vector3.zero;
		m_evt = gameObject.GetComponent<ITAudioEvent>();
	}

	protected override void OnActive()
	{
		GetTransform().Find("Bone_Weapon").Find("Ammo").gameObject.SetActiveRecursively(false);
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
		m_effectFire.Emit();
		m_evt.Trigger();
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		Vector3 vector = player.GetTransform().position + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1.6f), Random.Range(-1f, 1f)) - m_firePoint.transform.position;
		if (m_name == "W_RPG00")
		{
			m_transform.Find("RPG-7_2").gameObject.SetActiveRecursively(false);
		}
		GameObject gameObject = m_transform.Find("Bone_Weapon").Find("Ammo").gameObject;
		GameObject gameObject2 = Object.Instantiate(gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
		gameObject2.SetActive(true);
		gameObject2.name = m_name;
		gameObject2.layer = 17;
		gameObject2.transform.Find("AmmoExplodeEffect").gameObject.SetActiveRecursively(false);
		gameObject2.transform.Find("AmmoEffect").gameObject.SetActiveRecursively(true);
		gameObject2.AddComponent<TAudioController>();
		gameObject2.transform.position = m_transform.Find("Bone_Weapon").Find("FirePoint").position;
		gameObject2.transform.rotation = m_transform.Find("Bone_Weapon").Find("FirePoint").rotation;
		AmmoMissile ammoMissile = gameObject2.AddComponent<AmmoMissile>();
		ammoMissile.m_damage = m_attrDMG;
		ammoMissile.m_flySpeed = 15f;
		ammoMissile.m_explodeRadius = 3f;
		ammoMissile.m_dir = vector.normalized;
		ammoMissile.characterType = AmmoMissile.CharacterType.Enemy;
	}
}

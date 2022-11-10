using UnityEngine;

public class WeaponPlayerRPG : WeaponPlayer
{
	private WeaponEffectFireEmitPoolEx m_effectFire;

	private ITAudioEvent m_evt;

	private GameObject m_firePoint;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		SetHitTimePer(0.3f);
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

	protected override void OnDeactive()
	{
	}

	public override void Reload()
	{
		base.Reload();
		if (m_name == "W_RPG00")
		{
			m_transform.Find("RPG-7_2").gameObject.SetActiveRecursively(true);
		}
	}

	protected override void OnDoFire()
	{
		base.OnDoFire();
		m_effectFire.Emit();
		m_evt.Trigger();
		if (m_name == "W_RPG00")
		{
			m_transform.Find("RPG-7_2").gameObject.SetActiveRecursively(false);
		}
		GameObject gameObject = m_transform.Find("Bone_Weapon").Find("Ammo").gameObject;
		GameObject gameObject2 = Object.Instantiate(gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
		gameObject2.SetActive(true);
		gameObject2.name = m_name;
		gameObject2.layer = 16;
		gameObject2.transform.Find("AmmoExplodeEffect").gameObject.SetActiveRecursively(false);
		gameObject2.transform.Find("AmmoEffect").gameObject.SetActiveRecursively(true);
		gameObject2.transform.position = m_firePoint.transform.position;
		gameObject2.transform.rotation = m_firePoint.transform.rotation;
		gameObject2.AddComponent<TAudioController>();
		GameObject gameObject3 = gameObject2.transform.Find("AmmoExpand").gameObject;
		gameObject3.layer = 18;
		gameObject3.AddComponent<AmmoMissileExpand>();
		AmmoMissile ammoMissile = gameObject2.AddComponent<AmmoMissile>();
		float num = 1f;
		float num2 = 1f;
		if (m_characterTransform != null)
		{
			Character character = CharacterStub.GetCharacter(m_characterTransform.gameObject);
			num = character.m_factorWeaponAtk * num;
			num2 = character.m_factorRpgMissileAtkRange * num2;
			if (character.m_HPCurrent < character.m_HPMax * character.m_factorHpMax * 0.1f)
			{
				ammoMissile.leechLauch = true;
			}
		}
		ammoMissile.m_damage = (float)m_attrDMG * num;
		ammoMissile.m_flySpeed = 20f;
		ammoMissile.m_explodeRadius = 5f * num2;
		ammoMissile.m_dir = GameManager.Instance().GetCamera().transform.forward;
		ammoMissile.characterType = AmmoMissile.CharacterType.Player;
	}

	protected override void OnEmptyFire()
	{
		GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Out of ammo!");
	}
}

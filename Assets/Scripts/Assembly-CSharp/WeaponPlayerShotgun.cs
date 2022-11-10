using UnityEngine;

public class WeaponPlayerShotgun : WeaponPlayer
{
	private WeaponEffectFireEmitPool m_effectFire;

	private WeaponEffectCartridge m_effectCartridge;

	private ITAudioEvent m_evt;

	private ITAudioEvent m_evt2;

	private GameObject m_firePoint;

	private GameObject m_fireHitPoint;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		SetHitTimePer(0.3f);
		m_firePoint = m_gameObject.transform.Find("Bone_Weapon/FirePoint").gameObject;
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

	protected override void OnActive()
	{
	}

	protected override void OnDeactive()
	{
	}

	protected override void OnDoFire()
	{
		base.OnDoFire();
		m_effectFire.Emit();
		m_effectCartridge.Emit();
		m_evt.Trigger();
		GameCamera camera = GameManager.Instance().GetCamera();
		Character.HitInfo hitInfo3 = default(Character.HitInfo);
		for (int i = 0; i < camera.GetCrosshairPositionArray().Length; i++)
		{
			DataCenter.StateSingle().m_missionTempData.fireBullet++;
			Ray ray = new Ray(camera.transform.position, camera.GetCrosshairPositionArray()[i] - camera.transform.position);
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, 10f, 4216832))
			{
				continue;
			}
			Ray ray2 = new Ray(m_firePoint.transform.position, hitInfo.point - m_firePoint.transform.position);
			RaycastHit hitInfo2;
			if (Physics.Raycast(ray2, out hitInfo2, 10f, 4196352))
			{
				DataCenter.StateSingle().m_missionTempData.hitBullet++;
				Character character = CharacterStub.GetCharacter(hitInfo2.collider.transform.root.gameObject);
				hitInfo3.weaponType = m_type;
				hitInfo3.weaponName = m_name;
				float num = ((i != 0) ? 0.5f : 1f);
				hitInfo3.leechKill = false;
				if (m_characterTransform != null)
				{
					Character character2 = CharacterStub.GetCharacter(m_characterTransform.gameObject);
					num = character2.m_factorWeaponAtk * num;
					num = character2.m_factorShotGunAtk * num;
					if (character2.m_HPCurrent < character2.m_HPMax * character2.m_factorHpMax * 0.1f)
					{
						hitInfo3.leechKill = true;
					}
				}
				WeaponDamageModifierParam weaponDamageModifierParamByType = DataCenter.Conf().GetWeaponDamageModifierParamByType(WeaponType.Shotgun);
				float num2 = 1f;
				num2 = ((!(hitInfo2.distance > weaponDamageModifierParamByType.distance1)) ? weaponDamageModifierParamByType.damage1 : ((!(hitInfo2.distance < weaponDamageModifierParamByType.distance2)) ? weaponDamageModifierParamByType.damage3 : weaponDamageModifierParamByType.damage2));
				hitInfo3.damage = (float)m_attrDMG * num * num2;
				hitInfo3.hitPoint = hitInfo2.point;
				hitInfo3.localHitPoint = character.GetTransform().InverseTransformPoint(hitInfo2.point);
				hitInfo3.headShot = false;
				Vector3 vector = character.GetTransform().InverseTransformDirection(hitInfo2.normal);
				vector.y = 0f;
				hitInfo3.hitAngle = Vector3.Angle(character.GetTransform().forward, vector);
				hitInfo3.hitLocDic = vector;
				Nums num3 = default(Nums);
				num3.loc = new Vector2(Camera.main.WorldToScreenPoint(hitInfo3.hitPoint).x - (float)(Screen.width / 2), Camera.main.WorldToScreenPoint(hitInfo3.hitPoint).y - (float)(Screen.height / 2));
				if (character.GetHeadCollider().Raycast(ray2, out hitInfo, 10000f))
				{
					hitInfo3.headShot = true;
					num3.num = (int)hitInfo3.damage * 2;
					GameObject.FindWithTag("NumsPool").GetComponent<UtilUILevelNumsPool>().HInQueue(num3);
				}
				else
				{
					num3.num = (int)hitInfo3.damage;
					GameObject.FindWithTag("NumsPool").GetComponent<UtilUILevelNumsPool>().InQueue(num3);
				}
				character.OnHit(ref hitInfo3);
			}
			else
			{
				if (m_fireHitPoint == null)
				{
					m_fireHitPoint = GameObject.Find("FireHitPointPool");
				}
				m_fireHitPoint.GetComponent<FireHitPointGroup>().SetFireHitPoint(hitInfo.point, 0.5f);
			}
		}
	}

	protected override void OnEmptyFire()
	{
		m_evt2.Trigger();
		GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Out of ammo!");
	}
}

using UnityEngine;

public class Enemy : NPC
{
	public override void Initialize(GameObject prefab, string name, Vector3 position, float direction)
	{
		base.Initialize(prefab, name, position, direction);
		int num = Mathf.Max(DataCenter.Save().GetWeek() + DataCenter.StateSingle().GetLevelInfo().weekFluctuation, 1);
		if (num <= 5)
		{
			m_HPMax = 272 + (num - 1) * 34;
		}
		else if (num >= 6 && num <= 15)
		{
			m_HPMax = 544 + (num - 5) * 54;
		}
		else if (num >= 16 && num <= 30)
		{
			m_HPMax = 1088 + (num - 15) * 72;
		}
		else if (num >= 31 && num <= 50)
		{
			m_HPMax = 2176 + (num - 30) * 108;
		}
		else if (num >= 51)
		{
			m_HPMax = 4352f;
		}
		m_HPMax *= 1.66f;
		m_HPCurrent = m_HPMax;
		m_gameObject.layer = 11;
		GetHeadCollider().gameObject.layer = 22;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
	}

	public override void OnHit(ref HitInfo hitInfo)
	{
		base.OnHit(ref hitInfo);
		int num = Mathf.Max(DataCenter.StateSingle().GetLevelInfo().weekFluctuation + DataCenter.Save().GetWeek(), 0);
		float num2 = ((num > 3) ? Mathf.Max(0f, 1f - (float)(num - 3) * 0.05f) : 1f);
		float num3 = Random.Range(0f, 1f);
		if (num3 < num2)
		{
			m_hitStopFireInterveTime = 2f;
		}
		CharacterEventManager.Instance().PostDetectPlayer(GetTransform().position);
		if (m_HPCurrent <= 0f && !m_dead)
		{
			m_dead = true;
			if (hitInfo.headShot)
			{
				m_headShot = true;
			}
			FSMSwitchDeathState();
			DataCenter.StateSingle().m_missionTempData.emenyKill++;
			if (hitInfo.weaponType == WeaponType.SniperRifle)
			{
				DataCenter.StateSingle().m_missionTempData.weaponSniperKill++;
			}
			else if (hitInfo.weaponType == WeaponType.RPG)
			{
				DataCenter.StateSingle().m_missionTempData.weaponRPGKill++;
			}
			else if (hitInfo.weaponType == WeaponType.SubmachineGun)
			{
				DataCenter.StateSingle().m_missionTempData.weaponSubmachineGunKill++;
			}
			else if (hitInfo.weaponType == WeaponType.Shotgun)
			{
				DataCenter.StateSingle().m_missionTempData.weaponShotgunKill++;
			}
			else if (hitInfo.weaponType == WeaponType.Knife)
			{
				DataCenter.StateSingle().m_missionTempData.weaponKnifeKill++;
			}
			else if (hitInfo.weaponType == WeaponType.Handgun)
			{
				DataCenter.StateSingle().m_missionTempData.weaponHandgunKill++;
			}
			else if (hitInfo.weaponType == WeaponType.AssaultRifle)
			{
				DataCenter.StateSingle().m_missionTempData.weaponAssaultRifleKill++;
			}
			if (m_characterType == CharacterType.Scout)
			{
				DataCenter.StateSingle().m_missionTempData.killScoutCount++;
			}
			else if (m_characterType == CharacterType.Sniper)
			{
				DataCenter.StateSingle().m_missionTempData.killSniperCount++;
			}
			else if (m_characterType == CharacterType.Raider)
			{
				DataCenter.StateSingle().m_missionTempData.killRaiderCount++;
			}
			else if (m_characterType == CharacterType.Commando)
			{
				DataCenter.StateSingle().m_missionTempData.killCommandoCount++;
			}
			if (hitInfo.leechKill)
			{
				DataCenter.StateSingle().m_missionTempData.leechKill++;
			}
			if (hitInfo.headShot)
			{
				DataCenter.StateSingle().m_missionTempData.emenyKillHeadShot++;
			}
		}
	}

	public override void OnDeath()
	{
		Object.Destroy(m_gameObject.GetComponent<CapsuleCollider>());
		Object.Destroy(m_gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>());
	}

	public override void OnDestroy()
	{
		Object.Destroy(m_gameObject);
		m_gameObject = null;
		m_transform = null;
		m_headCollider = null;
	}
}

using System;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class WeaponEnemy : WeaponBase
{
	public float m_shotRate;

	public float m_headShotRate;

	private int fireTimes;

	public float fireCount;

	public int hitLefttimes = 1;

	private EnemyShotRateModifiersParam esrmp;

	private float distance1;

	private float distance2;

	private float shotRateBase;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		Regex regex = new Regex("[0-9][0-9]_tCrystal|[0-9][0-9]");
		m_attrDMG = DataCenter.Conf().GetWeaponInfoByName(regex.Replace(weaponInfo.name, "00")).attrDMG;
		if (weaponInfo.type == WeaponType.SniperRifle)
		{
			m_attrDMG = (int)((float)m_attrDMG * 0.25f);
		}
		else if (weaponInfo.type == WeaponType.Shotgun)
		{
			m_attrDMG = (int)((float)m_attrDMG * 0.667f);
		}
		else if (weaponInfo.type == WeaponType.RPG)
		{
			m_attrDMG = (int)((float)m_attrDMG * 0.5f * 0.667f);
		}
		esrmp = DataCenter.Conf().GetEnemyShotRateModifiersParamByLevelMode(DataCenter.StateSingle().GetLevelInfo().mode);
		distance1 = esrmp.distance1;
		distance2 = esrmp.distance2;
	}

	public override void Reload()
	{
		m_ammoCurrentAmount = m_ammoCapacity;
	}

	protected override void OnActive()
	{
	}

	protected override void OnDeactive()
	{
	}

	protected override void OnDoFire()
	{
		GameObject gameObject = m_gameObject.transform.Find("Bone_Weapon/FirePoint").gameObject;
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		DataCenter.Conf().GetEnemyDataCalcParam().EnemyDataCalc(m_attrDMG, m_attrRPM, DataCenter.Save().GetWeek(), ref m_shotRate, ref m_headShotRate);
		fireTimes++;
		fireCount += 1f;
		if (fireCount >= 1f / m_shotRate)
		{
			hitLefttimes++;
			fireCount -= 1f / m_shotRate;
		}
		float num = Vector3.Distance(gameObject.transform.position, player.GetTransform().position);
		if (num < distance1)
		{
			m_shotRate *= 2f;
		}
		else if (num > distance2)
		{
			m_shotRate *= 0.5f;
		}
		if (GetWeaponType() == WeaponType.SniperRifle)
		{
			m_shotRate *= 2f;
		}
		if ((float)(UnityEngine.Random.Range(0, 1000) * UnityEngine.Random.Range(0, 1000)) <= m_shotRate * 1000000f)
		{
			HitByRate(Mathf.Min(3f / (float)GameManager.Instance().GetLevel().GetEnemyInFightNum(), 1f));
		}
	}

	private void HitByRate(float factor)
	{
		Character.HitInfo hitInfo = default(Character.HitInfo);
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		hitInfo.headShot = false;
		hitInfo.weaponType = m_type;
		hitInfo.weaponName = m_name;
		hitInfo.damage = (float)m_attrDMG * factor;
		hitInfo.leechKill = false;
		if (player != null)
		{
			hitInfo.hitPoint = player.GetTransform().position;
			Vector3 vector = player.GetTransform().InverseTransformDirection(m_transform.root.position - player.GetTransform().position);
			vector.y = 0f;
			hitInfo.hitAngle = Vector3.Angle(player.GetTransform().forward, vector);
			hitInfo.hitLocDic = vector;
			CharacterController component = player.GetTransform().GetComponent<CharacterController>();
			if (component != null)
			{
				Vector2 vector2 = new Vector2(component.radius * Mathf.Cos((float)Math.PI / 180f * hitInfo.hitAngle), component.radius * Mathf.Sin((float)Math.PI / 180f * hitInfo.hitAngle));
				hitInfo.localHitPoint = new Vector3(vector2.x, UnityEngine.Random.Range(0.4f, 1.5f), vector2.y);
			}
		}
		if ((float)(UnityEngine.Random.Range(0, 1000) * UnityEngine.Random.Range(0, 1000)) < m_headShotRate * 1000000f)
		{
			hitInfo.headShot = true;
		}
		if (player != null)
		{
			player.OnHit(ref hitInfo);
			player.GetTransform().GetComponent<TAudioController>().PlayAudio("Fx_HitManLight");
		}
	}
}

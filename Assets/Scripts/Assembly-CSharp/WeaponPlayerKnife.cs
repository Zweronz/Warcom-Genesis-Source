using System;
using UnityEngine;

public class WeaponPlayerKnife : WeaponPlayer
{
	private float m_hpFactor;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		if (weaponInfo.name.Contains("_tCrystal"))
		{
			m_hpFactor = 0.51f;
		}
		else
		{
			m_hpFactor = 0.26f;
		}
		SetHitTimePer(0.3f);
	}

	protected override void OnActive()
	{
	}

	protected override void OnDeactive()
	{
	}

	protected override void OnStartFire()
	{
		base.OnStartFire();
		Reload();
	}

	protected override void OnEmptyFire()
	{
	}

	protected override void OnDoFire()
	{
		base.OnDoFire();
		Reload();
		Collider[] array = Physics.OverlapSphere(m_transform.root.position + new Vector3(0f, 1f, 0f), 2f, 2048);
		for (int i = 0; i < array.Length; i++)
		{
			float num = Vector3.Angle(m_transform.root.forward, array[i].transform.position - m_transform.root.position);
			if (!(num < 120f))
			{
				continue;
			}
			Character character = CharacterStub.GetCharacter(array[i].gameObject);
			Character.HitInfo hitInfo = default(Character.HitInfo);
			hitInfo.weaponType = m_type;
			hitInfo.weaponName = m_name;
			float num2 = 1f;
			hitInfo.leechKill = false;
			if (m_characterTransform != null)
			{
				Character character2 = CharacterStub.GetCharacter(m_characterTransform.gameObject);
				num2 = character2.m_factorWeaponAtk * num2;
				if (character2.m_HPCurrent < character2.m_HPMax * character2.m_factorHpMax * 0.1f)
				{
					hitInfo.leechKill = true;
				}
			}
			hitInfo.damage = character.m_HPMax * m_hpFactor * num2;
			hitInfo.hitPoint = character.GetTransform().position + new Vector3(0f, 1.5f, 0f);
			hitInfo.headShot = false;
			Vector3 vector = character.GetTransform().InverseTransformDirection(m_transform.root.position - character.GetTransform().position);
			vector.y = 0f;
			hitInfo.hitAngle = Vector3.Angle(character.GetTransform().forward, vector);
			hitInfo.hitLocDic = vector;
			CharacterController component = character.GetTransform().GetComponent<CharacterController>();
			if (component != null)
			{
				Vector2 vector2 = new Vector2(component.radius * Mathf.Cos((float)Math.PI / 180f * hitInfo.hitAngle), component.radius * Mathf.Sin((float)Math.PI / 180f * hitInfo.hitAngle));
				hitInfo.localHitPoint = new Vector3(vector2.x, 1.5f, vector2.y);
			}
			Nums num3 = default(Nums);
			num3.loc = new Vector2(Camera.main.WorldToScreenPoint(hitInfo.hitPoint).x - (float)(Screen.width / 2), Camera.main.WorldToScreenPoint(hitInfo.hitPoint).y - (float)(Screen.height / 2));
			num3.num = (int)hitInfo.damage;
			GameObject.FindWithTag("NumsPool").GetComponent<UtilUILevelNumsPool>().InQueue(num3);
			character.OnHit(ref hitInfo);
		}
	}
}

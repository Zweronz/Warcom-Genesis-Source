using System;
using UnityEngine;

public class Player : Character
{
	public override void Initialize(GameObject prefab, string name, Vector3 position, float direction)
	{
		base.Initialize(prefab, name, position, direction);
		m_gameObject.layer = 9;
		GetHeadCollider().gameObject.layer = 21;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (m_HPCurrent > 0f)
		{
			m_HPRecoverTime -= deltaTime;
			if (m_HPRecoverTime < 0f)
			{
				m_HPCurrent = Mathf.Clamp(m_HPCurrent + m_HPRecoverSpeed * m_factorHpRecover * deltaTime, 0f, m_HPMax * m_factorHpMax);
			}
		}
	}

	public override void OnEffect(ref Effect effect)
	{
		try
		{
			if (effect.m_type == DropItemEffectType.MissionGoal)
			{
				DataCenter.StateSingle().m_missionTempData.treasureBox++;
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_buff");
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Target located!");
			}
			else if (effect.m_type == DropItemEffectType.AddBullet)
			{
				AddWeaponAmmo((int)effect.m_param1);
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_bullets");
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Ammo!");
			}
			else if (effect.m_type == DropItemEffectType.RecoverHp)
			{
				m_HPCurrent = Mathf.Clamp(m_HPCurrent + m_HPMax * m_factorHpMax * effect.m_param1 * m_factorRecoverItemEffect, 0f, m_HPMax * m_factorHpMax);
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_Heal");
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("+ HP!");
			}
			else if (effect.m_type == DropItemEffectType.AddExp)
			{
				DataCenter.StateSingle().m_missionTempData.expBag++;
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_buff");
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Exp!");
			}
			else if (effect.m_type == DropItemEffectType.AddMoney)
			{
				DataCenter.StateSingle().m_missionTempData.moneyBag++;
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_treasure");
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Cash!");
			}
			if (m_characterEffectItemEmit != null)
			{
				m_characterEffectItemEmit.StartEmit(effect);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public override void OnHit(ref HitInfo hitInfo)
	{
		base.OnHit(ref hitInfo);
		if (m_HPCurrent <= 0f && !m_dead)
		{
			m_dead = true;
			DataCenter.StateSingle().m_missionTempData.dead++;
			if (hitInfo.headShot)
			{
				m_headShot = true;
				DataCenter.StateSingle().m_missionTempData.deadHeadShot++;
			}
			FSMSwitchDeathState();
		}
	}

	public override void OnDeath()
	{
		m_gameObject.GetComponent<CharacterController>().enabled = false;
	}

	public override void OnResurrection()
	{
		base.OnResurrection();
		m_gameObject.GetComponent<CharacterController>().enabled = true;
		m_HPCurrent = m_HPMax * m_factorHpMax;
	}

	public override void OnDestroy()
	{
	}
}

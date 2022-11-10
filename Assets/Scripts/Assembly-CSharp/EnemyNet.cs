using System;
using UnityEngine;

public class EnemyNet : Character
{
	public int m_seatIndex;

	public int m_netId;

	public UnityEngine.AI.NavMeshAgent m_agent;

	public EnemyNetReceive m_enemyNetReceive;

	public override void Initialize(GameObject prefab, string name, Vector3 position, float direction)
	{
		base.Initialize(prefab, name, position, direction);
		CharacterController component = m_gameObject.GetComponent<CharacterController>();
		Vector3 center = component.center;
		float radius = component.radius;
		UnityEngine.Object.Destroy(component);
		CapsuleCollider capsuleCollider = m_gameObject.AddComponent<CapsuleCollider>();
		capsuleCollider.radius = radius;
		capsuleCollider.center = center;
		m_agent = m_gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
		m_agent.angularSpeed = 720f;
		m_agent.updateRotation = false;
		m_agent.acceleration = 8f;
		m_enemyNetReceive = new EnemyNetReceive();
		m_gameObject.layer = 11;
		GetHeadCollider().gameObject.layer = 22;
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
		if (m_enemyNetReceive != null)
		{
			m_enemyNetReceive.Action(this, deltaTime);
		}
	}

	public override void OnDeath()
	{
		base.OnDeath();
		m_agent.enabled = false;
		m_gameObject.GetComponent<CapsuleCollider>().enabled = false;
	}

	public override void OnResurrection()
	{
		base.OnResurrection();
		m_characterEffectItemEmit.StartEmit(ItemEffectType.DefUpFull);
		CharacterBuff characterBuff = new CharacterBuff();
		characterBuff.buffName = "ResurrectionDefUpFull";
		characterBuff.time = Time.time;
		characterBuff.addValue = 1f;
		characterBuff.timeOut = 5f;
		AddBuff(characterBuff);
		m_gameObject.GetComponent<CapsuleCollider>().enabled = false;
	}

	public override void OnEffect(ref Effect effect)
	{
		try
		{
			if (effect.m_type == DropItemEffectType.AddBullet)
			{
				AddWeaponAmmo((int)effect.m_param1);
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_bullets");
			}
			else if (effect.m_type == DropItemEffectType.RecoverHp)
			{
				m_HPCurrent = Mathf.Clamp(m_HPCurrent + m_HPMax * m_factorHpMax * effect.m_param1 * m_factorRecoverItemEffect, 0f, m_HPMax * m_factorHpMax);
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_Heal");
			}
			else if (effect.m_type == DropItemEffectType.AddExp)
			{
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_buff");
			}
			else if (effect.m_type == DropItemEffectType.AddMoney)
			{
				m_transform.GetComponent<TAudioController>().PlayAudio("Fx_get_treasure");
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
		if (!m_dead)
		{
			if (m_characterEffectBloodEmitPool != null)
			{
				m_characterEffectBloodEmitPool.Emit(hitInfo);
			}
			NetHitInfo hi = new NetHitInfo(hitInfo.weaponType, hitInfo.damage, hitInfo.headShot, m_netId, DataCenter.StateMulti().m_tnet.Myself.Id);
			UtilsNet.SentHitInfo(hi, GameManager.Instance().GetLevel().GetPlayer()
				.GetGameObject()
				.name);
			}
		}

		public override void UseItems(ItemInfo itemInfo)
		{
			base.UseItems(itemInfo);
			if (itemInfo.type == ItemEffectType.MoveSpeedUp)
			{
				m_agent.speed = m_moveSpeed * 1.5f;
			}
		}

		public void Destroy()
		{
			UnityEngine.Object.Destroy(m_gameObject);
			m_gameObject = null;
			m_transform = null;
			m_headCollider = null;
			m_agent = null;
			m_enemyNetReceive = null;
		}
	}

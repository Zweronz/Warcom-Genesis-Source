using System;
using UnityEngine;

public class PlayerNet : Player
{
	protected PlayerNetSend m_playNetSend;

	public override void Initialize(GameObject prefab, string name, Vector3 position, float direction)
	{
		base.Initialize(prefab, name, position, direction);
		m_playNetSend = new PlayerNetSend();
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (m_playNetSend != null)
		{
			m_playNetSend.Action(this, deltaTime);
		}
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
		double networkTime = DataCenter.StateMulti().m_tnet.TimeManager.NetworkTime;
		LevelConfNet.ResurrectionPoint[] resurrectionPointArray = DataCenter.StateMulti().GetLevelConfDeathMatch().GetResurrectionPointArray();
		int num;
		Transform scenePointTransform;
		do
		{
			num = UnityEngine.Random.Range(0, resurrectionPointArray.Length * 3 + 1) % resurrectionPointArray.Length;
			scenePointTransform = ScenePointManager.GetScenePointTransform(resurrectionPointArray[num].point);
		}
		while (Physics.CheckSphere(scenePointTransform.position, 8f, 2048));
		UtilsNet.SentResurrectionAction(new NetPlayerResurrectionAction(resurrectionPointArray[num].point, resurrectionPointArray[num].direction, networkTime), GetGameObject().name);
		m_transform.position = scenePointTransform.position;
		m_transform.eulerAngles = new Vector3(0f, resurrectionPointArray[num].direction, 0f);
	}

	public override void ChangeWeapon(int index)
	{
		base.ChangeWeapon(index);
		double networkTime = DataCenter.StateMulti().m_tnet.TimeManager.NetworkTime;
		UtilsNet.SentChangeWeaponAction(new NetPlayerChangeWeaponAction(index, networkTime), GetGameObject().name);
	}

	public override void OpenReload()
	{
		base.OpenReload();
		double networkTime = DataCenter.StateMulti().m_tnet.TimeManager.NetworkTime;
		UtilsNet.SentReloadAction(new NetPlayerReloadAction(networkTime, GetWeapon().GetAmmoTotal(), GetWeapon().GetWeaponType()), GetGameObject().name);
	}

	public override void SetFire(bool fire, bool fireContinuous)
	{
		base.SetFire(fire, fireContinuous);
		double networkTime = DataCenter.StateMulti().m_tnet.TimeManager.NetworkTime;
		UtilsNet.SentFireAction(new NetPlayerFireAction(fire, networkTime), GetGameObject().name);
	}

	public override void OnHitWeb(NetHitInfo hi)
	{
		base.OnHitWeb(hi);
		if (hi.headShot)
		{
			m_HPCurrent -= hi.damage * m_factorHitDamage * 2f;
			if (m_HPCurrent > 0f)
			{
				m_HPRecoverTime = 5f * m_factorRecoverTime;
			}
		}
		else
		{
			m_HPCurrent -= hi.damage * m_factorHitDamage;
			if (m_HPCurrent > 0f)
			{
				m_HPRecoverTime = 5f * m_factorRecoverTime;
			}
		}
		if (!(m_HPCurrent <= 0f) || m_dead)
		{
			return;
		}
		m_dead = true;
		if (hi.headShot)
		{
			m_headShot = true;
		}
		double networkTime = DataCenter.StateMulti().m_tnet.TimeManager.NetworkTime;
		UtilsNet.SentDeadAction(new NetPlayerDeadAction(m_headShot, hi.byNetId, (int)hi.weaponType, networkTime), GetGameObject().name);
		string name = DataCenter.StateMulti().m_tnet.CurRoom.GetUserById(hi.byNetId).Name;
		if (m_headShot)
		{
			GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(name + " headshot You");
		}
		else
		{
			switch (hi.weaponType)
			{
			case WeaponType.Knife:
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(name + " mashed You");
				break;
			case WeaponType.Handgun:
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(name + " capped You");
				break;
			case WeaponType.AssaultRifle:
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(name + " blasted You");
				break;
			case WeaponType.RPG:
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(name + " splattered You");
				break;
			case WeaponType.Shotgun:
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(name + " shredded You");
				break;
			case WeaponType.SniperRifle:
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(name + " sniped You");
				break;
			case WeaponType.SubmachineGun:
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(name + " blasted You");
				break;
			}
		}
		FSMSwitchDeathState();
	}

	public override void UseItems(ItemInfo itemInfo)
	{
		base.UseItems(itemInfo);
		double networkTime = DataCenter.StateMulti().m_tnet.TimeManager.NetworkTime;
		UtilsNet.SentUseItemAction(new NetPlayerUseItemAction(itemInfo.name, networkTime), GetGameObject().name);
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
}

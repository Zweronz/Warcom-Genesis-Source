using System;
using UnityEngine;

public class CharacterPassivePara : MonoBehaviour
{
	private Character m_character;

	private ArmorInfo m_armor01Info;

	private ArmorInfo m_armor02Info;

	private ArmorInfo m_armor03Info;

	private void Awake()
	{
		m_character = CharacterStub.GetCharacter(base.gameObject);
	}

	public void ArmorEffectForUIShow(CharacterEquipInfo characterEquipInfo)
	{
		if (characterEquipInfo.armor01 != string.Empty)
		{
			m_armor01Info = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor01);
			CharacterArmorEffectForUIShow(m_armor01Info);
		}
		if (characterEquipInfo.armor02 != string.Empty)
		{
			m_armor02Info = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor02);
			CharacterArmorEffectForUIShow(m_armor02Info);
		}
		if (characterEquipInfo.armor03 != string.Empty)
		{
			m_armor03Info = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor03);
			CharacterArmorEffectForUIShow(m_armor03Info);
		}
	}

	public void ArmorEffect(CharacterEquipInfo characterEquipInfo)
	{
		if (characterEquipInfo.armor01 != string.Empty)
		{
			m_armor01Info = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor01);
			CharacterArmorEffect(m_armor01Info);
		}
		if (characterEquipInfo.armor02 != string.Empty)
		{
			m_armor02Info = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor02);
			CharacterArmorEffect(m_armor02Info);
		}
		if (characterEquipInfo.armor03 != string.Empty)
		{
			m_armor03Info = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor03);
			CharacterArmorEffect(m_armor03Info);
		}
	}

	public void ArmorEffect(string armor01, string armor02, string armor03)
	{
		try
		{
			if (armor01 != string.Empty)
			{
				m_armor01Info = DataCenter.Conf().GetArmorInfoByName(armor01);
				CharacterArmorEffect(m_armor01Info);
			}
			if (armor02 != string.Empty)
			{
				m_armor02Info = DataCenter.Conf().GetArmorInfoByName(armor02);
				CharacterArmorEffect(m_armor02Info);
			}
			if (armor03 != string.Empty)
			{
				m_armor03Info = DataCenter.Conf().GetArmorInfoByName(armor03);
				CharacterArmorEffect(m_armor03Info);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void CharacterArmorEffectForUIShow(ArmorInfo armorInfo)
	{
		if (armorInfo.param1 != -1f)
		{
			switch (armorInfo.type)
			{
			case ArmorEffectType.HpMax:
				m_character.m_factorHpMax += armorInfo.param1;
				m_character.m_HPCurrent = m_character.m_HPMax * m_character.m_factorHpMax;
				break;
			case ArmorEffectType.HpRecover:
				m_character.m_factorHpRecover += armorInfo.param1;
				break;
			case ArmorEffectType.MoveSpeed:
				m_character.m_factorMoveSpeed += armorInfo.param1;
				break;
			case ArmorEffectType.HitDamage:
				break;
			}
		}
	}

	private void CharacterArmorEffect(ArmorInfo armorInfo)
	{
		if (armorInfo.param1 != -1f)
		{
			switch (armorInfo.type)
			{
			case ArmorEffectType.HpMax:
				m_character.m_factorHpMax += armorInfo.param1;
				m_character.m_HPCurrent = m_character.m_HPMax * m_character.m_factorHpMax;
				break;
			case ArmorEffectType.HitDamage:
				m_character.m_factorHitDamage *= 1f - armorInfo.param1;
				break;
			case ArmorEffectType.HpRecover:
				m_character.m_factorHpRecover += armorInfo.param1;
				break;
			case ArmorEffectType.MoveSpeed:
				m_character.m_factorMoveSpeed += armorInfo.param1;
				break;
			case ArmorEffectType.WeaponAcc:
				m_character.m_factorWeaponAcc *= 1f - armorInfo.param1;
				GameManager.Instance().GetCamera().SetCrosshairAcc(m_character.m_factorWeaponAcc);
				break;
			case ArmorEffectType.WeaponAtk:
				m_character.m_factorWeaponAtk += armorInfo.param1;
				break;
			case ArmorEffectType.SniperFireSpeed:
				m_character.m_factorSniperFireSpeed *= 1f - armorInfo.param1;
				break;
			case ArmorEffectType.ColdWeaponFireSpeed:
				m_character.m_factorColdWeaponFireSpeed *= 1f - armorInfo.param1;
				break;
			case ArmorEffectType.HeadgunAtk:
				m_character.m_factorHandGunAtk += armorInfo.param1;
				break;
			case ArmorEffectType.AssaultAcc:
				m_character.m_factorAssaultAcc *= 1f - armorInfo.param1;
				GameManager.Instance().GetCamera().SetAssaultCrosshairAcc(m_character.m_factorAssaultAcc);
				break;
			case ArmorEffectType.SubmachineCapacity:
				m_character.m_factorSubmachineCapacity += armorInfo.param1;
				m_character.SetWeaponAmmoCapacity(WeaponType.SubmachineGun, m_character.m_factorSubmachineCapacity);
				break;
			case ArmorEffectType.ShotGunAtk:
				m_character.m_factorShotGunAtk += armorInfo.param1;
				break;
			case ArmorEffectType.RpgMissileAtkRange:
				m_character.m_factorRpgMissileAtkRange += armorInfo.param1;
				break;
			case ArmorEffectType.RecoverItemEffect:
				m_character.m_factorRecoverItemEffect += armorInfo.param1;
				break;
			case ArmorEffectType.NotUseBullet:
				m_character.m_armorNotUseBullet = true;
				m_character.SetWeaponNotUseAmmo();
				break;
			case ArmorEffectType.RecoverTime:
				m_character.m_factorRecoverTime *= 1f - armorInfo.param1;
				m_character.m_factorHpRecover += armorInfo.param2;
				break;
			}
		}
	}
}

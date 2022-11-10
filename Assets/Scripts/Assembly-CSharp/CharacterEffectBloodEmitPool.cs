using UnityEngine;

public class CharacterEffectBloodEmitPool
{
	private SpecialEffectParticleEmitPool m_effect_Heavy;

	private SpecialEffectParticleEmitPool m_effect_Mid;

	private SpecialEffectParticleEmitPool m_effect_Light;

	private SpecialEffectParticleEmitPool m_effect_Blade;

	private GameObject m_character;

	private GameObject m_heavy;

	private GameObject m_mid;

	private GameObject m_light;

	private GameObject m_blade;

	public CharacterEffectBloodEmitPool(GameObject character)
	{
		m_character = character;
		m_heavy = m_character.transform.Find("effectBloodPool_Heavy").gameObject;
		if (m_heavy.transform.childCount > 0)
		{
			m_effect_Heavy = m_heavy.transform.GetComponent<SpecialEffectParticleEmitPool>();
		}
		m_mid = m_character.transform.Find("effectBloodPool_Mid").gameObject;
		if (m_mid.transform.childCount > 0)
		{
			m_effect_Mid = m_mid.transform.GetComponent<SpecialEffectParticleEmitPool>();
		}
		m_light = m_character.transform.Find("effectBloodPool_Light").gameObject;
		if (m_light.transform.childCount > 0)
		{
			m_effect_Light = m_light.transform.GetComponent<SpecialEffectParticleEmitPool>();
		}
		m_blade = m_character.transform.Find("effectBloodPool_Blade").gameObject;
		if (m_blade.transform.childCount > 0)
		{
			m_effect_Blade = m_blade.transform.GetComponent<SpecialEffectParticleEmitPool>();
		}
	}

	public void Emit(Character.HitInfo hitInfo)
	{
		if (hitInfo.weaponType == WeaponType.Knife)
		{
			if (m_effect_Blade != null)
			{
				m_blade.transform.localPosition = hitInfo.localHitPoint;
				m_effect_Blade.Emit();
				m_effect_Blade.transform.forward = hitInfo.hitLocDic;
			}
		}
		else if (hitInfo.weaponType == WeaponType.SubmachineGun || hitInfo.weaponType == WeaponType.Handgun)
		{
			if (m_effect_Light != null)
			{
				m_light.transform.localPosition = hitInfo.localHitPoint;
				m_effect_Light.Emit();
				m_effect_Light.transform.forward = hitInfo.hitLocDic;
			}
		}
		else if (hitInfo.weaponType == WeaponType.Shotgun || hitInfo.weaponType == WeaponType.AssaultRifle)
		{
			if (m_effect_Mid != null)
			{
				m_mid.transform.localPosition = hitInfo.localHitPoint;
				m_effect_Mid.Emit();
				m_effect_Mid.transform.forward = hitInfo.hitLocDic;
			}
		}
		else if ((hitInfo.weaponType == WeaponType.SniperRifle || hitInfo.weaponType == WeaponType.RPG) && m_effect_Heavy != null)
		{
			m_heavy.transform.localPosition = hitInfo.localHitPoint;
			m_effect_Heavy.Emit();
			m_effect_Heavy.transform.forward = hitInfo.hitLocDic;
		}
	}
}

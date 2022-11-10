using UnityEngine;

public class CharacterEffectItemEmit
{
	private SpecialEffectParticleEmit effect_item_ammoplus;

	private SpecialEffectParticleEmit effect_item_defup;

	private SpecialEffectParticleContinuous effect_item_defup_2;

	private SpecialEffectParticleEmit effect_item_godlike;

	private SpecialEffectParticleContinuous effect_item_godlike_2;

	private SpecialEffectParticleEmit effect_item_medical;

	private SpecialEffectParticleEmit effect_item_powup;

	private SpecialEffectParticleContinuous effect_item_powup_2;

	private SpecialEffectCombineEmit effect_item_resurrection;

	private SpecialEffectParticleEmit effect_item_spdup;

	private SpecialEffectParticleContinuous effect_item_spdup_2;

	private SpecialEffectParticleEmit effect_item_treasure;

	private SpecialEffectAnimationEmitGroup effect_item_fullmetaljacket1;

	private SpecialEffectAnimationContinuousGroup effect_item_fullmetaljacket2;

	private GameObject m_effect_item_ammoplus_obj;

	private GameObject m_effect_item_defup_obj;

	private GameObject m_effect_item_defup_2_obj;

	private GameObject m_effect_item_fullmetaljacket1_obj;

	private GameObject m_effect_item_fullmetaljacket2_obj;

	private GameObject m_effect_item_godlike_obj;

	private GameObject m_effect_item_godlike_2_obj;

	private GameObject m_effect_item_medical_obj;

	private GameObject m_effect_item_powup_obj;

	private GameObject m_effect_item_powup_2_obj;

	private GameObject m_effect_item_resurrection_obj;

	private GameObject m_effect_item_spdup_obj;

	private GameObject m_effect_item_spdup_2_obj;

	private GameObject m_effect_item_treasure_obj;

	public CharacterEffectItemEmit(GameObject character)
	{
		m_effect_item_ammoplus_obj = character.transform.Find("effect_item_ammoplus").gameObject;
		if (m_effect_item_ammoplus_obj.transform.childCount > 0)
		{
			effect_item_ammoplus = m_effect_item_ammoplus_obj.transform.GetComponent<SpecialEffectParticleEmit>();
		}
		m_effect_item_defup_obj = character.transform.Find("effect_item_defup").gameObject;
		if (m_effect_item_defup_obj.transform.childCount > 0)
		{
			effect_item_defup = m_effect_item_defup_obj.transform.GetComponent<SpecialEffectParticleEmit>();
		}
		m_effect_item_defup_2_obj = character.transform.Find("effect_item_defup_2").gameObject;
		if (m_effect_item_defup_2_obj.transform.childCount > 0)
		{
			effect_item_defup_2 = m_effect_item_defup_2_obj.transform.GetComponent<SpecialEffectParticleContinuous>();
		}
		m_effect_item_fullmetaljacket1_obj = character.transform.Find("effect_item_fullmetaljacket1").gameObject;
		if (m_effect_item_fullmetaljacket1_obj.transform.childCount > 0)
		{
			effect_item_fullmetaljacket1 = m_effect_item_fullmetaljacket1_obj.transform.GetComponent<SpecialEffectAnimationEmitGroup>();
		}
		m_effect_item_fullmetaljacket2_obj = character.transform.Find("effect_item_fullmetaljacket2").gameObject;
		if (m_effect_item_fullmetaljacket2_obj.transform.childCount > 0)
		{
			effect_item_fullmetaljacket2 = m_effect_item_fullmetaljacket2_obj.transform.GetComponent<SpecialEffectAnimationContinuousGroup>();
		}
		m_effect_item_godlike_obj = character.transform.Find("effect_item_godlike").gameObject;
		if (m_effect_item_godlike_obj.transform.childCount > 0)
		{
			effect_item_godlike = m_effect_item_godlike_obj.transform.GetComponent<SpecialEffectParticleEmit>();
		}
		m_effect_item_godlike_2_obj = character.transform.Find("effect_item_godlike_2").gameObject;
		if (m_effect_item_godlike_2_obj.transform.childCount > 0)
		{
			effect_item_godlike_2 = m_effect_item_godlike_2_obj.transform.GetComponent<SpecialEffectParticleContinuous>();
		}
		m_effect_item_medical_obj = character.transform.Find("effect_item_medical").gameObject;
		if (m_effect_item_medical_obj.transform.childCount > 0)
		{
			effect_item_medical = m_effect_item_medical_obj.transform.GetComponent<SpecialEffectParticleEmit>();
		}
		m_effect_item_powup_obj = character.transform.Find("effect_item_powup").gameObject;
		if (m_effect_item_powup_obj.transform.childCount > 0)
		{
			effect_item_powup = m_effect_item_powup_obj.transform.GetComponent<SpecialEffectParticleEmit>();
		}
		m_effect_item_powup_2_obj = character.transform.Find("effect_item_powup_2").gameObject;
		if (m_effect_item_powup_2_obj.transform.childCount > 0)
		{
			effect_item_powup_2 = m_effect_item_powup_2_obj.transform.GetComponent<SpecialEffectParticleContinuous>();
		}
		m_effect_item_resurrection_obj = character.transform.Find("effect_item_resurrection").gameObject;
		if (m_effect_item_resurrection_obj.transform.childCount > 0)
		{
			effect_item_resurrection = m_effect_item_resurrection_obj.transform.GetComponent<SpecialEffectCombineEmit>();
		}
		m_effect_item_spdup_obj = character.transform.Find("effect_item_spdup").gameObject;
		if (m_effect_item_spdup_obj.transform.childCount > 0)
		{
			effect_item_spdup = m_effect_item_spdup_obj.transform.GetComponent<SpecialEffectParticleEmit>();
		}
		m_effect_item_spdup_2_obj = character.transform.Find("effect_item_spdup_2").gameObject;
		if (m_effect_item_spdup_2_obj.transform.childCount > 0)
		{
			effect_item_spdup_2 = m_effect_item_spdup_2_obj.transform.GetComponent<SpecialEffectParticleContinuous>();
		}
		m_effect_item_treasure_obj = character.transform.Find("effect_item_treasure").gameObject;
		if (m_effect_item_treasure_obj.transform.childCount > 0)
		{
			effect_item_treasure = m_effect_item_treasure_obj.transform.GetComponent<SpecialEffectParticleEmit>();
		}
	}

	public void StartEmit(Effect effect)
	{
		if (effect.m_type == DropItemEffectType.MissionGoal)
		{
			Color color = new Color(1f, 0.85f, 0.21f, 0.4f);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_01").GetComponent<Renderer>().material.SetColor("_TintColor", color);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_02").GetComponent<Renderer>().material.SetColor("_TintColor", color);
			if (effect_item_treasure != null)
			{
				effect_item_treasure.Emit();
			}
		}
		else if (effect.m_type == DropItemEffectType.AddBullet)
		{
			Color color2 = new Color(1f, 0.8f, 0.42f, 0.4f);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_01").GetComponent<Renderer>().material.SetColor("_TintColor", color2);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_02").GetComponent<Renderer>().material.SetColor("_TintColor", color2);
			if (effect_item_treasure != null)
			{
				effect_item_treasure.Emit();
			}
		}
		else if (effect.m_type == DropItemEffectType.AddExp)
		{
			Color color3 = new Color(0f, 1f, 0.04f, 0.4f);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_01").GetComponent<Renderer>().material.SetColor("_TintColor", color3);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_02").GetComponent<Renderer>().material.SetColor("_TintColor", color3);
			if (effect_item_treasure != null)
			{
				effect_item_treasure.Emit();
			}
		}
		else if (effect.m_type == DropItemEffectType.AddMoney)
		{
			Color color4 = new Color(1f, 0.64f, 0.11f, 0.4f);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_01").GetComponent<Renderer>().material.SetColor("_TintColor", color4);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_02").GetComponent<Renderer>().material.SetColor("_TintColor", color4);
			if (effect_item_treasure != null)
			{
				effect_item_treasure.Emit();
			}
		}
		else if (effect.m_type == DropItemEffectType.RecoverHp)
		{
			Color color5 = new Color(0.34f, 1f, 0.28f, 0.4f);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_01").GetComponent<Renderer>().material.SetColor("_TintColor", color5);
			m_effect_item_treasure_obj.transform.Find("effect_treasure_2_02").GetComponent<Renderer>().material.SetColor("_TintColor", color5);
			if (effect_item_treasure != null)
			{
				effect_item_treasure.Emit();
			}
		}
	}

	public void StartEmit(ItemEffectType effect)
	{
		switch (effect)
		{
		case ItemEffectType.AddHp:
			if (effect_item_ammoplus != null)
			{
				effect_item_ammoplus.Emit();
			}
			break;
		case ItemEffectType.MoveSpeedUp:
			if (effect_item_spdup != null)
			{
				effect_item_spdup.Emit();
			}
			if (effect_item_spdup_2 != null)
			{
				effect_item_spdup_2.StartEmit();
			}
			break;
		case ItemEffectType.PowerUp:
			if (effect_item_powup != null)
			{
				effect_item_powup.Emit();
			}
			if (effect_item_powup_2 != null)
			{
				effect_item_powup_2.StartEmit();
			}
			break;
		case ItemEffectType.DefUp:
			if (effect_item_defup != null)
			{
				effect_item_defup.Emit();
			}
			if (effect_item_defup_2 != null)
			{
				effect_item_defup_2.StartEmit();
			}
			break;
		case ItemEffectType.DefUpFull:
			if (effect_item_fullmetaljacket1 != null)
			{
				effect_item_fullmetaljacket1.Emit();
			}
			if (effect_item_fullmetaljacket2 != null)
			{
				effect_item_fullmetaljacket2.Play();
			}
			break;
		case ItemEffectType.GodLike:
			if (effect_item_godlike != null)
			{
				effect_item_godlike.Emit();
			}
			if (effect_item_godlike_2 != null)
			{
				effect_item_godlike_2.StartEmit();
			}
			break;
		case ItemEffectType.Resurrection:
			if (effect_item_resurrection != null)
			{
				effect_item_resurrection.Emit();
			}
			break;
		}
	}

	public void StopEmit(ItemEffectType effect)
	{
		switch (effect)
		{
		case ItemEffectType.MoveSpeedUp:
			effect_item_spdup_2.StopEmit();
			break;
		case ItemEffectType.PowerUp:
			effect_item_powup_2.StopEmit();
			break;
		case ItemEffectType.DefUp:
			effect_item_defup_2.StopEmit();
			break;
		case ItemEffectType.GodLike:
			effect_item_godlike_2.StopEmit();
			break;
		case ItemEffectType.DefUpFull:
			effect_item_fullmetaljacket2.Stop();
			break;
		}
	}
}

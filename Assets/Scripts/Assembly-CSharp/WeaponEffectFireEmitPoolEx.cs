using UnityEngine;

public class WeaponEffectFireEmitPoolEx
{
	private SpecialEffectParticleEmitPool m_effect;

	private SpecialEffectParticleEmitPool m_effect_back;

	public WeaponEffectFireEmitPoolEx(GameObject weapon)
	{
		GameObject gameObject = weapon.transform.Find("Bone_Weapon/FirePoint").gameObject;
		if (gameObject.transform.childCount > 0)
		{
			m_effect = gameObject.transform.GetChild(0).GetComponent<SpecialEffectParticleEmitPool>();
		}
		GameObject gameObject2 = weapon.transform.Find("Bone_Weapon/FlamePoint").gameObject;
		if (gameObject2.transform.childCount > 0)
		{
			m_effect_back = gameObject2.transform.GetChild(0).GetComponent<SpecialEffectParticleEmitPool>();
		}
	}

	public void Emit()
	{
		if (m_effect != null)
		{
			m_effect.Emit();
		}
		if (m_effect_back != null)
		{
			m_effect_back.Emit();
		}
	}
}

using UnityEngine;

public class WeaponEffectFireEmitPool
{
	private SpecialEffectParticleEmitPool m_effect;

	public WeaponEffectFireEmitPool(GameObject weapon)
	{
		GameObject gameObject = weapon.transform.Find("Bone_Weapon/FirePoint").gameObject;
		if (gameObject.transform.childCount > 0)
		{
			m_effect = gameObject.transform.GetChild(0).GetComponent<SpecialEffectParticleEmitPool>();
		}
	}

	public void Emit()
	{
		if (m_effect != null)
		{
			m_effect.Emit();
		}
	}
}

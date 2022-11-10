using UnityEngine;

public class WeaponEffectCartridge
{
	protected SpecialEffectAnimationEmitGroupOneByOne m_effect;

	public WeaponEffectCartridge(GameObject weapon)
	{
		Transform transform = weapon.transform.Find("Bone_Weapon/EjectionPortCover");
		if (transform != null && transform.childCount > 0)
		{
			m_effect = transform.GetChild(0).GetComponent<SpecialEffectAnimationEmitGroupOneByOne>();
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

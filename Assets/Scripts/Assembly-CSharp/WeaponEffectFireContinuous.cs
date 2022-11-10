using UnityEngine;

public class WeaponEffectFireContinuous
{
	private SpecialEffectParticleContinuous m_effect;

	public WeaponEffectFireContinuous(GameObject weapon)
	{
		GameObject gameObject = weapon.transform.Find("Bone_Weapon/FirePoint").gameObject;
		if (gameObject.transform.childCount > 0)
		{
			m_effect = gameObject.transform.GetChild(0).GetComponent<SpecialEffectParticleContinuous>();
		}
	}

	public void Start()
	{
		if (m_effect != null)
		{
			m_effect.StartEmit();
		}
	}

	public void Stop()
	{
		if (m_effect != null)
		{
			m_effect.StopEmit();
		}
	}
}

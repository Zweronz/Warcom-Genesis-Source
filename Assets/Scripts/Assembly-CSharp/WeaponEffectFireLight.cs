using UnityEngine;

public class WeaponEffectFireLight : MonoBehaviour
{
	private float m_time = 0.1f;

	private void Update()
	{
		m_time -= Time.deltaTime;
		if (m_time < 0f)
		{
			base.gameObject.SetActiveRecursively(false);
			m_time = 0.1f;
		}
	}
}

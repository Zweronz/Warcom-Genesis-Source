using UnityEngine;

public class FireHitPoint : MonoBehaviour
{
	public float m_lifeTime;

	public void Update()
	{
		m_lifeTime -= Time.deltaTime;
		if (m_lifeTime < 0f)
		{
			base.gameObject.SetActiveRecursively(false);
		}
	}
}

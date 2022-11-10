using UnityEngine;

public class FireLine : MonoBehaviour
{
	public float m_lifeTime;

	public void Update()
	{
		base.transform.position = base.transform.position + base.transform.right * Time.deltaTime * -120f;
		m_lifeTime -= Time.deltaTime;
		if (m_lifeTime < 0f)
		{
			base.gameObject.SetActiveRecursively(false);
		}
	}
}

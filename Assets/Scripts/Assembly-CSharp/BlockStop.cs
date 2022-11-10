using UnityEngine;

public class BlockStop : MonoBehaviour
{
	private float m_time;

	public void Update()
	{
		m_time += Time.deltaTime;
		base.transform.LookAt(Camera.main.transform);
		if (m_time > 1f)
		{
			if (base.transform.localScale == Vector3.zero)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = Vector3.zero;
			}
			m_time = 0f;
		}
	}
}

using UnityEngine;

public class UtilUILevelTipsText : MonoBehaviour
{
	public float m_floatSpeed;

	public float m_alphaSpeed;

	public float m_totalTime;

	private float m_time;

	private TUIMeshText m_text;

	public void Update()
	{
		if (m_time < m_totalTime)
		{
			m_time += Time.deltaTime;
			if (m_text != null)
			{
				m_text.color = new Color(1f, 1f, 1f, Mathf.Max(0f, 1f - m_time * m_alphaSpeed));
			}
			base.transform.localPosition = new Vector3(0f, base.transform.localPosition.y + Time.deltaTime * m_floatSpeed, 0f);
		}
		else
		{
			base.transform.gameObject.SetActiveRecursively(false);
			m_time = 0f;
			base.transform.localPosition = Vector3.zero;
		}
	}

	public void SetText(string text)
	{
		m_time = 0f;
		base.transform.localPosition = Vector3.zero;
		base.transform.gameObject.SetActiveRecursively(true);
		m_text = base.transform.GetComponent<TUIMeshText>();
		m_text.text = text;
		m_text.UpdateMesh();
	}
}

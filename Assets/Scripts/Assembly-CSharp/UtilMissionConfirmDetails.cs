using UnityEngine;

public class UtilMissionConfirmDetails : MonoBehaviour
{
	public TUIMeshText m_text;

	private string m_lastText;

	private float m_length;

	private Mesh m_mesh;

	private float m_speed = 50f;

	private float m_totalDistance;

	private float m_startPositionX;

	private void Start()
	{
		m_mesh = base.transform.GetComponent<MeshFilter>().mesh;
		m_startPositionX = base.transform.position.x;
		m_lastText = m_text.text;
	}

	private void Update()
	{
		if (m_text.text != m_lastText)
		{
			m_length = m_mesh.bounds.size.x;
			base.transform.position = new Vector3(m_startPositionX, base.transform.position.y, base.transform.position.z);
			m_lastText = m_text.text;
		}
		if (m_text.text != string.Empty)
		{
			if (m_totalDistance < m_length + 340f)
			{
				m_totalDistance += m_speed * Time.deltaTime;
				base.transform.position = new Vector3(base.transform.position.x - m_speed * Time.deltaTime, base.transform.position.y, base.transform.position.z);
			}
			else
			{
				base.transform.position = new Vector3(m_startPositionX, base.transform.position.y, base.transform.position.z);
				m_totalDistance = 0f;
			}
		}
	}
}

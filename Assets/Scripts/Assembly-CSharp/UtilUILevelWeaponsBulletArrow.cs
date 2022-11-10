using UnityEngine;

public class UtilUILevelWeaponsBulletArrow : MonoBehaviour
{
	public GameObject[] frameArray;

	public float m_time;

	private int m_currentFrame;

	private float m_interTime;

	private void Start()
	{
		m_interTime = m_time;
	}

	public void Update()
	{
		m_interTime -= Time.deltaTime;
		if (m_interTime < 0f)
		{
			m_currentFrame++;
			m_interTime = m_time;
		}
		for (int i = 0; i < frameArray.Length; i++)
		{
			if (m_currentFrame % (frameArray.Length + 1) == i)
			{
				frameArray[i].SetActiveRecursively(true);
			}
			else
			{
				frameArray[i].SetActiveRecursively(false);
			}
		}
	}
}

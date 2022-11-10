using UnityEngine;

public class StopMarkHide : MonoBehaviour
{
	private int updateLimit;

	public GameObject stopMark;

	private Player m_player;

	private float distanceToHide = 18.5f;

	private void Start()
	{
	}

	private void Update()
	{
		if (m_player == null)
		{
			updateLimit = 0;
			m_player = GameManager.Instance().GetLevel().GetPlayer();
			return;
		}
		updateLimit++;
		if (updateLimit > 10)
		{
			if (stopMark.active && Vector3.Distance(stopMark.transform.position, m_player.GetTransform().position) > distanceToHide + 3f)
			{
				stopMark.active = false;
			}
			else if (!stopMark.active && Vector3.Distance(stopMark.transform.position, m_player.GetTransform().position) < distanceToHide - 3f)
			{
				stopMark.active = true;
			}
			updateLimit = 0;
		}
	}
}

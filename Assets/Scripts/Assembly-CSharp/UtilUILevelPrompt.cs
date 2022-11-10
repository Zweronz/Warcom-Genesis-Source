using UnityEngine;

public class UtilUILevelPrompt : MonoBehaviour
{
	public TUIMeshText m_promptText;

	public GameObject m_clock;

	public GameObject m_skull;

	public GameObject m_treasureBox;

	public TUIMeshText m_countText;

	public bool isWeb;

	private float m_currentTime;

	private int count;

	public void Start()
	{
		if (!isWeb)
		{
			if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Fight)
			{
				m_skull.SetActiveRecursively(true);
			}
			else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Find)
			{
				m_treasureBox.SetActiveRecursively(true);
			}
			else
			{
				m_clock.SetActiveRecursively(true);
			}
		}
		else
		{
			m_clock.SetActiveRecursively(true);
		}
	}

	public void Update()
	{
		if (!isWeb)
		{
			DataCenter.StateSingle().m_missionTempData.time += Time.deltaTime;
			if (GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.StartMode)
			{
				return;
			}
			if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Fight)
			{
				count++;
				if (count > 4)
				{
					if (m_promptText.text != DataCenter.StateSingle().m_missionTempData.emenyKill + "/" + DataCenter.StateSingle().GetLevelConfFight().GetEnemyCount())
					{
						DataCenter.StateSingle().m_missionTempData.completeness = (float)DataCenter.StateSingle().m_missionTempData.emenyKill / (float)DataCenter.StateSingle().GetLevelConfFight().GetEnemyCount();
						m_promptText.text = DataCenter.StateSingle().m_missionTempData.emenyKill + "/" + DataCenter.StateSingle().GetLevelConfFight().GetEnemyCount();
						m_promptText.UpdateMesh();
					}
					count = 0;
				}
				return;
			}
			if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Find)
			{
				count++;
				if (count > 4)
				{
					if (m_promptText.text != DataCenter.StateSingle().m_missionTempData.treasureBox + "/" + DataCenter.StateSingle().GetLevelConfFind().GetDropItemMissionGoalCount())
					{
						DataCenter.StateSingle().m_missionTempData.completeness = (float)DataCenter.StateSingle().m_missionTempData.treasureBox / (float)DataCenter.StateSingle().GetLevelConfFind().GetDropItemMissionGoalCount();
						m_promptText.text = DataCenter.StateSingle().m_missionTempData.treasureBox + "/" + DataCenter.StateSingle().GetLevelConfFind().GetDropItemMissionGoalCount();
						m_promptText.UpdateMesh();
					}
					count = 0;
				}
				return;
			}
			if (count == 0)
			{
				m_currentTime = DataCenter.StateSingle().GetLevelConfSurvive().GetTimeLimit();
			}
			count++;
			m_currentTime -= Time.deltaTime;
			if (count > 5)
			{
				m_clock.transform.Find("TimeFront").GetComponent<TUIMeshSector>().angleEnd = 360f * (DataCenter.StateSingle().GetLevelConfSurvive().GetTimeLimit() - m_currentTime) / DataCenter.StateSingle().GetLevelConfSurvive().GetTimeLimit();
				m_clock.transform.Find("TimeFront").GetComponent<TUIMeshSector>().Update();
				m_currentTime = Mathf.Max(0f, m_currentTime);
				int num = (int)m_currentTime / 60;
				int num2 = (int)m_currentTime % 60;
				if (m_promptText.text != string.Format("{0:D2}", num) + ":" + string.Format("{0:D2}", num2))
				{
					DataCenter.StateSingle().m_missionTempData.completeness = 1f - m_currentTime / DataCenter.StateSingle().GetLevelConfSurvive().GetTimeLimit();
					m_promptText.text = string.Format("{0:D2}", num) + ":" + string.Format("{0:D2}", num2);
					m_promptText.UpdateMesh();
				}
				count = 1;
			}
			return;
		}
		if (count == 0)
		{
			m_currentTime = DataCenter.StateMulti().GetLevelConfDeathMatch().GetTimeLimit();
		}
		count++;
		m_currentTime -= Time.deltaTime;
		if (count > 5)
		{
			m_clock.transform.Find("TimeFront").GetComponent<TUIMeshSector>().angleEnd = 360f * (DataCenter.StateMulti().GetLevelConfDeathMatch().GetTimeLimit() - m_currentTime) / DataCenter.StateMulti().GetLevelConfDeathMatch().GetTimeLimit();
			m_clock.transform.Find("TimeFront").GetComponent<TUIMeshSector>().Update();
			m_currentTime = Mathf.Max(0f, m_currentTime);
			int num3 = (int)m_currentTime / 60;
			int num4 = (int)m_currentTime % 60;
			if (m_promptText.text != string.Format("{0:D2}", num3) + ":" + string.Format("{0:D2}", num4))
			{
				DataCenter.StateSingle().m_missionTempData.completeness = 1f - m_currentTime / DataCenter.StateMulti().GetLevelConfDeathMatch().GetTimeLimit();
				m_promptText.text = string.Format("{0:D2}", num3) + ":" + string.Format("{0:D2}", num4);
				m_promptText.UpdateMesh();
			}
			count = 1;
			if (m_countText.text != "Kills:" + DataCenter.StateMulti().m_missionTempData.emenyKill)
			{
				m_countText.text = "Kills:" + DataCenter.StateMulti().m_missionTempData.emenyKill;
				m_countText.UpdateMesh();
			}
		}
	}
}

using UnityEngine;

public class UtilUILevelMap : MonoBehaviour
{
	public TUIMeshCircleEx m_map;

	public GameObject m_viewDirc;

	public GameObject[] m_enemyPointArray;

	public GameObject[] m_enemyArrowArray;

	public GameObject[] m_dropItemPointArray;

	public GameObject[] m_dropItemMissionGoalPointArray;

	public GameObject[] m_dropItemMissionGoalArrowArray;

	public bool isWeb;

	private Player m_player;

	private int m_count;

	private float m_xPro;

	private float m_yPro;

	private float m_xOffset;

	private float m_yOffset;

	private float m_facter;

	private float m_scale = 1f;

	private float m_viewDistance = 18.5f;

	public void Start()
	{
		if (!isWeb)
		{
			if (DataCenter.Save().GetWeek() == -2 || DataCenter.Save().GetWeek() == 0)
			{
				m_map.frameName = "TutorialLevel01";
			}
			else if (DataCenter.Save().GetWeek() == -1)
			{
				m_map.frameName = "TutorialLevel02";
			}
			else
			{
				m_map.frameName = DataCenter.StateSingle().GetLevelInfo().scene.ToString();
			}
		}
		else
		{
			m_map.frameName = DataCenter.StateMulti().GetCurrentScene().ToString();
		}
		m_map.UpdateMesh();
		if (m_map.frameName == "TutorialLevel01")
		{
			m_xPro = 4.29f;
			m_yPro = 4.3f;
			m_xOffset = 126.267f;
			m_yOffset = 123.35f;
			m_facter = 0.757f;
			m_scale = 0.336f;
		}
		else if (m_map.frameName == "TutorialLevel02")
		{
			m_xPro = 3.18f;
			m_yPro = 3.18f;
			m_xOffset = 198f;
			m_yOffset = -50f;
			m_facter = 0.865f;
			m_scale = 0.375f;
		}
		else if (m_map.frameName == "Level01")
		{
			m_xPro = 2.268f;
			m_yPro = 2.268f;
			m_xOffset = 120.776f;
			m_yOffset = 142.217f;
			m_facter = 1f;
			m_scale = 0.432f;
		}
		else if (m_map.frameName == "Level02")
		{
			m_xPro = 1.522f;
			m_yPro = 1.524f;
			m_xOffset = 140.856f;
			m_yOffset = 119.543f;
			m_facter = 1f;
			m_scale = 0.703f;
		}
		else if (m_map.frameName == "Level03")
		{
			m_xPro = 1.894f;
			m_yPro = 1.895f;
			m_xOffset = 159.784f;
			m_yOffset = 14.587f;
			m_facter = 1f;
			m_scale = 0.578f;
		}
		else if (m_map.frameName == "Level04")
		{
			m_xPro = 1.327f;
			m_yPro = 1.338f;
			m_xOffset = 95.473f;
			m_yOffset = 13.96f;
			m_facter = 1f;
			m_scale = 0.757f;
		}
		m_xPro *= 2f;
		m_yPro *= 2f;
		m_xOffset *= 2f;
		m_yOffset *= 2f;
	}

	public void Update()
	{
		m_count++;
		if (m_player != null)
		{
			if (m_player.GetTransform() == null)
			{
				return;
			}
			m_map.textureOffset = new Vector2(m_player.GetTransform().position.x * (0f - m_xPro) + m_xOffset, m_player.GetTransform().position.z * m_yPro + m_yOffset);
			Quaternion rotation = Quaternion.FromToRotation(new Vector3(0f, -1f, 0f), new Vector3(m_player.GetTransform().forward.x, 0f - m_player.GetTransform().forward.z, 0f));
			m_viewDirc.transform.rotation = rotation;
			if (m_count <= 3)
			{
				return;
			}
			if (!isWeb)
			{
				Enemy[] enemyNotDeadArray = GameManager.Instance().GetLevel().GetEnemyNotDeadArray();
				for (int i = 0; i < m_enemyPointArray.Length; i++)
				{
					m_enemyPointArray[i].transform.localPosition = new Vector3(0f, 32000f, 0f);
				}
				for (int j = 0; j < m_enemyArrowArray.Length; j++)
				{
					m_enemyArrowArray[j].transform.localPosition = new Vector3(0f, 32000f, 0f);
				}
				for (int k = 0; k < enemyNotDeadArray.Length; k++)
				{
					if (enemyNotDeadArray[k].GetTransform() != null)
					{
						Vector3 vector = enemyNotDeadArray[k].GetTransform().position - m_player.GetTransform().position;
						if (Vector3.Magnitude(new Vector3(vector.x, 0f, vector.z)) < m_facter * m_viewDistance)
						{
							m_enemyPointArray[k].transform.localPosition = new Vector3(m_scale * (enemyNotDeadArray[k].GetTransform().position.x * (0f - m_xPro) + m_xOffset - m_map.textureOffset.x), m_scale * (enemyNotDeadArray[k].GetTransform().position.z * m_yPro + m_yOffset - m_map.textureOffset.y), 0f);
							continue;
						}
						Vector3 toDirection = new Vector3(enemyNotDeadArray[k].GetTransform().position.x * (0f - m_xPro) + m_xOffset - m_map.textureOffset.x, enemyNotDeadArray[k].GetTransform().position.z * m_yPro + m_yOffset - m_map.textureOffset.y, 0f);
						m_enemyArrowArray[k].transform.localPosition = toDirection.normalized * 46f;
						m_enemyArrowArray[k].transform.localRotation = Quaternion.FromToRotation(new Vector3(0f, -1f, 0f), toDirection);
					}
				}
			}
			else
			{
				EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
				for (int l = 0; l < m_enemyPointArray.Length; l++)
				{
					m_enemyPointArray[l].transform.localPosition = new Vector3(0f, 32000f, 0f);
				}
				for (int m = 0; m < m_enemyArrowArray.Length; m++)
				{
					m_enemyArrowArray[m].transform.localPosition = new Vector3(0f, 32000f, 0f);
				}
				for (int n = 0; n < enemyNetArray.Length; n++)
				{
					if (enemyNetArray[n].GetTransform() != null)
					{
						Vector3 vector2 = enemyNetArray[n].GetTransform().position - m_player.GetTransform().position;
						if (Vector3.Magnitude(new Vector3(vector2.x, 0f, vector2.z)) < m_facter * m_viewDistance)
						{
							m_enemyPointArray[n].transform.localPosition = new Vector3(m_scale * (enemyNetArray[n].GetTransform().position.x * (0f - m_xPro) + m_xOffset - m_map.textureOffset.x), m_scale * (enemyNetArray[n].GetTransform().position.z * m_yPro + m_yOffset - m_map.textureOffset.y), 0f);
							continue;
						}
						Vector3 toDirection2 = new Vector3(enemyNetArray[n].GetTransform().position.x * (0f - m_xPro) + m_xOffset - m_map.textureOffset.x, enemyNetArray[n].GetTransform().position.z * m_yPro + m_yOffset - m_map.textureOffset.y, 0f);
						m_enemyArrowArray[n].transform.localPosition = toDirection2.normalized * 46f;
						m_enemyArrowArray[n].transform.localRotation = Quaternion.FromToRotation(new Vector3(0f, -1f, 0f), toDirection2);
					}
				}
			}
			DropItem[] dropItemArray = GameManager.Instance().GetLevel().GetDropItemArray();
			DropItem[] dropItemMissionGoalArray = GameManager.Instance().GetLevel().GetDropItemMissionGoalArray();
			for (int num = 0; num < m_dropItemPointArray.Length; num++)
			{
				m_dropItemPointArray[num].transform.localPosition = new Vector3(0f, 32000f, 0f);
			}
			for (int num2 = 0; num2 < dropItemArray.Length; num2++)
			{
				if (num2 < m_dropItemPointArray.Length && dropItemArray[num2].GetTransform() != null)
				{
					Vector3 vector3 = dropItemArray[num2].GetTransform().position - m_player.GetTransform().position;
					if (Vector3.Magnitude(new Vector3(vector3.x, 0f, vector3.z)) < m_facter * m_viewDistance)
					{
						m_dropItemPointArray[num2].transform.localPosition = new Vector3(m_scale * (dropItemArray[num2].GetTransform().position.x * (0f - m_xPro) + m_xOffset - m_map.textureOffset.x), m_scale * (dropItemArray[num2].GetTransform().position.z * m_yPro + m_yOffset - m_map.textureOffset.y), 0f);
					}
				}
			}
			for (int num3 = 0; num3 < m_dropItemMissionGoalPointArray.Length; num3++)
			{
				m_dropItemMissionGoalPointArray[num3].transform.localPosition = new Vector3(0f, 32000f, 0f);
			}
			for (int num4 = 0; num4 < m_dropItemMissionGoalArrowArray.Length; num4++)
			{
				m_dropItemMissionGoalArrowArray[num4].transform.localPosition = new Vector3(0f, 32000f, 0f);
			}
			for (int num5 = 0; num5 < dropItemMissionGoalArray.Length; num5++)
			{
				if (dropItemMissionGoalArray[num5].GetTransform() != null)
				{
					Vector3 vector4 = dropItemMissionGoalArray[num5].GetTransform().position - m_player.GetTransform().position;
					if (Vector3.Magnitude(new Vector3(vector4.x, 0f, vector4.z)) < m_facter * m_viewDistance)
					{
						m_dropItemMissionGoalPointArray[num5].transform.localPosition = new Vector3(m_scale * (dropItemMissionGoalArray[num5].GetTransform().position.x * (0f - m_xPro) + m_xOffset - m_map.textureOffset.x), m_scale * (dropItemMissionGoalArray[num5].GetTransform().position.z * m_yPro + m_yOffset - m_map.textureOffset.y), 0f);
						continue;
					}
					Vector3 toDirection3 = new Vector3(dropItemMissionGoalArray[num5].GetTransform().position.x * (0f - m_xPro) + m_xOffset - m_map.textureOffset.x, dropItemMissionGoalArray[num5].GetTransform().position.z * m_yPro + m_yOffset - m_map.textureOffset.y, 0f);
					m_dropItemMissionGoalArrowArray[num5].transform.localPosition = toDirection3.normalized * 46f;
					m_dropItemMissionGoalArrowArray[num5].transform.localRotation = Quaternion.FromToRotation(new Vector3(0f, -1f, 0f), toDirection3);
				}
			}
			m_count = 0;
			m_map.UpdateMesh();
		}
		else
		{
			m_player = GameManager.Instance().GetLevel().GetPlayer();
			Quaternion rotation2 = Quaternion.FromToRotation(new Vector3(0f, -1f, 0f), new Vector3(m_player.GetTransform().forward.x, 0f - m_player.GetTransform().forward.z, 0f));
			m_viewDirc.transform.rotation = rotation2;
		}
	}
}

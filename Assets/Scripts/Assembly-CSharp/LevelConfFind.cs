using System.Collections.Generic;
using System.Xml;

public class LevelConfFind : LevelConf
{
	private PlayerSpawnPointConf m_playerSpawnPoint;

	private List<Block> m_block = new List<Block>();

	private List<EnemyWave> m_enemyWave = new List<EnemyWave>();

	private Dictionary<int, PatrollingParam> m_patrollingParam = new Dictionary<int, PatrollingParam>();

	public LevelConfFind(string file)
	{
		string xml = FileUtil.LoadResourcesFile(file);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_playerSpawnPoint = LoadPlayerSpawnPoint((XmlElement)documentElement.GetElementsByTagName("Player").Item(0));
		m_block = LoadBlockList((XmlElement)documentElement.GetElementsByTagName("Blocks").Item(0));
		m_enemyWave = LoadEnemyWaveList((XmlElement)documentElement.GetElementsByTagName("Waves").Item(0));
		m_patrollingParam = LoadPatrollingParamList((XmlElement)documentElement.GetElementsByTagName("PatrollingParams").Item(0));
	}

	public List<Block> GetBlockInfo()
	{
		return m_block;
	}

	public PlayerSpawnPointConf GetPlayerSpawnPoint()
	{
		return m_playerSpawnPoint;
	}

	public int GetWaveCount()
	{
		return m_enemyWave.Count;
	}

	public int GetEnemyCount()
	{
		int num = 0;
		for (int i = 0; i < m_enemyWave.Count; i++)
		{
			num += m_enemyWave[i].enemy_list.Count;
		}
		return num;
	}

	public int GetDropItemMissionGoalCount()
	{
		int num = 0;
		for (int i = 0; i < m_enemyWave.Count; i++)
		{
			for (int j = 0; j < m_enemyWave[i].dropItem_list.Count; j++)
			{
				if (m_enemyWave[i].dropItem_list[j].name == "MissionGoal01")
				{
					num++;
				}
			}
		}
		return num;
	}

	public EnemyWave GetWaveEnemy(int wave)
	{
		return m_enemyWave[wave];
	}

	public PatrollingParam GetPatrollingParam(int id)
	{
		if (m_patrollingParam.ContainsKey(id))
		{
			return m_patrollingParam[id];
		}
		return null;
	}
}

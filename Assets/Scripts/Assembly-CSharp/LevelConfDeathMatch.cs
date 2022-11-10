using System.Collections.Generic;
using System.Xml;

public class LevelConfDeathMatch : LevelConfNet
{
	private PlayerSpawnPointConf[] m_playerSpawnPointArray = new PlayerSpawnPointConf[8];

	private List<Block> m_block = new List<Block>();

	private float m_dropItemRefreshTime;

	private List<WaveNet> m_wave = new List<WaveNet>();

	private List<string> m_dropItemPoint = new List<string>();

	private List<ResurrectionPoint> m_resurrectionPoint = new List<ResurrectionPoint>();

	private float m_time;

	public LevelConfDeathMatch(string file)
	{
		string xml = FileUtil.LoadResourcesFile(file);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement documentElement = xmlDocument.DocumentElement;
		for (int i = 0; i < 8; i++)
		{
			m_playerSpawnPointArray[i] = LoadPlayerSpawnPoint((XmlElement)documentElement.GetElementsByTagName("Player" + string.Format("{0:D2}", i + 1)).Item(0));
		}
		m_wave = LoatWaveNetList((XmlElement)documentElement.GetElementsByTagName("Waves").Item(0));
		m_dropItemPoint = LoadDropItemPoints((XmlElement)documentElement.GetElementsByTagName("DropItemPoints").Item(0));
		m_dropItemRefreshTime = LoadDropItemRefreshTime((XmlElement)documentElement.GetElementsByTagName("DropItemRefreshTime").Item(0));
		m_resurrectionPoint = LoadResurrectionPoints((XmlElement)documentElement.GetElementsByTagName("ResurrectionPoints").Item(0));
		m_block = LoadBlockList((XmlElement)documentElement.GetElementsByTagName("Blocks").Item(0));
		m_time = LoadTimeLimit((XmlElement)documentElement.GetElementsByTagName("TimeLimit").Item(0));
	}

	public List<Block> GetBlockInfo()
	{
		return m_block;
	}

	public PlayerSpawnPointConf GetPlayerSpawnPointByID(int index)
	{
		return m_playerSpawnPointArray[index];
	}

	public float GetTimeLimit()
	{
		return m_time;
	}

	public WaveNet GetWaveNet(int index)
	{
		return m_wave[index];
	}

	public int GetWaveCount()
	{
		return m_wave.Count;
	}

	public ResurrectionPoint[] GetResurrectionPointArray()
	{
		return m_resurrectionPoint.ToArray();
	}

	public string[] GetDropItemPointArray()
	{
		return m_dropItemPoint.ToArray();
	}

	public float GetDropItemRefreshTime()
	{
		return m_dropItemRefreshTime;
	}
}

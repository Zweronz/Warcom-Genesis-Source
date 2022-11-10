using System.Collections.Generic;
using System.Xml;

public class LevelConf
{
	public class PlayerSpawnPointConf
	{
		public string spawnPoint;

		public float spawnDirection;

		public PlayerSpawnPointConf(string spawnPoint, float spawnDirection)
		{
			this.spawnPoint = spawnPoint;
			this.spawnDirection = spawnDirection;
		}
	}

	public class EnemyConf
	{
		public string name;

		public string spawnPoint;

		public int aiParam;

		public int patrollingParam;

		public EnemyConf(string name, string spawnPoint, int aiParam, int patrollingParam)
		{
			this.name = name;
			this.spawnPoint = spawnPoint;
			this.aiParam = aiParam;
			this.patrollingParam = patrollingParam;
		}
	}

	public class DropItemConf
	{
		public string name;

		public float spawnRate;

		public string[] spawnPoints;

		public DropItemConf(string name, string[] spawnPoints, float spawnRate)
		{
			this.name = name;
			this.spawnRate = spawnRate;
			this.spawnPoints = spawnPoints;
		}
	}

	public class EnemyWave
	{
		public string id;

		public List<EnemyConf> enemy_list = new List<EnemyConf>();

		public List<DropItemConf> dropItem_list = new List<DropItemConf>();

		public EnemyWave(string id, List<EnemyConf> enemy_list, List<DropItemConf> dropItem_list)
		{
			this.id = id;
			this.enemy_list = enemy_list;
			this.dropItem_list = dropItem_list;
		}
	}

	public class Block
	{
		public int id;

		public string name;

		public string enable;

		public Block(int id, string name, string enable)
		{
			this.id = id;
			this.name = name;
			this.enable = enable;
		}
	}

	public class PatrollingParam
	{
		public int id;

		public float stayTime;

		public bool loop;

		public string[] points;

		public PatrollingParam(int id, float stayTime, bool loop, string[] points)
		{
			this.id = id;
			this.stayTime = stayTime;
			this.loop = loop;
			this.points = points;
		}
	}

	public EnemyConf LoadEnemy(XmlElement enemy)
	{
		string attribute = enemy.GetAttribute("Name");
		string attribute2 = enemy.GetAttribute("SpawnPoint");
		int aiParam = int.Parse(enemy.GetAttribute("AIParam"));
		int patrollingParam = int.Parse(enemy.GetAttribute("PatrollingParam"));
		return new EnemyConf(attribute, attribute2, aiParam, patrollingParam);
	}

	public DropItemConf LoadDropItem(XmlElement dropItem)
	{
		string attribute = dropItem.GetAttribute("Name");
		float spawnRate = float.Parse(dropItem.GetAttribute("SpawnRate"));
		string[] spawnPoints = dropItem.GetAttribute("SpawnPoints").Split('/');
		return new DropItemConf(attribute, spawnPoints, spawnRate);
	}

	public EnemyWave LoadEnemyWave(XmlElement wave)
	{
		string attribute = wave.GetAttribute("Id");
		XmlElement xmlElement = (XmlElement)wave.GetElementsByTagName("Enemys").Item(0);
		List<EnemyConf> list = new List<EnemyConf>();
		foreach (XmlElement item in xmlElement.GetElementsByTagName("Enemy"))
		{
			list.Add(LoadEnemy(item));
		}
		XmlElement xmlElement2 = (XmlElement)wave.GetElementsByTagName("DropItems").Item(0);
		List<DropItemConf> list2 = new List<DropItemConf>();
		foreach (XmlElement item2 in xmlElement2.GetElementsByTagName("DropItem"))
		{
			list2.Add(LoadDropItem(item2));
		}
		return new EnemyWave(attribute, list, list2);
	}

	public List<EnemyWave> LoadEnemyWaveList(XmlElement waves)
	{
		List<EnemyWave> list = new List<EnemyWave>();
		foreach (XmlElement item in waves.GetElementsByTagName("Wave"))
		{
			list.Add(LoadEnemyWave(item));
		}
		return list;
	}

	public Block LoadBlock(XmlElement block)
	{
		int id = int.Parse(block.GetAttribute("Id"));
		string attribute = block.GetAttribute("Name");
		string attribute2 = block.GetAttribute("Enable");
		return new Block(id, attribute, attribute2);
	}

	public List<Block> LoadBlockList(XmlElement blocks)
	{
		List<Block> list = new List<Block>();
		foreach (XmlElement item in blocks.GetElementsByTagName("Block"))
		{
			list.Add(LoadBlock(item));
		}
		return list;
	}

	public string[] GetBlockNameArray(List<Block> blockinfo)
	{
		string[] array = new string[blockinfo.Count];
		for (int i = 0; i < blockinfo.Count; i++)
		{
			if (blockinfo[i].enable == "1")
			{
				array[i] = blockinfo[i].name;
			}
		}
		return array;
	}

	public PatrollingParam LoadPatrollingParam(XmlElement patrollingParam)
	{
		int id = int.Parse(patrollingParam.GetAttribute("Id"));
		float stayTime = float.Parse(patrollingParam.GetAttribute("StayTime"));
		bool loop = bool.Parse(patrollingParam.GetAttribute("Loop"));
		string[] points = patrollingParam.GetAttribute("Points").Split('/');
		return new PatrollingParam(id, stayTime, loop, points);
	}

	public Dictionary<int, PatrollingParam> LoadPatrollingParamList(XmlElement patrollingParams)
	{
		Dictionary<int, PatrollingParam> dictionary = new Dictionary<int, PatrollingParam>();
		foreach (XmlElement item in patrollingParams.GetElementsByTagName("PatrollingParam"))
		{
			PatrollingParam patrollingParam2 = LoadPatrollingParam(item);
			dictionary.Add(patrollingParam2.id, patrollingParam2);
		}
		return dictionary;
	}

	public PlayerSpawnPointConf LoadPlayerSpawnPoint(XmlElement player)
	{
		string attribute = player.GetAttribute("SpawnPoint");
		float spawnDirection = float.Parse(player.GetAttribute("Direction"));
		return new PlayerSpawnPointConf(attribute, spawnDirection);
	}

	public List<string> LoadSavePoints(XmlElement savePoints)
	{
		List<string> list = new List<string>();
		foreach (XmlElement item in savePoints.GetElementsByTagName("Safepoint"))
		{
			list.Add(item.GetAttribute("Point"));
		}
		return list;
	}

	public float LoadTimeLimit(XmlElement timeLimit)
	{
		return float.Parse(timeLimit.GetAttribute("Value"));
	}
}

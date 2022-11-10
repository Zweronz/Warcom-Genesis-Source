using System.Collections.Generic;

public class GameLevel
{
	private Player m_player;

	private List<Ally> m_ally;

	private List<Enemy> m_enemy;

	private List<DropItem> m_dropItem;

	private List<EnemyNet> m_enemyNet;

	public void StartGame()
	{
		m_player = null;
		m_ally = new List<Ally>();
		m_enemy = new List<Enemy>();
		m_dropItem = new List<DropItem>();
		m_enemyNet = new List<EnemyNet>();
	}

	public void StopGame()
	{
	}

	public void UpdateGame(float deltaTime)
	{
		if (m_player != null)
		{
			m_player.Update(deltaTime);
		}
		List<Ally> list = new List<Ally>();
		for (int i = 0; i < m_ally.Count; i++)
		{
			m_ally[i].Update(deltaTime);
			if (m_ally[i].GetGameObject() == null)
			{
				list.Add(m_ally[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			m_ally.Remove(list[j]);
		}
		list.Clear();
		list = null;
		List<Enemy> list2 = new List<Enemy>();
		for (int k = 0; k < m_enemy.Count; k++)
		{
			m_enemy[k].Update(deltaTime);
			if (m_enemy[k].GetGameObject() == null)
			{
				list2.Add(m_enemy[k]);
			}
		}
		for (int l = 0; l < list2.Count; l++)
		{
			m_enemy.Remove(list2[l]);
		}
		list2.Clear();
		list2 = null;
		List<DropItem> list3 = new List<DropItem>();
		for (int m = 0; m < m_dropItem.Count; m++)
		{
			if (m_dropItem[m].GetGameObject() == null)
			{
				list3.Add(m_dropItem[m]);
			}
		}
		for (int n = 0; n < list3.Count; n++)
		{
			m_dropItem.Remove(list3[n]);
		}
		list3.Clear();
		list3 = null;
		List<EnemyNet> list4 = new List<EnemyNet>();
		for (int num = 0; num < m_enemyNet.Count; num++)
		{
			m_enemyNet[num].Update(deltaTime);
			if (m_enemyNet[num].GetGameObject() == null)
			{
				list4.Add(m_enemyNet[num]);
			}
		}
		for (int num2 = 0; num2 < list4.Count; num2++)
		{
			m_enemyNet.Remove(list4[num2]);
		}
		list4.Clear();
		list4 = null;
	}

	public void SetPlayer(Player player)
	{
		m_player = player;
	}

	public Player GetPlayer()
	{
		return m_player;
	}

	public void AddEnemy(Enemy enemy)
	{
		m_enemy.Add(enemy);
	}

	public int GetEnemyCount()
	{
		return m_enemy.Count;
	}

	public Enemy[] GetEnemyArray()
	{
		return m_enemy.ToArray();
	}

	public Enemy[] GetEnemyNotDeadArray()
	{
		List<Enemy> list = new List<Enemy>();
		for (int i = 0; i < m_enemy.Count; i++)
		{
			if (!m_enemy[i].m_dead)
			{
				list.Add(m_enemy[i]);
			}
		}
		return list.ToArray();
	}

	public DropItem[] GetDropItemMissionGoalArray()
	{
		List<DropItem> list = new List<DropItem>();
		for (int i = 0; i < m_dropItem.Count; i++)
		{
			if (m_dropItem[i].GetItemEffect().m_type == DropItemEffectType.MissionGoal)
			{
				list.Add(m_dropItem[i]);
			}
		}
		return list.ToArray();
	}

	public DropItem[] GetDropItemArray()
	{
		List<DropItem> list = new List<DropItem>();
		for (int i = 0; i < m_dropItem.Count; i++)
		{
			if (m_dropItem[i].GetItemEffect().m_type != 0)
			{
				list.Add(m_dropItem[i]);
			}
		}
		return list.ToArray();
	}

	public int GetEnemyInFightNum()
	{
		int num = 0;
		foreach (Enemy item in m_enemy)
		{
			if (item.GetIsFire())
			{
				num++;
			}
		}
		return num;
	}

	public void AddDropItem(DropItem dropItem)
	{
		m_dropItem.Add(dropItem);
	}

	public int GetDropItemCount()
	{
		return m_dropItem.Count;
	}

	public int GetDropItemMissionGoalCount()
	{
		int num = 0;
		for (int i = 0; i < m_dropItem.Count; i++)
		{
			if (m_dropItem[i].GetItemEffect().m_type == DropItemEffectType.MissionGoal)
			{
				num++;
			}
		}
		return num;
	}

	public void AddEnemyNet(EnemyNet enemyNet)
	{
		m_enemyNet.Add(enemyNet);
	}

	public EnemyNet[] GetEnemyNetArray()
	{
		return m_enemyNet.ToArray();
	}

	public EnemyNet GetEnemyNetByNetId(int netId)
	{
		foreach (EnemyNet item in m_enemyNet)
		{
			if (netId == item.m_netId)
			{
				return item;
			}
		}
		return null;
	}

	public void RemoveEnemyNetByNetId(int netId)
	{
		EnemyNet item = null;
		foreach (EnemyNet item2 in m_enemyNet)
		{
			if (netId == item2.m_netId)
			{
				item = item2;
			}
		}
		m_enemyNet.Remove(item);
	}
}

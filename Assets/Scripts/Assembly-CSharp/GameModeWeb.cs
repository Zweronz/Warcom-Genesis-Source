using System.Collections;
using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class GameModeWeb : MonoBehaviour
{
	private float playerDeadTime;

	public void Awake()
	{
		LevelConfDeathMatch levelConfDeathMatch = new LevelConfDeathMatch(string.Concat("Conf/", DataCenter.StateMulti().GetCurrentScene(), "Web/DeathMatch"));
		DataCenter.StateMulti().SetLevelConfDeathMatch(levelConfDeathMatch);
	}

	public void Start()
	{
		RunDeadMatch();
	}

	public void Update()
	{
		if (GameManager.Instance().GetLevel().GetPlayer()
			.m_dead)
		{
			playerDeadTime += Time.deltaTime;
			if (playerDeadTime > 3f)
			{
				GameManager.Instance().GetLevel().GetPlayer()
					.OnResurrection();
				playerDeadTime = 0f;
			}
		}
	}

	private void RunDeadMatch()
	{
		StartCoroutine(DeadMatchProc());
	}

	private IEnumerator DeadMatchProc()
	{
		LevelConfDeathMatch conf = DataCenter.StateMulti().GetLevelConfDeathMatch();
		InitializeBlock(conf.GetBlockInfo());
		string[] dpa = conf.GetDropItemPointArray();
		for (int j = 0; j < dpa.Length; j++)
		{
			Transform point = ScenePointManager.GetScenePointTransform(dpa[j]);
			if (point != null)
			{
				point.gameObject.AddComponent<ScenePointDropItemRefresh>();
			}
		}
		GameObject playerSpawnPoint = GameObject.Find(conf.GetPlayerSpawnPointByID(DataCenter.StateMulti().m_tnet.Myself.SitIndex).spawnPoint);
		Player playerNet = CreatePlayer(playerSpawnPoint.transform.position, conf.GetPlayerSpawnPointByID(DataCenter.StateMulti().m_tnet.Myself.SitIndex).spawnDirection);
		yield return 0;
		List<TNetUser> userlist = DataCenter.StateMulti().m_tnet.CurRoom.UserList;
		foreach (TNetUser nu in userlist)
		{
			if (nu.SitIndex != DataCenter.StateMulti().m_tnet.Myself.SitIndex)
			{
				GameObject spawnPoint = GameObject.Find(conf.GetPlayerSpawnPointByID(nu.SitIndex).spawnPoint);
				CreateEnemy(nu, spawnPoint.transform.position, conf.GetPlayerSpawnPointByID(nu.SitIndex).spawnDirection);
			}
		}
		float startTime = Time.time + 4f;
		bool timeOut = false;
		float dropItemRefreshTime = startTime;
		bool tipFordropItemRefresh = false;
		int waveIndex = 0;
		while (!timeOut)
		{
			while (!(Time.time - startTime > conf.GetTimeLimit()))
			{
				if (DataCenter.StateMulti().m_isRoomMaster)
				{
					if (Time.time - dropItemRefreshTime > conf.GetDropItemRefreshTime() - 3f && !tipFordropItemRefresh)
					{
						GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("New supplies delivered.");
						tipFordropItemRefresh = true;
					}
					if (Time.time - dropItemRefreshTime > conf.GetDropItemRefreshTime())
					{
						LevelConfNet.WaveNet wave = conf.GetWaveNet(waveIndex++ % conf.GetWaveCount());
						for (int i = 0; i < wave.dropItemNum; i++)
						{
							while (true)
							{
								bool allIsOccupied = true;
								for (int k = 0; k < dpa.Length; k++)
								{
									if (!ScenePointManager.GetScenePointTransform(dpa[k]).GetComponent<ScenePointDropItemRefresh>().m_isOccupied)
									{
										allIsOccupied = false;
									}
								}
								if (allIsOccupied)
								{
									break;
								}
								int p = Random.Range(0, dpa.Length * 3) % dpa.Length;
								if (!ScenePointManager.GetScenePointTransform(dpa[p]).GetComponent<ScenePointDropItemRefresh>().m_isOccupied)
								{
									string dropItemName = wave.dropItemName[Random.Range(0, wave.dropItemName.Length * 3) % wave.dropItemName.Length];
									string netId = waveIndex + "_" + i;
									CreateDropItem(dpa[p], dropItemName, netId);
									UtilsNet.SentDropItemRefresh(new NetDropItemRefresh(dropItemName, dpa[p], netId));
									ScenePointManager.GetScenePointTransform(dpa[p]).GetComponent<ScenePointDropItemRefresh>().m_isOccupied = true;
									break;
								}
							}
						}
						tipFordropItemRefresh = false;
						dropItemRefreshTime = Time.time;
					}
				}
				yield return new WaitForSeconds(1f);
			}
			timeOut = true;
		}
		yield return new WaitForSeconds(3f);
		BlankLoading.nextScene = "WorldMapWeb";
		UtilsNet.SendLeave();
		GameManager.Instance().GetUIWeb().SwitchScene("BlankLoading");
		yield return 0;
	}

	private PlayerNet CreatePlayer(Vector3 position, float direction)
	{
		int currentCharactor = DataCenter.StateMulti().GetCurrentCharactor();
		CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo(currentCharactor);
		CharacterInfo characterInfo = DataCenter.Conf().GetCharacterInfo(currentCharactor);
		string text = "NetPlayer" + DataCenter.StateMulti().m_tnet.Myself.SitIndex;
		PlayerNet playerNet = CharacterBuilder.CreatePlayerNet(characterInfo, text, position, direction);
		playerNet.AddWeapon(1, WeaponBuilder.CreateWeaponPlayer(characterEquipInfo.weapon01));
		playerNet.AddWeapon(2, WeaponBuilder.CreateWeaponPlayer(characterEquipInfo.weapon02));
		playerNet.AddWeapon(3, WeaponBuilder.CreateWeaponPlayer(characterEquipInfo.weapon03));
		playerNet.UseWeapon(1);
		playerNet.GetTransform().GetComponent<CharacterPassivePara>().ArmorEffect(characterEquipInfo);
		GameManager.Instance().GetCamera().SetCurrentCrosshairByWeaponInfo(DataCenter.Conf().GetWeaponInfoByName(characterEquipInfo.weapon01));
		GameManager.Instance().GetLevel().SetPlayer(playerNet);
		return playerNet;
	}

	private void CreateEnemy(TNetUser netUserVar, Vector3 position, float direction)
	{
		NetPlayerSettingInfo userVariable = UtilsNet.GetUserVariable(netUserVar);
		string text = "NetPlayer" + userVariable.seatID;
		CharacterInfo characterInfo = DataCenter.Conf().GetCharacterInfo(userVariable.cTypeID);
		EnemyNet enemyNet = CharacterBuilder.CreateEnemyNet(characterInfo, userVariable.modelName, text, userVariable.seatID, userVariable.netID, position, direction);
		enemyNet.AddWeapon(1, WeaponBuilder.CreateWeaponNet(userVariable.weapon01));
		enemyNet.AddWeapon(2, WeaponBuilder.CreateWeaponNet(userVariable.weapon02));
		enemyNet.AddWeapon(3, WeaponBuilder.CreateWeaponNet(userVariable.weapon03));
		enemyNet.UseWeapon(1);
		enemyNet.GetTransform().GetComponent<CharacterPassivePara>().ArmorEffect(userVariable.armor01, userVariable.armor02, userVariable.armor03);
		enemyNet.m_agent.speed = enemyNet.m_moveSpeed * 1.5f;
		GameManager.Instance().GetLevel().AddEnemyNet(enemyNet);
	}

	private void CreateDropItem(string pointName, string dropItemName, string netId)
	{
		DropItem dropItem = DropItemBuilder.CreateDropItemNet(dropItemName, pointName, netId);
		GameManager.Instance().GetLevel().AddDropItem(dropItem);
	}

	private void InitializeBlock(List<LevelConf.Block> blockinfo)
	{
		List<string> list = new List<string>();
		foreach (LevelConf.Block item in blockinfo)
		{
			if (item.enable != "1")
			{
				GameObject gameObject = GameObject.Find("Level/" + item.name);
				gameObject.SetActiveRecursively(false);
				GameObject gameObject2 = GameObject.Find("LevelCollider/" + item.name);
				gameObject2.SetActiveRecursively(false);
			}
			else
			{
				list.Add(item.name);
			}
		}
	}

	private bool CanSee(Vector3 position, Vector3 direction)
	{
		Ray ray = new Ray(position + new Vector3(0f, 1.8f, 0f), direction);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, direction.magnitude, 20480))
		{
			return false;
		}
		return true;
	}
}

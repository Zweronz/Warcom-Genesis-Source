using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
	private float playerDeadTime;

	public void Awake()
	{
		if (DataCenter.StateSingle().GetLevelInfo() != null)
		{
			if (DataCenter.Save().GetWeek() == -2)
			{
				string text = DataCenter.StateSingle().GetLevelInfo().mode.ToString() + string.Format("_{0:D2}", DataCenter.StateSingle().GetLevelInfo().id);
				LevelConfFight levelConfFight = new LevelConfFight("Conf/TutorialLevel01/" + text);
				DataCenter.StateSingle().SetLevelConfFight(levelConfFight);
			}
			else if (DataCenter.Save().GetWeek() == -1)
			{
				string text2 = DataCenter.StateSingle().GetLevelInfo().mode.ToString() + string.Format("_{0:D2}", DataCenter.StateSingle().GetLevelInfo().id);
				LevelConfFind levelConfFind = new LevelConfFind("Conf/TutorialLevel02/" + text2);
				DataCenter.StateSingle().SetLevelConfFind(levelConfFind);
			}
			else if (DataCenter.Save().GetWeek() == 0)
			{
				string text3 = DataCenter.StateSingle().GetLevelInfo().mode.ToString() + string.Format("_{0:D2}", DataCenter.StateSingle().GetLevelInfo().id);
				LevelConfSurvive levelConfSurvive = new LevelConfSurvive("Conf/TutorialLevel01/" + text3);
				DataCenter.StateSingle().SetLevelConfSurvive(levelConfSurvive);
			}
			else if (DataCenter.Save().GetWeek() <= 9)
			{
				string text4 = ((DataCenter.StateSingle().GetLevelInfo().type != 0) ? (string.Format("{0:D2}", DataCenter.StateSingle().sideMissionWeek) + "_" + DataCenter.StateSingle().GetLevelInfo().scene.ToString() + "_" + DataCenter.StateSingle().GetLevelInfo().mode) : (string.Format("{0:D2}", DataCenter.Save().GetWeek()) + "_" + DataCenter.StateSingle().GetLevelInfo().scene.ToString() + "_" + DataCenter.StateSingle().GetLevelInfo().mode));
				if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Fight)
				{
					LevelConfFight levelConfFight2 = new LevelConfFight("Conf/Plus/" + text4);
					DataCenter.StateSingle().SetLevelConfFight(levelConfFight2);
				}
				else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Find)
				{
					LevelConfFind levelConfFind2 = new LevelConfFind("Conf/Plus/" + text4);
					DataCenter.StateSingle().SetLevelConfFind(levelConfFind2);
				}
				else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Survive)
				{
					LevelConfSurvive levelConfSurvive2 = new LevelConfSurvive("Conf/Plus/" + text4);
					DataCenter.StateSingle().SetLevelConfSurvive(levelConfSurvive2);
				}
			}
			else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Fight)
			{
				string text5 = DataCenter.StateSingle().GetLevelInfo().mode.ToString() + string.Format("_{0:D2}", DataCenter.StateSingle().GetLevelInfo().id);
				LevelConfFight levelConfFight3 = new LevelConfFight("Conf/" + DataCenter.StateSingle().GetLevelInfo().scene.ToString() + "/" + text5);
				DataCenter.StateSingle().SetLevelConfFight(levelConfFight3);
			}
			else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Find)
			{
				string text6 = DataCenter.StateSingle().GetLevelInfo().mode.ToString() + string.Format("_{0:D2}", DataCenter.StateSingle().GetLevelInfo().id);
				LevelConfFind levelConfFind3 = new LevelConfFind("Conf/" + DataCenter.StateSingle().GetLevelInfo().scene.ToString() + "/" + text6);
				DataCenter.StateSingle().SetLevelConfFind(levelConfFind3);
			}
			else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Survive)
			{
				string text7 = DataCenter.StateSingle().GetLevelInfo().mode.ToString() + string.Format("_{0:D2}", DataCenter.StateSingle().GetLevelInfo().id);
				LevelConfSurvive levelConfSurvive3 = new LevelConfSurvive("Conf/" + DataCenter.StateSingle().GetLevelInfo().scene.ToString() + "/" + text7);
				DataCenter.StateSingle().SetLevelConfSurvive(levelConfSurvive3);
			}
		}
		else
		{
			LevelInfo levelInfo = new LevelInfo();
			levelInfo.id = 1;
			levelInfo.mode = LevelMode.Survive;
			levelInfo.scene = LevelScene.Level01;
			levelInfo.type = LevelType.Branch;
			DataCenter.StateSingle().SetLevelInfo(levelInfo);
			string text8 = DataCenter.StateSingle().GetLevelInfo().mode.ToString() + string.Format("_{0:D2}", DataCenter.StateSingle().GetLevelInfo().id);
			LevelConfSurvive levelConfSurvive4 = new LevelConfSurvive("Conf/" + DataCenter.StateSingle().GetLevelInfo().scene.ToString() + "/" + text8);
			DataCenter.StateSingle().SetLevelConfSurvive(levelConfSurvive4);
		}
	}

	public void Start()
	{
		if (DataCenter.StateSingle().GetLevelInfo() != null)
		{
			if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Fight)
			{
				RunFight();
			}
			else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Find)
			{
				RunFind();
			}
			else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Survive)
			{
				RunSurvive();
			}
		}
	}

	public void Update()
	{
		if (GameManager.Instance().GetLevel().GetPlayer()
			.m_dead)
		{
			playerDeadTime += Time.deltaTime;
			if (playerDeadTime > 5f)
			{
				BlankLoading.nextScene = "MissionEnd";
				GameManager.Instance().GetUI().SwitchScene("BlankLoading");
				playerDeadTime = 0f;
			}
		}
	}

	private void RunFight()
	{
		StartCoroutine(FightProc());
	}

	private void RunFind()
	{
		StartCoroutine(FindProc());
	}

	private void RunSurvive()
	{
		StartCoroutine(SurviveProc());
	}

	private IEnumerator FightProc()
	{
		LevelConfFight conf = DataCenter.StateSingle().GetLevelConfFight();
		bool tipsForNewWave2 = false;
		InitializeBlock(conf.GetBlockInfo());
		GameObject playerSpawnPoint = GameObject.Find(conf.GetPlayerSpawnPoint().spawnPoint);
		Player player = CreatePlayer(playerSpawnPoint.transform.position, conf.GetPlayerSpawnPoint().spawnDirection);
		yield return 0;
		int waveCount = conf.GetWaveCount();
		for (int wave = 0; wave < waveCount; wave++)
		{
			List<string> savePoints = conf.GetSavePointsClone();
			LevelConf.EnemyWave waveEnemy = conf.GetWaveEnemy(wave);
			for (int j = 0; j < waveEnemy.enemy_list.Count; j++)
			{
				Vector3 playerPosToEnemySpawnPoint = ScenePointManager.GetScenePointTransform(waveEnemy.enemy_list[j].spawnPoint).position - player.GetTransform().position;
				if (Vector3.Magnitude(playerPosToEnemySpawnPoint) > 12f && !CanSee(player.GetTransform().position, playerPosToEnemySpawnPoint))
				{
					CreateEnemy(waveEnemy.enemy_list[j], conf.GetPatrollingParam(waveEnemy.enemy_list[j].patrollingParam), conf.GetBlockInfo());
					yield return new WaitForSeconds(0.1f);
				}
				else
				{
					CreateEnemy(waveEnemy.enemy_list[j], ref savePoints, conf.GetBlockInfo());
					yield return new WaitForSeconds(0.1f);
				}
			}
			for (int i = 0; i < waveEnemy.dropItem_list.Count; i++)
			{
				CreateDropItem(waveEnemy.dropItem_list[i]);
				yield return new WaitForSeconds(0.1f);
			}
			while (GameManager.Instance().GetLevel().GetEnemyNotDeadArray()
				.Length > 0)
			{
				yield return new WaitForSeconds(1f);
			}
			yield return new WaitForSeconds(1f);
			if (!tipsForNewWave2 && wave != waveCount - 1)
			{
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Enemies incoming!");
				tipsForNewWave2 = true;
			}
			yield return new WaitForSeconds(2f);
			tipsForNewWave2 = false;
		}
		DataCenter.StateSingle().m_missionTempData.completeness = 1f;
		DataCenter.StateSingle().GetLevelInfo().pass = true;
		yield return new WaitForSeconds(3f);
		BlankLoading.nextScene = "MissionEnd";
		GameManager.Instance().GetUI().SwitchScene("BlankLoading");
		yield return 0;
	}

	private IEnumerator FindProc()
	{
		LevelConfFind conf = DataCenter.StateSingle().GetLevelConfFind();
		bool tipsForNewWave2 = false;
		InitializeBlock(conf.GetBlockInfo());
		GameObject playerSpawnPoint = GameObject.Find(conf.GetPlayerSpawnPoint().spawnPoint);
		Player player = CreatePlayer(playerSpawnPoint.transform.position, conf.GetPlayerSpawnPoint().spawnDirection);
		yield return 0;
		int waveCount = conf.GetWaveCount();
		for (int wave = 0; wave < waveCount; wave++)
		{
			LevelConf.EnemyWave waveEnemy = conf.GetWaveEnemy(wave);
			for (int j = 0; j < waveEnemy.enemy_list.Count; j++)
			{
				CreateEnemy(waveEnemy.enemy_list[j], conf.GetPatrollingParam(waveEnemy.enemy_list[j].patrollingParam), conf.GetBlockInfo());
				yield return new WaitForSeconds(0.1f);
			}
			for (int i = 0; i < waveEnemy.dropItem_list.Count; i++)
			{
				CreateDropItem(waveEnemy.dropItem_list[i]);
				yield return new WaitForSeconds(0.1f);
			}
			while (GameManager.Instance().GetLevel().GetDropItemMissionGoalCount() > 0)
			{
				yield return new WaitForSeconds(1f);
			}
			if (!tipsForNewWave2 && wave != waveCount - 1)
			{
				GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Collect all supplies.");
				tipsForNewWave2 = true;
			}
			yield return new WaitForSeconds(0.5f);
			tipsForNewWave2 = false;
		}
		while (DataCenter.StateSingle().m_missionTempData.treasureBox < conf.GetDropItemMissionGoalCount())
		{
			yield return new WaitForSeconds(3f);
		}
		DataCenter.StateSingle().m_missionTempData.completeness = 1f;
		DataCenter.StateSingle().GetLevelInfo().pass = true;
		yield return new WaitForSeconds(3f);
		BlankLoading.nextScene = "MissionEnd";
		GameManager.Instance().GetUI().SwitchScene("BlankLoading");
		yield return 0;
	}

	private IEnumerator SurviveProc()
	{
		LevelConfSurvive conf = DataCenter.StateSingle().GetLevelConfSurvive();
		bool tipsForNewWave2 = false;
		InitializeBlock(conf.GetBlockInfo());
		GameObject playerSpawnPoint = GameObject.Find(conf.GetPlayerSpawnPoint().spawnPoint);
		Player player = CreatePlayer(playerSpawnPoint.transform.position, conf.GetPlayerSpawnPoint().spawnDirection);
		yield return 0;
		int waveCount = conf.GetWaveCount();
		float startTime = Time.time + 4f;
		bool timeOut = false;
		int wave = 0;
		while (!timeOut)
		{
			LevelConf.EnemyWave waveEnemy = conf.GetWaveEnemy(wave % 4);
			List<string> savePoints = conf.GetSavePointsClone();
			for (int j = 0; j < waveEnemy.enemy_list.Count; j++)
			{
				Vector3 playerPosToEnemySpawnPoint = ScenePointManager.GetScenePointTransform(waveEnemy.enemy_list[j].spawnPoint).position - player.GetTransform().position;
				if (Vector3.Magnitude(playerPosToEnemySpawnPoint) > 12f && !CanSee(player.GetTransform().position, playerPosToEnemySpawnPoint))
				{
					CreateEnemy(waveEnemy.enemy_list[j], conf.GetPatrollingParam(waveEnemy.enemy_list[j].patrollingParam), conf.GetBlockInfo());
					yield return new WaitForSeconds(0.1f);
				}
				else
				{
					CreateEnemy(waveEnemy.enemy_list[j], ref savePoints, conf.GetBlockInfo());
					yield return new WaitForSeconds(0.1f);
				}
			}
			for (int i = 0; i < waveEnemy.dropItem_list.Count; i++)
			{
				CreateDropItem(waveEnemy.dropItem_list[i]);
				yield return new WaitForSeconds(0.1f);
			}
			while (true)
			{
				if (Time.time - startTime > conf.GetTimeLimit())
				{
					timeOut = true;
					break;
				}
				if (GameManager.Instance().GetLevel().GetEnemyCount() <= 0)
				{
					yield return new WaitForSeconds(1f);
					if (!tipsForNewWave2)
					{
						GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Enemies incoming!");
						tipsForNewWave2 = true;
					}
					yield return new WaitForSeconds(2f);
					tipsForNewWave2 = false;
					break;
				}
				yield return new WaitForSeconds(1f);
			}
			wave++;
		}
		DataCenter.StateSingle().m_missionTempData.completeness = 1f;
		DataCenter.StateSingle().GetLevelInfo().pass = true;
		yield return new WaitForSeconds(3f);
		BlankLoading.nextScene = "MissionEnd";
		GameManager.Instance().GetUI().SwitchScene("BlankLoading");
		yield return 0;
	}

	private Player CreatePlayer(Vector3 position, float direction)
	{
		int currentCharactor = DataCenter.StateSingle().GetCurrentCharactor();
		CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo(currentCharactor);
		CharacterInfo characterInfo = DataCenter.Conf().GetCharacterInfo(currentCharactor);
		Player player = CharacterBuilder.CreatePlayer(characterInfo, position, direction);
		player.AddWeapon(1, WeaponBuilder.CreateWeaponPlayer(characterEquipInfo.weapon01));
		player.AddWeapon(2, WeaponBuilder.CreateWeaponPlayer(characterEquipInfo.weapon02));
		player.AddWeapon(3, WeaponBuilder.CreateWeaponPlayer(characterEquipInfo.weapon03));
		player.UseWeapon(1);
		player.GetTransform().GetComponent<CharacterPassivePara>().ArmorEffect(characterEquipInfo);
		GameManager.Instance().GetCamera().SetCurrentCrosshairByWeaponInfo(DataCenter.Conf().GetWeaponInfoByName(characterEquipInfo.weapon01));
		GameManager.Instance().GetLevel().SetPlayer(player);
		return player;
	}

	private void CreateEnemy(LevelConf.EnemyConf enemyConf, LevelConf.PatrollingParam patrollingParam, List<LevelConf.Block> blockinfo)
	{
		EnemyInfo enemyInfoByName = DataCenter.Conf().GetEnemyInfoByName(enemyConf.name);
		Transform scenePointTransform = ScenePointManager.GetScenePointTransform(enemyConf.spawnPoint);
		Enemy enemy = new Enemy();
		enemy = ((DataCenter.StateSingle().GetLevelInfo().enemyKind != 1) ? CharacterBuilder.CreateEnemy(enemyInfoByName.model.Replace("A", "B"), scenePointTransform.position, scenePointTransform.eulerAngles.y) : CharacterBuilder.CreateEnemy(enemyInfoByName.model, scenePointTransform.position, scenePointTransform.eulerAngles.y));
		enemy.AddWeapon(1, WeaponBuilder.CreateWeaponNPC(enemyInfoByName.weapon));
		enemy.UseWeapon(1);
		EnemyAI enemyAI = new EnemyAI();
		enemyAI.Initialize(enemy.GetTransform().position, DataCenter.Conf().GetAIParam(enemyConf.aiParam));
		if (patrollingParam != null)
		{
			enemyAI.EnablePatrolling(patrollingParam.stayTime, patrollingParam.loop, ScenePointManager.GetScenePointVector3(patrollingParam.points));
		}
		enemy.SetAI(enemyAI);
		enemy.m_agent.walkableMask = 1;
		foreach (LevelConf.Block item in blockinfo)
		{
			if (item.enable != "1")
			{
				enemy.m_agent.walkableMask += (int)Mathf.Pow(2f, (float)item.id + 2f);
			}
		}
		GameManager.Instance().GetLevel().AddEnemy(enemy);
	}

	private void CreateEnemy(LevelConf.EnemyConf enemyConf, ref List<string> savePoints, List<LevelConf.Block> blockinfo)
	{
		EnemyInfo enemyInfoByName = DataCenter.Conf().GetEnemyInfoByName(enemyConf.name);
		Transform transform = ChooseSavePoint(ref savePoints);
		if (transform == null)
		{
			transform = ScenePointManager.GetScenePointTransform(enemyConf.spawnPoint);
		}
		Enemy enemy = new Enemy();
		enemy = ((DataCenter.StateSingle().GetLevelInfo().enemyKind != 1) ? CharacterBuilder.CreateEnemy(enemyInfoByName.model.Replace("A", "B"), transform.position, transform.eulerAngles.y) : CharacterBuilder.CreateEnemy(enemyInfoByName.model, transform.position, transform.eulerAngles.y));
		enemy.AddWeapon(1, WeaponBuilder.CreateWeaponNPC(enemyInfoByName.weapon));
		enemy.UseWeapon(1);
		EnemyAI enemyAI = new EnemyAI();
		enemyAI.Initialize(enemy.GetTransform().position, DataCenter.Conf().GetAIParam(8));
		enemy.SetAI(enemyAI);
		enemy.m_agent.walkableMask = 1;
		foreach (LevelConf.Block item in blockinfo)
		{
			if (item.enable != "1")
			{
				enemy.m_agent.walkableMask += (int)Mathf.Pow(2f, item.id + 2);
			}
		}
		GameManager.Instance().GetLevel().AddEnemy(enemy);
	}

	private void CreateDropItem(LevelConf.DropItemConf dropItemConf)
	{
		Transform scenePointTransform = ScenePointManager.GetScenePointTransform(dropItemConf.spawnPoints[Random.Range(0, dropItemConf.spawnPoints.Length - 1)]);
		DropItem dropItem = DropItemBuilder.CreateDropItem(dropItemConf.name, scenePointTransform.position);
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

	private Transform ChooseSavePoint(ref List<string> savePoints)
	{
		Transform result = null;
		string item = null;
		float num = 0f;
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player != null)
		{
			Vector3 position = player.GetTransform().position;
			for (int i = 0; i < savePoints.Count; i++)
			{
				Transform scenePointTransform = ScenePointManager.GetScenePointTransform(savePoints[i]);
				if (num < Vector3.Magnitude(position - scenePointTransform.position))
				{
					num = Vector3.Magnitude(position - scenePointTransform.position);
					result = scenePointTransform;
					item = savePoints[i];
				}
			}
		}
		if (savePoints.Contains(item))
		{
			savePoints.Remove(item);
		}
		return result;
	}
}

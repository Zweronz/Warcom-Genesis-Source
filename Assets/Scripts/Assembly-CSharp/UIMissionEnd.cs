using System.Collections;
using UnityEngine;

public class UIMissionEnd : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	public int m_charactorIndex = 1;

	public GameObject UIBlockObj;

	public GameObject m_charactorUIShow;

	public Player m_player;

	public GameObject m_victoryObj;

	public GameObject m_loseObj;

	public GameObject m_content;

	public GameObject m_btn;

	public TUIMeshText m_missionExpText;

	public TUIMeshText m_missionExpTitle;

	public TUIMeshText m_missionCreditsText;

	public TUIMeshText m_missionCreditsTitle;

	public TUIMeshText m_bonusExpText;

	public TUIMeshText m_bonusExpTitle;

	public TUIMeshText m_bonusCreditsText;

	public TUIMeshText m_bonusCreditsTitle;

	public GameObject m_effectDouble2;

	public GameObject m_bar;

	private int m_missionExp;

	private int m_bonusExp;

	private int m_missionMoney;

	private int m_bonusMoney;

	private int m_missionExpClone;

	private int m_bonusExpClone;

	private int m_missionMoneyClone;

	private int m_bonusMoneyClone;

	private int m_exp;

	private int m_money;

	private bool m_processShow = true;

	private bool m_countLoopSoundFx;

	private bool m_moneySoundFx;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		TAudioController component = DataCenter.StateCommon().audio.GetComponent<TAudioController>();
		if (component != null)
		{
			component.PlayAudio("music_Meu");
			DataCenter.StateCommon().bgmName = "music_Meu";
			component.StopAudio("music_" + DataCenter.StateSingle().GetLevelInfo().scene);
		}
		DataCenter.StateCommon().enterInMissionEndTimes++;
		if (DataCenter.StateCommon().enterInMissionEndTimes % 5 == 0)
		{
			OpenClikPlugin.Show(true);
		}
		if (DataCenter.StateSingle().GetLevelInfo().pass)
		{
			if (DataCenter.StateSingle().GetLevelInfo().type == LevelType.Main)
			{
				DataCenter.Save().GetLastBoneLevel().pass = true;
			}
			m_victoryObj.SetActiveRecursively(true);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Score_VictoryRoar");
		}
		else
		{
			m_loseObj.SetActiveRecursively(true);
		}
		for (int i = 0; i < DataCenter.StateSingle().m_missionTempData.medalList.Count; i++)
		{
			int index = DataCenter.StateSingle().m_missionTempData.medalList[i];
			string conditionName = DataCenter.Conf().GetMedalInfoByIndex(index).conditionName;
			DataCenter.Save().UnlockMedal(index, conditionName);
		}
		DataCenter.Save().SetExp(DataCenter.Save().GetExp() + DataCenter.StateSingle().m_missionTempData.expMedal);
		DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + DataCenter.StateSingle().m_missionTempData.moneyMedal - DataCenter.StateSingle().m_missionTempData.buyAmmoMoney);
		m_exp = DataCenter.Save().GetExp();
		ExpForShow(0);
		m_money = DataCenter.Save().GetMoney();
		int week = Mathf.Clamp(DataCenter.StateSingle().GetLevelInfo().weekFluctuation + DataCenter.Save().GetWeek(), 1, 52);
		DataConf.LevelWeekExpMoney levelWeekExpMoneyByWeek = DataCenter.Conf().GetLevelWeekExpMoneyByWeek(week);
		m_missionExp = (int)((float)levelWeekExpMoneyByWeek.levelExp * DataCenter.StateSingle().m_missionTempData.completeness);
		m_bonusExp = (int)((float)levelWeekExpMoneyByWeek.expBagExp * (float)DataCenter.StateSingle().m_missionTempData.expBag * DataCenter.StateSingle().m_missionTempData.completeness);
		m_missionMoney = (int)((float)levelWeekExpMoneyByWeek.levelMoney * DataCenter.StateSingle().m_missionTempData.completeness);
		m_bonusMoney = (int)((float)levelWeekExpMoneyByWeek.moneyBagMoney * (float)DataCenter.StateSingle().m_missionTempData.moneyBag * DataCenter.StateSingle().m_missionTempData.completeness);
		m_missionExpClone = m_missionExp / 2;
		m_bonusExpClone = m_bonusExp / 2;
		m_missionMoneyClone = m_missionMoney / 2;
		m_bonusMoneyClone = m_bonusMoney / 2;
		m_charactorIndex = DataCenter.StateSingle().GetCurrentCharactor();
		m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
		if (Mathf.Min(Screen.width, Screen.height) > 641)
		{
			m_charactorUIShow.transform.Find("effect_Platform").localPosition = new Vector3(-1.8961f, -1.3f, 12.93605f);
			m_charactorUIShow.transform.Find("Platform").localPosition = new Vector3(-1.6461f, -0.8f, 13.13885f);
		}
		StartCoroutine(UpdateCharacterInfo());
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
		if (m_player != null)
		{
			m_player.Update(Time.deltaTime);
		}
	}

	public void OnDestroy()
	{
		DataCenter.StateSingle().SetLevelConfSurvive(null);
		DataCenter.StateSingle().SetLevelConfFind(null);
		DataCenter.StateSingle().SetLevelConfFight(null);
		DataCenter.StateSingle().m_missionTempData.Reset();
		DataCenter.StateSingle().SetLevelInfo(null);
		OpenClikPlugin.Hide();
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "CloseProcess" && eventType == 3)
		{
			m_processShow = false;
		}
		else if (control.name == "CharactorUIShowMove")
		{
			switch (eventType)
			{
			case 2:
				if (m_player != null)
				{
					m_player.TurnAround((0f - wparam) * 20f, lparam * 20f);
				}
				break;
			}
		}
		else if (control.name == "Map" && eventType == 3)
		{
			SwitchScene("WorldMap");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Back" && eventType == 3)
		{
			SwitchScene("WorldMap");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Back");
		}
		else if (control.name == "TMarket" && eventType == 3)
		{
			SwitchScene("IAP");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Function" && eventType == 1)
		{
			ShowAttribute();
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Function" && eventType == 2)
		{
			HideAttribute();
			UIBlockObj.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Base" && eventType == 3)
		{
			SwitchScene("Base");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Map" && eventType == 3)
		{
			SwitchScene("WorldMap");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Shop" && eventType == 3)
		{
			SwitchScene("Shop");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Achievements" && eventType == 3)
		{
			GameCenterPlugin.OpenAchievement();
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Rank" && eventType == 3)
		{
			GameCenterPlugin.OpenLeaderboard();
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Options" && eventType == 3)
		{
			SwitchScene("Options");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
	}

	private void SwitchScene(string scene)
	{
		m_processShow = false;
		UtilUIUpdateAttribute.SetLastScene("MissionEnd");
		if (scene == "Shop" || scene.Contains("MissionConfirm") || scene == "SuitUp")
		{
			UtilUIUpdateAttribute.SetNextScene(scene);
			m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
				.FadeOut("Loading");
		}
		else
		{
			UtilUIUpdateAttribute.SetNextScene(scene);
			BlankLoading.nextScene = scene;
			m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
				.FadeOut("BlankLoading");
		}
	}

	private void ShowAttribute()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Attribute").Find("StatusBar")
			.gameObject;
		gameObject.GetComponent<Animation>().Play("StatusBarAnim");
		GameObject gameObject2 = m_tui.transform.Find("TUIControl").Find("Attribute").Find("MenuBar")
			.gameObject;
		gameObject2.GetComponent<Animation>().Play("MenuBarAnim");
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButtonTop_Slipin");
	}

	private void HideAttribute()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Attribute").Find("StatusBar")
			.gameObject;
		gameObject.GetComponent<Animation>().Play("StatusBarBackAnim");
		GameObject gameObject2 = m_tui.transform.Find("TUIControl").Find("Attribute").Find("MenuBar")
			.gameObject;
		gameObject2.GetComponent<Animation>().Play("MenuBarBackAnim");
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButtonTop_Slipout");
	}

	private void CreatePlayerModel(int charactorIndex)
	{
		CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo(charactorIndex);
		CharacterInfo characterInfo = DataCenter.Conf().GetCharacterInfo(charactorIndex);
		Vector3 vector = new Vector3(-1.836f, -0.8f, 11f);
		if (Mathf.Min(Screen.width, Screen.height) > 641)
		{
			vector = new Vector3(-1.86171f, -0.8f, 12.93668f);
		}
		m_player = CharacterBuilder.CreatePlayer(characterInfo, m_charactorUIShow.transform.position + vector, 190f);
		m_player.GetTransform().parent = m_charactorUIShow.transform;
		m_player.AddWeapon(1, WeaponBuilder.CreateWeaponPlayer(characterEquipInfo.weapon01));
		m_player.UseWeapon(1);
		if (DataCenter.StateSingle().GetLevelInfo().pass)
		{
			m_player.SetWin(true);
		}
		else
		{
			m_player.SetLose(true);
		}
	}

	private void DeletePlayerModel()
	{
		if (m_player.GetGameObject() != null)
		{
			Object.Destroy(m_player.GetGameObject());
		}
	}

	public IEnumerator UpdateScore()
	{
		DataSave();
		m_missionExpText.text = m_missionExpClone.ToString();
		m_missionExpText.UpdateMesh();
		m_missionCreditsText.text = m_missionMoneyClone.ToString();
		m_missionCreditsText.UpdateMesh();
		m_bonusExpText.text = m_bonusExpClone.ToString();
		m_bonusExpText.UpdateMesh();
		m_bonusCreditsText.text = m_bonusMoneyClone.ToString();
		m_bonusCreditsText.UpdateMesh();
		m_content.GetComponent<Animation>().Play("MissionEndContent");
		yield return new WaitForSeconds(2f);
		if (m_victoryObj.active && !m_effectDouble2.active)
		{
			m_effectDouble2.SetActiveRecursively(true);
			m_effectDouble2.GetComponent<ParticleSystem>().Play();
			m_content.GetComponent<Animation>().Play("MissionEndContentNum");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Score_X2");
			m_missionExpClone *= 2;
			m_missionMoneyClone *= 2;
			m_bonusExpClone *= 2;
			m_bonusMoneyClone *= 2;
			m_missionExpText.text = m_missionExpClone.ToString();
			m_missionCreditsText.text = m_missionMoneyClone.ToString();
			m_bonusExpText.text = m_bonusExpClone.ToString();
			m_bonusCreditsText.text = m_bonusMoneyClone.ToString();
			yield return new WaitForSeconds(1f);
		}
		while (m_processShow)
		{
			if (m_missionExpClone > 0)
			{
				m_missionExpClone -= 10;
				m_missionExpText.text = m_missionExpClone.ToString();
				m_missionExpText.UpdateMesh();
				ExpForShow(10);
				if (!m_countLoopSoundFx)
				{
					DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Score_CountLoop");
					m_countLoopSoundFx = true;
				}
				yield return new WaitForSeconds(Time.deltaTime);
				continue;
			}
			ExpForShow(m_missionExpClone);
			m_missionExpClone = 0;
			m_missionExpText.text = m_missionExpClone.ToString();
			m_missionExpText.UpdateMesh();
			if (m_bonusExpClone > 0)
			{
				m_bonusExpClone -= 10;
				m_bonusExpText.text = m_bonusExpClone.ToString();
				m_bonusExpText.UpdateMesh();
				ExpForShow(10);
				yield return new WaitForSeconds(Time.deltaTime);
				continue;
			}
			ExpForShow(m_bonusExpClone);
			m_bonusExpClone = 0;
			m_bonusExpText.text = m_bonusExpClone.ToString();
			m_bonusExpText.UpdateMesh();
			if (m_missionMoneyClone > 0)
			{
				m_moneySoundFx = true;
				m_missionMoneyClone -= 10;
				m_missionCreditsText.text = m_missionMoneyClone.ToString();
				m_missionCreditsText.UpdateMesh();
				MoneyForShow(10);
				yield return new WaitForSeconds(Time.deltaTime);
				continue;
			}
			MoneyForShow(m_missionMoneyClone);
			m_missionMoneyClone = 0;
			m_missionCreditsText.text = m_missionMoneyClone.ToString();
			m_missionCreditsText.UpdateMesh();
			if (m_bonusMoneyClone > 0)
			{
				m_moneySoundFx = true;
				m_bonusMoneyClone -= 10;
				m_bonusCreditsText.text = m_bonusMoneyClone.ToString();
				m_bonusCreditsText.UpdateMesh();
				MoneyForShow(10);
				yield return new WaitForSeconds(Time.deltaTime);
				continue;
			}
			MoneyForShow(m_bonusMoneyClone);
			m_bonusMoneyClone = 0;
			m_bonusCreditsText.text = m_bonusMoneyClone.ToString();
			m_bonusCreditsText.UpdateMesh();
			break;
		}
		if (m_countLoopSoundFx)
		{
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().StopAudio("UI_Score_CountLoop");
			m_countLoopSoundFx = false;
		}
		if (m_moneySoundFx)
		{
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Score_Money");
			m_moneySoundFx = false;
		}
		m_bar.GetComponent<UtilUIUpdateAttribute>().refreshDate = true;
		m_missionExpText.text = "0";
		m_missionExpText.UpdateMesh();
		m_bonusExpText.text = "0";
		m_bonusExpText.UpdateMesh();
		m_missionCreditsText.text = "0";
		m_missionCreditsText.UpdateMesh();
		m_bonusCreditsText.text = "0";
		m_bonusCreditsText.UpdateMesh();
		m_btn.transform.localPosition = Vector3.zero;
		yield return new WaitForSeconds(2f);
	}

	public IEnumerator UpdateCharacterInfo()
	{
		TUIMeshText name = m_tui.transform.Find("TUIControl").Find("Name").Find("NameText")
			.GetComponent<TUIMeshText>();
		name.text = DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).name;
		name.UpdateMesh();
		ShowCharactorInfo();
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(UpdateScore());
		yield return 0;
	}

	private void ShowCharactorInfo()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Name").gameObject;
		gameObject.GetComponent<Animation>().Play("NameInAnim");
		GameObject gameObject2 = m_tui.transform.Find("TUIControl").Find("AchievementWindow").gameObject;
		gameObject2.GetComponent<Animation>().Play("EquipmentInAnim");
		GameObject gameObject3 = m_charactorUIShow.transform.Find("Camera").gameObject;
		gameObject3.GetComponent<Animation>().Play("CharactorUIShowCameraAnim");
		CreatePlayerModel(m_charactorIndex);
		GameObject gameObject4 = m_tui.transform.Find("TUIControl").Find("CharacterDetails").gameObject;
		gameObject4.GetComponent<Animation>().Play("CharacterDetailsIn");
		gameObject4.GetComponent<UtilMissionConfirmCharacterDetails>().RefreshCharacterDetails(m_player);
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_RightSide_Slipin");
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftCharactor_Slipin");
	}

	private void DataSave()
	{
		if (!DataCenter.StateSingle().GetLevelInfo().pass)
		{
			m_missionExp /= 2;
			m_bonusExp /= 2;
			m_missionMoney /= 2;
			m_bonusMoney /= 2;
		}
		DataCenter.Save().SetExp(m_missionExp + m_bonusExp + DataCenter.Save().GetExp());
		DataCenter.Save().SetMoney(m_missionMoney + m_bonusMoney + DataCenter.Save().GetMoney());
		int num = 0;
		for (int i = 0; i < DataCenter.Conf().GetLevelUpExpMap().Count; i++)
		{
			if (DataCenter.Save().GetExp() >= DataCenter.Conf().GetLevelUpExpMap()[i + 1].totalExp)
			{
				num++;
			}
		}
		DataCenter.Save().SetLv(num);
		CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateSingle().GetCurrentCharactor());
		characterEquipInfo.totalTime += (int)DataCenter.StateSingle().m_missionTempData.time;
		characterEquipInfo.dead += DataCenter.StateSingle().m_missionTempData.dead;
		characterEquipInfo.enemyKill += DataCenter.StateSingle().m_missionTempData.emenyKill;
		characterEquipInfo.enemyKillHeadShot += DataCenter.StateSingle().m_missionTempData.emenyKillHeadShot;
		characterEquipInfo.deadHeadShot += DataCenter.StateSingle().m_missionTempData.deadHeadShot;
		characterEquipInfo.itemUseCount += DataCenter.StateSingle().m_missionTempData.itemUseCount;
		characterEquipInfo.fireBullet += DataCenter.StateSingle().m_missionTempData.fireBullet;
		characterEquipInfo.hitBullet += DataCenter.StateSingle().m_missionTempData.hitBullet;
		characterEquipInfo.weaponKnifeKill += DataCenter.StateSingle().m_missionTempData.weaponKnifeKill;
		characterEquipInfo.weaponHandgunKill += DataCenter.StateSingle().m_missionTempData.weaponHandgunKill;
		characterEquipInfo.weaponAssaultRifleKill += DataCenter.StateSingle().m_missionTempData.weaponAssaultRifleKill;
		characterEquipInfo.weaponRPGKill += DataCenter.StateSingle().m_missionTempData.weaponRPGKill;
		characterEquipInfo.weaponShotgunKill += DataCenter.StateSingle().m_missionTempData.weaponShotgunKill;
		characterEquipInfo.weaponSubmachineGunKill += DataCenter.StateSingle().m_missionTempData.weaponSubmachineGunKill;
		characterEquipInfo.weaponSniperKill += DataCenter.StateSingle().m_missionTempData.weaponSniperKill;
		characterEquipInfo.killCommandoCount += DataCenter.StateSingle().m_missionTempData.killCommandoCount;
		characterEquipInfo.killRaiderCount += DataCenter.StateSingle().m_missionTempData.killRaiderCount;
		characterEquipInfo.killScoutCount += DataCenter.StateSingle().m_missionTempData.killScoutCount;
		characterEquipInfo.killSniperCount += DataCenter.StateSingle().m_missionTempData.killSniperCount;
		characterEquipInfo.leechKill += DataCenter.StateSingle().m_missionTempData.leechKill;
		characterEquipInfo.playTimes++;
		if (m_victoryObj.active)
		{
			if (DataCenter.StateSingle().GetLevelInfo().type == LevelType.Main)
			{
				characterEquipInfo.mainMissionComplete++;
				DataCenter.Save().SetWeek(Mathf.Min(52, DataCenter.Save().GetWeek() + 1));
			}
			else
			{
				characterEquipInfo.auxiliaryMissionComplete++;
			}
		}
		DataCenter.Save().Save();
		GameCenterPlugin.SubmitScore("com.trinitigame.warcorpsgenesis.l1", DataCenter.Save().GetEnemyKill());
		GameCenterPlugin.SubmitScore("com.trinitigame.warcorpsgenesis.l2", DataCenter.Save().GetEnemyHeadShot());
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a1", DataCenter.Save().GetEnemyKill() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a2", DataCenter.Save().GetDead() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a3", DataCenter.Save().GetEnemyHeadShot() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a4", (DataCenter.Save().GetUnlockWeaponCount() - 10) / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a5", DataCenter.Save().GetUnlockArmorCount() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a6", DataCenter.Save().GetItemUseCount() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a7", DataCenter.Save().GetAuxiliaryMissionComplete() / 40 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a8", DataCenter.Save().GetWeaponHandgunKill() / 500 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a9", DataCenter.Save().GetWeaponAssaultRifleKill() / 800 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a10", DataCenter.Save().GetWeaponShotgunGunKill() / 1000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a11", DataCenter.Save().GetWeaponSubmachineGunKill() / 1000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a12", DataCenter.Save().GetWeaponSniperKill() / 1000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a13", DataCenter.Save().GetWeaponRPGKill() / 1000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a14", DataCenter.Save().GetWeaponKnifeKill() / 250 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a15", DataCenter.Save().GetKillScoutCount() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a16", DataCenter.Save().GetKillRaiderCount() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a17", DataCenter.Save().GetKillCommandoCount() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a18", DataCenter.Save().GetKillSniperCount() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a19", DataCenter.Save().GetTotalTime() / 18000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a20", DataCenter.Save().GetPlayTimes() / 100 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a21", (DataCenter.Save().GetUnlockWeaponCount() - 10) / 20 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a22", DataCenter.Save().GetUnlockArmorCount() / 15 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a23", DataCenter.Save().GetUnlockItemCount() / 6 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a24", DataCenter.Save().GetItemUseCount() / 150 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a25", DataCenter.Save().GetEnemyKill() / 1500 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a26", DataCenter.Save().GetEnemyHeadShot() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a27", DataCenter.Save().GetDead() / 100 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a28", DataCenter.Save().GetFireBullet() / 15000 * 100);
	}

	private void ExpForShow(int add)
	{
		m_exp += add;
		int num = 0;
		for (int i = 0; i < DataCenter.Conf().GetLevelUpExpMap().Count; i++)
		{
			if (m_exp >= DataCenter.Conf().GetLevelUpExpMap()[i + 1].totalExp)
			{
				num++;
			}
		}
		m_bar.GetComponent<UtilUIUpdateAttribute>().UpdateLvBar(num, m_exp);
		if (add != 0 && m_bar.GetComponent<UtilUIUpdateAttribute>().UpdataLV(num))
		{
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Score_LeavlUp");
		}
	}

	private void MoneyForShow(int add)
	{
		m_money += add;
		m_bar.GetComponent<UtilUIUpdateAttribute>().UpdateMoney(m_money);
	}
}

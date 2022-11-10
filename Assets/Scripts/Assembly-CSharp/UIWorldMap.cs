using UnityEngine;

public class UIWorldMap : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	private LevelInfo[] m_levels;

	public GameObject UIBlockObj;

	public GameObject UIBlockObj2;

	public GameObject m_scrollObj;

	public TUIButtonPush UIFunctionBtn;

	public void Start()
	{
		TAudioController component = DataCenter.StateCommon().audio.GetComponent<TAudioController>();
		if (component != null && DataCenter.StateCommon().bgmName != "music_Meu")
		{
			component.PlayAudio("music_Meu");
			DataCenter.StateCommon().bgmName = "music_Meu";
		}
		m_levels = GameLevelCreator.CreateLevel();
		for (int i = 0; i < m_levels.Length; i++)
		{
			if (m_levels[i] != null)
			{
				Debug.Log(string.Concat("Mode:", m_levels[i].mode, ", Scene:", m_levels[i].scene, ", Type:", m_levels[i].type, ", Pass:", m_levels[i].pass, ", Id:", m_levels[i].id, ", EnemyKind:", m_levels[i].enemyKind, ", TextId:", m_levels[i].textId, ", WeekFluctuation", m_levels[i].weekFluctuation));
			}
		}
		DataCenter.StateSingle().SetLevelConfSurvive(null);
		DataCenter.StateSingle().SetLevelConfFind(null);
		DataCenter.StateSingle().SetLevelConfFight(null);
		DataCenter.StateSingle().m_missionTempData.Reset();
		DataCenter.StateSingle().SetLevelInfo(null);
		m_tui = TUI.Instance("TUI");
		m_tui.transform.GetComponentInChildren<UtilUIUpdateWorldMapLevel>().UpdateLevel(m_levels);
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		if (DataCenter.Save().GetMainMissionComplete() == 3 && !DataCenter.Save().GetWorldMapToturialShow())
		{
			DataCenter.Save().SetWorldMapToturialShow(true);
			DataCenter.Save().Save();
			ShowToturialPrompt();
			UIBlockObj.SetActiveRecursively(true);
		}
		m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Level01Button" && eventType == 1)
		{
			DataCenter.StateSingle().SetLevelInfo(m_levels[0]);
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			UIBlockObj.SetActiveRecursively(true);
			ShowPrompt(m_levels[0]);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Level02Button" && eventType == 1)
		{
			DataCenter.StateSingle().SetLevelInfo(m_levels[1]);
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			UIBlockObj.SetActiveRecursively(true);
			ShowPrompt(m_levels[1]);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Level03Button" && eventType == 1)
		{
			DataCenter.StateSingle().SetLevelInfo(m_levels[2]);
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			UIBlockObj.SetActiveRecursively(true);
			ShowPrompt(m_levels[2]);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Level04Button" && eventType == 1)
		{
			DataCenter.StateSingle().SetLevelInfo(m_levels[3]);
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			UIBlockObj.SetActiveRecursively(true);
			ShowPrompt(m_levels[3]);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Shop2" && eventType == 1)
		{
			SwitchScene("Shop");
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Base2" && eventType == 1)
		{
			SwitchScene("Base");
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "PromptPlay" && eventType == 3)
		{
			if (DataCenter.Save().GetWeek() == -2 || DataCenter.Save().GetWeek() == 0)
			{
				SwitchScene("TutorialLevel01");
			}
			else
			{
				SwitchScene("TutorialLevel02");
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Go");
		}
		else if (control.name == "PromptSuitup" && eventType == 3)
		{
			SwitchScene("MissionConfirm");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "PromptClose" && eventType == 3)
		{
			HidePrompt();
			m_scrollObj.SendMessage("ResetAllSelectBtnFalse");
			UIBlockObj.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "ToturialClose" && eventType == 3)
		{
			HideToturialPrompt();
			UIBlockObj.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Back" && eventType == 3)
		{
			SwitchScene("Entry");
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
			UIBlockObj2.SetActiveRecursively(true);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Function" && eventType == 2)
		{
			HideAttribute();
			UIBlockObj2.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Base" && eventType == 3)
		{
			SwitchScene("Base");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Map" && eventType == 3)
		{
			HideAttribute();
			UIBlockObj2.SetActiveRecursively(false);
			UIFunctionBtn.SetPressed(false);
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
		UtilUIUpdateAttribute.SetLastScene("WorldMap");
		if (scene == "Shop" || scene.Contains("MissionConfirm") || scene == "SuitUp" || scene.Contains("Level"))
		{
			UtilUIUpdateAttribute.SetNextScene(scene);
			m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
				.FadeOut("Loading");
		}
		else
		{
			m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
				.FadeOut(scene);
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

	private void ShowPrompt(LevelInfo levelInfo)
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Prompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 0f, gameObject.transform.localPosition.z);
		if (levelInfo.scene == LevelScene.Level01)
		{
			gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = "WorldMapLevel01";
		}
		else if (levelInfo.scene == LevelScene.Level02)
		{
			gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = "WorldMapLevel02";
		}
		else if (levelInfo.scene == LevelScene.Level03)
		{
			gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = "WorldMapLevel03";
		}
		else if (levelInfo.scene == LevelScene.Level04)
		{
			gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = "WorldMapLevel04";
		}
		gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().UpdateMesh();
		if (DataCenter.Save().GetWeek() < 1)
		{
			gameObject.transform.Find("Week").GetComponent<TUIMeshText>().text = string.Empty;
			gameObject.transform.Find("Week").GetComponent<TUIMeshText>().color = new Color(0.941f, 0.039f, 0.039f, 1f);
			gameObject.transform.Find("Mission").GetComponent<TUIMeshText>().text = "Tutorial";
			gameObject.transform.Find("Mission").GetComponent<TUIMeshText>().color = new Color(0.941f, 0.039f, 0.039f, 1f);
			gameObject.transform.Find("EnemyLv").GetComponent<TUIMeshText>().text = string.Empty;
			gameObject.transform.Find("EnemyLv").GetComponent<TUIMeshText>().color = new Color(0.941f, 0.039f, 0.039f, 1f);
			gameObject.transform.Find("PromptPlay").gameObject.SetActiveRecursively(true);
			gameObject.transform.Find("PromptPlay").gameObject.GetComponent<TUIButtonClick>().Reset();
			gameObject.transform.Find("PromptSuitup").gameObject.SetActiveRecursively(false);
		}
		else
		{
			gameObject.transform.Find("PromptPlay").gameObject.SetActiveRecursively(false);
			gameObject.transform.Find("PromptSuitup").gameObject.SetActiveRecursively(true);
			gameObject.transform.Find("PromptSuitup").gameObject.GetComponent<TUIButtonClick>().Reset();
			if (levelInfo.type == LevelType.Main)
			{
				gameObject.transform.Find("Week").GetComponent<TUIMeshText>().text = "Week: " + string.Format("{0:D2}", DataCenter.Save().GetWeek());
				gameObject.transform.Find("Week").GetComponent<TUIMeshText>().color = new Color(0.941f, 0.039f, 0.039f, 1f);
				gameObject.transform.Find("Mission").GetComponent<TUIMeshText>().text = "Mission: " + string.Format("{0:D2}", DataCenter.Save().GetWeek());
				gameObject.transform.Find("Mission").GetComponent<TUIMeshText>().color = new Color(0.941f, 0.039f, 0.039f, 1f);
				gameObject.transform.Find("EnemyLv").GetComponent<TUIMeshText>().text = "Lv: " + string.Format("{0:D2}", DataCenter.Save().GetWeek());
				gameObject.transform.Find("EnemyLv").GetComponent<TUIMeshText>().color = new Color(0.941f, 0.039f, 0.039f, 1f);
			}
			else
			{
				gameObject.transform.Find("Week").GetComponent<TUIMeshText>().color = new Color(0.043f, 0.557f, 0.894f, 1f);
				gameObject.transform.Find("Week").GetComponent<TUIMeshText>().text = "Week: " + string.Format("{0:D2}", DataCenter.Save().GetWeek());
				gameObject.transform.Find("Mission").GetComponent<TUIMeshText>().color = new Color(0.043f, 0.557f, 0.894f, 1f);
				gameObject.transform.Find("Mission").GetComponent<TUIMeshText>().text = "Side Mission";
				gameObject.transform.Find("EnemyLv").GetComponent<TUIMeshText>().color = new Color(0.043f, 0.557f, 0.894f, 1f);
				gameObject.transform.Find("EnemyLv").GetComponent<TUIMeshText>().text = "Lv: " + string.Format("{0:D2}", Mathf.Max(DataCenter.Save().GetWeek() + levelInfo.weekFluctuation, 1));
			}
		}
		gameObject.transform.Find("Week").GetComponent<TUIMeshText>().UpdateMesh();
		gameObject.transform.Find("Mission").GetComponent<TUIMeshText>().UpdateMesh();
		gameObject.transform.Find("EnemyLv").GetComponent<TUIMeshText>().UpdateMesh();
		if (levelInfo.mode == LevelMode.Fight)
		{
			if (levelInfo.type == LevelType.Main)
			{
				gameObject.transform.Find("Mode").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFightMain";
				gameObject.transform.Find("Frame").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFrame02";
			}
			else
			{
				gameObject.transform.Find("Mode").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFightBranch";
				gameObject.transform.Find("Frame").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFrame01";
			}
		}
		else if (levelInfo.mode == LevelMode.Find)
		{
			if (levelInfo.type == LevelType.Main)
			{
				gameObject.transform.Find("Mode").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFindMain";
				gameObject.transform.Find("Frame").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFrame02";
			}
			else
			{
				gameObject.transform.Find("Mode").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFindBranch";
				gameObject.transform.Find("Frame").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFrame01";
			}
		}
		else if (levelInfo.mode == LevelMode.Survive)
		{
			if (levelInfo.type == LevelType.Main)
			{
				gameObject.transform.Find("Mode").GetComponent<TUIMeshSprite>().frameName = "MissionPromptSurvivalMain";
				gameObject.transform.Find("Frame").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFrame02";
			}
			else
			{
				gameObject.transform.Find("Mode").GetComponent<TUIMeshSprite>().frameName = "MissionPromptSurvivalBranch";
				gameObject.transform.Find("Frame").GetComponent<TUIMeshSprite>().frameName = "MissionPromptFrame01";
			}
		}
		gameObject.transform.Find("Mode").GetComponent<TUIMeshSprite>().UpdateMesh();
		gameObject.transform.Find("Frame").GetComponent<TUIMeshSprite>().UpdateMesh();
		string[] levelMissionDesc = DataCenter.Conf().GetLevelMissionDesc((int)levelInfo.scene, (int)levelInfo.mode);
		if (DataCenter.Save().GetWeek() == -2)
		{
			gameObject.transform.Find("Details").GetComponent<TUIMeshText>().text = "Eliminate all enemies to win in \nElimination!";
		}
		else if (DataCenter.Save().GetWeek() == -1)
		{
			gameObject.transform.Find("Details").GetComponent<TUIMeshText>().text = "Collect all supplies to win in Grab and\nGo!";
		}
		else if (DataCenter.Save().GetWeek() == 0)
		{
			gameObject.transform.Find("Details").GetComponent<TUIMeshText>().text = "Stay alive to win in Survival!";
		}
		else if (levelInfo.enemyKind == 1)
		{
			gameObject.transform.Find("Details").GetComponent<TUIMeshText>().text = levelMissionDesc[levelInfo.textId - 1].Replace("###", "RAD");
		}
		else
		{
			gameObject.transform.Find("Details").GetComponent<TUIMeshText>().text = levelMissionDesc[levelInfo.textId - 1].Replace("###", "Fatwa");
		}
		gameObject.transform.Find("Details").GetComponent<TUIMeshText>().UpdateMesh();
	}

	private void HidePrompt()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Prompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 10000f, gameObject.transform.localPosition.z);
	}

	private void ShowToturialPrompt()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("ToturialPrompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 0f, gameObject.transform.localPosition.z);
		GameObject gameObject2 = gameObject.transform.Find("ToturialClose").gameObject;
		gameObject2.GetComponent<Animation>()["BtnFlash"].wrapMode = WrapMode.Loop;
	}

	private void HideToturialPrompt()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("ToturialPrompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 10000f, gameObject.transform.localPosition.z);
	}

	private void OnGUI()
	{
		if (DataConstant.bDebug && GUILayout.Button("Rich Man"))
		{
			DataCenter.Save().SetMoney(10000000);
			DataCenter.Save().SetCrystal(110000000);
		}
	}
}

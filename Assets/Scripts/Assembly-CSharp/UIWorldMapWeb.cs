using UnityEngine;

public class UIWorldMapWeb : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	private LevelInfo[] m_levels;

	public GameObject UIBlockObj;

	public GameObject UIBlockObj2;

	public GameObject m_scrollObj;

	public TUIButtonPush UIFunctionBtn;

	private TouchScreenKeyboard m_iphoneKeyboard;

	public void Start()
	{
		TAudioController component = DataCenter.StateCommon().audio.GetComponent<TAudioController>();
		if (component != null && DataCenter.StateCommon().bgmName != "music_Meu")
		{
			component.PlayAudio("music_Meu");
			DataCenter.StateCommon().bgmName = "music_Meu";
		}
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
		string @string = PlayerPrefs.GetString("UserName");
		if (@string.Length > 0)
		{
			DataCenter.StateMulti().SetUserName(@string);
		}
		else if (m_iphoneKeyboard == null)
		{
			m_iphoneKeyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default, true, false, false, true, "Please input your name...");
		}
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
		if (m_iphoneKeyboard != null && m_iphoneKeyboard.done)
		{
			DataCenter.StateMulti().SetUserName(m_iphoneKeyboard.text);
			m_iphoneKeyboard = null;
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Level01Button" && eventType == 1)
		{
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateMulti().SetCurrentScene(LevelScene.Level01);
			ShowPrompt(LevelScene.Level01);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Level02Button" && eventType == 1)
		{
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateMulti().SetCurrentScene(LevelScene.Level02);
			ShowPrompt(LevelScene.Level02);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Level03Button" && eventType == 1)
		{
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateMulti().SetCurrentScene(LevelScene.Level03);
			ShowPrompt(LevelScene.Level03);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Level04Button" && eventType == 1)
		{
			control.gameObject.transform.parent.parent.SendMessage("SelectOneBtnStatus", control.gameObject.transform.parent.gameObject);
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateMulti().SetCurrentScene(LevelScene.Level04);
			ShowPrompt(LevelScene.Level04);
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
		else if (control.name == "PromptSuitup" && eventType == 3)
		{
			SwitchScene("MissionConfirmWeb");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "PromptClose" && eventType == 3)
		{
			HidePrompt();
			m_scrollObj.SendMessage("ResetAllSelectBtnFalse");
			UIBlockObj.SetActiveRecursively(false);
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
		UtilUIUpdateAttribute.SetLastScene("WorldMapWeb");
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

	private void ShowPrompt(LevelScene scene)
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Prompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 0f, gameObject.transform.localPosition.z);
		switch (scene)
		{
		case LevelScene.Level01:
			gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = "WorldMapLevel01";
			break;
		case LevelScene.Level02:
			gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = "WorldMapLevel02";
			break;
		case LevelScene.Level03:
			gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = "WorldMapLevel03";
			break;
		case LevelScene.Level04:
			gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = "WorldMapLevel04";
			break;
		}
		gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>().UpdateMesh();
	}

	private void HidePrompt()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Prompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 10000f, gameObject.transform.localPosition.z);
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(250f, 100f, 60f, 20f), DataCenter.StateMulti().GetUserName());
		if (GUI.Button(new Rect(500f, 100f, 100f, 100f), "192.168.0.190"))
		{
			DataCenter.StateMulti().SetIP("192.168.0.190");
		}
		if (GUI.Button(new Rect(500f, 250f, 100f, 100f), "203.124.98.26"))
		{
			DataCenter.StateMulti().SetIP("203.124.98.26");
		}
		if (GUI.Button(new Rect(500f, 450f, 100f, 100f), "LVMONEY"))
		{
			DataCenter.Save().SetLv(50);
			DataCenter.Save().SetMoney(1000000000);
			DataCenter.Save().SetCrystal(300000);
			DataCenter.Save().Save();
		}
	}
}

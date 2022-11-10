using UnityEngine;

public class UIOptions : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	public GameObject UIBlockObj;

	public TUIButtonPush UIFunctionBtn;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Panel").Find("BarMusic")
			.GetComponent<UtilUISlider>()
			.SetSliderRate((float)DataCenter.Save().GetOptionMusic() / 100f);
		m_tui.transform.Find("TUIControl").Find("Panel").Find("BarSound")
			.GetComponent<UtilUISlider>()
			.SetSliderRate((float)DataCenter.Save().GetOptionSound() / 100f);
		m_tui.transform.Find("TUIControl").Find("Panel").Find("BarJoystick")
			.GetComponent<UtilUISlider>()
			.SetSliderRate((float)DataCenter.Save().GetOptionJoystick() / 100f);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
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
		if (control.name == "Credits" && eventType == 3)
		{
			SwitchScene("Credit");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "HowTo" && eventType == 3)
		{
			SwitchScene("HowTo");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Review" && eventType == 3)
		{
			Application.OpenURL("market://details?id=com.trinitigame.android.warcorpsgenesis");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Support" && eventType == 3)
		{
			Application.OpenURL("http://www.trinitigame.com/support?game=wcg&version=1.1.3");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if ((control.name == "ButtonMusic" || control.name == "ButtonJoystick" || control.name == "ButtonSound") && eventType == 1)
		{
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "ButtonMusic" && eventType == 3)
		{
			DataCenter.Save().SetOptionMusic((int)(100f * wparam));
			TAudioManager.instance.musicVolume = (float)DataCenter.Save().GetOptionMusic() * 0.01f;
		}
		else if (control.name == "ButtonMusic" && eventType == 2)
		{
			DataCenter.Save().Save();
		}
		else if (control.name == "ButtonSound" && eventType == 3)
		{
			DataCenter.Save().SetOptionSound((int)(100f * wparam));
			TAudioManager.instance.soundVolume = (float)DataCenter.Save().GetOptionSound() * 0.01f;
		}
		else if (control.name == "ButtonSound" && eventType == 2)
		{
			DataCenter.Save().Save();
		}
		else if (control.name == "ButtonJoystick" && eventType == 3)
		{
			DataCenter.Save().SetOptionJoystick((int)(100f * wparam));
		}
		else if (control.name == "ButtonJoystick" && eventType == 2)
		{
			DataCenter.Save().Save();
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
			HideAttribute();
			UIBlockObj.SetActiveRecursively(false);
			UIFunctionBtn.SetPressed(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
	}

	private void SwitchScene(string scene)
	{
		UtilUIUpdateAttribute.SetLastScene("Options");
		if (scene == "Shop" || scene.Contains("MissionConfirm") || scene == "SuitUp")
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
}

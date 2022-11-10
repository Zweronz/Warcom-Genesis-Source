using System.Collections;
using UnityEngine;

public class UIScore : MonoBehaviour, TUIHandler
{
	public GameObject m_charactorUIShow;

	public GameObject UIBlockObj;

	public Player m_player;

	public GameObject m_left;

	public GameObject m_right;

	public TUIMeshText m_timePlayerNum;

	public TUIMeshText m_missionsExecutedNum;

	public TUIMeshText m_missionsCompletedNum;

	public TUIMeshText m_mainMissionsCompleteNum;

	public TUIMeshText m_optionalMissionsCompleteNum;

	public TUIMeshText m_totalKillsNum;

	public TUIMeshText m_bulletsFiredNum;

	public TUIMeshText m_hitsNum;

	public TUIMeshText m_accuracyNum;

	public TUIMeshText m_headshotsNum;

	public TUIMeshText m_itemsUsedNum;

	public int m_charactorIndex = 1;

	private bool m_charactorScoreInfoShow = true;

	private TUI m_tui;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		StartCoroutine(UpdateCharacterScoreInfo(m_charactorIndex));
		m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
		if (Mathf.Min(Screen.width, Screen.height) > 641)
		{
			m_charactorUIShow.transform.Find("effect_Platform").localPosition = new Vector3(-1.8961f, -1.3f, 12.93605f);
			m_charactorUIShow.transform.Find("Platform").localPosition = new Vector3(-1.6461f, -0.8f, 13.13885f);
		}
		CreatePlayerModel(m_charactorIndex);
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

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Left" && eventType == 3)
		{
			m_charactorIndex--;
			if (m_charactorIndex < 1)
			{
				m_charactorIndex = 4;
			}
			StartCoroutine(UpdateCharacterScoreInfo(m_charactorIndex));
			m_right.SetActiveRecursively(false);
			m_left.SetActiveRecursively(false);
		}
		else if (control.name == "Right" && eventType == 3)
		{
			m_charactorIndex++;
			if (m_charactorIndex > 4)
			{
				m_charactorIndex = 1;
			}
			StartCoroutine(UpdateCharacterScoreInfo(m_charactorIndex));
			m_right.SetActiveRecursively(false);
			m_left.SetActiveRecursively(false);
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
		else if (control.name == "Back" && eventType == 3)
		{
			SwitchScene("Base");
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

	public IEnumerator UpdateCharacterScoreInfo(int index)
	{
		if (m_charactorScoreInfoShow)
		{
			HideCharactorInfo();
			m_charactorScoreInfoShow = false;
			yield return new WaitForSeconds(0.25f);
		}
		TUIMeshText name = m_tui.transform.Find("TUIControl").Find("Name").Find("NameText")
			.GetComponent<TUIMeshText>();
		name.text = DataCenter.Save().GetCharacterEquipInfo(index).name;
		name.UpdateMesh();
		UpdateCharacterScore();
		ShowCharactorInfo();
		m_charactorScoreInfoShow = true;
		yield return new WaitForSeconds(0.25f);
		yield return 0;
	}

	private void ShowCharactorInfo()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Name").gameObject;
		gameObject.GetComponent<Animation>().Play("NameInAnim");
		GameObject gameObject2 = m_tui.transform.Find("TUIControl").Find("ScoreWindow").gameObject;
		gameObject2.GetComponent<Animation>().Play("EquipmentInAnim");
		GameObject gameObject3 = m_charactorUIShow.transform.Find("Camera").gameObject;
		gameObject3.GetComponent<Animation>().Play("CharactorUIShowCameraAnim");
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftCharactor_Slipin");
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_RightSide_Slipin");
		DeletePlayerModel();
		CreatePlayerModel(m_charactorIndex);
		GameObject gameObject4 = m_tui.transform.Find("TUIControl").Find("CharacterDetails").gameObject;
		gameObject4.GetComponent<Animation>().Play("CharacterDetailsIn");
		gameObject4.GetComponent<UtilMissionConfirmCharacterDetails>().RefreshCharacterDetails(m_player);
	}

	private void HideCharactorInfo()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Name").gameObject;
		gameObject.GetComponent<Animation>().Play("NameAnim");
		GameObject gameObject2 = m_tui.transform.Find("TUIControl").Find("ScoreWindow").gameObject;
		gameObject2.GetComponent<Animation>().Play("EquipmentAnim");
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_RightSide_Slipout");
		GameObject gameObject3 = m_charactorUIShow.transform.Find("Camera").gameObject;
		gameObject3.GetComponent<Animation>().Play("CharactorUIShowCameraAnimOut");
		GameObject gameObject4 = m_tui.transform.Find("TUIControl").Find("CharacterDetails").gameObject;
		gameObject4.GetComponent<Animation>().Play("CharacterDetailsOut");
	}

	private void UpdateCharacterScore()
	{
		int num = DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).totalTime / 3600;
		int num2 = (DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).totalTime - 3600 * num) / 60;
		int num3 = DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).totalTime - 3600 * num - 60 * num2;
		m_timePlayerNum.text = "Time Played: " + string.Format("{0:D2}", num) + ":" + string.Format("{0:D2}", num2) + ":" + string.Format("{0:D2}", num3);
		m_timePlayerNum.UpdateMesh();
		m_missionsExecutedNum.text = "Missions Attempted: " + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).playTimes;
		m_missionsExecutedNum.UpdateMesh();
		m_missionsCompletedNum.text = "Missions Completed: " + (DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).mainMissionComplete + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).auxiliaryMissionComplete);
		m_missionsCompletedNum.UpdateMesh();
		m_mainMissionsCompleteNum.text = "Main Missions Completed: " + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).mainMissionComplete;
		m_mainMissionsCompleteNum.UpdateMesh();
		m_optionalMissionsCompleteNum.text = "Side Missions Completed: " + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).auxiliaryMissionComplete;
		m_optionalMissionsCompleteNum.UpdateMesh();
		m_totalKillsNum.text = "Total Kills: " + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).enemyKill;
		m_totalKillsNum.UpdateMesh();
		m_bulletsFiredNum.text = "Bullets Fired: " + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).fireBullet;
		m_bulletsFiredNum.UpdateMesh();
		m_hitsNum.text = "Hits: " + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).hitBullet;
		m_hitsNum.UpdateMesh();
		if (DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).fireBullet == 0)
		{
			m_accuracyNum.text = "Accuracy: 0%";
		}
		else
		{
			m_accuracyNum.text = "Accuracy: " + (int)((float)DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).hitBullet * 100f / (float)DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).fireBullet) + "%";
		}
		m_accuracyNum.UpdateMesh();
		m_headshotsNum.text = "Headshots: " + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).enemyKillHeadShot;
		m_headshotsNum.UpdateMesh();
		m_itemsUsedNum.text = "Items Used: " + DataCenter.Save().GetCharacterEquipInfo(m_charactorIndex).itemUseCount;
		m_itemsUsedNum.UpdateMesh();
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
	}

	private void DeletePlayerModel()
	{
		if (m_player.GetGameObject() != null)
		{
			Object.Destroy(m_player.GetGameObject());
		}
	}

	private void SwitchScene(string scene)
	{
		UtilUIUpdateAttribute.SetLastScene("Score");
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

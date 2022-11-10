using UnityEngine;

public class UIMedals : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	public GameObject UIBlockObj;

	private Transform m_medalShowWindowTransform;

	private GameObject m_medalScroll;

	private int m_count;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		m_medalShowWindowTransform = m_tui.transform.Find("TUIControl").Find("MedalShowWindow");
		m_medalScroll = m_tui.transform.Find("TUIControl").Find("Medals").gameObject;
		m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
		UpdateMedalScroll();
		UpdateMedalShowWindow(m_medalScroll.transform.Find("ScrollObject").GetComponent<TUIButtonSelectGroupEx2>().current);
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
		if (control.name == "BkBtn" && eventType == 1)
		{
			UpdateMedalShowWindow(int.Parse(control.transform.parent.name));
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Meu_Medal");
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
		else if (control.name == "Scroll" && eventType == 1)
		{
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Meu_Slip");
		}
	}

	private void SwitchScene(string scene)
	{
		UtilUIUpdateAttribute.SetLastScene("Medals");
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

	private void UpdateMedalScroll()
	{
		GameObject gameObject = m_medalScroll.transform.Find("ScrollObject").Find("ItemPrefab").gameObject;
		int medalCount = DataCenter.Conf().GetMedalCount();
		for (int i = 0; i < medalCount; i++)
		{
			MedalInfo medalInfoByIndex = DataCenter.Conf().GetMedalInfoByIndex(i + 1);
			GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.name = (i + 1).ToString();
			gameObject2.transform.parent = gameObject.transform.parent;
			gameObject2.transform.localPosition = new Vector3(-175 + i / 2 * 60, 77 - i % 2 * 60, 0f);
			TUIMeshSprite component = gameObject2.transform.Find("Icon").GetComponent<TUIMeshSprite>();
			component.frameName = medalInfoByIndex.iconName;
			component.UpdateMesh();
			TUIMeshSprite component2 = gameObject2.transform.Find("IconGray").GetComponent<TUIMeshSprite>();
			component2.frameName = medalInfoByIndex.iconName;
			component2.UpdateMesh();
			if (DataCenter.Save().IsMedalUnlock(i + 1))
			{
				component.gameObject.SetActiveRecursively(true);
				component2.gameObject.SetActiveRecursively(false);
			}
			else
			{
				component.gameObject.SetActiveRecursively(false);
				component2.gameObject.SetActiveRecursively(true);
			}
			if (i == 0)
			{
				gameObject2.transform.Find("BkBtn").GetComponent<TUIButtonSelect>().SetSelected(true);
			}
		}
		int num = (medalCount + 3) / 4;
		TUIScroll component3 = m_medalScroll.transform.Find("Scroll").gameObject.GetComponent<TUIScroll>();
		component3.rangeXMin = -(num - 1) * 60 * 2;
		component3.borderXMin = component3.rangeXMin - 240f;
		component3.rangeXMax = 0f;
		component3.borderXMax = component3.rangeXMax + 240f;
		component3.pageX = new float[num];
		for (int j = 0; j < num; j++)
		{
			component3.pageX[j] = -(num - 1 - j) * 60 * 2;
		}
	}

	private void UpdateMedalShowWindow(int currentMedalIndex)
	{
		MedalInfo medalInfoByIndex = DataCenter.Conf().GetMedalInfoByIndex(currentMedalIndex);
		TUIMeshText component = m_medalShowWindowTransform.Find("Reward").GetComponent<TUIMeshText>();
		component.text = medalInfoByIndex.rewardMoney.ToString();
		component.UpdateMesh();
		TUIMeshText component2 = m_medalShowWindowTransform.Find("Details").GetComponent<TUIMeshText>();
		component2.text = medalInfoByIndex.desc;
		component2.UpdateMesh();
		TUIMeshText component3 = m_medalShowWindowTransform.Find("Progress").GetComponent<TUIMeshText>();
		component3.text = MedalAch(medalInfoByIndex) + "/" + medalInfoByIndex.conditionParam;
		component3.UpdateMesh();
		TUIMeshSprite component4 = m_medalShowWindowTransform.Find("Icon").GetComponent<TUIMeshSprite>();
		component4.frameName = medalInfoByIndex.iconName;
		component4.UpdateMesh();
		TUIMeshSprite component5 = m_medalShowWindowTransform.Find("IconGray").GetComponent<TUIMeshSprite>();
		component5.frameName = medalInfoByIndex.iconName;
		component5.UpdateMesh();
		if (DataCenter.Save().IsMedalUnlock(currentMedalIndex))
		{
			component4.gameObject.SetActiveRecursively(true);
			component5.gameObject.SetActiveRecursively(false);
		}
		else
		{
			component4.gameObject.SetActiveRecursively(false);
			component5.gameObject.SetActiveRecursively(true);
		}
		m_medalShowWindowTransform.Find("Achievements").gameObject.SetActiveRecursively(false);
	}

	private int MedalAch(MedalInfo medalInfo)
	{
		if (medalInfo.conditionName == "TimeCount")
		{
			if (DataCenter.Save().GetTotalTime() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetTotalTime();
		}
		if (medalInfo.conditionName == "KillCount")
		{
			if (DataCenter.Save().GetEnemyKill() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetEnemyKill();
		}
		if (medalInfo.conditionName == "HeadshotCount")
		{
			if (DataCenter.Save().GetEnemyHeadShot() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetEnemyHeadShot();
		}
		if (medalInfo.conditionName == "DeathCount")
		{
			if (DataCenter.Save().GetDead() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetDead();
		}
		if (medalInfo.conditionName == "BeHeadshotCount")
		{
			if (DataCenter.Save().GetDeadHeadShot() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetDeadHeadShot();
		}
		if (medalInfo.conditionName == "UseItemCount")
		{
			if (DataCenter.Save().GetItemUseCount() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetItemUseCount();
		}
		if (medalInfo.conditionName == "BulletCount")
		{
			if (DataCenter.Save().GetFireBullet() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetFireBullet();
		}
		if (medalInfo.conditionName == "MeleeKillCount")
		{
			if (DataCenter.Save().GetWeaponKnifeKill() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetWeaponKnifeKill();
		}
		if (medalInfo.conditionName == "PistolKillCount")
		{
			if (DataCenter.Save().GetWeaponHandgunKill() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetWeaponHandgunKill();
		}
		if (medalInfo.conditionName == "RifleKillCount")
		{
			if (DataCenter.Save().GetWeaponAssaultRifleKill() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetWeaponAssaultRifleKill();
		}
		if (medalInfo.conditionName == "RocketKillCount")
		{
			if (DataCenter.Save().GetWeaponRPGKill() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetWeaponRPGKill();
		}
		if (medalInfo.conditionName == "ShotgunKillCount")
		{
			if (DataCenter.Save().GetWeaponShotgunGunKill() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetWeaponShotgunGunKill();
		}
		if (medalInfo.conditionName == "SMGKillCount")
		{
			if (DataCenter.Save().GetWeaponSubmachineGunKill() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetWeaponSubmachineGunKill();
		}
		if (medalInfo.conditionName == "SniperKillCount")
		{
			if (DataCenter.Save().GetWeaponSniperKill() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetWeaponSniperKill();
		}
		if (medalInfo.conditionName == "KillScoutCount")
		{
			if (DataCenter.Save().GetKillScoutCount() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetKillScoutCount();
		}
		if (medalInfo.conditionName == "KillCommandoCount")
		{
			if (DataCenter.Save().GetKillCommandoCount() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetKillCommandoCount();
		}
		if (medalInfo.conditionName == "KillSniperCount")
		{
			if (DataCenter.Save().GetKillSniperCount() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetKillSniperCount();
		}
		if (medalInfo.conditionName == "KillRaiderCount")
		{
			if (DataCenter.Save().GetKillRaiderCount() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetKillRaiderCount();
		}
		if (medalInfo.conditionName == "BattlesCount")
		{
			if (DataCenter.Save().GetPlayTimes() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetPlayTimes();
		}
		if (medalInfo.conditionName == "TaskCount")
		{
			if (DataCenter.Save().GetMainMissionComplete() + DataCenter.Save().GetAuxiliaryMissionComplete() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetMainMissionComplete() + DataCenter.Save().GetAuxiliaryMissionComplete();
		}
		if (medalInfo.conditionName == "BuyWeaponCount")
		{
			if (DataCenter.Save().GetUnlockWeaponCount() - 10 >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetUnlockWeaponCount() - 10;
		}
		if (medalInfo.conditionName == "BuyArmorCount")
		{
			if (DataCenter.Save().GetUnlockArmorCount() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetUnlockArmorCount();
		}
		if (medalInfo.conditionName == "BuyItemCount")
		{
			if (DataCenter.Save().GetUnlockItemCount() >= medalInfo.conditionParam)
			{
				return medalInfo.conditionParam;
			}
			return DataCenter.Save().GetUnlockItemCount();
		}
		return 0;
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UIMissionConfirm : MonoBehaviour, TUIHandler
{
	public GameObject m_charactorUIShow;

	public TUITexture m_TUITextureDynamic;

	public TUITexture m_TUITextureDynamicLow;

	public GameObject UIBlockObj;

	public GameObject UIBlockObj2;

	public Player m_player;

	public int m_charactorIndex = 1;

	public GameObject m_cover;

	public GameObject m_left;

	public GameObject m_right;

	private TUI m_tui;

	private bool m_equipmentExpandShow;

	private bool m_charactorInfoShow = true;

	private bool m_equipmentWindowShow;

	private TUIScroll m_scroll;

	private GameObject m_scrollObect;

	private GameObject m_sliderObject;

	private string m_equipmentsExpandAnimName;

	private int m_lastCharacterCanUse = 1;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		m_scroll = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
			.Find("EquipmentExpandScroll")
			.Find("Scroll")
			.gameObject.GetComponent<TUIScroll>();
		m_scrollObect = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
			.Find("EquipmentExpandScroll")
			.Find("ScrollObject")
			.gameObject;
		m_sliderObject = m_tui.transform.Find("TUIControl").Find("Slider").Find("SliderEquip")
			.gameObject;
		StartCoroutine(UpdateCharacterEquipInfo(m_charactorIndex));
		StartCoroutine(UpdateCharacterInfo(m_charactorIndex));
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
			m_right.SetActiveRecursively(false);
			m_left.SetActiveRecursively(false);
			StartCoroutine(UpdateCharacterEquipInfo(m_charactorIndex));
			StartCoroutine(UpdateCharacterInfo(m_charactorIndex));
			DeletScrollChild();
			if (m_equipmentExpandShow)
			{
				m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
					.gameObject.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
				m_equipmentExpandShow = false;
			}
			TUIButtonSelect currentButton = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("Equipments")
				.GetComponent<TUIButtonSelectGroupEx2>()
				.GetCurrentButton();
			if (currentButton != null)
			{
				currentButton.SetSelected(false);
			}
			m_cover.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
			if (DataCenter.Save().IsCharacterUnlock(m_charactorIndex))
			{
				m_lastCharacterCanUse = m_charactorIndex;
			}
		}
		else if (control.name == "Right" && eventType == 3)
		{
			m_charactorIndex++;
			if (m_charactorIndex > 4)
			{
				m_charactorIndex = 1;
			}
			m_right.SetActiveRecursively(false);
			m_left.SetActiveRecursively(false);
			StartCoroutine(UpdateCharacterEquipInfo(m_charactorIndex));
			StartCoroutine(UpdateCharacterInfo(m_charactorIndex));
			DeletScrollChild();
			if (m_equipmentExpandShow)
			{
				m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
					.gameObject.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
				m_equipmentExpandShow = false;
			}
			TUIButtonSelect currentButton2 = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("Equipments")
				.GetComponent<TUIButtonSelectGroupEx2>()
				.GetCurrentButton();
			if (currentButton2 != null)
			{
				currentButton2.SetSelected(false);
			}
			m_cover.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
			if (DataCenter.Save().IsCharacterUnlock(m_charactorIndex))
			{
				m_lastCharacterCanUse = m_charactorIndex;
			}
		}
		else if (control.name == "PromptClose" && eventType == 3)
		{
			HidePrompt();
			UIBlockObj2.SetActiveRecursively(false);
			m_charactorIndex = m_lastCharacterCanUse;
			m_right.SetActiveRecursively(false);
			m_left.SetActiveRecursively(false);
			StartCoroutine(UpdateCharacterEquipInfo(m_charactorIndex));
			StartCoroutine(UpdateCharacterInfo(m_charactorIndex));
			DeletScrollChild();
			if (m_equipmentExpandShow)
			{
				m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
					.gameObject.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
				m_equipmentExpandShow = false;
			}
			TUIButtonSelect currentButton3 = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("Equipments")
				.GetComponent<TUIButtonSelectGroupEx2>()
				.GetCurrentButton();
			if (currentButton3 != null)
			{
				currentButton3.SetSelected(false);
			}
			m_cover.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "PromptGo" && eventType == 3)
		{
			SwitchScene("IAP");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "CharactorPromptUnlock" && eventType == 3)
		{
			int num = 30;
			if (m_charactorIndex == 2)
			{
				num = 5;
			}
			else if (m_charactorIndex == 3)
			{
				num = 10;
			}
			else if (m_charactorIndex == 4)
			{
				num = 10;
			}
			int crystal = DataCenter.Save().GetCrystal();
			if (crystal >= num)
			{
				DataCenter.Save().SetCrystal(crystal - num);
				DataCenter.Save().UnlockCharacter(m_charactorIndex);
				DataCenter.Save().Save();
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
				HideCharacterPrompt();
				UIBlockObj2.SetActiveRecursively(false);
				m_tui.transform.Find("TUIControl/CharacterDetails/Locked").gameObject.SetActiveRecursively(false);
			}
			else
			{
				HideCharacterPrompt();
				ShowPrompt();
				UIBlockObj2.SetActiveRecursively(true);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Back");
			}
		}
		else if (control.name == "CharactorPromptClose" && eventType == 3)
		{
			HideCharacterPrompt();
			UIBlockObj2.SetActiveRecursively(false);
			m_charactorIndex = m_lastCharacterCanUse;
			m_right.SetActiveRecursively(false);
			m_left.SetActiveRecursively(false);
			StartCoroutine(UpdateCharacterEquipInfo(m_charactorIndex));
			StartCoroutine(UpdateCharacterInfo(m_charactorIndex));
			DeletScrollChild();
			if (m_equipmentExpandShow)
			{
				m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
					.gameObject.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
				m_equipmentExpandShow = false;
			}
			TUIButtonSelect currentButton4 = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("Equipments")
				.GetComponent<TUIButtonSelectGroupEx2>()
				.GetCurrentButton();
			if (currentButton4 != null)
			{
				currentButton4.SetSelected(false);
			}
			m_cover.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
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
		else if (control.name == "WeaponBtn" && eventType == 1)
		{
			StartCoroutine(UpdateWeaponsExpand(control.transform));
			TUIButtonSelect currentButton5 = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
				.Find("EquipmentExpandScroll")
				.Find("ScrollObject")
				.GetComponent<TUIButtonSelectGroupEx2>()
				.GetCurrentButton();
			if (currentButton5 != null)
			{
				currentButton5.SetSelected(false);
			}
			if (DataCenter.Conf().GetWeaponInfoByName(control.transform.parent.name).type == WeaponType.Knife)
			{
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Melee_Drawup&Select");
			}
			else
			{
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Gun_Drawup&Select");
			}
		}
		else if (control.name == "ItemsBtn" && eventType == 1)
		{
			StartCoroutine(UpdateItemsExpand(control.transform));
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Items_Drawup&Select");
		}
		else if (control.name == "ArmorBtn" && eventType == 1)
		{
			StartCoroutine(UpdateArmorsExpand(control.transform));
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Ammor_Drawup&Select");
		}
		else if (control.name == "Btn" && eventType == 1)
		{
			m_cover.transform.parent = control.transform.parent;
			m_cover.SetActiveRecursively(true);
			m_cover.transform.position = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z - 1f);
			m_cover.transform.rotation = control.transform.rotation;
			m_cover.transform.GetComponent<TUIMeshSprite>().frameName = "EquipmentCover02";
			m_cover.transform.GetComponent<TUIMeshSprite>().UpdateMesh();
			string text = string.Empty;
			if (control.transform.parent.name.Contains("W_"))
			{
				Regex regex = new Regex("(\\s)+");
				text = DataCenter.Conf().GetWeaponInfoByName(m_cover.transform.parent.name).desc;
				text = regex.Replace(DataCenter.Conf().GetWeaponInfoByName(m_cover.transform.parent.name).desc.Replace("\n", ", "), " ");
				if (DataCenter.Conf().GetWeaponInfoByName(control.transform.parent.name).type == WeaponType.Knife)
				{
					DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Melee_Drawup&Select");
				}
				else
				{
					DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Gun_Drawup&Select");
				}
			}
			else if (control.transform.parent.name.Contains("A_"))
			{
				text = DataCenter.Conf().GetArmorInfoByName(m_cover.transform.parent.name).desc.Replace("\n", string.Empty);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Ammor_Drawup&Select");
			}
			else if (control.transform.parent.name.Contains("I_"))
			{
				text = DataCenter.Conf().GetItemInfoByName(m_cover.transform.parent.name).desc.Replace("\n", string.Empty);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Items_Drawup&Select");
			}
			m_tui.transform.Find("TUIControl").Find("EquipmentDetails").Find("Text")
				.GetComponent<TUIMeshText>()
				.text = text;
			m_tui.transform.Find("TUIControl").Find("EquipmentDetails").Find("Text")
				.GetComponent<TUIMeshText>()
				.UpdateMesh();
		}
		else if (control.name == "CloseExpand" && eventType == 3)
		{
			TUIButtonSelect currentButton6 = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("Equipments")
				.GetComponent<TUIButtonSelectGroupEx2>()
				.GetCurrentButton();
			if (currentButton6 != null)
			{
				currentButton6.SetSelected(false);
			}
			m_cover.SetActiveRecursively(false);
			if (!m_charactorInfoShow)
			{
				ShowCharactorInfo();
				m_charactorInfoShow = true;
			}
			DeletScrollChild();
			m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
				.gameObject.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
			m_tui.transform.Find("TUIControl").Find("EquipmentDetails").gameObject.GetComponent<Animation>().Play("EquipmentDetailsAnim");
			m_tui.transform.Find("TUIControl").Find("EquipmentWindow").transform.Find("EquipTitle").gameObject.GetComponent<Animation>().Play("NameAnim");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
			m_equipmentExpandShow = false;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Go" && eventType == 3)
		{
			SwitchScene(DataCenter.StateSingle().GetLevelInfo().scene.ToString());
			DataCenter.StateSingle().SetCurrentCharactor(m_charactorIndex);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_GO");
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

	public IEnumerator UpdateCharacterInfo(int index)
	{
		if (!DataCenter.Save().IsCharacterUnlock(index))
		{
			ShowCharacterPrompt(index);
			m_tui.transform.Find("TUIControl/CharacterDetails/Locked").gameObject.SetActiveRecursively(true);
		}
		else
		{
			HideCharacterPrompt();
			UIBlockObj2.SetActiveRecursively(false);
			m_tui.transform.Find("TUIControl/CharacterDetails/Locked").gameObject.SetActiveRecursively(false);
		}
		if (m_charactorInfoShow)
		{
			HideCharactorInfo();
			m_charactorInfoShow = false;
			yield return new WaitForSeconds(0.25f);
		}
		TUIMeshText name = m_tui.transform.Find("TUIControl").Find("Name").Find("NameText")
			.GetComponent<TUIMeshText>();
		name.text = DataCenter.Save().GetCharacterEquipInfo(index).name;
		name.UpdateMesh();
		ShowCharactorInfo();
		m_charactorInfoShow = true;
		yield return new WaitForSeconds(0.25f);
		yield return 0;
	}

	public IEnumerator UpdateCharacterEquipInfo(int index)
	{
		Transform equipmentWindow = m_tui.transform.Find("TUIControl").Find("EquipmentWindow");
		if (m_equipmentExpandShow)
		{
			equipmentWindow.transform.Find("EquipmentsExpand").gameObject.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
			m_tui.transform.Find("TUIControl").Find("EquipmentDetails").gameObject.GetComponent<Animation>().Play("EquipmentDetailsAnim");
			equipmentWindow.transform.Find("EquipTitle").gameObject.GetComponent<Animation>().Play("NameAnim");
			m_equipmentExpandShow = false;
		}
		if (m_equipmentWindowShow)
		{
			equipmentWindow.gameObject.GetComponent<Animation>().Play("EquipmentAnim");
			m_equipmentWindowShow = false;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_RightSide_Slipout");
			yield return new WaitForSeconds(0.25f);
		}
		RefreshCharacterEquipInfo(m_charactorIndex);
		equipmentWindow.gameObject.GetComponent<Animation>().Play("EquipmentInAnim");
		m_equipmentWindowShow = true;
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_RightSide_Slipin");
		yield return new WaitForSeconds(0.25f);
		yield return 0;
	}

	public void RefreshCharacterEquipInfo(int index)
	{
		Transform transform = m_tui.transform.Find("TUIControl").Find("EquipmentWindow");
		CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo(index);
		if (characterEquipInfo.weapon01 != string.Empty)
		{
			WeaponInfo weaponInfoByName = DataCenter.Conf().GetWeaponInfoByName(characterEquipInfo.weapon01);
			GameObject weapon01Object = transform.Find("Equipments").GetComponent<UtilEquipments>().weapon01Object;
			weapon01Object.name = weaponInfoByName.name;
			TUIMeshText component = weapon01Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component.text = weaponInfoByName.textName;
			component.UpdateMesh();
			TUIMeshSprite component2 = weapon01Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component2.frameName = weaponInfoByName.icon;
			LoadIconTexture(component2.frameName);
			component2.UpdateMesh();
			TUIMeshText component3 = weapon01Object.transform.Find("Content").Find("Ammo").GetComponent<TUIMeshText>();
			component3.text = weaponInfoByName.ammoCapacity + "/" + (weaponInfoByName.ammoAmount - weaponInfoByName.ammoCapacity).ToString();
			component3.UpdateMesh();
		}
		if (characterEquipInfo.weapon02 != string.Empty)
		{
			WeaponInfo weaponInfoByName2 = DataCenter.Conf().GetWeaponInfoByName(characterEquipInfo.weapon02);
			GameObject weapon02Object = transform.Find("Equipments").GetComponent<UtilEquipments>().weapon02Object;
			weapon02Object.name = weaponInfoByName2.name;
			TUIMeshText component4 = weapon02Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component4.text = weaponInfoByName2.textName;
			component4.UpdateMesh();
			TUIMeshSprite component5 = weapon02Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component5.frameName = weaponInfoByName2.icon;
			LoadIconTexture(component5.frameName);
			component5.UpdateMesh();
			TUIMeshText component6 = weapon02Object.transform.Find("Content").Find("Ammo").GetComponent<TUIMeshText>();
			component6.text = weaponInfoByName2.ammoCapacity + "/" + (weaponInfoByName2.ammoAmount - weaponInfoByName2.ammoCapacity).ToString();
			component6.UpdateMesh();
		}
		if (characterEquipInfo.weapon03 != string.Empty)
		{
			WeaponInfo weaponInfoByName3 = DataCenter.Conf().GetWeaponInfoByName(characterEquipInfo.weapon03);
			GameObject weapon03Object = transform.Find("Equipments").GetComponent<UtilEquipments>().weapon03Object;
			weapon03Object.name = weaponInfoByName3.name;
			TUIMeshText component7 = weapon03Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component7.text = weaponInfoByName3.textName;
			component7.UpdateMesh();
			TUIMeshSprite component8 = weapon03Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component8.frameName = weaponInfoByName3.icon;
			LoadIconTexture(component8.frameName);
			component8.UpdateMesh();
		}
		GameObject armor01Object = transform.Find("Equipments").GetComponent<UtilEquipments>().armor01Object;
		if (characterEquipInfo.armor01 != string.Empty)
		{
			armor01Object.transform.Find("Content").gameObject.SetActiveRecursively(true);
			armor01Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(false);
			ArmorInfo armorInfoByName = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor01);
			armor01Object.name = characterEquipInfo.armor01;
			TUIMeshText component9 = armor01Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component9.text = armorInfoByName.textName;
			component9.UpdateMesh();
			TUIMeshSprite component10 = armor01Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component10.frameName = armorInfoByName.icon;
			LoadIconTexture(component10.frameName);
			component10.UpdateMesh();
			armor01Object.transform.Find("Content").Find("Num").gameObject.SetActiveRecursively(false);
		}
		else
		{
			armor01Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(true);
			armor01Object.transform.Find("Content").gameObject.SetActiveRecursively(false);
		}
		GameObject armor02Object = transform.Find("Equipments").GetComponent<UtilEquipments>().armor02Object;
		if (characterEquipInfo.armor02 != string.Empty)
		{
			armor02Object.transform.Find("Content").gameObject.SetActiveRecursively(true);
			armor02Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(false);
			ArmorInfo armorInfoByName2 = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor02);
			armor02Object.name = characterEquipInfo.armor02;
			TUIMeshText component11 = armor02Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component11.text = armorInfoByName2.textName;
			component11.UpdateMesh();
			TUIMeshSprite component12 = armor02Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component12.frameName = armorInfoByName2.icon;
			LoadIconTexture(component12.frameName);
			component12.UpdateMesh();
			armor02Object.transform.Find("Content").Find("Num").gameObject.SetActiveRecursively(false);
		}
		else
		{
			armor02Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(true);
			armor02Object.transform.Find("Content").gameObject.SetActiveRecursively(false);
		}
		GameObject armor03Object = transform.Find("Equipments").GetComponent<UtilEquipments>().armor03Object;
		if (characterEquipInfo.armor03 != string.Empty)
		{
			armor03Object.transform.Find("Content").gameObject.SetActiveRecursively(true);
			armor03Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(false);
			ArmorInfo armorInfoByName3 = DataCenter.Conf().GetArmorInfoByName(characterEquipInfo.armor03);
			armor03Object.name = characterEquipInfo.armor03;
			TUIMeshText component13 = armor03Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component13.text = armorInfoByName3.textName;
			component13.UpdateMesh();
			TUIMeshSprite component14 = armor03Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component14.frameName = armorInfoByName3.icon;
			LoadIconTexture(component14.frameName);
			component14.UpdateMesh();
			armor03Object.transform.Find("Content").Find("Num").gameObject.SetActiveRecursively(false);
		}
		else
		{
			armor03Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(true);
			armor03Object.transform.Find("Content").gameObject.SetActiveRecursively(false);
		}
		GameObject items01Object = transform.Find("Equipments").GetComponent<UtilEquipments>().items01Object;
		if (characterEquipInfo.item01 != string.Empty)
		{
			items01Object.transform.Find("Content").gameObject.SetActiveRecursively(true);
			items01Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(false);
			ItemInfo itemInfoByName = DataCenter.Conf().GetItemInfoByName(characterEquipInfo.item01);
			items01Object.name = characterEquipInfo.item01;
			TUIMeshText component15 = items01Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component15.text = itemInfoByName.textName;
			component15.UpdateMesh();
			TUIMeshSprite component16 = items01Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component16.frameName = itemInfoByName.icon;
			LoadIconTexture(component16.frameName);
			component16.UpdateMesh();
			TUIMeshText component17 = items01Object.transform.Find("Content").Find("Num").GetComponent<TUIMeshText>();
			component17.text = "X" + DataCenter.Save().GetItemNums(itemInfoByName.name);
			component17.UpdateMesh();
		}
		else
		{
			items01Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(true);
			items01Object.transform.Find("Content").gameObject.SetActiveRecursively(false);
		}
		GameObject items02Object = transform.Find("Equipments").GetComponent<UtilEquipments>().items02Object;
		if (characterEquipInfo.item02 != string.Empty)
		{
			items02Object.transform.Find("Content").gameObject.SetActiveRecursively(true);
			items02Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(false);
			ItemInfo itemInfoByName2 = DataCenter.Conf().GetItemInfoByName(characterEquipInfo.item02);
			items02Object.name = characterEquipInfo.item02;
			TUIMeshText component18 = items02Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component18.text = itemInfoByName2.textName;
			component18.UpdateMesh();
			TUIMeshSprite component19 = items02Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component19.frameName = itemInfoByName2.icon;
			LoadIconTexture(component19.frameName);
			component19.UpdateMesh();
			TUIMeshText component20 = items02Object.transform.Find("Content").Find("Num").GetComponent<TUIMeshText>();
			component20.text = "X" + DataCenter.Save().GetItemNums(itemInfoByName2.name);
			component20.UpdateMesh();
		}
		else
		{
			items02Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(true);
			items02Object.transform.Find("Content").gameObject.SetActiveRecursively(false);
		}
		GameObject items03Object = transform.Find("Equipments").GetComponent<UtilEquipments>().items03Object;
		if (characterEquipInfo.item03 != string.Empty)
		{
			items03Object.transform.Find("Content").gameObject.SetActiveRecursively(true);
			items03Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(false);
			ItemInfo itemInfoByName3 = DataCenter.Conf().GetItemInfoByName(characterEquipInfo.item03);
			items03Object.name = characterEquipInfo.item03;
			TUIMeshText component21 = items03Object.transform.Find("Content").Find("Name").GetComponent<TUIMeshText>();
			component21.text = itemInfoByName3.textName;
			component21.UpdateMesh();
			TUIMeshSprite component22 = items03Object.transform.Find("Content").Find("Icon").GetComponent<TUIMeshSprite>();
			component22.frameName = itemInfoByName3.icon;
			LoadIconTexture(component22.frameName);
			component22.UpdateMesh();
			TUIMeshText component23 = items03Object.transform.Find("Content").Find("Num").GetComponent<TUIMeshText>();
			component23.text = "X" + DataCenter.Save().GetItemNums(itemInfoByName3.name);
			component23.UpdateMesh();
		}
		else
		{
			items03Object.transform.Find("BtnImg").gameObject.SetActiveRecursively(true);
			items03Object.transform.Find("Content").gameObject.SetActiveRecursively(false);
		}
	}

	private void SwitchScene(string scene)
	{
		UtilUIUpdateAttribute.SetLastScene("MissionConfirm");
		if (scene == "Shop" || scene.Contains("MissionConfirm") || scene == "SuitUp" || scene.Contains("Level"))
		{
			UtilUIUpdateAttribute.SetNextScene(scene);
			m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
				.FadeOut("Loading");
		}
		else
		{
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

	private void ShowCharactorInfo()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Name").gameObject;
		gameObject.GetComponent<Animation>().Play("NameInAnim");
		GameObject gameObject2 = m_charactorUIShow.transform.Find("Camera").gameObject;
		gameObject2.GetComponent<Animation>().Play("CharactorUIShowCameraAnim");
		DeletePlayerModel();
		CreatePlayerModel(m_charactorIndex);
		GameObject gameObject3 = m_tui.transform.Find("TUIControl").Find("CharacterDetails").gameObject;
		gameObject3.GetComponent<Animation>().Play("CharacterDetailsIn");
		gameObject3.GetComponent<UtilMissionConfirmCharacterDetails>().RefreshCharacterDetails(m_player);
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftCharactor_Slipin");
	}

	private void HideCharactorInfo()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("Name").gameObject;
		gameObject.GetComponent<Animation>().Play("NameAnim");
		GameObject gameObject2 = m_tui.transform.Find("TUIControl").Find("CharacterDetails").gameObject;
		gameObject2.GetComponent<Animation>().Play("CharacterDetailsOut");
		GameObject gameObject3 = m_charactorUIShow.transform.Find("Camera").gameObject;
		gameObject3.GetComponent<Animation>().Play("CharactorUIShowCameraAnimOut");
	}

	private IEnumerator UpdateWeaponsExpand(Transform control)
	{
		m_cover.SetActiveRecursively(true);
		m_cover.transform.position = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z - 1f);
		m_cover.transform.rotation = control.transform.rotation;
		m_cover.transform.GetComponent<TUIMeshSprite>().frameName = "EquipmentCover01";
		m_cover.transform.GetComponent<TUIMeshSprite>().UpdateMesh();
		string weaponName = control.transform.parent.name;
		WeaponType type = DataCenter.Conf().GetWeaponInfoByName(weaponName).type;
		GameObject equipmentsExpand = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
			.gameObject;
		DeletScrollChild();
		m_cover.transform.parent = control.transform.parent;
		Regex regExp = new Regex("(\\s)+");
		string desc = regExp.Replace(DataCenter.Conf().GetWeaponInfoByName(m_cover.transform.parent.name).desc.Replace("\n", ", "), " ");
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").Find("Text")
			.GetComponent<TUIMeshText>()
			.text = desc;
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").Find("Text")
			.GetComponent<TUIMeshText>()
			.UpdateMesh();
		GameObject itemPrefab = equipmentsExpand.transform.Find("EquipmentExpandScroll").Find("ScrollObject").Find("ItemPrefab")
			.gameObject;
		int l;
		if (type == WeaponType.Knife)
		{
			string coldWeaponName = string.Empty;
			if (weaponName.Contains("Wrench"))
			{
				coldWeaponName = "Wrench";
			}
			else if (weaponName.Contains("Bayonet"))
			{
				coldWeaponName = "Bayonet";
			}
			else if (weaponName.Contains("Ka-Bar"))
			{
				coldWeaponName = "Ka-Bar";
			}
			else if (weaponName.Contains("Hatchet"))
			{
				coldWeaponName = "Hatchet";
			}
			List<WeaponInfo> coldWeaponMap = DataCenter.Conf().GetColdWeaponByCharacter(coldWeaponName);
			int count2 = coldWeaponMap.Count;
			l = 0;
			for (int k = 0; k < count2; k++)
			{
				WeaponInfo weaponInfo2 = coldWeaponMap[k];
				if (DataCenter.Save().IsWeaponUnlock(weaponInfo2.name))
				{
					GameObject item2 = Object.Instantiate(itemPrefab) as GameObject;
					item2.name = weaponInfo2.name;
					item2.transform.parent = itemPrefab.transform.parent;
					item2.transform.localPosition = new Vector3(34f, 70 - l * 65, 0f);
					TUIMeshText name2 = item2.transform.Find("Name").GetComponent<TUIMeshText>();
					name2.text = weaponInfo2.textName;
					name2.UpdateMesh();
					TUIMeshSprite icon2 = item2.transform.Find("Icon").GetComponent<TUIMeshSprite>();
					icon2.frameName = weaponInfo2.icon;
					LoadIconTexture(icon2.frameName);
					icon2.UpdateMesh();
					GameObject numObject2 = item2.transform.Find("Num").gameObject;
					numObject2.SetActiveRecursively(false);
					l++;
				}
			}
		}
		else
		{
			int count2 = DataCenter.Conf().GetWeaponCount(type);
			l = 0;
			for (int j = 0; j < count2; j++)
			{
				WeaponInfo weaponInfo = DataCenter.Conf().GetWeaponInfo(type, j);
				if (DataCenter.Save().IsWeaponUnlock(weaponInfo.name))
				{
					GameObject item = Object.Instantiate(itemPrefab) as GameObject;
					item.name = weaponInfo.name;
					item.transform.parent = itemPrefab.transform.parent;
					item.transform.localPosition = new Vector3(34f, 70 - l * 65, 0f);
					TUIMeshText name = item.transform.Find("Name").GetComponent<TUIMeshText>();
					name.text = weaponInfo.textName;
					name.UpdateMesh();
					TUIMeshSprite icon = item.transform.Find("Icon").GetComponent<TUIMeshSprite>();
					icon.frameName = weaponInfo.icon;
					LoadIconTexture(icon.frameName);
					icon.UpdateMesh();
					GameObject numObject = item.transform.Find("Num").gameObject;
					numObject.SetActiveRecursively(false);
					l++;
				}
			}
		}
		m_scroll.rangeYMin = 0f;
		m_scroll.borderYMin = m_scroll.rangeYMin - 240f;
		m_scroll.rangeYMax = l * 65;
		m_scroll.borderYMax = m_scroll.rangeYMax + 240f;
		m_scroll.pageY = new float[l];
		m_scroll.position = Vector2.zero;
		m_scroll.Reset();
		m_scrollObect.transform.localPosition = Vector2.zero;
		for (int i = 0; i < l; i++)
		{
			m_scroll.pageY[i] = (l - 1 - i) * 65;
		}
		if (m_charactorInfoShow)
		{
			HideCharactorInfo();
			m_charactorInfoShow = false;
		}
		if (m_equipmentExpandShow)
		{
			equipmentsExpand.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
			m_tui.transform.Find("TUIControl").Find("EquipmentDetails").gameObject.GetComponent<Animation>().Play("EquipmentDetailsAnim");
			equipmentsExpand.transform.parent.Find("EquipTitle").gameObject.GetComponent<Animation>().Play("NameAnim");
			m_equipmentExpandShow = false;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
			yield return new WaitForSeconds(0.25f);
		}
		equipmentsExpand.GetComponent<Animation>().Play("EquipmentExpandAnimUp");
		m_equipmentsExpandAnimName = "EquipmentExpandAnimUp";
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").gameObject.GetComponent<Animation>().Play("EquipmentDetailsInAnim");
		equipmentsExpand.transform.parent.Find("EquipTitle").gameObject.GetComponent<Animation>().Play("NameInAnim");
		switch (type)
		{
		case WeaponType.Knife:
			equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
				.text = "MELEE";
			break;
		case WeaponType.Handgun:
			equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
				.text = "PISTOLS";
			break;
		case WeaponType.AssaultRifle:
			equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
				.text = "RIFLES";
			break;
		case WeaponType.SubmachineGun:
			equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
				.text = "SMGS";
			break;
		case WeaponType.Shotgun:
			equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
				.text = "SHOTGUNS";
			break;
		case WeaponType.SniperRifle:
			equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
				.text = "SNIPERS";
			break;
		case WeaponType.RPG:
			equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
				.text = "RPGS";
			break;
		}
		equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
			.UpdateMesh();
		m_equipmentExpandShow = true;
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipin");
		yield return 0;
	}

	private IEnumerator UpdateArmorsExpand(Transform control)
	{
		m_cover.SetActiveRecursively(true);
		m_cover.transform.position = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z - 1f);
		m_cover.transform.rotation = control.transform.rotation;
		m_cover.transform.GetComponent<TUIMeshSprite>().frameName = "EquipmentCover01";
		m_cover.transform.GetComponent<TUIMeshSprite>().UpdateMesh();
		GameObject equipmentsExpand = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
			.gameObject;
		DeletScrollChild();
		m_cover.transform.parent = control.transform.parent;
		string desc = (m_cover.transform.parent.Find("Content").gameObject.active ? DataCenter.Conf().GetArmorInfoByName(m_cover.transform.parent.name).desc.Replace("\n", string.Empty) : string.Empty);
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").Find("Text")
			.GetComponent<TUIMeshText>()
			.text = desc;
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").Find("Text")
			.GetComponent<TUIMeshText>()
			.UpdateMesh();
		GameObject itemPrefab = equipmentsExpand.transform.Find("EquipmentExpandScroll").Find("ScrollObject").Find("ItemPrefab")
			.gameObject;
		int totalCount = 0;
		List<ArmorInfo> armorList = DataCenter.Conf().GetArmorInfo();
		for (int j = 0; j < armorList.Count; j++)
		{
			ArmorInfo armorInfo = armorList[j];
			if (DataCenter.Save().IsArmorUnlock(armorInfo.name))
			{
				GameObject item = Object.Instantiate(itemPrefab) as GameObject;
				item.name = armorInfo.name;
				item.transform.parent = itemPrefab.transform.parent;
				item.transform.localPosition = new Vector3(34f, 70 - totalCount * 65, 0f);
				TUIMeshText name = item.transform.Find("Name").GetComponent<TUIMeshText>();
				name.text = armorInfo.textName;
				name.UpdateMesh();
				TUIMeshSprite icon = item.transform.Find("Icon").GetComponent<TUIMeshSprite>();
				icon.frameName = armorInfo.icon;
				LoadIconTexture(icon.frameName);
				icon.UpdateMesh();
				GameObject numObject = item.transform.Find("Num").gameObject;
				numObject.SetActiveRecursively(false);
				totalCount++;
			}
		}
		m_scroll.rangeYMin = 0f;
		m_scroll.borderYMin = m_scroll.rangeYMin - 240f;
		m_scroll.rangeYMax = totalCount * 65;
		m_scroll.borderYMax = m_scroll.rangeYMax + 240f;
		m_scroll.pageY = new float[totalCount];
		m_scroll.position = Vector2.zero;
		m_scroll.Reset();
		m_scrollObect.transform.localPosition = Vector2.zero;
		for (int i = 0; i < totalCount; i++)
		{
			m_scroll.pageY[i] = (totalCount - 1 - i) * 65;
		}
		if (m_charactorInfoShow)
		{
			HideCharactorInfo();
			m_charactorInfoShow = false;
		}
		if (m_equipmentExpandShow)
		{
			equipmentsExpand.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
			m_tui.transform.Find("TUIControl").Find("EquipmentDetails").gameObject.GetComponent<Animation>().Play("EquipmentDetailsAnim");
			equipmentsExpand.transform.parent.Find("EquipTitle").gameObject.GetComponent<Animation>().Play("NameAnim");
			m_equipmentExpandShow = false;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
			yield return new WaitForSeconds(0.25f);
		}
		equipmentsExpand.GetComponent<Animation>().Play("EquipmentExpandAnim");
		m_equipmentsExpandAnimName = "EquipmentExpandAnim";
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").gameObject.GetComponent<Animation>().Play("EquipmentDetailsInAnim");
		equipmentsExpand.transform.parent.Find("EquipTitle").gameObject.GetComponent<Animation>().Play("NameInAnim");
		equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
			.text = "GEARS";
		equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
			.UpdateMesh();
		m_equipmentExpandShow = true;
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipin");
		yield return 0;
	}

	private IEnumerator UpdateItemsExpand(Transform control)
	{
		m_cover.SetActiveRecursively(true);
		m_cover.transform.position = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z - 1f);
		m_cover.transform.rotation = control.transform.rotation;
		m_cover.transform.GetComponent<TUIMeshSprite>().frameName = "EquipmentCover01";
		m_cover.transform.GetComponent<TUIMeshSprite>().UpdateMesh();
		GameObject equipmentsExpand = m_tui.transform.Find("TUIControl").Find("EquipmentWindow").Find("EquipmentsExpand")
			.gameObject;
		DeletScrollChild();
		m_cover.transform.parent = control.transform.parent;
		string desc = (m_cover.transform.parent.Find("Content").gameObject.active ? DataCenter.Conf().GetItemInfoByName(m_cover.transform.parent.name).desc.Replace("\n", string.Empty) : string.Empty);
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").Find("Text")
			.GetComponent<TUIMeshText>()
			.text = desc;
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").Find("Text")
			.GetComponent<TUIMeshText>()
			.UpdateMesh();
		GameObject itemPrefab = equipmentsExpand.transform.Find("EquipmentExpandScroll").Find("ScrollObject").Find("ItemPrefab")
			.gameObject;
		int totalCount = 0;
		List<ItemInfo> itemList = DataCenter.Conf().GetItemInfo();
		for (int j = 0; j < itemList.Count; j++)
		{
			ItemInfo itemInfo = itemList[j];
			if (DataCenter.Save().IsItemUnlock(itemInfo.name) && DataCenter.Save().GetItemNums(itemInfo.name) > 0)
			{
				GameObject item = Object.Instantiate(itemPrefab) as GameObject;
				item.name = itemInfo.name;
				item.transform.parent = itemPrefab.transform.parent;
				item.transform.localPosition = new Vector3(34f, 70 - totalCount * 65, 0f);
				TUIMeshText name = item.transform.Find("Name").GetComponent<TUIMeshText>();
				name.text = itemInfo.textName;
				name.UpdateMesh();
				TUIMeshSprite icon = item.transform.Find("Icon").GetComponent<TUIMeshSprite>();
				icon.frameName = itemInfo.icon;
				LoadIconTexture(icon.frameName);
				icon.UpdateMesh();
				GameObject numObject = item.transform.Find("Num").gameObject;
				numObject.SetActiveRecursively(true);
				numObject.GetComponent<TUIMeshText>().text = "X" + DataCenter.Save().GetItemNums(itemInfo.name);
				numObject.GetComponent<TUIMeshText>().UpdateMesh();
				totalCount++;
			}
		}
		m_scroll.rangeYMin = 0f;
		m_scroll.borderYMin = m_scroll.rangeYMin - 240f;
		m_scroll.rangeYMax = totalCount * 65;
		m_scroll.borderYMax = m_scroll.rangeYMax + 240f;
		m_scroll.pageY = new float[totalCount];
		m_scroll.position = Vector2.zero;
		m_scroll.Reset();
		m_scrollObect.transform.localPosition = Vector2.zero;
		for (int i = 0; i < totalCount; i++)
		{
			m_scroll.pageY[i] = (totalCount - 1 - i) * 65;
		}
		if (m_charactorInfoShow)
		{
			HideCharactorInfo();
			m_charactorInfoShow = false;
		}
		if (m_equipmentExpandShow)
		{
			equipmentsExpand.GetComponent<Animation>().Play(m_equipmentsExpandAnimName.Replace("ExpandAnim", "ExpandOutAnim"));
			m_tui.transform.Find("TUIControl").Find("EquipmentDetails").gameObject.GetComponent<Animation>().Play("EquipmentDetailsAnim");
			equipmentsExpand.transform.parent.Find("EquipTitle").gameObject.GetComponent<Animation>().Play("NameAnim");
			m_equipmentExpandShow = false;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipout");
			yield return new WaitForSeconds(0.25f);
		}
		equipmentsExpand.GetComponent<Animation>().Play("EquipmentExpandAnimDown");
		m_equipmentsExpandAnimName = "EquipmentExpandAnimDown";
		m_tui.transform.Find("TUIControl").Find("EquipmentDetails").gameObject.GetComponent<Animation>().Play("EquipmentDetailsInAnim");
		equipmentsExpand.transform.parent.Find("EquipTitle").gameObject.GetComponent<Animation>().Play("NameInAnim");
		equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
			.text = "ITEMS";
		equipmentsExpand.transform.parent.Find("EquipTitle").Find("NameText").GetComponent<TUIMeshText>()
			.UpdateMesh();
		m_equipmentExpandShow = true;
		DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_SuitUp_LeftSide_Slipin");
		yield return 0;
	}

	private void DeletScrollChild()
	{
		for (int i = 0; i < m_scrollObect.transform.childCount; i++)
		{
			if (m_cover.transform.IsChildOf(m_scrollObect.transform.GetChild(i)))
			{
				m_cover.transform.parent = m_tui.transform;
				m_cover.SetActiveRecursively(false);
			}
			if (m_scrollObect.transform.GetChild(i).name != "ItemPrefab")
			{
				Object.Destroy(m_scrollObect.transform.GetChild(i).gameObject);
			}
		}
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
		m_player.GetTransform().GetComponent<CharacterPassivePara>().ArmorEffectForUIShow(characterEquipInfo);
	}

	private void DeletePlayerModel()
	{
		if (m_player.GetGameObject() != null)
		{
			Object.Destroy(m_player.GetGameObject());
		}
	}

	private void ShowPrompt()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("IapPrompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 0f, gameObject.transform.localPosition.z);
		gameObject.GetComponent<Animation>().Play("IapPromptOpen");
	}

	private void HidePrompt()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("IapPrompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 20000f, gameObject.transform.localPosition.z);
	}

	private void ShowCharacterPrompt(int characterIndex)
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("CharactorPrompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 0f, gameObject.transform.localPosition.z);
		gameObject.GetComponent<Animation>().Play("IapPromptOpen");
		UIBlockObj2.SetActiveRecursively(true);
		TUIMeshText component = gameObject.transform.Find("Content/Details01").GetComponent<TUIMeshText>();
		switch (characterIndex)
		{
		case 2:
			component.text = "Osprey - Sniper";
			break;
		case 3:
			component.text = "Harrier - Commando";
			break;
		case 4:
			component.text = "Cassowary - Raider";
			break;
		}
		component.UpdateMesh();
		TUIMeshText component2 = gameObject.transform.Find("Content/CrystalText").GetComponent<TUIMeshText>();
		switch (characterIndex)
		{
		case 2:
			component2.text = "5";
			break;
		case 3:
			component2.text = "10";
			break;
		case 4:
			component2.text = "10";
			break;
		}
		component2.UpdateMesh();
	}

	private void HideCharacterPrompt()
	{
		GameObject gameObject = m_tui.transform.Find("TUIControl").Find("CharactorPrompt").gameObject;
		gameObject.transform.localPosition = new Vector3(0f, 20000f, gameObject.transform.localPosition.z);
	}

	private void LoadIconTexture(string iconFrameName)
	{
		m_TUITextureDynamic.LoadFrame(iconFrameName);
	}
}

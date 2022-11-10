using UnityEngine;

public class UIShop : MonoBehaviour, TUIHandler
{
	public int m_charactorIndex = 1;

	private string m_equipmentName = "W_SubmachineGun00";

	public GameObject UIBlockObj;

	public TUIButtonPush UIFunctionBtn;

	private Transform m_currentBtn;

	private TUI m_tui;

	private int unlockCharacterIndex;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		unlockCharacterIndex = 0;
		m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
		UpdateWeaponInfoInShowWindow();
		RefreshCharacterLocked();
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
		if (control.name == "Buy" && eventType == 3)
		{
			RefreshCurrentEquipmentAndCharactor();
			if (m_equipmentName.Contains("W_"))
			{
				WeaponInfo weaponInfoByName = DataCenter.Conf().GetWeaponInfoByName(m_equipmentName);
				if (weaponInfoByName.buyMoney != -1)
				{
					if (DataCenter.Save().GetMoney() >= weaponInfoByName.buyMoney)
					{
						int money = DataCenter.Save().GetMoney();
						DataCenter.Save().SetMoney(money - weaponInfoByName.buyMoney);
						DataCenter.Save().UnlockWeapon(m_equipmentName);
						DataCenter.Save().Save();
						UpdateWeaponInfoInShowWindow();
						DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
						return;
					}
				}
				else if (DataCenter.Save().GetCrystal() >= weaponInfoByName.buyCrystal)
				{
					int crystal = DataCenter.Save().GetCrystal();
					DataCenter.Save().SetCrystal(crystal - weaponInfoByName.buyCrystal);
					DataCenter.Save().UnlockWeapon(m_equipmentName);
					DataCenter.Save().Save();
					UpdateWeaponInfoInShowWindow();
					DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
					return;
				}
			}
			else if (m_equipmentName.Contains("A_"))
			{
				ArmorInfo armorInfoByName = DataCenter.Conf().GetArmorInfoByName(m_equipmentName);
				if (armorInfoByName.buyMoney != -1)
				{
					if (DataCenter.Save().GetMoney() >= armorInfoByName.buyMoney)
					{
						int money2 = DataCenter.Save().GetMoney();
						DataCenter.Save().SetMoney(money2 - armorInfoByName.buyMoney);
						DataCenter.Save().UnlockArmor(m_equipmentName, 0);
						DataCenter.Save().Save();
						UpdateArmorInfoInShowWindow();
						DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
						return;
					}
				}
				else if (DataCenter.Save().GetCrystal() >= armorInfoByName.buyCrystal)
				{
					int crystal2 = DataCenter.Save().GetCrystal();
					DataCenter.Save().SetCrystal(crystal2 - armorInfoByName.buyCrystal);
					DataCenter.Save().UnlockArmor(m_equipmentName, 0);
					DataCenter.Save().Save();
					UpdateArmorInfoInShowWindow();
					DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
					return;
				}
			}
			else if (m_equipmentName.Contains("I_"))
			{
				ItemInfo itemInfoByName = DataCenter.Conf().GetItemInfoByName(m_equipmentName);
				if (itemInfoByName.buyMoney != -1)
				{
					if (DataCenter.Save().GetMoney() >= itemInfoByName.buyMoney)
					{
						TAnalyticsManager.AttributeAddInteger(itemInfoByName.name, itemInfoByName.buyAmount);
						int money3 = DataCenter.Save().GetMoney();
						DataCenter.Save().SetMoney(money3 - itemInfoByName.buyMoney);
						if (!DataCenter.Save().IsItemUnlock(m_equipmentName))
						{
							DataCenter.Save().UnlockItem(m_equipmentName, 0);
							DataCenter.Save().SetItemNums(m_equipmentName, itemInfoByName.buyAmount);
						}
						else
						{
							DataCenter.Save().SetItemNums(m_equipmentName, itemInfoByName.buyAmount + DataCenter.Save().GetItemNums(m_equipmentName));
						}
						DataCenter.Save().Save();
						UpdateItemInfoInShowWindow();
						DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
						return;
					}
				}
				else if (DataCenter.Save().GetCrystal() >= itemInfoByName.buyCrystal)
				{
					TAnalyticsManager.AttributeAddInteger(itemInfoByName.name, itemInfoByName.buyAmount);
					int crystal3 = DataCenter.Save().GetCrystal();
					DataCenter.Save().SetCrystal(crystal3 - itemInfoByName.buyCrystal);
					if (!DataCenter.Save().IsItemUnlock(m_equipmentName))
					{
						DataCenter.Save().UnlockItem(m_equipmentName, 0);
						DataCenter.Save().SetItemNums(m_equipmentName, itemInfoByName.buyAmount);
					}
					else
					{
						DataCenter.Save().SetItemNums(m_equipmentName, itemInfoByName.buyAmount + DataCenter.Save().GetItemNums(m_equipmentName));
					}
					DataCenter.Save().Save();
					UpdateItemInfoInShowWindow();
					DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
					return;
				}
			}
			ShowPrompt();
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Back");
		}
		else if (control.name == "PromptClose" && eventType == 3)
		{
			HidePrompt();
			UIBlockObj.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "PromptGo" && eventType == 3)
		{
			SwitchScene("IAP");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Charactor02Locked" && eventType == 3)
		{
			unlockCharacterIndex = 2;
			ShowCharacterPrompt(2);
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Charactor03Locked" && eventType == 3)
		{
			unlockCharacterIndex = 3;
			ShowCharacterPrompt(3);
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Charactor04Locked" && eventType == 3)
		{
			unlockCharacterIndex = 4;
			ShowCharacterPrompt(4);
			UIBlockObj.SetActiveRecursively(true);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "CharactorPromptUnlock" && eventType == 3)
		{
			int num = 30;
			if (unlockCharacterIndex == 2)
			{
				num = 5;
			}
			else if (unlockCharacterIndex == 3)
			{
				num = 10;
			}
			else if (unlockCharacterIndex == 4)
			{
				num = 10;
			}
			int crystal4 = DataCenter.Save().GetCrystal();
			if (crystal4 >= num)
			{
				DataCenter.Save().SetCrystal(crystal4 - num);
				DataCenter.Save().UnlockCharacter(unlockCharacterIndex);
				DataCenter.Save().Save();
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
				RefreshCharacterLocked();
				HideCharacterPrompt();
				UIBlockObj.SetActiveRecursively(false);
			}
			else
			{
				HideCharacterPrompt();
				ShowPrompt();
				UIBlockObj.SetActiveRecursively(true);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Back");
			}
		}
		else if (control.name == "CharactorPromptClose" && eventType == 3)
		{
			HideCharacterPrompt();
			UIBlockObj.SetActiveRecursively(false);
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Back" && eventType == 3)
		{
			if (UtilUIUpdateAttribute.GetLastScene() != "MissionEnd" && UtilUIUpdateAttribute.GetLastScene() != "IAP")
			{
				SwitchScene(UtilUIUpdateAttribute.GetLastScene());
			}
			else
			{
				SwitchScene("WorldMap");
			}
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
			HideAttribute();
			UIBlockObj.SetActiveRecursively(false);
			UIFunctionBtn.SetPressed(false);
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
		else if (control.GetType() == typeof(TUIButtonSelect) && eventType == 1)
		{
			RefreshCurrentEquipmentAndCharactor();
			if (m_equipmentName.Contains("W_"))
			{
				UpdateWeaponInfoInShowWindow();
			}
			else if (m_equipmentName.Contains("A_"))
			{
				UpdateArmorInfoInShowWindow();
			}
			else if (m_equipmentName.Contains("I_"))
			{
				UpdateItemInfoInShowWindow();
			}
			if (control.name.Contains("Charactor"))
			{
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Charactar");
			}
			else
			{
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
			}
		}
		else if (control.GetType() == typeof(TUIScroll) && eventType == 1)
		{
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Meu_Slip");
		}
	}

	private void SwitchScene(string scene)
	{
		UtilUIUpdateAttribute.SetLastScene("Shop");
		if (scene == "Shop" || scene.Contains("MissionConfirm") || scene == "SuitUp")
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

	private void UpdateWeaponInfoInShowWindow()
	{
		Transform transform = m_tui.transform.Find("TUIControl").Find("ShowWindow");
		WeaponInfo weaponInfoByName = DataCenter.Conf().GetWeaponInfoByName(m_equipmentName);
		TUIMeshText component = transform.Find("Name").GetComponent<TUIMeshText>();
		component.text = weaponInfoByName.textName;
		component.UpdateMesh();
		TUIMeshSprite component2 = transform.Find("Icon").GetComponent<TUIMeshSprite>();
		component2.frameName = weaponInfoByName.icon;
		component2.UpdateMesh();
		TUIMeshSprite component3 = transform.Find("IconGary").GetComponent<TUIMeshSprite>();
		component3.frameName = weaponInfoByName.icon;
		component3.UpdateMesh();
		TUIMeshText component4 = transform.Find("Details").GetComponent<TUIMeshText>();
		component4.text = weaponInfoByName.desc;
		component4.UpdateMesh();
		TUIMeshSprite component5 = transform.transform.Find("Price").GetComponent<TUIMeshSprite>();
		TUIMeshText component6 = transform.transform.Find("PriceText").GetComponent<TUIMeshText>();
		if (weaponInfoByName.buyMoney == -1)
		{
			component5.frameName = "StatusBarCrystalImg";
			component5.UpdateMesh();
			component6.text = weaponInfoByName.buyCrystal.ToString("###,###");
			component6.UpdateMesh();
		}
		else
		{
			component5.frameName = "StatusBarMoneyImg";
			component5.UpdateMesh();
			component6.text = weaponInfoByName.buyMoney.ToString("###,###");
			component6.UpdateMesh();
		}
		transform.Find("ItemNum").gameObject.SetActiveRecursively(false);
		transform.Find("ItemText").gameObject.SetActiveRecursively(false);
		transform.Find("RightBtn").Find("Buy").transform.localPosition = new Vector3(0f, 10000f, 0f);
		TUIMeshText component7 = transform.transform.Find("UnlockLevelText").GetComponent<TUIMeshText>();
		transform.Find("YourOwnTitle").gameObject.SetActiveRecursively(false);
		if (weaponInfoByName.unlockLevel > DataCenter.Save().GetLv())
		{
			component2.gameObject.SetActiveRecursively(false);
			component3.gameObject.SetActiveRecursively(true);
			transform.Find("LockBk").gameObject.SetActiveRecursively(true);
			component2.UpdateMesh();
			transform.Find("Owned").gameObject.SetActiveRecursively(false);
			component7.gameObject.SetActiveRecursively(true);
			component7.text = "UnlockLevel: " + weaponInfoByName.unlockLevel;
			component7.UpdateMesh();
			return;
		}
		component2.gameObject.SetActiveRecursively(true);
		component3.gameObject.SetActiveRecursively(false);
		transform.Find("LockBk").gameObject.SetActiveRecursively(false);
		component2.UpdateMesh();
		component7.gameObject.SetActiveRecursively(false);
		if (DataCenter.Save().IsWeaponUnlock(weaponInfoByName.name))
		{
			transform.Find("Owned").gameObject.SetActiveRecursively(true);
			return;
		}
		transform.Find("Owned").gameObject.SetActiveRecursively(false);
		transform.Find("RightBtn").Find("Buy").transform.localPosition = Vector3.zero;
	}

	private void UpdateArmorInfoInShowWindow()
	{
		Transform transform = m_tui.transform.Find("TUIControl").Find("ShowWindow");
		ArmorInfo armorInfoByName = DataCenter.Conf().GetArmorInfoByName(m_equipmentName);
		TUIMeshText component = transform.Find("Name").GetComponent<TUIMeshText>();
		component.text = armorInfoByName.textName;
		component.UpdateMesh();
		TUIMeshSprite component2 = transform.Find("Icon").GetComponent<TUIMeshSprite>();
		component2.frameName = armorInfoByName.icon;
		component2.UpdateMesh();
		TUIMeshSprite component3 = transform.Find("IconGary").GetComponent<TUIMeshSprite>();
		component3.frameName = armorInfoByName.icon;
		component3.UpdateMesh();
		TUIMeshText component4 = transform.Find("Details").GetComponent<TUIMeshText>();
		component4.text = armorInfoByName.desc;
		component4.UpdateMesh();
		TUIMeshSprite component5 = transform.transform.Find("Price").GetComponent<TUIMeshSprite>();
		TUIMeshText component6 = transform.transform.Find("PriceText").GetComponent<TUIMeshText>();
		if (armorInfoByName.buyMoney == -1)
		{
			component5.frameName = "StatusBarCrystalImg";
			component5.UpdateMesh();
			component6.text = armorInfoByName.buyCrystal.ToString("###,###");
			component6.UpdateMesh();
		}
		else
		{
			component5.frameName = "StatusBarMoneyImg";
			component5.UpdateMesh();
			component6.text = armorInfoByName.buyMoney.ToString("###,###");
			component6.UpdateMesh();
		}
		transform.Find("ItemNum").gameObject.SetActiveRecursively(false);
		transform.Find("ItemText").gameObject.SetActiveRecursively(false);
		transform.Find("RightBtn").Find("Buy").transform.localPosition = new Vector3(0f, 10000f, 0f);
		TUIMeshText component7 = transform.transform.Find("UnlockLevelText").GetComponent<TUIMeshText>();
		transform.Find("YourOwnTitle").gameObject.SetActiveRecursively(false);
		if (armorInfoByName.unlockLevel > DataCenter.Save().GetLv())
		{
			component2.gameObject.SetActiveRecursively(false);
			component3.gameObject.SetActiveRecursively(true);
			transform.Find("LockBk").gameObject.SetActiveRecursively(true);
			component2.UpdateMesh();
			transform.Find("Owned").gameObject.SetActiveRecursively(false);
			component7.gameObject.SetActiveRecursively(true);
			component7.text = "UnlockLevel: " + armorInfoByName.unlockLevel;
			component7.UpdateMesh();
			return;
		}
		component2.gameObject.SetActiveRecursively(true);
		component3.gameObject.SetActiveRecursively(false);
		transform.Find("LockBk").gameObject.SetActiveRecursively(false);
		component2.UpdateMesh();
		component7.gameObject.SetActiveRecursively(false);
		if (DataCenter.Save().IsArmorUnlock(armorInfoByName.name))
		{
			transform.Find("Owned").gameObject.SetActiveRecursively(true);
			return;
		}
		transform.Find("Owned").gameObject.SetActiveRecursively(false);
		transform.Find("RightBtn").Find("Buy").transform.localPosition = Vector3.zero;
	}

	private void UpdateItemInfoInShowWindow()
	{
		Transform transform = m_tui.transform.Find("TUIControl").Find("ShowWindow");
		ItemInfo itemInfoByName = DataCenter.Conf().GetItemInfoByName(m_equipmentName);
		TUIMeshText component = transform.Find("Name").GetComponent<TUIMeshText>();
		component.text = itemInfoByName.textName;
		component.UpdateMesh();
		TUIMeshSprite component2 = transform.Find("Icon").GetComponent<TUIMeshSprite>();
		component2.frameName = itemInfoByName.icon;
		component2.UpdateMesh();
		TUIMeshSprite component3 = transform.Find("IconGary").GetComponent<TUIMeshSprite>();
		component3.frameName = itemInfoByName.icon;
		component3.UpdateMesh();
		TUIMeshText component4 = transform.Find("Details").GetComponent<TUIMeshText>();
		component4.text = itemInfoByName.desc;
		component4.UpdateMesh();
		TUIMeshSprite component5 = transform.transform.Find("Price").GetComponent<TUIMeshSprite>();
		TUIMeshText component6 = transform.transform.Find("PriceText").GetComponent<TUIMeshText>();
		if (itemInfoByName.buyMoney == -1)
		{
			component5.frameName = "StatusBarCrystalImg";
			component5.UpdateMesh();
			component6.text = itemInfoByName.buyCrystal.ToString("###,###");
			component6.UpdateMesh();
		}
		else
		{
			component5.frameName = "StatusBarMoneyImg";
			component5.UpdateMesh();
			component6.text = itemInfoByName.buyMoney.ToString("###,###");
			component6.UpdateMesh();
		}
		TUIMeshText component7 = transform.Find("ItemNum").GetComponent<TUIMeshText>();
		component7.text = "x" + DataCenter.Conf().GetItemInfoByName(m_equipmentName).buyAmount;
		component7.UpdateMesh();
		component7.transform.gameObject.SetActiveRecursively(true);
		transform.Find("RightBtn").Find("Buy").transform.localPosition = new Vector3(0f, 10000f, 0f);
		TUIMeshText component8 = transform.Find("ItemText").GetComponent<TUIMeshText>();
		TUIMeshText component9 = transform.transform.Find("UnlockLevelText").GetComponent<TUIMeshText>();
		transform.Find("Owned").gameObject.SetActiveRecursively(false);
		transform.Find("LockBk").gameObject.SetActiveRecursively(true);
		if (itemInfoByName.unlockLevel > DataCenter.Save().GetLv())
		{
			component2.gameObject.SetActiveRecursively(false);
			component3.gameObject.SetActiveRecursively(true);
			transform.Find("YourOwnTitle").gameObject.SetActiveRecursively(false);
			component2.UpdateMesh();
			component9.gameObject.SetActiveRecursively(true);
			component9.text = "UnlockLevel: " + itemInfoByName.unlockLevel;
			component9.UpdateMesh();
			component8.gameObject.SetActiveRecursively(false);
			return;
		}
		component2.gameObject.SetActiveRecursively(true);
		component3.gameObject.SetActiveRecursively(false);
		transform.Find("YourOwnTitle").gameObject.SetActiveRecursively(true);
		component2.UpdateMesh();
		component9.gameObject.SetActiveRecursively(false);
		if (DataCenter.Save().IsItemUnlock(itemInfoByName.name))
		{
			component8.text = DataCenter.Save().GetItemNums(m_equipmentName) + "->" + (DataCenter.Save().GetItemNums(m_equipmentName) + DataCenter.Conf().GetItemInfoByName(m_equipmentName).buyAmount);
			component8.gameObject.SetActiveRecursively(true);
			transform.Find("RightBtn").Find("Buy").transform.localPosition = Vector3.zero;
		}
		else
		{
			component8.text = "0 ->" + DataCenter.Conf().GetItemInfoByName(m_equipmentName).buyAmount;
			component8.gameObject.SetActiveRecursively(true);
			transform.Find("RightBtn").Find("Buy").transform.localPosition = Vector3.zero;
		}
		component8.UpdateMesh();
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

	private void RefreshCurrentEquipmentAndCharactor()
	{
		TUIButtonSelectGroupEx component = m_tui.transform.Find("TUIControl").Find("CharactorsNavbar").GetComponent<TUIButtonSelectGroupEx>();
		m_charactorIndex = int.Parse(component.tabInfo[component.current].panel.name.Replace("PanelCharactor", string.Empty));
		TUIButtonSelectGroupEx component2 = m_tui.transform.Find("TUIControl").Find(component.tabInfo[component.current].panel.name).Find("ClassifyType")
			.GetComponent<TUIButtonSelectGroupEx>();
		TUIButtonSelectGroupEx2 component3 = m_tui.transform.Find("TUIControl").Find(component.tabInfo[component.current].panel.name).Find(component2.tabInfo[component2.current].panel.name)
			.Find("ScrollObject")
			.GetComponent<TUIButtonSelectGroupEx2>();
		m_equipmentName = component3.GetCurrentButton().transform.parent.name;
		m_currentBtn = component3.GetCurrentButton().transform.parent;
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

	private void RefreshCharacterLocked()
	{
		if (!DataCenter.Save().IsCharacterUnlock(2))
		{
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor02Locked").gameObject.SetActiveRecursively(true);
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor02Locked").GetComponent<TUIButtonClick>().Reset();
		}
		else
		{
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor02Locked").gameObject.SetActiveRecursively(false);
		}
		if (!DataCenter.Save().IsCharacterUnlock(3))
		{
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor03Locked").gameObject.SetActiveRecursively(true);
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor03Locked").GetComponent<TUIButtonClick>().Reset();
		}
		else
		{
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor03Locked").gameObject.SetActiveRecursively(false);
		}
		if (!DataCenter.Save().IsCharacterUnlock(4))
		{
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor04Locked").gameObject.SetActiveRecursively(true);
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor04Locked").GetComponent<TUIButtonClick>().Reset();
		}
		else
		{
			m_tui.transform.Find("TUIControl/CharactorsNavbar/Charactor04Locked").gameObject.SetActiveRecursively(false);
		}
	}
}

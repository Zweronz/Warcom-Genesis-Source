using UnityEngine;

public class UILevelWeb : MonoBehaviour, TUIHandler
{
	public GameObject m_block;

	public GameObject m_pauseBlock;

	public GameObject m_sniperMode;

	public GameObject m_missionStart;

	public GameObject m_tutorial;

	private TUI m_tui;

	private GameObject m_weaponObject;

	private bool m_snipe;

	private float m_sensitivety = 0.4f;

	private float m_sensitivety_Yfacter = 0.667f;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		m_weaponObject = m_tui.transform.Find("TUIControl/UI/NormalMode/Weapons/Weapons").gameObject;
		m_missionStart.GetComponent<Animation>().Play("mission start");
		m_missionStart.GetComponent<Animation>()["mission start"].wrapMode = WrapMode.Once;
		if (DataCenter.StateMulti().GetCurrentCharactor() != 2)
		{
			m_tui.transform.Find("TUIControl/UI/Snipe").gameObject.SetActiveRecursively(false);
		}
		m_tui.transform.Find("TUIControl/UI/ChangeWeapon").localPosition = new Vector3(242f, 135f, m_tui.transform.Find("TUIControl/UI/ChangeWeapon").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/HP").localPosition = new Vector3(-196f, 137f, m_tui.transform.Find("TUIControl/UI/HP").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/HP/bloodstain").localPosition = new Vector3(196f, -137f, m_tui.transform.Find("TUIControl/UI/HP/bloodstain").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/HP/bloodstain").localScale = new Vector3(1f, 1.184f, 1f);
		m_tui.transform.Find("TUIControl/UI/JoystickA").localPosition = new Vector3(-228f, -100f, m_tui.transform.Find("TUIControl/UI/JoystickA").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/JoystickB").localPosition = new Vector3(198f, -100f, m_tui.transform.Find("TUIControl/UI/JoystickB").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/NormalMode/Items").localPosition = new Vector3(0f, -135f, m_tui.transform.Find("TUIControl/UI/NormalMode/Items").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/NormalMode/Map").localPosition = new Vector3(-233f, 70f, m_tui.transform.Find("TUIControl/UI/NormalMode/Map").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/NormalMode/Weapons").localPosition = new Vector3(44f, 0f, m_tui.transform.Find("TUIControl/UI/NormalMode/Weapons").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/Snipe").localPosition = new Vector3(260f, -21.5f, m_tui.transform.Find("TUIControl/UI/Snipe").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/Time").localPosition = new Vector3(42f, 146f, m_tui.transform.Find("TUIControl/UI/Time").localPosition.z);
		m_tui.transform.Find("TUIControl/UI/Touchpad").GetComponent<TUIMove>().size = new Vector2(568f, 320f);
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
		if (GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.DeadMode && m_snipe)
		{
			m_snipe = false;
			Player player = GameManager.Instance().GetLevel().GetPlayer();
			if (player != null)
			{
				WeaponInfo weaponInfoByName = DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName());
				if (weaponInfoByName.type != WeaponType.SniperRifle)
				{
					float zoomChangeTime = DataCenter.Conf().GetCameraParam(weaponInfoByName.camera).zoomChangeTime;
					GameManager.Instance().GetCamera().ZoomOut(zoomChangeTime);
				}
				else
				{
					float zoomChangeTime2 = DataCenter.Conf().GetCameraParam(weaponInfoByName.camera).zoomChangeTime;
					GameManager.Instance().GetCamera().SnipeOutCrosshair();
					GameManager.Instance().GetCamera().ZoomOut(zoomChangeTime2);
					UIOutSnipeMode();
				}
			}
		}
		if (GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.NormalMode)
		{
			TAudioController component = DataCenter.StateCommon().audio.GetComponent<TAudioController>();
			if (DataCenter.StateCommon().bgmName != "music_" + DataCenter.StateMulti().GetCurrentScene())
			{
				component.PlayAudio("music_" + DataCenter.StateMulti().GetCurrentScene());
				DataCenter.StateCommon().bgmName = "music_" + DataCenter.StateMulti().GetCurrentScene();
			}
		}
		if (GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.StartMode || GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.EndMode)
		{
			Player player2 = GameManager.Instance().GetLevel().GetPlayer();
			if (player2 != null)
			{
				if (m_snipe)
				{
					m_snipe = false;
					WeaponInfo weaponInfoByName2 = DataCenter.Conf().GetWeaponInfoByName(player2.GetWeapon().GetName());
					if (weaponInfoByName2.type != WeaponType.SniperRifle)
					{
						float zoomChangeTime3 = DataCenter.Conf().GetCameraParam(weaponInfoByName2.camera).zoomChangeTime;
						GameManager.Instance().GetCamera().ZoomOut(zoomChangeTime3);
					}
					else
					{
						float zoomChangeTime4 = DataCenter.Conf().GetCameraParam(weaponInfoByName2.camera).zoomChangeTime;
						GameManager.Instance().GetCamera().SnipeOutCrosshair();
						GameManager.Instance().GetCamera().ZoomOut(zoomChangeTime4);
						UIOutSnipeMode();
					}
				}
				player2.SetMove(false, true, Vector2.zero, 1f);
				player2.SetFire(false, false);
			}
			UINotShow();
		}
		else
		{
			UIShow();
		}
		if (GameManager.Instance().GetCamera().m_cameraMode != GameCamera.CameraMode.NormalMode)
		{
			return;
		}
		Player player3 = GameManager.Instance().GetLevel().GetPlayer();
		int num = 0;
		int num2 = 0;
		num = ((Input.GetAxis("Horizontal") < 0f) ? (-1) : ((Input.GetAxis("Horizontal") > 0f) ? 1 : 0));
		num2 = ((Input.GetAxis("Vertical") < 0f) ? (-1) : ((Input.GetAxis("Vertical") > 0f) ? 1 : 0));
		if (num == 0 && num2 == 0)
		{
			player3.SetMove(false, false, Vector2.zero, 1f);
		}
		else
		{
			player3.SetMove(true, true, new Vector2(num, num2).normalized, 0.8f);
		}
		float zoomFieldOfView = GameManager.Instance().GetCamera().GetZoomFieldOfView();
		float num3 = (float)DataCenter.Save().GetOptionJoystick() * m_sensitivety * 0.06f * zoomFieldOfView + 3f;
		player3.TurnAround(Input.GetAxis("Mouse X") * num3, Input.GetAxis("Mouse Y") * num3 * m_sensitivety_Yfacter);
		if (Input.GetButtonDown("Fire1"))
		{
			player3.SetFire(true, true);
		}
		if (Input.GetButtonUp("Fire1"))
		{
			player3.SetFire(false, false);
		}
		if (Input.GetKeyDown(KeyCode.Q) && !player3.m_throwing && !player3.m_reloading && !player3.m_dead)
		{
			m_weaponObject.GetComponent<Animation>().Play("WeaponChangeAnim");
		}
		if (Input.GetKeyDown(KeyCode.E) && !player3.m_throwing && !player3.m_reloading && !player3.m_dead)
		{
			m_weaponObject.GetComponent<Animation>().Play("WeaponChangeAnim2");
		}
		if (Input.GetKeyDown(KeyCode.R) && !player3.m_dead && !player3.m_throwing && !player3.m_changing && player3.GetWeapon().GetWeaponType() != 0)
		{
			player3.OpenReload();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SwitchScene("WorldMap");
			Cursor.visible = true;
			Screen.lockCursor = false;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Back");
			UtilsNet.SendLeave();
		}
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			DataCenter.Save().SetOptionJoystick(Mathf.Min(DataCenter.Save().GetOptionJoystick() + 1, 100));
			DataCenter.Save().Save();
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			DataCenter.Save().SetOptionJoystick(Mathf.Max(DataCenter.Save().GetOptionJoystick() - 1, 1));
			DataCenter.Save().Save();
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			if (!player3.m_dead)
			{
				int buyAmmoPrice = DataCenter.Conf().GetWeaponInfoByName(player3.GetWeapon().GetName()).buyAmmoPrice;
				player3.GetWeapon().AddAmmo(DataCenter.Conf().GetWeaponInfoByName(player3.GetWeapon().GetName()).ammoAmount);
				TAnalyticsManager.AttributeAddInteger("BuyAmmoMoney", buyAmmoPrice);
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_GameButton_BuyBullets");
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateMulti().GetCurrentCharactor());
			if (characterEquipInfo.item01 == string.Empty)
			{
				return;
			}
			ItemInfo itemInfoByName = DataCenter.Conf().GetItemInfoByName(characterEquipInfo.item01);
			if (itemInfoByName.type != ItemEffectType.Resurrection)
			{
				if (!player3.m_dead)
				{
					if (itemInfoByName.type == ItemEffectType.Grenade && (player3.m_changing || player3.m_reloading || player3.m_throwing))
					{
						return;
					}
					int itemNums = DataCenter.Save().GetItemNums(characterEquipInfo.item01);
					if (itemNums >= 1)
					{
						DataCenter.Save().SetItemNums(characterEquipInfo.item01, itemNums - 1);
						TUIMeshText component2 = GameObject.Find("Item01Text").GetComponent<TUIMeshText>();
						component2.text = (itemNums - 1).ToString();
						component2.UpdateMesh();
						player3.UseItems(itemInfoByName);
						if (itemNums - 1 == 0)
						{
							GameObject.Find("ItemIcon01").SetActiveRecursively(false);
							DataCenter.Save().UnequipItemAll(characterEquipInfo.item01);
							GameObject.Find("Item01").SetActiveRecursively(false);
							component2.gameObject.SetActiveRecursively(false);
						}
						DataCenter.Save().Save();
					}
				}
			}
			else if (player3.m_dead)
			{
				int itemNums2 = DataCenter.Save().GetItemNums(characterEquipInfo.item01);
				if (itemNums2 >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo.item01, itemNums2 - 1);
					TUIMeshText component3 = GameObject.Find("Item01Text").GetComponent<TUIMeshText>();
					component3.text = (itemNums2 - 1).ToString();
					component3.UpdateMesh();
					player3.UseItems(itemInfoByName);
					if (itemNums2 - 1 == 0)
					{
						GameObject.Find("ItemIcon01").gameObject.SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo.item01);
						GameObject.Find("Item01").gameObject.SetActiveRecursively(false);
						component3.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			CharacterEquipInfo characterEquipInfo2 = DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateMulti().GetCurrentCharactor());
			if (characterEquipInfo2.item02 == string.Empty)
			{
				return;
			}
			ItemInfo itemInfoByName2 = DataCenter.Conf().GetItemInfoByName(characterEquipInfo2.item02);
			if (itemInfoByName2.type != ItemEffectType.Resurrection)
			{
				if (!player3.m_dead)
				{
					if (itemInfoByName2.type == ItemEffectType.Grenade && (player3.m_changing || player3.m_reloading || player3.m_throwing))
					{
						return;
					}
					int itemNums3 = DataCenter.Save().GetItemNums(characterEquipInfo2.item02);
					if (itemNums3 >= 1)
					{
						DataCenter.Save().SetItemNums(characterEquipInfo2.item02, itemNums3 - 1);
						TUIMeshText component4 = GameObject.Find("Item02Text").GetComponent<TUIMeshText>();
						component4.text = (itemNums3 - 1).ToString();
						component4.UpdateMesh();
						player3.UseItems(itemInfoByName2);
						if (itemNums3 - 1 == 0)
						{
							GameObject.Find("ItemIcon02").SetActiveRecursively(false);
							DataCenter.Save().UnequipItemAll(characterEquipInfo2.item02);
							GameObject.Find("Item02").SetActiveRecursively(false);
							component4.gameObject.SetActiveRecursively(false);
						}
						DataCenter.Save().Save();
					}
				}
			}
			else if (player3.m_dead)
			{
				int itemNums4 = DataCenter.Save().GetItemNums(characterEquipInfo2.item02);
				if (itemNums4 >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo2.item02, itemNums4 - 1);
					TUIMeshText component5 = GameObject.Find("Item02Text").GetComponent<TUIMeshText>();
					component5.text = (itemNums4 - 1).ToString();
					component5.UpdateMesh();
					player3.UseItems(itemInfoByName2);
					if (itemNums4 - 1 == 0)
					{
						GameObject.Find("ItemIcon02").SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo2.item02);
						GameObject.Find("Item02").SetActiveRecursively(false);
						component5.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			CharacterEquipInfo characterEquipInfo3 = DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateMulti().GetCurrentCharactor());
			if (characterEquipInfo3.item03 == string.Empty)
			{
				return;
			}
			ItemInfo itemInfoByName3 = DataCenter.Conf().GetItemInfoByName(characterEquipInfo3.item03);
			if (itemInfoByName3.type != ItemEffectType.Resurrection)
			{
				if (!player3.m_dead)
				{
					if (itemInfoByName3.type == ItemEffectType.Grenade && (player3.m_changing || player3.m_reloading || player3.m_throwing))
					{
						return;
					}
					int itemNums5 = DataCenter.Save().GetItemNums(characterEquipInfo3.item03);
					if (itemNums5 >= 1)
					{
						DataCenter.Save().SetItemNums(characterEquipInfo3.item03, itemNums5 - 1);
						TUIMeshText component6 = GameObject.Find("Item03Text").GetComponent<TUIMeshText>();
						component6.text = (itemNums5 - 1).ToString();
						component6.UpdateMesh();
						player3.UseItems(itemInfoByName3);
						if (itemNums5 - 1 == 0)
						{
							GameObject.Find("ItemIcon03").SetActiveRecursively(false);
							DataCenter.Save().UnequipItemAll(characterEquipInfo3.item03);
							GameObject.Find("Item03").SetActiveRecursively(false);
							component6.gameObject.SetActiveRecursively(false);
						}
						DataCenter.Save().Save();
					}
				}
			}
			else if (player3.m_dead)
			{
				int itemNums6 = DataCenter.Save().GetItemNums(characterEquipInfo3.item03);
				if (itemNums6 >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo3.item03, itemNums6 - 1);
					TUIMeshText component7 = GameObject.Find("Item03Text").GetComponent<TUIMeshText>();
					component7.text = (itemNums6 - 1).ToString();
					component7.UpdateMesh();
					player3.UseItems(itemInfoByName3);
					if (itemNums6 - 1 == 0)
					{
						GameObject.Find("ItemIcon03").SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo3.item03);
						GameObject.Find("Item03").SetActiveRecursively(false);
						component7.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
		}
		if (!Input.GetButtonDown("Fire2") || player3.m_dead)
		{
			return;
		}
		if (!m_snipe)
		{
			m_snipe = true;
			if (player3.GetWeapon().GetWeaponType() == WeaponType.SniperRifle)
			{
				m_tui.transform.Find("TUIControl/UI/JoystickA").gameObject.GetComponent<GUIJoystick>().Reset();
				WeaponInfo weaponInfoByName3 = DataCenter.Conf().GetWeaponInfoByName(player3.GetWeapon().GetName());
				float angle = DataCenter.Conf().GetCameraParam(weaponInfoByName3.camera).angle;
				float distance = DataCenter.Conf().GetCameraParam(weaponInfoByName3.camera).distance;
				float zoomChangeTime5 = DataCenter.Conf().GetCameraParam(weaponInfoByName3.camera).zoomChangeTime;
				GameManager.Instance().GetCamera().ZoomIn(angle, distance, zoomChangeTime5);
				GameManager.Instance().GetCamera().SnipeInCrosshair();
				UIInSnipeMode();
			}
		}
		else
		{
			m_snipe = false;
			if (player3.GetWeapon().GetWeaponType() == WeaponType.SniperRifle)
			{
				m_tui.transform.Find("TUIControl/UI/JoystickA").gameObject.GetComponent<GUIJoystick>().Reset();
				WeaponInfo weaponInfoByName4 = DataCenter.Conf().GetWeaponInfoByName(player3.GetWeapon().GetName());
				float zoomChangeTime6 = DataCenter.Conf().GetCameraParam(weaponInfoByName4.camera).zoomChangeTime;
				GameManager.Instance().GetCamera().SnipeOutCrosshair();
				GameManager.Instance().GetCamera().ZoomOut(zoomChangeTime6);
				UIOutSnipeMode();
			}
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player == null || GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.StartMode || GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.EndMode)
		{
			return;
		}
		if (m_tutorial.active)
		{
			m_tutorial.SetActiveRecursively(false);
		}
		if (control.name == "JoystickA")
		{
			if (!player.m_dead)
			{
				switch (eventType)
				{
				case 1:
					player.SetMove(false, true, Vector2.zero, 1f);
					break;
				case 2:
					player.SetMove(true, true, new Vector2(wparam, lparam).normalized, Mathf.Clamp(Mathf.Sqrt(wparam * wparam + lparam * lparam), 0.5f, 0.8f));
					break;
				case 3:
					player.SetMove(false, true, Vector2.zero, 1f);
					break;
				}
			}
		}
		else if (control.name == "JoystickB")
		{
			if (!player.m_dead)
			{
				switch (eventType)
				{
				case 1:
					player.SetFire(true, true);
					break;
				case 2:
				{
					float zoomFieldOfView = GameManager.Instance().GetCamera().GetZoomFieldOfView();
					float num = (float)DataCenter.Save().GetOptionJoystick() * m_sensitivety * 0.003f * zoomFieldOfView + 0.2f;
					player.TurnAround(wparam * num, lparam * num * m_sensitivety_Yfacter);
					player.SetFire(true, true);
					break;
				}
				case 3:
					player.SetFire(false, false);
					break;
				}
			}
		}
		else if (control.name == "Touchpad")
		{
			if (!player.m_dead)
			{
				switch (eventType)
				{
				case 2:
				{
					float zoomFieldOfView2 = GameManager.Instance().GetCamera().GetZoomFieldOfView();
					float num2 = (float)DataCenter.Save().GetOptionJoystick() * m_sensitivety * 0.006f * zoomFieldOfView2 + 0.3f;
					player.TurnAround(wparam * num2, lparam * num2 * m_sensitivety_Yfacter);
					break;
				}
				}
			}
		}
		else if (control.name == "Item01" && eventType == 3)
		{
			CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateMulti().GetCurrentCharactor());
			ItemInfo itemInfoByName = DataCenter.Conf().GetItemInfoByName(characterEquipInfo.item01);
			if (itemInfoByName.type != ItemEffectType.Resurrection)
			{
				if (player.m_dead || (itemInfoByName.type == ItemEffectType.Grenade && (player.m_changing || player.m_reloading || player.m_throwing)))
				{
					return;
				}
				int itemNums = DataCenter.Save().GetItemNums(characterEquipInfo.item01);
				if (itemNums >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo.item01, itemNums - 1);
					TUIMeshText component = control.transform.parent.Find("Item01Text").GetComponent<TUIMeshText>();
					component.text = (itemNums - 1).ToString();
					component.UpdateMesh();
					player.UseItems(itemInfoByName);
					if (itemNums - 1 == 0)
					{
						control.transform.parent.Find("ItemIcon01").gameObject.SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo.item01);
						control.gameObject.SetActiveRecursively(false);
						component.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
			else
			{
				if (!player.m_dead)
				{
					return;
				}
				int itemNums2 = DataCenter.Save().GetItemNums(characterEquipInfo.item01);
				if (itemNums2 >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo.item01, itemNums2 - 1);
					TUIMeshText component2 = control.transform.parent.Find("Item01Text").GetComponent<TUIMeshText>();
					component2.text = (itemNums2 - 1).ToString();
					component2.UpdateMesh();
					player.UseItems(itemInfoByName);
					if (itemNums2 - 1 == 0)
					{
						control.transform.parent.Find("ItemIcon01").gameObject.SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo.item01);
						control.gameObject.SetActiveRecursively(false);
						component2.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
		}
		else if (control.name == "Item02" && eventType == 3)
		{
			CharacterEquipInfo characterEquipInfo2 = DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateMulti().GetCurrentCharactor());
			ItemInfo itemInfoByName2 = DataCenter.Conf().GetItemInfoByName(characterEquipInfo2.item02);
			if (itemInfoByName2.type != ItemEffectType.Resurrection)
			{
				if (player.m_dead || (itemInfoByName2.type == ItemEffectType.Grenade && (player.m_changing || player.m_reloading || player.m_throwing)))
				{
					return;
				}
				int itemNums3 = DataCenter.Save().GetItemNums(characterEquipInfo2.item02);
				if (itemNums3 >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo2.item02, itemNums3 - 1);
					TUIMeshText component3 = control.transform.parent.Find("Item02Text").GetComponent<TUIMeshText>();
					component3.text = (itemNums3 - 1).ToString();
					component3.UpdateMesh();
					player.UseItems(itemInfoByName2);
					if (itemNums3 - 1 == 0)
					{
						control.transform.parent.Find("ItemIcon02").gameObject.SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo2.item02);
						control.gameObject.SetActiveRecursively(false);
						component3.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
			else
			{
				if (!player.m_dead)
				{
					return;
				}
				int itemNums4 = DataCenter.Save().GetItemNums(characterEquipInfo2.item02);
				if (itemNums4 >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo2.item02, itemNums4 - 1);
					TUIMeshText component4 = control.transform.parent.Find("Item02Text").GetComponent<TUIMeshText>();
					component4.text = (itemNums4 - 1).ToString();
					component4.UpdateMesh();
					player.UseItems(itemInfoByName2);
					if (itemNums4 - 1 == 0)
					{
						control.transform.parent.Find("ItemIcon02").gameObject.SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo2.item02);
						control.gameObject.SetActiveRecursively(false);
						component4.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
		}
		else if (control.name == "Item03" && eventType == 3)
		{
			CharacterEquipInfo characterEquipInfo3 = DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateMulti().GetCurrentCharactor());
			ItemInfo itemInfoByName3 = DataCenter.Conf().GetItemInfoByName(characterEquipInfo3.item03);
			if (itemInfoByName3.type != ItemEffectType.Resurrection)
			{
				if (player.m_dead || (itemInfoByName3.type == ItemEffectType.Grenade && (player.m_changing || player.m_reloading || player.m_throwing)))
				{
					return;
				}
				int itemNums5 = DataCenter.Save().GetItemNums(characterEquipInfo3.item03);
				if (itemNums5 >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo3.item03, itemNums5 - 1);
					TUIMeshText component5 = control.transform.parent.Find("Item03Text").GetComponent<TUIMeshText>();
					component5.text = (itemNums5 - 1).ToString();
					component5.UpdateMesh();
					player.UseItems(itemInfoByName3);
					if (itemNums5 - 1 == 0)
					{
						control.transform.parent.Find("ItemIcon03").gameObject.SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo3.item03);
						control.gameObject.SetActiveRecursively(false);
						component5.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
			else
			{
				if (!player.m_dead)
				{
					return;
				}
				int itemNums6 = DataCenter.Save().GetItemNums(characterEquipInfo3.item03);
				if (itemNums6 >= 1)
				{
					DataCenter.Save().SetItemNums(characterEquipInfo3.item03, itemNums6 - 1);
					TUIMeshText component6 = control.transform.parent.Find("Item03Text").GetComponent<TUIMeshText>();
					component6.text = (itemNums6 - 1).ToString();
					component6.UpdateMesh();
					player.UseItems(itemInfoByName3);
					if (itemNums6 - 1 == 0)
					{
						control.transform.parent.Find("ItemIcon03").gameObject.SetActiveRecursively(false);
						DataCenter.Save().UnequipItemAll(characterEquipInfo3.item03);
						control.gameObject.SetActiveRecursively(false);
						component6.gameObject.SetActiveRecursively(false);
					}
					DataCenter.Save().Save();
				}
			}
		}
		else if (control.name == "Reload" && eventType == 3)
		{
			if (!player.m_dead && !player.m_throwing && !player.m_changing && player.GetWeapon().GetWeaponType() != 0)
			{
				player.OpenReload();
			}
		}
		else if (control.name == "BuyBullet" && eventType == 3)
		{
			if (!player.m_dead)
			{
				int buyAmmoPrice = DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName()).buyAmmoPrice;
				player.GetWeapon().AddAmmo(DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName()).ammoAmount);
				TAnalyticsManager.AttributeAddInteger("BuyAmmoMoney", buyAmmoPrice);
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_GameButton_BuyBullets");
		}
		else if (control.name == "Pause" && eventType == 3)
		{
			m_pauseBlock.SetActiveRecursively(true);
			ShowPauseBroad();
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Quite" && eventType == 3)
		{
			UtilsNet.SendLeave();
			m_block.SetActiveRecursively(true);
			SwitchScene("WorldMap");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Back");
		}
		else if (control.name == "Resume" && eventType == 3)
		{
			m_pauseBlock.SetActiveRecursively(false);
			HidePauseBroad();
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "ButtonMusic" && eventType == 1)
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
		else if (control.name == "ButtonSound" && eventType == 1)
		{
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
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
		else if (control.name == "ButtonJoystick" && eventType == 1)
		{
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "ButtonJoystick" && eventType == 3)
		{
			DataCenter.Save().SetOptionJoystick((int)(100f * wparam));
		}
		else if (control.name == "ButtonJoystick" && eventType == 2)
		{
			DataCenter.Save().Save();
		}
		else if (control.name == "ChangeWeapon")
		{
			if (player.m_throwing || player.m_reloading || player.m_dead)
			{
				return;
			}
			switch (eventType)
			{
			case 2:
				if (wparam < -3f)
				{
					m_weaponObject.GetComponent<Animation>().Play("WeaponChangeAnim");
				}
				else if (wparam > 3f)
				{
					m_weaponObject.GetComponent<Animation>().Play("WeaponChangeAnim2");
				}
				break;
			}
		}
		else
		{
			if (!(control.name == "Snipe") || eventType != 3 || player.m_dead)
			{
				return;
			}
			if (!m_snipe)
			{
				m_snipe = true;
				if (player.GetWeapon().GetWeaponType() != WeaponType.SniperRifle)
				{
					WeaponInfo weaponInfoByName = DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName());
					float angle = DataCenter.Conf().GetCameraParam(weaponInfoByName.camera).angle;
					float distance = DataCenter.Conf().GetCameraParam(weaponInfoByName.camera).distance;
					float zoomChangeTime = DataCenter.Conf().GetCameraParam(weaponInfoByName.camera).zoomChangeTime;
					GameManager.Instance().GetCamera().ZoomIn(angle, distance, zoomChangeTime);
				}
				else
				{
					m_tui.transform.Find("TUIControl/UI/JoystickA").gameObject.GetComponent<GUIJoystick>().Reset();
					WeaponInfo weaponInfoByName2 = DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName());
					float angle2 = DataCenter.Conf().GetCameraParam(weaponInfoByName2.camera).angle;
					float distance2 = DataCenter.Conf().GetCameraParam(weaponInfoByName2.camera).distance;
					float zoomChangeTime2 = DataCenter.Conf().GetCameraParam(weaponInfoByName2.camera).zoomChangeTime;
					GameManager.Instance().GetCamera().ZoomIn(angle2, distance2, zoomChangeTime2);
					GameManager.Instance().GetCamera().SnipeInCrosshair();
					UIInSnipeMode();
				}
			}
			else
			{
				m_snipe = false;
				if (player.GetWeapon().GetWeaponType() != WeaponType.SniperRifle)
				{
					WeaponInfo weaponInfoByName3 = DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName());
					float zoomChangeTime3 = DataCenter.Conf().GetCameraParam(weaponInfoByName3.camera).zoomChangeTime;
					GameManager.Instance().GetCamera().ZoomOut(zoomChangeTime3);
					return;
				}
				m_tui.transform.Find("TUIControl/UI/JoystickA").gameObject.GetComponent<GUIJoystick>().Reset();
				WeaponInfo weaponInfoByName4 = DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName());
				float zoomChangeTime4 = DataCenter.Conf().GetCameraParam(weaponInfoByName4.camera).zoomChangeTime;
				GameManager.Instance().GetCamera().SnipeOutCrosshair();
				GameManager.Instance().GetCamera().ZoomOut(zoomChangeTime4);
				UIOutSnipeMode();
			}
		}
	}

	public void SwitchScene(string scene)
	{
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut(scene);
	}

	public bool GetIsSnipe()
	{
		return m_snipe;
	}

	private void ShowPauseBroad()
	{
		OpenClikPlugin.Show(false);
		Transform transform = m_tui.transform.Find("TUIControl/UI/PauseBroad");
		transform.GetComponent<UtilPausePrompt>().Play();
		m_tui.transform.Find("TUIControl/UI/PauseBroad").Find("Bk/Content/Panel/BarMusic").GetComponent<UtilUISlider>()
			.SetSliderRate((float)DataCenter.Save().GetOptionMusic() / 100f);
		m_tui.transform.Find("TUIControl/UI/PauseBroad").Find("Bk/Content/Panel/BarSound").GetComponent<UtilUISlider>()
			.SetSliderRate((float)DataCenter.Save().GetOptionSound() / 100f);
		m_tui.transform.Find("TUIControl/UI/PauseBroad").Find("Bk/Content/Panel/BarJoystick").GetComponent<UtilUISlider>()
			.SetSliderRate((float)DataCenter.Save().GetOptionJoystick() / 100f);
	}

	private void HidePauseBroad()
	{
		OpenClikPlugin.Hide();
		Transform transform = m_tui.transform.Find("TUIControl/UI/PauseBroad");
		transform.localPosition = new Vector3(0f, 450f, transform.localPosition.z);
	}

	private void UIInSnipeMode()
	{
		m_tui.transform.Find("TUIControl/UI/NormalMode").localPosition = new Vector3(0f, 30000f, 0f);
		m_tui.transform.Find("TUIControl/UI/JoystickA").gameObject.SetActiveRecursively(false);
		m_tui.transform.Find("TUIControl/UI/ChangeWeapon").gameObject.SetActiveRecursively(false);
		m_sniperMode.transform.localPosition = Vector3.zero;
	}

	private void UIOutSnipeMode()
	{
		m_tui.transform.Find("TUIControl/UI/NormalMode").localPosition = Vector3.zero;
		m_tui.transform.Find("TUIControl/UI/JoystickA").gameObject.SetActiveRecursively(true);
		m_tui.transform.Find("TUIControl/UI/ChangeWeapon").gameObject.SetActiveRecursively(true);
		m_sniperMode.transform.localPosition = new Vector3(0f, 30000f, 0f);
	}

	private void UINotShow()
	{
		m_tui.transform.Find("TUIControl/UI").localPosition = new Vector3(0f, 20000f, 0f);
	}

	private void UIShow()
	{
		m_tui.transform.Find("TUIControl/UI").localPosition = Vector3.zero;
	}

	public void OnGUI()
	{
		double averagePing = DataCenter.StateMulti().m_tnet.TimeManager.AveragePing;
		if (averagePing <= 200.0)
		{
			GUI.backgroundColor = Color.green;
		}
		else if (averagePing > 400.0 && averagePing <= 800.0)
		{
			GUI.backgroundColor = Color.yellow;
		}
		else if (averagePing > 800.0)
		{
			GUI.backgroundColor = Color.red;
		}
		GUI.Button(new Rect(10f, 5f, 200f, 30f), averagePing.ToString("F04"));
	}
}

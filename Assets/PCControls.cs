using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCControls : MonoBehaviour {
	private float m_sensitivety = 0.1f;

	private float m_sensitivety_Yfacter = 0.667f;

	private GameObject m_weaponObject;

	private TUI m_tui;
	// Use this for initialization
	void Start () {
		m_tui = TUI.Instance("TUI");
		m_weaponObject = m_tui.transform.Find("TUIControl/UI/NormalMode/Weapons/Weapons").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
				if (GameManager.Instance().GetCamera().m_cameraMode != GameCamera.CameraMode.NormalMode || Application.isMobilePlatform)
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
		if (Screen.lockCursor)
		{
			player3.TurnAround(Input.GetAxis("Mouse X") * num3, Input.GetAxis("Mouse Y") * num3 * m_sensitivety_Yfacter);
			if (Input.GetButtonDown("Fire1"))
			{
				player3.SetFire(true, true);
			}
		}
		if (Input.GetButtonUp("Fire1"))
		{
			player3.SetFire(false, false);
		}
		if (Input.GetKeyDown(KeyCode.E) && !player3.m_throwing && !player3.m_reloading && !player3.m_dead)
		{
			m_weaponObject.GetComponent<Animation>().Play("WeaponChangeAnim");
		}
		if (Input.GetKeyDown(KeyCode.Q) && !player3.m_throwing && !player3.m_reloading && !player3.m_dead)
		{
			m_weaponObject.GetComponent<Animation>().Play("WeaponChangeAnim2");
		}
		if (Input.GetKeyDown(KeyCode.R) && !player3.m_dead && !player3.m_throwing && !player3.m_changing && player3.GetWeapon().GetWeaponType() != 0)
		{
			player3.OpenReload();
		}
		//if (Input.GetKeyDown(KeyCode.Escape))
		//{
		//	SwitchScene("WorldMap");
		//	Cursor.visible = true;
		//	Screen.lockCursor = false;
		//	DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Back");
		//	UtilsNet.SendLeave();
		//}
		//if (Input.GetKeyDown(KeyCode.KeypadPlus))
		//{
		//	DataCenter.Save().SetOptionJoystick(Mathf.Min(DataCenter.Save().GetOptionJoystick() + 1, 100));
		//	DataCenter.Save().Save();
		//}
		//if (Input.GetKeyDown(KeyCode.KeypadMinus))
		//{
		//	DataCenter.Save().SetOptionJoystick(Mathf.Max(DataCenter.Save().GetOptionJoystick() - 1, 1));
		//	DataCenter.Save().Save();
		//}
		//if (Input.GetKeyDown(KeyCode.B))
		//{
		//	if (!player3.m_dead)
		//	{
		//		int buyAmmoPrice = DataCenter.Conf().GetWeaponInfoByName(player3.GetWeapon().GetName()).buyAmmoPrice;
		//		player3.GetWeapon().AddAmmo(DataCenter.Conf().GetWeaponInfoByName(player3.GetWeapon().GetName()).ammoAmount);
		//		TAnalyticsManager.AttributeAddInteger("BuyAmmoMoney", buyAmmoPrice);
		//	}
		//	DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_GameButton_BuyBullets");
		//}
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
	}
}

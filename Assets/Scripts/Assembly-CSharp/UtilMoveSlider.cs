using UnityEngine;

public class UtilMoveSlider : MonoBehaviour
{
	public UIMissionConfirm m_missionConfirm;

	private GameObject m_sliderObject;

	private WeaponInfo m_weaponInfo;

	private ArmorInfo m_armorInfo;

	private ItemInfo m_itemInfo;

	private void OnTUIMoveBegin(TUIInput input)
	{
		m_sliderObject = base.transform.parent.parent.parent.parent.parent.parent.Find("Slider").Find("SliderEquip").gameObject;
		if (base.transform.parent.name.Contains("W_"))
		{
			m_weaponInfo = DataCenter.Conf().GetWeaponInfoByName(base.transform.parent.name);
			m_sliderObject.transform.Find("Normal").GetComponent<TUIMeshSprite>().frameName = m_weaponInfo.icon;
			m_sliderObject.transform.Find("Pressed").GetComponent<TUIMeshSprite>().frameName = m_weaponInfo.icon;
			if (m_weaponInfo.type == WeaponType.Knife)
			{
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Melee_Drawup&Select");
			}
			else
			{
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Gun_Drawup&Select");
			}
		}
		else if (base.transform.parent.name.Contains("A_"))
		{
			m_armorInfo = DataCenter.Conf().GetArmorInfoByName(base.transform.parent.name);
			m_sliderObject.transform.Find("Normal").GetComponent<TUIMeshSprite>().frameName = m_armorInfo.icon;
			m_sliderObject.transform.Find("Pressed").GetComponent<TUIMeshSprite>().frameName = m_armorInfo.icon;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Ammor_Drawup&Select");
		}
		else if (base.transform.parent.name.Contains("I_"))
		{
			m_itemInfo = DataCenter.Conf().GetItemInfoByName(base.transform.parent.name);
			m_sliderObject.transform.Find("Normal").GetComponent<TUIMeshSprite>().frameName = m_itemInfo.icon;
			m_sliderObject.transform.Find("Pressed").GetComponent<TUIMeshSprite>().frameName = m_itemInfo.icon;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Items_Drawup&Select");
		}
		m_sliderObject.transform.localPosition = new Vector3(input.position.x, input.position.y, 0f);
		m_sliderObject.transform.GetComponent<TUIButtonSliderCallBack>().begin_delegate = OnButtonSliderBeginDelegate;
		m_sliderObject.transform.GetComponent<TUIButtonSliderCallBack>().move_delegate = OnButtonSliderMoveDelegate;
		m_sliderObject.transform.GetComponent<TUIButtonSliderCallBack>().end_delegate = OnButtonSliderEndDelegate;
		m_sliderObject.transform.GetComponent<TUIButtonSliderCallBack>().SimCommandDown(input);
	}

	private void OnButtonSliderBeginDelegate(TUIInput input)
	{
	}

	private void OnButtonSliderMoveDelegate(TUIInput input)
	{
	}

	private void OnButtonSliderEndDelegate(TUIInput input)
	{
		Transform transform = m_sliderObject.transform.parent.parent.Find("EquipmentWindow").Find("Equipments");
		TUIButtonSelect component = transform.GetComponent<UtilEquipments>().weapon01Object.transform.Find("WeaponBtn").GetComponent<TUIButtonSelect>();
		TUIButtonSelect component2 = transform.GetComponent<UtilEquipments>().weapon02Object.transform.Find("WeaponBtn").GetComponent<TUIButtonSelect>();
		TUIButtonSelect component3 = transform.GetComponent<UtilEquipments>().weapon03Object.transform.Find("WeaponBtn").GetComponent<TUIButtonSelect>();
		TUIButtonSelect component4 = transform.GetComponent<UtilEquipments>().armor01Object.transform.Find("ArmorBtn").GetComponent<TUIButtonSelect>();
		TUIButtonSelect component5 = transform.GetComponent<UtilEquipments>().armor02Object.transform.Find("ArmorBtn").GetComponent<TUIButtonSelect>();
		TUIButtonSelect component6 = transform.GetComponent<UtilEquipments>().armor03Object.transform.Find("ArmorBtn").GetComponent<TUIButtonSelect>();
		TUIButtonSelect component7 = transform.GetComponent<UtilEquipments>().items01Object.transform.Find("ItemsBtn").GetComponent<TUIButtonSelect>();
		TUIButtonSelect component8 = transform.GetComponent<UtilEquipments>().items02Object.transform.Find("ItemsBtn").GetComponent<TUIButtonSelect>();
		TUIButtonSelect component9 = transform.GetComponent<UtilEquipments>().items03Object.transform.Find("ItemsBtn").GetComponent<TUIButtonSelect>();
		if (component.IsSelected() && component.PtInControl(input.position))
		{
			if (m_weaponInfo != null)
			{
				WeaponType type = DataCenter.Conf().GetWeaponInfoByName(transform.GetComponent<UtilEquipments>().weapon01Object.name).type;
				if (m_weaponInfo.type == type)
				{
					DataCenter.Save().EquipWeapon(m_missionConfirm.m_charactorIndex, m_weaponInfo.name);
				}
				else
				{
					DataCenter.Save().EquipWeaponInPos(m_missionConfirm.m_charactorIndex, m_weaponInfo.name, 1);
				}
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Gun_DrawDown");
			}
		}
		else if (component2.IsSelected() && component2.PtInControl(input.position))
		{
			if (m_weaponInfo != null)
			{
				WeaponType type2 = DataCenter.Conf().GetWeaponInfoByName(transform.GetComponent<UtilEquipments>().weapon02Object.name).type;
				if (m_weaponInfo.type == type2)
				{
					DataCenter.Save().EquipWeapon(m_missionConfirm.m_charactorIndex, m_weaponInfo.name);
				}
				else
				{
					DataCenter.Save().EquipWeaponInPos(m_missionConfirm.m_charactorIndex, m_weaponInfo.name, 2);
				}
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Gun_DrawDown");
			}
		}
		else if (component3.IsSelected() && component3.PtInControl(input.position))
		{
			if (m_weaponInfo != null)
			{
				WeaponType type3 = DataCenter.Conf().GetWeaponInfoByName(transform.GetComponent<UtilEquipments>().weapon03Object.name).type;
				if (m_weaponInfo.type == type3)
				{
					DataCenter.Save().EquipWeapon(m_missionConfirm.m_charactorIndex, m_weaponInfo.name);
				}
				else
				{
					DataCenter.Save().EquipWeaponInPos(m_missionConfirm.m_charactorIndex, m_weaponInfo.name, 3);
				}
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Melee_DrawDown");
			}
		}
		else if (component4.IsSelected() && component4.PtInControl(input.position))
		{
			if (m_armorInfo != null)
			{
				DataCenter.Save().EquipArmorInPos(m_missionConfirm.m_charactorIndex, m_armorInfo.name, 1);
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Ammor_DrawDown");
		}
		else if (component5.IsSelected() && component5.PtInControl(input.position))
		{
			if (m_armorInfo != null)
			{
				DataCenter.Save().EquipArmorInPos(m_missionConfirm.m_charactorIndex, m_armorInfo.name, 2);
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Ammor_DrawDown");
		}
		else if (component6.IsSelected() && component6.PtInControl(input.position))
		{
			if (m_armorInfo != null)
			{
				DataCenter.Save().EquipArmorInPos(m_missionConfirm.m_charactorIndex, m_armorInfo.name, 3);
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Ammor_DrawDown");
		}
		else if (component7.IsSelected() && component7.PtInControl(input.position))
		{
			if (m_itemInfo != null)
			{
				DataCenter.Save().EquipItemInPos(m_missionConfirm.m_charactorIndex, m_itemInfo.name, 1);
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Items_DrawDown");
		}
		else if (component8.IsSelected() && component8.PtInControl(input.position))
		{
			if (m_itemInfo != null)
			{
				DataCenter.Save().EquipItemInPos(m_missionConfirm.m_charactorIndex, m_itemInfo.name, 2);
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Items_DrawDown");
		}
		else if (component9.IsSelected() && component9.PtInControl(input.position))
		{
			if (m_itemInfo != null)
			{
				DataCenter.Save().EquipItemInPos(m_missionConfirm.m_charactorIndex, m_itemInfo.name, 3);
			}
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_Items_DrawDown");
		}
		DataCenter.Save().Save();
		m_missionConfirm.RefreshCharacterEquipInfo(m_missionConfirm.m_charactorIndex);
		m_sliderObject.transform.localPosition = new Vector3(0f, 10000f, 0f);
		base.transform.GetComponent<TUIMoveCallBack>().Reset();
		base.transform.parent.Find("Btn").GetComponent<TUIButtonSelect>().Reset();
	}
}

using UnityEngine;

public class UtilUILevelItems : MonoBehaviour
{
	public TUITexture m_TUITextureDynamic;

	public TUIMeshSprite m_item01Icon;

	public TUIMeshSprite m_item02Icon;

	public TUIMeshSprite m_item03Icon;

	public TUIButtonClick m_item01Btn;

	public TUIButtonClick m_item02Btn;

	public TUIButtonClick m_item03Btn;

	public TUIMeshText m_item01Text;

	public TUIMeshText m_item02Text;

	public TUIMeshText m_item03Text;

	public bool isWeb;

	private string m_item01IconName;

	private string m_item02IconName;

	private string m_item03IconName;

	private CharacterEquipInfo m_characterEquipInfo;

	public void Awake()
	{
		CharacterEquipInfo characterEquipInfo = (isWeb ? DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateMulti().GetCurrentCharactor()) : DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateSingle().GetCurrentCharactor()));
		if (characterEquipInfo.item01 != string.Empty)
		{
			m_item01IconName = DataCenter.Conf().GetItemInfoByName(characterEquipInfo.item01).icon;
			m_TUITextureDynamic.LoadFrame(m_item01IconName);
			m_item01Text.text = DataCenter.Save().GetItemNums(characterEquipInfo.item01).ToString();
			m_item01Text.UpdateMesh();
		}
		if (characterEquipInfo.item02 != string.Empty)
		{
			m_item02IconName = DataCenter.Conf().GetItemInfoByName(characterEquipInfo.item02).icon;
			m_TUITextureDynamic.LoadFrame(m_item02IconName);
			m_item02Text.text = DataCenter.Save().GetItemNums(characterEquipInfo.item02).ToString();
			m_item02Text.UpdateMesh();
		}
		if (characterEquipInfo.item03 != string.Empty)
		{
			m_item03IconName = DataCenter.Conf().GetItemInfoByName(characterEquipInfo.item03).icon;
			m_TUITextureDynamic.LoadFrame(m_item03IconName);
			m_item03Text.text = DataCenter.Save().GetItemNums(characterEquipInfo.item03).ToString();
			m_item03Text.UpdateMesh();
		}
	}

	public void Start()
	{
		if (m_item01IconName != null)
		{
			m_item01Icon.frameName = m_item01IconName;
			m_item01Icon.UpdateMesh();
		}
		else
		{
			m_item01Btn.gameObject.SetActiveRecursively(false);
			m_item01Icon.gameObject.SetActiveRecursively(false);
			m_item01Text.gameObject.SetActiveRecursively(false);
		}
		if (m_item02IconName != null)
		{
			m_item02Icon.frameName = m_item02IconName;
			m_item02Icon.UpdateMesh();
		}
		else
		{
			m_item02Btn.gameObject.SetActiveRecursively(false);
			m_item02Icon.gameObject.SetActiveRecursively(false);
			m_item02Text.gameObject.SetActiveRecursively(false);
		}
		if (m_item03IconName != null)
		{
			m_item03Icon.frameName = m_item03IconName;
			m_item03Icon.UpdateMesh();
		}
		else
		{
			m_item03Btn.gameObject.SetActiveRecursively(false);
			m_item03Icon.gameObject.SetActiveRecursively(false);
			m_item03Text.gameObject.SetActiveRecursively(false);
		}
	}
}

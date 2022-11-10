using UnityEngine;

public class UtilUIUpdateWorldMapLevel : MonoBehaviour
{
	public GameObject m_level01;

	public GameObject m_level02;

	public GameObject m_level03;

	public GameObject m_level04;

	public GameObject m_base;

	public GameObject m_shop;

	public void UpdateLevel(LevelInfo[] levels)
	{
		LevelBtnSetting(levels[0], m_level01);
		LevelBtnSetting(levels[1], m_level02);
		LevelBtnSetting(levels[2], m_level03);
		LevelBtnSetting(levels[3], m_level04);
	}

	private void LevelBtnSetting(LevelInfo levelInfo, GameObject levelBtnObj)
	{
		if (levelInfo != null)
		{
			if (levelInfo.mode == LevelMode.Fight)
			{
				if (levelInfo.type == LevelType.Main)
				{
					levelBtnObj.transform.Find("Title").Find("Title").Find("NormalBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapFightTitle01";
					levelBtnObj.transform.Find("Title").Find("Title").Find("PressedBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapFightTitle02";
				}
				else
				{
					levelBtnObj.transform.Find("Title").Find("Title").Find("NormalBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapFightBranchTitle01";
					levelBtnObj.transform.Find("Title").Find("Title").Find("PressedBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapFightBranchTitle02";
				}
			}
			else if (levelInfo.mode == LevelMode.Find)
			{
				if (levelInfo.type == LevelType.Main)
				{
					levelBtnObj.transform.Find("Title").Find("Title").Find("NormalBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapFindTitle01";
					levelBtnObj.transform.Find("Title").Find("Title").Find("PressedBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapFindTitle02";
				}
				else
				{
					levelBtnObj.transform.Find("Title").Find("Title").Find("NormalBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapFindBranchTitle01";
					levelBtnObj.transform.Find("Title").Find("Title").Find("PressedBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapFindBranchTitle02";
				}
			}
			else if (levelInfo.mode == LevelMode.Survive)
			{
				if (levelInfo.type == LevelType.Main)
				{
					levelBtnObj.transform.Find("Title").Find("Title").Find("NormalBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapSurviveTitle01";
					levelBtnObj.transform.Find("Title").Find("Title").Find("PressedBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapSurviveTitle02";
				}
				else
				{
					levelBtnObj.transform.Find("Title").Find("Title").Find("NormalBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapSurviveBranchTitle01";
					levelBtnObj.transform.Find("Title").Find("Title").Find("PressedBk")
						.GetComponent<TUIMeshSprite>()
						.frameName = "WorldMapSurviveBranchTitle02";
				}
			}
		}
		else
		{
			levelBtnObj.SetActiveRecursively(false);
			if (DataCenter.Save().GetWeek() < 1)
			{
				m_base.SetActiveRecursively(false);
				m_shop.SetActiveRecursively(false);
			}
		}
	}

	public void ResetAllSelectBtnFalse()
	{
		m_level01.transform.Find("Level01Button").GetComponent<TUIButtonSelect>().SetSelected(false);
		m_level02.transform.Find("Level02Button").GetComponent<TUIButtonSelect>().SetSelected(false);
		m_level03.transform.Find("Level03Button").GetComponent<TUIButtonSelect>().SetSelected(false);
		m_level04.transform.Find("Level04Button").GetComponent<TUIButtonSelect>().SetSelected(false);
		m_shop.transform.Find("Shop2").GetComponent<TUIButtonSelect>().SetSelected(false);
		m_base.transform.Find("Base2").GetComponent<TUIButtonSelect>().SetSelected(false);
		m_level01.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(false);
		m_level02.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(false);
		m_level03.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(false);
		m_level04.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(false);
		m_shop.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(false);
		m_base.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(false);
		if (DataCenter.Save().GetWeek() < 1)
		{
			m_base.SetActiveRecursively(false);
			m_shop.SetActiveRecursively(false);
		}
	}

	public void SelectOneBtnStatus(GameObject selectBtnObj)
	{
		ResetAllSelectBtnFalse();
		if (m_level01 == selectBtnObj)
		{
			m_level01.transform.Find("Level01Button").GetComponent<TUIButtonSelect>().SetSelected(true);
			m_level01.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		else if (m_level02 == selectBtnObj)
		{
			m_level02.transform.Find("Level02Button").GetComponent<TUIButtonSelect>().SetSelected(true);
			m_level02.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		else if (m_level03 == selectBtnObj)
		{
			m_level03.transform.Find("Level03Button").GetComponent<TUIButtonSelect>().SetSelected(true);
			m_level03.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		else if (m_level04 == selectBtnObj)
		{
			m_level04.transform.Find("Level04Button").GetComponent<TUIButtonSelect>().SetSelected(true);
			m_level04.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		else if (m_shop == selectBtnObj)
		{
			m_shop.transform.Find("Shop2").GetComponent<TUIButtonSelect>().SetSelected(true);
			m_shop.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		else if (m_base == selectBtnObj)
		{
			m_base.transform.Find("Base2").GetComponent<TUIButtonSelect>().SetSelected(true);
			m_base.transform.Find("Title").Find("Title").gameObject.GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		if (DataCenter.Save().GetWeek() < 1)
		{
			m_base.SetActiveRecursively(false);
			m_shop.SetActiveRecursively(false);
		}
	}
}

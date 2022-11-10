using UnityEngine;

public class UtilUIUpdateAttribute : MonoBehaviour
{
	private int m_money = -1;

	private int m_crystal = -1;

	private int m_lv = -1;

	private int m_exp = -1;

	private static string m_lastScene;

	private static string m_nextScene;

	public bool refreshDate = true;

	public void Start()
	{
		base.transform.Find("MenuBar/Achievements").gameObject.SetActiveRecursively(false);
		base.transform.Find("MenuBar/Rank").gameObject.SetActiveRecursively(false);
		if (m_money != DataCenter.Save().GetMoney())
		{
			m_money = DataCenter.Save().GetMoney();
			UpdateMoney();
		}
		if (m_crystal != DataCenter.Save().GetCrystal())
		{
			m_crystal = DataCenter.Save().GetCrystal();
			UpdateCrystal();
		}
		if (m_lv != DataCenter.Save().GetLv())
		{
			m_lv = DataCenter.Save().GetLv();
			UpdataLV();
		}
		if (m_exp != DataCenter.Save().GetExp())
		{
			m_exp = DataCenter.Save().GetExp();
			UpdateLvBar();
		}
	}

	public void Update()
	{
		if (refreshDate)
		{
			if (m_money != DataCenter.Save().GetMoney())
			{
				m_money = DataCenter.Save().GetMoney();
				UpdateMoney();
			}
			if (m_crystal != DataCenter.Save().GetCrystal())
			{
				m_crystal = DataCenter.Save().GetCrystal();
				UpdateCrystal();
			}
			if (m_lv != DataCenter.Save().GetLv())
			{
				m_lv = DataCenter.Save().GetLv();
				UpdataLV();
			}
			if (m_exp != DataCenter.Save().GetExp())
			{
				m_exp = DataCenter.Save().GetExp();
				UpdateLvBar();
			}
		}
	}

	public static void SetNextScene(string nextScene)
	{
		m_nextScene = nextScene;
	}

	public static string GetNextScene()
	{
		return m_nextScene;
	}

	public static void SetLastScene(string lastScene)
	{
		m_lastScene = lastScene;
	}

	public static string GetLastScene()
	{
		return m_lastScene;
	}

	private void UpdateMoney()
	{
		Transform transform = base.transform.Find("StatusBar").Find("MoneyText");
		if (transform != null)
		{
			TUIMeshText component = transform.GetComponent<TUIMeshText>();
			if (m_money > 0)
			{
				component.text = m_money.ToString("###,###");
			}
			else
			{
				component.text = "0";
			}
			component.UpdateMesh();
		}
	}

	private void UpdateCrystal()
	{
		Transform transform = base.transform.Find("StatusBar").Find("CrystalText");
		if (transform != null)
		{
			TUIMeshText component = transform.GetComponent<TUIMeshText>();
			if (m_crystal == 0)
			{
				component.text = "0";
			}
			else
			{
				component.text = m_crystal.ToString("###,###");
			}
			component.UpdateMesh();
		}
	}

	private void UpdataLV()
	{
		Transform transform = base.transform.Find("StatusBar").Find("LevelText");
		if (transform != null)
		{
			TUIMeshText component = transform.GetComponent<TUIMeshText>();
			component.text = "LV" + m_lv;
			component.UpdateMesh();
		}
	}

	private void UpdateLvBar()
	{
		Transform transform = base.transform.Find("StatusBar").Find("ExpBar").Find("Bar");
		Transform transform2 = transform.Find("Clip");
		TUIRect component = transform2.GetComponent<TUIRect>();
		int lv = DataCenter.Save().GetLv();
		int num = DataCenter.Save().GetExp() - DataCenter.Conf().GetLevelUpExpMap()[lv].totalExp;
		int exp = DataCenter.Conf().GetLevelUpExpByLv(lv).exp;
		component.rect.width = (0f - component.rect.x) * 2f * (float)num / (float)exp;
		component.UpdateRect();
		TUIMeshSprite component2 = transform.Find("BarMiddle").GetComponent<TUIMeshSprite>();
		component2.UpdateMesh();
		TUIMeshSprite component3 = transform.Find("BarLeft").GetComponent<TUIMeshSprite>();
		component3.gameObject.SetActiveRecursively(component.rect.width != 0f);
		TUIMeshSprite component4 = transform.Find("BarRight").GetComponent<TUIMeshSprite>();
		component4.gameObject.SetActiveRecursively(component.rect.width >= 62f);
	}

	public void UpdateMoney(int money)
	{
		Transform transform = base.transform.Find("StatusBar").Find("MoneyText");
		if (transform != null)
		{
			TUIMeshText component = transform.GetComponent<TUIMeshText>();
			if (money > 0)
			{
				component.text = money.ToString("###,###");
			}
			else
			{
				component.text = "0";
			}
			component.UpdateMesh();
		}
	}

	public bool UpdataLV(int lv)
	{
		Transform transform = base.transform.Find("StatusBar").Find("LevelText");
		if (transform != null)
		{
			TUIMeshText component = transform.GetComponent<TUIMeshText>();
			if (component.text != "LV" + lv)
			{
				component.text = "LV" + lv;
				component.UpdateMesh();
				return true;
			}
		}
		return false;
	}

	public void UpdateLvBar(int lv, int exp)
	{
		Transform transform = base.transform.Find("StatusBar").Find("ExpBar").Find("Bar");
		Transform transform2 = transform.Find("Clip");
		TUIRect component = transform2.GetComponent<TUIRect>();
		int num = exp - DataCenter.Conf().GetLevelUpExpMap()[lv].totalExp;
		int exp2 = DataCenter.Conf().GetLevelUpExpByLv(lv).exp;
		component.rect.width = (0f - component.rect.x) * 2f * (float)num / (float)exp2;
		component.UpdateRect();
		TUIMeshSprite component2 = transform.Find("BarMiddle").GetComponent<TUIMeshSprite>();
		component2.UpdateMesh();
		TUIMeshSprite component3 = transform.Find("BarLeft").GetComponent<TUIMeshSprite>();
		component3.gameObject.SetActiveRecursively(component.rect.width != 0f);
		TUIMeshSprite component4 = transform.Find("BarRight").GetComponent<TUIMeshSprite>();
		component4.gameObject.SetActiveRecursively(component.rect.width >= 62f);
	}
}

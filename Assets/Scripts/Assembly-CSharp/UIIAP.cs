using UnityEngine;

public class UIIAP : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	public GameObject UIBlockObj;

	public GameObject UIBlockObj2;

	public int purchasetype = -1;

	private bool purchaseKey;

	private float purchaseOutTime;

	public static bool bAndroidInitOK;

	private static GameObject androidEventListener;

	private static GameObject androidIAPManager;

	public void Awake()
	{
	}

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
		RelevanceIABBasicEvent();
		BuyCrystal();
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
		if (!purchaseKey)
		{
			return;
		}
		purchaseOutTime += Time.deltaTime;
		if (purchaseOutTime > 30f)
		{
			HideIndicator();
			purchasetype = -1;
			purchaseKey = false;
			if (Utils.ShowMessageBox1(string.Empty, "Connection timed out!", "OK") == 0)
			{
				return;
			}
		}
		int num = -1000;
		switch (IAPPlugin.GetPurchaseStatus())
		{
		case -3:
			HideIndicator();
			purchasetype = -1;
			purchaseKey = false;
			Utils.ShowMessageBox1(string.Empty, "Sorry, the connection failed!", "OK");
			break;
		case -2:
			HideIndicator();
			purchasetype = -1;
			purchaseKey = false;
			break;
		case -1:
			HideIndicator();
			purchasetype = -1;
			purchaseKey = false;
			break;
		case 0:
			break;
		case 1:
			HideIndicator();
			PurchaseFunction(purchasetype);
			purchasetype = -1;
			purchaseKey = false;
			break;
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "ToMoney" && eventType == 3)
		{
			BuyMoney();
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "ToCrystal" && eventType == 3)
		{
			BuyCrystal();
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
		else if (control.name == "Buy" && eventType == 3)
		{
			if (control.transform.parent.name == "Crystal01")
			{
				BuyIAP(1, "com.trinitigame.warcorpsgenesis.099cents", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Crystal02")
			{
				BuyIAP(2, "com.trinitigame.warcorpsgenesis.299cents", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Crystal03")
			{
				BuyIAP(3, "com.trinitigame.warcorpsgenesis.499cents", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Crystal04")
			{
				BuyIAP(4, "com.trinitigame.warcorpsgenesis.999cents", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Crystal05")
			{
				BuyIAP(5, "com.trinitigame.warcorpsgenesis.1999cents", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Crystal06")
			{
				BuyIAP(6, "com.trinitigame.warcorpsgenesis.2999cents", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Crystal07")
			{
				BuyIAP(7, "com.trinitigame.warcorpsgenesis.4999cents", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Crystal08")
			{
				BuyIAP(8, "com.trinitigame.warcorpsgenesis.9999cents", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Money01")
			{
				BuyIAP(9, "com.trinitigame.warcorpsgenesis.099cents2", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Money02")
			{
				BuyIAP(10, "com.trinitigame.warcorpsgenesis.299cents2", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Money03")
			{
				BuyIAP(11, "com.trinitigame.warcorpsgenesis.499cents2", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Money04")
			{
				BuyIAP(12, "com.trinitigame.warcorpsgenesis.999cents2", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Money05")
			{
				BuyIAP(13, "com.trinitigame.warcorpsgenesis.1999cents2", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Money06")
			{
				BuyIAP(14, "com.trinitigame.warcorpsgenesis.2999cents2", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Money07")
			{
				BuyIAP(15, "com.trinitigame.warcorpsgenesis.4999cents2", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
			else if (control.transform.parent.name == "Money08")
			{
				BuyIAP(16, "com.trinitigame.warcorpsgenesis.9999cents2", control.gameObject);
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_BuyItem");
			}
		}
		else if (control.name == "Back" && eventType == 3)
		{
			if (UtilUIUpdateAttribute.GetLastScene() != "MissionEnd")
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

	private void SwitchScene(string scene)
	{
		UtilUIUpdateAttribute.SetLastScene("IAP");
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

	private void ShowIndicator()
	{
		purchaseOutTime = 0f;
		UIBlockObj2.SetActiveRecursively(true);
		Utils.ShowIndicatorSystem_int(1, Utils.ScreenCenter(), 0f, 0f, 0f, 0f);
	}

	public void HideIndicator()
	{
		UIBlockObj2.SetActiveRecursively(false);
		Utils.HideIndicatorSystem();
	}

	private void BuyCrystal()
	{
		m_tui.transform.Find("TUIControl/Crystal").localPosition = Vector3.zero;
		m_tui.transform.Find("TUIControl/Money").localPosition = new Vector3(0f, 10000f, 0f);
	}

	private void BuyMoney()
	{
		m_tui.transform.Find("TUIControl/Money").transform.localPosition = Vector3.zero;
		m_tui.transform.Find("TUIControl/Crystal").transform.localPosition = new Vector3(0f, 10000f, 0f);
	}

	public void PurchaseFunction(int type)
	{
		switch (type)
		{
		case 1:
			DataCenter.Save().SetCrystal(DataCenter.Save().GetCrystal() + 10);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.099cents", 1);
			break;
		case 2:
			DataCenter.Save().SetCrystal(DataCenter.Save().GetCrystal() + 32);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.299cents", 1);
			break;
		case 3:
			DataCenter.Save().SetCrystal(DataCenter.Save().GetCrystal() + 54);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.499cents", 1);
			break;
		case 4:
			DataCenter.Save().SetCrystal(DataCenter.Save().GetCrystal() + 110);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.999cents", 1);
			break;
		case 5:
			DataCenter.Save().SetCrystal(DataCenter.Save().GetCrystal() + 225);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.1999cents", 1);
			break;
		case 6:
			DataCenter.Save().SetCrystal(DataCenter.Save().GetCrystal() + 345);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.2999cents", 1);
			break;
		case 7:
			DataCenter.Save().SetCrystal(DataCenter.Save().GetCrystal() + 585);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.4999cents", 1);
			break;
		case 8:
			DataCenter.Save().SetCrystal(DataCenter.Save().GetCrystal() + 1200);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.9999cents", 1);
			break;
		case 9:
			DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + 10000);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.099cents2", 1);
			break;
		case 10:
			DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + 32000);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.299cents2", 1);
			break;
		case 11:
			DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + 54000);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.499cents2", 1);
			break;
		case 12:
			DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + 110000);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.999cents2", 1);
			break;
		case 13:
			DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + 225000);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.1999cents2", 1);
			break;
		case 14:
			DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + 345000);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.2999cents2", 1);
			break;
		case 15:
			DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + 585000);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.4999cents2", 1);
			break;
		case 16:
			DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + 1200000);
			DataCenter.Save().Save();
			TAnalyticsManager.IAP("com.trinitigame.warcorpsgenesis.9999cents2", 1);
			break;
		}
	}

	public void RelevanceAmazonBasicEvent()
	{
		Debug.Log("RelevanceAmazonBasicEvent Begin,  androidEventListener is null " + (androidEventListener == null));
		if (androidEventListener == null)
		{
			androidEventListener = Object.Instantiate(Resources.Load("Android/AmazonIAPEventListener", typeof(GameObject))) as GameObject;
			Object.DontDestroyOnLoad(androidEventListener);
		}
		if (androidIAPManager == null)
		{
			androidIAPManager = Object.Instantiate(Resources.Load("Android/AmazonIAPManager", typeof(GameObject))) as GameObject;
			Object.DontDestroyOnLoad(androidIAPManager);
		}
		if (!bAndroidInitOK)
		{
			AmazonIAP.initiateItemDataRequest(new string[16]
			{
				"com.trinitigame.warcorpsgenesis.099cents", "com.trinitigame.warcorpsgenesis.299cents", "com.trinitigame.warcorpsgenesis.499cents", "com.trinitigame.warcorpsgenesis.999cents", "com.trinitigame.warcorpsgenesis.1999cents", "com.trinitigame.warcorpsgenesis.2999cents", "com.trinitigame.warcorpsgenesis.4999cents", "com.trinitigame.warcorpsgenesis.9999cents", "com.trinitigame.warcorpsgenesis.099cents2", "com.trinitigame.warcorpsgenesis.299cents2",
				"com.trinitigame.warcorpsgenesis.499cents2", "com.trinitigame.warcorpsgenesis.999cents2", "com.trinitigame.warcorpsgenesis.1999cents2", "com.trinitigame.warcorpsgenesis.2999cents2", "com.trinitigame.warcorpsgenesis.4999cents2", "com.trinitigame.warcorpsgenesis.9999cents2"
			});
			ShowIndicator();
		}
	}

	public void RelevanceIABBasicEvent()
	{
		Debug.Log("RelevanceIABBasicEvent Begin,  androidEventListener is null " + (androidEventListener == null));
		if (androidEventListener == null)
		{
			androidEventListener = Object.Instantiate(Resources.Load("Android/IABAndroidEventListener", typeof(GameObject))) as GameObject;
			Object.DontDestroyOnLoad(androidEventListener);
		}
		if (androidIAPManager == null)
		{
			GameObject target = Object.Instantiate(Resources.Load("Android/IABAndroidManager", typeof(GameObject))) as GameObject;
			Object.DontDestroyOnLoad(target);
		}
		if (!bAndroidInitOK)
		{
			IABAndroid.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAswqzY36PRohKXYH8/9Yi8Fa2CQ1fkRurivpK0mj/roJLQIANFY2B+qjaztxh5dE1FsdR4sqkZ6dWnCLYTH9zwfTsXE8FfdHgJkXmClHLj8HCOYva4+nHj9gs7nmrV/HT40ZaEoAVLT4vL7ReNDjl1OP+qWvhxTPXMKp80FIkGYEQDJcYtjOElN7v2dgQoSALucOvcE+fpkAgJ7Cvijqb++T5N4hJ9hbUdykZ1/aHFtup3rQvdweLCcnJvpZLNqqd/uamy8DcJ1s2qMXaprJJTk2Nn4weVGlaIB0tAlPDi7n9L7a+hyfD0VsAlzTdfg7LtOlyakum6S0iJQIejxrEJQIDAQAB");
			ShowIndicator();
		}
	}

	public void AndoridIAPBuySuccessed()
	{
		PurchaseFunction(purchasetype);
		purchasetype = -1;
		purchaseKey = false;
	}

	public void BuyIAP(int purtype, string iapKey, GameObject control)
	{
		if (bAndroidInitOK)
		{
			IABAndroid.purchaseProduct(iapKey);
			ShowIndicator();
		}
		else
		{
			control.GetComponent<TUIButtonClick>().Reset();
		}
	}
}

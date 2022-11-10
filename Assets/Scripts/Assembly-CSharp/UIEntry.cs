using System;
using System.IO;
using UnityEngine;

public class UIEntry : MonoBehaviour, TUIHandler
{
	public GameObject btn;

	private TUI m_tui;

	private int m_reveiw_count;

	public void Awake()
	{
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			DataConstant.bDebug = true;
		}
		else
		{
			DataConstant.bDebug = false;
		}
	}

	public void Start()
	{
		OpenClikPlugin.Initialize("51ccf42c16ba475771000000", "2c04c7e1a50ee42df3dcd060550bb856c33bddbe");
		MyFlurry.InitFlurry();
		TAnalyticsManager.Initialize();
		Application.targetFrameRate = 60;
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		btn.GetComponent<Animation>()["BtnFlash"].wrapMode = WrapMode.Loop;
		if (DataCenter.StateCommon().audio == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "AudioController";
			gameObject.AddComponent<TAudioController>();
			DataCenter.StateCommon().audio = gameObject;
			DataCenter.StateCommon().bgmName = "music_Meu";
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("music_Meu");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		TAudioManager.instance.musicVolume = (float)DataCenter.Save().GetOptionMusic() * 0.01f;
		TAudioManager.instance.soundVolume = (float)DataCenter.Save().GetOptionSound() * 0.01f;
		if (DataCenter.Save().GetCharacterEquipInfo(1).unlocked)
		{
			TAnalyticsManager.AttributeSetInteger("UnlockCharacter01", 1);
		}
		else
		{
			TAnalyticsManager.AttributeSetInteger("UnlockCharacter01", 0);
		}
		if (DataCenter.Save().GetCharacterEquipInfo(2).unlocked)
		{
			TAnalyticsManager.AttributeSetInteger("UnlockCharacter02", 1);
		}
		else
		{
			TAnalyticsManager.AttributeSetInteger("UnlockCharacter02", 0);
		}
		if (DataCenter.Save().GetCharacterEquipInfo(3).unlocked)
		{
			TAnalyticsManager.AttributeSetInteger("UnlockCharacter03", 1);
		}
		else
		{
			TAnalyticsManager.AttributeSetInteger("UnlockCharacter03", 0);
		}
		if (DataCenter.Save().GetCharacterEquipInfo(4).unlocked)
		{
			TAnalyticsManager.AttributeSetInteger("UnlockCharacter04", 1);
		}
		else
		{
			TAnalyticsManager.AttributeSetInteger("UnlockCharacter04", 0);
		}
		TAnalyticsManager.AttributeSetInteger("Level", DataCenter.Save().GetLv());
		TAnalyticsManager.AttributeSetInteger("CurrentMoney", DataCenter.Save().GetMoney());
		TAnalyticsManager.AttributeSetInteger("CurrentCrystal", DataCenter.Save().GetCrystal());
		TAnalyticsManager.AttributeSetInteger("Character01PlayTotalTime", DataCenter.Save().GetCharacterEquipInfo(1).totalTime);
		TAnalyticsManager.AttributeSetInteger("Character02PlayTotalTime", DataCenter.Save().GetCharacterEquipInfo(2).totalTime);
		TAnalyticsManager.AttributeSetInteger("Character03PlayTotalTime", DataCenter.Save().GetCharacterEquipInfo(3).totalTime);
		TAnalyticsManager.AttributeSetInteger("Character04PlayTotalTime", DataCenter.Save().GetCharacterEquipInfo(4).totalTime);
		TAnalyticsManager.AttributeSetString("UnlockWeapon", DataCenter.Save().GetUnlockWeaponName());
		TAnalyticsManager.AttributeSetString("UnlockArmor", DataCenter.Save().GetUnlockArmorName());
		GameCenterPlugin.Initialize();
		if (GameCenterPlugin.IsSupported() && !GameCenterPlugin.IsLogin())
		{
			GameCenterPlugin.Login();
		}
		if (!DataCenter.StateCommon().review)
		{
			OpenClikPlugin.Show(true);
			DataCenter.StateCommon().review = true;
			ReadReviewCount();
			if (m_reveiw_count < 4)
			{
				m_reveiw_count++;
			}
			SaveReviewCount();
			if (m_reveiw_count == 3 && Utils.ShowMessageBox2(string.Empty, "Having fun? Rate this app!", "YES", "LATER") == 0)
			{
				Application.OpenURL("market://details?id=com.trinitigame.android.warcorpsgenesis");
			}
		}
		else
		{
			OpenClikPlugin.Show(false);
		}
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
	}

	public void OnDestroy()
	{
		OpenClikPlugin.Hide();
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Begin" && eventType == 3)
		{
			SwitchScene("WorldMap");
			UtilUIUpdateAttribute.SetNextScene("WorldMap");
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
	}

	private void SwitchScene(string scene)
	{
		UtilUIUpdateAttribute.SetLastScene("Entry");
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut(scene);
	}

	public void ReadReviewCount()
	{
		string path = FileUtil.GetSavePath() + "/reveiw.dat";
		if (File.Exists(path))
		{
			FileStream fileStream = new FileStream(path, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			m_reveiw_count = binaryReader.ReadInt32();
			binaryReader.Close();
			fileStream.Close();
		}
		else
		{
			SaveReviewCount();
		}
	}

	public void SaveReviewCount()
	{
		string path = FileUtil.GetSavePath() + "/reveiw.dat";
		try
		{
			FileStream fileStream = new FileStream(path, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(m_reveiw_count);
			binaryWriter.Close();
			fileStream.Close();
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
		}
	}

	private void UpdateAchievement()
	{
		GameCenterPlugin.SubmitScore("com.trinitigame.warcorpsgenesis.l1", DataCenter.Save().GetEnemyKill());
		GameCenterPlugin.SubmitScore("com.trinitigame.warcorpsgenesis.l2", DataCenter.Save().GetEnemyHeadShot());
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a1", DataCenter.Save().GetEnemyKill() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a2", DataCenter.Save().GetDead() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a3", DataCenter.Save().GetEnemyHeadShot() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a4", (DataCenter.Save().GetUnlockWeaponCount() - 10) / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a5", DataCenter.Save().GetUnlockArmorCount() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a6", DataCenter.Save().GetItemUseCount() / 1 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a7", (DataCenter.Save().GetAuxiliaryMissionComplete() + DataCenter.Save().GetMainMissionComplete()) / 40 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a8", DataCenter.Save().GetWeaponHandgunKill() / 500 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a9", DataCenter.Save().GetWeaponAssaultRifleKill() / 800 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a10", DataCenter.Save().GetWeaponShotgunGunKill() / 1000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a11", DataCenter.Save().GetWeaponSubmachineGunKill() / 1000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a12", DataCenter.Save().GetWeaponSniperKill() / 1000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a13", DataCenter.Save().GetWeaponRPGKill() / 1000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a14", DataCenter.Save().GetWeaponKnifeKill() / 250 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a15", DataCenter.Save().GetKillScoutCount() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a16", DataCenter.Save().GetKillRaiderCount() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a17", DataCenter.Save().GetKillCommandoCount() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a18", DataCenter.Save().GetKillSniperCount() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a19", DataCenter.Save().GetTotalTime() / 18000 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a20", DataCenter.Save().GetPlayTimes() / 100 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a21", (DataCenter.Save().GetUnlockWeaponCount() - 10) / 20 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a22", DataCenter.Save().GetUnlockArmorCount() / 15 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a23", DataCenter.Save().GetUnlockItemCount() / 6 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a24", DataCenter.Save().GetItemUseCount() / 150 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a25", DataCenter.Save().GetEnemyKill() / 1500 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a26", DataCenter.Save().GetEnemyHeadShot() / 300 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a27", DataCenter.Save().GetDead() / 100 * 100);
		GameCenterPlugin.SubmitAchievement("com.trinitigame.warcorpsgenesis.a28", DataCenter.Save().GetFireBullet() / 15000 * 100);
	}
}

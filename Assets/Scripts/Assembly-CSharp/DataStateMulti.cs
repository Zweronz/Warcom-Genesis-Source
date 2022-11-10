using System.Collections.Generic;
using TNetSdk;

public class DataStateMulti
{
	public class MissionTempData
	{
		public int emenyKill;

		public int emenyKillHeadShot;

		public int weaponSniperKill;

		public int weaponKnifeKill;

		public int weaponHandgunKill;

		public int weaponAssaultRifleKill;

		public int weaponSubmachineGunKill;

		public int weaponShotgunKill;

		public int weaponRPGKill;

		public int killScoutCount;

		public int killSniperCount;

		public int killCommandoCount;

		public int killRaiderCount;

		public int leechKill;

		public int itemUseCount;

		public int fireBullet;

		public int hitBullet;

		public int dead;

		public int deadHeadShot;

		public int expBag;

		public int moneyBag;

		public float time;

		public List<int> medalList = new List<int>();

		public int expMedal;

		public int moneyMedal;

		public int buyAmmoMoney;

		public void Reset()
		{
			emenyKill = 0;
			emenyKillHeadShot = 0;
			weaponSniperKill = 0;
			weaponKnifeKill = 0;
			weaponHandgunKill = 0;
			weaponAssaultRifleKill = 0;
			weaponSubmachineGunKill = 0;
			weaponShotgunKill = 0;
			weaponRPGKill = 0;
			killScoutCount = 0;
			killSniperCount = 0;
			killCommandoCount = 0;
			killRaiderCount = 0;
			leechKill = 0;
			itemUseCount = 0;
			fireBullet = 0;
			hitBullet = 0;
			dead = 0;
			deadHeadShot = 0;
			expBag = 0;
			moneyBag = 0;
			time = 0f;
			medalList.Clear();
			expMedal = 0;
			moneyMedal = 0;
			buyAmmoMoney = 0;
		}
	}

	public TNetObject m_tnet;

	public bool m_isRoomMaster;

	private string IP = "192.168.0.190";

	private int PORT = 7005;

	private string m_userName;

	private bool m_tnetConnect;

	private bool m_tnetLogin;

	private int m_charactorIndex;

	private string m_modelName;

	private LevelScene m_scene;

	private LevelConfDeathMatch m_deathMatchConf;

	public MissionTempData m_missionTempData;

	public DataStateMulti()
	{
		m_charactorIndex = 1;
		m_missionTempData = new MissionTempData();
	}

	public int GetCurrentCharactor()
	{
		return m_charactorIndex;
	}

	public void SetCurrentCharactor(int index)
	{
		m_charactorIndex = index;
	}

	public string GetCurrentModelName()
	{
		return m_modelName;
	}

	public void SetCurrentModelName(string modelName)
	{
		m_modelName = modelName;
	}

	public LevelScene GetCurrentScene()
	{
		return m_scene;
	}

	public void SetCurrentScene(LevelScene scene)
	{
		m_scene = scene;
	}

	public void SetLevelConfDeathMatch(LevelConfDeathMatch deathMatchConf)
	{
		m_deathMatchConf = deathMatchConf;
	}

	public LevelConfDeathMatch GetLevelConfDeathMatch()
	{
		return m_deathMatchConf;
	}

	public void SetUserName(string userName)
	{
		m_userName = userName;
	}

	public string GetUserName()
	{
		return m_userName;
	}

	public bool IsConnect()
	{
		return m_tnetConnect;
	}

	public bool IsLogin()
	{
		return m_tnetLogin;
	}

	public void SetIP(string ip)
	{
		IP = ip;
	}

	public void Connect()
	{
		m_tnet.Connect(IP, PORT);
	}

	public void ConnectSuccess()
	{
		m_tnetConnect = true;
	}

	public void ConnectLost()
	{
		m_tnetLogin = false;
		m_tnetConnect = false;
		if (m_tnet != null)
		{
			m_tnet.RemoveAllEventListeners();
		}
		m_tnet = null;
	}

	public void ManualDisconnect()
	{
		if (m_tnet != null)
		{
			m_tnet.Close();
			ConnectLost();
		}
	}

	public void Login()
	{
		if (m_tnet != null)
		{
			m_tnet.Send(new LoginRequest(m_userName));
		}
	}

	public void LoginSuccess()
	{
		m_tnetLogin = true;
	}

	private void OnApplicationQuit()
	{
		ManualDisconnect();
	}
}

using System.Collections.Generic;

public class DataStateSingle
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

		public int treasureBox;

		public int expBag;

		public int moneyBag;

		public float completeness;

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
			treasureBox = 0;
			expBag = 0;
			moneyBag = 0;
			completeness = 0f;
			time = 0f;
			medalList.Clear();
			expMedal = 0;
			moneyMedal = 0;
			buyAmmoMoney = 0;
		}
	}

	private LevelInfo m_levelInfo;

	private int m_charactorIndex;

	private LevelConfFight m_fightConf;

	private LevelConfFind m_findConf;

	private LevelConfSurvive m_surviveConf;

	public MissionTempData m_missionTempData;

	public int sideMissionWeek;

	public DataStateSingle()
	{
		m_levelInfo = null;
		m_charactorIndex = 1;
		m_fightConf = null;
		m_findConf = null;
		m_surviveConf = null;
		m_missionTempData = new MissionTempData();
	}

	public void SetLevelInfo(LevelInfo levelInfo)
	{
		m_levelInfo = levelInfo;
	}

	public LevelInfo GetLevelInfo()
	{
		return m_levelInfo;
	}

	public int GetCurrentCharactor()
	{
		return m_charactorIndex;
	}

	public void SetCurrentCharactor(int index)
	{
		m_charactorIndex = index;
	}

	public void SetLevelConfFight(LevelConfFight fightConf)
	{
		m_fightConf = fightConf;
	}

	public LevelConfFight GetLevelConfFight()
	{
		return m_fightConf;
	}

	public void SetLevelConfFind(LevelConfFind findConf)
	{
		m_findConf = findConf;
	}

	public LevelConfFind GetLevelConfFind()
	{
		return m_findConf;
	}

	public void SetLevelConfSurvive(LevelConfSurvive surviveConf)
	{
		m_surviveConf = surviveConf;
	}

	public LevelConfSurvive GetLevelConfSurvive()
	{
		return m_surviveConf;
	}
}

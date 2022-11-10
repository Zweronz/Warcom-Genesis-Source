using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;

public class DataSave
{
	private int m_optionMusic;

	private int m_optionSound;

	private int m_optionJoystick;

	private bool m_worldToturialShow;

	private int m_week;

	private int m_money;

	private int m_crystal;

	private int m_teamLv;

	private int m_teamExp;

	private List<LevelInfo> m_levels = new List<LevelInfo>();

	private Dictionary<int, CharacterEquipInfo> m_characterEquipInfoMap = new Dictionary<int, CharacterEquipInfo>();

	private int m_defaultCharacter;

	private Dictionary<string, int> m_weaponMap = new Dictionary<string, int>();

	private Dictionary<string, int> m_armorMap = new Dictionary<string, int>();

	private Dictionary<string, int> m_itemMap = new Dictionary<string, int>();

	private Dictionary<int, string> m_medalMap = new Dictionary<int, string>();

	public DataSave()
	{
		if (!Load())
		{
			Deafult();
		}
	}

	public int GetOptionMusic()
	{
		return m_optionMusic;
	}

	public void SetOptionMusic(int music)
	{
		m_optionMusic = music;
	}

	public int GetOptionSound()
	{
		return m_optionSound;
	}

	public void SetOptionSound(int sound)
	{
		m_optionSound = sound;
	}

	public int GetOptionJoystick()
	{
		return m_optionJoystick;
	}

	public void SetOptionJoystick(int joystick)
	{
		m_optionJoystick = joystick;
	}

	public void SetWorldMapToturialShow(bool isShow)
	{
		m_worldToturialShow = isShow;
	}

	public bool GetWorldMapToturialShow()
	{
		return m_worldToturialShow;
	}

	public int GetWeek()
	{
		return m_week;
	}

	public void SetWeek(int week)
	{
		m_week = week;
	}

	public int GetExp()
	{
		return m_teamExp;
	}

	public void SetExp(int exp)
	{
		m_teamExp = Mathf.Min(exp, DataCenter.Conf().GetLevelUpExpByLv(DataCenter.Conf().GetLevelUpExpMap().Count).totalExp);
	}

	public int GetLv()
	{
		return m_teamLv;
	}

	public void SetLv(int lv)
	{
		m_teamLv = lv;
	}

	public int GetMoney()
	{
		return m_money;
	}

	public void SetMoney(int money)
	{
		m_money = money;
	}

	public int GetCrystal()
	{
		return m_crystal;
	}

	public void SetCrystal(int crystal)
	{
		m_crystal = crystal;
	}

	public int GetEnemyKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.enemyKill;
		}
		return num;
	}

	public int GetEnemyHeadShot()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.enemyKillHeadShot;
		}
		return num;
	}

	public int GetTotalTime()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.totalTime;
		}
		return num;
	}

	public int GetGetMoney()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.getMoney;
		}
		return num;
	}

	public int GetUseMoney()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.useMoney;
		}
		return num;
	}

	public int GetPlayTimes()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.playTimes;
		}
		return num;
	}

	public int GetMainMissionComplete()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.mainMissionComplete;
		}
		return num;
	}

	public int GetAuxiliaryMissionComplete()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.auxiliaryMissionComplete;
		}
		return num;
	}

	public int GetWeaponKnifeKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.weaponKnifeKill;
		}
		return num;
	}

	public int GetWeaponHandgunKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.weaponHandgunKill;
		}
		return num;
	}

	public int GetWeaponAssaultRifleKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.weaponAssaultRifleKill;
		}
		return num;
	}

	public int GetWeaponSubmachineGunKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.weaponSubmachineGunKill;
		}
		return num;
	}

	public int GetWeaponShotgunGunKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.weaponShotgunKill;
		}
		return num;
	}

	public int GetWeaponSniperKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.weaponSniperKill;
		}
		return num;
	}

	public int GetWeaponRPGKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.weaponRPGKill;
		}
		return num;
	}

	public int GetKillScoutCount()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.killScoutCount;
		}
		return num;
	}

	public int GetKillSniperCount()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.killSniperCount;
		}
		return num;
	}

	public int GetKillCommandoCount()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.killCommandoCount;
		}
		return num;
	}

	public int GetKillRaiderCount()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.killRaiderCount;
		}
		return num;
	}

	public int GetLeechKill()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.leechKill;
		}
		return num;
	}

	public int GetDead()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.dead;
		}
		return num;
	}

	public int GetDeadHeadShot()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.deadHeadShot;
		}
		return num;
	}

	public int GetItemUseCount()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.itemUseCount;
		}
		return num;
	}

	public int GetFireBullet()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.fireBullet;
		}
		return num;
	}

	public int GetHitBullet()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			num += item.Value.hitBullet;
		}
		return num;
	}

	public LevelInfo[] GetLevels()
	{
		return m_levels.ToArray();
	}

	public LevelInfo GetLastBoneLevel()
	{
		if (m_levels != null)
		{
			if (m_levels.Count == 0)
			{
				return null;
			}
			return m_levels[m_levels.Count - 1];
		}
		return null;
	}

	public void AddLevel(LevelInfo boneLevel)
	{
		m_levels.Add(boneLevel);
	}

	public bool IsCharacterUnlock(int index)
	{
		return m_characterEquipInfoMap[index].unlocked;
	}

	public void UnlockCharacter(int index)
	{
		m_characterEquipInfoMap[index].unlocked = true;
	}

	public int GetCharacterUnlockCount()
	{
		int num = 0;
		for (int i = 0; i < m_characterEquipInfoMap.Count; i++)
		{
			if (m_characterEquipInfoMap[i + 1].unlocked)
			{
				num++;
			}
		}
		return num;
	}

	public CharacterEquipInfo GetCharacterEquipInfo(int index)
	{
		return m_characterEquipInfoMap[index];
	}

	public int GetDefaultCharacter()
	{
		return m_defaultCharacter;
	}

	public void SetDefaultCharacter(int index)
	{
		m_defaultCharacter = index;
	}

	public bool IsWeaponEquiped(int index, string name)
	{
		bool result = false;
		if (m_characterEquipInfoMap[index].weapon01 == name)
		{
			result = true;
		}
		else if (m_characterEquipInfoMap[index].weapon02 == name)
		{
			result = true;
		}
		else if (m_characterEquipInfoMap[index].weapon03 == name)
		{
			result = true;
		}
		return result;
	}

	public bool IsWeaponTypeEquiped(int index, WeaponType type)
	{
		bool result = false;
		if (DataCenter.Conf().GetWeaponInfoByName(m_characterEquipInfoMap[index].weapon01).type == type)
		{
			result = true;
		}
		else if (DataCenter.Conf().GetWeaponInfoByName(m_characterEquipInfoMap[index].weapon02).type == type)
		{
			result = true;
		}
		else if (DataCenter.Conf().GetWeaponInfoByName(m_characterEquipInfoMap[index].weapon03).type == type)
		{
			result = true;
		}
		return result;
	}

	public void EquipWeapon(int index, string name)
	{
		Regex regex = new Regex("[0-9][0-9]_tCrystal|[0-9][0-9]");
		if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon01, string.Empty))
		{
			m_characterEquipInfoMap[index].weapon01 = name;
		}
		else if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon02, string.Empty))
		{
			m_characterEquipInfoMap[index].weapon02 = name;
		}
		else if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon03, string.Empty))
		{
			m_characterEquipInfoMap[index].weapon03 = name;
		}
		else if (m_characterEquipInfoMap[index].weapon01 == string.Empty)
		{
			m_characterEquipInfoMap[index].weapon01 = name;
		}
		else if (m_characterEquipInfoMap[index].weapon02 == string.Empty)
		{
			m_characterEquipInfoMap[index].weapon02 = name;
		}
		else if (m_characterEquipInfoMap[index].weapon03 == string.Empty)
		{
			m_characterEquipInfoMap[index].weapon03 = name;
		}
	}

	public void EquipWeaponInPos(int index, string name, int pos)
	{
		Regex regex = new Regex("[0-9][0-9]");
		switch (pos)
		{
		case 1:
			if (regex.Replace(name, string.Empty) != regex.Replace(m_characterEquipInfoMap[index].weapon01, string.Empty))
			{
				if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon02, string.Empty))
				{
					m_characterEquipInfoMap[index].weapon02 = m_characterEquipInfoMap[index].weapon01;
				}
				else if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon03, string.Empty))
				{
					m_characterEquipInfoMap[index].weapon03 = m_characterEquipInfoMap[index].weapon01;
				}
			}
			m_characterEquipInfoMap[index].weapon01 = name;
			break;
		case 2:
			if (regex.Replace(name, string.Empty) != regex.Replace(m_characterEquipInfoMap[index].weapon02, string.Empty))
			{
				if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon01, string.Empty))
				{
					m_characterEquipInfoMap[index].weapon01 = m_characterEquipInfoMap[index].weapon02;
				}
				else if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon03, string.Empty))
				{
					m_characterEquipInfoMap[index].weapon03 = m_characterEquipInfoMap[index].weapon02;
				}
			}
			m_characterEquipInfoMap[index].weapon02 = name;
			break;
		case 3:
			if (regex.Replace(name, string.Empty) != regex.Replace(m_characterEquipInfoMap[index].weapon03, string.Empty))
			{
				if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon01, string.Empty))
				{
					m_characterEquipInfoMap[index].weapon01 = m_characterEquipInfoMap[index].weapon03;
				}
				else if (regex.Replace(name, string.Empty) == regex.Replace(m_characterEquipInfoMap[index].weapon02, string.Empty))
				{
					m_characterEquipInfoMap[index].weapon02 = m_characterEquipInfoMap[index].weapon03;
				}
			}
			m_characterEquipInfoMap[index].weapon03 = name;
			break;
		}
	}

	public void UnequipWeapon(int index, string name)
	{
		if (m_characterEquipInfoMap[index].weapon01 == name)
		{
			m_characterEquipInfoMap[index].weapon01 = string.Empty;
		}
		if (m_characterEquipInfoMap[index].weapon02 == name)
		{
			m_characterEquipInfoMap[index].weapon02 = string.Empty;
		}
		if (m_characterEquipInfoMap[index].weapon03 == name)
		{
			m_characterEquipInfoMap[index].weapon03 = string.Empty;
		}
	}

	public bool IsArmorEquiped(int index, string name)
	{
		bool result = false;
		if (m_characterEquipInfoMap[index].armor01 == name)
		{
			result = true;
		}
		else if (m_characterEquipInfoMap[index].armor02 == name)
		{
			result = true;
		}
		else if (m_characterEquipInfoMap[index].armor03 == name)
		{
			result = true;
		}
		return result;
	}

	public void EquipArmor(int index, string name)
	{
		if (m_characterEquipInfoMap[index].armor01 == string.Empty)
		{
			m_characterEquipInfoMap[index].armor01 = name;
		}
		else if (m_characterEquipInfoMap[index].armor02 == string.Empty)
		{
			m_characterEquipInfoMap[index].armor02 = name;
		}
		else
		{
			m_characterEquipInfoMap[index].armor03 = name;
		}
	}

	public void EquipArmorInPos(int index, string name, int pos)
	{
		switch (pos)
		{
		case 1:
			m_characterEquipInfoMap[index].armor01 = name;
			if (m_characterEquipInfoMap[index].armor02 == name)
			{
				m_characterEquipInfoMap[index].armor02 = string.Empty;
			}
			else if (m_characterEquipInfoMap[index].armor03 == name)
			{
				m_characterEquipInfoMap[index].armor03 = string.Empty;
			}
			break;
		case 2:
			m_characterEquipInfoMap[index].armor02 = name;
			if (m_characterEquipInfoMap[index].armor01 == name)
			{
				m_characterEquipInfoMap[index].armor01 = string.Empty;
			}
			else if (m_characterEquipInfoMap[index].armor03 == name)
			{
				m_characterEquipInfoMap[index].armor03 = string.Empty;
			}
			break;
		case 3:
			m_characterEquipInfoMap[index].armor03 = name;
			if (m_characterEquipInfoMap[index].armor01 == name)
			{
				m_characterEquipInfoMap[index].armor01 = string.Empty;
			}
			else if (m_characterEquipInfoMap[index].armor02 == name)
			{
				m_characterEquipInfoMap[index].armor02 = string.Empty;
			}
			break;
		}
	}

	public void UnequipArmor(int index, string name)
	{
		if (m_characterEquipInfoMap[index].armor01 == name)
		{
			m_characterEquipInfoMap[index].armor01 = string.Empty;
		}
		if (m_characterEquipInfoMap[index].armor02 == name)
		{
			m_characterEquipInfoMap[index].armor02 = string.Empty;
		}
		if (m_characterEquipInfoMap[index].armor03 == name)
		{
			m_characterEquipInfoMap[index].armor03 = string.Empty;
		}
	}

	public bool IsItemEquiped(int index, string name)
	{
		bool result = false;
		if (m_characterEquipInfoMap[index].item01 == name)
		{
			result = true;
		}
		else if (m_characterEquipInfoMap[index].item02 == name)
		{
			result = true;
		}
		else if (m_characterEquipInfoMap[index].item03 == name)
		{
			result = true;
		}
		return result;
	}

	public void EquipItem(int index, string name)
	{
		if (m_characterEquipInfoMap[index].item01 == string.Empty)
		{
			m_characterEquipInfoMap[index].item01 = name;
		}
		else if (m_characterEquipInfoMap[index].item02 == string.Empty)
		{
			m_characterEquipInfoMap[index].item02 = name;
		}
		else
		{
			m_characterEquipInfoMap[index].item03 = name;
		}
	}

	public void EquipItemInPos(int index, string name, int pos)
	{
		switch (pos)
		{
		case 1:
			m_characterEquipInfoMap[index].item01 = name;
			if (m_characterEquipInfoMap[index].item02 == name)
			{
				m_characterEquipInfoMap[index].item02 = string.Empty;
			}
			else if (m_characterEquipInfoMap[index].item03 == name)
			{
				m_characterEquipInfoMap[index].item03 = string.Empty;
			}
			break;
		case 2:
			m_characterEquipInfoMap[index].item02 = name;
			if (m_characterEquipInfoMap[index].item01 == name)
			{
				m_characterEquipInfoMap[index].item01 = string.Empty;
			}
			else if (m_characterEquipInfoMap[index].item03 == name)
			{
				m_characterEquipInfoMap[index].item03 = string.Empty;
			}
			break;
		case 3:
			m_characterEquipInfoMap[index].item03 = name;
			if (m_characterEquipInfoMap[index].item01 == name)
			{
				m_characterEquipInfoMap[index].item01 = string.Empty;
			}
			else if (m_characterEquipInfoMap[index].item02 == name)
			{
				m_characterEquipInfoMap[index].item02 = string.Empty;
			}
			break;
		}
	}

	public void UnequipItem(int index, string name)
	{
		if (m_characterEquipInfoMap[index].item01 == name)
		{
			m_characterEquipInfoMap[index].item01 = string.Empty;
			Debug.Log("CharactorId:" + index + ",item01" + name);
		}
		if (m_characterEquipInfoMap[index].item02 == name)
		{
			m_characterEquipInfoMap[index].item02 = string.Empty;
			Debug.Log("CharactorId:" + index + ",item02" + name);
		}
		if (m_characterEquipInfoMap[index].item03 == name)
		{
			m_characterEquipInfoMap[index].item03 = string.Empty;
			Debug.Log("CharactorId:" + index + ",item03" + name);
		}
	}

	public void UnequipItemAll(string name)
	{
		for (int i = 1; i <= 4; i++)
		{
			if (m_characterEquipInfoMap[i].item01 == name)
			{
				m_characterEquipInfoMap[i].item01 = string.Empty;
			}
			if (m_characterEquipInfoMap[i].item02 == name)
			{
				m_characterEquipInfoMap[i].item02 = string.Empty;
			}
			if (m_characterEquipInfoMap[i].item03 == name)
			{
				m_characterEquipInfoMap[i].item03 = string.Empty;
			}
		}
	}

	public bool IsWeaponUnlock(string name)
	{
		return m_weaponMap.ContainsKey(name);
	}

	public void UnlockWeapon(string name)
	{
		m_weaponMap.Add(name, 100);
	}

	public int GetWeaponDur(string name)
	{
		return m_weaponMap[name];
	}

	public void SetWeaponDur(string name, int dur)
	{
		m_weaponMap[name] = dur;
	}

	public int GetUnlockWeaponCount()
	{
		return m_weaponMap.Count;
	}

	public string GetUnlockWeaponName()
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, int> item in m_weaponMap)
		{
			text = text + item.Key + "|";
		}
		return text;
	}

	public bool IsArmorUnlock(string name)
	{
		return m_armorMap.ContainsKey(name);
	}

	public void UnlockArmor(string name, int times)
	{
		m_armorMap.Add(name, times);
	}

	public int GetArmorNums(string name)
	{
		return m_armorMap[name];
	}

	public void SetArmorTimes(string name, int times)
	{
		m_armorMap[name] = times;
	}

	public int GetUnlockArmorCount()
	{
		return m_armorMap.Count;
	}

	public string GetUnlockArmorName()
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, int> item in m_armorMap)
		{
			text = text + item.Key + "|";
		}
		return text;
	}

	public bool IsItemUnlock(string name)
	{
		return m_itemMap.ContainsKey(name);
	}

	public void UnlockItem(string name, int nums)
	{
		m_itemMap.Add(name, nums);
	}

	public int GetItemNums(string name)
	{
		return m_itemMap[name];
	}

	public void SetItemNums(string name, int nums)
	{
		m_itemMap[name] = nums;
	}

	public int GetUnlockItemCount()
	{
		return m_itemMap.Count;
	}

	public bool IsMedalUnlock(int index)
	{
		return m_medalMap.ContainsKey(index);
	}

	public void UnlockMedal(int index, string conditionName)
	{
		m_medalMap.Add(index, conditionName);
	}

	public void Save()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "no"));
		XmlElement xmlElement = xmlDocument.CreateElement("Save");
		xmlElement.SetAttribute("Version", "1.0");
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("Options");
		xmlElement2.SetAttribute("Music", m_optionMusic.ToString());
		xmlElement2.SetAttribute("Sound", m_optionSound.ToString());
		xmlElement2.SetAttribute("Joystick", m_optionJoystick.ToString());
		xmlElement.AppendChild(xmlElement2);
		XmlElement xmlElement3 = xmlDocument.CreateElement("Property");
		xmlElement3.SetAttribute("Week", m_week.ToString());
		xmlElement3.SetAttribute("Money", m_money.ToString());
		xmlElement3.SetAttribute("Crystal", m_crystal.ToString());
		xmlElement3.SetAttribute("TeamLv", m_teamLv.ToString());
		xmlElement3.SetAttribute("TeamExp", m_teamExp.ToString());
		xmlElement3.SetAttribute("WorldTutorialShow", m_worldToturialShow.ToString());
		xmlElement.AppendChild(xmlElement3);
		XmlElement xmlElement4 = xmlDocument.CreateElement("LevelInfos");
		if (m_levels != null)
		{
			foreach (LevelInfo level in m_levels)
			{
				XmlElement xmlElement5 = xmlDocument.CreateElement("LevelInfo");
				int type = (int)level.type;
				xmlElement5.SetAttribute("Type", type.ToString());
				int mode = (int)level.mode;
				xmlElement5.SetAttribute("Mode", mode.ToString());
				xmlElement5.SetAttribute("Id", level.id.ToString());
				int scene = (int)level.scene;
				xmlElement5.SetAttribute("Scene", scene.ToString());
				xmlElement5.SetAttribute("Pass", level.pass.ToString());
				xmlElement5.SetAttribute("EnemyKind", level.enemyKind.ToString());
				xmlElement5.SetAttribute("TextId", level.textId.ToString());
				xmlElement5.SetAttribute("WeekFluctuation", level.weekFluctuation.ToString());
				xmlElement4.AppendChild(xmlElement5);
			}
		}
		xmlElement.AppendChild(xmlElement4);
		XmlElement xmlElement6 = xmlDocument.CreateElement("CharacterEquipInfos");
		xmlElement6.SetAttribute("Default", m_defaultCharacter.ToString());
		foreach (KeyValuePair<int, CharacterEquipInfo> item in m_characterEquipInfoMap)
		{
			XmlElement xmlElement7 = xmlDocument.CreateElement("CharacterEquipInfo");
			xmlElement7.SetAttribute("Index", item.Key.ToString());
			xmlElement7.SetAttribute("Name", item.Value.name);
			xmlElement7.SetAttribute("Weapon01", item.Value.weapon01);
			xmlElement7.SetAttribute("Weapon02", item.Value.weapon02);
			xmlElement7.SetAttribute("Weapon03", item.Value.weapon03);
			xmlElement7.SetAttribute("Armor01", item.Value.armor01);
			xmlElement7.SetAttribute("Armor02", item.Value.armor02);
			xmlElement7.SetAttribute("Armor03", item.Value.armor03);
			xmlElement7.SetAttribute("Item01", item.Value.item01);
			xmlElement7.SetAttribute("Item02", item.Value.item02);
			xmlElement7.SetAttribute("Item03", item.Value.item03);
			xmlElement7.SetAttribute("Unlocked", item.Value.unlocked.ToString());
			xmlElement7.SetAttribute("TotalTime", item.Value.totalTime.ToString());
			xmlElement7.SetAttribute("GetMoney", item.Value.getMoney.ToString());
			xmlElement7.SetAttribute("UseMoney", item.Value.useMoney.ToString());
			xmlElement7.SetAttribute("PlayTimes", item.Value.playTimes.ToString());
			xmlElement7.SetAttribute("MainMissionComplete", item.Value.mainMissionComplete.ToString());
			xmlElement7.SetAttribute("AuxiliaryMissionComplete", item.Value.auxiliaryMissionComplete.ToString());
			xmlElement7.SetAttribute("EmenyKill", item.Value.enemyKill.ToString());
			xmlElement7.SetAttribute("EmenyKillHeadShot", item.Value.enemyKillHeadShot.ToString());
			xmlElement7.SetAttribute("WeaponKnifeKill", item.Value.weaponKnifeKill.ToString());
			xmlElement7.SetAttribute("WeaponHandgunKill", item.Value.weaponHandgunKill.ToString());
			xmlElement7.SetAttribute("WeaponAssaultRifleKill", item.Value.weaponAssaultRifleKill.ToString());
			xmlElement7.SetAttribute("WeaponSubmachineGunKill", item.Value.weaponSubmachineGunKill.ToString());
			xmlElement7.SetAttribute("WeaponShotgunKill", item.Value.weaponShotgunKill.ToString());
			xmlElement7.SetAttribute("WeaponSniperKill", item.Value.weaponSniperKill.ToString());
			xmlElement7.SetAttribute("WeaponRPGKill", item.Value.weaponRPGKill.ToString());
			xmlElement7.SetAttribute("KillScoutCount", item.Value.killScoutCount.ToString());
			xmlElement7.SetAttribute("KillSniperCount", item.Value.killSniperCount.ToString());
			xmlElement7.SetAttribute("KillCommandoCount", item.Value.killCommandoCount.ToString());
			xmlElement7.SetAttribute("KillRaiderCount", item.Value.killRaiderCount.ToString());
			xmlElement7.SetAttribute("LeechKill", item.Value.leechKill.ToString());
			xmlElement7.SetAttribute("Dead", item.Value.dead.ToString());
			xmlElement7.SetAttribute("DeadHeadShot", item.Value.deadHeadShot.ToString());
			xmlElement7.SetAttribute("ItemUseCount", item.Value.itemUseCount.ToString());
			xmlElement7.SetAttribute("FireBullet", item.Value.fireBullet.ToString());
			xmlElement7.SetAttribute("HitBullet", item.Value.hitBullet.ToString());
			xmlElement6.AppendChild(xmlElement7);
		}
		xmlElement.AppendChild(xmlElement6);
		XmlElement xmlElement8 = xmlDocument.CreateElement("Weapons");
		foreach (KeyValuePair<string, int> item2 in m_weaponMap)
		{
			XmlElement xmlElement9 = xmlDocument.CreateElement("Weapon");
			xmlElement9.SetAttribute("Name", item2.Key);
			xmlElement9.SetAttribute("Dur", item2.Value.ToString());
			xmlElement8.AppendChild(xmlElement9);
		}
		xmlElement.AppendChild(xmlElement8);
		XmlElement xmlElement10 = xmlDocument.CreateElement("Armors");
		foreach (KeyValuePair<string, int> item3 in m_armorMap)
		{
			XmlElement xmlElement11 = xmlDocument.CreateElement("Armor");
			xmlElement11.SetAttribute("Name", item3.Key);
			xmlElement11.SetAttribute("Times", item3.Value.ToString());
			xmlElement10.AppendChild(xmlElement11);
		}
		xmlElement.AppendChild(xmlElement10);
		XmlElement xmlElement12 = xmlDocument.CreateElement("Items");
		foreach (KeyValuePair<string, int> item4 in m_itemMap)
		{
			XmlElement xmlElement13 = xmlDocument.CreateElement("Item");
			xmlElement13.SetAttribute("Name", item4.Key);
			xmlElement13.SetAttribute("Nums", item4.Value.ToString());
			xmlElement12.AppendChild(xmlElement13);
		}
		xmlElement.AppendChild(xmlElement12);
		XmlElement xmlElement14 = xmlDocument.CreateElement("Medals");
		foreach (KeyValuePair<int, string> item5 in m_medalMap)
		{
			XmlElement xmlElement15 = xmlDocument.CreateElement("Medal");
			xmlElement15.SetAttribute("Index", item5.Key.ToString());
			xmlElement15.SetAttribute("MedalConditionName", item5.Value);
			xmlElement14.AppendChild(xmlElement15);
		}
		xmlElement.AppendChild(xmlElement14);
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.NewLineChars = "\r\n";
		xmlWriterSettings.Indent = true;
		xmlWriterSettings.IndentChars = "\t";
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
		xmlDocument.Save(xmlWriter);
		FileUtil.WriteSave("Save2.xml", Encrypt.Encode2(stringBuilder.ToString()));
	}

	private bool Load()
	{
		//Discarded unreachable code: IL_08cf
		string empty = string.Empty;
		empty = FileUtil.ReadSave("Save2.xml");
		if (empty.Length > 0)
		{
			empty = Encrypt.Decode2(empty);
		}
		if (empty.Length <= 0)
		{
			empty = FileUtil.ReadSave("Save.xml");
			if (empty.Length > 0)
			{
				empty = Encrypt.Decode(empty);
			}
		}
		if (empty.Length <= 0)
		{
			return false;
		}
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(empty);
			XmlElement documentElement = xmlDocument.DocumentElement;
			XmlElement xmlElement = (XmlElement)documentElement.GetElementsByTagName("Options").Item(0);
			m_optionMusic = int.Parse(xmlElement.GetAttribute("Music"));
			m_optionSound = int.Parse(xmlElement.GetAttribute("Sound"));
			m_optionJoystick = int.Parse(xmlElement.GetAttribute("Joystick"));
			XmlElement xmlElement2 = (XmlElement)documentElement.GetElementsByTagName("Property").Item(0);
			m_week = int.Parse(xmlElement2.GetAttribute("Week"));
			m_money = int.Parse(xmlElement2.GetAttribute("Money"));
			m_crystal = int.Parse(xmlElement2.GetAttribute("Crystal"));
			m_teamLv = int.Parse(xmlElement2.GetAttribute("TeamLv"));
			m_teamExp = int.Parse(xmlElement2.GetAttribute("TeamExp"));
			m_worldToturialShow = bool.Parse(xmlElement2.GetAttribute("WorldTutorialShow"));
			XmlNodeList elementsByTagName = ((XmlElement)documentElement.GetElementsByTagName("LevelInfos").Item(0)).GetElementsByTagName("LevelInfo");
			for (int i = 0; i < elementsByTagName.Count; i++)
			{
				LevelInfo levelInfo = new LevelInfo();
				XmlElement xmlElement3 = (XmlElement)elementsByTagName.Item(i);
				levelInfo.type = (LevelType)int.Parse(xmlElement3.GetAttribute("Type"));
				levelInfo.mode = (LevelMode)int.Parse(xmlElement3.GetAttribute("Mode"));
				levelInfo.id = int.Parse(xmlElement3.GetAttribute("Id"));
				levelInfo.scene = (LevelScene)int.Parse(xmlElement3.GetAttribute("Scene"));
				levelInfo.pass = bool.Parse(xmlElement3.GetAttribute("Pass"));
				levelInfo.enemyKind = int.Parse(xmlElement3.GetAttribute("EnemyKind"));
				levelInfo.textId = int.Parse(xmlElement3.GetAttribute("TextId"));
				levelInfo.weekFluctuation = int.Parse(xmlElement3.GetAttribute("WeekFluctuation"));
				m_levels.Add(levelInfo);
			}
			XmlElement xmlElement4 = (XmlElement)documentElement.GetElementsByTagName("CharacterEquipInfos").Item(0);
			m_defaultCharacter = int.Parse(xmlElement4.GetAttribute("Default"));
			foreach (XmlElement item in xmlElement4.GetElementsByTagName("CharacterEquipInfo"))
			{
				int key = int.Parse(item.GetAttribute("Index"));
				CharacterEquipInfo characterEquipInfo = new CharacterEquipInfo();
				characterEquipInfo.name = item.GetAttribute("Name");
				characterEquipInfo.weapon01 = item.GetAttribute("Weapon01");
				characterEquipInfo.weapon02 = item.GetAttribute("Weapon02");
				characterEquipInfo.weapon03 = item.GetAttribute("Weapon03");
				characterEquipInfo.armor01 = item.GetAttribute("Armor01");
				characterEquipInfo.armor02 = item.GetAttribute("Armor02");
				characterEquipInfo.armor03 = item.GetAttribute("Armor03");
				characterEquipInfo.item01 = item.GetAttribute("Item01");
				characterEquipInfo.item02 = item.GetAttribute("Item02");
				characterEquipInfo.item03 = item.GetAttribute("Item03");
				characterEquipInfo.unlocked = bool.Parse(item.GetAttribute("Unlocked"));
				characterEquipInfo.totalTime = int.Parse(item.GetAttribute("TotalTime"));
				characterEquipInfo.getMoney = int.Parse(item.GetAttribute("GetMoney"));
				characterEquipInfo.useMoney = int.Parse(item.GetAttribute("UseMoney"));
				characterEquipInfo.playTimes = int.Parse(item.GetAttribute("PlayTimes"));
				characterEquipInfo.mainMissionComplete = int.Parse(item.GetAttribute("MainMissionComplete"));
				characterEquipInfo.auxiliaryMissionComplete = int.Parse(item.GetAttribute("AuxiliaryMissionComplete"));
				characterEquipInfo.enemyKill = int.Parse(item.GetAttribute("EmenyKill"));
				characterEquipInfo.enemyKillHeadShot = int.Parse(item.GetAttribute("EmenyKillHeadShot"));
				characterEquipInfo.weaponKnifeKill = int.Parse(item.GetAttribute("WeaponKnifeKill"));
				characterEquipInfo.weaponHandgunKill = int.Parse(item.GetAttribute("WeaponHandgunKill"));
				characterEquipInfo.weaponAssaultRifleKill = int.Parse(item.GetAttribute("WeaponAssaultRifleKill"));
				characterEquipInfo.weaponSubmachineGunKill = int.Parse(item.GetAttribute("WeaponSubmachineGunKill"));
				characterEquipInfo.weaponShotgunKill = int.Parse(item.GetAttribute("WeaponShotgunKill"));
				characterEquipInfo.weaponSniperKill = int.Parse(item.GetAttribute("WeaponSniperKill"));
				characterEquipInfo.weaponRPGKill = int.Parse(item.GetAttribute("WeaponRPGKill"));
				characterEquipInfo.killScoutCount = int.Parse(item.GetAttribute("KillScoutCount"));
				characterEquipInfo.killSniperCount = int.Parse(item.GetAttribute("KillSniperCount"));
				characterEquipInfo.killCommandoCount = int.Parse(item.GetAttribute("KillCommandoCount"));
				characterEquipInfo.killRaiderCount = int.Parse(item.GetAttribute("KillRaiderCount"));
				characterEquipInfo.leechKill = int.Parse(item.GetAttribute("LeechKill"));
				characterEquipInfo.dead = int.Parse(item.GetAttribute("Dead"));
				characterEquipInfo.deadHeadShot = int.Parse(item.GetAttribute("DeadHeadShot"));
				characterEquipInfo.itemUseCount = int.Parse(item.GetAttribute("ItemUseCount"));
				characterEquipInfo.fireBullet = int.Parse(item.GetAttribute("FireBullet"));
				characterEquipInfo.hitBullet = int.Parse(item.GetAttribute("HitBullet"));
				m_characterEquipInfoMap.Add(key, characterEquipInfo);
			}
			XmlElement xmlElement6 = (XmlElement)documentElement.GetElementsByTagName("Weapons").Item(0);
			foreach (XmlElement item2 in xmlElement6.GetElementsByTagName("Weapon"))
			{
				string attribute = item2.GetAttribute("Name");
				int value = int.Parse(item2.GetAttribute("Dur"));
				m_weaponMap.Add(attribute, value);
			}
			XmlElement xmlElement8 = (XmlElement)documentElement.GetElementsByTagName("Armors").Item(0);
			foreach (XmlElement item3 in xmlElement8.GetElementsByTagName("Armor"))
			{
				string attribute2 = item3.GetAttribute("Name");
				int value2 = int.Parse(item3.GetAttribute("Times"));
				m_armorMap.Add(attribute2, value2);
			}
			XmlElement xmlElement10 = (XmlElement)documentElement.GetElementsByTagName("Items").Item(0);
			foreach (XmlElement item4 in xmlElement10.GetElementsByTagName("Item"))
			{
				string attribute3 = item4.GetAttribute("Name");
				int value3 = int.Parse(item4.GetAttribute("Nums"));
				m_itemMap.Add(attribute3, value3);
			}
			XmlElement xmlElement12 = (XmlElement)documentElement.GetElementsByTagName("Medals").Item(0);
			foreach (XmlElement item5 in xmlElement12.GetElementsByTagName("Medal"))
			{
				int key2 = int.Parse(item5.GetAttribute("Index"));
				string attribute4 = item5.GetAttribute("MedalConditionName");
				m_medalMap.Add(key2, attribute4);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			return false;
		}
		return true;
	}

	private void Deafult()
	{
		m_optionMusic = 50;
		m_optionSound = 100;
		m_optionJoystick = 50;
		m_worldToturialShow = false;
		m_week = -2;
		m_money = 0;
		m_crystal = 0;
		m_teamExp = 0;
		m_teamLv = 1;
		m_levels.Clear();
		CharacterEquipInfo characterEquipInfo = new CharacterEquipInfo();
		characterEquipInfo.name = "FALCON";
		characterEquipInfo.weapon01 = "W_SubmachineGun00";
		characterEquipInfo.weapon02 = "W_Handgun00";
		characterEquipInfo.weapon03 = "W_Wrench_Knife00";
		characterEquipInfo.unlocked = true;
		CharacterEquipInfo characterEquipInfo2 = new CharacterEquipInfo();
		characterEquipInfo2.name = "OSPREY";
		characterEquipInfo2.weapon01 = "W_SniperRifle00";
		characterEquipInfo2.weapon02 = "W_Handgun00";
		characterEquipInfo2.weapon03 = "W_Ka-Bar_Knife02";
		characterEquipInfo2.unlocked = false;
		CharacterEquipInfo characterEquipInfo3 = new CharacterEquipInfo();
		characterEquipInfo3.name = "HARRIER";
		characterEquipInfo3.weapon01 = "W_Shotgun00";
		characterEquipInfo3.weapon02 = "W_AssaultRifle00";
		characterEquipInfo3.weapon03 = "W_Bayonet_Knife01";
		characterEquipInfo3.unlocked = false;
		CharacterEquipInfo characterEquipInfo4 = new CharacterEquipInfo();
		characterEquipInfo4.name = "CASSOWARY";
		characterEquipInfo4.weapon01 = "W_RPG00";
		characterEquipInfo4.weapon02 = "W_AssaultRifle00";
		characterEquipInfo4.weapon03 = "W_Hatchet_Knife03";
		characterEquipInfo4.unlocked = false;
		m_characterEquipInfoMap.Add(1, characterEquipInfo);
		m_characterEquipInfoMap.Add(2, characterEquipInfo2);
		m_characterEquipInfoMap.Add(3, characterEquipInfo3);
		m_characterEquipInfoMap.Add(4, characterEquipInfo4);
		m_weaponMap.Add("W_SubmachineGun00", 100);
		m_weaponMap.Add("W_Handgun00", 100);
		m_weaponMap.Add("W_Wrench_Knife00", 100);
		m_weaponMap.Add("W_Bayonet_Knife01", 100);
		m_weaponMap.Add("W_Ka-Bar_Knife02", 100);
		m_weaponMap.Add("W_Hatchet_Knife03", 100);
		m_weaponMap.Add("W_SniperRifle00", 100);
		m_weaponMap.Add("W_Shotgun00", 100);
		m_weaponMap.Add("W_RPG00", 100);
		m_weaponMap.Add("W_AssaultRifle00", 100);
		m_defaultCharacter = 1;
		Save();
	}
}

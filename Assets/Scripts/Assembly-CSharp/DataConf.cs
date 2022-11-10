using System;
using System.Collections.Generic;
using System.Xml;

public class DataConf
{
	private class LevelTypeInfo
	{
		public string name;

		public string desc;

		public LevelTypeInfo(string name, string desc)
		{
			this.name = name;
			this.desc = desc;
		}
	}

	private class LevelModeInfo
	{
		public string name;

		public string desc;

		public LevelModeInfo(string name, string desc)
		{
			this.name = name;
			this.desc = desc;
		}
	}

	private class LevelSceneInfo
	{
		public string name;

		public string desc;

		public LevelSceneInfo(string name, string desc)
		{
			this.name = name;
			this.desc = desc;
		}
	}

	public class LevelWeekExpMoney
	{
		public int week;

		public int levelExp;

		public int expBagExp;

		public int levelMoney;

		public int moneyBagMoney;
	}

	public class LevelUpExp
	{
		public int lv;

		public int exp;

		public int totalExp;
	}

	private class LevelMissionInfo
	{
		public int scene;

		public int mode;

		public string[] desc;

		public LevelMissionInfo(int scene, int mode, string[] desc)
		{
			this.scene = scene;
			this.mode = mode;
			this.desc = desc;
		}
	}

	private Dictionary<LevelType, LevelTypeInfo> m_levelTypeInfoMap = new Dictionary<LevelType, LevelTypeInfo>();

	private Dictionary<LevelMode, LevelModeInfo> m_levelModeInfoMap = new Dictionary<LevelMode, LevelModeInfo>();

	private Dictionary<int, string> m_tipsMap = new Dictionary<int, string>();

	private Dictionary<int, LevelMissionInfo> m_levelMissionInfoMap = new Dictionary<int, LevelMissionInfo>();

	private Dictionary<LevelScene, LevelSceneInfo> m_levelSceneInfoMap = new Dictionary<LevelScene, LevelSceneInfo>();

	private Dictionary<int, LevelWeekExpMoney> m_levelWeekExpMoneyMap = new Dictionary<int, LevelWeekExpMoney>();

	private Dictionary<int, LevelUpExp> m_levelUpExpMap = new Dictionary<int, LevelUpExp>();

	private Dictionary<int, CharacterInfo> m_characterInfoMap = new Dictionary<int, CharacterInfo>();

	private Dictionary<string, CharacterInfo> m_characterInfoMapByName = new Dictionary<string, CharacterInfo>();

	private Dictionary<int, EnemyInfo> m_enemyInfoMap = new Dictionary<int, EnemyInfo>();

	private Dictionary<string, EnemyInfo> m_enemyInfoMapByName = new Dictionary<string, EnemyInfo>();

	private EnemyDataCalcParam m_enemyDataCalcParam = new EnemyDataCalcParam();

	private Dictionary<WeaponType, WeaponDamageModifierParam> m_weaponDamageModifierParamMapByType = new Dictionary<WeaponType, WeaponDamageModifierParam>();

	private Dictionary<LevelMode, EnemyShotRateModifiersParam> m_enemyShotRateModifiersParamMapByType = new Dictionary<LevelMode, EnemyShotRateModifiersParam>();

	private Dictionary<int, AIParam> m_aiParamMap = new Dictionary<int, AIParam>();

	private Dictionary<int, CrosshairParam> m_crosshairParam = new Dictionary<int, CrosshairParam>();

	private Dictionary<int, CameraParam> m_cameraParam = new Dictionary<int, CameraParam>();

	private Dictionary<WeaponType, WeaponInfo[]> m_weaponInfoMap = new Dictionary<WeaponType, WeaponInfo[]>();

	private Dictionary<string, WeaponInfo> m_weaponInfoMapByName = new Dictionary<string, WeaponInfo>();

	private Dictionary<ArmorEffectType, ArmorInfo[]> m_armorInfoMap = new Dictionary<ArmorEffectType, ArmorInfo[]>();

	private Dictionary<string, ArmorInfo> m_armorInfoMapByName = new Dictionary<string, ArmorInfo>();

	private List<ArmorInfo> m_armorInfoList = new List<ArmorInfo>();

	private Dictionary<ItemEffectType, ItemInfo[]> m_itemInfoMap = new Dictionary<ItemEffectType, ItemInfo[]>();

	private Dictionary<string, ItemInfo> m_itemInfoMapByName = new Dictionary<string, ItemInfo>();

	private List<ItemInfo> m_itemInfoList = new List<ItemInfo>();

	private Dictionary<int, MedalInfo> m_medalInfoMapByIndex = new Dictionary<int, MedalInfo>();

	private Dictionary<int, MedalInfo> m_medalInfoMapById = new Dictionary<int, MedalInfo>();

	private Dictionary<DropItemEffectType, DropItemInfo[]> m_dropItemInfoMap = new Dictionary<DropItemEffectType, DropItemInfo[]>();

	private Dictionary<string, DropItemInfo> m_dropItemInfoMapByName = new Dictionary<string, DropItemInfo>();

	public DataConf()
	{
		LoadConf();
	}

	public int GetUpgradeExp(int lv)
	{
		return lv * 200;
	}

	public string GetLevelTypeName(LevelType type)
	{
		return m_levelTypeInfoMap[type].name;
	}

	public string GetLevelTypeDesc(LevelType type)
	{
		return m_levelTypeInfoMap[type].desc;
	}

	public string GetLevelModeName(LevelMode mode)
	{
		return m_levelModeInfoMap[mode].name;
	}

	public string GetLevelModeDesc(LevelMode mode)
	{
		return m_levelModeInfoMap[mode].desc;
	}

	public string GetLevelSceneName(LevelScene scene)
	{
		return m_levelSceneInfoMap[scene].name;
	}

	public string GetLevelSceneDesc(LevelScene scene)
	{
		return m_levelSceneInfoMap[scene].desc;
	}

	public string GetTipsById(int id)
	{
		return m_tipsMap[id];
	}

	public int GetTipsCount()
	{
		return m_tipsMap.Count;
	}

	public string[] GetLevelMissionDesc(int scene, int mode)
	{
		int key = scene * 10 + mode;
		return m_levelMissionInfoMap[key].desc;
	}

	public LevelWeekExpMoney GetLevelWeekExpMoneyByWeek(int week)
	{
		return m_levelWeekExpMoneyMap[week];
	}

	public Dictionary<int, LevelUpExp> GetLevelUpExpMap()
	{
		return m_levelUpExpMap;
	}

	public LevelUpExp GetLevelUpExpByLv(int lv)
	{
		return m_levelUpExpMap[lv];
	}

	public CharacterInfo GetCharacterInfo(int index)
	{
		return m_characterInfoMap[index];
	}

	public CharacterInfo GetCharacterInfoByName(string name)
	{
		return m_characterInfoMapByName[name];
	}

	public EnemyInfo GetEnemyInfo(int index)
	{
		return m_enemyInfoMap[index];
	}

	public EnemyInfo GetEnemyInfoByName(string name)
	{
		return m_enemyInfoMapByName[name];
	}

	public EnemyDataCalcParam GetEnemyDataCalcParam()
	{
		return m_enemyDataCalcParam;
	}

	public CrosshairParam GetCrosshairParam(int index)
	{
		return m_crosshairParam[index];
	}

	public CameraParam GetCameraParam(int index)
	{
		return m_cameraParam[index];
	}

	public WeaponDamageModifierParam GetWeaponDamageModifierParamByType(WeaponType weaponType)
	{
		return m_weaponDamageModifierParamMapByType[weaponType];
	}

	public EnemyShotRateModifiersParam GetEnemyShotRateModifiersParamByLevelMode(LevelMode levelMode)
	{
		return m_enemyShotRateModifiersParamMapByType[levelMode];
	}

	public AIParam GetAIParam(int index)
	{
		return m_aiParamMap[index];
	}

	public int GetWeaponCount(WeaponType type)
	{
		return m_weaponInfoMap[type].Length;
	}

	public List<WeaponInfo> GetColdWeaponByCharacter(string name)
	{
		List<WeaponInfo> list = new List<WeaponInfo>();
		for (int i = 0; i < m_weaponInfoMap[WeaponType.Knife].Length; i++)
		{
			if (m_weaponInfoMap[WeaponType.Knife][i].name.Contains(name))
			{
				list.Add(m_weaponInfoMap[WeaponType.Knife][i]);
			}
		}
		return list;
	}

	public WeaponInfo GetWeaponInfo(WeaponType type, int index)
	{
		return m_weaponInfoMap[type][index];
	}

	public WeaponInfo GetWeaponInfoByName(string name)
	{
		return m_weaponInfoMapByName[name];
	}

	public int GetArmorCount(ArmorEffectType type)
	{
		return m_armorInfoMap[type].Length;
	}

	public bool ArmorTypeCheck(ArmorEffectType type)
	{
		return m_armorInfoMap.ContainsKey(type);
	}

	public ArmorInfo GetArmorInfo(ArmorEffectType type, int index)
	{
		return m_armorInfoMap[type][index];
	}

	public List<ArmorInfo> GetArmorInfo()
	{
		return m_armorInfoList;
	}

	public ArmorInfo GetArmorInfoByName(string name)
	{
		return m_armorInfoMapByName[name];
	}

	public int GetItemCount(ItemEffectType type)
	{
		return m_itemInfoMap[type].Length;
	}

	public bool ItemTypeCheck(ItemEffectType type)
	{
		return m_itemInfoMap.ContainsKey(type);
	}

	public ItemInfo GetItemInfo(ItemEffectType type, int index)
	{
		return m_itemInfoMap[type][index];
	}

	public List<ItemInfo> GetItemInfo()
	{
		return m_itemInfoList;
	}

	public ItemInfo GetItemInfoByName(string name)
	{
		return m_itemInfoMapByName[name];
	}

	public int GetDropItemCount(DropItemEffectType type)
	{
		return m_dropItemInfoMap[type].Length;
	}

	public DropItemInfo GetDropItemInfo(DropItemEffectType type, int index)
	{
		return m_dropItemInfoMap[type][index];
	}

	public DropItemInfo GetDropItemInfoByName(string name)
	{
		return m_dropItemInfoMapByName[name];
	}

	public int GetMedalCount()
	{
		return m_medalInfoMapByIndex.Count;
	}

	public MedalInfo GetMedalInfoByIndex(int index)
	{
		return m_medalInfoMapByIndex[index];
	}

	public MedalInfo GetMedalInfoById(int id)
	{
		return m_medalInfoMapById[id];
	}

	private void LoadConf()
	{
		string xml = FileUtil.LoadResourcesFile("Conf/Conf");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement documentElement = xmlDocument.DocumentElement;
		XmlElement xmlElement = (XmlElement)documentElement.GetElementsByTagName("LevelTypes").Item(0);
		foreach (XmlElement item2 in xmlElement.GetElementsByTagName("LevelType"))
		{
			int key = int.Parse(item2.GetAttribute("Type"));
			LevelTypeInfo value = new LevelTypeInfo(item2.GetAttribute("Name"), item2.GetAttribute("Desc"));
			m_levelTypeInfoMap.Add((LevelType)key, value);
		}
		XmlElement xmlElement3 = (XmlElement)documentElement.GetElementsByTagName("LevelModes").Item(0);
		foreach (XmlElement item3 in xmlElement3.GetElementsByTagName("LevelMode"))
		{
			int key2 = int.Parse(item3.GetAttribute("Mode"));
			LevelModeInfo value2 = new LevelModeInfo(item3.GetAttribute("Name"), item3.GetAttribute("Desc"));
			m_levelModeInfoMap.Add((LevelMode)key2, value2);
		}
		XmlElement xmlElement5 = (XmlElement)documentElement.GetElementsByTagName("LevelScenes").Item(0);
		foreach (XmlElement item4 in xmlElement5.GetElementsByTagName("LevelScene"))
		{
			int key3 = int.Parse(item4.GetAttribute("Scene"));
			LevelSceneInfo value3 = new LevelSceneInfo(item4.GetAttribute("Name"), item4.GetAttribute("Desc"));
			m_levelSceneInfoMap.Add((LevelScene)key3, value3);
		}
		XmlElement xmlElement7 = (XmlElement)documentElement.GetElementsByTagName("Tips").Item(0);
		foreach (XmlElement item5 in xmlElement7.GetElementsByTagName("Tip"))
		{
			int key4 = int.Parse(item5.GetAttribute("Id"));
			string value4 = item5.GetAttribute("Text").Replace("\\n", "\n");
			m_tipsMap.Add(key4, value4);
		}
		XmlElement xmlElement9 = (XmlElement)documentElement.GetElementsByTagName("LevelTexts").Item(0);
		foreach (XmlElement item6 in xmlElement9.GetElementsByTagName("LevelText"))
		{
			int num = int.Parse(item6.GetAttribute("Mode"));
			int num2 = int.Parse(item6.GetAttribute("Scene"));
			XmlElement xmlElement11 = (XmlElement)item6.GetElementsByTagName("Descs").Item(0);
			List<string> list = new List<string>();
			foreach (XmlElement item7 in xmlElement11.GetElementsByTagName("Desc"))
			{
				int num3 = int.Parse(item7.GetAttribute("Id"));
				string item = item7.GetAttribute("Text").Replace("\\n", "\n");
				list.Add(item);
			}
			LevelMissionInfo value5 = new LevelMissionInfo(num2, num, list.ToArray());
			m_levelMissionInfoMap.Add(num2 * 10 + num, value5);
		}
		XmlElement xmlElement13 = (XmlElement)documentElement.GetElementsByTagName("LevelWeekExpMoneys").Item(0);
		foreach (XmlElement item8 in xmlElement13.GetElementsByTagName("LevelWeekExpMoney"))
		{
			int num4 = int.Parse(item8.GetAttribute("Week"));
			LevelWeekExpMoney levelWeekExpMoney = new LevelWeekExpMoney();
			levelWeekExpMoney.week = num4;
			levelWeekExpMoney.levelExp = int.Parse(item8.GetAttribute("LevelExp"));
			levelWeekExpMoney.expBagExp = int.Parse(item8.GetAttribute("ExpBagExp"));
			levelWeekExpMoney.levelMoney = int.Parse(item8.GetAttribute("LevelMoney"));
			levelWeekExpMoney.moneyBagMoney = int.Parse(item8.GetAttribute("MoneyBagMoney"));
			m_levelWeekExpMoneyMap.Add(num4, levelWeekExpMoney);
		}
		XmlElement xmlElement15 = (XmlElement)documentElement.GetElementsByTagName("LevelUpExps").Item(0);
		foreach (XmlElement item9 in xmlElement15.GetElementsByTagName("LevelUpExp"))
		{
			int num5 = int.Parse(item9.GetAttribute("Lv"));
			LevelUpExp levelUpExp = new LevelUpExp();
			levelUpExp.lv = num5;
			levelUpExp.exp = int.Parse(item9.GetAttribute("Exp"));
			levelUpExp.totalExp = int.Parse(item9.GetAttribute("TotalExp"));
			m_levelUpExpMap.Add(num5, levelUpExp);
		}
		XmlElement xmlElement17 = (XmlElement)documentElement.GetElementsByTagName("Characters").Item(0);
		foreach (XmlElement item10 in xmlElement17.GetElementsByTagName("Character"))
		{
			int num6 = int.Parse(item10.GetAttribute("Id"));
			string attribute = item10.GetAttribute("Name");
			CharacterInfo characterInfo = new CharacterInfo();
			characterInfo.id = num6;
			characterInfo.name = attribute;
			characterInfo.model = item10.GetAttribute("Model");
			characterInfo.hp = int.Parse(item10.GetAttribute("HP"));
			characterInfo.hpRecoverSpeed = int.Parse(item10.GetAttribute("HPRecoverSpeed"));
			characterInfo.speed = float.Parse(item10.GetAttribute("Speed"));
			m_characterInfoMap.Add(num6, characterInfo);
			m_characterInfoMapByName.Add(attribute, characterInfo);
		}
		XmlElement xmlElement19 = (XmlElement)documentElement.GetElementsByTagName("Enemies").Item(0);
		foreach (XmlElement item11 in xmlElement19.GetElementsByTagName("Enemy"))
		{
			int num7 = int.Parse(item11.GetAttribute("Id"));
			string attribute2 = item11.GetAttribute("Name");
			EnemyInfo enemyInfo = new EnemyInfo();
			enemyInfo.id = num7;
			enemyInfo.name = attribute2;
			enemyInfo.model = item11.GetAttribute("Model");
			enemyInfo.skin = item11.GetAttribute("Skin");
			enemyInfo.weapon = item11.GetAttribute("Weapon");
			m_enemyInfoMap.Add(num7, enemyInfo);
			m_enemyInfoMapByName.Add(attribute2, enemyInfo);
		}
		XmlElement xmlElement21 = (XmlElement)documentElement.GetElementsByTagName("EnemyDataCalc").Item(0);
		m_enemyDataCalcParam.m_playerBaseHp = float.Parse(xmlElement21.GetAttribute("PlayerBaseHp"));
		m_enemyDataCalcParam.m_playerDeadTimeBegin = float.Parse(xmlElement21.GetAttribute("PlayerDeadTimeBegin"));
		m_enemyDataCalcParam.m_playerDeadTimeEnd = float.Parse(xmlElement21.GetAttribute("PlayerDeadTimeEnd"));
		m_enemyDataCalcParam.m_maxEnemyShotRate = float.Parse(xmlElement21.GetAttribute("MaxEnemyShotRate"));
		m_enemyDataCalcParam.m_maxEnemyHeadshotRate = float.Parse(xmlElement21.GetAttribute("MaxEnemyHeadshotRate"));
		m_enemyDataCalcParam.m_maxEnemyInFightCount = int.Parse(xmlElement21.GetAttribute("MaxEnemyInFightCount"));
		XmlElement xmlElement22 = (XmlElement)documentElement.GetElementsByTagName("WeaponDamageModifiers").Item(0);
		foreach (XmlElement item12 in xmlElement22.GetElementsByTagName("WeaponDamageModifier"))
		{
			WeaponDamageModifierParam weaponDamageModifierParam = new WeaponDamageModifierParam();
			WeaponType key5 = (weaponDamageModifierParam.weaponType = (WeaponType)(int)Enum.Parse(typeof(WeaponType), item12.GetAttribute("WeaponType")));
			weaponDamageModifierParam.distance1 = float.Parse(item12.GetAttribute("Distance1"));
			weaponDamageModifierParam.distance2 = float.Parse(item12.GetAttribute("Distance2"));
			weaponDamageModifierParam.damage1 = float.Parse(item12.GetAttribute("Damage1"));
			weaponDamageModifierParam.damage2 = float.Parse(item12.GetAttribute("Damage2"));
			weaponDamageModifierParam.damage3 = float.Parse(item12.GetAttribute("Damage3"));
			m_weaponDamageModifierParamMapByType.Add(key5, weaponDamageModifierParam);
		}
		XmlElement xmlElement24 = (XmlElement)documentElement.GetElementsByTagName("EnemyShotRateModifiers").Item(0);
		foreach (XmlElement item13 in xmlElement24.GetElementsByTagName("EnemyShotRateModifier"))
		{
			EnemyShotRateModifiersParam enemyShotRateModifiersParam = new EnemyShotRateModifiersParam();
			LevelMode key6 = (enemyShotRateModifiersParam.levelMode = (LevelMode)(int)Enum.Parse(typeof(LevelMode), item13.GetAttribute("LevelMode")));
			enemyShotRateModifiersParam.distance1 = float.Parse(item13.GetAttribute("Distance1"));
			enemyShotRateModifiersParam.distance2 = float.Parse(item13.GetAttribute("Distance2"));
			m_enemyShotRateModifiersParamMapByType.Add(key6, enemyShotRateModifiersParam);
		}
		XmlElement xmlElement26 = (XmlElement)documentElement.GetElementsByTagName("AIParams").Item(0);
		foreach (XmlElement item14 in xmlElement26.GetElementsByTagName("AIParam"))
		{
			int num8 = int.Parse(item14.GetAttribute("Id"));
			AIParam aIParam = new AIParam();
			aIParam.id = num8;
			aIParam.state = item14.GetAttribute("State");
			aIParam.detectActiveRadius = float.Parse(item14.GetAttribute("DetectActiveRadius"));
			aIParam.detectActiveAngle = float.Parse(item14.GetAttribute("DetectActiveAngle"));
			aIParam.detectPassiveRadius = float.Parse(item14.GetAttribute("DetectPassiveRadius"));
			aIParam.wanderBorderRadius = float.Parse(item14.GetAttribute("WanderBorderRadius"));
			aIParam.chaseType = item14.GetAttribute("ChaseType");
			aIParam.chaseDistance = float.Parse(item14.GetAttribute("ChaseDistance"));
			aIParam.attackMoveProbability = float.Parse(item14.GetAttribute("AttackMoveProbability"));
			m_aiParamMap.Add(num8, aIParam);
		}
		XmlElement xmlElement28 = (XmlElement)documentElement.GetElementsByTagName("CrosshairParams").Item(0);
		foreach (XmlElement item15 in xmlElement28.GetElementsByTagName("CrosshairParam"))
		{
			int num9 = int.Parse(item15.GetAttribute("Id"));
			CrosshairParam crosshairParam = new CrosshairParam();
			crosshairParam.id = num9;
			crosshairParam.shotPointBox.x = float.Parse(item15.GetAttribute("SizeX"));
			crosshairParam.shotPointBox.y = float.Parse(item15.GetAttribute("SizeY"));
			crosshairParam.time = float.Parse(item15.GetAttribute("Time"));
			crosshairParam.maxChangeScale = float.Parse(item15.GetAttribute("MaxChangeScale"));
			crosshairParam.oneShotAdd = float.Parse(item15.GetAttribute("OneShotAdd"));
			m_crosshairParam.Add(num9, crosshairParam);
		}
		XmlElement xmlElement30 = (XmlElement)documentElement.GetElementsByTagName("CameraParams").Item(0);
		foreach (XmlElement item16 in xmlElement30.GetElementsByTagName("CameraParam"))
		{
			int num10 = int.Parse(item16.GetAttribute("Id"));
			CameraParam cameraParam = new CameraParam();
			cameraParam.id = num10;
			cameraParam.distance = float.Parse(item16.GetAttribute("Distance"));
			cameraParam.angle = float.Parse(item16.GetAttribute("Angle"));
			cameraParam.zoomChangeTime = float.Parse(item16.GetAttribute("ZoomChangeTime"));
			m_cameraParam.Add(num10, cameraParam);
		}
		Dictionary<WeaponType, List<WeaponInfo>> dictionary = new Dictionary<WeaponType, List<WeaponInfo>>();
		XmlElement xmlElement32 = (XmlElement)documentElement.GetElementsByTagName("Weapons").Item(0);
		foreach (XmlElement item17 in xmlElement32.GetElementsByTagName("Weapon"))
		{
			WeaponType weaponType = (WeaponType)int.Parse(item17.GetAttribute("Type"));
			string attribute3 = item17.GetAttribute("Name");
			WeaponInfo weaponInfo = new WeaponInfo();
			weaponInfo.type = weaponType;
			weaponInfo.name = attribute3;
			weaponInfo.model = item17.GetAttribute("Model");
			weaponInfo.animation = item17.GetAttribute("Animation");
			weaponInfo.icon = item17.GetAttribute("Icon");
			weaponInfo.textName = item17.GetAttribute("TextName");
			weaponInfo.rarityText = item17.GetAttribute("RarityText");
			weaponInfo.desc = item17.GetAttribute("Desc").Replace("\\n", "\n");
			weaponInfo.unlockLevel = int.Parse(item17.GetAttribute("UnlockLevel"));
			weaponInfo.buyMoney = int.Parse(item17.GetAttribute("UnlockMoney"));
			weaponInfo.buyCrystal = int.Parse(item17.GetAttribute("UnlockCrystal"));
			weaponInfo.ammoCapacity = int.Parse(item17.GetAttribute("AmmoCapacity"));
			weaponInfo.ammoAmount = int.Parse(item17.GetAttribute("AmmoAmount"));
			weaponInfo.attrDMG = int.Parse(item17.GetAttribute("AttrDMG"));
			weaponInfo.attrRPM = int.Parse(item17.GetAttribute("AttrRPM"));
			weaponInfo.attrACC = int.Parse(item17.GetAttribute("AttrACC"));
			weaponInfo.buyAmmoPrice = int.Parse(item17.GetAttribute("FixPrice"));
			weaponInfo.crosshairType = item17.GetAttribute("CrosshairType");
			weaponInfo.crosshair = int.Parse(item17.GetAttribute("Crosshair"));
			weaponInfo.camera = int.Parse(item17.GetAttribute("Camera"));
			if (!dictionary.ContainsKey(weaponType))
			{
				dictionary.Add(weaponType, new List<WeaponInfo>());
			}
			dictionary[weaponType].Add(weaponInfo);
			m_weaponInfoMapByName.Add(attribute3, weaponInfo);
		}
		foreach (KeyValuePair<WeaponType, List<WeaponInfo>> item18 in dictionary)
		{
			m_weaponInfoMap.Add(item18.Key, item18.Value.ToArray());
		}
		Dictionary<ArmorEffectType, List<ArmorInfo>> dictionary2 = new Dictionary<ArmorEffectType, List<ArmorInfo>>();
		XmlElement xmlElement34 = (XmlElement)documentElement.GetElementsByTagName("Armors").Item(0);
		foreach (XmlElement item19 in xmlElement34.GetElementsByTagName("Armor"))
		{
			ArmorEffectType armorEffectType = (ArmorEffectType)int.Parse(item19.GetAttribute("Type"));
			string attribute4 = item19.GetAttribute("Name");
			ArmorInfo armorInfo = new ArmorInfo();
			armorInfo.type = armorEffectType;
			armorInfo.name = attribute4;
			armorInfo.icon = item19.GetAttribute("Icon");
			armorInfo.textName = item19.GetAttribute("TextName");
			armorInfo.rarityText = item19.GetAttribute("RarityText");
			armorInfo.desc = item19.GetAttribute("Desc").Replace("\\n", "\n");
			armorInfo.unlockLevel = int.Parse(item19.GetAttribute("UnlockLevel"));
			armorInfo.buyMoney = int.Parse(item19.GetAttribute("BuyMoney"));
			armorInfo.buyCrystal = int.Parse(item19.GetAttribute("BuyCrystal"));
			armorInfo.buyAmount = int.Parse(item19.GetAttribute("BuyAmount"));
			armorInfo.param1 = float.Parse(item19.GetAttribute("Param1"));
			armorInfo.param2 = float.Parse(item19.GetAttribute("Param2"));
			armorInfo.param3 = float.Parse(item19.GetAttribute("Param3"));
			if (!dictionary2.ContainsKey(armorEffectType))
			{
				dictionary2.Add(armorEffectType, new List<ArmorInfo>());
			}
			dictionary2[armorEffectType].Add(armorInfo);
			m_armorInfoMapByName.Add(attribute4, armorInfo);
			m_armorInfoList.Add(armorInfo);
		}
		foreach (KeyValuePair<ArmorEffectType, List<ArmorInfo>> item20 in dictionary2)
		{
			m_armorInfoMap.Add(item20.Key, item20.Value.ToArray());
		}
		Dictionary<ItemEffectType, List<ItemInfo>> dictionary3 = new Dictionary<ItemEffectType, List<ItemInfo>>();
		XmlElement xmlElement36 = (XmlElement)documentElement.GetElementsByTagName("Items").Item(0);
		foreach (XmlElement item21 in xmlElement36.GetElementsByTagName("Item"))
		{
			ItemEffectType itemEffectType = (ItemEffectType)int.Parse(item21.GetAttribute("Type"));
			string attribute5 = item21.GetAttribute("Name");
			ItemInfo itemInfo = new ItemInfo();
			itemInfo.type = itemEffectType;
			itemInfo.name = attribute5;
			itemInfo.icon = item21.GetAttribute("Icon");
			itemInfo.textName = item21.GetAttribute("TextName");
			itemInfo.rarityText = item21.GetAttribute("RarityText");
			itemInfo.desc = item21.GetAttribute("Desc").Replace("\\n", "\n");
			itemInfo.unlockLevel = int.Parse(item21.GetAttribute("UnlockLevel"));
			itemInfo.buyMoney = int.Parse(item21.GetAttribute("BuyMoney"));
			itemInfo.buyCrystal = int.Parse(item21.GetAttribute("BuyCrystal"));
			itemInfo.buyAmount = int.Parse(item21.GetAttribute("BuyAmount"));
			itemInfo.param1 = float.Parse(item21.GetAttribute("Param1"));
			itemInfo.param2 = float.Parse(item21.GetAttribute("Param2"));
			if (!dictionary3.ContainsKey(itemEffectType))
			{
				dictionary3.Add(itemEffectType, new List<ItemInfo>());
			}
			dictionary3[itemEffectType].Add(itemInfo);
			m_itemInfoMapByName.Add(attribute5, itemInfo);
			m_itemInfoList.Add(itemInfo);
		}
		foreach (KeyValuePair<ItemEffectType, List<ItemInfo>> item22 in dictionary3)
		{
			m_itemInfoMap.Add(item22.Key, item22.Value.ToArray());
		}
		XmlElement xmlElement38 = (XmlElement)documentElement.GetElementsByTagName("Medals").Item(0);
		foreach (XmlElement item23 in xmlElement38.GetElementsByTagName("Medal"))
		{
			int num11 = int.Parse(item23.GetAttribute("Index"));
			int num12 = int.Parse(item23.GetAttribute("Id"));
			MedalInfo medalInfo = new MedalInfo();
			medalInfo.index = num11;
			medalInfo.id = num12;
			medalInfo.designId = int.Parse(item23.GetAttribute("DesignId"));
			medalInfo.name = item23.GetAttribute("Name");
			medalInfo.conditionName = item23.GetAttribute("ConditionName");
			medalInfo.conditionParam = int.Parse(item23.GetAttribute("ConditionPara"));
			medalInfo.iconName = item23.GetAttribute("IconName");
			medalInfo.desc = item23.GetAttribute("Desc").Replace("\\n", "\n");
			medalInfo.gameCenterScore = int.Parse(item23.GetAttribute("GameCenterScore"));
			medalInfo.rewardMoney = int.Parse(item23.GetAttribute("RewardMoney"));
			medalInfo.rewardExp = int.Parse(item23.GetAttribute("RewardExp"));
			medalInfo.rewardCrystal = int.Parse(item23.GetAttribute("RewardCrystal"));
			medalInfo.rewardDesc = item23.GetAttribute("RewardDesc").Replace("\\n", "\n");
			m_medalInfoMapByIndex.Add(num11, medalInfo);
			m_medalInfoMapById.Add(num12, medalInfo);
		}
		Dictionary<DropItemEffectType, List<DropItemInfo>> dictionary4 = new Dictionary<DropItemEffectType, List<DropItemInfo>>();
		XmlElement xmlElement40 = (XmlElement)documentElement.GetElementsByTagName("DropItems").Item(0);
		foreach (XmlElement item24 in xmlElement40.GetElementsByTagName("DropItem"))
		{
			DropItemEffectType dropItemEffectType = (DropItemEffectType)(int)Enum.Parse(typeof(DropItemEffectType), item24.GetAttribute("Type"));
			string attribute6 = item24.GetAttribute("Name");
			DropItemInfo dropItemInfo = new DropItemInfo();
			dropItemInfo.type = dropItemEffectType;
			dropItemInfo.param1 = float.Parse(item24.GetAttribute("Param1"));
			dropItemInfo.param2 = float.Parse(item24.GetAttribute("Param2"));
			dropItemInfo.param3 = float.Parse(item24.GetAttribute("Param3"));
			dropItemInfo.name = attribute6;
			dropItemInfo.triggerRadius = float.Parse(item24.GetAttribute("TriggerRadius"));
			dropItemInfo.model = item24.GetAttribute("Model");
			if (!dictionary4.ContainsKey(dropItemEffectType))
			{
				dictionary4.Add(dropItemEffectType, new List<DropItemInfo>());
			}
			dictionary4[dropItemEffectType].Add(dropItemInfo);
			m_dropItemInfoMapByName.Add(attribute6, dropItemInfo);
		}
		foreach (KeyValuePair<DropItemEffectType, List<DropItemInfo>> item25 in dictionary4)
		{
			m_dropItemInfoMap.Add(item25.Key, item25.Value.ToArray());
		}
	}
}

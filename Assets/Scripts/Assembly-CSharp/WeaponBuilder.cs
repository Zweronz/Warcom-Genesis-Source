using UnityEngine;

public class WeaponBuilder
{
	public static Weapon CreateWeaponPlayer(string name)
	{
		WeaponInfo weaponInfoByName = DataCenter.Conf().GetWeaponInfoByName(name);
		WeaponPlayer weaponPlayer = NewWeaponPlayer(weaponInfoByName.type);
		weaponPlayer.Initialize(PrefabCache.Load<GameObject>("Models/Weapons/" + weaponInfoByName.model), weaponInfoByName);
		weaponPlayer.Reload();
		return weaponPlayer;
	}

	public static Weapon CreateWeaponNPC(string name)
	{
		WeaponInfo weaponInfoByName = DataCenter.Conf().GetWeaponInfoByName(name);
		WeaponEnemy weaponEnemy = NewWeaponEnemy(weaponInfoByName.type);
		weaponEnemy.Initialize(PrefabCache.Load<GameObject>("Models/Weapons/" + weaponInfoByName.model), weaponInfoByName);
		weaponEnemy.Reload();
		return weaponEnemy;
	}

	public static Weapon CreateWeaponNet(string name)
	{
		WeaponInfo weaponInfoByName = DataCenter.Conf().GetWeaponInfoByName(name);
		WeaponNet weaponNet = NewWeaponNet(weaponInfoByName.type);
		weaponNet.Initialize(PrefabCache.Load<GameObject>("Models/Weapons/" + weaponInfoByName.model), weaponInfoByName);
		weaponNet.Reload();
		return weaponNet;
	}

	private static WeaponPlayer NewWeaponPlayer(WeaponType weaponType)
	{
		WeaponPlayer result = null;
		switch (weaponType)
		{
		case WeaponType.Handgun:
			result = new WeaponPlayerHandgun();
			break;
		case WeaponType.AssaultRifle:
			result = new WeaponPlayerAssaultRifle();
			break;
		case WeaponType.MachineGun:
			result = new WeaponPlayerMachineGun();
			break;
		case WeaponType.SubmachineGun:
			result = new WeaponPlayerSubmachineGun();
			break;
		case WeaponType.Shotgun:
			result = new WeaponPlayerShotgun();
			break;
		case WeaponType.SniperRifle:
			result = new WeaponPlayerSniperRifle();
			break;
		case WeaponType.RPG:
			result = new WeaponPlayerRPG();
			break;
		case WeaponType.Knife:
			result = new WeaponPlayerKnife();
			break;
		}
		return result;
	}

	private static WeaponEnemy NewWeaponEnemy(WeaponType weaponType)
	{
		WeaponEnemy result = null;
		switch (weaponType)
		{
		case WeaponType.Handgun:
			result = new WeaponEnemyHandgun();
			break;
		case WeaponType.AssaultRifle:
			result = new WeaponEnemyAssaultRifle();
			break;
		case WeaponType.MachineGun:
			result = new WeaponEnemyMachineGun();
			break;
		case WeaponType.SubmachineGun:
			result = new WeaponEnemySubmachineGun();
			break;
		case WeaponType.Shotgun:
			result = new WeaponEnemyShotgun();
			break;
		case WeaponType.SniperRifle:
			result = new WeaponEnemySniperRifle();
			break;
		case WeaponType.RPG:
			result = new WeaponEnemyRPG();
			break;
		}
		return result;
	}

	private static WeaponNet NewWeaponNet(WeaponType weaponType)
	{
		WeaponNet result = null;
		switch (weaponType)
		{
		case WeaponType.Handgun:
			result = new WeaponNetHandgun();
			break;
		case WeaponType.AssaultRifle:
			result = new WeaponNetAssaultRifle();
			break;
		case WeaponType.MachineGun:
			result = new WeaponNetMachineGun();
			break;
		case WeaponType.SubmachineGun:
			result = new WeaponNetSubmachineGun();
			break;
		case WeaponType.Shotgun:
			result = new WeaponNetShotgun();
			break;
		case WeaponType.SniperRifle:
			result = new WeaponNetSniperRifle();
			break;
		case WeaponType.RPG:
			result = new WeaponNetRPG();
			break;
		case WeaponType.Knife:
			result = new WeaponNetKnife();
			break;
		}
		return result;
	}
}

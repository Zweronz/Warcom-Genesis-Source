using System;
using TNetSdk;
using UnityEngine;

public class NetDataToNetObject
{
	public static SFSObject PlayerSettingToJsonObject(NetPlayerSettingInfo npi)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddInt(npi.cTypeID);
		sFSArray.AddUtfString(npi.modelName);
		sFSArray.AddInt(npi.seatID);
		sFSArray.AddInt(npi.netID);
		sFSArray.AddUtfString(npi.nickName);
		sFSArray.AddUtfString(npi.weapon01);
		sFSArray.AddUtfString(npi.weapon02);
		sFSArray.AddUtfString(npi.weapon03);
		sFSArray.AddUtfString(npi.armor01);
		sFSArray.AddUtfString(npi.armor02);
		sFSArray.AddUtfString(npi.armor03);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 1);
		return sFSObject;
	}

	public static NetPlayerSettingInfo PlayerSettingFromJsonObject(ISFSObject obj)
	{
		//Discarded unreachable code: IL_00aa, IL_00bd
		try
		{
			NetPlayerSettingInfo netPlayerSettingInfo = new NetPlayerSettingInfo();
			ISFSArray sFSArray = obj.GetSFSArray("param");
			netPlayerSettingInfo.cTypeID = sFSArray.GetInt(0);
			netPlayerSettingInfo.modelName = sFSArray.GetUtfString(1);
			netPlayerSettingInfo.seatID = sFSArray.GetInt(2);
			netPlayerSettingInfo.netID = sFSArray.GetInt(3);
			netPlayerSettingInfo.nickName = sFSArray.GetUtfString(4);
			netPlayerSettingInfo.weapon01 = sFSArray.GetUtfString(5);
			netPlayerSettingInfo.weapon02 = sFSArray.GetUtfString(6);
			netPlayerSettingInfo.weapon03 = sFSArray.GetUtfString(7);
			netPlayerSettingInfo.armor01 = sFSArray.GetUtfString(8);
			netPlayerSettingInfo.armor02 = sFSArray.GetUtfString(9);
			netPlayerSettingInfo.armor03 = sFSArray.GetUtfString(10);
			return netPlayerSettingInfo;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject PlayerStatusToJsonObject(NetPlayerStatusInfo nps)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddFloat(nps.pitchAngle);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 2);
		return sFSObject;
	}

	public static NetPlayerStatusInfo PlayerStatusFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_001a, IL_002d
		try
		{
			NetPlayerStatusInfo netPlayerStatusInfo = new NetPlayerStatusInfo();
			netPlayerStatusInfo.pitchAngle = param.GetFloat(0);
			return netPlayerStatusInfo;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject DropItemRefreshToJsonObject(NetDropItemRefresh ndr)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddUtfString(ndr.dropItemName);
		sFSArray.AddUtfString(ndr.pointName);
		sFSArray.AddUtfString(ndr.netId);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 3);
		return sFSObject;
	}

	public static NetDropItemRefresh DropItemRefreshFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_0034, IL_0047
		try
		{
			NetDropItemRefresh netDropItemRefresh = new NetDropItemRefresh();
			netDropItemRefresh.dropItemName = param.GetUtfString(0);
			netDropItemRefresh.pointName = param.GetUtfString(1);
			netDropItemRefresh.netId = param.GetUtfString(2);
			return netDropItemRefresh;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject PlayerFireActionToJsonObject(NetPlayerFireAction nfc)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddBool(nfc.fire);
		sFSArray.AddDouble(nfc.timeStamp);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 5);
		return sFSObject;
	}

	public static NetPlayerFireAction PlayerFireActionFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_0027, IL_003a
		try
		{
			NetPlayerFireAction netPlayerFireAction = new NetPlayerFireAction();
			netPlayerFireAction.fire = param.GetBool(0);
			netPlayerFireAction.timeStamp = param.GetDouble(1);
			return netPlayerFireAction;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject PlayerDeadActionToJsonObject(NetPlayerDeadAction ndc)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddBool(ndc.headShot);
		sFSArray.AddInt(ndc.byWho);
		sFSArray.AddInt(ndc.byWhat);
		sFSArray.AddDouble(ndc.timeStamp);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 6);
		return sFSObject;
	}

	public static NetPlayerDeadAction PlayerDeadActionFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_0041, IL_0054
		try
		{
			NetPlayerDeadAction netPlayerDeadAction = new NetPlayerDeadAction();
			netPlayerDeadAction.headShot = param.GetBool(0);
			netPlayerDeadAction.byWho = param.GetInt(1);
			netPlayerDeadAction.byWhat = param.GetInt(2);
			netPlayerDeadAction.timeStamp = param.GetDouble(3);
			return netPlayerDeadAction;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject PlayerReloadActionToJsonObject(NetPlayerReloadAction nrc)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddDouble(nrc.timeStamp);
		sFSArray.AddInt(nrc.currentAmmoTotal);
		sFSArray.AddInt((int)nrc.currentWeapon);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 7);
		return sFSObject;
	}

	public static NetPlayerReloadAction PlayerReloadActionFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_0034, IL_0047
		try
		{
			NetPlayerReloadAction netPlayerReloadAction = new NetPlayerReloadAction();
			netPlayerReloadAction.timeStamp = param.GetDouble(0);
			netPlayerReloadAction.currentAmmoTotal = param.GetInt(1);
			netPlayerReloadAction.currentWeapon = (WeaponType)param.GetInt(2);
			return netPlayerReloadAction;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject PlayerChangeWeaponActionToJsonObject(NetPlayerChangeWeaponAction ncc)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddInt(ncc.changetWeaponId);
		sFSArray.AddDouble(ncc.timeStamp);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 8);
		return sFSObject;
	}

	public static NetPlayerChangeWeaponAction PlayerChangeWeaponActionFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_0027, IL_003a
		try
		{
			NetPlayerChangeWeaponAction netPlayerChangeWeaponAction = new NetPlayerChangeWeaponAction();
			netPlayerChangeWeaponAction.changetWeaponId = param.GetInt(0);
			netPlayerChangeWeaponAction.timeStamp = param.GetDouble(1);
			return netPlayerChangeWeaponAction;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject PlayerResurrectionActionToJsonObject(NetPlayerResurrectionAction nrsc)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddUtfString(nrsc.resurrectPoint);
		sFSArray.AddFloat(nrsc.direction);
		sFSArray.AddDouble(nrsc.timeStamp);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 9);
		return sFSObject;
	}

	public static NetPlayerResurrectionAction PlayerResurrectionActionFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_0034, IL_0047
		try
		{
			NetPlayerResurrectionAction netPlayerResurrectionAction = new NetPlayerResurrectionAction();
			netPlayerResurrectionAction.resurrectPoint = param.GetUtfString(0);
			netPlayerResurrectionAction.direction = param.GetFloat(1);
			netPlayerResurrectionAction.timeStamp = param.GetDouble(2);
			return netPlayerResurrectionAction;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject PlayerUseItemActionToJsonObject(NetPlayerUseItemAction nuic)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddUtfString(nuic.itemName);
		sFSArray.AddDouble(nuic.timeStamp);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 12);
		return sFSObject;
	}

	public static NetPlayerUseItemAction PlayerUseItemActionFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_0027, IL_003a
		try
		{
			NetPlayerUseItemAction netPlayerUseItemAction = new NetPlayerUseItemAction();
			netPlayerUseItemAction.itemName = param.GetUtfString(0);
			netPlayerUseItemAction.timeStamp = param.GetDouble(1);
			return netPlayerUseItemAction;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject NetDropItemEatToJsonObject(NetDropItemEat nde)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddUtfString(nde.dropItemNetId);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 4);
		return sFSObject;
	}

	public static NetDropItemEat NetDropItemEatFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_001a, IL_002d
		try
		{
			NetDropItemEat netDropItemEat = new NetDropItemEat();
			netDropItemEat.dropItemNetId = param.GetUtfString(0);
			return netDropItemEat;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject HitInfoToJsonObject(NetHitInfo hi)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddInt((int)hi.weaponType);
		sFSArray.AddFloat(hi.damage);
		sFSArray.AddBool(hi.headShot);
		sFSArray.AddInt(hi.netId);
		sFSArray.AddInt(hi.byNetId);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 10);
		return sFSObject;
	}

	public static NetHitInfo HitInfoFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_004e, IL_0061
		try
		{
			NetHitInfo netHitInfo = new NetHitInfo();
			netHitInfo.weaponType = (WeaponType)param.GetInt(0);
			netHitInfo.damage = param.GetFloat(1);
			netHitInfo.headShot = param.GetBool(2);
			netHitInfo.netId = param.GetInt(3);
			netHitInfo.byNetId = param.GetInt(4);
			return netHitInfo;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}

	public static SFSObject TransformToJsonObject(TransformInfo tfi)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray val = tfi.trans.ToSFSArray();
		SFSArray sFSArray = new SFSArray();
		sFSArray.AddUtfString(tfi.objectId);
		sFSArray.AddSFSArray(val);
		sFSObject.PutSFSArray("param", sFSArray);
		sFSObject.PutInt("type", 0);
		return sFSObject;
	}

	public static TransformInfo TransformFromJsonObject(ISFSArray param)
	{
		//Discarded unreachable code: IL_002c, IL_003f
		try
		{
			TransformInfo transformInfo = new TransformInfo();
			transformInfo.objectId = param.GetUtfString(0);
			transformInfo.trans = NetworkTransform.FromSFSArray(param.GetSFSArray(1));
			return transformInfo;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			return null;
		}
	}
}

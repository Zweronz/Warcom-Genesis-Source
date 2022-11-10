using System;
using TNetSdk;
using UnityEngine;

public class UtilsNet
{
	public static void UpdateUserVariables(NetPlayerSettingInfo npi)
	{
		try
		{
			SFSObject value = NetDataToNetObject.PlayerSettingToJsonObject(npi);
			if (DataCenter.StateMulti().m_tnet != null)
			{
				DataCenter.StateMulti().m_tnet.Send(new SetUserVariableRequest(TNetUserVarType.PlayerSetting, value));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static NetPlayerSettingInfo GetUserVariable(TNetUser user)
	{
		if (user.GetVariable(TNetUserVarType.PlayerSetting) != null)
		{
			return NetDataToNetObject.PlayerSettingFromJsonObject(user.GetVariable(TNetUserVarType.PlayerSetting));
		}
		return null;
	}

	public static void SendTransform(NetworkTransform trans, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				TransformInfo tfi = new TransformInfo(trans, DataCenter.StateMulti().m_tnet.TimeManager.NetworkTime, id);
				SFSObject sfs_object = NetDataToNetObject.TransformToJsonObject(tfi);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentStateInfo(NetPlayerStatusInfo nps, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.PlayerStatusToJsonObject(nps);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentDropItemRefresh(NetDropItemRefresh ndr)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.DropItemRefreshToJsonObject(ndr);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentFireAction(NetPlayerFireAction nfc, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.PlayerFireActionToJsonObject(nfc);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentDeadAction(NetPlayerDeadAction ndc, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.PlayerDeadActionToJsonObject(ndc);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentReloadAction(NetPlayerReloadAction nrc, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.PlayerReloadActionToJsonObject(nrc);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentChangeWeaponAction(NetPlayerChangeWeaponAction ncc, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.PlayerChangeWeaponActionToJsonObject(ncc);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentResurrectionAction(NetPlayerResurrectionAction nrsc, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.PlayerResurrectionActionToJsonObject(nrsc);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentUseItemAction(NetPlayerUseItemAction nuic, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.PlayerUseItemActionToJsonObject(nuic);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentHitInfo(NetHitInfo hi, string id)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.HitInfoToJsonObject(hi);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SentDropItemEatAction(NetDropItemEat nde)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				SFSObject sfs_object = NetDataToNetObject.NetDropItemEatToJsonObject(nde);
				DataCenter.StateMulti().m_tnet.Send(new BroadcastMessageRequest(sfs_object));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SendLock(string lockId)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				DataCenter.StateMulti().m_tnet.Send(new LockSthRequest(lockId));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public static void SendLeave()
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				DataCenter.StateMulti().m_tnet.Send(new LeaveRoomRequest());
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}
}

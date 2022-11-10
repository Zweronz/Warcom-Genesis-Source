using System;
using TNetSdk;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	private static NetworkManager instance;

	public static NetworkManager Instance()
	{
		if (instance == null)
		{
			instance = new NetworkManager();
		}
		return instance;
	}

	private void Awake()
	{
		try
		{
			Application.runInBackground = true;
			if (DataCenter.StateMulti().m_tnet == null)
			{
				OnConnectionLost(null);
			}
			else
			{
				SubscribeDelegates();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void FixedUpdate()
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				DataCenter.StateMulti().m_tnet.Update(Time.fixedDeltaTime);
			}
			else
			{
				OnConnectionLost(null);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		try
		{
			UnsubscribeDelegates();
			instance = null;
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void OnConnectionLost(TNetEventData evt)
	{
		try
		{
			DataCenter.StateMulti().ConnectLost();
			Application.LoadLevel("WorldMapWeb");
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void UnsubscribeDelegates()
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				DataCenter.StateMulti().m_tnet.RemoveAllEventListeners();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void SubscribeDelegates()
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null)
			{
				DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventSystem.CONNECTION_KILLED, OnConnectionLost);
				DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventSystem.CONNECTION_TIMEOUT, OnConnectionLost);
				DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventSystem.REVERSE_HEART_TIMEOUT, OnConnectionLost);
				DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.USER_VARIABLES_UPDATE, OnUserVariablesUpdate);
				DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.USER_EXIT_ROOM, OnUserLeaveRoom);
				DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.OBJECT_MESSAGE, OnObjectMessage);
				DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.LOCK_STH, OnLock);
				DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.ROOM_MASTER_CHANGE, OnHostChange);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnUserVariablesUpdate(TNetEventData evt)
	{
		try
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			TNetUserVarType tNetUserVarType = (TNetUserVarType)(int)evt.data["key"];
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void OnUserLeaveRoom(TNetEventData evt)
	{
		try
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			if (DataCenter.StateMulti().m_tnet != null)
			{
				if (tNetUser != DataCenter.StateMulti().m_tnet.Myself)
				{
					TNetRoom curRoom = DataCenter.StateMulti().m_tnet.CurRoom;
					GameManager.Instance().GetLevel().GetEnemyNetByNetId(tNetUser.Id)
						.Destroy();
					GameManager.Instance().GetLevel().RemoveEnemyNetByNetId(tNetUser.Id);
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(tNetUser.Name + " has disconnected.");
				}
				else
				{
					UtilsNet.UpdateUserVariables(new NetPlayerSettingInfo());
					UnityEngine.Object.Destroy(this);
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void OnObjectMessage(TNetEventData evt)
	{
		try
		{
			SFSObject dt = (SFSObject)evt.data["message"];
			TNetUser sender = (TNetUser)evt.data["user"];
			HandleObjectMessage(sender, dt);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnLock(TNetEventData evt)
	{
		try
		{
			RoomLockResCmd.Result result = (RoomLockResCmd.Result)(int)evt.data["result"];
			string lockId = evt.data["key"].ToString();
			int success = ((result != 0) ? (-1) : 0);
			HandleLock(success, lockId);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnHostChange(TNetEventData evt)
	{
		try
		{
			TNetUser user = (TNetUser)evt.data["user"];
			HandleHostChange(user);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleHostChange(TNetUser user)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet != null && user == DataCenter.StateMulti().m_tnet.Myself)
			{
				DataCenter.StateMulti().m_isRoomMaster = true;
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleLock(int success, string lockId)
	{
		try
		{
			if (DataCenter.StateMulti().m_tnet == null || success != 0)
			{
				return;
			}
			UtilsNet.SentDropItemEatAction(new NetDropItemEat(lockId));
			DropItem[] dropItemArray = GameManager.Instance().GetLevel().GetDropItemArray();
			for (int i = 0; i < dropItemArray.Length; i++)
			{
				if (((DropItemNet)dropItemArray[i]).m_netId == lockId)
				{
					dropItemArray[i].m_isEffect = true;
					Effect effect = dropItemArray[i].GetItemEffect();
					GameManager.Instance().GetLevel().GetPlayer()
						.OnEffect(ref effect);
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleObjectMessage(TNetUser sender, SFSObject dt)
	{
		try
		{
			ObjectMessageType @int = (ObjectMessageType)dt.GetInt("type");
			ISFSArray sFSArray = dt.GetSFSArray("param");
			switch (@int)
			{
			case ObjectMessageType.TransformSynchronize:
				HandleTransformSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.PlayerStatus:
				HandleStateSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.FireAction:
				HandleFireActionSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.DeadAction:
				HandleDeadActionSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.ChangeAction:
				HandleChangeWeaponActionSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.ReloadAction:
				HandleReloadActionSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.ResurrectionAction:
				HandleResurrectionSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.UseItem:
				HandleUseItemActionSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.HitInfo:
				HandleOnHitSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.DropItemRefresh:
				HandleDropItemRefreshSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.DropItemEat:
				HandleDropItemEatActionSynchronize(sender, sFSArray);
				break;
			case ObjectMessageType.PlayerSetting:
			case ObjectMessageType.GetItem:
				break;
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleUseItemActionSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetPlayerUseItemAction netPlayerUseItemAction = NetDataToNetObject.PlayerUseItemActionFromJsonObject(param);
			if (netPlayerUseItemAction != null && DataCenter.StateMulti().m_tnet != null && user != DataCenter.StateMulti().m_tnet.Myself)
			{
				EnemyNet enemyNetByNetId = GameManager.Instance().GetLevel().GetEnemyNetByNetId(user.Id);
				ItemInfo itemInfoByName = DataCenter.Conf().GetItemInfoByName(netPlayerUseItemAction.itemName);
				enemyNetByNetId.UseItems(itemInfoByName);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleDropItemRefreshSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetDropItemRefresh netDropItemRefresh = NetDataToNetObject.DropItemRefreshFromJsonObject(param);
			if (netDropItemRefresh != null && DataCenter.StateMulti().m_tnet != null && user != DataCenter.StateMulti().m_tnet.Myself)
			{
				DropItem dropItem = DropItemBuilder.CreateDropItemNet(netDropItemRefresh.dropItemName, netDropItemRefresh.pointName, netDropItemRefresh.netId);
				GameManager.Instance().GetLevel().AddDropItem(dropItem);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleDropItemEatActionSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetDropItemEat netDropItemEat = NetDataToNetObject.NetDropItemEatFromJsonObject(param);
			if (netDropItemEat == null || DataCenter.StateMulti().m_tnet == null || user == DataCenter.StateMulti().m_tnet.Myself)
			{
				return;
			}
			DropItem[] dropItemArray = GameManager.Instance().GetLevel().GetDropItemArray();
			for (int i = 0; i < dropItemArray.Length; i++)
			{
				if (((DropItemNet)dropItemArray[i]).m_netId == netDropItemEat.dropItemNetId)
				{
					dropItemArray[i].m_isEffect = true;
					Effect effect = dropItemArray[i].GetItemEffect();
					GameManager.Instance().GetLevel().GetEnemyNetByNetId(user.Id)
						.OnEffect(ref effect);
				}
			}
			GameManager.Instance().GetLevel().GetEnemyNetByNetId(user.Id);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleTransformSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			TransformInfo transformInfo = NetDataToNetObject.TransformFromJsonObject(param);
			if (transformInfo == null || DataCenter.StateMulti().m_tnet == null)
			{
				return;
			}
			EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
			foreach (EnemyNet enemyNet in enemyNetArray)
			{
				if (user.Id == enemyNet.m_netId)
				{
					enemyNet.m_enemyNetReceive.SetTransformInfo(transformInfo);
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleStateSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetPlayerStatusInfo netPlayerStatusInfo = NetDataToNetObject.PlayerStatusFromJsonObject(param);
			if (netPlayerStatusInfo == null || DataCenter.StateMulti().m_tnet == null)
			{
				return;
			}
			EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
			foreach (EnemyNet enemyNet in enemyNetArray)
			{
				if (user.Id == enemyNet.m_netId)
				{
					enemyNet.m_enemyNetReceive.SetStateInfo(netPlayerStatusInfo);
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleFireActionSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetPlayerFireAction netPlayerFireAction = NetDataToNetObject.PlayerFireActionFromJsonObject(param);
			if (netPlayerFireAction == null || DataCenter.StateMulti().m_tnet == null)
			{
				return;
			}
			EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
			foreach (EnemyNet enemyNet in enemyNetArray)
			{
				if (user.Id == enemyNet.m_netId)
				{
					enemyNet.SetFire(netPlayerFireAction.fire, netPlayerFireAction.fire);
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleDeadActionSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetPlayerDeadAction netPlayerDeadAction = NetDataToNetObject.PlayerDeadActionFromJsonObject(param);
			if (netPlayerDeadAction == null || DataCenter.StateMulti().m_tnet == null)
			{
				return;
			}
			EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
			foreach (EnemyNet enemyNet in enemyNetArray)
			{
				if (user.Id != enemyNet.m_netId)
				{
					continue;
				}
				enemyNet.m_dead = true;
				enemyNet.m_headShot = netPlayerDeadAction.headShot;
				enemyNet.FSMSwitchDeathState();
				string text = DataCenter.StateMulti().m_tnet.CurRoom.GetUserById(netPlayerDeadAction.byWho).Name;
				if (netPlayerDeadAction.headShot)
				{
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(text + " headshot " + user.Name);
					continue;
				}
				switch (netPlayerDeadAction.byWhat)
				{
				case 0:
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(text + " mashed " + user.Name);
					break;
				case 1:
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(text + " capped " + user.Name);
					break;
				case 2:
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(text + " blasted " + user.Name);
					break;
				case 7:
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(text + " splattered " + user.Name);
					break;
				case 5:
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(text + " shredded " + user.Name);
					break;
				case 6:
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(text + " sniped " + user.Name);
					break;
				case 4:
					GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack(text + " blasted " + user.Name);
					break;
				}
			}
			if (netPlayerDeadAction.byWho == DataCenter.StateMulti().m_tnet.Myself.Id)
			{
				DataCenter.StateMulti().m_missionTempData.emenyKill++;
				if (netPlayerDeadAction.headShot)
				{
					DataCenter.StateMulti().m_missionTempData.emenyKillHeadShot++;
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleChangeWeaponActionSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetPlayerChangeWeaponAction netPlayerChangeWeaponAction = NetDataToNetObject.PlayerChangeWeaponActionFromJsonObject(param);
			if (netPlayerChangeWeaponAction == null || DataCenter.StateMulti().m_tnet == null)
			{
				return;
			}
			EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
			foreach (EnemyNet enemyNet in enemyNetArray)
			{
				if (user.Id == enemyNet.m_netId)
				{
					enemyNet.ChangeWeapon(netPlayerChangeWeaponAction.changetWeaponId);
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleReloadActionSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetPlayerReloadAction netPlayerReloadAction = NetDataToNetObject.PlayerReloadActionFromJsonObject(param);
			if (netPlayerReloadAction == null || DataCenter.StateMulti().m_tnet == null)
			{
				return;
			}
			EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
			foreach (EnemyNet enemyNet in enemyNetArray)
			{
				if (user.Id == enemyNet.m_netId)
				{
					enemyNet.OpenReload();
					enemyNet.SynchronizeWeaponAmmo(netPlayerReloadAction.currentAmmoTotal, netPlayerReloadAction.currentWeapon);
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleResurrectionSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetPlayerResurrectionAction netPlayerResurrectionAction = NetDataToNetObject.PlayerResurrectionActionFromJsonObject(param);
			if (netPlayerResurrectionAction == null || DataCenter.StateMulti().m_tnet == null)
			{
				return;
			}
			EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
			foreach (EnemyNet enemyNet in enemyNetArray)
			{
				if (user.Id == enemyNet.m_netId)
				{
					enemyNet.OnResurrection();
					enemyNet.GetTransform().position = ScenePointManager.GetScenePointVector3(netPlayerResurrectionAction.resurrectPoint);
					enemyNet.GetTransform().eulerAngles = new Vector3(0f, netPlayerResurrectionAction.direction, 0f);
					enemyNet.m_agent.enabled = true;
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleOnHitSynchronize(TNetUser user, ISFSArray param)
	{
		try
		{
			NetHitInfo netHitInfo = NetDataToNetObject.HitInfoFromJsonObject(param);
			if (netHitInfo == null || DataCenter.StateMulti().m_tnet.Myself.Id == netHitInfo.byNetId || DataCenter.StateMulti().m_tnet == null)
			{
				return;
			}
			if (DataCenter.StateMulti().m_tnet.Myself.Id == netHitInfo.netId)
			{
				GameManager.Instance().GetLevel().GetPlayer()
					.OnHitWeb(netHitInfo);
				return;
			}
			EnemyNet[] enemyNetArray = GameManager.Instance().GetLevel().GetEnemyNetArray();
			foreach (EnemyNet enemyNet in enemyNetArray)
			{
				if (netHitInfo.netId == enemyNet.m_netId)
				{
					enemyNet.OnHitWeb(netHitInfo);
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}
}

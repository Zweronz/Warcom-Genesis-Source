using System;
using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class UILoadingWeb : MonoBehaviour
{
	private int getRoomListPage;

	private bool joinRoomSuccess;

	private bool isRoomStart;

	private int m_count;

	public void Awake()
	{
	}

	public void Start()
	{
		GC.Collect();
		Resources.UnloadUnusedAssets();
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventSystem.DISCONNECT, OnDisconnect);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventSystem.CONNECTION_KILLED, OnConnectionKilled);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventSystem.CONNECTION_TIMEOUT, OnConnectTimeout);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.GET_ROOM_LIST, OnGetRoomList);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.ROOM_CREATION, OnRoomCreation);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.ROOM_JOIN, OnJoinRoom);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.ROOM_MASTER_CHANGE, OnRoomMasterChange);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.ROOM_START, OnRoomStart);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.USER_ENTER_ROOM, OnJoinRoomNotify);
		DataCenter.StateMulti().m_tnet.AddEventListener(TNetEventRoom.USER_EXIT_ROOM, OnLeaveRoomNotify);
		DataCenter.StateMulti().m_isRoomMaster = false;
		DataCenter.StateMulti().m_tnet.Send(new GetRoomListRequest(0, getRoomListPage, 10, RoomDragListCmd.ListType.not_full_not_game));
	}

	public void OnDestroy()
	{
		OpenClikPlugin.Hide();
		DataCenter.StateMulti().m_tnet.RemoveAllEventListeners();
	}

	public void Update()
	{
		if (DataCenter.StateMulti().m_tnet != null)
		{
			DataCenter.StateMulti().m_tnet.Update(Time.deltaTime);
		}
		if (!isRoomStart)
		{
			return;
		}
		Application.LoadLevel(UtilUIUpdateAttribute.GetNextScene());
		if (UtilUIUpdateAttribute.GetNextScene().Contains("Level"))
		{
			TAudioController component = DataCenter.StateCommon().audio.GetComponent<TAudioController>();
			if (component != null)
			{
				component.StopAudio("music_Meu");
			}
			UnityEngine.Object.Destroy(DataCenter.StateCommon().audio.transform.Find("Audio").gameObject);
		}
	}

	private void OnDisconnect(TNetEventData tEvent)
	{
		DataCenter.StateMulti().ManualDisconnect();
	}

	private void OnConnectionKilled(TNetEventData tEvent)
	{
		DataCenter.StateMulti().ConnectLost();
		Application.LoadLevel("WorldMapWeb");
	}

	private void OnConnectTimeout(TNetEventData tEvent)
	{
		DataCenter.StateMulti().ConnectLost();
		Application.LoadLevel("WorldMapWeb");
	}

	private void OnGetRoomList(TNetEventData tEvent)
	{
		List<TNetRoom> list = (List<TNetRoom>)tEvent.data["roomList"];
		if (list.Count == 0)
		{
			if ((ushort)getRoomListPage == (ushort)tEvent.data["pageSum"])
			{
				DataCenter.StateMulti().m_tnet.Send(new CreateRoomRequest(DataCenter.StateMulti().GetUserName() + "room", string.Empty, 0, 8, RoomCreateCmd.RoomType.limit, RoomCreateCmd.RoomSwitchMasterType.Auto, string.Empty));
				return;
			}
			getRoomListPage++;
			DataCenter.StateMulti().m_tnet.Send(new GetRoomListRequest(0, getRoomListPage, 10, RoomDragListCmd.ListType.not_full_not_game));
		}
		else
		{
			DataCenter.StateMulti().m_tnet.Send(new JoinRoomRequest(list[0].Id, string.Empty));
		}
	}

	private void OnRoomCreation(TNetEventData tEvent)
	{
		DataCenter.StateMulti().m_isRoomMaster = true;
	}

	private void OnRoomMasterChange(TNetEventData tEvent)
	{
		DataCenter.StateMulti().m_isRoomMaster = true;
	}

	private void OnRoomStart(TNetEventData tEvent)
	{
		isRoomStart = true;
	}

	private void OnJoinRoom(TNetEventData tEvent)
	{
		joinRoomSuccess = true;
		int currentCharactor = DataCenter.StateMulti().GetCurrentCharactor();
		CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo(currentCharactor);
		NetPlayerSettingInfo netPlayerSettingInfo = new NetPlayerSettingInfo();
		netPlayerSettingInfo.cTypeID = currentCharactor;
		netPlayerSettingInfo.modelName = DataCenter.StateMulti().GetCurrentModelName();
		netPlayerSettingInfo.seatID = DataCenter.StateMulti().m_tnet.Myself.SitIndex;
		netPlayerSettingInfo.netID = DataCenter.StateMulti().m_tnet.Myself.Id;
		netPlayerSettingInfo.nickName = DataCenter.StateMulti().GetUserName();
		netPlayerSettingInfo.weapon01 = characterEquipInfo.weapon01;
		netPlayerSettingInfo.weapon02 = characterEquipInfo.weapon02;
		netPlayerSettingInfo.weapon03 = characterEquipInfo.weapon03;
		netPlayerSettingInfo.armor01 = characterEquipInfo.armor01;
		netPlayerSettingInfo.armor02 = characterEquipInfo.armor02;
		netPlayerSettingInfo.armor03 = characterEquipInfo.armor03;
		UtilsNet.UpdateUserVariables(netPlayerSettingInfo);
	}

	private void OnJoinRoomNotify(TNetEventData tEvent)
	{
		TNetUser tNetUser = (TNetUser)tEvent.data["user"];
	}

	private void OnLeaveRoomNotify(TNetEventData tEvent)
	{
	}

	private void OnGUI()
	{
		if (joinRoomSuccess)
		{
			GUI.Label(new Rect((float)Screen.width * 0.5f - 50f, (float)Screen.height * 0.5f - 50f, 100f, 100f), "PlayerCounts:" + DataCenter.StateMulti().m_tnet.CurRoom.UserCount);
		}
		if (DataCenter.StateMulti().m_isRoomMaster && GUI.Button(new Rect((float)Screen.width * 0.5f - 200f, (float)Screen.height * 0.5f + 100f, 400f, 400f), "Start"))
		{
			DataCenter.StateMulti().m_tnet.Send(new RoomStartRequest());
		}
	}
}

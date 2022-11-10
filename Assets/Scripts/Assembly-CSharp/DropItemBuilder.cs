using UnityEngine;

public class DropItemBuilder
{
	public static DropItem CreateDropItem(string name, Vector3 position)
	{
		DropItemInfo dropItemInfoByName = DataCenter.Conf().GetDropItemInfoByName(name);
		Effect effect = new Effect();
		effect.m_type = dropItemInfoByName.type;
		effect.m_param1 = dropItemInfoByName.param1;
		effect.m_param2 = dropItemInfoByName.param2;
		effect.m_param3 = dropItemInfoByName.param3;
		DropItem dropItem = new DropItem();
		dropItem.Initialize(PrefabCache.Load<GameObject>("Models/DropItems/" + dropItemInfoByName.model), dropItemInfoByName, position, effect);
		dropItem.GetGameObject().AddComponent<UtilDropItem>();
		return dropItem;
	}

	public static DropItem CreateDropItemNet(string name, string pointName, string netId)
	{
		DropItemInfo dropItemInfoByName = DataCenter.Conf().GetDropItemInfoByName(name);
		Effect effect = new Effect();
		effect.m_type = dropItemInfoByName.type;
		effect.m_param1 = dropItemInfoByName.param1;
		effect.m_param2 = dropItemInfoByName.param2;
		effect.m_param3 = dropItemInfoByName.param3;
		Vector3 scenePointVector = ScenePointManager.GetScenePointVector3(pointName);
		DropItemNet dropItemNet = new DropItemNet();
		dropItemNet.Initialize(PrefabCache.Load<GameObject>("Models/DropItems/" + dropItemInfoByName.model), dropItemInfoByName, scenePointVector, effect);
		dropItemNet.GetGameObject().AddComponent<UtilDropItemNet>();
		dropItemNet.m_netId = netId;
		dropItemNet.m_point = pointName;
		return dropItemNet;
	}
}

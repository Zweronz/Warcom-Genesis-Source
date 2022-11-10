using UnityEngine;

public class DropItemStub : MonoBehaviour
{
	private DropItem m_dropItem;

	public static void BindDropItem(GameObject gameObject, DropItem dropItem)
	{
		DropItemStub dropItemStub = gameObject.AddComponent<DropItemStub>();
		dropItemStub.m_dropItem = dropItem;
	}

	public static DropItem GetDropItem(GameObject gameObject)
	{
		DropItemStub component = gameObject.GetComponent<DropItemStub>();
		return component.m_dropItem;
	}
}

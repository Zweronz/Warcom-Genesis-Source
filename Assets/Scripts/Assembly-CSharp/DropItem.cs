using UnityEngine;

public class DropItem
{
	public bool m_isEffect;

	protected GameObject m_gameObject;

	protected Transform m_transform;

	protected Effect m_effect;

	protected string m_name;

	public virtual void Initialize(GameObject prefab, DropItemInfo ItemInfo, Vector3 position, Effect effect)
	{
		m_gameObject = Object.Instantiate(prefab, position, prefab.transform.rotation) as GameObject;
		m_gameObject.name = ItemInfo.name;
		DropItemStub.BindDropItem(m_gameObject, this);
		m_transform = m_gameObject.transform;
		m_effect = effect;
		m_name = ItemInfo.name;
		m_isEffect = false;
	}

	public GameObject GetGameObject()
	{
		return m_gameObject;
	}

	public Transform GetTransform()
	{
		return m_transform;
	}

	public string GetName()
	{
		return m_name;
	}

	public Effect GetItemEffect()
	{
		return m_effect;
	}
}

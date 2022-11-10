using System.Collections.Generic;
using UnityEngine;

public class PrefabCache : MonoBehaviour
{
	private static PrefabCache m_instance;

	private Dictionary<string, Object> m_prefabCache = new Dictionary<string, Object>();

	public static T Load<T>(string path) where T : Object
	{
		return m_instance.DoLoad(path) as T;
	}

	public void Awake()
	{
		m_instance = this;
	}

	private Object DoLoad(string path)
	{
		if (m_prefabCache.ContainsKey(path))
		{
			return m_prefabCache[path];
		}
		Object @object = Resources.Load(path);
		m_prefabCache[path] = @object;
		return @object;
	}
}

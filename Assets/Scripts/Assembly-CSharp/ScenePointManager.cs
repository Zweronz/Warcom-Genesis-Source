using System.Collections.Generic;
using UnityEngine;

public class ScenePointManager : MonoBehaviour
{
	private static ScenePointManager instance;

	private Dictionary<string, Transform> m_scenePointMap = new Dictionary<string, Transform>();

	public static Transform GetScenePointTransform(string name)
	{
		return Instance().DoGetScenePoint(name);
	}

	public static string GetScenePointNameRandom()
	{
		int num = Random.Range(0, Instance().m_scenePointMap.Count);
		int num2 = 0;
		string result = null;
		foreach (KeyValuePair<string, Transform> item in Instance().m_scenePointMap)
		{
			num2++;
			result = item.Key;
			if (num2 == num)
			{
				return result;
			}
		}
		return result;
	}

	public static Transform[] GetScenePointTransform(string[] name)
	{
		Transform[] array = new Transform[name.Length];
		for (int i = 0; i < name.Length; i++)
		{
			array[i] = Instance().DoGetScenePoint(name[i]);
		}
		return array;
	}

	public static Vector3 GetScenePointVector3(string name)
	{
		return Instance().DoGetScenePoint(name).position;
	}

	public static Vector3[] GetScenePointVector3(string[] name)
	{
		Vector3[] array = new Vector3[name.Length];
		for (int i = 0; i < name.Length; i++)
		{
			array[i] = Instance().DoGetScenePoint(name[i]).position;
		}
		return array;
	}

	private static ScenePointManager Instance()
	{
		return instance;
	}

	public void Awake()
	{
		instance = this;
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			m_scenePointMap.Add(componentsInChildren[i].name, componentsInChildren[i]);
		}
	}

	private Transform DoGetScenePoint(string name)
	{
		if (m_scenePointMap.ContainsKey(name))
		{
			return m_scenePointMap[name];
		}
		return null;
	}
}

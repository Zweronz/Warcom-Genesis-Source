using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilLevelPlayer : MonoBehaviour
{
	[Serializable]
	public class OnDestroyCallFunction
	{
		public GameObject m_gameObject;

		public string m_functionName;

		public object m_param;
	}

	public List<OnDestroyCallFunction> m_function = new List<OnDestroyCallFunction>();

	private void OnDestroy()
	{
		foreach (OnDestroyCallFunction item in m_function)
		{
			if (item.m_gameObject == null)
			{
				Debug.LogWarning("GameObject Null");
			}
			else
			{
				item.m_gameObject.SendMessage(item.m_functionName, item.m_param);
			}
		}
	}

	public void Register(GameObject gameObject, string functionName, object param)
	{
		OnDestroyCallFunction onDestroyCallFunction = new OnDestroyCallFunction();
		onDestroyCallFunction.m_gameObject = gameObject;
		onDestroyCallFunction.m_functionName = functionName;
		onDestroyCallFunction.m_param = param;
		m_function.Add(onDestroyCallFunction);
	}
}

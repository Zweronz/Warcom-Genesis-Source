using System.Collections.Generic;
using UnityEngine;

public class UtilUILevelTips : MonoBehaviour
{
	public UtilUILevelTipsText[] m_tipsPool;

	private Stack<string> m_tipsTextStack = new Stack<string>();

	private float m_time;

	private int m_step;

	public void Start()
	{
		m_tipsTextStack.Clear();
		m_time = 0f;
		m_step = 0;
	}

	public void Update()
	{
		if (m_tipsTextStack.Count != 0)
		{
			m_time += Time.deltaTime;
			if (m_time > 0.5f)
			{
				m_tipsPool[m_step].SetText(m_tipsTextStack.Pop());
				m_step++;
				m_time = 0f;
			}
			if (m_step > 2)
			{
				m_step = 0;
			}
		}
	}

	public void PushTipsInStack(string tips)
	{
		if (m_tipsTextStack.Count < 6)
		{
			m_tipsTextStack.Push(tips);
		}
	}
}

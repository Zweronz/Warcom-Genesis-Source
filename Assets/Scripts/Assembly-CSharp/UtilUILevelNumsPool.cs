using System.Collections.Generic;
using UnityEngine;

public class UtilUILevelNumsPool : MonoBehaviour
{
	public Queue<Nums> m_hNums = new Queue<Nums>();

	public Queue<Nums> m_nums = new Queue<Nums>();

	public GameObject[] m_headShotNumsObj;

	public GameObject[] m_numsObj;

	private int m_hNumsStep;

	private int m_numsStep;

	private void Start()
	{
		m_hNums.Clear();
		m_nums.Clear();
		m_hNumsStep = 0;
		m_numsStep = 0;
	}

	private void Update()
	{
		if (m_hNums.Count != 0)
		{
			m_headShotNumsObj[m_hNumsStep].GetComponent<UtilUILevelNums>().SetNums(m_hNums.Dequeue());
			m_hNumsStep++;
			if (m_hNumsStep > 12)
			{
				m_hNumsStep = 0;
			}
		}
		if (m_nums.Count != 0)
		{
			m_numsObj[m_numsStep].GetComponent<UtilUILevelNums>().SetNums(m_nums.Dequeue());
			m_numsStep++;
			if (m_numsStep > 12)
			{
				m_numsStep = 0;
			}
		}
	}

	public void HInQueue(Nums num)
	{
		m_hNums.Enqueue(num);
	}

	public void InQueue(Nums num)
	{
		m_nums.Enqueue(num);
	}
}

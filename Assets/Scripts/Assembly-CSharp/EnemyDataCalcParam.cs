using UnityEngine;

public class EnemyDataCalcParam
{
	public float m_playerBaseHp;

	public float m_playerDeadTimeBegin;

	public float m_playerDeadTimeEnd;

	public float m_maxEnemyShotRate;

	public float m_maxEnemyHeadshotRate;

	public int m_maxEnemyInFightCount;

	public void EnemyDataCalc(int attrDMG, int attrRPM, int week, ref float shotRate, ref float headShotRate)
	{
		float num = Mathf.Max(m_playerDeadTimeEnd, m_playerDeadTimeBegin - (float)(week - 1) * (m_playerDeadTimeBegin - m_playerDeadTimeEnd) / 49f);
		shotRate = Mathf.Min(m_maxEnemyShotRate, Mathf.Ceil(m_playerBaseHp / (float)attrDMG) / (num / (60f / (float)attrRPM)) / (float)m_maxEnemyInFightCount);
		if (week >= 19)
		{
			headShotRate = Mathf.Min(m_maxEnemyHeadshotRate, 60f / (float)attrRPM / num / (float)m_maxEnemyInFightCount);
		}
		else
		{
			headShotRate = 0f;
		}
	}
}

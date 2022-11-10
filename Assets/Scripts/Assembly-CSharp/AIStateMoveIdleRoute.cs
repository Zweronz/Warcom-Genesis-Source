using UnityEngine;

public class AIStateMoveIdleRoute : AIStateContainerFSM
{
	private class AIStateSelectPath : AIState
	{
		private Vector4[] m_path;

		private int m_pathIndex = -1;

		public void SetPath(Vector4[] path)
		{
			m_path = path;
		}

		public int GetPathIndex()
		{
			return m_pathIndex;
		}

		public override string Action(NPC npc, float deltaTime)
		{
			if (m_path == null)
			{
				return null;
			}
			Vector3 position = npc.GetTransform().position;
			for (int i = 0; i < m_path.Length; i++)
			{
				Vector3 vector = new Vector3(m_path[i].x, m_path[i].y, m_path[i].z);
				if ((vector - position).sqrMagnitude < m_path[i].w * m_path[i].w)
				{
					m_pathIndex = i + 1;
					return "SelectPath.Complete";
				}
			}
			m_pathIndex = -1;
			return null;
		}
	}

	private Vector4[] m_path;

	private bool m_loop;

	private int m_direction = 1;

	private int m_wanderTimesMin = 2;

	private int m_wanderTimesMax = 4;

	private float m_stayTimeMin = 1f;

	private float m_stayTimeMax = 2f;

	private int m_pathIndex = -1;

	private int m_pathDirection = 1;

	private int m_wanderTimesTotal;

	private int m_wanderTimesCurrent;

	private AIStateSelectPath m_stateSelectPath;

	private AIStateCommonMove m_stateMove;

	private AIStateCommonStay m_stateStay;

	private AIStateCommonWander m_stateWander;

	public AIStateMoveIdleRoute()
	{
		m_stateSelectPath = new AIStateSelectPath();
		Add(string.Empty, m_stateSelectPath, true);
		m_stateMove = new AIStateCommonMove();
		Add("SelectPath.Complete", m_stateMove);
		m_stateStay = new AIStateCommonStay();
		m_stateStay.SetStayTime(m_stayTimeMin, m_stayTimeMax);
		Add("CommonMove.Complete", m_stateStay);
		m_stateWander = new AIStateCommonWander();
		m_stateWander.SetStayTime(m_stayTimeMin, m_stayTimeMax);
		Add("CommonStay.Complete", m_stateWander);
		Add("CommonWander.Complete", m_stateWander);
	}

	public void SetPath(Vector4[] path, bool loop)
	{
		SetPath(path, loop, 1);
	}

	public void SetPath(Vector4[] path, bool loop, int direction)
	{
		m_path = path;
		m_loop = loop;
		m_direction = direction;
		m_stateSelectPath.SetPath(m_path);
	}

	public void SetWanderTimes(int wanderTimesMin, int wanderTimesMax)
	{
		m_wanderTimesMin = wanderTimesMin;
		m_wanderTimesMax = wanderTimesMax;
	}

	public void SetWanderTimes(float stayTimeMin, float stayTimeMax)
	{
		m_stayTimeMin = stayTimeMin;
		m_stayTimeMax = stayTimeMax;
		m_stateStay.SetStayTime(m_stayTimeMin, m_stayTimeMax);
		m_stateWander.SetStayTime(m_stayTimeMin, m_stayTimeMax);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		if (m_currentAI == null)
		{
			return null;
		}
		string text = m_currentAI.Action(npc, deltaTime);
		if (text == null)
		{
			return null;
		}
		m_currentAI.Exit(npc);
		m_currentAI = null;
		switch (text)
		{
		case "SelectPath.Complete":
			m_pathIndex = m_stateSelectPath.GetPathIndex();
			m_pathDirection = m_direction;
			UpdatePath();
			break;
		case "CommonStay.Complete":
			m_wanderTimesTotal = Random.Range(m_wanderTimesMin, m_wanderTimesMax);
			m_wanderTimesCurrent = 0;
			break;
		case "CommonWander.Complete":
			m_wanderTimesCurrent++;
			if (m_wanderTimesCurrent >= m_wanderTimesTotal)
			{
				text = NextPath();
				UpdatePath();
			}
			break;
		}
		if (m_ai.ContainsKey(text))
		{
			m_currentAI = m_ai[text];
			m_currentAI.Enter(npc);
			return null;
		}
		return text;
	}

	private string NextPath()
	{
		string result = "SelectPath.Complete";
		if (m_pathDirection > 0)
		{
			m_pathIndex++;
			if (m_pathIndex >= m_path.Length)
			{
				if (m_loop)
				{
					m_pathIndex = 0;
				}
				else
				{
					result = "MoveIdleRoute.Complete";
				}
			}
		}
		else
		{
			m_pathIndex--;
			if (m_pathIndex < 0)
			{
				if (m_loop)
				{
					m_pathIndex = m_path.Length - 1;
				}
				else
				{
					result = "MoveIdleRoute.Complete";
				}
			}
		}
		m_pathIndex = Mathf.Clamp(m_pathIndex, 0, m_path.Length - 1);
		return result;
	}

	private void UpdatePath()
	{
		Vector3 vector = new Vector3(m_path[m_pathIndex].x, m_path[m_pathIndex].y, m_path[m_pathIndex].z);
		m_stateMove.SetTarget(vector);
		m_stateWander.SetBorder(vector, m_path[m_pathIndex].w);
	}
}

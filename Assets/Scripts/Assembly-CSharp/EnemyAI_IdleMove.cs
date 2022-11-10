using UnityEngine;

public class EnemyAI_IdleMove : AIStateContainerFSM
{
	private AIStateMoveIdleReturn m_stateReturn;

	private AIStateMoveIdleWander m_stateWander;

	private AIStateMoveIdleRoute m_stateRoute;

	private AIStateMoveIdleChase m_stateChase;

	public EnemyAI_IdleMove()
	{
		m_stateReturn = new AIStateMoveIdleReturn();
		Add(string.Empty, m_stateReturn, true);
	}

	public void SetReturnTarget(Vector3 target)
	{
		m_stateReturn.SetTarget(target);
	}

	public void InitializeModeWander(Vector3 borderCenter, float borderRadius)
	{
		m_stateWander = new AIStateMoveIdleWander();
		m_stateWander.SetBorder(borderCenter, borderRadius);
		Add("MoveIdleReturn.Complete", m_stateWander);
	}

	public void InitializeModeRoute()
	{
		m_stateRoute = new AIStateMoveIdleRoute();
		Add("MoveIdleReturn.Complete", m_stateRoute);
	}

	public void InitializeModeChase()
	{
		m_stateChase = new AIStateMoveIdleChase();
		Add("MoveIdleReturn.Complete", m_stateChase);
	}

	public override string Action(NPC npc, float deltaTime)
	{
		string text = base.Action(npc, deltaTime);
		if (text == "MoveIdleRoute.Complete")
		{
			return "MoveRoute.Complete";
		}
		return text;
	}

	public void SetRouteParams(float stayTime, bool loop, Vector3[] path)
	{
		if (path != null && path.Length != 0 && m_stateRoute != null)
		{
			Vector4[] array = new Vector4[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i];
				array[i].w = 2f;
			}
			m_stateRoute.SetPath(array, loop);
			m_stateRoute.SetWanderTimes(stayTime, stayTime);
		}
	}
}

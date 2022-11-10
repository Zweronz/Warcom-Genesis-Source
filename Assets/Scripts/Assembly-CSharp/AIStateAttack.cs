using UnityEngine;

public class AIStateAttack : AIState
{
	private float m_time;

	private bool stopFire;

	public override void Enter(NPC npc)
	{
	}

	public override void Exit(NPC npc)
	{
		npc.SetFire(false, false);
		npc.ResetPitckUpDown(npc.GetWeapon());
	}

	public override string Action(NPC npc, float deltaTime)
	{
		m_time += deltaTime;
		if (m_time > 1f)
		{
			m_time = 0f;
			int num = Mathf.Max(DataCenter.StateSingle().GetLevelInfo().weekFluctuation + DataCenter.Save().GetWeek(), 0);
			float num2 = 0.15f;
			float num3 = Random.Range(0f, 1f);
			if (num3 < num2)
			{
				stopFire = true;
			}
			else
			{
				stopFire = false;
			}
		}
		npc.m_hitStopFireInterveTime -= deltaTime;
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player != null && GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.NormalMode)
		{
			Vector3 from = player.GetTransform().position - npc.GetTransform().position;
			Vector3 vector = new Vector3(from.x, 0f, from.z);
			float num4 = Vector3.Angle(from, vector);
			if (from.y > 0f)
			{
				npc.SetPitchAngle(0f - num4);
			}
			else
			{
				npc.SetPitchAngle(num4);
			}
			if (Vector3.Angle(vector, npc.GetTransform().forward) < 10f && npc.m_hitStopFireInterveTime <= 0f && !stopFire)
			{
				npc.SetFire(true, true);
				return null;
			}
			npc.SetFire(false, false);
		}
		return null;
	}
}

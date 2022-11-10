using UnityEngine;

public class AIStateInfect : AIState
{
	public Vector3 m_infectStartPos;

	public override string Action(NPC npc, float deltaTime)
	{
		foreach (CharacterEvent @event in npc.GetEventList())
		{
			if (@event.type == "DetectPlayer" && !@event.response)
			{
				m_infectStartPos = new Vector3(float.Parse(@event.param1), float.Parse(@event.param2), float.Parse(@event.param3));
				@event.response = true;
				return "Infect.Yes";
			}
		}
		return null;
	}
}

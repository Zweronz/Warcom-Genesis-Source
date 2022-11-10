using UnityEngine;

public class CharacterEventManager
{
	private static CharacterEventManager m_instance;

	public static CharacterEventManager Instance()
	{
		if (m_instance == null)
		{
			m_instance = new CharacterEventManager();
		}
		return m_instance;
	}

	public void PostDetectPlayer(Vector3 position)
	{
		CharacterEvent characterEvent = new CharacterEvent();
		characterEvent.type = "DetectPlayer";
		characterEvent.time = Time.time;
		characterEvent.timeOut = 3f;
		characterEvent.response = false;
		characterEvent.param1 = position.x.ToString();
		characterEvent.param2 = position.y.ToString();
		characterEvent.param3 = position.z.ToString();
		Collider[] array = Physics.OverlapSphere(position, 9f, 2048);
		for (int i = 0; i < array.Length; i++)
		{
			Character character = CharacterStub.GetCharacter(array[i].gameObject);
			character.PostEvent(characterEvent);
		}
	}
}

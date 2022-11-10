using UnityEngine;

public class CharacterStub : MonoBehaviour
{
	private Character m_character;

	public static void BindCharacter(GameObject gameObject, Character character)
	{
		CharacterStub characterStub = gameObject.AddComponent<CharacterStub>();
		characterStub.m_character = character;
	}

	public static Character GetCharacter(GameObject gameObject)
	{
		CharacterStub component = gameObject.GetComponent<CharacterStub>();
		return component.m_character;
	}
}

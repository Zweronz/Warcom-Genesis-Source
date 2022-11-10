using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
	public void ThrowGrenades()
	{
		GameObject gameObject = Object.Instantiate(PrefabCache.Load<GameObject>("Models/Grenades/Grenades")) as GameObject;
		gameObject.name = "Grenades";
		gameObject.layer = 17;
		gameObject.transform.Find("AmmoExplodeEffect").gameObject.SetActiveRecursively(false);
		Rigidbody component = gameObject.GetComponent<Rigidbody>();
		if (component != null)
		{
			Character character = CharacterStub.GetCharacter(base.gameObject);
			Vector3 vector = Quaternion.Euler(character.GetPitchAngle() - 15f, character.GetTransform().eulerAngles.y, 0f) * Vector3.forward;
			component.AddForce(vector * 16f, ForceMode.Impulse);
		}
		gameObject.transform.position = base.transform.Find("Bip01/Bip01 Spine/Bip01 Prop1").position;
		gameObject.AddComponent<TAudioController>();
		AmmoGrenades ammoGrenades = gameObject.AddComponent<AmmoGrenades>();
		ammoGrenades.m_explodeRadius = 5f;
		ammoGrenades.characterType = AmmoGrenades.CharacterType.Enemy;
	}
}

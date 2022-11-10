using System;
using UnityEngine;

public class AmmoGrenades : MonoBehaviour
{
	public enum CharacterType
	{
		Player = 0,
		Enemy = 1
	}

	public CharacterType characterType;

	public float m_explodeRadius;

	private bool m_explode;

	private float m_lifeTime;

	private float m_lifeTimeLaunch = 1.5f;

	private float m_hitDelayTime;

	public void Update()
	{
		if (m_explode)
		{
			m_lifeTime -= Time.deltaTime;
			if (m_lifeTime < 0f)
			{
				UnityEngine.Object.DestroyObject(base.gameObject);
				return;
			}
		}
		m_lifeTimeLaunch -= Time.deltaTime;
		if (m_lifeTimeLaunch < -2f)
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
		else if (m_lifeTimeLaunch < 0f && !m_explode)
		{
			Explode();
			m_explode = true;
		}
	}

	public void Explode()
	{
		base.transform.Find("Grenades").gameObject.SetActiveRecursively(false);
		base.transform.GetComponent<TAudioController>().PlayAudio("Fx_Explo");
		base.transform.Find("AmmoExplodeEffect").gameObject.SetActiveRecursively(true);
		base.transform.Find("AmmoExplodeEffect").GetComponent<SpecialEffectParticleEmit>().Emit();
		m_lifeTime = 2f;
		Collider[] array = new Collider[20];
		if (characterType == CharacterType.Player)
		{
			array = Physics.OverlapSphere(base.transform.position, m_explodeRadius, 2048);
		}
		else if (characterType == CharacterType.Enemy)
		{
			array = Physics.OverlapSphere(base.transform.position, m_explodeRadius, 512);
		}
		for (int i = 0; i < array.Length; i++)
		{
			Character character = CharacterStub.GetCharacter(array[i].gameObject);
			Character.HitInfo hitInfo = default(Character.HitInfo);
			hitInfo.weaponType = WeaponType.RPG;
			hitInfo.weaponName = base.name;
			float num = Vector3.Distance(character.GetTransform().position, base.transform.position);
			if (num < 1f)
			{
				hitInfo.damage = character.m_HPMax * 0.9f;
			}
			else if (num >= 1f && num < 3f)
			{
				hitInfo.damage = character.m_HPMax * 0.6f;
			}
			else
			{
				hitInfo.damage = character.m_HPMax * 0.3f;
			}
			hitInfo.headShot = false;
			hitInfo.hitPoint = character.GetTransform().position;
			Vector3 vector = character.GetTransform().InverseTransformDirection(base.transform.position - character.GetTransform().position);
			vector.y = 0f;
			hitInfo.hitAngle = Vector3.Angle(character.GetTransform().forward, vector);
			hitInfo.hitLocDic = vector;
			CharacterController component = character.GetTransform().GetComponent<CharacterController>();
			if (component != null)
			{
				Vector2 vector2 = new Vector2(component.radius * Mathf.Cos((float)Math.PI / 180f * hitInfo.hitAngle), component.radius * Mathf.Sin((float)Math.PI / 180f * hitInfo.hitAngle));
				hitInfo.localHitPoint = new Vector3(vector2.x, UnityEngine.Random.Range(0.5f, 1.5f), vector2.y);
			}
			if (characterType == CharacterType.Player)
			{
				Nums num2 = default(Nums);
				num2.loc = new Vector2(Camera.main.WorldToScreenPoint(hitInfo.hitPoint).x - (float)(Screen.width / 2), Camera.main.WorldToScreenPoint(hitInfo.hitPoint).y - (float)(Screen.height / 2));
				num2.num = (int)hitInfo.damage;
				GameObject.FindWithTag("NumsPool").GetComponent<UtilUILevelNumsPool>().InQueue(num2);
			}
			character.OnHit(ref hitInfo);
		}
	}
}

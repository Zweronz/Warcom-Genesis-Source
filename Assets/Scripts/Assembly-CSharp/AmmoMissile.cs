using System;
using UnityEngine;

public class AmmoMissile : MonoBehaviour
{
	public enum CharacterType
	{
		Player = 0,
		Enemy = 1
	}

	public CharacterType characterType;

	public bool leechLauch;

	public Vector3 m_dir;

	public Quaternion m_qua;

	public float m_angleSpeed;

	public float m_explodeRadius;

	public float m_damage;

	public float m_flySpeed;

	private float m_downSpeed;

	private float m_gravity = 0.5f;

	private bool m_explode;

	private float m_lifeTime;

	private float m_lifeTimeLaunch = 8f;

	private float m_hitDelayTime;

	public void Start()
	{
		base.transform.Find("AmmoEffect/effect").GetComponent<SpecialEffectParticleContinuous>().StartEmit();
	}

	public void Update()
	{
		m_downSpeed += m_gravity * Time.deltaTime;
		base.transform.position = base.transform.position + m_dir * Time.deltaTime * m_flySpeed + Vector3.down * Time.deltaTime * m_downSpeed;
		base.transform.right = -m_dir;
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
		if (m_lifeTimeLaunch < 0f)
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
	}

	public void OnTriggerEnter(Collider collider)
	{
		if (m_explode)
		{
			return;
		}
		m_flySpeed = 0f;
		m_downSpeed = 0f;
		m_gravity = 0f;
		base.transform.Find("AmmoExplodeEffect").gameObject.SetActiveRecursively(true);
		base.transform.GetComponent<TAudioController>().PlayAudio("Fx_Explo");
		base.transform.Find("AmmoEffect").gameObject.SetActiveRecursively(false);
		base.transform.Find("AmmoExplodeEffect").GetComponent<SpecialEffectParticleEmit>().Emit();
		m_explode = true;
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
			if (leechLauch)
			{
				hitInfo.leechKill = true;
			}
			else
			{
				hitInfo.leechKill = false;
			}
			float num = Vector3.Distance(character.GetTransform().position, base.transform.position);
			if (num < 2f)
			{
				hitInfo.damage = m_damage;
			}
			else
			{
				hitInfo.damage = m_damage * 0.5f;
			}
			hitInfo.headShot = false;
			hitInfo.hitPoint = character.GetTransform().position + new Vector3(0f, 1.5f, 0f);
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

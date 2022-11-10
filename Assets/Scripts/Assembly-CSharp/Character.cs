using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : CharacterFSM
{
	public class CharacterState : State
	{
		protected Character m_character;

		public CharacterState(Character character)
		{
			m_character = character;
		}
	}

	public class StateMove : CharacterState
	{
		protected CharacterController m_characterController;

		protected string m_downAnimation;

		public StateMove(Character character)
			: base(character)
		{
			m_characterController = m_character.GetGameObject().GetComponent<CharacterController>();
		}

		protected override void OnEnter()
		{
			GameObject gameObject = m_character.GetTransform().Find("shadow").gameObject;
			if (gameObject != null)
			{
				gameObject.SetActiveRecursively(true);
			}
			if (m_character.m_weapon != null)
			{
				m_downAnimation = m_character.m_weapon.GetAnimation() + "_Down_Move_Idle";
				m_character.AnimationPlay(m_downAnimation, true);
			}
			Push(m_character.m_stateIdle);
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
			string text = m_character.m_weapon.GetAnimation() + GetDownAnimation();
			if (m_downAnimation != text)
			{
				m_downAnimation = text;
				m_character.AnimationCrossFade(m_downAnimation, true, 0.3f);
			}
			if (m_character.m_UIControlMove)
			{
				if (m_character.m_move)
				{
					Vector3 direction = new Vector3(m_character.m_moveDirection.x, 0f, m_character.m_moveDirection.y);
					m_characterController.Move(m_character.GetTransform().TransformDirection(direction) * deltaTime * m_character.m_moveSpeed * m_character.m_factorMoveSpeed);
				}
				else
				{
					Vector3 direction2 = new Vector3(0f, 0f, 0f);
					m_characterController.Move(m_character.GetTransform().TransformDirection(direction2) * deltaTime * m_character.m_moveSpeed * m_character.m_factorMoveSpeed);
				}
				Vector3 position = m_character.GetTransform().position;
				Ray ray = new Ray(position + new Vector3(0f, 0.5f, 0f), new Vector3(0f, -1f, 0f));
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, 10f, (1 << LayerMask.NameToLayer("Obstacle")) | (1 << LayerMask.NameToLayer("Ground"))))
				{
					if (hitInfo.point.y > position.y)
					{
						m_character.GetTransform().position = hitInfo.point;
					}
					else if (position.y - hitInfo.point.y < 0.1f)
					{
						m_character.GetTransform().position = hitInfo.point;
					}
					else
					{
						position.y -= deltaTime * 8f;
						position.y = Mathf.Max(position.y, hitInfo.point.y);
						m_character.GetTransform().position = position;
					}
				}
			}
			if (m_character.m_weaponChange != null)
			{
				m_character.DoChangeWeapon();
				Pop();
				Push(m_character.m_stateChange);
			}
			m_character.PitchUpDown(m_character.m_weapon);
		}

		private string GetDownAnimation()
		{
			if (!m_character.m_move)
			{
				return "_Down_Move_Idle";
			}
			if (Mathf.Abs(m_character.m_moveDirection.y) >= Mathf.Abs(m_character.m_moveDirection.x))
			{
				if (m_character.m_moveDirection.y >= 0f)
				{
					return "_Down_Move_Forward";
				}
				return "_Down_Move_Backward";
			}
			if (m_character.m_moveDirection.x >= 0f)
			{
				return "_Down_Move_Right";
			}
			return "_Down_Move_Left";
		}
	}

	public class StateIdle : CharacterState
	{
		private float m_glanceTime;

		public StateIdle(Character character)
			: base(character)
		{
		}

		protected override void OnEnter()
		{
			m_glanceTime = UnityEngine.Random.Range(5f, 10f);
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (!m_character.m_move)
			{
				m_glanceTime -= deltaTime;
				if (m_glanceTime < 0f)
				{
					string text = m_character.m_weapon.GetAnimation() + "_Up_Glance";
					m_character.AnimationPlay(text, false);
					if (m_character.m_characterType == CharacterType.Sniper && (double)m_character.GetPitchUpDownWeight(m_character.m_weapon) < 0.4)
					{
						m_character.AnimationPlay(text.Replace("Up", "Cloak"), false);
					}
					m_glanceTime = UnityEngine.Random.Range(5f, 10f);
				}
			}
			if (m_character.m_throw)
			{
				NextState(m_character.m_stateThrow);
			}
			else if (m_character.m_fire)
			{
				NextState(m_character.m_stateFire);
			}
			else if (m_character.m_reload || m_character.m_weapon.NeedReload())
			{
				NextState(m_character.m_stateReload);
			}
		}
	}

	public class StateFire : CharacterState
	{
		public float m_totalTime;

		public float m_fireTime;

		public float m_hitTime;

		public float m_time;

		private int m_count;

		private bool m_updateFireFinish;

		private string m_fireAnimation;

		private bool m_animIsFire;

		public StateFire(Character character)
			: base(character)
		{
		}

		protected override void OnEnter()
		{
			if (m_character.m_weapon.GetAnimation() != "Knife")
			{
				m_fireAnimation = m_character.m_weapon.GetAnimation() + "_Up_Fire";
			}
			else
			{
				m_fireAnimation = "Knife_Up_Fire_0" + (m_count % 2 + 1);
			}
			m_totalTime = m_character.m_weapon.GetFireIntervalTime();
			if (m_character.m_weapon.GetWeaponType() == WeaponType.Knife)
			{
				m_totalTime *= m_character.m_factorColdWeaponFireSpeed;
			}
			else if (m_character.m_weapon.GetWeaponType() == WeaponType.SniperRifle)
			{
				m_totalTime *= m_character.m_factorSniperFireSpeed;
			}
			m_fireTime = Mathf.Clamp(m_character.AnimationLength(m_fireAnimation), 0f, m_totalTime);
			m_hitTime = m_fireTime * m_character.m_weapon.GetHitTimePer();
			m_time = 0f;
			m_updateFireFinish = false;
			m_character.AnimationPlay(m_fireAnimation, false);
			if (m_character.m_characterType == CharacterType.Sniper && !m_character.m_move)
			{
				m_character.AnimationPlay(m_fireAnimation.Replace("Up", "Cloak"), false);
			}
			m_animIsFire = true;
			m_character.m_weapon.StartFire();
		}

		protected override void OnExit()
		{
			if (m_animIsFire)
			{
				m_character.AnimationStop(m_fireAnimation);
				m_animIsFire = false;
			}
			m_character.m_weapon.StopFire();
			m_count = 0;
		}

		protected override void OnUpdate(float deltaTime)
		{
			m_character.m_HPRecoverTime = 5f * m_character.m_factorRecoverTime;
			m_time += deltaTime;
			if (m_time < m_totalTime)
			{
				if (m_time < m_fireTime)
				{
					if (m_time > m_hitTime && !m_updateFireFinish)
					{
						m_character.m_weapon.UpdateFire();
						m_updateFireFinish = true;
						m_count++;
					}
				}
				else if (m_animIsFire)
				{
					m_character.AnimationStop(m_fireAnimation);
					if (m_character.m_weapon.GetAnimation() == "Sniper")
					{
						m_character.AnimationPlay("Sniper_Up_Aftershoot", false);
						m_character.AnimationCrossFade("Sniper_Cloak_Aftershoot", false, 0.3f);
					}
					m_animIsFire = false;
				}
				return;
			}
			if (m_animIsFire)
			{
				m_character.AnimationStop(m_fireAnimation);
				if (m_character.m_weapon.GetAnimation() == "Sniper")
				{
					m_character.AnimationPlay("Sniper_Up_Aftershoot", false);
					m_character.AnimationCrossFade("Sniper_Cloak_Aftershoot", false, 0.3f);
				}
				m_animIsFire = false;
			}
			else
			{
				m_time = 0f;
				if (m_character.m_fireContinuous)
				{
					if (m_character.m_weapon.GetAnimation() != "Knife")
					{
						m_fireAnimation = m_character.m_weapon.GetAnimation() + "_Up_Fire";
					}
					else
					{
						m_fireAnimation = "Knife_Up_Fire_0" + (m_count % 2 + 1);
					}
					m_character.AnimationPlay(m_fireAnimation, false);
					if (m_character.m_characterType == CharacterType.Sniper && !m_character.m_move)
					{
						m_character.AnimationPlay(m_fireAnimation.Replace("Up", "Cloak"), false);
					}
					m_animIsFire = true;
					m_updateFireFinish = false;
				}
				else
				{
					m_character.m_fire = false;
				}
			}
			if (m_character.m_throw)
			{
				NextState(m_character.m_stateThrow);
			}
			else if (m_character.m_weapon.NeedReload() || m_character.m_reload)
			{
				NextState(m_character.m_stateReload);
			}
			else if (!m_character.m_fire)
			{
				NextState(m_character.m_stateIdle);
			}
		}
	}

	public class StateReload : CharacterState
	{
		private float m_length;

		private float m_time;

		public StateReload(Character character)
			: base(character)
		{
		}

		protected override void OnEnter()
		{
			m_character.m_reloading = true;
			string text = m_character.m_weapon.GetAnimation() + "_Up_Reload";
			m_character.AnimationPlay(text, false);
			if (m_character.m_characterType == CharacterType.Sniper)
			{
				m_character.AnimationPlay(text.Replace("Up", "Cloak"), false);
			}
			m_length = m_character.AnimationLength(text);
			m_time = 0f;
		}

		protected override void OnExit()
		{
			m_character.m_reloading = false;
			m_character.CloseReload();
			m_character.ResetPitckUpDownWeight(m_character.m_weapon);
		}

		protected override void OnUpdate(float deltaTime)
		{
			m_time += deltaTime;
			if (!(m_time < m_length))
			{
				m_character.m_weapon.Reload();
				if (m_character.m_fire)
				{
					NextState(m_character.m_stateFire);
				}
				else
				{
					NextState(m_character.m_stateIdle);
				}
			}
		}
	}

	public class StateThrow : CharacterState
	{
		private float m_length;

		private float m_time;

		public StateThrow(Character character)
			: base(character)
		{
		}

		protected override void OnEnter()
		{
			m_character.m_throwing = true;
			m_character.GetWeapon().SetActive(false);
			string text = "Up_Throw";
			m_character.AnimationPlay(text, false);
			if (m_character.m_characterType == CharacterType.Sniper)
			{
				m_character.AnimationPlay(text.Replace("Up", "Cloak"), false);
			}
			m_length = m_character.AnimationLength(text);
			m_time = 0f;
		}

		protected override void OnExit()
		{
			m_character.m_throwing = false;
			m_character.GetWeapon().SetActive(true);
			m_character.CloseThrow();
			m_character.ResetPitckUpDownWeight(m_character.m_weapon);
		}

		protected override void OnUpdate(float deltaTime)
		{
			m_time += deltaTime;
			if (!(m_time < m_length))
			{
				if (m_character.m_fire)
				{
					NextState(m_character.m_stateFire);
				}
				else
				{
					NextState(m_character.m_stateIdle);
				}
			}
		}
	}

	public class StateChange : CharacterState
	{
		private float m_length;

		private float m_time;

		public StateChange(Character character)
			: base(character)
		{
		}

		protected override void OnEnter()
		{
			m_character.m_changing = true;
			string text = m_character.m_weapon.GetAnimation() + "_Up_Change";
			m_character.AnimationPlay(text, false);
			if (m_character.m_characterType == CharacterType.Sniper)
			{
				m_character.AnimationPlay(text.Replace("Up", "Cloak"), false);
			}
			m_length = m_character.AnimationLength(text);
			m_time = 0f;
		}

		protected override void OnExit()
		{
			m_character.m_changing = false;
			m_character.ResetPitckUpDownWeight(m_character.m_weapon);
		}

		protected override void OnUpdate(float deltaTime)
		{
			m_time += deltaTime;
			if (!(m_time < m_length))
			{
				if (m_character.m_fire)
				{
					NextState(m_character.m_stateFire);
				}
				else
				{
					NextState(m_character.m_stateIdle);
				}
			}
		}
	}

	public class StateDeath : CharacterState
	{
		private bool m_headShot;

		private float m_length;

		private float m_time;

		public StateDeath(Character character, bool headShot)
			: base(character)
		{
			m_headShot = headShot;
		}

		protected override void OnEnter()
		{
			m_character.OnDeath();
			m_character.ResetPitckUpDown(m_character.GetWeapon());
			m_character.AnimationStop();
			GameObject gameObject = m_character.GetTransform().Find("shadow").gameObject;
			if (gameObject != null)
			{
				gameObject.SetActiveRecursively(false);
			}
			string name = ((!m_headShot) ? (m_character.m_weapon.GetAnimation() + "_Up_Death_0" + (UnityEngine.Random.Range(1, 9) % 3 + 1)) : (m_character.m_weapon.GetAnimation() + "_Up_HeadShot_0" + (UnityEngine.Random.Range(1, 4) % 2 + 1)));
			m_character.AnimationPlay(name, false);
			m_length = m_character.AnimationLength(name);
			m_time = 0f;
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
			m_time += deltaTime;
			if (!(m_time < m_length))
			{
				NextState(m_character.m_stateDestroy);
			}
		}
	}

	public class StateDestroy : CharacterState
	{
		private float m_time;

		public StateDestroy(Character character)
			: base(character)
		{
		}

		protected override void OnEnter()
		{
			m_time = 0f;
		}

		protected override void OnExit()
		{
			m_character.OnDestroy();
		}

		protected override void OnUpdate(float deltaTime)
		{
			m_time += deltaTime;
			if (m_time > 6f)
			{
				NextState(null);
			}
		}
	}

	public class StateLose : CharacterState
	{
		private float m_lengthIdle;

		private float m_length;

		private float m_time;

		private string m_currentAnim;

		public StateLose(Character character)
			: base(character)
		{
		}

		protected override void OnEnter()
		{
			m_character.ResetPitckUpDown(m_character.GetWeapon());
			m_character.AnimationStop();
			m_currentAnim = m_character.m_weapon.GetAnimation() + "_Up_LoseIdle";
			m_character.AnimationPlay(m_currentAnim, true);
			m_lengthIdle = m_character.AnimationLength(m_currentAnim);
			m_length = m_character.AnimationLength(m_character.m_weapon.GetAnimation() + "_Up_Lose");
			m_time = 0f;
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
			m_time += deltaTime;
			if (m_currentAnim.Contains("_Up_LoseIdle"))
			{
				if (!(m_time < m_lengthIdle))
				{
					m_time = 0f;
					m_currentAnim = m_character.m_weapon.GetAnimation() + "_Up_Lose";
					m_character.AnimationPlay(m_currentAnim, false);
				}
			}
			else if (!(m_time < m_length))
			{
				m_time = 0f - UnityEngine.Random.Range(5f, 10f);
				m_currentAnim = m_character.m_weapon.GetAnimation() + "_Up_LoseIdle";
				m_character.AnimationPlay(m_currentAnim, true);
			}
		}
	}

	public class StateWin : CharacterState
	{
		private float m_lengthIdle;

		private float m_length;

		private float m_time;

		private string m_currentAnim;

		public StateWin(Character character)
			: base(character)
		{
		}

		protected override void OnEnter()
		{
			m_character.ResetPitckUpDown(m_character.GetWeapon());
			m_character.AnimationStop();
			m_currentAnim = m_character.m_weapon.GetAnimation() + "_Up_WinIdle";
			m_character.AnimationPlay(m_currentAnim, true);
			m_lengthIdle = m_character.AnimationLength(m_currentAnim);
			m_length = m_character.AnimationLength(m_character.m_weapon.GetAnimation() + "_Up_Win");
			m_time = 0f;
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
			m_time += deltaTime;
			if (m_currentAnim.Contains("_Up_WinIdle"))
			{
				if (!(m_time < m_lengthIdle))
				{
					m_time = 0f;
					m_currentAnim = m_character.m_weapon.GetAnimation() + "_Up_Win";
					m_character.AnimationPlay(m_currentAnim, false);
				}
			}
			else if (!(m_time < m_length))
			{
				m_time = 0f - UnityEngine.Random.Range(5f, 10f);
				m_currentAnim = m_character.m_weapon.GetAnimation() + "_Up_WinIdle";
				m_character.AnimationPlay(m_currentAnim, true);
			}
		}
	}

	public struct HitInfo
	{
		public WeaponType weaponType;

		public string weaponName;

		public float damage;

		public Vector3 hitPoint;

		public Vector3 localHitPoint;

		public bool headShot;

		public float hitAngle;

		public Vector3 hitLocDic;

		public bool leechKill;
	}

	protected GameObject m_gameObject;

	protected Transform m_transform;

	protected CharacterType m_characterType;

	protected string m_modelName;

	protected Collider m_headCollider;

	public List<CharacterBuff> m_buff;

	protected List<CharacterEvent> m_event;

	private float m_pitchAngle;

	private bool m_UIControlMove;

	private bool m_move;

	private Vector2 m_moveDirection;

	public float m_moveSpeed;

	public float m_moveFactor;

	public bool m_dead;

	public bool m_headShot;

	private bool m_fire;

	private bool m_fireContinuous;

	private bool m_throw;

	private bool m_reload;

	public bool m_throwing;

	public bool m_reloading;

	public bool m_changing;

	private bool m_lose;

	private bool m_win;

	protected Dictionary<int, Weapon> m_weaponMap;

	protected Weapon m_weapon;

	protected Weapon m_weaponChange;

	protected int m_weaponChangeId;

	public float m_HPMax;

	public float m_HPCurrent;

	public float m_HPRecoverSpeed;

	public float m_HPRecoverTime;

	public CharacterEffectBloodEmitPool m_characterEffectBloodEmitPool;

	public CharacterEffectItemEmit m_characterEffectItemEmit;

	private StateMove m_stateMove;

	private StateIdle m_stateIdle;

	private StateFire m_stateFire;

	private StateReload m_stateReload;

	private StateThrow m_stateThrow;

	private StateChange m_stateChange;

	private StateDeath m_stateDeath;

	private StateDeath m_stateHeadShot;

	private StateDestroy m_stateDestroy;

	private StateLose m_stateLose;

	private StateWin m_stateWin;

	public float m_factorHpMax = 1f;

	public float m_factorHitDamage = 1f;

	public float m_factorHpRecover = 1f;

	public float m_factorMoveSpeed = 1f;

	public float m_factorWeaponAcc = 1f;

	public float m_factorWeaponAtk = 1f;

	public float m_factorWeaponReloadSpeed = 1f;

	public float m_factorHandGunAtk = 1f;

	public float m_factorAssaultAcc = 1f;

	public float m_factorSubmachineCapacity = 1f;

	public float m_factorShotGunAtk = 1f;

	public float m_factorRpgMissileAtkRange = 1f;

	public float m_factorSniperFireSpeed = 1f;

	public float m_factorColdWeaponFireSpeed = 1f;

	public float m_factorCharacter01 = 1f;

	public float m_factorCharacter02 = 1f;

	public float m_factorCharacter03 = 1f;

	public float m_factorCharacter04 = 1f;

	public float m_factorAmmoTotal = 1f;

	public float m_factorExp = 1f;

	public float m_factorRecoverItemEffect = 1f;

	public float m_factorRecoverTime = 1f;

	public float m_factorHeadShotLucky = 1f;

	public bool m_armorNotUseBullet;

	public void SetCharacterType(CharacterType characterType)
	{
		m_characterType = characterType;
	}

	public void SetModelName(string modelName)
	{
		m_modelName = modelName;
	}

	public List<CharacterEvent> GetEventList()
	{
		return m_event;
	}

	public bool GetIsFire()
	{
		return m_fire;
	}

	public virtual void OpenThrow()
	{
		m_throw = true;
	}

	public virtual void CloseThrow()
	{
		m_throw = false;
	}

	public void SetLose(bool lose)
	{
		if (lose)
		{
			SwitchFSM(m_stateLose);
		}
		else
		{
			SwitchFSM(m_stateMove);
		}
	}

	public void SetWin(bool win)
	{
		if (win)
		{
			SwitchFSM(m_stateWin);
		}
		else
		{
			SwitchFSM(m_stateMove);
		}
	}

	public int GetChangeWeaponId()
	{
		return m_weaponChangeId;
	}

	public StateFire GetCharacterStateFire()
	{
		return m_stateFire;
	}

	public virtual void Initialize(GameObject prefab, string name, Vector3 position, float direction)
	{
		m_gameObject = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
		m_gameObject.name = name;
		m_gameObject.transform.eulerAngles = new Vector3(0f, direction, 0f);
		CharacterStub.BindCharacter(m_gameObject, this);
		m_transform = m_gameObject.transform;
		m_headCollider = m_transform.Find("Bip01/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head/Head").gameObject.GetComponent<BoxCollider>();
		m_event = new List<CharacterEvent>();
		m_buff = new List<CharacterBuff>();
		m_pitchAngle = 0f;
		m_UIControlMove = false;
		m_move = false;
		m_moveDirection = Vector2.zero;
		m_moveSpeed = 3f;
		m_fire = false;
		m_fireContinuous = false;
		m_reload = false;
		m_weaponMap = new Dictionary<int, Weapon>();
		m_weapon = null;
		m_weaponChange = null;
		m_stateMove = new StateMove(this);
		m_stateIdle = new StateIdle(this);
		m_stateFire = new StateFire(this);
		m_stateReload = new StateReload(this);
		m_stateThrow = new StateThrow(this);
		m_stateChange = new StateChange(this);
		m_stateDeath = new StateDeath(this, false);
		m_stateHeadShot = new StateDeath(this, true);
		m_stateDestroy = new StateDestroy(this);
		m_stateLose = new StateLose(this);
		m_stateWin = new StateWin(this);
		SwitchFSM(m_stateMove);
	}

	public virtual void Update(float deltaTime)
	{
		UpdateFSM(deltaTime);
		DeletOutTimeEvent();
		DeletOutTimeBuff();
	}

	public GameObject GetGameObject()
	{
		return m_gameObject;
	}

	public Transform GetTransform()
	{
		return m_transform;
	}

	public Collider GetHeadCollider()
	{
		return m_headCollider;
	}

	public void AnimationSyncLayer(int layer)
	{
		m_gameObject.GetComponent<Animation>().SyncLayer(layer);
	}

	public bool AnimationIsPlaying(string name)
	{
		return m_gameObject.GetComponent<Animation>().IsPlaying(name);
	}

	public void AnimationPlay(string name, bool loop)
	{
		m_gameObject.GetComponent<Animation>()[name].wrapMode = ((!loop) ? WrapMode.Once : WrapMode.Loop);
		m_gameObject.GetComponent<Animation>().Play(name);
	}

	public void AnimationSmooth(string lastAnim, string nextAnim)
	{
		m_gameObject.GetComponent<Animation>()[nextAnim].weight = 1f - m_gameObject.GetComponent<Animation>()[lastAnim].weight;
		m_gameObject.GetComponent<Animation>()[lastAnim].layer = m_gameObject.GetComponent<Animation>()[nextAnim].layer;
		m_gameObject.GetComponent<Animation>().Stop(nextAnim);
		m_gameObject.GetComponent<Animation>()[nextAnim].time = m_gameObject.GetComponent<Animation>()[nextAnim].time;
		m_gameObject.GetComponent<Animation>().CrossFade(nextAnim);
	}

	public void AnimationStop(string name)
	{
		m_gameObject.GetComponent<Animation>().Stop(name);
	}

	public void AnimationStop()
	{
		m_gameObject.GetComponent<Animation>().Stop();
	}

	public void AnimationCrossFade(string name, bool loop, float fadeLength)
	{
		m_gameObject.GetComponent<Animation>()[name].wrapMode = ((!loop) ? WrapMode.Once : WrapMode.Loop);
		m_gameObject.GetComponent<Animation>().CrossFade(name, fadeLength);
	}

	public void AnimationSpeedUp(string name, float factorSpeedUp)
	{
		m_gameObject.GetComponent<Animation>()[name].speed = factorSpeedUp;
	}

	public float AnimationLength(string name)
	{
		return m_gameObject.GetComponent<Animation>()[name].length;
	}

	public float GetPitchAngle()
	{
		return m_pitchAngle;
	}

	public void SetMove(bool move, bool UIControlMove, Vector2 moveDirection, float moveFactor)
	{
		m_move = move;
		m_UIControlMove = UIControlMove;
		m_moveDirection = moveDirection;
		if (m_weapon == null)
		{
			Debug.LogError("MoveWithOutWeapon!");
		}
		m_moveFactor = moveFactor;
		ResetMoveFactor();
	}

	public void ResetMoveFactor()
	{
		AnimationSpeedUp(m_weapon.GetAnimation() + "_Down_Move_Forward", m_moveFactor);
		AnimationSpeedUp(m_weapon.GetAnimation() + "_Down_Move_Backward", m_moveFactor);
		AnimationSpeedUp(m_weapon.GetAnimation() + "_Down_Move_Right", m_moveFactor);
		AnimationSpeedUp(m_weapon.GetAnimation() + "_Down_Move_Left", m_moveFactor);
	}

	public virtual void SetFire(bool fire, bool fireContinuous)
	{
		m_fire = fire;
		m_fireContinuous = fireContinuous;
	}

	public virtual void OpenReload()
	{
		m_reload = true;
	}

	public virtual void CloseReload()
	{
		m_reload = false;
	}

	public void TurnAround(float yawPixel, float pitchPixel)
	{
		m_transform.Rotate(Vector3.up, yawPixel * 0.1f);
		m_pitchAngle = Mathf.Clamp(m_pitchAngle + pitchPixel * -0.1f, -55f, 55f);
	}

	public void PitchUpDown(Weapon weapon)
	{
		if (weapon.GetWeaponType() == WeaponType.Knife)
		{
			return;
		}
		if (m_pitchAngle >= 0f)
		{
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].enabled = true;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].weight = 0f;
			if (m_characterType == CharacterType.Sniper)
			{
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].enabled = true;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].weight = 0f;
			}
			if (m_pitchAngle > 45f)
			{
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight = Mathf.Lerp(m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight, 1f, Time.deltaTime * 16f);
				if (m_characterType == CharacterType.Sniper)
				{
					m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].weight = Mathf.Lerp(m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].weight, 1f, Time.deltaTime * 16f);
				}
			}
			else
			{
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight = Mathf.Lerp(m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight, m_pitchAngle / 45f, Time.deltaTime * 16f);
				if (m_characterType == CharacterType.Sniper)
				{
					m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].weight = Mathf.Lerp(m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].weight, m_pitchAngle / 45f, Time.deltaTime * 16f);
				}
			}
		}
		else
		{
			if (!(m_pitchAngle < 0f))
			{
				return;
			}
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].enabled = true;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight = 0f;
			if (m_characterType == CharacterType.Sniper)
			{
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].enabled = true;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].weight = 0f;
			}
			if (m_pitchAngle < -45f)
			{
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].weight = Mathf.Lerp(m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].weight, 1f, Time.deltaTime * 16f);
				if (m_characterType == CharacterType.Sniper)
				{
					m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].weight = Mathf.Lerp(m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].weight, 1f, Time.deltaTime * 16f);
				}
			}
			else
			{
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].weight = Mathf.Lerp(m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].weight, m_pitchAngle / -45f, Time.deltaTime * 16f);
				if (m_characterType == CharacterType.Sniper)
				{
					m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].weight = Mathf.Lerp(m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].weight, m_pitchAngle / -45f, Time.deltaTime * 16f);
				}
			}
		}
	}

	public void SetRotationY(float y)
	{
		m_transform.eulerAngles = new Vector3(0f, y, 0f);
	}

	public void SetRotationForward(Vector3 forward)
	{
		forward.y = 0f;
		m_transform.forward = forward;
	}

	public void SetPitchAngle(float pitchAngle)
	{
		m_pitchAngle = pitchAngle;
	}

	public void ResetPitckUpDown(Weapon weapon)
	{
		if (weapon.GetWeaponType() != 0)
		{
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].enabled = false;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].enabled = false;
			if (m_characterType == CharacterType.Sniper)
			{
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].enabled = false;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].enabled = false;
			}
		}
	}

	public void ResetPitckUpDownWeight(Weapon weapon)
	{
		if (weapon.GetWeaponType() != 0)
		{
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight = 0f;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].weight = 0f;
			if (m_characterType == CharacterType.Sniper)
			{
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].weight = 0f;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].weight = 0f;
			}
		}
	}

	public float GetPitchUpDownWeight(Weapon weapon)
	{
		float num = 0f;
		if (m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight != 0f)
		{
			return m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight;
		}
		return m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].weight;
	}

	public void AddWeapon(int index, Weapon weapon)
	{
		if (m_weaponMap.ContainsKey(index))
		{
			return;
		}
		weapon.SetActive(false);
		weapon.Mount(m_transform, m_transform.Find("Bip01/Bip01 Spine/Bip01 Prop1"));
		Transform transform = m_transform.Find("Bip01/Bip01 Spine");
		m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Glance"].layer = 1;
		m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Glance"].AddMixingTransform(transform);
		m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Change"].layer = 3;
		m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Change"].AddMixingTransform(transform);
		m_gameObject.GetComponent<Animation>()["Up_Throw"].layer = 3;
		m_gameObject.GetComponent<Animation>()["Up_Throw"].AddMixingTransform(transform);
		if (weapon.GetWeaponType() != 0)
		{
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].layer = 2;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].AddMixingTransform(transform);
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].weight = 0f;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].enabled = false;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireDown"].wrapMode = WrapMode.Loop;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].layer = 2;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].AddMixingTransform(transform);
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].weight = 0f;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].enabled = false;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_FireUp"].wrapMode = WrapMode.Loop;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Fire"].layer = 1;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Fire"].AddMixingTransform(transform);
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Reload"].layer = 3;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Reload"].AddMixingTransform(transform);
			if (m_characterType == CharacterType.Sniper)
			{
				Transform mix = transform.Find("Bip01 Spine1/Sniper_Up");
				if (weapon.GetWeaponType() == WeaponType.SniperRifle)
				{
					m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Aftershoot"].layer = 1;
					m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Aftershoot"].AddMixingTransform(transform);
					m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Aftershoot"].layer = 4;
					m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Aftershoot"].AddMixingTransform(mix);
					m_gameObject.GetComponent<Animation>()["Cloak_Throw"].layer = 4;
					m_gameObject.GetComponent<Animation>()["Cloak_Throw"].AddMixingTransform(mix);
				}
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Fire"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Fire"].AddMixingTransform(mix);
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].weight = 0f;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].enabled = false;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].wrapMode = WrapMode.Loop;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireUp"].AddMixingTransform(mix);
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].weight = 0f;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].enabled = false;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].wrapMode = WrapMode.Loop;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_FireDown"].AddMixingTransform(mix);
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Change"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Change"].AddMixingTransform(mix);
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Reload"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Reload"].AddMixingTransform(mix);
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Glance"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Glance"].AddMixingTransform(mix);
			}
		}
		else
		{
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Fire_01"].layer = 1;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Fire_01"].AddMixingTransform(transform);
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Fire_02"].layer = 1;
			m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Up_Fire_02"].AddMixingTransform(transform);
			if (m_characterType == CharacterType.Sniper)
			{
				Transform mix2 = transform.Find("Bip01 Spine1/Sniper_Up");
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Fire_01"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Fire_01"].AddMixingTransform(mix2);
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Fire_02"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Fire_02"].AddMixingTransform(mix2);
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Change"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Change"].AddMixingTransform(mix2);
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Glance"].layer = 4;
				m_gameObject.GetComponent<Animation>()[weapon.GetAnimation() + "_Cloak_Glance"].AddMixingTransform(mix2);
			}
		}
		m_weaponMap.Add(index, weapon);
	}

	public void SetWeaponNotUseAmmo()
	{
		for (int i = 1; i <= m_weaponMap.Count; i++)
		{
			if (m_armorNotUseBullet && m_weaponMap[i] != null)
			{
				m_weaponMap[i].CloseUseAmmo();
			}
		}
	}

	public void RemoveWeapon(int index)
	{
		if (m_weaponMap.ContainsKey(index))
		{
			Weapon weapon = m_weaponMap[index];
			if (m_weapon == weapon)
			{
				m_weapon = null;
			}
			weapon.SetActive(false);
			weapon.Unmount();
			m_weaponMap.Remove(index);
		}
	}

	public void AddWeaponAmmo(int clip)
	{
		foreach (KeyValuePair<int, Weapon> item in m_weaponMap)
		{
			if (item.Value.GetWeaponType() != 0)
			{
				item.Value.AddAmmoClip(clip);
			}
		}
	}

	public void SynchronizeWeaponAmmo(int totalAmmo, WeaponType weaponType)
	{
		foreach (KeyValuePair<int, Weapon> item in m_weaponMap)
		{
			if (item.Value.GetWeaponType() != 0 && item.Value.GetWeaponType() == weaponType)
			{
				item.Value.SynchronizeAmmoTotal(totalAmmo);
			}
		}
	}

	public void UseWeapon(int index)
	{
		if (m_weapon != null)
		{
			m_weapon.SetActive(false);
			m_weapon = null;
		}
		if (m_weaponMap.ContainsKey(index))
		{
			m_weapon = m_weaponMap[index];
			m_weapon.SetActive(true);
		}
	}

	public virtual void ChangeWeapon(int index)
	{
		if (m_weaponMap.ContainsKey(index))
		{
			m_weaponChangeId = index;
			m_weaponChange = m_weaponMap[index];
			ResetPitckUpDown(m_weapon);
		}
	}

	public void DoChangeWeapon()
	{
		if (m_weaponChange != null)
		{
			if (m_weapon != null)
			{
				m_weapon.SetActive(false);
				m_weapon = null;
			}
			m_weapon = m_weaponChange;
			m_weapon.SetActive(true);
			ResetMoveFactor();
			m_weaponChange = null;
		}
	}

	public Weapon GetWeapon()
	{
		return m_weapon;
	}

	public void SetWeaponAmmoCapacity(WeaponType weaponType, float factor)
	{
		for (int i = 1; i <= 3; i++)
		{
			if (m_weaponMap[i].GetWeaponType() == weaponType)
			{
				m_weaponMap[i].SetAmmoCapacity(factor);
			}
		}
	}

	public Weapon GetChangeWeapon()
	{
		return m_weaponChange;
	}

	public virtual void OnHit(ref HitInfo hitInfo)
	{
		if (m_HPCurrent <= 0f)
		{
			return;
		}
		if (m_characterEffectBloodEmitPool != null)
		{
			m_characterEffectBloodEmitPool.Emit(hitInfo);
		}
		if (hitInfo.headShot)
		{
			m_HPCurrent -= hitInfo.damage * m_factorHitDamage * 2f;
			if (m_HPCurrent > 0f)
			{
				m_HPRecoverTime = 5f * m_factorRecoverTime;
			}
		}
		else
		{
			m_HPCurrent -= hitInfo.damage * m_factorHitDamage;
			if (m_HPCurrent > 0f)
			{
				m_HPRecoverTime = 5f * m_factorRecoverTime;
			}
		}
	}

	public virtual void OnHitWeb(NetHitInfo hi)
	{
		if (m_dead)
		{
			return;
		}
		EnemyNet enemyNetByNetId = GameManager.Instance().GetLevel().GetEnemyNetByNetId(hi.byNetId);
		if (enemyNetByNetId != null)
		{
			HitInfo hitInfo = default(HitInfo);
			hitInfo.headShot = hi.headShot;
			hitInfo.weaponType = hi.weaponType;
			hitInfo.damage = hi.damage;
			hitInfo.hitPoint = GetTransform().position;
			Vector3 vector = GetTransform().InverseTransformDirection(enemyNetByNetId.GetTransform().position - GetTransform().position);
			vector.y = 0f;
			hitInfo.hitAngle = Vector3.Angle(GetTransform().forward, vector);
			hitInfo.hitLocDic = vector;
			CharacterController component = GetTransform().GetComponent<CharacterController>();
			if (component != null)
			{
				Vector2 vector2 = new Vector2(component.radius * Mathf.Cos((float)Math.PI / 180f * hitInfo.hitAngle), component.radius * Mathf.Sin((float)Math.PI / 180f * hitInfo.hitAngle));
				hitInfo.localHitPoint = new Vector3(vector2.x, UnityEngine.Random.Range(0.4f, 1.5f), vector2.y);
			}
			if (m_characterEffectBloodEmitPool != null)
			{
				m_characterEffectBloodEmitPool.Emit(hitInfo);
			}
		}
	}

	public virtual void OnDeath()
	{
	}

	public virtual void OnDestroy()
	{
	}

	public virtual void OnResurrection()
	{
		RemoveAllBuff();
		SwitchFSM(m_stateMove);
		m_fire = false;
		m_move = false;
		m_dead = false;
		m_headShot = false;
	}

	public void FSMSwitchDeathState()
	{
		if (m_headShot)
		{
			SwitchFSM(m_stateHeadShot);
		}
		else
		{
			SwitchFSM(m_stateDeath);
		}
	}

	public virtual void OnEffect(ref Effect effect)
	{
	}

	public void PostEvent(CharacterEvent ce)
	{
		foreach (CharacterEvent item in m_event)
		{
			if (item.type == ce.type)
			{
				return;
			}
		}
		m_event.Add(ce);
	}

	private void DeletOutTimeEvent()
	{
		if (m_event.Count == 0)
		{
			return;
		}
		for (int i = 0; i < m_event.Count; i++)
		{
			if (Time.time - m_event[i].time > m_event[i].timeOut || m_event[i].response)
			{
				m_event.Remove(m_event[i]);
			}
		}
	}

	public virtual void UseItems(ItemInfo itemInfo)
	{
		if (itemInfo.type == ItemEffectType.AddHp)
		{
			m_characterEffectItemEmit.StartEmit(itemInfo.type);
			GetTransform().GetComponent<TAudioController>().PlayAudio("Fx_eat_Heal");
			if (itemInfo.param1 != -1f)
			{
				m_HPCurrent = Mathf.Clamp(m_HPCurrent + m_HPMax * m_factorHpMax * itemInfo.param1, 0f, m_HPMax * m_factorHpMax);
			}
			else
			{
				Debug.LogWarning(itemInfo.name + ", Param1 == -1");
			}
		}
		else if (itemInfo.type == ItemEffectType.Resurrection)
		{
			m_characterEffectItemEmit.StartEmit(itemInfo.type);
			OnResurrection();
			GetTransform().GetComponent<TAudioController>().PlayAudio("Fx_eat_Resurrection");
		}
		else if (itemInfo.type == ItemEffectType.MoveSpeedUp)
		{
			if (itemInfo.param1 != -1f)
			{
				CharacterBuff characterBuff = new CharacterBuff();
				characterBuff.buffName = itemInfo.type.ToString();
				characterBuff.time = Time.time;
				characterBuff.addValue = itemInfo.param1;
				if (itemInfo.param2 != -1f)
				{
					characterBuff.timeOut = itemInfo.param2;
				}
				else
				{
					Debug.LogWarning(itemInfo.name + ", Param2 == -1");
				}
				AddBuff(characterBuff);
			}
			else
			{
				Debug.LogWarning(itemInfo.name + ", Param1 == -1");
			}
			m_characterEffectItemEmit.StartEmit(itemInfo.type);
			GetTransform().GetComponent<TAudioController>().PlayAudio("Fx_eat_PowerUp");
		}
		else if (itemInfo.type == ItemEffectType.PowerUp)
		{
			if (itemInfo.param1 != -1f)
			{
				CharacterBuff characterBuff2 = new CharacterBuff();
				characterBuff2.buffName = itemInfo.type.ToString();
				characterBuff2.time = Time.time;
				characterBuff2.addValue = itemInfo.param1;
				if (itemInfo.param2 != -1f)
				{
					characterBuff2.timeOut = itemInfo.param2;
				}
				else
				{
					Debug.LogWarning(itemInfo.name + ", Param2 == -1");
				}
				AddBuff(characterBuff2);
			}
			else
			{
				Debug.LogWarning(itemInfo.name + ", Param1 == -1");
			}
			m_characterEffectItemEmit.StartEmit(itemInfo.type);
			GetTransform().GetComponent<TAudioController>().PlayAudio("Fx_eat_PowerUp");
		}
		else if (itemInfo.type == ItemEffectType.DefUp)
		{
			if (itemInfo.param1 != -1f)
			{
				CharacterBuff characterBuff3 = new CharacterBuff();
				characterBuff3.buffName = itemInfo.type.ToString();
				characterBuff3.time = Time.time;
				characterBuff3.addValue = itemInfo.param1;
				if (itemInfo.param2 != -1f)
				{
					characterBuff3.timeOut = itemInfo.param2;
				}
				else
				{
					Debug.LogWarning(itemInfo.name + ", Param2 == -1");
				}
				AddBuff(characterBuff3);
			}
			else
			{
				Debug.LogWarning(itemInfo.name + ", Param1 == -1");
			}
			m_characterEffectItemEmit.StartEmit(itemInfo.type);
			GetTransform().GetComponent<TAudioController>().PlayAudio("Fx_eat_PowerUp");
		}
		else if (itemInfo.type == ItemEffectType.GodLike)
		{
			if (itemInfo.param1 != -1f)
			{
				CharacterBuff characterBuff4 = new CharacterBuff();
				characterBuff4.buffName = itemInfo.type.ToString();
				characterBuff4.time = Time.time;
				characterBuff4.addValue = itemInfo.param1;
				if (itemInfo.param2 != -1f)
				{
					characterBuff4.timeOut = itemInfo.param2;
				}
				else
				{
					Debug.LogWarning(itemInfo.name + ", Param2 == -1");
				}
				AddBuff(characterBuff4);
			}
			else
			{
				Debug.LogWarning(itemInfo.name + ", Param1 == -1");
			}
			m_characterEffectItemEmit.StartEmit(itemInfo.type);
			GetTransform().GetComponent<TAudioController>().PlayAudio("Fx_eat_PowerUp");
		}
		else if (itemInfo.type == ItemEffectType.DefUpFull)
		{
			if (itemInfo.param1 != -1f)
			{
				CharacterBuff characterBuff5 = new CharacterBuff();
				characterBuff5.buffName = itemInfo.type.ToString();
				characterBuff5.time = Time.time;
				characterBuff5.addValue = itemInfo.param1;
				if (itemInfo.param2 != -1f)
				{
					characterBuff5.timeOut = itemInfo.param2;
				}
				else
				{
					Debug.LogWarning(itemInfo.name + ", Param2 == -1");
				}
				AddBuff(characterBuff5);
			}
			else
			{
				Debug.LogWarning(itemInfo.name + ", Param1 == -1");
			}
			m_characterEffectItemEmit.StartEmit(itemInfo.type);
			GetTransform().GetComponent<TAudioController>().PlayAudio("Fx_eat_GodLike");
		}
		else if (itemInfo.type == ItemEffectType.Grenade)
		{
			OpenThrow();
		}
	}

	protected void AddBuff(CharacterBuff bf)
	{
		for (int i = 0; i < m_buff.Count; i++)
		{
			if (bf.buffName == m_buff[i].buffName)
			{
				RemoveBuff(m_buff[i]);
			}
		}
		if (bf.buffName == "MoveSpeedUp")
		{
			m_factorMoveSpeed += bf.addValue;
		}
		else if (bf.buffName == "PowerUp")
		{
			m_factorWeaponAtk += bf.addValue;
		}
		else if (bf.buffName == "DefUp")
		{
			m_factorHitDamage *= 1f - bf.addValue;
		}
		else if (bf.buffName == "GodLike")
		{
			m_factorWeaponAtk += bf.addValue;
			for (int j = 1; j <= 3; j++)
			{
				m_weaponMap[j].CloseUseAmmo();
			}
		}
		else if (bf.buffName == "DefUpFull")
		{
			m_factorHitDamage *= 1f - bf.addValue;
		}
		else if (bf.buffName == "ResurrectionDefUpFull")
		{
			m_factorHitDamage *= 1f - bf.addValue;
		}
		m_buff.Add(bf);
	}

	private void DeletOutTimeBuff()
	{
		if (m_buff.Count == 0)
		{
			return;
		}
		if (m_fire)
		{
			RemoveResurrectionBuff();
		}
		for (int i = 0; i < m_buff.Count; i++)
		{
			if (Time.time - m_buff[i].time > m_buff[i].timeOut)
			{
				RemoveBuff(m_buff[i]);
			}
		}
	}

	private void RemoveResurrectionBuff()
	{
		CharacterBuff characterBuff = null;
		foreach (CharacterBuff item in m_buff)
		{
			if (item.buffName == "ResurrectionDefUpFull")
			{
				characterBuff = item;
			}
		}
		if (characterBuff != null)
		{
			m_factorHitDamage /= 1f - characterBuff.addValue;
			m_characterEffectItemEmit.StopEmit(ItemEffectType.DefUpFull);
			m_buff.Remove(characterBuff);
			ReCalcFactor();
		}
	}

	private void RemoveAllBuff()
	{
		foreach (CharacterBuff item in m_buff)
		{
			if (item.buffName == "MoveSpeedUp")
			{
				m_characterEffectItemEmit.StopEmit(ItemEffectType.MoveSpeedUp);
			}
			else if (item.buffName == "PowerUp")
			{
				m_characterEffectItemEmit.StopEmit(ItemEffectType.PowerUp);
			}
			else if (item.buffName == "DefUp")
			{
				m_characterEffectItemEmit.StopEmit(ItemEffectType.DefUp);
			}
			else if (item.buffName == "GodLike")
			{
				m_characterEffectItemEmit.StopEmit(ItemEffectType.GodLike);
			}
			else if (item.buffName == "DefUpFull")
			{
				m_characterEffectItemEmit.StopEmit(ItemEffectType.DefUpFull);
			}
			else if (item.buffName == "ResurrectionDefUpFull")
			{
				m_characterEffectItemEmit.StopEmit(ItemEffectType.DefUpFull);
			}
			else
			{
				Debug.LogWarning("WithOut This Buff! " + item.buffName);
			}
		}
		m_buff.Clear();
		ReCalcFactor();
	}

	private void RemoveBuff(CharacterBuff buff)
	{
		if (buff.buffName == "MoveSpeedUp")
		{
			m_factorMoveSpeed -= buff.addValue;
			m_characterEffectItemEmit.StopEmit(ItemEffectType.MoveSpeedUp);
		}
		else if (buff.buffName == "PowerUp")
		{
			m_factorWeaponAtk -= buff.addValue;
			m_characterEffectItemEmit.StopEmit(ItemEffectType.PowerUp);
		}
		else if (buff.buffName == "DefUp")
		{
			m_factorHitDamage /= 1f - buff.addValue;
			m_characterEffectItemEmit.StopEmit(ItemEffectType.DefUp);
		}
		else if (buff.buffName == "GodLike")
		{
			m_factorWeaponAtk -= buff.addValue;
			m_characterEffectItemEmit.StopEmit(ItemEffectType.GodLike);
			for (int i = 1; i <= 3; i++)
			{
				if (!m_armorNotUseBullet)
				{
					m_weaponMap[i].OpenUseAmmo();
				}
			}
		}
		else if (buff.buffName == "DefUpFull")
		{
			m_factorHitDamage /= 1f - buff.addValue;
			m_characterEffectItemEmit.StopEmit(ItemEffectType.DefUpFull);
		}
		else if (buff.buffName == "ResurrectionDefUpFull")
		{
			m_factorHitDamage /= 1f - buff.addValue;
			m_characterEffectItemEmit.StopEmit(ItemEffectType.DefUpFull);
		}
		else
		{
			Debug.LogWarning("WithOut This Buff! " + buff.buffName);
		}
		m_buff.Remove(buff);
		ReCalcFactor();
	}

	private void ReCalcFactor()
	{
		m_factorHpMax = 1f;
		m_factorHitDamage = 1f;
		m_factorHpRecover = 1f;
		m_factorMoveSpeed = 1f;
		m_factorWeaponAcc = 1f;
		m_factorWeaponAtk = 1f;
		m_factorWeaponReloadSpeed = 1f;
		m_factorHandGunAtk = 1f;
		m_factorAssaultAcc = 1f;
		m_factorSubmachineCapacity = 1f;
		m_factorShotGunAtk = 1f;
		m_factorRpgMissileAtkRange = 1f;
		m_factorSniperFireSpeed = 1f;
		m_factorColdWeaponFireSpeed = 1f;
		m_factorCharacter01 = 1f;
		m_factorCharacter02 = 1f;
		m_factorCharacter03 = 1f;
		m_factorCharacter04 = 1f;
		m_factorAmmoTotal = 1f;
		m_factorExp = 1f;
		m_factorRecoverItemEffect = 1f;
		m_factorRecoverTime = 1f;
		m_factorHeadShotLucky = 1f;
		m_armorNotUseBullet = false;
		CharacterEquipInfo characterEquipInfo = DataCenter.Save().GetCharacterEquipInfo((int)m_characterType);
		GetTransform().GetComponent<CharacterPassivePara>().ArmorEffect(characterEquipInfo);
		for (int i = 0; i < m_buff.Count; i++)
		{
			if (m_buff[i].buffName == "MoveSpeedUp")
			{
				m_factorMoveSpeed += m_buff[i].addValue;
			}
			else if (m_buff[i].buffName == "PowerUp")
			{
				m_factorWeaponAtk += m_buff[i].addValue;
			}
			else if (m_buff[i].buffName == "DefUp")
			{
				m_factorHitDamage *= 1f - m_buff[i].addValue;
			}
			else if (m_buff[i].buffName == "GodLike")
			{
				m_factorWeaponAtk += m_buff[i].addValue;
			}
			else if (m_buff[i].buffName == "DefUpFull")
			{
				m_factorHitDamage *= 1f - m_buff[i].addValue;
			}
			else
			{
				Debug.LogWarning("WithOut This Buff! " + m_buff[i].buffName);
			}
		}
	}
}

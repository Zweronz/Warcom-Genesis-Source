using System;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
	public enum CameraMode
	{
		StartMode = 0,
		NormalMode = 1,
		DeadMode = 2,
		EndMode = 3
	}

	private const float const_reverseTime = 3f;

	private float m_cameraRadius = 1.8f;

	private Vector3 m_cameraCenter = new Vector3(0.5f, 1.6f, 0.6f);

	private float m_rotateAngle;

	private float m_reverseTime = 3f;

	private Vector3 velocity = Vector3.zero;

	public CameraMode m_cameraMode;

	public bool isWeb;

	private float m_defaultFieldOfView = 60f;

	private float m_zoomFieldOfView = 60f;

	private float m_zoomDistance;

	private bool m_zoom;

	private float m_zoomFieldOfViewBegin;

	private float m_zoomFieldOfViewEnd;

	private float m_zoomDistanceBegin;

	private float m_zoomDistanceEnd;

	private float m_zoomTimeTotal;

	private float m_zoomTime;

	private Crosshair m_crossHairCurrent;

	private CrosshairA m_crossHairA;

	private CrosshairB m_crossHairB;

	private CrosshairC m_crossHairC;

	private CrosshairD m_crossHairD;

	private CrosshairE m_crossHairE;

	private bool m_shake;

	private Vector2 m_shakeOffset;

	private void Awake()
	{
		switch (isWeb ? DataCenter.StateMulti().GetCurrentCharactor() : DataCenter.StateSingle().GetCurrentCharactor())
		{
		case 1:
			m_cameraCenter = new Vector3(0.5f, 1.7f, 0.6f);
			m_cameraRadius = 1.9f;
			break;
		case 2:
			m_cameraCenter = new Vector3(0.5f, 1.7f, 0.6f);
			m_cameraRadius = 1.9f;
			break;
		case 3:
			m_cameraCenter = new Vector3(0.5f, 1.7f, 0.6f);
			m_cameraRadius = 1.9f;
			break;
		case 4:
			m_cameraCenter = new Vector3(0.65f, 1.6f, 0.7f);
			m_cameraRadius = 2.5f;
			break;
		}
		m_crossHairA = new CrosshairA();
		m_crossHairB = new CrosshairB();
		m_crossHairC = new CrosshairC();
		m_crossHairD = new CrosshairD();
		m_crossHairE = new CrosshairE();
	}

	public void Update()
	{
		if (m_zoom)
		{
			m_zoomTime += Time.deltaTime;
			if (m_zoomTime < m_zoomTimeTotal)
			{
				float t = m_zoomTime / m_zoomTimeTotal;
				m_zoomFieldOfView = Mathf.Lerp(m_zoomFieldOfViewBegin, m_zoomFieldOfViewEnd, t);
				m_zoomDistance = Mathf.Lerp(m_zoomDistanceBegin, m_zoomDistanceEnd, t);
			}
			else
			{
				m_zoomFieldOfView = m_zoomFieldOfViewEnd;
				m_zoomDistance = m_zoomDistanceEnd;
				m_zoom = false;
			}
		}
		if (m_crossHairCurrent.GetCurrentSize() != m_crossHairCurrent.m_sizeMin * m_crossHairCurrent.m_acc)
		{
			m_crossHairCurrent.CoolDown();
		}
	}

	public void LateUpdate()
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player == null)
		{
			return;
		}
		float pitchAngle = player.GetPitchAngle();
		float f = pitchAngle * ((float)Math.PI / 180f);
		if (m_cameraMode == CameraMode.StartMode)
		{
			m_rotateAngle += Time.deltaTime * 360f / 3f;
			m_reverseTime -= Time.deltaTime;
			if (m_reverseTime < 0f)
			{
				Vector3 position = m_cameraCenter + new Vector3(0f, m_cameraRadius * Mathf.Sin(f), (0f - m_cameraRadius) * Mathf.Cos(f));
				Vector3 vector = player.GetTransform().TransformPoint(position);
				float t = (0f - m_reverseTime) / 1.5f;
				base.transform.position = Vector3.Lerp(base.transform.position, vector, t);
				if (m_reverseTime < -1.5f)
				{
					m_cameraMode = CameraMode.NormalMode;
					m_rotateAngle = 0f;
					m_reverseTime = 0f;
					base.transform.position = vector;
					base.transform.rotation = player.GetTransform().rotation;
				}
			}
			else if (m_reverseTime >= 0f)
			{
				Vector3 vector2 = new Vector3(m_cameraRadius * Mathf.Sin((m_rotateAngle - player.GetTransform().eulerAngles.y) * ((float)Math.PI / 180f)), m_cameraRadius * Mathf.Sin(f) + m_cameraCenter.y, (0f - m_cameraRadius) * Mathf.Cos((m_rotateAngle - player.GetTransform().eulerAngles.y) * ((float)Math.PI / 180f)));
				base.transform.position = player.GetTransform().position + vector2;
				base.transform.forward = player.GetTransform().position + new Vector3(0f, m_cameraRadius * Mathf.Sin(f) + m_cameraCenter.y, 0f) - base.transform.position;
			}
		}
		else if (m_cameraMode == CameraMode.NormalMode)
		{
			Vector3 position2 = m_cameraCenter + new Vector3(0f, m_cameraRadius * Mathf.Sin(f), (0f - m_cameraRadius) * Mathf.Cos(f)) + new Vector3(m_shakeOffset.x, m_shakeOffset.y, 0f);
			base.transform.position = player.GetTransform().TransformPoint(position2);
			base.transform.rotation = Quaternion.Euler(player.GetPitchAngle(), player.GetTransform().eulerAngles.y, 0f);
			base.GetComponent<Camera>().fieldOfView = m_zoomFieldOfView;
			if (m_zoomDistance > 0f)
			{
				base.transform.position += base.transform.forward * m_zoomDistance;
			}
			if (player.m_dead)
			{
				m_cameraMode = CameraMode.DeadMode;
			}
			if (!isWeb && DataCenter.StateSingle().GetLevelInfo().pass)
			{
				m_cameraMode = CameraMode.EndMode;
			}
		}
		else if (m_cameraMode == CameraMode.DeadMode)
		{
			if (!player.m_dead)
			{
				m_cameraMode = CameraMode.NormalMode;
			}
			m_reverseTime -= Time.deltaTime;
			if (m_reverseTime > -0.5f)
			{
				float t2 = (0f - m_reverseTime) / 0.5f;
				base.transform.forward = Vector3.Lerp(base.transform.forward, player.GetTransform().position - base.transform.position, t2);
			}
			else if (m_reverseTime <= -0.5f)
			{
				m_rotateAngle += Time.deltaTime * 360f / 3f;
				Vector3 vector3 = new Vector3(m_cameraRadius * Mathf.Sin(m_rotateAngle * ((float)Math.PI / 180f)), m_cameraRadius * Mathf.Sin(f) + m_cameraCenter.y, (0f - m_cameraRadius) * Mathf.Cos(m_rotateAngle * ((float)Math.PI / 180f)));
				base.transform.position = player.GetTransform().position + vector3;
				base.transform.forward = player.GetTransform().position - base.transform.position;
			}
			base.GetComponent<Camera>().fieldOfView = m_zoomFieldOfView;
		}
		else if (m_cameraMode == CameraMode.EndMode)
		{
			m_rotateAngle += Time.deltaTime * 250f / 3f;
			m_reverseTime -= Time.deltaTime;
			Vector3 vector4 = new Vector3(m_cameraRadius * Mathf.Sin(m_rotateAngle * ((float)Math.PI / 180f)), m_cameraRadius * Mathf.Sin(f) + m_cameraCenter.y, (0f - m_cameraRadius) * Mathf.Cos(m_rotateAngle * ((float)Math.PI / 180f)));
			base.transform.position = player.GetTransform().position + vector4;
			base.transform.forward = player.GetTransform().position + new Vector3(0f, m_cameraRadius * Mathf.Sin(f) + m_cameraCenter.y, 0f) - base.transform.position;
			base.GetComponent<Camera>().fieldOfView = m_zoomFieldOfView;
		}
		CameraCollision();
		if (m_shake)
		{
			float x = m_shakeOffset.x;
			if (m_shakeOffset.x > 0f)
			{
				m_shakeOffset.x -= Time.deltaTime * 1f;
			}
			else
			{
				m_shakeOffset.x += Time.deltaTime * 1f;
			}
			float y = m_shakeOffset.y;
			if (m_shakeOffset.y > 0f)
			{
				m_shakeOffset.y -= Time.deltaTime * 1f;
			}
			else
			{
				m_shakeOffset.y += Time.deltaTime * 1f;
			}
			if (x * m_shakeOffset.x < 0f || y * m_shakeOffset.y < 0f)
			{
				m_shakeOffset.x = 0f;
				m_shakeOffset.y = 0f;
				m_shake = false;
			}
		}
	}

	public void SetCurrentCrosshairByWeaponInfo(WeaponInfo weaponInfo)
	{
		m_crossHairA.Close();
		m_crossHairB.Close();
		m_crossHairC.Close();
		m_crossHairD.Close();
		m_crossHairE.Close();
		if (weaponInfo.crosshairType == "A")
		{
			m_crossHairA.SetParams(DataCenter.Conf().GetCrosshairParam(weaponInfo.crosshair), weaponInfo.type);
			m_crossHairCurrent = m_crossHairA;
		}
		else if (weaponInfo.crosshairType == "B")
		{
			m_crossHairB.SetParams(DataCenter.Conf().GetCrosshairParam(weaponInfo.crosshair), weaponInfo.type);
			m_crossHairCurrent = m_crossHairB;
		}
		else if (weaponInfo.crosshairType == "C")
		{
			m_crossHairC.SetParams(DataCenter.Conf().GetCrosshairParam(weaponInfo.crosshair), weaponInfo.type);
			m_crossHairCurrent = m_crossHairC;
		}
		else if (weaponInfo.crosshairType == "D")
		{
			m_crossHairD.SetParams(DataCenter.Conf().GetCrosshairParam(weaponInfo.crosshair), weaponInfo.type);
			m_crossHairCurrent = m_crossHairD;
		}
		else if (weaponInfo.crosshairType == "E")
		{
			m_crossHairE.SetParams(DataCenter.Conf().GetCrosshairParam(weaponInfo.crosshair), weaponInfo.type);
			m_crossHairCurrent = m_crossHairE;
		}
		m_crossHairCurrent.Open();
	}

	public void ZoomIn(float fieldOfView, float distance, float zoomTimeTotal)
	{
		m_zoom = true;
		m_zoomFieldOfViewBegin = m_zoomFieldOfView;
		m_zoomFieldOfViewEnd = fieldOfView;
		m_zoomDistanceBegin = m_zoomDistance;
		m_zoomDistanceEnd = distance;
		m_zoomTimeTotal = zoomTimeTotal;
		m_zoomTime = 0f;
	}

	public void ZoomOut(float zoomTimeTotal)
	{
		m_zoom = true;
		m_zoomFieldOfViewBegin = m_zoomFieldOfView;
		m_zoomFieldOfViewEnd = m_defaultFieldOfView;
		m_zoomDistanceBegin = m_zoomDistance;
		m_zoomDistanceEnd = 0f;
		m_zoomTimeTotal = zoomTimeTotal;
		m_zoomTime = 0f;
	}

	public void Shake(Vector2 offset)
	{
		m_shake = true;
		m_shakeOffset += offset;
	}

	public float GetZoomFieldOfView()
	{
		return m_zoomFieldOfView;
	}

	public void SnipeInCrosshair()
	{
		m_crossHairA.Close();
		m_crossHairCurrent = m_crossHairE;
		m_crossHairCurrent.Open();
	}

	public void SnipeOutCrosshair()
	{
		m_crossHairE.Close();
		m_crossHairCurrent = m_crossHairA;
		m_crossHairCurrent.Open();
	}

	public Crosshair GetCurrentCrosshair()
	{
		return m_crossHairCurrent;
	}

	public void SetCrosshairAcc(float acc)
	{
		m_crossHairA.m_acc = acc;
		m_crossHairB.m_acc = acc;
		m_crossHairC.m_acc = acc;
		m_crossHairD.m_acc = acc;
		m_crossHairE.m_acc = acc;
	}

	public void SetAssaultCrosshairAcc(float acc)
	{
		m_crossHairB.m_accB = acc;
	}

	public Vector3 GetCameraPosition()
	{
		return base.transform.position;
	}

	public Vector3 GetCrosshairPosition()
	{
		return base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + m_crossHairCurrent.GetShotPoint().x, (float)Screen.height / 2f + m_crossHairCurrent.GetShotPoint().y, base.GetComponent<Camera>().farClipPlane));
	}

	public Vector3[] GetCrosshairPositionArray()
	{
		List<Vector3> list = new List<Vector3>();
		Vector2 shotPoint = m_crossHairCurrent.GetShotPoint();
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x, (float)Screen.height / 2f + shotPoint.y, base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(0f, 30f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(0f, 30f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(0f, 75f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(0f, 50f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(0f, 140f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(0f, 100f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(-30f, 0f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(0f, 30f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(-75f, 0f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(0f, 50f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(-140f, 0f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(0f, 100f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(-30f, 0f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(-30f, 0f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(-75f, 0f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(-50f, 0f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(-140f, 0f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(-100f, 0f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(0f, 30f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(-30f, 0f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(0f, 75f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(-50f, 0f), base.GetComponent<Camera>().farClipPlane)));
		list.Add(base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width / 2f + shotPoint.x + UnityEngine.Random.Range(0f, 140f), (float)Screen.height / 2f + shotPoint.y + UnityEngine.Random.Range(-100f, 0f), base.GetComponent<Camera>().farClipPlane)));
		return list.ToArray();
	}

	private void CameraCollision()
	{
		Vector3 forward = base.transform.forward;
		Ray ray = new Ray(base.transform.position, forward);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 5f, 32768))
		{
			Ray ray2 = new Ray(hitInfo.point, -forward);
			RaycastHit hitInfo2;
			if (Physics.Raycast(ray2, out hitInfo2, Vector3.Distance(base.transform.position, hitInfo.point), 20480))
			{
				base.transform.position = hitInfo2.point + forward * 0.2f;
			}
		}
	}
}

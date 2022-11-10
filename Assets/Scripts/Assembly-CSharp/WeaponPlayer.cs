using System;
using UnityEngine;

public abstract class WeaponPlayer : WeaponBase
{
	private float m_cameraZoomInFieldOfView;

	private float m_cameraZoomInDistance;

	private Rect m_cameraShakeRange;

	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
		m_cameraZoomInFieldOfView = DataCenter.Conf().GetCameraParam(weaponInfo.camera).angle;
		m_cameraZoomInDistance = DataCenter.Conf().GetCameraParam(weaponInfo.camera).distance;
		m_cameraShakeRange = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);
	}

	public override void Reload()
	{
		if (m_type != 0)
		{
			if (m_ammoTotal + m_ammoCurrentAmount > m_ammoCapacity)
			{
				m_ammoTotal = m_ammoTotal - m_ammoCapacity + m_ammoCurrentAmount;
				m_ammoCurrentAmount = m_ammoCapacity;
			}
			else
			{
				m_ammoCurrentAmount = m_ammoTotal + m_ammoCurrentAmount;
				m_ammoTotal = 0;
			}
		}
		else
		{
			m_ammoCurrentAmount = m_ammoCapacity;
		}
	}

	protected override void OnStartFire()
	{
	}

	protected override void OnStopFire()
	{
	}

	protected override void OnDoFire()
	{
		GameManager.Instance().GetCamera().GetCurrentCrosshair()
			.FireUp();
	}

	protected void CameraZoomIn()
	{
		GameManager.Instance().GetCamera().ZoomIn(m_cameraZoomInFieldOfView, m_cameraZoomInDistance, 0.1f);
	}

	protected void CameraZoomOut()
	{
		GameManager.Instance().GetCamera().ZoomOut(0.1f);
	}

	protected void CameraShake()
	{
		float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
		Vector2 offset = new Vector2(Mathf.Sin(f), Mathf.Cos(f)) * 0.1f;
		GameManager.Instance().GetCamera().Shake(offset);
	}
}

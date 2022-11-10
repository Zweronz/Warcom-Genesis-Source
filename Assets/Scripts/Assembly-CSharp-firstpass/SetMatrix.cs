using UnityEngine;

public class SetMatrix : MonoBehaviour
{
	public Camera m_camera;

	public Material m_material;

	public Transform m_object;

	private void Start()
	{
	}

	private void LateUpdate()
	{
		if (base.GetComponent<Renderer>().isVisible)
		{
			m_material.SetMatrix("_ShadowMatrix", getMVP(m_object, m_camera));
		}
	}

	private Matrix4x4 getMVP(Transform TT, Camera m_cam)
	{
		bool flag = SystemInfo.graphicsDeviceVersion.IndexOf("Direct3D") > -1;
		Matrix4x4 localToWorldMatrix = TT.localToWorldMatrix;
		Matrix4x4 worldToCameraMatrix = m_cam.worldToCameraMatrix;
		Matrix4x4 projectionMatrix = m_cam.projectionMatrix;
		if (flag)
		{
			for (int i = 0; i < 4; i++)
			{
				projectionMatrix[1, i] = 0f - projectionMatrix[1, i];
			}
			for (int j = 0; j < 4; j++)
			{
				projectionMatrix[2, j] = projectionMatrix[2, j] * 0.5f + projectionMatrix[3, j] * 0.5f;
			}
		}
		return projectionMatrix * worldToCameraMatrix * localToWorldMatrix;
	}
}

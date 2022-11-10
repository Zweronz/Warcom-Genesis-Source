using UnityEngine;

public class UtilUIIpadMask : MonoBehaviour
{
	public Texture2D m_texture2dUp;

	public Texture2D m_texture2dDown;

	public Texture2D m_texture2dRight;

	public Texture2D m_texture2dLeft;

	public Material m_materialUp;

	public Material m_materialDown;

	public Material m_materialRight;

	public Material m_materialLeft;

	protected Material m_materialCloneUp;

	protected Material m_materialCloneDown;

	protected Material m_materialCloneRight;

	protected Material m_materialCloneLeft;

	private void Awake()
	{
		if ((bool)m_materialUp)
		{
			m_materialCloneUp = Object.Instantiate(m_materialUp) as Material;
			m_materialCloneUp.hideFlags = HideFlags.DontSave;
			m_materialCloneUp.mainTexture = m_texture2dUp;
		}
		else
		{
			m_materialCloneUp = null;
		}
		TUITextureManager.Instance().AddFrame("CommonMaskUp", m_texture2dUp, m_materialCloneUp, new Vector2(1024f, 64f), new Vector4(0f, 1f, 1f, 0f));
		if ((bool)m_materialDown)
		{
			m_materialCloneDown = Object.Instantiate(m_materialDown) as Material;
			m_materialCloneDown.hideFlags = HideFlags.DontSave;
			m_materialCloneDown.mainTexture = m_texture2dDown;
		}
		else
		{
			m_materialCloneDown = null;
		}
		TUITextureManager.Instance().AddFrame("CommonMaskDown", m_texture2dDown, m_materialCloneDown, new Vector2(1024f, 64f), new Vector4(0f, 1f, 1f, 0f));
		if ((bool)m_materialRight)
		{
			m_materialCloneRight = Object.Instantiate(m_materialRight) as Material;
			m_materialCloneRight.hideFlags = HideFlags.DontSave;
			m_materialCloneRight.mainTexture = m_texture2dRight;
		}
		else
		{
			m_materialCloneRight = null;
		}
		TUITextureManager.Instance().AddFrame("CommonMaskRight", m_texture2dRight, m_materialCloneRight, new Vector2(64f, 512f), new Vector4(0f, 1f, 1f, 0f));
		if ((bool)m_materialLeft)
		{
			m_materialCloneLeft = Object.Instantiate(m_materialLeft) as Material;
			m_materialCloneLeft.hideFlags = HideFlags.DontSave;
			m_materialCloneLeft.mainTexture = m_texture2dLeft;
		}
		else
		{
			m_materialCloneLeft = null;
		}
		TUITextureManager.Instance().AddFrame("CommonMaskLeft", m_texture2dLeft, m_materialCloneLeft, new Vector2(64f, 512f), new Vector4(0f, 1f, 1f, 0f));
	}

	private void OnDestroy()
	{
		TUITextureManager.Instance().RemoveFrame("CommonMaskUp");
		TUITextureManager.Instance().RemoveFrame("CommonMaskDown");
		TUITextureManager.Instance().RemoveFrame("CommonMaskRight");
		TUITextureManager.Instance().RemoveFrame("CommonMaskLeft");
	}
}

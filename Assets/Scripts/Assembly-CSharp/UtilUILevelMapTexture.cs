using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilUILevelMapTexture : MonoBehaviour
{
	[Serializable]
	public class BlockValue
	{
		public string m_name;

		public Color m_color;

		public Texture2D m_texture2d;

		public Vector2 m_position;

		public BlockValue()
		{
			m_texture2d = null;
			m_color = Color.black;
		}
	}

	public Material m_material;

	public bool isWeb;

	protected Material m_materialClone;

	public BlockValue m_map;

	public List<BlockValue> m_blockList = new List<BlockValue>();

	public Texture2D TEXTURE
	{
		get
		{
			Texture2D texture2d = m_map.m_texture2d;
			if (null == texture2d)
			{
				Debug.Log("no map texture!!!!!!!!!!!");
				return null;
			}
			Texture2D texture2D = new Texture2D(texture2d.width, texture2d.height, TextureFormat.ARGB32, false);
			texture2D.SetPixels(texture2d.GetPixels());
			List<LevelConf.Block> list = new List<LevelConf.Block>();
			if (!isWeb)
			{
				if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Fight)
				{
					list = DataCenter.StateSingle().GetLevelConfFight().GetBlockInfo();
				}
				else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Find)
				{
					list = DataCenter.StateSingle().GetLevelConfFind().GetBlockInfo();
				}
				else if (DataCenter.StateSingle().GetLevelInfo().mode == LevelMode.Survive)
				{
					list = DataCenter.StateSingle().GetLevelConfSurvive().GetBlockInfo();
				}
			}
			else
			{
				list = DataCenter.StateMulti().GetLevelConfDeathMatch().GetBlockInfo();
			}
			for (int i = 0; i < m_blockList.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (!(list[j].name == m_blockList[i].m_name) || !(list[j].enable == "1"))
					{
						continue;
					}
					Texture2D texture2d2 = m_blockList[i].m_texture2d;
					if (!(null != texture2d2))
					{
						continue;
					}
					int x = (int)(m_blockList[i].m_position.x / 512f * (float)texture2d.width);
					int y = (int)(m_blockList[i].m_position.y / 512f * (float)texture2d.height);
					Color[] pixels = texture2d2.GetPixels();
					Color[] pixels2 = texture2D.GetPixels(x, y, texture2d2.width, texture2d2.height);
					for (int k = 0; k < pixels.Length; k++)
					{
						if (0.1f < pixels[k].a)
						{
							pixels2[k] = AlphaBlend(pixels2[k], pixels[k]);
						}
					}
					texture2D.SetPixels(x, y, texture2d2.width, texture2d2.height, pixels2);
				}
			}
			texture2D.Apply();
			return texture2D;
		}
	}

	private Color AlphaBlend(Color src0, Color src1)
	{
		Color result = default(Color);
		if (src0.a != 0f)
		{
			result.a = 1f;
		}
		else
		{
			result.a = src1.a;
		}
		result.r = src1.r * src1.a + src0.r * (1f - src1.a);
		result.g = src1.g * src1.a + src0.g * (1f - src1.a);
		result.b = src1.b * src1.a + src0.b * (1f - src1.a);
		return result;
	}

	private void Start()
	{
		Texture2D tEXTURE = TEXTURE;
		if ((bool)m_material)
		{
			m_materialClone = UnityEngine.Object.Instantiate(m_material) as Material;
			m_materialClone.hideFlags = HideFlags.DontSave;
			m_materialClone.mainTexture = tEXTURE;
		}
		else
		{
			m_materialClone = null;
		}
		TUITextureManager.Instance().AddFrame(m_map.m_name, tEXTURE, m_materialClone, new Vector2(512f, 512f), new Vector4(0f, 1f, 1f, 0f));
	}

	private void OnDestroy()
	{
		TUITextureManager.Instance().RemoveFrame(m_map.m_name);
	}
}

using System;
using UnityEngine;

[ExecuteInEditMode]
public class TUITexture : MonoBehaviour
{
	[Serializable]
	public class FrameInfo
	{
		public string frameName;

		public Vector2 size;

		public Vector4 uv;
	}

	[Serializable]
	public class TextureInfo
	{
		public string textureFile;

		public FrameInfo[] frames;
	}

	public bool auto = true;

	public string path;

	public bool hd = true;

	public string atlasPath;

	public string atlasFile;

	public TextureInfo[] textureInfo;

	public TextureInfo[] textureInfoHD;

	public Material material;

	private Material[] materialClone;

	public void Awake()
	{
		if (auto)
		{
			LoadFrameAll();
		}
	}

	public void OnDestroy()
	{
		DeleteFrameAll();
	}

	public void LoadFrame(string frameName)
	{
		if (TUITextureManager.Instance().CheckFrame(frameName))
		{
			return;
		}
		TextureInfo[] array = ChooseTexture();
		if (materialClone == null)
		{
			materialClone = new Material[array.Length];
		}
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < array[i].frames.Length; j++)
			{
				if (array[i].frames[j].frameName == frameName)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Texture2D texture2D = Resources.Load(array[i].textureFile, typeof(Texture2D)) as Texture2D;
				if ((bool)material)
				{
					materialClone[i] = UnityEngine.Object.Instantiate(material) as Material;
					materialClone[i].hideFlags = HideFlags.DontSave;
					materialClone[i].mainTexture = texture2D;
				}
				else
				{
					materialClone[i] = null;
				}
				for (int k = 0; k < array[i].frames.Length; k++)
				{
					TUITextureManager.Instance().AddFrame(array[i].frames[k].frameName, texture2D, materialClone[i], array[i].frames[k].size, array[i].frames[k].uv);
				}
			}
		}
	}

	private void LoadFrameAll()
	{
		TextureInfo[] array = ChooseTexture();
		if (materialClone == null)
		{
			materialClone = new Material[array.Length];
		}
		for (int i = 0; i < array.Length; i++)
		{
			Texture2D texture2D = Resources.Load(array[i].textureFile, typeof(Texture2D)) as Texture2D;
			if ((bool)material)
			{
				materialClone[i] = UnityEngine.Object.Instantiate(material) as Material;
				materialClone[i].hideFlags = HideFlags.DontSave;
				materialClone[i].mainTexture = texture2D;
			}
			else
			{
				materialClone[i] = null;
			}
			for (int j = 0; j < array[i].frames.Length; j++)
			{
				TUITextureManager.Instance().AddFrame(array[i].frames[j].frameName, texture2D, materialClone[i], array[i].frames[j].size, array[i].frames[j].uv);
			}
		}
	}

	private void DeleteFrameAll()
	{
		TextureInfo[] array = ChooseTexture();
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < array[i].frames.Length; j++)
			{
				TUITextureManager.Instance().RemoveFrame(array[i].frames[j].frameName);
			}
		}
		if (materialClone == null)
		{
			return;
		}
		for (int k = 0; k < materialClone.Length; k++)
		{
			if (materialClone[k] != null)
			{
				UnityEngine.Object.DestroyImmediate(materialClone[k]);
			}
		}
	}

	private TextureInfo[] ChooseTexture()
	{
		if (!hd || !HD())
		{
			return textureInfo;
		}
		return textureInfoHD;
	}

	private bool HD()
	{
		if (Application.isPlaying)
		{
			if (Mathf.Max(Screen.width, Screen.height) > 900)
			{
				return true;
			}
			return false;
		}
		return false;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class TUIMeshSector : MonoBehaviour
{
	public bool Static = true;

	public Material material;

	public string frameName;

	public Color color = Color.white;

	public bool flipX;

	public bool flipY;

	public float angleBegin;

	public float angleEnd;

	protected Material materialClone;

	protected MeshFilter meshFilter;

	protected MeshRenderer meshRender;

	public void Awake()
	{
		if ((bool)material)
		{
			materialClone = UnityEngine.Object.Instantiate(material) as Material;
			materialClone.hideFlags = HideFlags.DontSave;
		}
		else
		{
			materialClone = null;
		}
		meshFilter = base.gameObject.GetComponent<MeshFilter>();
		meshRender = base.gameObject.GetComponent<MeshRenderer>();
		meshFilter.sharedMesh = new Mesh();
		meshFilter.sharedMesh.hideFlags = HideFlags.DontSave;
		meshRender.castShadows = false;
		meshRender.receiveShadows = false;
	}

	public void Start()
	{
		UpdateMesh();
	}

	private void OnDestroy()
	{
		if ((bool)meshFilter && (bool)meshFilter.sharedMesh)
		{
			UnityEngine.Object.DestroyImmediate(meshFilter.sharedMesh);
		}
		if ((bool)materialClone)
		{
			UnityEngine.Object.DestroyImmediate(materialClone);
		}
	}

	public void Update()
	{
		if (!Static)
		{
			UpdateMesh();
		}
	}

	public virtual void UpdateMesh()
	{
		if (meshFilter == null || meshRender == null)
		{
			return;
		}
		TUITextureManager.Frame frame = TUITextureManager.Instance().GetFrame(frameName);
		if ((bool)materialClone)
		{
			materialClone.mainTexture = frame.texture;
			meshRender.sharedMaterial = materialClone;
		}
		else if ((bool)frame.material)
		{
			meshRender.sharedMaterial = frame.material;
		}
		Vector4 uv = frame.uv;
		if (flipX)
		{
			float x = uv.x;
			uv.x = uv.z;
			uv.z = x;
		}
		if (flipY)
		{
			float y = uv.y;
			uv.y = uv.w;
			uv.w = y;
		}
		Vector3 vector = new Vector3(base.transform.position.x - (float)(int)base.transform.position.x, base.transform.position.y - (float)(int)base.transform.position.y, 0f);
		Vector3 vector2 = new Vector3(frame.size.x % 2f / 2f, frame.size.y % 2f / 2f, 0f);
		Vector3 vector3 = vector + vector2;
		Vector3[] sprite_vertices = new Vector3[4]
		{
			new Vector3(frame.size.x * -0.5f, frame.size.y * 0.5f, 0f) - vector3,
			new Vector3(frame.size.x * 0.5f, frame.size.y * 0.5f, 0f) - vector3,
			new Vector3(frame.size.x * 0.5f, frame.size.y * -0.5f, 0f) - vector3,
			new Vector3(frame.size.x * -0.5f, frame.size.y * -0.5f, 0f) - vector3
		};
		Vector2[] array = new Vector2[4]
		{
			new Vector2(uv.x, uv.y),
			new Vector2(uv.z, uv.y),
			new Vector2(uv.z, uv.w),
			new Vector2(uv.x, uv.w)
		};
		while (angleBegin < 0f)
		{
			angleBegin += 360f;
		}
		while (angleBegin > 360f)
		{
			angleBegin -= 360f;
		}
		while (angleEnd < 0f)
		{
			angleEnd += 360f;
		}
		while (angleEnd > 360f)
		{
			angleEnd -= 360f;
		}
		List<Vector3> list = new List<Vector3>();
		List<Vector2> list2 = new List<Vector2>();
		List<Color> list3 = new List<Color>();
		List<int> list4 = new List<int>();
		list.Add(vector3);
		list2.Add((array[0] + array[1] + array[2] + array[3]) / 4f);
		list3.Add(color);
		float[] array2 = new float[6] { 0f, 45f, 135f, 225f, 315f, 360f };
		bool flag = false;
		for (int i = 0; i < 5; i++)
		{
			float num = array2[i];
			float num2 = array2[i + 1];
			if (!flag)
			{
				if (!(angleBegin >= num) || !(angleBegin <= num2))
				{
					continue;
				}
				num = angleBegin;
				flag = true;
				Vector3 vertex = GetVertex(num, sprite_vertices);
				list.Add(vertex);
				list2.Add(GetUV(num, array, sprite_vertices, vertex));
				list3.Add(color);
			}
			if (angleEnd >= num && angleEnd <= num2)
			{
				num2 = angleEnd;
				flag = false;
			}
			Vector3 vertex2 = GetVertex(num2, sprite_vertices);
			list.Add(vertex2);
			list2.Add(GetUV(num2, array, sprite_vertices, vertex2));
			list3.Add(color);
			list4.Add(0);
			list4.Add(list.Count - 1);
			list4.Add(list.Count - 2);
		}
		meshFilter.sharedMesh.Clear();
		meshFilter.sharedMesh.vertices = list.ToArray();
		meshFilter.sharedMesh.uv = list2.ToArray();
		meshFilter.sharedMesh.colors = list3.ToArray();
		meshFilter.sharedMesh.triangles = list4.ToArray();
	}

	private Vector3 GetVertex(float angle, Vector3[] sprite_vertices)
	{
		float num = 0f;
		float num2 = 0f;
		if (angle >= 0f && angle < 45f)
		{
			num = sprite_vertices[1].x;
			num2 = num * Mathf.Tan(angle * ((float)Math.PI / 180f));
		}
		else if (angle >= 45f && angle < 90f)
		{
			num2 = sprite_vertices[1].y;
			num = num2 * Mathf.Tan((90f - angle) * ((float)Math.PI / 180f));
		}
		else if (angle >= 90f && angle < 135f)
		{
			num2 = sprite_vertices[1].y;
			num = (0f - num2) * Mathf.Tan((angle - 90f) * ((float)Math.PI / 180f));
		}
		else if (angle >= 135f && angle < 180f)
		{
			num = sprite_vertices[0].x;
			num2 = (0f - num) * Mathf.Tan((180f - angle) * ((float)Math.PI / 180f));
		}
		else if (angle >= 180f && angle < 225f)
		{
			num = sprite_vertices[0].x;
			num2 = num * Mathf.Tan((angle - 180f) * ((float)Math.PI / 180f));
		}
		else if (angle >= 225f && angle < 270f)
		{
			num2 = sprite_vertices[3].y;
			num = num2 * Mathf.Tan((270f - angle) * ((float)Math.PI / 180f));
		}
		else if (angle >= 270f && angle < 315f)
		{
			num2 = sprite_vertices[3].y;
			num = (0f - num2) * Mathf.Tan((angle - 270f) * ((float)Math.PI / 180f));
		}
		else
		{
			num = sprite_vertices[1].x;
			num2 = (0f - num) * Mathf.Tan((360f - angle) * ((float)Math.PI / 180f));
		}
		return new Vector3(num, num2, 0f);
	}

	private Vector2 GetUV(float angle, Vector2[] sprite_uv, Vector3[] sprite_vertices, Vector3 vertex)
	{
		if (angle >= 0f && angle < 45f)
		{
			float t = (vertex.y - sprite_vertices[1].y) / (sprite_vertices[2].y - sprite_vertices[1].y);
			return Vector2.Lerp(sprite_uv[1], sprite_uv[2], t);
		}
		if (angle >= 45f && angle < 135f)
		{
			float t2 = (vertex.x - sprite_vertices[0].x) / (sprite_vertices[1].x - sprite_vertices[0].x);
			return Vector2.Lerp(sprite_uv[0], sprite_uv[1], t2);
		}
		if (angle >= 135f && angle < 225f)
		{
			float t3 = (vertex.y - sprite_vertices[0].y) / (sprite_vertices[3].y - sprite_vertices[0].y);
			return Vector2.Lerp(sprite_uv[0], sprite_uv[3], t3);
		}
		if (angle >= 225f && angle < 315f)
		{
			float t4 = (vertex.x - sprite_vertices[2].x) / (sprite_vertices[3].x - sprite_vertices[2].x);
			return Vector2.Lerp(sprite_uv[2], sprite_uv[3], t4);
		}
		float t5 = (vertex.y - sprite_vertices[1].y) / (sprite_vertices[2].y - sprite_vertices[1].y);
		return Vector2.Lerp(sprite_uv[1], sprite_uv[2], t5);
	}
}

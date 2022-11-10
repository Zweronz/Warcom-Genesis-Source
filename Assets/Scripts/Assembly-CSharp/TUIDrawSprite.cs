using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class TUIDrawSprite : MonoBehaviour
{
	public enum Clipping
	{
		None = 0,
		HardClip = 1
	}

	public Material material;

	public Clipping clippingType;

	public TUIRect2 clippingRect;

	protected MeshFilter meshFilter;

	protected MeshRenderer meshRenderer;

	private Bounds bounds = default(Bounds);

	private bool isDirty = true;

	public Material PlayingMaterial
	{
		get
		{
			if (Application.isPlaying && null != meshRenderer)
			{
				return meshRenderer.material;
			}
			return null;
		}
	}

	public Bounds Bounds
	{
		get
		{
			RecalculateBounds();
			return bounds;
		}
	}

	public void Draw(List<Vector3> vertices, List<int> triangles, List<Vector2> uv)
	{
		Initialize();
		meshFilter.sharedMesh.Clear();
		meshFilter.sharedMesh.vertices = vertices.ToArray();
		meshFilter.sharedMesh.triangles = triangles.ToArray();
		meshFilter.sharedMesh.uv = uv.ToArray();
		isDirty = true;
	}

	public void Draw(List<Vector3> vertices, List<int> triangles, List<Vector2> uv, List<Color> colors)
	{
		Draw(vertices, triangles, uv);
		meshFilter.sharedMesh.colors = colors.ToArray();
	}

	private void Initialize()
	{
		if (null == meshFilter)
		{
			meshFilter = GetComponent<MeshFilter>();
			meshFilter.sharedMesh = new Mesh();
		}
		if (null == meshRenderer)
		{
			meshRenderer = GetComponent<MeshRenderer>();
		}
		if (null == meshRenderer.sharedMaterial || material != meshRenderer.sharedMaterial)
		{
			if (Application.isPlaying)
			{
				meshRenderer.material = material;
			}
			else
			{
				meshRenderer.sharedMaterial = material;
			}
		}
		if (Application.isPlaying && null != meshRenderer.material)
		{
			meshRenderer.material.shader = SwitchClipShader();
		}
	}

	private void RecalculateBounds()
	{
		if (null != meshFilter && null != meshFilter.sharedMesh && isDirty)
		{
			meshFilter.sharedMesh.RecalculateBounds();
			bounds = meshFilter.sharedMesh.bounds;
			isDirty = false;
		}
	}

	private void OnDestroy()
	{
		if (null != meshFilter && null != meshFilter.sharedMesh)
		{
			Object.DestroyImmediate(meshFilter.sharedMesh);
		}
	}

	private void Update()
	{
		if (clippingType != 0 && null != meshRenderer && null != clippingRect)
		{
			if (Application.isPlaying && null != meshRenderer.material)
			{
				meshRenderer.material.SetVector("_Rect", clippingRect.GetRectViewPort());
			}
			else
			{
				meshRenderer.sharedMaterial.SetVector("_Rect", clippingRect.GetRectViewPort());
			}
		}
	}

	private Shader SwitchClipShader()
	{
		Clipping clipping = clippingType;
		if (clipping == Clipping.HardClip)
		{
			return Shader.Find("Unlit/Transparent Colored TwoTexture Vertex Color(HardClip)");
		}
		return Shader.Find("Triniti/Sprite");
	}
}

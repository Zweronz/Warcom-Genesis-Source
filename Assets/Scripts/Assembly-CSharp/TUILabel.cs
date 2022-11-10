using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("TUI/Control/Label")]
[RequireComponent(typeof(TUIDrawSprite))]
public class TUILabel : MonoBehaviour
{
	public enum TUIPivot
	{
		TopLeft = 0,
		Top = 1,
		TopRight = 2,
		Left = 3,
		Center = 4,
		Right = 5,
		BottomLeft = 6,
		Bottom = 7,
		BottomRight = 8
	}

	[SerializeField]
	protected TUIFont2 font;

	[SerializeField]
	protected TUIFont2 fontHD;

	[SerializeField]
	protected float scale = 1f;

	[SerializeField]
	protected TUIFont2.Alignment alignment;

	[SerializeField]
	protected TUIPivot pivot;

	[SerializeField]
	protected string textID = string.Empty;

	[SerializeField]
	protected string text = string.Empty;

	[SerializeField]
	protected int maxLineWidth;

	[SerializeField]
	protected bool encoding = true;

	[SerializeField]
	protected bool multiline = true;

	[SerializeField]
	protected bool password;

	[SerializeField]
	protected bool showLastChar;

	[SerializeField]
	protected int lineWidth;

	public Color color = Color.white;

	public Color colorBK = Color.black;

	protected bool shouldBeProcessed = true;

	protected string processedText;

	protected string lastTextID = string.Empty;

	protected string lastText = string.Empty;

	protected int lastWidth;

	protected bool lastEncoding = true;

	protected bool lastMulti = true;

	protected bool lastPass;

	protected bool lastShow;

	protected TUIPivot lastPivot;

	protected TUIFont2.Alignment lastAlignment;

	protected Color lastColor = Color.white;

	protected Color lastColorBK = Color.black;

	protected float lastScale = 1f;

	protected TUIFont2 currentFont;

	protected TUIDrawSprite drawSprite;

	protected Material material;

	protected TUIGeometry geometry = new TUIGeometry();

	public float Scale
	{
		get
		{
			return scale;
		}
		set
		{
			if (scale != value)
			{
				scale = value;
			}
		}
	}

	public TUIPivot Pivot
	{
		get
		{
			return pivot;
		}
		set
		{
			pivot = value;
			HasChanged = true;
		}
	}

	public string TextID
	{
		get
		{
			return textID;
		}
		set
		{
			textID = value;
			Text = TUITextManager.Instance().GetString(textID);
		}
	}

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			if (value != null && text != value)
			{
				text = value;
				HasChanged = true;
			}
		}
	}

	public bool SupportEncoding
	{
		get
		{
			return encoding;
		}
		set
		{
			if (encoding != value)
			{
				encoding = value;
				HasChanged = true;
				if (value)
				{
					password = false;
				}
			}
		}
	}

	public int LineWidth
	{
		get
		{
			return maxLineWidth;
		}
		set
		{
			if (maxLineWidth != value)
			{
				maxLineWidth = value;
				HasChanged = true;
			}
		}
	}

	public bool MultiLine
	{
		get
		{
			return multiline;
		}
		set
		{
			if (multiline != value)
			{
				multiline = value;
				HasChanged = true;
				if (value)
				{
					password = false;
				}
			}
		}
	}

	public bool Password
	{
		get
		{
			return password;
		}
		set
		{
			if (password != value)
			{
				password = value;
				multiline = false;
				encoding = false;
				HasChanged = true;
			}
		}
	}

	public bool ShowLastPasswordChar
	{
		get
		{
			return showLastChar;
		}
		set
		{
			if (showLastChar != value)
			{
				showLastChar = value;
				HasChanged = true;
			}
		}
	}

	public string ProcessedText
	{
		get
		{
			if (shouldBeProcessed)
			{
				if (!string.IsNullOrEmpty(textID))
				{
					TextID = textID;
				}
				if (TUI.IsRetina())
				{
					if (null != fontHD)
					{
						processedText = fontHD.WrapText(text, (float)(maxLineWidth * 2) / scale, multiline, encoding);
					}
					else
					{
						processedText = font.WrapText(text, (float)maxLineWidth / scale, multiline, encoding);
					}
				}
				else if (null != font)
				{
					processedText = font.WrapText(text, (float)maxLineWidth / scale, multiline, encoding);
				}
				else
				{
					processedText = fontHD.WrapText(text, (float)(maxLineWidth * 2) / scale, multiline, encoding);
				}
				shouldBeProcessed = false;
			}
			return processedText;
		}
	}

	public Bounds Bounds
	{
		get
		{
			return geometry.Bounds;
		}
	}

	private bool HasChanged
	{
		get
		{
			return shouldBeProcessed || (lastText != text && string.IsNullOrEmpty(textID)) || lastWidth != maxLineWidth || lastEncoding != encoding || lastMulti != multiline || lastPass != password || lastShow != showLastChar || lastPivot != pivot || lastAlignment != alignment || lastColor != color || lastColorBK != colorBK || lastTextID != textID || lastScale != scale;
		}
		set
		{
			if (value)
			{
				shouldBeProcessed = true;
				return;
			}
			shouldBeProcessed = false;
			lastTextID = textID;
			lastText = text;
			lastWidth = maxLineWidth;
			lastEncoding = encoding;
			lastMulti = multiline;
			lastPass = password;
			lastShow = showLastChar;
			lastPivot = pivot;
			lastAlignment = alignment;
			lastColor = color;
			lastColorBK = colorBK;
		}
	}

	public void SetFormatText(string textId, params object[] parms)
	{
		Text = TUITool.StringFormat(TUITextManager.Instance().GetString(textId), parms);
	}

	protected void Layout()
	{
		geometry.RecalculateBounds();
		Bounds bounds = geometry.Bounds;
		Matrix4x4 identity = Matrix4x4.identity;
		Vector3 zero = Vector3.zero;
		switch (Pivot)
		{
		case TUIPivot.Center:
			zero.x = bounds.center.x * -1f;
			zero.y = bounds.center.y * -1f;
			break;
		case TUIPivot.Left:
			zero.x = bounds.min.x * -1f;
			zero.y = bounds.center.y * -1f;
			break;
		case TUIPivot.Right:
			zero.x = bounds.max.x * -1f;
			zero.y = bounds.center.y * -1f;
			break;
		case TUIPivot.Top:
			zero.x = bounds.center.x * -1f;
			zero.y = bounds.max.y * -1f;
			break;
		case TUIPivot.Bottom:
			zero.x = bounds.center.x * -1f;
			zero.y = bounds.min.y * -1f;
			break;
		case TUIPivot.BottomLeft:
			zero.x = bounds.min.x * -1f;
			zero.y = bounds.min.y * -1f;
			break;
		case TUIPivot.TopLeft:
			zero.x = bounds.min.x * -1f;
			zero.y = bounds.max.y * -1f;
			break;
		case TUIPivot.TopRight:
			zero.x = bounds.max.x * -1f;
			zero.y = bounds.max.y * -1f;
			break;
		case TUIPivot.BottomRight:
			zero.x = bounds.max.x * -1f;
			zero.y = bounds.min.y * -1f;
			break;
		}
		identity.SetTRS(zero, Quaternion.identity, Vector3.one);
		geometry.TRS(identity);
	}

	private void MakePositionPerfect()
	{
		Vector3 position = base.transform.position;
		position.x = Mathf.RoundToInt(position.x);
		position.y = Mathf.RoundToInt(position.y);
		Vector3 size = geometry.Bounds.size;
		int num = Mathf.RoundToInt(size.x);
		int num2 = Mathf.RoundToInt(size.y);
		if (num % 2 == 1 && (pivot == TUIPivot.Top || pivot == TUIPivot.Center || pivot == TUIPivot.Bottom))
		{
			position.x += 0.5f;
		}
		if (num2 % 2 == 1 && (pivot == TUIPivot.Left || pivot == TUIPivot.Center || pivot == TUIPivot.Right))
		{
			position.y -= 0.5f;
		}
		base.transform.position = position;
	}

	private void Draw()
	{
		geometry.Clear();
		if (null == currentFont)
		{
			ChooseFont();
			if (null != currentFont)
			{
				material = currentFont.material;
				drawSprite.material = material;
			}
		}
		if (!(null == currentFont))
		{
			if (currentFont == fontHD)
			{
				fontHD.Print(ProcessedText, color, geometry.Vertices, geometry.Triangles, geometry.Uv, geometry.Colors, 0.5f * scale, encoding, alignment, lineWidth);
			}
			else
			{
				font.Print(ProcessedText, color, geometry.Vertices, geometry.Triangles, geometry.Uv, geometry.Colors, scale, encoding, alignment, lineWidth);
			}
			Layout();
			MakePositionPerfect();
			drawSprite.Draw(geometry.Vertices, geometry.Triangles, geometry.Uv, geometry.Colors);
			if (null != drawSprite.PlayingMaterial)
			{
				drawSprite.PlayingMaterial.SetColor("_ColorB", colorBK);
			}
		}
	}

	private void Start()
	{
		drawSprite = GetComponent<TUIDrawSprite>();
		ChooseFont();
		if (null != currentFont)
		{
			material = currentFont.material;
			drawSprite.material = material;
		}
	}

	private void LateUpdate()
	{
		if (HasChanged)
		{
			shouldBeProcessed = true;
			Draw();
			HasChanged = false;
		}
	}

	private void ChooseFont()
	{
		if (null == font && null != fontHD)
		{
			currentFont = fontHD;
		}
		else if (null == fontHD && null != font)
		{
			currentFont = font;
		}
		else if (null != fontHD && null != font)
		{
			if (TUI.IsRetina())
			{
				currentFont = fontHD;
			}
			else
			{
				currentFont = font;
			}
		}
	}
}

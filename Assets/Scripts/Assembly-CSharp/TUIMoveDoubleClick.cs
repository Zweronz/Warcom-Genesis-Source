using UnityEngine;

public class TUIMoveDoubleClick : TUIControlImpl
{
	public const int CommandBegin = 1;

	public const int CommandMove = 2;

	public const int CommandEnd = 3;

	public const int CommandClick = 4;

	public const int CommandDouleClickBegin = 5;

	public const int CommandDouleClickEnd = 6;

	public float minX;

	public float minY;

	public GameObject frame;

	protected int fingerId = -1;

	protected Vector2 position = Vector2.zero;

	protected bool move;

	protected bool doubleClickOn;

	private float m_beginTime;

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (m_beginTime != 0f)
			{
				if (Time.time - m_beginTime > 0.4f)
				{
					m_beginTime = Time.time;
					doubleClickOn = false;
				}
				else
				{
					doubleClickOn = true;
				}
			}
			else
			{
				doubleClickOn = false;
				m_beginTime = Time.time;
			}
			if (PtInControl(input.position))
			{
				if (doubleClickOn && Time.time - m_beginTime < 0.4f)
				{
					PostEvent(this, 5, 0f, 0f, null);
					DoReset();
					DoMove(input.position);
				}
				if (move)
				{
					fingerId = -1;
					position = Vector2.zero;
					move = false;
					PostEvent(this, 3, 0f, 0f, null);
				}
				fingerId = input.fingerId;
				position = input.position;
				move = false;
			}
			return false;
		}
		if (input.fingerId != fingerId)
		{
			return false;
		}
		if (input.inputType == TUIInputType.Moved)
		{
			if (!PtInControl(input.position))
			{
				return false;
			}
			float num = input.position.x - position.x;
			float num2 = input.position.y - position.y;
			if (move)
			{
				position = input.position;
				if (doubleClickOn)
				{
					DoMove(input.position);
				}
				PostEvent(this, 2, num, num2, null);
			}
			else
			{
				float num3 = ((!(num >= 0f)) ? (0f - num) : num);
				float num4 = ((!(num2 >= 0f)) ? (0f - num2) : num2);
				if (num3 > minX || num4 > minY)
				{
					position = input.position;
					move = true;
					PostEvent(this, 1, 0f, 0f, null);
				}
			}
			return true;
		}
		if (input.inputType == TUIInputType.Ended)
		{
			bool flag = move;
			fingerId = -1;
			position = Vector2.zero;
			move = false;
			if (!doubleClickOn)
			{
				if (Time.time - m_beginTime < 0.2f && !flag)
				{
					PostEvent(this, 4, 0f, 0f, null);
					return true;
				}
				if (flag)
				{
					PostEvent(this, 3, 0f, 0f, null);
					return true;
				}
				return false;
			}
			PostEvent(this, 6, 0f, 0f, null);
			DoReset();
			return true;
		}
		return false;
	}

	private void DoReset()
	{
		frame.transform.position = new Vector3(0f, 30000f, frame.transform.position.z);
	}

	private void DoMove(Vector2 position)
	{
		frame.transform.position = position;
	}
}

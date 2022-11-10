using UnityEngine;

public class GUIJoystickB : TUIControlImpl
{
	public const int CommandDown = 1;

	public const int CommandMove = 2;

	public const int CommandUp = 3;

	public float minX;

	public float minY;

	protected int fingerId = -1;

	protected Vector2 position = Vector2.zero;

	protected bool move;

	public GameObject frame;

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				if (move)
				{
					fingerId = -1;
					position = Vector2.zero;
					move = false;
					PostEvent(this, 3, 0f, 0f, null);
					DoUp();
				}
				fingerId = input.fingerId;
				position = input.position;
				PostEvent(this, 1, 0f, 0f, null);
				DoDown();
				move = false;
				return true;
			}
			return false;
		}
		if (input.fingerId != fingerId)
		{
			return false;
		}
		if (input.inputType == TUIInputType.Moved)
		{
			float num = input.position.x - position.x;
			float num2 = input.position.y - position.y;
			if (move)
			{
				position = input.position;
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
					PostEvent(this, 2, num, num2, null);
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
			PostEvent(this, 3, 0f, 0f, null);
			DoUp();
			return true;
		}
		return false;
	}

	private void DoDown()
	{
		frame.GetComponent<TUIMeshSprite>().frameName = "JoystickFireButton02";
		frame.GetComponent<TUIMeshSprite>().UpdateMesh();
	}

	private void DoUp()
	{
		frame.GetComponent<TUIMeshSprite>().frameName = "JoystickFireButton01";
		frame.GetComponent<TUIMeshSprite>().UpdateMesh();
	}
}

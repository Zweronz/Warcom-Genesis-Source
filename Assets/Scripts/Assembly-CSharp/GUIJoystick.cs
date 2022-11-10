using UnityEngine;

public class GUIJoystick : TUIControlImpl
{
	public const int CommandDown = 1;

	public const int CommandMove = 2;

	public const int CommandUp = 3;

	public float min;

	public float max;

	public GameObject frame;

	protected int fingerId = -1;

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				fingerId = input.fingerId;
				DoReset();
				PostEvent(this, 1, 0f, 0f, null);
				Vector2 vector = DoMove(input.position);
				PostEvent(this, 2, vector.x, vector.y, null);
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
			Vector2 vector2 = DoMove(input.position);
			PostEvent(this, 2, vector2.x, vector2.y, null);
			return true;
		}
		if (input.inputType == TUIInputType.Ended)
		{
			fingerId = -1;
			DoReset();
			PostEvent(this, 3, 0f, 0f, null);
			return true;
		}
		return false;
	}

	public void Reset()
	{
		fingerId = -1;
		DoReset();
		PostEvent(this, 3, 0f, 0f, null);
	}

	private void DoReset()
	{
		frame.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, frame.transform.position.z);
	}

	private Vector2 DoMove(Vector2 position)
	{
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y);
		Vector2 vector2 = position - vector;
		float magnitude = vector2.magnitude;
		float value = (magnitude - min) / (max - min);
		value = Mathf.Clamp(value, 0f, 1f);
		Vector2 vector3 = value * vector2 / magnitude;
		Vector2 vector4 = vector + vector3 * max;
		frame.transform.position = new Vector3(vector4.x, vector4.y, frame.transform.position.z);
		return vector3;
	}
}

using System.Collections.Generic;
using UnityEngine;

internal class TUIInputManagerandroid
{
	private static TUIInput[] m_input;

	public static void UpdateInput()
	{
		List<TUIInput> list = new List<TUIInput>();
		TUIInput item = default(TUIInput);
		TUIInput item2 = default(TUIInput);
		TUIInput item3 = default(TUIInput);
		TUIInput item4 = default(TUIInput);
		TUIInput item5 = default(TUIInput);
		for (int i = 0; i < Input.touches.Length; i++)
		{
			if (Input.touches[i].phase == TouchPhase.Began)
			{
				item.fingerId = Input.touches[i].fingerId;
				item.inputType = TUIInputType.Began;
				item.position = Input.touches[i].position;
				list.Add(item);
			}
			else if (Input.touches[i].phase == TouchPhase.Moved)
			{
				item2.fingerId = Input.touches[i].fingerId;
				item2.inputType = TUIInputType.Moved;
				item2.position = Input.touches[i].position;
				list.Add(item2);
			}
			else if (Input.touches[i].phase == TouchPhase.Ended)
			{
				item3.fingerId = Input.touches[i].fingerId;
				item3.inputType = TUIInputType.Ended;
				item3.position = Input.touches[i].position;
				list.Add(item3);
			}
			else if (Input.touches[i].phase == TouchPhase.Stationary)
			{
				item4.fingerId = Input.touches[i].fingerId;
				item4.inputType = TUIInputType.Stationary;
				item4.position = Input.touches[i].position;
				list.Add(item4);
			}
			else if (Input.touches[i].phase == TouchPhase.Canceled)
			{
				item5.fingerId = Input.touches[i].fingerId;
				item5.inputType = TUIInputType.Canceled;
				item5.position = Input.touches[i].position;
				list.Add(item5);
			}
		}
		m_input = list.ToArray();
	}

	public static TUIInput[] GetInput()
	{
		return m_input;
	}
}

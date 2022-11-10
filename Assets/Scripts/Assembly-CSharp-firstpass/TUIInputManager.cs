using UnityEngine;

public class TUIInputManager
{
	private static int m_lastFrameCount = -1;

	public static TUIInput[] GetInput()
	{
		if (Time.frameCount != m_lastFrameCount)
		{
			if (!Application.isMobilePlatform)
			{
				TUIInputManagerWindows.UpdateInput();
			}
			else
			{
				TUIInputManagerandroid.UpdateInput();
			}
		}
		m_lastFrameCount = Time.frameCount;
		if (!Application.isMobilePlatform)
		{
			return TUIInputManagerWindows.GetInput();
		}
		return TUIInputManagerandroid.GetInput();
	}
}

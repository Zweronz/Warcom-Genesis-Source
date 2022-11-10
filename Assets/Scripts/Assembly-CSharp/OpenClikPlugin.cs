public class OpenClikPlugin
{
	private enum Status
	{
		kShowBottom = 0,
		kShowFull = 1,
		kShowTop = 2,
		kHide = 3
	}

	private static Status s_Status;

	public static void Initialize(string key, string appSignature = "")
	{
		ChartBoostAndroid.init(key, appSignature);
		ChartBoostAndroid.onStart();
		ChartBoostAndroid.cacheInterstitial(null);
		s_Status = Status.kHide;
	}

	public static void Request(int type)
	{
	}

	public static void Show(int type)
	{
		ChartBoostAndroid.showInterstitial(null);
	}

	public static void Show(bool show_full)
	{
		if (s_Status == Status.kHide)
		{
			if (show_full)
			{
				ChartBoostAndroid.showInterstitial(null);
			}
			if (show_full)
			{
				s_Status = Status.kShowFull;
			}
			else
			{
				s_Status = Status.kShowBottom;
			}
		}
		else if (s_Status == Status.kShowFull)
		{
			if (!show_full)
			{
				if (show_full)
				{
					ChartBoostAndroid.showInterstitial(null);
				}
				s_Status = Status.kShowBottom;
			}
		}
		else if (s_Status == Status.kShowBottom && show_full)
		{
			if (show_full)
			{
				ChartBoostAndroid.showInterstitial(null);
			}
			s_Status = Status.kShowFull;
		}
	}

	public static void Hide()
	{
		s_Status = Status.kHide;
	}

	public static bool IsAdReady()
	{
		return true;
	}

	public static void Refresh()
	{
	}
}

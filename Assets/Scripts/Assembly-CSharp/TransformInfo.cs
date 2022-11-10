public class TransformInfo
{
	public NetworkTransform trans;

	public string objectId;

	public TransformInfo(NetworkTransform _trans, double time, string _objectId)
	{
		trans = _trans;
		trans.TimeStamp = time;
		objectId = _objectId;
	}

	public TransformInfo(NetworkTransform _trans, string _objectId)
	{
		trans = _trans;
		objectId = _objectId;
	}

	public TransformInfo()
	{
	}

	public void SetTimeStamp(double time)
	{
		if (trans != null)
		{
			trans.TimeStamp = time;
		}
	}
}

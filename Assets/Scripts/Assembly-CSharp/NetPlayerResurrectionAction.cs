public class NetPlayerResurrectionAction
{
	public string resurrectPoint;

	public float direction;

	public double timeStamp;

	public NetPlayerResurrectionAction(string resurrectPoint, float direction, double timeStamp)
	{
		this.resurrectPoint = resurrectPoint;
		this.direction = direction;
		this.timeStamp = timeStamp;
	}

	public NetPlayerResurrectionAction()
	{
	}
}

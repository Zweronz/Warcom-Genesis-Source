public class NetPlayerDeadAction
{
	public bool headShot;

	public int byWho;

	public int byWhat;

	public double timeStamp;

	public NetPlayerDeadAction(bool headShot, int byWho, int byWhat, double timeStamp)
	{
		this.headShot = headShot;
		this.byWho = byWho;
		this.byWhat = byWhat;
		this.timeStamp = timeStamp;
	}

	public NetPlayerDeadAction()
	{
	}
}

public class NetPlayerSettingInfo
{
	public int cTypeID;

	public string modelName;

	public int seatID;

	public int netID;

	public string nickName;

	public string weapon01 = string.Empty;

	public string weapon02 = string.Empty;

	public string weapon03 = string.Empty;

	public string armor01 = string.Empty;

	public string armor02 = string.Empty;

	public string armor03 = string.Empty;

	public NetPlayerSettingInfo(int cInfoID, string modelName, int seatID, int netID, string nickName, string weapon01, string weapon02, string weapon03, string armor01, string armor02, string armor03)
	{
		cTypeID = cInfoID;
		this.modelName = modelName;
		this.seatID = seatID;
		this.netID = netID;
		this.nickName = nickName;
		this.weapon01 = weapon01;
		this.weapon02 = weapon02;
		this.weapon03 = weapon03;
		this.armor01 = armor01;
		this.armor02 = armor02;
		this.armor03 = armor03;
	}

	public NetPlayerSettingInfo()
	{
	}
}

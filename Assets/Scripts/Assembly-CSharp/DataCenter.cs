public class DataCenter
{
	private static DataCenter instance;

	private DataConf dataConf;

	private DataSave dataSave;

	private DataStateCommon dataStateCommon;

	private DataStateSingle dataStateSingle;

	private DataStateMulti dataStateMulti;

	public DataCenter()
	{
		dataConf = new DataConf();
		dataSave = new DataSave();
		dataStateCommon = new DataStateCommon();
		dataStateSingle = new DataStateSingle();
		dataStateMulti = new DataStateMulti();
	}

	public static DataConf Conf()
	{
		return Instance().GetDataConf();
	}

	public static DataSave Save()
	{
		return Instance().GetDataSave();
	}

	public static DataStateCommon StateCommon()
	{
		return Instance().GetDataStateCommon();
	}

	public static DataStateSingle StateSingle()
	{
		return Instance().GetDataStateSingle();
	}

	public static DataStateMulti StateMulti()
	{
		return Instance().GetDataStateMulti();
	}

	private static DataCenter Instance()
	{
		if (instance == null)
		{
			instance = new DataCenter();
		}
		return instance;
	}

	private DataConf GetDataConf()
	{
		return dataConf;
	}

	private DataSave GetDataSave()
	{
		return dataSave;
	}

	private DataStateCommon GetDataStateCommon()
	{
		return dataStateCommon;
	}

	private DataStateSingle GetDataStateSingle()
	{
		return dataStateSingle;
	}

	private DataStateMulti GetDataStateMulti()
	{
		return dataStateMulti;
	}
}

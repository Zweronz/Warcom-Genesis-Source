public class DataConstant
{
	public enum DevicedType
	{
		E_IOS = 0,
		E_GooglePlay = 1,
		E_Amazon = 2
	}

	public const DevicedType DeviceType = DevicedType.E_GooglePlay;

	public const string m_strGooglePlayKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAswqzY36PRohKXYH8/9Yi8Fa2CQ1fkRurivpK0mj/roJLQIANFY2B+qjaztxh5dE1FsdR4sqkZ6dWnCLYTH9zwfTsXE8FfdHgJkXmClHLj8HCOYva4+nHj9gs7nmrV/HT40ZaEoAVLT4vL7ReNDjl1OP+qWvhxTPXMKp80FIkGYEQDJcYtjOElN7v2dgQoSALucOvcE+fpkAgJ7Cvijqb++T5N4hJ9hbUdykZ1/aHFtup3rQvdweLCcnJvpZLNqqd/uamy8DcJ1s2qMXaprJJTk2Nn4weVGlaIB0tAlPDi7n9L7a+hyfD0VsAlzTdfg7LtOlyakum6S0iJQIejxrEJQIDAQAB";

	public static bool bDebug = true;
}

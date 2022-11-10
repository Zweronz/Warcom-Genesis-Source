public class LevelInfo
{
	public LevelType type;

	public LevelMode mode;

	public LevelScene scene;

	public int id;

	public int enemyKind;

	public int textId;

	public bool pass;

	public int weekFluctuation;

	public LevelInfo clone()
	{
		if (this != null)
		{
			LevelInfo levelInfo = new LevelInfo();
			levelInfo.type = type;
			levelInfo.mode = mode;
			levelInfo.scene = scene;
			levelInfo.id = id;
			levelInfo.enemyKind = enemyKind;
			levelInfo.textId = textId;
			levelInfo.pass = pass;
			levelInfo.weekFluctuation = weekFluctuation;
			return levelInfo;
		}
		return null;
	}
}

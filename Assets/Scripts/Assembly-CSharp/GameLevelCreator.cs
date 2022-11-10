using UnityEngine;

public class GameLevelCreator
{
	private static LevelInfo[] plusLevelInfo = new LevelInfo[9];

	public static LevelInfo[] CreateLevel()
	{
		LevelInfo levelInfo = new LevelInfo();
		levelInfo = ((DataCenter.Save().GetLastBoneLevel() == null) ? null : DataCenter.Save().GetLastBoneLevel().clone());
		plusLevelInfo[0] = new LevelInfo();
		plusLevelInfo[0].type = LevelType.Main;
		plusLevelInfo[0].scene = LevelScene.Level04;
		plusLevelInfo[0].mode = LevelMode.Fight;
		plusLevelInfo[0].id = -1;
		plusLevelInfo[0].pass = false;
		plusLevelInfo[0].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[0].enemyKind = 1;
		plusLevelInfo[0].weekFluctuation = 0;
		plusLevelInfo[1] = new LevelInfo();
		plusLevelInfo[1].type = LevelType.Main;
		plusLevelInfo[1].scene = LevelScene.Level02;
		plusLevelInfo[1].mode = LevelMode.Find;
		plusLevelInfo[1].id = -1;
		plusLevelInfo[1].pass = false;
		plusLevelInfo[1].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[1].enemyKind = 1;
		plusLevelInfo[1].weekFluctuation = 0;
		plusLevelInfo[2] = new LevelInfo();
		plusLevelInfo[2].type = LevelType.Main;
		plusLevelInfo[2].scene = LevelScene.Level03;
		plusLevelInfo[2].mode = LevelMode.Survive;
		plusLevelInfo[2].id = -1;
		plusLevelInfo[2].pass = false;
		plusLevelInfo[2].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[2].enemyKind = 1;
		plusLevelInfo[2].weekFluctuation = 0;
		plusLevelInfo[3] = new LevelInfo();
		plusLevelInfo[3].type = LevelType.Main;
		plusLevelInfo[3].scene = LevelScene.Level01;
		plusLevelInfo[3].mode = LevelMode.Fight;
		plusLevelInfo[3].id = -1;
		plusLevelInfo[3].pass = false;
		plusLevelInfo[3].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[3].enemyKind = 1;
		plusLevelInfo[3].weekFluctuation = 0;
		plusLevelInfo[4] = new LevelInfo();
		plusLevelInfo[4].type = LevelType.Main;
		plusLevelInfo[4].scene = LevelScene.Level04;
		plusLevelInfo[4].mode = LevelMode.Find;
		plusLevelInfo[4].id = 0;
		plusLevelInfo[4].pass = false;
		plusLevelInfo[4].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[4].enemyKind = 1;
		plusLevelInfo[4].weekFluctuation = 0;
		plusLevelInfo[5] = new LevelInfo();
		plusLevelInfo[5].type = LevelType.Main;
		plusLevelInfo[5].scene = LevelScene.Level03;
		plusLevelInfo[5].mode = LevelMode.Survive;
		plusLevelInfo[5].id = 0;
		plusLevelInfo[5].pass = false;
		plusLevelInfo[5].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[5].enemyKind = 1;
		plusLevelInfo[5].weekFluctuation = 0;
		plusLevelInfo[6] = new LevelInfo();
		plusLevelInfo[6].type = LevelType.Main;
		plusLevelInfo[6].scene = LevelScene.Level02;
		plusLevelInfo[6].mode = LevelMode.Fight;
		plusLevelInfo[6].id = 0;
		plusLevelInfo[6].pass = false;
		plusLevelInfo[6].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[6].enemyKind = 1;
		plusLevelInfo[6].weekFluctuation = 0;
		plusLevelInfo[7] = new LevelInfo();
		plusLevelInfo[7].type = LevelType.Main;
		plusLevelInfo[7].scene = LevelScene.Level04;
		plusLevelInfo[7].mode = LevelMode.Find;
		plusLevelInfo[7].id = 0;
		plusLevelInfo[7].pass = false;
		plusLevelInfo[7].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[7].enemyKind = 1;
		plusLevelInfo[7].weekFluctuation = 0;
		plusLevelInfo[8] = new LevelInfo();
		plusLevelInfo[8].type = LevelType.Main;
		plusLevelInfo[8].scene = LevelScene.Level03;
		plusLevelInfo[8].mode = LevelMode.Survive;
		plusLevelInfo[8].id = -1;
		plusLevelInfo[8].pass = false;
		plusLevelInfo[8].textId = Random.Range(0, 10) % 5 + 1;
		plusLevelInfo[8].enemyKind = 1;
		plusLevelInfo[8].weekFluctuation = 0;
		LevelInfo[] array = new LevelInfo[4];
		if (levelInfo != null)
		{
			if (!levelInfo.pass)
			{
				LevelScene scene = levelInfo.scene;
			}
			else if (DataCenter.Save().GetWeek() == -1)
			{
				levelInfo = new LevelInfo();
				levelInfo.type = LevelType.Main;
				levelInfo.scene = LevelScene.Level03;
				levelInfo.mode = LevelMode.Find;
				levelInfo.id = 0;
				levelInfo.pass = false;
				levelInfo.textId = Random.Range(0, 10) % 5 + 1;
				levelInfo.enemyKind = 1;
				levelInfo.weekFluctuation = 0;
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 0)
			{
				levelInfo = new LevelInfo();
				levelInfo.type = LevelType.Main;
				levelInfo.scene = LevelScene.Level01;
				levelInfo.mode = LevelMode.Survive;
				levelInfo.id = 0;
				levelInfo.pass = false;
				levelInfo.textId = Random.Range(0, 10) % 5 + 1;
				levelInfo.enemyKind = 1;
				levelInfo.weekFluctuation = 0;
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 1)
			{
				levelInfo = plusLevelInfo[0];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 2)
			{
				levelInfo = plusLevelInfo[1];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 3)
			{
				levelInfo = plusLevelInfo[2];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 4)
			{
				levelInfo = plusLevelInfo[3];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 5)
			{
				levelInfo = plusLevelInfo[4];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 6)
			{
				levelInfo = plusLevelInfo[5];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 7)
			{
				levelInfo = plusLevelInfo[6];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 8)
			{
				levelInfo = plusLevelInfo[7];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else if (DataCenter.Save().GetWeek() == 9)
			{
				levelInfo = plusLevelInfo[8];
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
			else
			{
				LevelScene scene;
				do
				{
					scene = (LevelScene)(Random.Range(0, 12) % 4 + 1);
				}
				while (scene == levelInfo.scene);
				levelInfo = new LevelInfo();
				levelInfo.type = LevelType.Main;
				levelInfo.scene = scene;
				levelInfo.mode = (LevelMode)(Random.Range(0, 9) % 3);
				if (DataCenter.Save().GetWeek() <= 36)
				{
					levelInfo.id = (DataCenter.Save().GetWeek() - 1) / 12 * 2 + 1;
				}
				else
				{
					levelInfo.id = Random.Range(0, 10) % 5 + 1;
				}
				levelInfo.pass = false;
				levelInfo.textId = Random.Range(0, 10) % 5 + 1;
				levelInfo.enemyKind = Random.Range(0, 4) % 2 + 1;
				levelInfo.weekFluctuation = 0;
				DataCenter.Save().AddLevel(levelInfo);
				DataCenter.Save().Save();
			}
		}
		else if (DataCenter.Save().GetWeek() == -2)
		{
			levelInfo = new LevelInfo();
			levelInfo.type = LevelType.Main;
			levelInfo.scene = LevelScene.Level01;
			levelInfo.mode = LevelMode.Fight;
			levelInfo.id = 0;
			levelInfo.pass = false;
			levelInfo.textId = Random.Range(0, 10) % 5 + 1;
			levelInfo.enemyKind = 1;
			levelInfo.weekFluctuation = 0;
			DataCenter.Save().AddLevel(levelInfo);
			DataCenter.Save().Save();
		}
		array[(int)(levelInfo.scene - 1)] = levelInfo;
		if (DataCenter.Save().GetWeek() > 1 && DataCenter.Save().GetWeek() <= 9)
		{
			int num = 0;
			LevelInfo levelInfo2 = null;
			do
			{
				num = Random.Range(0, 100) % (DataCenter.Save().GetWeek() - 1) + 1;
				levelInfo2 = plusLevelInfo[num - 1];
			}
			while (levelInfo2.scene == levelInfo.scene);
			DataCenter.StateSingle().sideMissionWeek = num;
			array[(int)(levelInfo2.scene - 1)] = new LevelInfo();
			array[(int)(levelInfo2.scene - 1)].type = LevelType.Branch;
			array[(int)(levelInfo2.scene - 1)].scene = levelInfo2.scene;
			array[(int)(levelInfo2.scene - 1)].mode = levelInfo2.mode;
			array[(int)(levelInfo2.scene - 1)].id = levelInfo2.id;
			array[(int)(levelInfo2.scene - 1)].pass = levelInfo2.pass;
			array[(int)(levelInfo2.scene - 1)].textId = levelInfo2.textId;
			array[(int)(levelInfo2.scene - 1)].enemyKind = levelInfo2.enemyKind;
			array[(int)(levelInfo2.scene - 1)].weekFluctuation = levelInfo2.weekFluctuation;
		}
		else if (DataCenter.Save().GetWeek() > 9 && DataCenter.Save().GetWeek() <= 12)
		{
			LevelScene scene;
			do
			{
				scene = (LevelScene)(Random.Range(0, 12) % 4 + 1);
			}
			while (scene == levelInfo.scene);
			array[(int)(scene - 1)] = new LevelInfo();
			array[(int)(scene - 1)].type = LevelType.Branch;
			array[(int)(scene - 1)].scene = scene;
			array[(int)(scene - 1)].mode = (LevelMode)(Random.Range(0, 9) % 3);
			array[(int)(scene - 1)].id = Random.Range(0, 4) % 2 + (DataCenter.Save().GetWeek() - 1) / 12 * 2 + 1;
			array[(int)(scene - 1)].pass = false;
			array[(int)(scene - 1)].textId = Random.Range(0, 10) % 5 + 1;
			array[(int)(scene - 1)].enemyKind = Random.Range(0, 4) % 2 + 1;
			array[(int)(scene - 1)].weekFluctuation = Random.Range(-2, 3);
		}
		else if (DataCenter.Save().GetWeek() > 12 && DataCenter.Save().GetWeek() <= 24)
		{
			for (int i = 0; i < 2; i++)
			{
				LevelScene scene;
				do
				{
					scene = (LevelScene)(Random.Range(0, 12) % 4 + 1);
				}
				while (scene == levelInfo.scene);
				array[(int)(scene - 1)] = new LevelInfo();
				array[(int)(scene - 1)].type = LevelType.Branch;
				array[(int)(scene - 1)].scene = scene;
				array[(int)(scene - 1)].mode = (LevelMode)(Random.Range(0, 9) % 3);
				array[(int)(scene - 1)].id = Random.Range(0, 4) % 2 + (DataCenter.Save().GetWeek() - 1) / 12 * 2 + 1;
				array[(int)(scene - 1)].pass = false;
				array[(int)(scene - 1)].textId = Random.Range(0, 10) % 5 + 1;
				array[(int)(scene - 1)].enemyKind = Random.Range(0, 4) % 2 + 1;
				array[(int)(scene - 1)].weekFluctuation = Random.Range(-2, 3);
			}
		}
		else if (DataCenter.Save().GetWeek() > 1)
		{
			for (int j = 0; j < 3; j++)
			{
				LevelScene scene;
				do
				{
					scene = (LevelScene)(Random.Range(0, 12) % 4 + 1);
				}
				while (scene == levelInfo.scene);
				array[(int)(scene - 1)] = new LevelInfo();
				array[(int)(scene - 1)].type = LevelType.Branch;
				array[(int)(scene - 1)].scene = scene;
				array[(int)(scene - 1)].mode = (LevelMode)(Random.Range(0, 9) % 3);
				array[(int)(scene - 1)].id = Random.Range(0, 10) % 5 + 1;
				array[(int)(scene - 1)].pass = false;
				array[(int)(scene - 1)].textId = Random.Range(0, 10) % 5 + 1;
				array[(int)(scene - 1)].enemyKind = Random.Range(0, 4) % 2 + 1;
				array[(int)(scene - 1)].weekFluctuation = Random.Range(-2, 3);
			}
		}
		return array;
	}
}

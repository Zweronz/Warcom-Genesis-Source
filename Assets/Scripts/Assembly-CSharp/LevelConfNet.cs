using System.Collections.Generic;
using System.Xml;

public class LevelConfNet : LevelConf
{
	public class WaveNet
	{
		public int id;

		public int dropItemNum;

		public string[] dropItemName;

		public WaveNet(int id, int dropItemNum, string[] dropItemName)
		{
			this.id = id;
			this.dropItemNum = dropItemNum;
			this.dropItemName = dropItemName;
		}
	}

	public class ResurrectionPoint
	{
		public string point;

		public float direction;

		public ResurrectionPoint(string point, float direction)
		{
			this.point = point;
			this.direction = direction;
		}
	}

	public List<ResurrectionPoint> LoadResurrectionPoints(XmlElement resurrectionPoints)
	{
		List<ResurrectionPoint> list = new List<ResurrectionPoint>();
		foreach (XmlElement item2 in resurrectionPoints.GetElementsByTagName("ResurrectionPoint"))
		{
			ResurrectionPoint item = new ResurrectionPoint(item2.GetAttribute("Point"), float.Parse(item2.GetAttribute("Direction")));
			list.Add(item);
		}
		return list;
	}

	public List<string> LoadDropItemPoints(XmlElement dropItemPoints)
	{
		List<string> list = new List<string>();
		foreach (XmlElement item in dropItemPoints.GetElementsByTagName("DropItemPoint"))
		{
			list.Add(item.GetAttribute("Point"));
		}
		return list;
	}

	public float LoadDropItemRefreshTime(XmlElement dropItemRefreshTime)
	{
		return float.Parse(dropItemRefreshTime.GetAttribute("Time"));
	}

	public WaveNet LoadWaveNet(XmlElement wave)
	{
		int id = int.Parse(wave.GetAttribute("Id"));
		int dropItemNum = int.Parse(wave.GetAttribute("DropItemNum"));
		string[] dropItemName = wave.GetAttribute("MaybeDropItemName").Split('/');
		return new WaveNet(id, dropItemNum, dropItemName);
	}

	public List<WaveNet> LoatWaveNetList(XmlElement waves)
	{
		List<WaveNet> list = new List<WaveNet>();
		foreach (XmlElement item in waves.GetElementsByTagName("Wave"))
		{
			list.Add(LoadWaveNet(item));
		}
		return list;
	}
}

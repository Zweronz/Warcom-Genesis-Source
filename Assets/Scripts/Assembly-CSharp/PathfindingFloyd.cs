using System;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingFloyd : MonoBehaviour, PathfindingImpl
{
	[Serializable]
	public class DoorInfo
	{
		public string name;

		public int begin;

		public int end;
	}

	public float rayHeight = 0.5f;

	public LayerMask rayLayer;

	public int N;

	public Vector4[] waypoints;

	public float[] distances;

	public DoorInfo[] doorInfoArray;

	public int[] paths;

	public Vector4[] lastPath;

	public void Awake()
	{
		Pathfinding.Instance().SetImpl(this);
	}

	public void OnDestroy()
	{
		Pathfinding.Instance().SetImpl(null);
	}

	public Vector4[] FindPath(Vector3 begin, Vector3 end)
	{
		if (CanSee(begin, end))
		{
			lastPath = new Vector4[1];
			lastPath[0] = new Vector4(end.x, end.y, end.z, 0.5f);
			return lastPath;
		}
		float num = 100000000f;
		float num2 = 100000000f;
		int num3 = -1;
		int num4 = -1;
		for (int i = 0; i < N; i++)
		{
			Vector3 vector = new Vector3(waypoints[i].x, waypoints[i].y, waypoints[i].z);
			float sqrMagnitude = (vector - begin).sqrMagnitude;
			if (sqrMagnitude < num && CanSee(begin, vector))
			{
				num3 = i;
				num = sqrMagnitude;
			}
			sqrMagnitude = (vector - end).sqrMagnitude;
			if (sqrMagnitude < num2 && CanSee(end, vector))
			{
				num4 = i;
				num2 = sqrMagnitude;
			}
		}
		if (num3 < 0 || num4 < 0)
		{
			return null;
		}
		LinkedList<Vector4> linkedList = ReversePath(num4, num3);
		if (linkedList == null)
		{
			return null;
		}
		if (linkedList.Count >= 2)
		{
			Vector4 value = linkedList.First.Next.Value;
			if (CanSee(begin, new Vector3(value.x, value.y, value.z)))
			{
				linkedList.RemoveFirst();
			}
		}
		if (linkedList.Count >= 2)
		{
			Vector4 value2 = linkedList.Last.Previous.Value;
			if (CanSee(end, new Vector3(value2.x, value2.y, value2.z)))
			{
				linkedList.RemoveLast();
			}
		}
		lastPath = new Vector4[linkedList.Count + 1];
		linkedList.CopyTo(lastPath, 0);
		lastPath[linkedList.Count] = new Vector4(end.x, end.y, end.z, 0.5f);
		return lastPath;
	}

	public void CloseDoor(string[] doorNameArray)
	{
		foreach (string text in doorNameArray)
		{
			DoorInfo[] array = doorInfoArray;
			foreach (DoorInfo doorInfo in array)
			{
				if (text == doorInfo.name)
				{
					distances[doorInfo.begin * N + doorInfo.end] = -1f;
					distances[doorInfo.end * N + doorInfo.begin] = -1f;
				}
			}
		}
		CalcPath();
	}

	private bool CanSee(Vector3 p1, Vector3 p2)
	{
		Ray ray = new Ray(p1 + new Vector3(0f, rayHeight, 0f), p2 - p1);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, (p2 - p1).magnitude, rayLayer.value))
		{
			return false;
		}
		return true;
	}

	private LinkedList<Vector4> ReversePath(int begin, int end)
	{
		if (paths[begin * N + end] < 0)
		{
			return null;
		}
		LinkedList<Vector4> linkedList = new LinkedList<Vector4>();
		for (int num = end; num != begin; num = paths[begin * N + num])
		{
			linkedList.AddLast(waypoints[num]);
		}
		linkedList.AddLast(waypoints[begin]);
		return linkedList;
	}

	public void OnDrawGizmos()
	{
		if (lastPath == null)
		{
			return;
		}
		for (int i = 0; i < lastPath.Length; i++)
		{
			Vector3 vector = new Vector3(lastPath[i].x, lastPath[i].y, lastPath[i].z);
			float w = lastPath[i].w;
			if (i == 0)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(vector, w);
			}
			else if (i == lastPath.Length - 1)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(vector, w);
			}
			if (i < lastPath.Length - 1)
			{
				Vector3 to = new Vector3(lastPath[i + 1].x, lastPath[i + 1].y, lastPath[i + 1].z);
				Gizmos.color = Color.red;
				Gizmos.DrawLine(vector, to);
			}
		}
	}

	[ContextMenu("UpdatePathfindingData")]
	public void UpdatePathfindingData()
	{
		WayPoint[] componentsInChildren = base.gameObject.GetComponentsInChildren<WayPoint>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].index = i;
			componentsInChildren[i].gameObject.name = "WayPoint" + i;
		}
		N = componentsInChildren.Length;
		waypoints = new Vector4[N];
		for (int j = 0; j < N; j++)
		{
			Vector3 position = componentsInChildren[j].gameObject.transform.position;
			waypoints[j] = new Vector4(position.x, position.y, position.z, componentsInChildren[j].radius);
		}
		distances = new float[N * N];
		for (int k = 0; k < N; k++)
		{
			for (int l = 0; l < N; l++)
			{
				distances[k * N + l] = -1f;
			}
			distances[k * N + k] = 0f;
		}
		for (int m = 0; m < componentsInChildren.Length; m++)
		{
			for (int n = 0; n < componentsInChildren[m].edges.Length; n++)
			{
				float magnitude = (componentsInChildren[m].gameObject.transform.position - componentsInChildren[m].edges[n].gameObject.transform.position).magnitude;
				distances[componentsInChildren[m].index * N + componentsInChildren[m].edges[n].index] = magnitude;
				distances[componentsInChildren[m].edges[n].index * N + componentsInChildren[m].index] = magnitude;
			}
		}
		LinkedList<DoorInfo> linkedList = new LinkedList<DoorInfo>();
		for (int num = 0; num < N; num++)
		{
			for (int num2 = num + 1; num2 < N; num2++)
			{
				if (distances[num * N + num2] < 0f)
				{
					continue;
				}
				Vector3 position2 = componentsInChildren[num].gameObject.transform.position;
				Vector3 position3 = componentsInChildren[num2].gameObject.transform.position;
				Ray ray = new Ray(position2 + new Vector3(0f, rayHeight, 0f), position3 - position2);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, (position3 - position2).magnitude, rayLayer.value))
				{
					DoorInfo doorInfo = new DoorInfo();
					doorInfo.name = hitInfo.collider.gameObject.name;
					doorInfo.begin = num;
					doorInfo.end = num2;
					linkedList.AddFirst(doorInfo);
					if (!doorInfo.name.StartsWith("BLOCK_"))
					{
						Debug.Log(doorInfo.name);
						Debug.Log("There is other thing on the way,except Block!");
					}
				}
			}
		}
		doorInfoArray = new DoorInfo[linkedList.Count];
		linkedList.CopyTo(doorInfoArray, 0);
		CalcPath();
	}

	private void CalcPath()
	{
		float[] array = distances.Clone() as float[];
		paths = new int[N * N];
		for (int i = 0; i < N; i++)
		{
			for (int j = 0; j < N; j++)
			{
				paths[i * N + j] = i;
			}
		}
		for (int k = 0; k < N; k++)
		{
			for (int l = 0; l < N; l++)
			{
				for (int m = 0; m < N; m++)
				{
					if (array[l * N + k] >= 0f && array[k * N + m] >= 0f)
					{
						float num = array[l * N + k] + array[k * N + m];
						if (array[l * N + m] < 0f || array[l * N + m] > num)
						{
							paths[l * N + m] = paths[k * N + m];
							array[l * N + m] = num;
						}
					}
				}
			}
		}
		for (int n = 0; n < N; n++)
		{
			for (int num2 = 0; num2 < N; num2++)
			{
				if (array[n * N + num2] < 0f)
				{
					paths[n * N + num2] = -1;
				}
			}
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class EnemyAvatar : MonoBehaviour
{
	public void SetSkin(string skin)
	{
		if (skin.Length <= 0)
		{
			return;
		}
		string[] array = skin.Split('_');
		if (array.Length == 4)
		{
			Dictionary<string, SkinnedMeshRenderer> dictionary = new Dictionary<string, SkinnedMeshRenderer>();
			SkinnedMeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
			{
				dictionary.Add(skinnedMeshRenderer.name, skinnedMeshRenderer);
			}
			dictionary["Enemy_head"].material = PrefabCache.Load<Material>("Models/Characters/EnemyMaterials/" + array[0]);
			dictionary["Enemy_body"].material = PrefabCache.Load<Material>("Models/Characters/EnemyMaterials/" + array[1]);
			dictionary["Enemy_hand"].material = PrefabCache.Load<Material>("Models/Characters/EnemyMaterials/" + array[2]);
			dictionary["Enemy_coat"].material = PrefabCache.Load<Material>("Models/Characters/EnemyMaterials/" + array[3]);
		}
	}
}

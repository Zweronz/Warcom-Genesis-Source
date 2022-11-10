using UnityEngine;

public class CharacterBuilder
{
	public static Player CreatePlayer(CharacterInfo charactorInfo, Vector3 position, float direction)
	{
		Player player = new Player();
		player.Initialize(PrefabCache.Load<GameObject>("Models/Characters/" + charactorInfo.model), charactorInfo.model, position, direction);
		player.SetCharacterType((CharacterType)charactorInfo.id);
		player.SetModelName(charactorInfo.model);
		player.GetGameObject().AddComponent<CharacterPassivePara>();
		GameObject gameObject = new GameObject();
		gameObject.name = "PlayerPlane";
		gameObject.layer = 15;
		gameObject.transform.parent = player.GetTransform();
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.center = new Vector3(0.4f, 1.2f, 0.2f);
		boxCollider.size = new Vector3(1.3f, 2.4f, 0.1f);
		player.m_characterEffectBloodEmitPool = new CharacterEffectBloodEmitPool(player.GetGameObject());
		player.m_characterEffectItemEmit = new CharacterEffectItemEmit(player.GetGameObject());
		float num = 1f;
		int num2 = Mathf.Max(DataCenter.Save().GetWeek(), 0);
		num = ((num2 > 3) ? Mathf.Max(2f - (float)(num2 - 1) * 0.33f, 1f) : 2f);
		player.m_HPMax = (float)charactorInfo.hp * num;
		player.m_HPCurrent = player.m_HPMax;
		player.m_HPRecoverSpeed = (float)charactorInfo.hpRecoverSpeed * num;
		player.m_moveSpeed = charactorInfo.speed;
		player.GetGameObject().AddComponent<PlayerAnimEvent>();
		return player;
	}

	public static Enemy CreateEnemy(string model, Vector3 position, float direction)
	{
		Enemy enemy = new Enemy();
		enemy.Initialize(PrefabCache.Load<GameObject>("Models/Characters/" + model), "Enemy", position, direction);
		enemy.SetModelName(model);
		enemy.m_characterEffectBloodEmitPool = new CharacterEffectBloodEmitPool(enemy.GetGameObject());
		if (model.Contains("01"))
		{
			enemy.SetCharacterType(CharacterType.Scout);
			enemy.m_agent.speed = 4f;
		}
		else if (model.Contains("02"))
		{
			enemy.SetCharacterType(CharacterType.Sniper);
			enemy.m_agent.speed = 3.5f;
		}
		else if (model.Contains("03"))
		{
			enemy.SetCharacterType(CharacterType.Raider);
			enemy.m_agent.speed = 4.5f;
		}
		else if (model.Contains("04"))
		{
			enemy.SetCharacterType(CharacterType.Commando);
			enemy.m_agent.speed = 3f;
		}
		enemy.m_agent.angularSpeed = 360f;
		enemy.GetGameObject().AddComponent<EnemyAnimEvent>();
		return enemy;
	}

	public static PlayerNet CreatePlayerNet(CharacterInfo charactorInfo, string name, Vector3 position, float direction)
	{
		PlayerNet playerNet = new PlayerNet();
		playerNet.Initialize(PrefabCache.Load<GameObject>("Models/Characters/" + charactorInfo.model), name, position, direction);
		playerNet.SetCharacterType((CharacterType)charactorInfo.id);
		playerNet.SetModelName(charactorInfo.model);
		playerNet.GetGameObject().AddComponent<CharacterPassivePara>();
		GameObject gameObject = new GameObject();
		gameObject.name = "PlayerPlane";
		gameObject.layer = 15;
		gameObject.transform.parent = playerNet.GetTransform();
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.center = new Vector3(0.4f, 1.2f, 0.2f);
		boxCollider.size = new Vector3(1.3f, 2.4f, 0.1f);
		playerNet.m_characterEffectBloodEmitPool = new CharacterEffectBloodEmitPool(playerNet.GetGameObject());
		playerNet.m_characterEffectItemEmit = new CharacterEffectItemEmit(playerNet.GetGameObject());
		playerNet.m_HPMax = charactorInfo.hp;
		playerNet.m_HPCurrent = playerNet.m_HPMax;
		playerNet.m_HPRecoverSpeed = charactorInfo.hpRecoverSpeed;
		playerNet.m_moveSpeed = charactorInfo.speed;
		playerNet.GetGameObject().AddComponent<PlayerAnimEvent>();
		return playerNet;
	}

	public static EnemyNet CreateEnemyNet(CharacterInfo charactorInfo, string model, string name, int seatIndex, int id, Vector3 position, float direction)
	{
		EnemyNet enemyNet = new EnemyNet();
		enemyNet.Initialize(PrefabCache.Load<GameObject>("Models/Characters/" + model), name, position, direction);
		enemyNet.SetCharacterType((CharacterType)charactorInfo.id);
		enemyNet.SetModelName(model);
		enemyNet.GetGameObject().AddComponent<CharacterPassivePara>();
		enemyNet.m_seatIndex = seatIndex;
		enemyNet.m_netId = id;
		enemyNet.m_characterEffectBloodEmitPool = new CharacterEffectBloodEmitPool(enemyNet.GetGameObject());
		enemyNet.m_characterEffectItemEmit = new CharacterEffectItemEmit(enemyNet.GetGameObject());
		enemyNet.m_HPMax = charactorInfo.hp;
		enemyNet.m_HPCurrent = enemyNet.m_HPMax;
		enemyNet.m_HPRecoverSpeed = charactorInfo.hpRecoverSpeed;
		enemyNet.m_moveSpeed = charactorInfo.speed;
		enemyNet.GetGameObject().AddComponent<EnemyAnimEvent>();
		return enemyNet;
	}
}

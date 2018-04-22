﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PacketHandler {

	public static void HandleJoinResponse(string packet)
	{
		JoinResponse json = JsonUtility.FromJson<JoinResponse>(packet);
		Debug.Log(json.response);
	}
	public static void HandleGameStart(string packet)
	{
		GameStart json = JsonUtility.FromJson<GameStart>(packet);
		PersistentSettings.gameKey = json.gameKey;
		SceneManager.ins.StartMatch(json.mapID);
	}
	public static void HandleEndTurn(string packet)
	{
		EndTurn json = JsonUtility.FromJson<EndTurn>(packet);
		MapManager.ins.RemoveAllUnits();
		GameScene.ins.NextTurn();
		GameScene.ins.gameUI.UnpauseTransition();
		foreach(UnitStats u in json.units)
		{
			var gpos = u.position.ToVector2();
			GameScene.ins.SpawnUnit((UnitType)u.unitType, (Team)u.team, gpos);
			MapManager.ins.unitGrid[gpos].health = u.health;
		}
		if(!GameScene.ins.CheckForWin((Team)GameScene.ins.clientTeam)) {
			GameScene.ins.CheckForWin(GameScene.ins.OtherTeam());
		}
	}
	public static void HandleEndGame()
	{
		SceneManager.ins.ToTitle();
	}
	public static void HandleGameKey (string packet)
	{
		GameKey json = JsonUtility.FromJson<GameKey>(packet);
		PersistentSettings.gameKey = json.gameKey;
		Debug.Log(PersistentSettings.gameKey);
	}
}
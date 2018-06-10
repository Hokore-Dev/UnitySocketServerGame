using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour 
{
	private Dictionary<string, GameObject> _userObjects = new Dictionary<string, GameObject>();
	
	public void Init(Dictionary<string, ServerModel.User> inUserDic, ServerModel.User inCurrentUser)
	{
		for (int i = 0; i < inUserDic.Count; i++)
		{
			ServerModel.User data = inUserDic.ElementAt(i).Value;
			GenerateUser(data, data == inCurrentUser);
		}
		RegistEvent();
	}

	private void GenerateUser(ServerModel.User inData, bool isPlayer = false)
	{
		Character character = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Character")).GetComponent<Character>();
		character.transform.SetParent(this.transform);
		character.Init(inData, isPlayer);
			
		_userObjects.Add(inData.name, character.gameObject);
	}

	private void RegistEvent()
	{
		NetworkManager.it.AddEventCallback(ServerMethod.OTHER_PLAYER_UPDATE, (data) =>
		{
			var user = JsonUtility.FromJson<ServerModel.User>(data);
			if (_userObjects.ContainsKey(user.name))
			{
				_userObjects[user.name].transform.localPosition = user.position;
			}
		});
		
		NetworkManager.it.AddEventCallback(ServerMethod.OTHER_USER_DISCONNECT, (data) =>
		{
			var user = JsonUtility.FromJson<ServerModel.User>(data);
			if (_userObjects.ContainsKey(user.name))
			{
				Destroy(_userObjects[user.name]);
				_userObjects.Remove(user.name);
			}
		});
		
		NetworkManager.it.AddEventCallback(ServerMethod.OTHER_USER_CONNECT, (data) =>
		{
			var user = JsonUtility.FromJson<ServerModel.User>(data);
			if (!_userObjects.ContainsKey(user.name))
			{
				GenerateUser(user);
			}
		});
	}
}
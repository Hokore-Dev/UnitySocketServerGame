using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	[SerializeField] private Text _txtName;
	
	private ServerModel.User _userData;
	private bool _player;
	private Vector2 _lastPosition;

	public void Init(ServerModel.User inUserData, bool inPlayer = false)
	{
		_userData = inUserData;
		_player   = inPlayer;
		_txtName.text = _userData.name;

		this.name = _userData.name;
		this.transform.localPosition = _userData.position;

		if (_player)
		{
			Camera.main.transform.SetParent(this.transform);
			Camera.main.transform.localPosition = new Vector3(0,0,-10);
		}
	}
	
	void FixedUpdate () 
	{
		if (_player)
		{
			float horizon = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			
			transform.localPosition = new Vector2(
				transform.localPosition.x + horizon  * 5,
				transform.localPosition.y + vertical * 5);
			
			if (_lastPosition != (Vector2)this.transform.localPosition)
			{
				_userData.position = this.transform.localPosition;
				NetworkManager.it.Emit(ServerMethod.PLAYER_UPDATE, _userData.ToJSON());
			}
			_lastPosition = this.transform.localPosition;
		}
	}
}

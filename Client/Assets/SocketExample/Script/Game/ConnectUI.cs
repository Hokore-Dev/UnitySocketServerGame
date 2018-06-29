using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System.Collections.Generic;

public class ConnectUI : MonoBehaviour
{
    [SerializeField] private InputField _userName;
    [SerializeField] private Game _game;

    private Dictionary<string, ServerModel.User> _userDic = new Dictionary<string, ServerModel.User>();
    private ServerModel.User _currentUser;
    
    private void Start()
    {
        SocketIO.SocketIOComponent.Instance.On(ServerMethod.CONNECT, (data) =>
        {
            Debug.Log("Connect Socket Server");
        });

        SocketIO.SocketIOComponent.Instance.On(ServerMethod.OTHER_USER_CONNECT,
            (data) =>
            {
                var user    = JsonUtility.FromJson<ServerModel.User>(data.data.ToString());
                if (!_userDic.ContainsKey(user.name))
                {
                    _userDic.Add(user.name, user);
                }
                if (user.name == _userName.text)
                {
                    _game.Init(_userDic, _currentUser);
                    this.gameObject.SetActive(false);
                }
            });
    }

    public void OnGameStart()
    {
        string name = _userName.text;
        if (name.IsNullOrEmpty())
            return;

        _currentUser = new ServerModel.User()
        {
            name = name,
            position = new Vector2(Random.Range(0, 1280) - 640, Random.Range(0, 720) - 360)
        };
        _userDic.Add(_currentUser.name, _currentUser);
        SocketIO.SocketIOComponent.Instance.Emit(ServerMethod.USER_CONNECT, _currentUser.ToJSON());
    }
}

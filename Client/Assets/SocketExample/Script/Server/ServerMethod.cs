using System.Collections.Generic;

public class ServerMethod
{
    public static readonly string CONNECT               = "connect";
    public static readonly string DISCONNECT            = "disconnect";
    public static readonly string USER_CONNECT          = "user_connect";
    public static readonly string PLAYER_UPDATE         = "player_update";
    public static readonly string OTHER_PLAYER_UPDATE   = "other_player_update";
    public static readonly string OTHER_USER_CONNECT    = "other_user_connect";
    public static readonly string OTHER_USER_DISCONNECT = "other_user_disconnect";

    public static List<string> GetConnectMethod()
    {
        return new List<string>()
        {
            CONNECT,
            DISCONNECT,
            USER_CONNECT,
            PLAYER_UPDATE,
            OTHER_PLAYER_UPDATE,
            OTHER_USER_CONNECT,
            OTHER_USER_DISCONNECT
        };
    }
}
using UnityEngine;
using System;

namespace ServerModel
{
    [Serializable]
    public class User
    {
        public string name;
        public Vector2 position;

        public JSONObject ToJSON()
        {
            return new JSONObject(JsonUtility.ToJson(this));
        }
    }
}
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity.Network
{
    public class SocketInterface /*: MonoBehaviour*/
    {
        public virtual void Connect() { }
        public virtual void Logout() { }
        public virtual void Close() { }
        public virtual void Emit(int type, JSONObject content) { }
    }
}
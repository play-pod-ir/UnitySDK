using System;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity.Base
{
    public class AsyncResponse
    {
        private readonly JSONObject _sendData = new JSONObject();
        private readonly Network.Network _network;
        private readonly bool _canSend;

        public AsyncResponse(JSONObject message, Network.Network network)
        {
            try
            {
                var type = message["type"].AsInt;
                if (type == 4 || type == 5)
                {
                    var receivers = new JSONArray();
                    receivers.Add(message["senderId"].AsInt);
                    _sendData.Add("receivers", receivers);
                    _sendData.Add("messageId", message["id"].AsInt);
                    _canSend = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            this._network = network;
        }

        public void Call()
        {
            if (_canSend)
            {
                try
                {
                    _network.PushSendMessage(6, _sendData);
                }
                catch (ServiceException e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }
            }
        }
    }
}
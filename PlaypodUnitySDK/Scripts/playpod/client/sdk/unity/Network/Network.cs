using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using playpod.client.sdk.unity.Base;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace playpod.client.sdk.unity.Network
{
    public class Network /*: MonoBehaviour*/
    {
        public JSONObject dic;
        public string lang;
        public string appId;
        public string deviceId;

        private MonoBehaviour _mono;
        private StateMachine _stateMachine;

        private SocketInterface _socket;

        private IParent _parent;
        private bool _connectionState;
        private bool _isSocketOpen;
        private bool _isDeviceRegisterInPush;
        private bool _isServerRegisterInPush;
        private readonly string _chatServerName = string.Empty;
        private string _gameCenterName;
        private long _peerId;

        private int _reconnectTimeout;
        private int _pushMessageTtl;

        private int _activePeerRetryCount;
        private int _lastMessageId;

        // ReSharper disable once CollectionNeverUpdated.Local
        private Dictionary<string, Action<JSONObject>> _ackCallback;

        private Dictionary<string, Dictionary<string, EventCallback>> _eventCallback;

        private Dictionary<string, Dictionary<string, Action<JSONObject>>> _onceEventCallback;

        private List<JSONObject> _pushSendDataQueue;

        private string _registerDeviceInPushTimeoutId;
        private string _registerPeerInGameCenterTimeoutId;
        private string _registerServerInPushTimeoutId;

        private string _activePeerInGameCenterTimeoutId;
        //static Logger log = Logger.getLogger(Network.class);

        //private int tps = 3;

        //private int currentThroughput = 0;
        //private long currentThrouputSec = (long)Time.time;
        //private List<Dictionary<string, object>> sendMessageQueue = new List<Dictionary<string, object>>();

        //private static ThreadPoolExecutor executor;

        public bool GetSocketConnectionState()
        {
            return _connectionState;
        }

        public void Initialize(IParent parent, MonoBehaviour mono, StateMachine stateMachine, string newAppId, string newDeviceId, JSONObject newDic, string newLang)
        {
//            Debug.Log("Network.initialize");

            if (_mono == null && mono != null)
            {
                _mono = mono;
            }

            if (_stateMachine == null && stateMachine != null)
            {
                _stateMachine = stateMachine;
            }
            
            if (_eventCallback == null)
            {
                _eventCallback = new Dictionary<string, Dictionary<string, EventCallback>>();
            }

            _eventCallback.Add("connect", new Dictionary<string, EventCallback>());
            _eventCallback.Add("disconnect", new Dictionary<string, EventCallback>());
            _eventCallback.Add("reconnect", new Dictionary<string, EventCallback>());
            _eventCallback.Add("message", new Dictionary<string, EventCallback>());

            if (_onceEventCallback == null)
            {
                _onceEventCallback = new Dictionary<string, Dictionary<string, Action<JSONObject>>>();
            }

            _onceEventCallback.Add("open", new Dictionary<string, Action<JSONObject>>());


            //        this.parent = parent;
            this._parent = parent;
            this.appId = newAppId;
            this.deviceId = newDeviceId;
            this.dic = newDic;
            this.lang = newLang;

            _ackCallback = new Dictionary<string, Action<JSONObject>>();
            _pushSendDataQueue = new List<JSONObject>();

            //        int cores = Runtime.getRuntime().availableProcessors();
            //        if (cores > 1) {
            //            cores = cores - 1;
            //        }
            //
            //        executor = (ThreadPoolExecutor) Executors.newFixedThreadPool(cores);

            
        }

        public void Init(bool isForChat)
        {
            //Debug.Log("Network.init(): isForChat: " + isForChat);
            _reconnectTimeout = ConfigData.Wsto;
            _pushMessageTtl = ConfigData.Pmttl;

            if (isForChat)
            {
                if (ConfigData.Utc)
                {
                    InitTcpSocket(ConfigData.Csat);
                }
                else
                {
                    InitWebSocket(ConfigData.Csa);
                }
            }
            else
            {
                if (ConfigData.Utc)
                {
                    InitTcpSocket(ConfigData.Psat);
                }
                else
                {
                    InitWebSocket(ConfigData.Psa);
                }
            }
        }

        private void InitTcpSocket(string socketServerAddress)
        {
            // todo: implement later
            Debug.Log("initTCPSocket " + socketServerAddress + appId + " ");
            //        socket = new TCPSocketHandler(socketServerAddress) {
            //
            //            @Override
            //            public void onOpen(SocketInterface socketInterface) {
            //            try {
            //            onSocketOpen(socketInterface);
            //        } catch (Exception e) {
            //            e.printStackTrace();
            //        }
            //        }
            //
            //        @Override
            //        public void onMessage(JSONObject message) {
            //            try {
            //                onSocketMessage(message);
            //            } catch (ServiceException e) {
            //                e.printStackTrace();
            //            }
            //
            //        }
            //
            //        @Override
            //        public void onClose(int errorCode) {
            //            onSocketClose(errorCode);
            //        }
            //        };
        }

        private void InitWebSocket(string socketServerAddress)
        {
//            Debug.Log("initWebSocket " + socketServerAddress + appId + " ");

            _socket = new WebSocketHandler(socketServerAddress, _stateMachine);

            ((WebSocketHandler) _socket).InitWebSocket(socketServerAddress, this, _stateMachine,

                // onMessage
                delegate(JSONObject message) { OnSocketMessage(message); },

                // onOpen
                delegate(SocketInterface socketInterface) { OnSocketOpen(socketInterface); },

                // onClose
                delegate(int errorCode) { OnSocketClose(errorCode); });
        }

        public void OnSocketOpen(SocketInterface socketInterface)
        {
//            Debug.Log("initWebSocket_open " + appId + " ");
            _isSocketOpen = true;
            _socket = socketInterface;
            RegisterDeviceInPush(false);
            var data = new JSONObject();
            FireOnceEvents("open", data);
        }

        private void RegisterDeviceInPush(bool isRetry)
        {
            //Debug.Log("registerDeviceInPush " + appId + " " + isDeviceRegisterInPush);
            _isDeviceRegisterInPush = false;
            var content = new JSONObject();
            content.Add("appId", appId);
            content.Add("deviceId", deviceId);

            if (_peerId != default(long))
            {
                content.Add("peerId", _peerId);
                content.Add("refresh", true);
            }
            else
            {
                if (!isRetry)
                {
                    content.Add("renew", true);
                }
            }

            _socket.Emit(2, content);
        }

        private void FireOnceEvents(string categoryName, JSONObject data)
        {
            var events = _onceEventCallback[categoryName];
            if (events != null)
            {
                foreach (var item in events.Keys)
                {
                    var onFire = events[item];

                    Debug.Log("on fire for " + item);
                    onFire(data);

                    events.Remove(item);
                }
            }
        }

        public void OnSocketClose(int errorCode)
        {
//            Debug.Log("initWebSocket_close " + appId + " " + errorCode);
            _isSocketOpen = false;
            _isDeviceRegisterInPush = false;
            if (_connectionState)
            {
                _connectionState = false;
                FireEvents("disconnect", new JSONObject());
            }

            ClearAllTimeout();

            var timeout = (errorCode == 4002 || errorCode == 1005) ? 1000 : _reconnectTimeout;

            Util.SetTimeout(() => { _socket.Connect(); }, timeout);
        }

        private void FireEvents(string categoryName, JSONObject data)
        {
            var events = _eventCallback[categoryName];
            if (events != null)
            {
                foreach (var item in events.Keys)
                {
                    var callback = events[item];

                    callback.OnFire(data);
                }
            }
        }

        private void FireEvents(String categoryName, JSONObject msg, AsyncResponse res)
        {
            var events = _eventCallback[categoryName];
            if (events != null)
            {
                foreach (var item in events.Keys)
                {
                    var callback = events[item];
                    callback.OnFire(msg, res);
                }
            }
        }

        public void OnSocketMessage(JSONObject message)
        {
//            Debug.Log("Network.onSocketMessage\n\nmessage: " + Service.PrettyJson(message));
            var res = new AsyncResponse(message, this);
            Action<JSONObject> onResult = null;
            try
            {
                var type = message["type"].AsInt;
                string senderMessageId = null;

                if (message.HasKeyNotNull("senderMessageId"))
                {
                    senderMessageId = message["senderMessageId"].ToString();
                }

                switch (type)
                {
                    case 1:

                        if (message.HasKeyNotNull("senderName") &&
                            message["senderName"].ToString().Equals(_chatServerName))
                        {
                            Debug.LogError("fireEvents with AsyncResponse not implemented yet");
                            FireEvents("message", message, res);
                        }
                        else
                        {
                            ActivePeerInGameCenter();
                        }

                        break;

                    case 2:
                        string content = message["content"];
                        HandleDeviceRegisterMessage(long.Parse(content));
                        break;

                    case 3:
                        if (senderMessageId != null)
                        {
                            onResult = _ackCallback[senderMessageId];
                        }

                        if (senderMessageId != null && onResult != null)
                        {
                            onResult(JSON.Parse(message["content"].ToString()).AsObject);
                            _ackCallback.Remove(senderMessageId);
                        }
                        else
                        {
                            FireEvents("message", message);
                        }

                        break;

                    case 4:
                    case 5:
                        //Debug.LogError("fireEvents with AsyncResponse not implemented yet");
                        FireEvents("message", message, res);
                        break;


                    case 6:

                        if (senderMessageId != null)
                        {
                            onResult = _ackCallback[senderMessageId];
                            if (onResult != null)
                            {
                                onResult(Util.CreateReturnData(false, "", 0, new JSONObject()));
                                _ackCallback.Remove(senderMessageId);
                            }
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }
        
        public void OnLogin(JSONObject userData) {

        }

        private void HandleDeviceRegisterMessage(long peerId)
        {
            //Debug.Log("handleDeviceRegisterMessage " + appId + " " + this.peerId + " " + peerId + " " + gameCenterName + " " + isServerRegisterInPush);
            if (_isDeviceRegisterInPush)
            {
                return;
            }

            if (_registerDeviceInPushTimeoutId != null)
            {
                Util.ClearTimeout(_registerDeviceInPushTimeoutId);
                _registerDeviceInPushTimeoutId = null;
            }

            _isDeviceRegisterInPush = true;

            if (_gameCenterName != null)
            {
                if (this._peerId == default(long) || !this._peerId.Equals(peerId))
                {
                    this._peerId = peerId;
                    RegisterPeerInGameCenter();
                }
                else
                {
                    if (_isServerRegisterInPush)
                    {
                        _connectionState = true;
                        pushSendDataQueueHandler();
                        var data = new JSONObject();
                        data.Add("peerId", peerId);
                        FireEvents("reconnect", data);
                    }
                    else
                    {
                        RegisterServerInPush();
                    }
                }
            }
            else
            {
                this._peerId = peerId;
                RegisterPeerInGameCenter();
            }
        }

        private void RegisterServerInPush()
        {
            //Debug.Log("registerServerInPush: gameCenterName: " + gameCenterName);
            var content = new JSONObject();
            content.Add("name", _gameCenterName);
            _socket.Emit(1, content);

            _registerServerInPushTimeoutId = Util.SetTimeout(() =>
            {
                if (!_isServerRegisterInPush)
                {
                    try
                    {
                        RegisterServerInPush();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                    }
                }
            }, ConfigData.Pcrit);
        }

        private void pushSendDataQueueHandler()
        {
            //Debug.Log("pushSendDataQueueHandler");
            try
            {
                foreach (var data in _pushSendDataQueue)
                {
                    PushSendMessage(data["type"].AsInt, data["content"].AsObject);
                }

                _pushSendDataQueue.Clear();
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        public void PushSendMessage(int type, JSONObject content)
        {
            if (_connectionState)
            {
                _socket.Emit(type, content);
            }
            else
            {
                var data = new JSONObject();
                try
                {
                    data.Add("type", type);
                    data.Add("content", content);
                    _pushSendDataQueue.Add(data);
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                    //throw new ServiceException(e);
                }
            }
        }

        private void RegisterPeerInGameCenter()
        {
            //Debug.Log("registerPeerInGameCenter_0 " + appId + " " + peerId);

            _isServerRegisterInPush = false;


            //Debug.Log("parent.registerPeerId");
            _parent.RegisterPeerId(_peerId, res =>
            {
//                Debug.Log("parent.registerPeerId.onResult: res:\n" + Service.PrettyJson(res));
                try
                {
                    if (res["hasError"].AsBool)
                    {
                        _registerPeerInGameCenterTimeoutId =
                            Util.SetTimeout(() => { RegisterPeerInGameCenter(); }, 5000);
                    }
                    else
                    {
                        _gameCenterName = res["result"];
                        RegisterServerInPush();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }
            });
        }

        private void ActivePeerInGameCenter()
        {
            //Debug.Log("activePeerInGameCenter_0 " + appId + " " + peerId + " " + isServerRegisterInPush);
            if (_isServerRegisterInPush || _peerId == default(long))
            {
                return;
            }

            _isServerRegisterInPush = true;

            _parent.ActivatePeerId(_peerId, result =>
            {
                try
                {
                    if (result["hasError"].AsBool)
                    {
                        _isServerRegisterInPush = false;
                        _activePeerInGameCenterTimeoutId =
                            Util.SetTimeout(() => { ActivePeerInGameCenter(); }, ConfigData.Smit);
                    }
                    else
                    {
                        _connectionState = true;
                        pushSendDataQueueHandler();
                        var data = new JSONObject();
                        data.Add("peerId", _peerId);
                        if (_activePeerRetryCount == 0)
                        {
                            FireEvents("connect", data);
                        }
                        else
                        {
                            FireEvents("reconnect", data);
                        }

                        _activePeerRetryCount += 1;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }
            });
        }

        public void HttpRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            //        Debug.Log("Network.httpRequest");
            _mono.StartCoroutine(Request(Params, onResult));
        }

        public void PostRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            //Debug.Log("postRequest");
            _mono.StartCoroutine(Request(Params, onResult));
        }

        IEnumerator Request(JSONObject Params, Action<JSONObject> onResult)
        {
            //Debug.Log("Network.request");
            var returnData = new JSONObject();

            if (!Params.HasKey("url") || Params["url"] == null)
            {
                Debug.LogError("Exception: " + "URL cannot be null!");
                //throw new ServiceException("URL cannot be null!");
            }

            string url = Params["url"];

            string method;

            if (!Params.HasKey("method") || Params["method"] == null)
            {
                method = "GET";
            }
            else
            {
                method = ((string) Params["method"]).Equals("GET") ? "GET" : "POST";
            }

            var request = new UnityWebRequest(url)
            {
                method = method,
                timeout = ConfigData.Hrt / 1000,
                downloadHandler = new DownloadHandlerBuffer()
            };

            if (Params.HasKey("headers") && Params["headers"] != null)
            {
                var headers = Params["headers"].AsObject;

                if (headers.Count == 1 && headers.HasKey("Content-Type") && headers["Content-Type"] != null)
                {
                    request.SetRequestHeader("Content-Type", headers["Content-Type"]);
                }
            }


            if (Params.HasKey("data") && Params["data"] != null)
            {
                string data = Params["data"];

                var rawData = Encoding.UTF8.GetBytes(data);

                if (rawData.Length > 0)
                {
                    request.uploadHandler = new UploadHandlerRaw(rawData);
                }
            }

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                returnData.Add("HasError", true);
                returnData.Add("ErrorMessage", "خطایی در اجرای درخواست شما رخ داد!!!");
                returnData.Add("ErrorCode", ErrorCodes.Exception);
                returnData.Add("Result", null);

//                Debug.Log("NETWORK_RESPONSE ERROR: \n\nParams: " + Service.PrettyJson(Params) + "\nResponse Code: " +
//                          request.responseCode + "\n\nResponseBody: {}\n");
            }
            else
            {
                var response = JSON.Parse(request.downloadHandler.text).AsObject;

                returnData.Add("HasError", false);
                returnData.Add("ErrorMessage", "");
                returnData.Add("ErrorCode", 0);
                returnData.Add("Result", response);

                //Debug.Log("NETWORK_RESPONSE SUCCESS: \n\nParams: " + Service.PrettyJson(Params) + "\nResponse Code: " +
                //          request.responseCode + "\n\nResponseBody: " + Service.PrettyJson(JSON.Parse(request.downloadHandler.text).AsObject) + "\n");
            }

            onResult(returnData);

            request.Dispose();
        }

        public string On(string eventName, EventCallback callback)
        {
//            Debug.Log("Network.on...\n\neventName: " + eventName);
            try
            {
                var events = _eventCallback[eventName];
                if (events != null)
                {
                    var id = Guid.NewGuid().ToString();
                    events.Add(id, callback);

                    if (eventName.Equals("connect") && _connectionState)
                    {
                        Debug.Log("eventName.Equals(\"connect\")");
                        var data = new JSONObject();
                        data.Add("peerId", _peerId);
                        callback.OnFire(data);
                    }

                    if (eventName.Equals("open") && _isSocketOpen)
                    {
                        Debug.Log("eventName == open && isSocketOpen");
                        var data = new JSONObject();
                        callback.OnFire(data);
                    }

                    return id;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }

            return null;
        }

        public void OnLogout()
        {
            Debug.Log("LOGOUT " + appId);
            _peerId = default(long);
            _isServerRegisterInPush = false;
            _isDeviceRegisterInPush = false;
            _activePeerRetryCount = 0;
            Debug.Log("peerId: " + _peerId);
            Debug.Log("isServerRegisterInPush: " + _isServerRegisterInPush);
            Debug.Log("isDeviceRegisterInPush: " + _isDeviceRegisterInPush);
            Debug.Log("activePeerRetryCount: " + _activePeerRetryCount);
            ClearAllTimeout();

            //        if (socket != null) {
            //            socket.logout();
            //        }
        }

        private void ClearAllTimeout()
        {
            if (_registerDeviceInPushTimeoutId != null)
            {
                Util.ClearTimeout(_registerDeviceInPushTimeoutId);
                _registerDeviceInPushTimeoutId = null;
            }

            if (_registerServerInPushTimeoutId != null)
            {
                Util.ClearTimeout(_registerServerInPushTimeoutId);
                _registerServerInPushTimeoutId = null;
            }

            if (_activePeerInGameCenterTimeoutId != null)
            {
                Util.ClearTimeout(_activePeerInGameCenterTimeoutId);
                _activePeerInGameCenterTimeoutId = null;
            }

            if (_registerPeerInGameCenterTimeoutId != null)
            {
                Util.ClearTimeout(_registerPeerInGameCenterTimeoutId);
                _registerPeerInGameCenterTimeoutId = null;
            }
        }

        public bool IsSocketOpen()
        {
            return _isSocketOpen;
        }

        public void Emit(JSONObject Params, Action<JSONObject> onResult)
        {
            //        long sec = TimeUnit.MILLISECONDS.toSeconds(System.currentTimeMillis());
            //        if (sec != currentThrouputSec ) {
            //            currentThroughput = 1;
            //            currentThrouputSec = sec;
            //            _emit(params, callback);
            //        } else {
            //
            //            if (currentThroughput < tps) {
            //
            //            }
            //        }
            _emit(Params, onResult);
        }

        private void _emit(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var type = Params["type"].AsInt;
                JSONObject content;
                try
                {
                    content = Params["content"].AsObject;
                }
                catch (Exception)
                {
                    // content is string
                    content = JSON.Parse(Params["content"].ToString()).AsObject;
                }

                if (type == 4 || type == 5)
                {
                    _lastMessageId += 1;
                    string messageId = _lastMessageId.ToString();
                    _ackCallback.Add(messageId, onResult);

                    content.Add("ttl", _pushMessageTtl);
                    content.Add("messageId", _lastMessageId);

                    if (Params.HasKey("timeout") && Params["timeout"] != null)
                    {
                        Util.SetTimeout(() =>
                        {
                            Action<JSONObject> callback = _ackCallback[messageId];

                            if (callback != null)
                            {
                                callback(Util.CreateServerReturnData(true, "Request Timeout", ErrorCodes.Timeout,
                                    new JSONObject()));
                                _ackCallback.Remove(messageId);
                            }
                        }, Params["timeout"].AsLong);
                    }
                }

                PushSendMessage(type, content);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                throw new ServiceException(e);
            }
        }

        public interface IParent
        {
            void RegisterPeerId(long peerId, Action<JSONObject> onResult);
            void ActivatePeerId(long peerId, Action<JSONObject> onResult);
        }
    }
}
using System;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;
using WebSocketSharp;

namespace playpod.client.sdk.unity.Network
{
    public class WebSocketHandler : SocketInterface
    {
        private StateMachine _stateMachine;

        private WebSocketHandler _self;
        private string _pushServerAddress;
        private WebSocket _socket;
        private DateTime _lastMessageTime;
        private int _pcct;//push check connection timeout
        private int _pcctt;//webSocket check connection timeout threshold
        //private int WSCWTI;//webSocket connection wait time interval
        private int _pingTimeCheck;

        private string _lastPingTimeoutId;
        private string _lastMessageTimeoutId;

        public Network network { get; private set; }


        private Action<JSONObject> _onMessage;
        private Action<SocketInterface> _onOpen;
        private Action<int> _onClose;

        public WebSocketHandler(string serverAddress, StateMachine stateMachine)
        {
            _self = this;

            _stateMachine = stateMachine;
            _pushServerAddress = serverAddress;
            //psa = "wss://bus.fanapium.com/ws";
            _pcct = ConfigData.Pcct;
            _pcctt = ConfigData.Pcctt;
            //WSCWTI = ConfigData.WSCWTI;
            _pingTimeCheck = _pcct - _pcctt;
//            Connect();
        }

        public void InitWebSocket(string serverAddress, Network _network, StateMachine stateMachine, Action<JSONObject> onMessage, Action<SocketInterface> onOpen, Action<int> onClose)
        {
            _self = this;
            _pushServerAddress = serverAddress;
            //psa = "wss://bus.fanapium.com/ws";
            _pcct = ConfigData.Pcct;
            _pcctt = ConfigData.Pcctt;
            //WSCWTI = ConfigData.WSCWTI;
            _pingTimeCheck = _pcct - _pcctt;

            _stateMachine = stateMachine;
            network = _network;

            this._onMessage = onMessage;
            this._onOpen = onOpen;
            this._onClose = onClose;

            Connect();
        }

        private void Ping()
        {
            //Debug.Log("ping_0 " + pushServerAddress);
//            Debug.Log("Pinging...\n\n");
            Emit(0, null);

            _lastPingTimeoutId = Util.SetTimeout(() =>
            {
                Debug.Log("Ping timed out");

                var currentDate = DateTime.Now;

                var diff = currentDate.Subtract(_lastMessageTime);

                var difmillis = diff.Seconds * 1000 + diff.Milliseconds;

                Debug.Log("ping_1 " + (difmillis) + " " + _lastMessageTime + " " + (_pcct + _pcctt));
                if (difmillis > (_pcct + _pcctt))
                {
                    Debug.Log("CLOSE_BY_OWN "/* + socket.isOpen()*/);
                    _socket.CloseAsync();
                }
            }, _pcct);
        }

        public override void Connect()
        {
            //Debug.Log("INIT_SOCKET_CONNECTION " + pushServerAddress);

            _socket = new WebSocket(_pushServerAddress);

            _socket.OnOpen += OnOpenHandler;
            _socket.OnMessage += OnMessageHandler;
            _socket.OnClose += OnCloseHandler;

            _stateMachine.AddHandler(State.Connected, msg =>
            {
                OnOpen(_self);
            });

            _stateMachine.AddHandler(State.Pong, (msg) =>
            {
                _lastMessageTime = DateTime.Now;

                if (_lastMessageTimeoutId != null)
                {
                    Util.ClearTimeout(_lastMessageTimeoutId);
                }

                if (_lastPingTimeoutId != null)
                {
                    Util.ClearTimeout(_lastPingTimeoutId);
                }

                _lastMessageTimeoutId = Util.SetTimeout(() =>
                {
                    var currentDate = DateTime.Now;

                    var diff = currentDate.Subtract(_lastMessageTime);

                    var difmillis = diff.Seconds * 1000 + diff.Milliseconds;

                    if (difmillis >= _pingTimeCheck)
                    {
                        try
                        {
                            Ping();
                        }
                        catch (ServiceException ex)
                        {
                            Debug.LogError("Exception: " + ex.Message);
                        }
                    }
                    else
                    {
                        Debug.Log("else");
                    }
                }, _pcct);

                OnMessage(msg);

            });

            _stateMachine.AddHandler(State.Done, msg =>
            {
                Util.ClearTimeout(_lastMessageTimeoutId);
                Util.ClearTimeout(_lastPingTimeoutId);
                OnClose(4002);
            });

            _socket.ConnectAsync();
        }

        public override void Emit(int type, JSONObject content)
        {
//            Debug.Log("Socket emit: " + Service.PrettyJson(content));
            var data = new JSONObject();

            try
            {
                data.Add("type", type);
                if (content != null)
                {
                    data.Add("content", content.ToString());
                }

                if (_socket != null)
                {
//                    Debug.Log("Socket emit: " + Service.PrettyJson(data));
                    //Debug.Log("sending data to web socket...\n\ndata: \n" + SDKSetup.PrettyJson(data));
                    _socket.SendAsync(data.ToString(), isSuccess =>
                    {
                        if (type == 0)
                        {
//                            Debug.Log("Ping send status: " + (isSuccess ? "success" : "failed") + "\n\n");
                            return;
                        }
//                        Debug.Log("Message send status: " + (isSuccess ? "success" : "failed") + "\n\n");
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }

        }

        public override void Logout()
        {
            if (_lastMessageTimeoutId != null)
            {
                Util.ClearTimeout(_lastMessageTimeoutId);
                _lastMessageTimeoutId = null;
            }

            if (_lastPingTimeoutId != null)
            {
                Util.ClearTimeout(_lastPingTimeoutId);
                _lastPingTimeoutId = null;
            }

            if (_socket != null)
            {
                _socket.Close();
            }

        }

        public override void Close()
        {
            if (_lastMessageTimeoutId != null)
            {
                Util.ClearTimeout(_lastMessageTimeoutId);
                _lastMessageTimeoutId = null;
            }

            if (_lastPingTimeoutId != null)
            {
                Util.ClearTimeout(_lastPingTimeoutId);
                _lastPingTimeoutId = null;
            }
            if (_socket != null)
            {
                _socket.Close();
            }
        }

        public void OnMessage(JSONObject message)
        {
            _onMessage(message);
        }

        public void OnOpen(SocketInterface webSocket)
        {
            _onOpen(webSocket);
        }

        public void OnClose(int errorCode)
        {
            _onClose(errorCode);
        }

        private void OnOpenHandler(object sender, System.EventArgs e)
        {
//            Debug.Log("WebSocket connected...\n\n");
            _stateMachine.Transition(State.Connected, new JSONObject());
        }

        private void OnMessageHandler(object sender, MessageEventArgs e)
        {
            //Debug.Log("WebSocket server said: " + e.Data);

            if (e.IsText)
            {
                // Do something with e.Data.
                //Debug.Log("OnMessage: " + e.Data);

                //Debug.Log("SOCKET_MESSAGE_1 " + pushServerAddress + " " + e.Data);
                var msg = JSON.Parse(e.Data).AsObject;
                _stateMachine.Transition(State.Pong, msg);
                //try
                //{

                //}
                //catch (Exception ex)
                //{
                //    Debug.LogError("Exception: " + ex.Message);
                //    throw new ServiceException(ex);
                //}
                return;
            }

            if (e.IsBinary)
            {
                // Do something with e.RawData.
                Debug.Log("OnMessage: " + e.RawData);
                return;
            }

            if (e.IsPing)
            {
                // Do something to notify that a ping has been received.
                Debug.Log("OnMessage: a ping has been received");
                return;
            }
        }

        private void OnCloseHandler(object sender, CloseEventArgs e)
        {
            Debug.Log(string.Format("WebSocket closed with \n\ncode: {0}\n\nReason: {1}\n\n", e.Code, e.Reason));
            _stateMachine.Transition(State.Done, new JSONObject());
        }

    }
}

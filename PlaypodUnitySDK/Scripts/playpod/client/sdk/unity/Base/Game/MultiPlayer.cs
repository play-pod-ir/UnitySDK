using System;
using System.Collections.Generic;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity.Base.Game
{
    public class MultiPlayer
    {
        private Service _service;
        private Network.Network _network;
        private bool _isFinished;
        private bool _isRun;
        private bool _isStart;
        private bool _isResume;
        private bool _isEnd;
        //private bool isQuick = false;
        //private bool isReload = false;

        private string _matchId;
        private string _gameId;
        private string _leagueId;
        private string _opponentPeerId;
        private JSONObject _opponentsData;
        private JSONObject _ownData;
        //private DateTime startTime;
        private readonly Dictionary<string, Dictionary<string, object>> _sendDataAckState;
        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly Dictionary<string, bool> _receivedData;
        private List<Dictionary<string, object>> _receiveDataQueue;
//        private Logger _log = Logger.GetLogger(this.GetType().Name);

        public MultiPlayer
        (
            Action<JSONObject, AsyncResponse> onReceiveData,
            Action onConnect,
            Action onReconnect,
            Action onDisconnect,
            Action<JSONObject> onLeave,
            Action<JSONObject> onInit,
            Action onStart,
            Action onPause,
            Action onResume,
            Action<JSONObject> onEnd,
            Action<JSONObject> onReceiveDataAck,
            Action<JSONObject> onSentData
        )
        {
            this._onReceiveDataAction = onReceiveData;
            this._onConnectAction = onConnect;
            this._onReconnectAction = onReconnect;
            this._onDisconnectAction = onDisconnect;
            this._onLeaveAction = onLeave;
            this._onInitAction = onInit;
            this._onStartAction = onStart;
            this._onPauseAction = onPause;
            this._onResumeAction = onResume;
            this._onEndAction = onEnd;
            this._onReceiveDataAckAction = onReceiveDataAck;
            this._onSentDataAction = onSentData;
            
            _sendDataAckState = new Dictionary<string, Dictionary<string, object>>();
            _receivedData = new Dictionary<string, bool>();
            _receiveDataQueue = new List<Dictionary<string, object>>();
        }

        private readonly Action<JSONObject, AsyncResponse> _onReceiveDataAction;
        private readonly Action _onConnectAction;
        private readonly Action _onReconnectAction;
        private readonly Action _onDisconnectAction;
        private readonly Action<JSONObject> _onLeaveAction;
        private readonly Action<JSONObject> _onInitAction;
        private readonly Action _onStartAction;
        private readonly Action _onPauseAction;
        private readonly Action _onResumeAction;
        private readonly Action<JSONObject> _onEndAction;
        private readonly Action<JSONObject> _onReceiveDataAckAction;
        private readonly Action<JSONObject> _onSentDataAction;

        public void Initialize(Service service, Network.Network network, JSONObject matchData)
        {
            this._service = service;
            this._network = network;
            Init(matchData);
        }

        private void Init(JSONObject matchData)
        {
            try
            {
                _matchId = matchData["matchId"];
                _gameId = matchData["gameId"];
                _leagueId = matchData["leagueId"];

                if (matchData.HasKeyNotNull("isResume") && matchData["isResume"].AsBool)
                {
                    _isResume = true;
                    //isReload = true;
                }

                if (matchData.HasKeyNotNull("isQuick") && matchData["isQuick"].AsBool)
                {
                    //isQuick = true;
                }

                _ownData = matchData["ownData"].AsObject;
                _opponentsData = matchData["opponentsData"].AsArray[0].AsObject;
                _opponentPeerId = _opponentsData["peerId"];

                this.OnInit(matchData);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void CancelMatchRequest(int type, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();
                requestData.Add("matchId", _matchId);
                requestData.Add("type", type);

                _service.HttpPostRequest(RequestUrls.MatchCancel, requestData, result =>
                {
                    try
                    {
                        var hasError = result["hasError"].AsBool;

                        var errorCode = 0;
                        if (result.HasKeyNotNull("errorCode"))
                        {
                            errorCode = result["errorCode"].AsInt;
                        }

                        if (hasError && (errorCode == ErrorCodes.Runtime || errorCode == ErrorCodes.RequestFailed))
                        {
                            Util.SetTimeout(() =>
                            {
                                try
                                {
                                    CancelMatchRequest(type, onResult);
                                }
                                catch (ServiceException e)
                                {
                                    Debug.LogError("Exception: " + e.Message);
                                }
                            }, ConfigData.Smit);
                        }
                        else
                        {
                            var retData = new JSONObject();
                            retData.Add("hasError", hasError);
                            if (hasError)
                            {
                                retData.Add("errorMessage", errorCode);
                                retData.Add("errorCode", errorCode);
                            }

                            if (onResult != null)
                            {
                                onResult(retData);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        private void CancelMatch(bool sendCancelState, Action<JSONObject> onResult)
        {
            if (_isFinished)
            {
                return;
            }

            _isFinished = true;

            if (sendCancelState)
            {
                CancelMatchRequest(2, onResult);
            }
        }

        private void SendReady(Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();
                requestData.Add("userId", _ownData["id"]);
                requestData.Add("sessionId", _service.GetUserData()["peerId"]);
                requestData.Add("matchId", _matchId);
                //            requestData.put("ready", requestData.toString());
                _service.HttpPostRequest(RequestUrls.MatchReady, requestData, result =>
                {
                    try
                    {
                        var hasError = result["hasError"].AsBool;
                        var returnData = new JSONObject();
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["errorMessage"]);

                        if (hasError)
                        {
                            var errorCode = result["errorCode"].AsInt;

                            if (errorCode == ErrorCodes.Runtime || errorCode == ErrorCodes.RequestFailed)
                            {
                                Util.SetTimeout(() => { SendReady(onResult); }, ConfigData.Smit);
                            }
                            else
                            {
                                CancelMatch(false, null);
                                onResult(returnData);
                            }
                        }
                        else
                        {
                            onResult(returnData);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }


        /**
         * <div style='width: 100%;text-align: right'> ارسال داده های بازی</div>
         * @param Params
         *      <ul>
         *          <li>{JSONObject|string} sendData - داده ارسالی</li>
         *          <li>{string} [dataId] - شناسه داده</li>
         *      </ul>
         *
         * @param  callback
         * events : onReceive - بعد از دریافت پیام توسط حریف , این متد فراخوانی می شود
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * @return id شناسه داده ارسال شده
         * */
        public JSONObject SendData(JSONObject Params, Action<JSONObject> onReceive)
        {
            var retData = new JSONObject();
            Debug.Log("sendData_1");

            var dataId = (Params.HasKeyNotNull("dataId")) ? Params["dataId"].ToString() : Guid.NewGuid().ToString();
            //            bool sequential = (Params.has("dataId") && !Params.isNull("sequential")) && Params.getBoolean("sequential");
            string matchId = Params["matchId"];

            var receiversPeerIdPeerId = new JSONArray();
            receiversPeerIdPeerId.Add(_opponentPeerId);
            var ackObj = _sendDataAckState[dataId];
            Debug.Log("sendData_2");
            if (ackObj != null)
            {
                var state = (bool) ackObj["state"];
                Debug.Log("sendData_3 " + state);
                if (state)
                {
                    return null;
                }
            }
            else
            {
                Debug.Log("sendData_4");
                ackObj = new Dictionary<string, object> {{"state", false}, {"sendTryCount", 0}};

                if (onReceive != null)
                {
                    ackObj.Add("callback", onReceive);
                }

                _sendDataAckState.Add(dataId, ackObj);

                ackObj.Add("sendTryCount", (int) ackObj["sendTryCount"] + 1);

                var emitData = new JSONObject();
                emitData.Add("receivers", receiversPeerIdPeerId);

                var pushContent = new JSONObject();
                pushContent.Add("type", PushMessageContentTypes.DataPack);

                var gameContent = new JSONObject();
                gameContent.Add("dataId", dataId);
                gameContent.Add("matchId", matchId);
                gameContent.Add("data", Params["sendData"]);

                pushContent.Add("content", gameContent);

                emitData.Add("content", pushContent);

                var timeoutId = Util.SetTimeout(() =>
                {
                    var ackObjTemp = _sendDataAckState[dataId];
                    Debug.Log("sendData_6 ");
                    if (!(Boolean) ackObjTemp["state"])
                    {
                        Debug.Log("sendData_7 ");
                        if (_isFinished && (int) ackObjTemp["sendTryCount"] > ConfigData.Msdtc)
                        {
                            Debug.Log("sendData_8 ");
                            return;
                        }

                        try
                        {
                            Params.Add("resend", true);
                            SendData(Params, onReceive);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Exception: " + e.Message);
                        }
                    }
                }, ConfigData.Pmto);

                var reqData = new JSONObject();
                reqData.Add("type", 5);
                reqData.Add("content", emitData);
                Debug.Log("sendData_5 " + reqData);

                var t1 = DateTime.Now;
                _network.Emit(reqData, result =>
                {
                    try
                    {
                        var diff = DateTime.Now.Subtract(t1);
                        Debug.Log("sendData_9 " + (diff));
                        var ackObjTemp = _sendDataAckState[dataId];
                        if (!(bool) ackObjTemp["state"])
                        {
                            ackObjTemp.Add("state", true);

                            if (!_isFinished)
                            {
                                var recDataAck = new JSONObject();
                                recDataAck.Add("dataId", dataId);
                                if (Params.HasKeyNotNull("sequential"))
                                {
                                    recDataAck.Add("sequential", Params["sequential"]);
                                }

                                OnReceiveDataAck(recDataAck);
                            }

                            Util.ClearTimeout(timeoutId);

                            var retDataTemp = new JSONObject();
                            retDataTemp.Add("dataId", dataId);
                            Debug.Log("sendData_10 " + retDataTemp);
                            if (onReceive != null) onReceive(retDataTemp);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                    }
                });

                if (Params.HasKeyNotNull("resend") && !Params["resend"].AsBool)
                {
                    var sentData = new JSONObject();
                    sentData.Add("dataId", dataId);
                    if (Params.HasKeyNotNull("sequential"))
                    {
                        sentData.Add("sequential", Params["sequential"]);
                    }

                    var sequentialDataQueueLength = 0;
                    if (Params.HasKeyNotNull("sequentialDataQueueLength") &&
                        Params["sequentialDataQueueLength"].AsInt > 0)
                    {
                        sequentialDataQueueLength = Params["sequentialDataQueueLength"].AsInt;
                    }

                    sentData.Add("sequentialDataQueueLength", sequentialDataQueueLength);
                    OnSentData(sentData);
                }
            }

            retData.Add("dataId", dataId);
            return retData;
        }

        //    public void saveStateData(JSONObject Params) {
        //
        //    }
        //
        //    public void saveStaticData(JSONObject Params) {
        //
        //    }

        /**
         *
         * <div style='width: 100%;text-align: right'> اعلام نتیجه بازی</div>
         * @param Params
         *      <ul>
         *          <li>{JSONObject} result - هر یک از کلید های این آبجکت شناسه بازیکن می باشد و و مقدار آن نیز نتیجه بازیکن
         *              <ul>
         *                  <li>
         *                  {
         *                      5 : [
         *                          {
         *                              name : "field1",//امتیاز
         *                              value : 3
         *                          },
         *                          {
         *                              name : "field2",//
         *                              value : 1
         *                          }
         *                      ],
         *                      6 : [
         *                          {
         *                              name : "field1",//برد
         *                              value : 0
         *                          },
         *                          {
         *                              name : "field2",//باخت
         *                              value : 0
         *                          }
         *                      ]
         *                  }
         *                  </li>
         *              </ul>
         *          </li>
         *          <li> {Integer} [reasonCode]-  کد دلیل نتیجه</li>
         *      </ul>
         *
         * @param  res
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{Boolean} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void SendResult(JSONObject Params, Action<JSONObject> res)
        {
            _isRun = false;
            try
            {
                if (Params.HasKeyNotNull("result"))
                {
                    var gameResult = Params["result"].AsObject;

                    string ownId = this._ownData["id"];
                    string opponentId = _opponentsData["id"];

                    var resData = new JSONArray();

                    var ownData = new JSONObject();
                    ownData.Add("player1Id", ownId);
                    ownData.Add("scores", gameResult[ownId]);
                    resData.Add(ownData);

                    var opponentData = new JSONObject();
                    opponentData.Add("player1Id", opponentId);
                    opponentData.Add("scores", gameResult[opponentId]);
                    resData.Add(opponentData);


                    var result = resData.ToString();

                    var requestData = new JSONObject();
                    requestData.Add("matchResult", result);
                    requestData.Add("matchId", _matchId);
                    requestData.Add("gameId", _gameId);

                    _isEnd = true;
                    _service.SendMatchResultRequest(requestData, res);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        /**
         * اعلام آمادگی بازی برای شروع بازی
         * @param  res رخداد جواب درخواست
         * */
        public void Ready(Action<JSONObject> onResult)
        {
            if (_isResume)
            {
                this.OnStart();
                this.OnPause();
            }

            SendReady(onResult);
        }


        //    public void getSequentialDataQueue(JSONObject Params) {
        //
        //    }

        public bool IsFinished()
        {
            return _isFinished;
        }

        public bool IsStarted()
        {
            return _isStart;
        }

        public bool IsRun()
        {
            return _isRun;
        }

        /**
         * <div style='width: 100%;text-align: right'>اعلام کنسل شدن بازی</div>
         * @param  callback
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{Boolean} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *      </ul>
         *
         * @throws ServiceException خطای اجرای درخواست
         * */
        public void Cancel(Action<JSONObject> onResult)
        {
            CancelMatch(true, onResult);
        }

        /**
         * درخواست بازی دوباره با حریف
         * @param res رخداد جواب درخواست
         * */
        public void ReMatch(MatchRequestCallback res)
        {
            try
            {
                var requestData = new JSONObject();
                requestData.Add("opponentId", _opponentsData["id"]);
                requestData.Add("gameId", _gameId);
                requestData.Add("leagueId", _leagueId);
                _service.MatchRequest(requestData, res);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //e.printStackTrace();
            }

            //catch (ServiceException e)
            //{
            //    e.printStackTrace();
            //}
        }

        /**
         * دریافت امتیازات برتر بازیکنان و یا خود بازیکن
         *
         * @param  Params
         *      <ul>
         *          <li>{Boolean} [currentLeague] - تعیین دریافت بر اساس لیگی که کاربر مشغول بازی است</li>
         *          <li>{Boolean} [isGlobal] - برترین در بین کاربران و یا امتیازات خود کاربر</li>
         *      </ul>
         *
         * @param res رخداد جواب درخواست
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetTopScore(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();
                requestData.Add("gameId", _gameId);
                if (Params != null)
                {
                    if (Params.HasKeyNotNull("currentLeague") && Params["currentLeague"].AsBool)
                    {
                        requestData.Add("leagueId", _leagueId);
                    }

                    if (Params.HasKeyNotNull("isGlobal"))
                    {
                        requestData.Add("leagueId", Params["isGlobal"].AsBool);
                    }
                }

                _service.GetTopScore(requestData, onResult);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        /**
         * <div style='width: 100%;text-align: right'>اعلام ترک بازی</div>
         * @param  callback
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{Boolean} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *      </ul>
         *
         * @throws ServiceException خطای اجرای درخواست
         * */
        public void Leave(Action<JSONObject> onResult)
        {
            CancelMatchRequest(1, onResult);
        }

        public void _onReceiveData(JSONObject Params, AsyncResponse res)
        {
            try
            {
                if (_isStart && _isRun)
                {
                    if (_isEnd)
                    {
                        res.Call();
                    }
                    else
                    {
                        if (_receivedData.ContainsKey(Params["dataId"]))
                        {
                            res.Call();
                        }
                        else
                        {
                            OnReceiveData(Params, res);
                        }
                    }
                }
                else
                {
                    var queueData = new Dictionary<string, object> {{"Params", Params}, {"res", res}};
                    _receiveDataQueue.Add(queueData);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        public void _onLeave(JSONObject Params)
        {
            try
            {
                if (!_isFinished)
                {
                    if (Params.HasKeyNotNull("opponentSessionId"))
                    {
                        _opponentPeerId = Params["opponentSessionId"];
                    }

                    var data = new JSONObject();
                    data.Add("opponentPeerId", _opponentPeerId);
                    OnLeave(data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        public void _onStart(JSONObject Params)
        {
            try
            {
                _isStart = true;
                _isRun = true;
                //startTime = DateTime.Now;

                if (Params.HasKeyNotNull("opponentSessionId"))
                {
                    _opponentPeerId = Params["opponentSessionId"];
                }

                OnStart();

                if (_receiveDataQueue.Count > 0)
                {
                    // todo: handle this error later
                    
//                    for (var i = 0; i < _receiveDataQueue.Count; i++)
//                    {
//                        //AsyncResponse res = (AsyncResponse) Params["res"];
//                        //onReceiveData(receiveDataQueue[i], res);
//                    }

                    _receiveDataQueue = new List<Dictionary<string, object>>();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        public void _onPause(JSONObject Params)
        {
            try
            {
                if (!_isFinished)
                {
                    if (Params.HasKeyNotNull("opponentSessionId"))
                    {
                        _opponentPeerId = Params["opponentSessionId"];
                    }

                    _isRun = false;
                    OnPause();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        public void _onResume(JSONObject Params)
        {
            try
            {
                if (!_isFinished)
                {
                    if (Params.HasKey("opponentSessionId"))
                    {
                        _opponentPeerId = Params["opponentSessionId"];
                    }
                }

                OnResume();

                if (_receiveDataQueue.Count > 0)
                {
                    // todo: handle this error later
//                    for (var i = 0; i < _receiveDataQueue.Count; i++)
//                    {
//                        //AsyncResponse res = (AsyncResponse)Params["res"];
//                        //onReceiveData(receiveDataQueue[i], res);
//                    }

                    _receiveDataQueue = new List<Dictionary<string, object>>();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        public void _onEnd(JSONObject Params)
        {
            _isEnd = true;
            OnEnd(Params);
        }


        /**
         * <div style='width: 100%;text-align: right'> این متد بعد از دریافت پیامی جدید از سوی حریف , فراخوانی می شود</div>
         * @param  receivedData
         *      <ul>
         *          <li>{JSONObject|string} data - پیام ارسال شده از سوی حریف</li>
         *          <li>{JSONObject|string} dataId - شناسه پیام ارسال شده</li>
         *      </ul>
         *
         * @param  res کابکی که بعد از دریافت پیام جدید باید فراخوانی شود.با فرخوانی این متد فرد مقابل می فهمد که پیام به دست شما رسیده است
         * */
        private void OnReceiveData(JSONObject receivedData, AsyncResponse res)
        {
            _onReceiveDataAction(receivedData, res);
        }

        /**
         * <div style='width: 100%;text-align: right'> این متد بعد از اتصال به سرور ایسینک فراخوانی می شود</div>
         * */
        public void OnConnect()
        {
            _onConnectAction();
        }

        /**
         * <div style='width: 100%;text-align: right'>این متد بعد از اتصال مجدد به سرور ایسینک فراخوانی می شود</div>
         * */
        public void OnReconnect()
        {
            _onReconnectAction();
        }

        /**
         * <div style='width: 100%;text-align: right'> این متد بعد از قطع اتصال به سرور ایسینک فراخوانی می شود</div>
         * */
        public void OnDisconnect()
        {
            _onDisconnectAction();
        }

        /**
         * <div style='width: 100%;text-align: right'> این متد بعد از ترک بازی توسط حریف فراخوانی می شود</div>
         * @param  Params
         *      <ul>
         *          <li>{string} opponentId شناسه حریفی که بازی را ترک کرده</li>
         *      </ul>
         * */
        private void OnLeave(JSONObject Params)
        {
            _onLeaveAction(Params);
        }

        /**
         * <div style='width: 100%;text-align: right'>بعد از فراخوانی این متد می توانید به کلیه توابع دیگر که در این کلاس وجود دارد دسترسی داشته باشید</div>
         *  @param Params
         *      <ul>
         *          <li>{string} gameId - شناسه بازی</li>
         *          <li>{string} leagueId - شناسه لیگ</li>
         *          <li>{string} leagueName - نام لیگ</li>
         *          <li>{string} matchId - شناسه مسابقه</li>
         *          <li>{‌Boolean} isQuick - ‌تعیین اینکه مسابقه از طریق حریف می طلبم ایجاد شده یا خیر</li>
         *          <li>{JSONObject} ownData
         *              <ul>
         *                  <li>{string} name - نام کاربر</li>
         *                  <li>{string} id - شناسه کاربر</li>
         *                  <li>{Boolean} applicant - تعیین اینکه آیا درخواست دهنده بازی بوده یا خیر</li>
         *                  <li>{JSONObject} [image] - اطلاعات تصویر کاربر
         *                      <ul>
         *                          <li>{string} id - شناسه تصویر</li>
         *                          <li>{string} url - لینک تصویر</li>
         *                          <li>{Integer} width - اندازه رزولیشن افقی تصویر</li>
         *                          <li>{Integer} height - اندازه رزولیشن عمودی تصویر</li>
         *                      </ul>
         *                  </li>
         *              </ul>
         *          </li>
         *          <li>{JSONArray} opponentsData - اطلاعات حربف
         *               <ul>
         *                  <li>{string} name - نام کاربر</li>
         *                  <li>{string} id - شناسه کاربر</li>
         *                  <li>{Boolean} applicant - تعیین اینکه آیا درخواست دهنده بازی بوده یا خیر</li>
         *                  <li>{JSONObject} [image] - اطلاعات تصویر کاربر
         *                      <ul>
         *                          <li>{string} id - شناسه تصویر</li>
         *                          <li>{string} url - لینک تصویر</li>
         *                          <li>{Integer} width - اندازه رزولیشن افقی تصویر</li>
         *                          <li>{Integer} height - اندازه رزولیشن عمودی تصویر</li>
         *                      </ul>
         *                  </li>
         *               </ul>
         *          </li>
         *      </ul>
         * */
        private void OnInit(JSONObject Params)
        {
            _onInitAction(Params);
        }

        /**
         * <div style='width: 100%;text-align: right'> بعد از اعلام شروع بازی از سمت گیم سنتر این متد فراخوانی می شود</div>
         * */
        private void OnStart()
        {
            _onStartAction();
        }

        /**
         * <div style='width: 100%;text-align: right'>بعد از اعلام توقف بازی از سمت گیم سنتر این متد فراخوانی می شود</div>
         * */
        private void OnPause()
        {
            _onPauseAction();
        }

        /**
         * <div style='width: 100%;text-align: right'> بعد از اعلام شروع مجدد بازی از سمت گیم سنتر این متد فراخوانی می شود</div>
         * */
        private void OnResume()
        {
            _onResumeAction();
        }

        /**
         * <div style='width: 100%;text-align: right'> بعد از بررسی بازی از سمت گیم سنتر این متد فراخوانی می شود</div>
         * @param  Params
         *      <ul>
         *          <li>{Boolean} state - تعیین معتبر و یا نامعتبر بودن نتیجه مسابقه</li>
         *      </ul>
         * */
        private void OnEnd(JSONObject Params)
        {
            _onEndAction(Params);
        }

        /**
         * <div style='width: 100%;text-align: right'>بعد از دریافت پیام توسط حریف این متد فراخوانی می شود</div>
         * @param  Params
         *      <ul>
         *          <li>{string} dataId - شناسه پیام</li>
         *          <li>{‌bool} sequential - تعین ترتیبی بودن یا نبودن پیام</li>
         *      </ul>
         * */
        private void OnReceiveDataAck(JSONObject Params)
        {
            _onReceiveDataAckAction(Params);
        }

        /**
         * <div style='width: 100%;text-align: right'> بعد از ارسال پیام این متد فراخوانی می شود</div>
         * @param  Params
         *      <ul>
         *          <li>{string} dataId - شناسه پیام</li>
         *          <li>{‌bool} sequential - تعین ترتیبی بودن یا نبودن پیام</li>
         *      </ul>
         * */
        private void OnSentData(JSONObject Params)
        {
            _onSentDataAction(Params);
        }
    }
}
using System;
using System.Globalization;
using playpod.client.sdk.unity.Base;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity
{
    public partial class Service
    {
        private void InitNetwork()
        {
            //Debug.Log("network.initialize\n\n" + string.Format("appId: {0}\n\ndeviceId: {1}\n\nlang: {2}\n\n", appId, deviceId, lang));
            _network.Initialize(this, _mono, _stateMachine, _appId, _deviceId, _dic, _lang);

            _network.On("connect", new EventCallback(
                // onFire
                delegate(JSONObject Params)
                {
                    //Debug.Log("SOCKET_CONNECT " + Params);
                    try
                    {
                        _userData.Add("peerId", Params["peerId"].ToString());
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                    }

                    FireEvents("connect", Params);

                    LoginActionWithPeer();
                    CheckPeerAndSocketSync();

                    foreach (var item in _activeMatch.Values)
                    {
                        item.OnConnect();
                    }
                }
            ));

            _network.On("disconnect", new EventCallback(
                // onFire
                delegate(JSONObject Params)
                {
                    Debug.Log("SOCKET_DISCONNECT " + Params);

                    if (_userData.HasKey("peerId"))
                    {
                        _userData.Remove("peerId");
                    }

                    FireEvents("disconnect", new JSONObject());
                    foreach (var item in _activeMatch.Values)
                    {
                        item.OnDisconnect();
                    }
                }
            ));

            _network.On("reconnect", new EventCallback(
                // onFire
                delegate(JSONObject Params)
                {
                    Debug.Log("SOCKET_RECONNECT " + Params);

                    try
                    {
                        _userData.Add("peerId",
                            Params["peerId"].ToString());
                        FireEvents("reconnect", Params);
                        foreach (var item in _activeMatch.Values)
                        {
                            item.OnReconnect();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                    }
                }
            ));

            _network.On("message", new EventCallback(
                null,

                // OnFire
                onFireAsync: delegate(JSONObject message, AsyncResponse res)
                {
                    HandlePushMessageContent(message, res);
                    FireEvents("message", message, res);
                }
            ));
        }

        private void HandlePushMessageContent(JSONObject Params, AsyncResponse res)
        {
//            Debug.Log("handlePushMessageContent_1 " + PrettyJson(Params));
            try
            {
                var message = JSON.Parse(Params["content"]).AsObject;
                var senderMessageId = Params["senderMessageId"].AsInt.ToString();

                var messageType = message["type"].AsInt;
                var data = JSON.Parse(message["content"]).AsObject;
                
                if (messageType == PushMessageContentTypes.DataPack)
                {
                    OnReceiveDataPackAction(data, res);
                }
                else
                {
                    if (!_gameCenterMessagesId.ContainsKey(senderMessageId))
                    {
                        switch (messageType)
                        {
                            case PushMessageContentTypes.RequestIdState:
                                Debug.Log("REQUEST_ID_STATE " + data);
                                OnReceiveRequestIdStateAction(data);
                                break;
                            case PushMessageContentTypes.MatchNew:
                                Debug.Log("MATCH_NEW " + data);
                                OnReceiveNewMatchAction(data);
                                break;
                            case PushMessageContentTypes.MatchStart:
                                Debug.Log("MATCH_START " + data);
                                OnReceiveStartMatchAction(data);
                                break;
                            case PushMessageContentTypes.MatchResume:
                                Debug.Log("MATCH_RESUME " + data);
                                OnReceiveResumeMachAction(data);
                                break;
                            case PushMessageContentTypes.MatchPause:
                                Debug.Log("MATCH_PAUSE " + data);
                                OnReceivePauseMatchAction(data);
                                break;
                            case PushMessageContentTypes.MatchRequest:
                                Debug.Log("MATCH_REQUEST " + data);
                                OnReceiveRequestMatchAction(data);
                                break;
                            case PushMessageContentTypes.MatchResult:
                                Debug.Log("MATCH_RESULT " + data);
                                OnReceiveMatchResultAction(data);
                                break;
                            case PushMessageContentTypes.Message:
                                Debug.Log("MESSAGE " + data);
                                OnReceiveMessageAction(data);
                                break;
                            case PushMessageContentTypes.MatchReconnect:
                                Debug.Log("MATCH_RECONNECT " + data);
                                OnReceiveResumeMachAction(data);
                                break;
                            case PushMessageContentTypes.MatchLeave:
                                Debug.Log("MATCH_LEAVE " + data);
                                OnReceiveLeaveMachAction(data);
                                break;
                        }

                        _gameCenterMessagesId.Add(senderMessageId, true);
                        res.Call();
                    }
                    else
                    {
                        res.Call();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void Request(RequestUrls.UrlData urlData, JSONObject data, Action<JSONObject> onResult,
            JSONObject setting)
        {
            //        Debug.Log("Service.request: setting = null");
            if (urlData.Encrypt && ConfigData.Ure)
            {
                EncryptRequest(urlData, data, res => { requestResponseHandler(res, onResult); }, setting);
            }
            else
            {
                requestHandler(urlData, data, res => { requestResponseHandler(res, onResult); }, setting);
            }
        }

        private void requestHandler(RequestUrls.UrlData urlData, JSONObject data, Action<JSONObject> onResult,
            JSONObject setting)
        {
            //Debug.Log("Service.requestHandler");
            string uri;
            string url;

            if (setting != null && (setting.HasKey("url") || setting.HasKey("uri")))
            {
                try
                {
                    uri = setting.HasKey("url") ? setting["url"] : setting["uri"];
                }
                catch (Exception)
                {
                    uri = urlData.Uri;
                }
            }
            else
            {
                uri = urlData.Uri;
            }

            if (urlData.HostName.Equals("BAZITECH"))
            {
                var newUrlData = urlData.Copy();

                if (newUrlData.Encrypt && ConfigData.Ure)
                {
                    newUrlData.Uri = newUrlData.Uri.Replace("/srv", "/srv/enc");
                }

                if (ConfigData.Har)
                {
                    AsyncRequest(newUrlData, data, onResult, setting);
                    return;
                }
                else
                {
                    url = ConfigData.Gca + uri;
                }
            }
            else
            {
                url = ConfigData.Opsa + uri;
            }

            if (data.HasKey("token") && data["token"] != null &&
                data.HasKey("tokenIssuer") && data["tokenIssuer"] != null)
            {
                try
                {
                    data.Add("_token", data["token"]);
                    data.Add("_token_issuer", data["tokenIssuer"]);
                    data.Remove("token");
                    data.Remove("tokenIssuer");
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }
            }
            else if (_userData.HasKey("token"))
            {
                try
                {
                    data.Add("_token", _userData["token"]);
                    data.Add("_token_issuer", _userData["tokenIssuer"]);
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }
            }

            var requestData = new JSONObject();

            if (urlData.Encrypt && ConfigData.Ure)
            {
                url = url.Replace("/srv", "/srv/enc");
            }

            try
            {
                requestData.Add("url", url);
                requestData.Add("data", data);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            _network.PostRequest(requestData, resData =>
            {
                var returnData = resData;

                try
                {
                    if (!resData["HasError"].AsBool)
                    {
                        returnData = resData["Result"].AsObject;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }

                onResult(returnData);
            });
        }

        private void requestResponseHandler(JSONObject res, Action<JSONObject> onResult)
        {
            //        Debug.Log("requestResponseHandler: ");
            onResult(res);

            try
            {
                if (res["HasError"].AsBool &&
                    res["ErrorCode"].AsInt == -21 &&
                    _userData.HasKey("token") && _userData["token"] != null
                )
                {
                    LogoutAction();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void EncryptRequest(RequestUrls.UrlData url, JSONObject data, Action<JSONObject> onResult,
            JSONObject setting)
        {
            try
            {
                EncryptData(data, result =>
                {
                    try
                    {
                        if (result["hasError"].AsBool)
                        {
                            var retData = new JSONObject();
                            retData.Add("HasError", true);
                            retData.Add("ErrorMessage", result["errorMessage"]);
                            retData.Add("ErrorCode", result["errorCode"]);
                            onResult(retData);
                        }
                        else
                        {
                            requestHandler(url, result["result"].AsObject, onResult, setting);
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

        private void AsyncRequest(RequestUrls.UrlData urlData, JSONObject data, Action<JSONObject> onResult,
            JSONObject setting)
        {
            //        Debug.Log("Service.asyncRequest");

            //        getPeerIdFromHttpRequest(new RequestCallback() {
            //            @Override
            //            public void onResult(JSONObject result) {
            //
            //            }
            //        });

            try
            {
                //            string peerId = result.getString("peerId");
                string uri;

                if (urlData.Uri != null)
                {
                    uri = urlData.Uri;
                }
                else
                {
                    uri = setting["uri"];
                }

                var parameters = new JSONArray();
                var keys = new JSONArray();

                foreach (var key in data.Keys)
                {
                    keys.Add(key);
                }

                for (var i = 0; i < keys.Count; i++)
                {
                    var paramData = new JSONObject();
                    string keyName = keys[i];


                    paramData.Add("name", keyName);
                    paramData.Add("value", data[keyName]);

                    parameters.Add(paramData);
                }

                var canFromSocket = true; // some request same as device register and server register can not


                if (setting != null)
                {
                    if (setting.HasKey("parameters") && setting["parameters"] != null)
                    {
                        var Params = setting["parameters"].AsArray;

                        for (var i = 0; i < Params.Count; i++)
                        {
                            parameters.Add(Params[i]);
                        }
                    }

                    if (setting.HasKey("fromSocket") && setting["fromSocket"] != null)
                    {
                        canFromSocket = setting["fromSocket"].AsBool;
                    }
                }

                var asyncData = new JSONObject();
                var messageVo = new JSONObject();
                var gcParamData = new JSONObject();

                var clientMessageId = Guid.NewGuid() + "_" + _appId;

                gcParamData.Add("remoteAddr", null);
                gcParamData.Add("clientMessageId", clientMessageId);
                gcParamData.Add("serverKey", 0);
                gcParamData.Add("oneTimeToken", null);
                gcParamData.Add("parameters", parameters);
                gcParamData.Add("msgType", 3);
                gcParamData.Add("uri", uri);

                if (data.HasKey("token") && data["token"] != null &&
                    data.HasKey("tokenIssuer") && data["tokenIssuer"] != null)
                {
                    gcParamData.Add("token", data["token"]);
                    gcParamData.Add("tokenIssuer", data["tokenIssuer"]);
                }
                else if (_userData.HasKey("token") && _userData["token"] != null)
                {
                    gcParamData.Add("token", _userData["token"]);
                    gcParamData.Add("tokenIssuer", _userData["tokenIssuer"]);
                }

                gcParamData.Add("messageId", 1001);
                gcParamData.Add("expireTime", 0);

                messageVo.Add("content", gcParamData.ToString());
                messageVo.Add("messageId", 1001);
                messageVo.Add("priority", "1");
                messageVo.Add("peerName", ConfigData.Ahrrn);
                //                    messageVO.put("ttl", ConfigData.hrt);
                messageVo.Add("ttl", 0);

                asyncData.Add("content", messageVo.ToString());
                //                    asyncData.put("trackerId", 1001);

                asyncData.Add("type", 3);


                if (ConfigData.Harfs && _network.IsSocketOpen() && canFromSocket)
                {
                    asyncData.Add("type", 5);
                    asyncData.Add("timeout", ConfigData.Hrt);

                    Debug.Log("REQUEST_SEND " + clientMessageId + " " + asyncData);

                    _network.Emit(asyncData, result =>
                        {
                            Debug.Log("REQUEST_RESPONSE " + clientMessageId + " " + result);
                            onResult(result);
                        }
                    );
                }
                else
                {
                    var set = setting;
                    if (set == null)
                    {
                        set = new JSONObject();
                    }

                    var headers = new JSONObject();
                    headers.Add("Content-Type", "application/x-www-form-urlencoded;  charset=utf-8");
                    set.Add("headers", headers);

                    var url = ConfigData.Aha + "/srv";
                    set.Add("method", "POST");

                    //Debug.Log("final request Params: " + PrettyJson(asyncData));

                    var pData = "data=" + Uri.EscapeUriString(asyncData.ToString());

                    //                pData += ("&peerId=" + peerId);

                    HttpRequest(url, pData, result =>
                    {
                        //                    Debug.Log("Service.AsyncRequest.HttpRequest.onResult: result: " + result.ToString());
                        JSONObject retData;
                        try
                        {
                            var hasError = result["HasError"].AsBool;
                            if (!hasError)
                            {
                                var obj = JSON.Parse(result["Result"].ToString()).AsObject;
                                retData = JSON.Parse(obj["content"]).AsObject;
                            }
                            else
                            {
                                retData = result;
                            }
                        }
                        catch (Exception e)
                        {
                            retData = new JSONObject();
                            try
                            {
                                retData.Add("HasError", true);
                                retData.Add("ErrorMessage", e.Message);
                                retData.Add("ErrorCode", ErrorCodes.Exception);
                                retData.Add("Result", new JSONObject());
                            }
                            catch (Exception e1)
                            {
                                Debug.LogError("Exception: " + e1.Message);
                            }
                        }

                        if (onResult != null)
                        {
//                            Debug.Log("Service.AsyncRequest.HttpRequest.onResult: retData: " + retData.ToString());
                            onResult(retData);
                        }
                    }, set);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void HttpRequest(string url, string data, Action<JSONObject> onResult, JSONObject setting)
        {
//            Debug.Log("Service.HttpRequest");
            var method = "POST";

            if (setting != null && setting.HasKey("method") && setting["method"] != null &&
                setting["method"].ToString() == "GET")
            {
                method = "GET";
            }

            var requestData = new JSONObject();
            requestData.Add("url", url);
            requestData.Add("method", method);

            if (data != null)
            {
                requestData.Add("data", data);
            }

            if (setting != null && (setting.HasKey("headers") && setting["headers"] != null))
            {
                requestData.Add("headers", setting["headers"]);
            }

            _network.HttpRequest(requestData, onResult);
        }

        private void EncryptData(JSONObject data, Action<JSONObject> onResult)
        {
            try
            {
                data.Add("timestamp", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                var reqData = new JSONArray();
                var keys = new JSONArray();

                foreach (var key in data.Keys)
                {
                    keys.Add(key);
                }

                for (var i = 0; i < keys.Count; i++)
                {
                    var dataObj = new JSONObject();
                    dataObj.Add("name", keys[i]);

                    try
                    {
                        dataObj.Add("value", data[(string) keys[i]]);
                    }
                    catch (Exception)
                    {
                        dataObj.Add("value", data[(string) keys[i]]);
                    }

                    reqData.Add(dataObj);
                }

                EncryptHandshake(result =>
                {
                    try
                    {
                        if (result["hasError"].AsBool)
                        {
                            onResult(result);
                        }
                        else
                        {
                            var encryptData = result["result"].AsObject;
                            string algorithm = encryptData["algorithm"];
                            string key = encryptData["secretKey"];
                            string iv = encryptData["initializationVector"];

                            var strData = reqData.ToString();

                            var hash = Util.Md5(strData).ToUpper();
                            var plusData = hash + ConfigData.Ehd + strData;

                            string encrypt;

                            switch (algorithm)
                            {
                                case "AES":
                                    encrypt = Util.AesEncrypt(plusData, key, iv);
                                    break;

                                case "DES":
                                    encrypt = Util.DesEncrypt(plusData, key, iv);
                                    break;

                                default:
                                    encrypt = "";
                                    break;
                            }

                            var encRes = new JSONObject();
                            encRes.Add("data", encrypt);
                            encRes.Add("h", hash);
                            encRes.Add("clientId", _appId);

                            onResult(Util.CreateReturnData(false, "", 0, encRes));
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

        private void EncryptHandshake(Action<JSONObject> onResult)
        {
            onResult(new JSONObject());
            try
            {
                //            if (
                //                    encryptHandshakeData.encryptData != null && encryptHandshakeData.updateTime != null &&
                //                            (new Date().getTime() - encryptHandshakeData.updateTime.getTime() < ConfigData.ehet)
                //                    ) {
                //
                //                callback.onResult(Util.createReturnData(false, "", 0, encryptHandshakeData.getEncryptData()));
                //            } else {
                //
                //                if (encryptHandshakeData.updating) {
                //                    encryptHandshakeData.addCallbackQueue(callback);
                //                    encryptHandshakeData.updating = true;
                //                    return;
                //                }
                //
                //                JSONObject param = new JSONObject();
                //                param.put("clientId", appId);
                //
                //                request(RequestUrls.ENCRYPT_HAND_SHAKE, param, new Network.HttpRequestCallback() {
                //                    @Override
                //                    public void onResult(JSONObject result){
                //                        JSONObject returnData = null;
                //                        try {
                //                            if (result.getBoolean("HasError")) {
                //                                returnData = Util.createReturnData(true, result.getString("ErrorMessage"), result.getInt("ErrorCode"), new JSONObject());
                //
                //                            } else {
                //                                JSONObject encResult = result.getJSONObject("Result");
                //
                //                                encryptHandshakeData.setEncryptData(encResult.get("IV").ToString(), encResult.get("Alg").ToString(), encResult.get("SecretKey").ToString());
                //                                returnData = Util.createReturnData(false, "", 0, encryptHandshakeData.getEncryptData());
                //                            }
                //
                //                            callback.onResult(returnData);
                //
                //                            encryptHandshakeData.executeQueue(returnData);
                //                            encryptHandshakeData.updating = false;
                //                        } catch (Exception e) {
                //                            e.printStackTrace();
                //                        }
                //
                //
                //                    }
                //                });
                //            }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        public void HttpPostRequest(RequestUrls.UrlData url, JSONObject data, Action<JSONObject> onResult)
        {
//            Debug.Log("Service.HttpPostRequest");
            Request(url, data, result =>
            {
                try
                {
                    var returnData = new JSONObject();
                    returnData.Add("hasError", result["HasError"].AsBool);
                    returnData.Add("errorMessage", result["ErrorMessage"]);

                    returnData.Add("errorCode", result["ErrorCode"].AsInt);
                    if (result.HasKeyNotNull("Result"))
                    {
                        returnData.Add("result", result["Result"]);
                    }

                    onResult(returnData);
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }
            });
        }

        private void Request(RequestUrls.UrlData url, JSONObject data, Action<JSONObject> onResult)
        {
            //        Debug.Log("Service.request");
            try
            {
                Request(url, data, onResult, null);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }
    }
}
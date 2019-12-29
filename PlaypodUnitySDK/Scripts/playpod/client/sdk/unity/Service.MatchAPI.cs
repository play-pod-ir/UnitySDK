using System;
using System.Collections.Generic;
using playpod.client.sdk.unity.Base;
using playpod.client.sdk.unity.Base.Game;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity
{
    public partial class Service {
        
        
        
        /**
        * <div style='width: 100%;text-align: right'>ارسال نتیجه مسابقه </div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *      JSONObject reqData = new JSONObject();
        *      reqData.put("gameId", gameId);
        *      JSONArray player1Scores = new JSONArray();
        *      JSONArray player2Scores = new JSONArray();
        *
        *      JSONObject data1 = new JSONObject();
        *      data1.put("name", "field1");
        *      data1.put("value ", 33);
        *      player1Scores.put(data1);
        *
        *      JSONObject data2 = new JSONObject();
        *      data2.put("name", "field2");
        *      data2.put("value ", 10);
        *      player1Scores.put(data2);
        *
        *      JSONObject player1Data = new JSONObject();
        *      player1Data.put("playerId","**");
        *      player1Data.put("scores",player1Scores);
        *
        *      ///------------------
        *      JSONObject data1 = new JSONObject();
        *      data1.put("name", "field1");
        *      data1.put("value ", 33);
        *      player2Scores.put(data1);
        *
        *      JSONObject data2 = new JSONObject();
        *      data2.put("name", "field2");
        *      data2.put("value ", 10);
        *      player2Scores.put(data2);
        *
        *      JSONObject player2Data = new JSONObject();
        *      player2Data.put("playerId","**");
        *      player2Data.put("scores",player2Scores);
        *
        *      JSONArray playersData = new JSONArray();
        *      playersData.put(player1Data);
        *      playersData.put(player2Data);
        *      reqData.put("result",playersData);
        *      service.sendMatchResultRequest(reqData, new RequestCallback() {
        *          {@code @Override}
        *          public void onResult(JSONObject result) {
        *              System.out.println("sendMatchResultRequest method : " + result);
        *          }
        *      });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} gameId - شناسه بازی</li>
        *          <li>{JSONArray} result - نتیجه بازی
        *          array of JSONObject that each object contain :
        *              <ul>
        *                  <li>{string}playerId</li>
        *                  <li>{JSONArray}scores</li>
        *              </ul>
        *          </li>
        *      </ul>
        *
        * @param  callback
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{bool} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *      </ul>
        *
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void SendMatchResultRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                if (!_userData.HasKeyNotNull("loginState") || !_userData["loginState"].AsBool)
                {
                    Debug.LogError("first login in game center");
                    throw new ServiceException("first login in game center");
                }

                var gameResults = Params["result"].AsArray;
                string gameId = Params["gameId"];
                JSONObject ownResult;
                JSONObject opponentResult = null;
                JSONArray result = new JSONArray();
                var firstData = gameResults[0].AsObject;
                if (gameResults.Count > 1)
                {
                    ownResult = gameResults[1].AsObject;
                    opponentResult = firstData;
                }
                else
                {
                    ownResult = firstData;
                }

                result.Add(ownResult);

                if (opponentResult != null)
                {
                    result.Add(opponentResult);
                }

                var param = new JSONObject();
                param.Add("result", result.ToString());
                param.Add("forceAddToTable", true);
                param.Add("gameId", gameId);

                _sendResult(param, onResult);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }
        
        /**
        * <div style='width: 100%;text-align: right'> درخواست بازی </div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *      JSONObject reqData = new JSONObject();
        *      reqData.put("gameId", "gameId");
        *      reqData.put("leagueId", "leagueId");
        *      reqData.put("opponentId", "userId");
        *
        *      service.matchRequest(reqData, new MatchRequestCallback() {
        *          {@code @Override}
        *          public void onResult(JSONObject data) {
        *              System.out.println("matchRequest method -- onResult : " + data);
        *          }
        *          {@code @Override}
        *          public void onAccept(JSONObject data) {
        *              System.out.println("matchRequest method -- onAccept : " + data);
        *          }
        *          {@code @Override}
        *          public void onReject(JSONObject data) {
        *              System.out.println("matchRequest method -- onReject : " + data);
        *          }
        *          {@code @Override}
        *          public void onCancel(JSONObject data) {
        *              System.out.println("matchRequest method -- onCancel : " + data);
        *          }
        *      });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} opponentId</li>
        *          <li>{string} gameId</li>
        *          <li>{string} leagueId</li>
        *          <li>{bool} [isOffline=false]</li>
        *          <li>{Integer} [requestTime] -  for offline request</li>
        *      </ul>
        *
        * @param  callback
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{bool} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *          <li>{JSONObject} result
        *              <ul>
        *                  <li>{JSONObject} requestId - شناسه درخواست </li>
        *              </ul>
        *          </li>
        *      </ul>
        *
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void MatchRequest(JSONObject Params, MatchRequestCallback callback)
        {
            try
            {
                string gameId = (Params.HasKeyNotNull("gameId")) ? Params["gameId"] : null;
                string leagueId = (Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;
                string opponentId = (Params.HasKeyNotNull("opponentId")) ? Params["opponentId"] : null;
                var requestUrl = RequestUrls.RequestMatch;
                var requestData = new JSONObject();

                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    return;
                    //throw new ServiceException(" not exist in Params");
                }

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId not exist in Params");
                }

                if (opponentId == null)
                {
                    Debug.LogError("opponentId not exist in Params");
                    return;
                    //throw new ServiceException(" not exist in Params");
                }

                var isOffline = (Params.HasKeyNotNull("isOffline")) && Params["isOffline"].AsBool;

                if (isOffline)
                {
                    requestUrl = RequestUrls.OfflineMatchRequest;
                    requestData.Add("opponentUserId", opponentId);
                    requestData.Add("gameId", gameId);
                    requestData.Add("leagueId", leagueId);

                    if (Params.HasKeyNotNull("requestTime"))
                    {
                        requestData.Add("timestamp", Params["requestTime"].AsInt);
                    }
                }
                else
                {
                    if (!_isMultiTab && (IsGameRun() || _currentMatchRequestCount > 0))
                    {
                        var retData = new JSONObject();

                        string errorMessage = _dic["CANTNOTREQUESTINPLAING"].AsObject[_lang];
                        if (_currentMatchRequestCount > 0)
                        {
                            errorMessage = _dic["WAITFORPREVIOUSREQUEST"].AsObject[_lang];
                        }

                        retData.Add("hasError", true);
                        retData.Add("errorMessage", errorMessage);

                        callback.OnResult(retData);
                        return;
                    }

                    //Debug.Log("matchRequest__ " + network.getSocketConnectionState() + " " + userData.ToString());

                    if (!_network.GetSocketConnectionState() || !_userData.HasKey("peerId") || _userData["peerId"] == null)
                    {
                        var returnData = new JSONObject();
                        returnData.Add("hasError", true);
                        returnData.Add("errorCode", ErrorCodes.UserNotConnected);
                        returnData.Add("errorMessage", _dic["CONNECTINGTOPUSH"].AsObject[_lang]);
                        callback.OnResult(returnData);
                        return;
                    }

                    _currentMatchRequestCount += 1;
                    requestData.Add("opponentId", opponentId);
                    requestData.Add("gameId", gameId);
                    requestData.Add("leagueId", leagueId);
                    requestData.Add("id", 0);
                    requestData.Add("sessionId", _userData["peerId"]);

                    string gameVersion = null;

                    if (Params.HasKeyNotNull("version"))
                    {
                        gameVersion = Params["version"];
                    }
                    else
                    {
                        if (_games.HasKeyNotNull(gameId))
                        {
                            gameVersion = _games[gameId].AsObject["version"];
                        }
                    }

                    if (gameVersion == null)
                    {
                        Debug.LogError("version not exist in Params");
                        return;
                        //throw new ServiceException(" not exist in Params");
                    }

                    requestData.Add("version", gameVersion);

                    //                requestData.put("request", requestData.toString());
                }

                Request(requestUrl, requestData, result =>
                {
//                    Debug.Log("Service.matchRequest.request.response: " + PrettyJson(result));
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);

                        if (hasError)
                        {
                            if (!isOffline)
                            {
                                _currentMatchRequestCount -= 1;
                            }

                            returnData.Add("errorMessage", result["ErrorMessage"]);
                            returnData.Add("errorCode", result["ErrorCode"].AsInt);
                        }
                        else
                        {
                            returnData.Add("errorMessage", "");

                            var retRes = new JSONObject();

                            if (!isOffline)
                            {
//                                Debug.Log("isOffline: " + isOffline);
                                var requestId = result["Result"].AsObject["requestId"].AsInt.ToString();

                                _requestIdLeague.Add(requestId, leagueId);

                                Dictionary<string, object> leaguesRequest;

                                if (!_activeMatchRequest.ContainsKey(leagueId))
                                {
                                    leaguesRequest = new Dictionary<string, object>();
                                    _activeMatchRequest.Add(leagueId, leaguesRequest);
                                }
                                else
                                {
                                    leaguesRequest = _activeMatchRequest[leagueId];
                                }

                                var timeoutId = Util.SetTimeout(() =>
                                {
                                    try
                                    {
                                        var leaguData = _activeMatchRequest[leagueId];
                                        if (leaguData[requestId] != null)
                                        {
                                            _currentMatchRequestCount -= 1;
                                            leaguData.Remove(requestId);
                                            var data = new JSONObject();
                                            data.Add("requestId", requestId);
                                            callback.OnCancel(data);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.LogError("Exception: " + e.Message);
                                    }
                                }, ConfigData.Mrt);

                                var requestDataTemp = new Dictionary<string, object>();
                                requestDataTemp.Add("opponentId", opponentId);
                                requestDataTemp.Add("timeoutId", timeoutId);
                                requestDataTemp.Add("callback", callback);

                                leaguesRequest.Add(requestId, requestDataTemp);

                                retRes.Add("requestId", requestId);
                            }

                            returnData.Add("result", retRes);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                        returnData = ExceptionErrorData(e);
                    }

                    callback.OnResult(returnData);
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        /**
        * <div style='width: 100%;text-align: right'>  دریافت برترین امتیازات </div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *      JSONObject reqData = new JSONObject();
        *      reqData.put("gameId", "2");
        *      reqData.put("isGlobal", true);
        *      service.getTopScore(reqData, new RequestCallback() {
        *          {@code @Override}
        *          public void onResult(JSONObject result) {
        *              System.out.println("getTopScore method : " + result);
        *          }
        *            });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} gameId - شناسه بازی</li>
        *          <li>{string} [leagueId] - شناسه لیگ</li>
        *          <li>{bool} [isGlobal] - برترین در بین کاربران و یا امتیازات خود کاربر</li>
        *      </ul>
        *
        * @param  callback
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{bool} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *          <li>{JSONArray} result آرایه ای از آبجکت می باشد که هر کدام دارای مقادیر زیر است
        *              <ul>
        *                  <li>{string} id</li>
        *                  <li>{string} name</li>
        *                  <li>{string} score</li>
        *              </ul>
        *          </li>
        *      </ul>
        *
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void GetTopScore(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string gameId = (Params.HasKeyNotNull("gameId")) ? Params["gameId"] : null;

                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    //throw new ServiceException("gameId not exist in Params");
                }

                string leagueId = (Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;
                var isGlobal = (Params.HasKeyNotNull("isGlobal")) ? (bool?) Params["isGlobal"].AsBool : null;

                var requestData = new JSONObject();
                requestData.Add("gameId", gameId);

                if (leagueId != null)
                {
                    requestData.Add("leagueId", leagueId);
                }

                if (isGlobal != null)
                {
                    requestData.Add("isGlobal", isGlobal);
                }

                Request(RequestUrls.TopScore, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        if (!hasError)
                        {
                            var all = new JSONArray();
                            var scores = result["Result"].AsObject["topScores"].AsArray;
                            for (var i = 0; i < scores.Count; i++)
                            {
                                var user = new JSONObject();
                                user.Add("id", scores[i].AsObject["PlayerID"]);
                                user.Add("name", scores[i].AsObject["playerName"]);
                                user.Add("score", scores[i].AsObject["Score"]);
                                all.Add(user);
                            }

                            returnData.Add("result", all);
                        }
                    }
                    catch (Exception e)
                    {
                        returnData = ExceptionErrorData(e);
                    }

                    onResult(returnData);
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }
        
        /**
        * <div style='width: 100%;text-align: right'>لغو درخواست مسابقه </div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *     JSONObject reqData = new JSONObject();
        *     reqData.put("gameId", "gameId");
        *     reqData.put("leagueId", "leagueId");
        *     reqData.put("opponentId", userId);
        *
        *     // درخواست مسابقه
        *     service.matchRequest(reqData, new MatchRequestCallback() {
        *         {@code @Override}
        *         public void onResult(JSONObject result) {
        *             try {
        *                 bool hasError = result.getBoolean("hasError");
        *                 if (!hasError) {
        *                     string requestId = result.getJSONObject("result").getString("requestId");// شناسه درخواست
        *                     JSONObject reqData = new JSONObject();
        *                     reqData.put("requestId", requestId);
        *                     // لغو درخواست
        *                     service.cancelMatchRequest(reqData, new RequestCallback() {
        *                         {@code @Override}
        *                         public void onResult(JSONObject result) {
        *                             System.out.println("cancelMatchRequest method : " + result);
        *                         }
        *                     });
        *                 }
        *             } catch (JSONException|ServiceException e) {
        *                 e.printStackTrace();
        *             }
        *         }
        *     });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} requestId - شناسه درخواست</li>
        *      </ul>
        *
        * @param  res
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{bool} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *      </ul>
        *
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void CancelMatchRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string requestId = (Params.HasKeyNotNull("requestId")) ? Params["requestId"] : null;

                if (requestId == null)
                {
                    Debug.LogError("requestId not exist in Params");
                    throw new ServiceException("requestId not exist in Params");
                }

                var requestData = new JSONObject();
                requestData.Add("requestId", requestId);

                Request(RequestUrls.CancelMatchRequest, requestData, result =>
                {
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        var returnData = new JSONObject();
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var leagueId = _requestIdLeague[requestId];
                        if (!hasError)
                        {
                            _currentMatchRequestCount -= 1;
                            if (leagueId != null)
                            {
                                var leagueData = _activeMatchRequest[leagueId];
                                if (leagueData[requestId] != null)
                                {
                                    leagueData.Remove(requestId);
                                }
                            }
                        }

                        onResult(returnData);
                    }
                    catch (Exception e)
                    {
                        onResult(ExceptionErrorData(e));
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        /**
        * <div style='width: 100%;text-align: right'>  درخواست شناسه بازی </div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *      JSONObject reqData = new JSONObject();
        *      reqData.put("gameId", gameId);
        *      reqData.put("leagueId", leagueId);
        *      service.matchIdRequest(reqData, new RequestCallback() {
        *          {@code @Override}
        *          public void onResult(JSONObject result) {
        *              System.out.println("matchIdRequest method : " + result);
        *          }
        *      });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} gameId - شناسه بازی</li>
        *          <li>{string} leagueId - شناسه لیگ </li>
        *      </ul>
        *
        * @param callback
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{bool} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *          <li>{JSONObject} result
        *              <ul>
        *                  <li>{string} result.matchId - شناسه بازی</li>
        *              </ul>
        *          </li>
        *      </ul>
        *
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void MatchIdRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string gameId = (Params.HasKeyNotNull("gameId")) ? Params["gameId"] : null;
                string leagueId = (Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;

                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    throw new ServiceException("gameId not exist in Params");
                }

                if (leagueId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    throw new ServiceException("leagueId not exist in Params");
                }

                var requestData = new JSONObject();
                requestData.Add("gameId", gameId);
                requestData.Add("leagueId", leagueId);


                Request(RequestUrls.RequestMatchId, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        var retResult = new JSONObject();

                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"]);
                        returnData.Add("result", retResult);

                        if (!hasError)
                        {
                            retResult.Add("matchId", result["Result"].AsObject["ID"]);
                        }
                    }
                    catch (Exception e)
                    {
                        returnData = ExceptionErrorData(e);
                    }

                    onResult(returnData);
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        /**
        * <div style='width: 100%;text-align: right'>درخواست اضافه شدن به لیست حریف می طلبم</div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *       JSONObject reqData = new JSONObject();
        *       reqData.put("leagueId", leagueId);
        *       reqData.put("gameId", gameId);
        *       service.quickMatchRequest(reqData, new QuickMatchRequestCallback() {
        *           {@code @Override}
        *           public void onResult(JSONObject result) {
        *               System.out.println("quickMatchRequest method -- onResult : " + result);
        *           }
        *           {@code @Override}
        *           public void onCancel(JSONObject data) {
        *               System.out.println("quickMatchRequest method -- onCancel : " + data);
        *           }
        *           {@code @Override}
        *           public void onAccept(JSONObject data) {
        *               System.out.println("quickMatchRequest method -- onAccept : " + data);
        *           }
        *       });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} gameId - شناسه بازی</li>
        *          <li>{string} leagueId - شناسه لیگ</li>
        *      </ul>
        *
        * @param  callback
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{bool} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *      </ul>
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void QuickMatchRequest(JSONObject Params, QuickMatchRequestCallback callback)
        {
            try
            {
                string gameId = (Params.HasKeyNotNull("gameId")) ? Params["gameId"] : null;
                string leagueId = (Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;

                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    throw new ServiceException("gameId not exist in Params");
                }

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    throw new ServiceException("leagueId not exist in Params");
                }

                if (!_network.GetSocketConnectionState())
                {
                    var returnData = new JSONObject();
                    returnData.Add("hasError", true);
                    returnData.Add("errorMessage", _dic["CONNECTINGTOPUSH"].AsObject[_lang]);
                    callback.OnResult(returnData);
                    return;
                }

                if (!_isMultiTab && (IsGameRun() || _currentMatchRequestCount > 0 ||
                                     (int) _quickMatchData["requestCount"] > 0))
                {
                    string errorMessage = _dic["CANTNOTREQUESTINPLAING"].AsObject[_lang];

                    if (_currentMatchRequestCount > 0 || (int) _quickMatchData["requestCount"] > 0)
                    {
                        errorMessage = _dic["WAITFORPREVIOUSREQUEST"].AsObject[_lang];
                    }

                    var returnData = new JSONObject();
                    returnData.Add("hasError", true);
                    returnData.Add("errorMessage", errorMessage);
                    callback.OnResult(returnData);
                    return;
                }

                var leaguesQuickData = (Dictionary<string, object>) _quickMatchData["leagues"];
                var quickData = (Dictionary<string, object>) leaguesQuickData[leagueId];
                if (quickData == null)
                {
                    quickData = new Dictionary<string, object>();
                    quickData.Add("state", true);
                    quickData.Add("callback", callback);
                    leaguesQuickData.Add(leagueId, quickData);
                }
                else
                {
                    if ((bool) quickData["state"])
                    {
                        return;
                    }

                    quickData.Add("state", true);
                    quickData.Add("callback", callback);
                }

                _quickMatchData.Add("requestCount", (int) _quickMatchData["requestCount"] + 1);
                _quickMatchData.Add("lastLeagueId", leagueId);

                var requestData = new JSONObject();
                requestData.Add("leagueId", leagueId);
                requestData.Add("peerId", _userData["peerId"]);


                QuickRequest(requestData, false, callback);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        /**
        * <div style='width: 100%;text-align: right'> استریم درخواست شناسه بازی </div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *      JSONObject reqData = new JSONObject();
        *      reqData.put("gameId", gameId);
        *      reqData.put("leagueId", leagueId);
        *      service.streamMatchIdRequest(reqData, new RequestCallback() {
        *          {@code @Override}
        *          public void onResult(JSONObject result) {
        *              System.out.println("streamMatchIdRequest method : " + result);
        *          }
        *      });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} gameId - شناسه بازی</li>
        *          <li>{string} leagueId - شناسه لیگ </li>
        *          <li>{Integer} clientType -
        *              1 => WEB
        *              2 => ANDROID
        *              3 => PC
        *          </li>
        *      </ul>
        *
        * @param callback
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{bool} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *          <li>{JSONObject} result
        *              <ul>
        *                  <li>{string} matchId - شناسه بازی</li>
        *                  <li>{string} ip </li>
        *                  <li>{Integer} ws </li>
        *                  <li>{Integer} wss </li>
        *                  <li>{Integer} io </li>
        *                  <li>{Integer} rtsp </li>
        *                  <li>{Integer} width </li>
        *                  <li>{Integer} height </li>
        *              </ul>
        *          </li>
        *      </ul>
        *
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void StreamMatchIdRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string gameId = (Params.HasKeyNotNull("gameId")) ? Params["gameId"] : null;
                string leagueId = (Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;
                var clientType = (Params.HasKeyNotNull("clientType")) ? (int?) Params["clientType"].AsInt : null;

                if (gameId == null && leagueId == null)
                {
                    Debug.LogError("gameId or leagueId not exist in Params");
                    throw new ServiceException("gameId or leagueId not exist in Params");
                }

                if (clientType == null)
                {
                    Debug.LogError("clientType not exist in Params");
                    throw new ServiceException("clientType not exist in Params");
                }


                var requestData = new JSONObject();
                requestData.Add("clientType", clientType);

                if (gameId != null)
                {
                    requestData.Add("gameId", gameId);
                }

                if (leagueId != null)
                {
                    requestData.Add("leagueId", leagueId);
                }

                if (_userData.HasKeyNotNull("peerId"))
                {
                    requestData.Add("peerIds", _userData["peerId"]);
                }


                Request(RequestUrls.RequestStreamMatchId, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        var retResult = new JSONObject();

                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"]);
                        returnData.Add("result", retResult);

                        if (!hasError)
                        {
                            var innerResult = result["Result"].AsObject;
                            retResult.Add("matchId", innerResult["matchId"]);

                            retResult.Add("ip", innerResult["ip"]);


                            if (innerResult.HasKeyNotNull("ws"))
                            {
                                retResult.Add("ws", innerResult["ws"]);
                            }

                            if (innerResult.HasKeyNotNull("wss"))
                            {
                                retResult.Add("wss", innerResult["wss"]);
                            }

                            if (innerResult.HasKeyNotNull("io"))
                            {
                                retResult.Add("io", innerResult["io"]);
                            }

                            if (innerResult.HasKeyNotNull("rtsp"))
                            {
                                retResult.Add("rtsp", innerResult["rtsp"]);
                            }

                            if (innerResult.HasKeyNotNull("w"))
                            {
                                retResult.Add("width", innerResult["w"]);
                            }

                            if (innerResult.HasKeyNotNull("h"))
                            {
                                retResult.Add("height", innerResult["h"]);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        returnData = ExceptionErrorData(e);
                    }

                    onResult(returnData);
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }
        
         /**
        * <div style='width: 100%;text-align: right'> جواب درخواست مسابقه</div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *      JSONObject reqData = new JSONObject();
        *      reqData.put("requestId", "123456");
        *      reqData.put("rejectReasonType", 1);
        *      service.matchRequestResponse(reqData, new RequestCallback() {
        *          {@code @Override}
        *          public void onResult(JSONObject result) {
        *              System.out.println("matchRequestResponse method : " + result);
        *          }
        *      });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} requestId - شناسه درخواست</li>
        *          <li>{Integer} [rejectReasonType] -  در صورت عدم پذیرش درخواست مقدار آن کد مورد نظر برای رد درخواست می باشد</li>
        *          <li>{string} [rejectMessage] - دلیل رد درخواست در صورت منفی بودن جواب درخواست</li>
        *      </ul>
        *
        * @param  callback
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{Boolean} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *          <li>{JSONObject} result
        *              <ul>
        *                  <li>{Double} birthDate</li>
        *              </ul>
        *          </li>
        *      </ul>
        *
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void MatchRequestResponse(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var state = true;
                var requestData = new JSONObject();
                requestData.Add("requestId", Params["requestId"]);

                string peerId = (_userData.HasKeyNotNull("peerId")) ? _userData["peerId"] : null;

                if (peerId == null)
                {
                    Debug.LogError("در حال ارتباط با سرور, دوباره تلاش کنید");
                    throw new ServiceException("در حال ارتباط با سرور, دوباره تلاش کنید");
                }

                requestData.Add("sessionId", peerId);


                var rejectReasonType = (Params.HasKeyNotNull("rejectReasonType"))
                    ? (int?) Params["rejectReasonType"].AsInt
                    : null;
                if (rejectReasonType != null)
                {
                    state = false;
                    requestData.Add("rejectReasonType", rejectReasonType);
                    if (Params.HasKeyNotNull("rejectMessage"))
                    {
                        requestData.Add("rejectMessage", Params["rejectMessage"]);
                    }
                    else
                    {
                        requestData.Add("rejectMessage",
                            MatchRequestRejectTypes.GetMessage((int) rejectReasonType,
                                LanguageTypes.Fa));
                    }
                }

                requestData.Add("result", state);
                //            requestData.put("result", requestData.toString());

                Request(RequestUrls.MatchRequestResponse, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        returnData.Add("hasError", result["HasError"].AsBool);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"]);
                    }
                    catch (Exception e)
                    {
                        returnData = ExceptionErrorData(e);
                    }

                    onResult(returnData);
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        /**
        * <div style='width: 100%;text-align: right'>درخواست حذف از لیست حریف می طلبم</div>
        * <pre>
        *  <code style='float:right'>نمونه کد</code>
        *  <code>
        *      JSONObject reqData = new JSONObject();
        *      reqData.put("leagueId", "leagueId");
        *      service.cancelQuickMatchRequest(reqData, new RequestCallback() {
        *          {@code @Override}
        *          public void onResult(JSONObject result) {
        *              System.out.println("cancelQuickMatchRequest method : " + result);
        *          }
        *      });
        *  </code>
        * </pre>
        * @param  Params
        *      <ul>
        *          <li>{string} leagueId - شناسه بازی</li>
        *      </ul>
        * @param  callback
        *      <p>onResult method Params is JSONObject that has</p>
        *      <ul>
        *          <li>{Boolean} hasError</li>
        *          <li>{string} errorMessage</li>
        *          <li>{Integer} errorCode</li>
        *      </ul>
        *
        * @throws ServiceException خطای پارامتر های ورودی
        * */
        public void CancelQuickMatchRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string leagueId = (Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    throw new ServiceException("leagueId not exist in Params");
                }

                var requestData = new JSONObject();
                requestData.Add("leagueId", leagueId);
                requestData.Add("peerId", _userData["peerId"]);


                Request(RequestUrls.CancelQuickMatch, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);


                        var leaguesQuickData =
                            (Dictionary<string, object>) _quickMatchData["leagues"];
                        var quickData = (Dictionary<string, object>) leaguesQuickData[leagueId];
                        if (!hasError && quickData != null)
                        {
                            var timeoutId = (string) quickData["timeoutId"];
                            var quickres = (QuickMatchRequestCallback) quickData["callback"];
                            if (timeoutId != null)
                            {
                                Util.ClearTimeout(timeoutId);
                                quickData.Add("timeoutId", null);
                            }

                            var cancelData = new JSONObject();
                            cancelData.Add("leagueId", leagueId);
                            cancelData.Add("message", "");
                            cancelData.Add("state", true);
                            quickres.OnCancel(cancelData);

                            quickData.Add("state", false);
                            _quickMatchData.Add("requestCount", (int) _quickMatchData["requestCount"] - 1);
                            _quickMatchData.Add("lastLeagueId", null);
                        }
                    }
                    catch (Exception e)
                    {
                        returnData = ExceptionErrorData(e);
                    }

                    onResult(returnData);
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }
        
        private void OnReceiveLeaveMachAction(JSONObject data)
        {
            try
            {
                string matchId = data["matchId"];

                if (_activeMatch[matchId] != null)
                {
                    _activeMatch[matchId]._onLeave(data);
                }

                FireEvents("matchLeave", data);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void OnReceiveMatchResultAction(JSONObject data)
        {
            try
            {
                string matchId = data["matchId"];

                if (_activeMatch[matchId] != null)
                {
                    _activeMatch[matchId]._onEnd(data);
                }

                FireEvents("matchEnd", data);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void OnReceiveMessageAction(JSONObject Params)
        {
            try
            {
                var type = Params["type"].AsInt;
                var data = JSON.Parse(Params["content"]).AsObject;

                switch (type)
                {
                    case 1:

                        var content = new JSONObject();

                        content.Add("title", data["title"]);
                        content.Add("message", data["msg"]);
                        if (data.HasKeyNotNull("to"))
                        {
                            content.Add("timeout", data["to"]);
                        }

                        if (data.HasKeyNotNull("imgLink"))
                        {
                            content.Add("icon", data["imgLink"]);
                        }

                        if (data.HasKeyNotNull("oAction"))
                        {
                            content.Add("operationAction", data["oAction"]);
                        }

                        if (data.HasKeyNotNull("oContent"))
                        {
                            content.Add("operationContent", data["oContent"]);
                        }

                        if (data.HasKeyNotNull("owc"))
                        {
                            content.Add("webOperationContent", data["owc"]);
                        }


                        FireEvents("message", content);

                        break;
                    default:
                        Debug.Log("onReceiveMessageAction_ " + "NOT SUPPORTED MESSAGE");
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        private void OnReceiveRequestMatchAction(JSONObject data)
        {
            try
            {
                string requestId = data["requestId"];
                string gameId = data["gameId"];
                string leagueId = data["gameId"];

                if (!_userData["loginState"].AsBool)
                {
                    return;
                }


                var content = new JSONObject();
                content.Add("name", data["name"]);
                content.Add("id", data["id"]);
                content.Add("gameName", data["gameName"]);
                content.Add("leagueName", data["leagueName"]);
                content.Add("requestId", requestId);
                content.Add("packageName", data["packageName"]);
                content.Add("version", data["version"]);
                content.Add("platform", data["platform"]);
                content.Add("gameId", gameId);
                content.Add("leagueId", leagueId);


                if (data.HasKeyNotNull("player1Image"))
                {
                    var image = data["player1Image"].AsObject;
                    image.Add("id", image["id"]);
                    content.Add("image", image);
                }

                if (data.HasKeyNotNull("player1ImageUrl"))
                {
                    content.Add("imageUrl", data["player1ImageUrl"]);
                }


                if (_games.HasKeyNotNull(gameId))
                {
                    if (_autoMatchRequestAccept)
                    {
                        var requestData = new JSONObject();
                        requestData.Add("requestId", requestId);
                        MatchRequestResponse(requestData, null);
                        return;
                    }

                    var hasMajorConflict = HasMajorConflict(gameId, data["version"]);

                    if (hasMajorConflict)
                    {
                        var res = new JSONObject();
                        res.Add("requestId", requestId);
                        res.Add("rejectReasonType", MatchRequestRejectTypes.UserVersionConflict);
                        MatchRequestResponse(res, null);
                        var gameData = _games[gameId].AsObject;
                        if (gameData.HasKeyNotNull("info"))
                        {
                            var info = gameData["info"].AsObject;
                            info.Add("lastVersion", data["version"]);
                        }

                        MajorConflictAction();
                    }
                    else
                    {
                        if (_isMultiTab)
                        {
                            FireEvents("matchRequest", content);
                        }
                        else
                        {
                            if (!IsGameRun())
                            {
                                FireEvents("matchRequest", content);
                            }
                        }
                    }
                }
                else
                {
                    FireEvents("matchRequest", content);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }
        
        private void OnReceivePauseMatchAction(JSONObject data)
        {
            try
            {
                string matchId = data["matchId"];

                if (_activeMatch[matchId] != null)
                {
                    _activeMatch[matchId]._onPause(data);
                }

                FireEvents("matchPause", data);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void OnReceiveResumeMachAction(JSONObject data)
        {
            try
            {
                string matchId = data["matchId"];

                if (_activeMatch[matchId] != null)
                {
                    _activeMatch[matchId]._onResume(data);
                }

                FireEvents("matchResume", data);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void OnReceiveStartMatchAction(JSONObject data)
        {
            try
            {
                string matchId = data["matchId"];

                if (_activeMatch[matchId] != null)
                {
                    _activeMatch[matchId]._onStart(data);
                }

                FireEvents("matchStart", data);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void OnReceiveNewMatchAction(JSONObject data)
        {
            try
            {
                if (!(_userData.HasKeyNotNull("loginState") && _userData["loginState"].AsBool))
                {
                    return;
                }

                data.Add("isMultiPlayer", true);
                NewMatch(data);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void NewMatch(JSONObject matchData)
        {
            Debug.Log("newMatch_1 " + matchData);
            try
            {
                var isMultiPlayer = matchData["isMultiPlayer"].AsBool;

                string gameId = matchData["gameId"];

                if (isMultiPlayer)
                {
                    var hasGameCallback = true;
                    JSONObject gameObjectJson = null;
                    string callback = null;
                    if (_games.HasKeyNotNull(gameId))
                    {
                        gameObjectJson = _games[gameId].AsObject;
                    }

                    if (gameObjectJson != null && gameObjectJson.HasKeyNotNull("callback"))
                    {
                        callback = gameObjectJson["callback"];
                    }

                    MultiPlayer game = null;
                    var mData = ReformatMultiPlayerMatchData(matchData);

                    if (callback != null)
                    {
                        // todo: implement later. VERY IMPORTANT
                        // we should have a reference of a class which inherits from the MULTIPLAYER class here
                        // which all the abstract functions of MULTIPLAYER is defined in that class
                        // and a new instance of that class is created every time we get here.

                        // game = (MultiPlayer)Class.forName(callback).newInstance();
                    }
                    else
                    {
                        hasGameCallback = false;
                        game = new MultiPlayer(
                            // onReceiveData
                            delegate(JSONObject receivedData, AsyncResponse asyncCallback) { asyncCallback.Call(); },

                            // onConnect
                            delegate { },

                            // onReconnect
                            delegate { },

                            // onDisconnect
                            delegate { },

                            // onLeave
                            delegate { },

                            // onInit
                            delegate { },

                            // onStart
                            delegate { },

                            // onPause
                            delegate { },

                            // onResume
                            delegate { },

                            // onEnd
                            delegate { },

                            // onReceiveDataAck
                            delegate { },

                            // onSentData
                            delegate { });
                    }

                    if (game != null)
                    {
                        game.Initialize(this, _network, mData);
                        string matchId = matchData["matchId"];
                        string leagueId = matchData["leagueId"];
                        string requestId = matchData.HasKeyNotNull("requestId") ? matchData["requestId"] : null;

                        var leagueData = _activeMatchRequest[leagueId];
                        if (leagueData != null)
                        {
                            var requestData = (Dictionary<string, object>) leagueData[requestId];
                            if (requestData != null)
                            {
                                var res = (MatchRequestCallback) requestData["callback"];
                                var data = new JSONObject();
                                data.Add("requestId", requestId);
                                res.OnAccept(data);
                                _currentMatchRequestCount -= 1;
                                Util.ClearTimeout((string) requestData["timeoutId"]);
                                leagueData.Remove(requestId);
                            }
                        }

                        if (matchData.HasKeyNotNull("isQuick") && matchData["isQuick"].AsBool)
                        {
                            var leaguesQuickData =
                                (Dictionary<string, object>) _quickMatchData["leagues"];
                            var quickData = (Dictionary<string, object>) leaguesQuickData[leagueId];
                            if (quickData != null && (bool) quickData["state"])
                            {
                                quickData.Add("state", false);
                                _quickMatchData.Add("requestCount", (int) _quickMatchData["requestCount"] - 1);
                                _quickMatchData.Add("lastLeagueId", null);
                                var res = (QuickMatchRequestCallback) quickData["callback"];
                                var quickRet = new JSONObject();
                                quickRet.Add("leagueId", leagueId);
                                res.OnAccept(quickRet);


                                var timeoutId = (string) quickData["timeoutId"];
                                if (timeoutId != null)
                                {
                                    Util.ClearTimeout(timeoutId);
                                    quickData.Add("timeoutId", null);
                                }
                            }
                        }

                        _activeMatch.Add(matchId, game);
                    }

                    if (!hasGameCallback)
                    {
                        FireEvents("newMatch", mData);
                    }
                }
                else
                {
                    //                players = new JSONObject();
                    //                players.put("player1", player1Data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private static JSONObject ReformatMultiPlayerMatchData(JSONObject matchData)
        {
            var data = new JSONObject();

            try
            {
                var ownData = new JSONObject();
                ownData.Add("id", matchData["id"]);
                ownData.Add("name", matchData["name"]);
                ownData.Add("applicant", matchData["applicant"].AsBool);

                if (matchData.HasKeyNotNull("image"))
                {
                    var image = matchData["image"].AsObject;
                    image.Add("id", image["id"]);
                    ownData.Add("image", image);
                }

                if (matchData.HasKeyNotNull("imageUrl"))
                {
                    ownData.Add("imageUrl", matchData["imageUrl"]);
                }

                var opponent = matchData["opponentData"].AsObject;
                var opponentData = new JSONObject();
                opponentData.Add("id", opponent["id"]);
                opponentData.Add("name", opponent["name"]);
                opponentData.Add("applicant", !matchData["applicant"].AsBool);
                opponentData.Add("peerId", opponent["sessionId"]);

                if (opponent.HasKeyNotNull("image"))
                {
                    var image = opponent["image"].AsObject;
                    image.Add("id", image["id"]);
                    opponentData.Add("image", image);
                }

                if (opponent.HasKeyNotNull("imageUrl"))
                {
                    opponentData.Add("imageUrl", opponent["imageUrl"]);
                }

                data.Add("matchId", matchData["matchId"]);
                data.Add("gameId", matchData["gameId"]);
                data.Add("leagueId", matchData["leagueId"]);
                data.Add("isQuick", matchData["isQuick"].AsBool);
                data.Add("isReload", false);
                data.Add("leagueName", matchData["leagueName"]);
                data.Add("gameName", matchData["gameName"]);
                data.Add("platform", matchData["platform"]);
                data.Add("packageName", matchData["packageName"]);
                data.Add("ownData", ownData);

                var opponents = new JSONArray();
                opponents.Add(opponentData);
                data.Add("opponentsData", opponents);
                data.Add("opponentData", opponentData);
                data.Add("isMultiPlayer", matchData["isMultiPlayer"].AsBool);

                if (matchData.HasKeyNotNull("config"))
                {
                    var cfg = JSON.Parse(matchData["config"].ToString()).AsObject;
                    data.Add("config", cfg);
                }

                if (matchData.HasKeyNotNull("webUrl"))
                {
                    data.Add("webUrl", matchData["webUrl"]);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            return data;
        }

        private void OnReceiveRequestIdStateAction(JSONObject Params)
        {
//            Debug.Log("onReceiveRequestIdStateAction - 0 " + Params);
            try
            {
                var message = "";
                var rejectReasonType = Params["rejectReasonType"].AsInt;
                switch (rejectReasonType)
                {
                    case MatchRequestRejectTypes.UserNotAccept:

                        if (Params.HasKeyNotNull("rejectMessage") && Params["rejectMessage"].ToString().Length > 0)
                        {
                            message = Params["rejectMessage"];
                        }
                        else
                        {
                            message = MatchRequestRejectTypes.GetMessage(MatchRequestRejectTypes.UserNotAccept,
                                LanguageTypes.Fa);
                        }

                        break;

                    case MatchRequestRejectTypes.AppNotInstalled:
                        if (Params.HasKeyNotNull("rejectMessage") && Params["rejectMessage"].ToString().Length > 0)
                        {
                            message = Params["rejectMessage"];
                        }
                        else
                        {
                            message = MatchRequestRejectTypes.GetMessage(MatchRequestRejectTypes.AppNotInstalled,
                                LanguageTypes.Fa);
                        }

                        break;

                    case MatchRequestRejectTypes.UserVersionConflict:
                        if (Params.HasKeyNotNull("rejectMessage") && Params["rejectMessage"].ToString().Length > 0)
                        {
                            message = Params["rejectMessage"];
                        }
                        else
                        {
                            message = MatchRequestRejectTypes.GetMessage(MatchRequestRejectTypes.UserVersionConflict,
                                LanguageTypes.Fa);
                        }

                        break;

                    case MatchRequestRejectTypes.UserIsBusy:
                        if (Params.HasKeyNotNull("rejectMessage") && Params["rejectMessage"].ToString().Length > 0)
                        {
                            message = Params["rejectMessage"];
                        }
                        else
                        {
                            message = MatchRequestRejectTypes.GetMessage(MatchRequestRejectTypes.UserIsBusy,
                                LanguageTypes.Fa);
                        }

                        break;
                }

//                Debug.Log("onReceiveRequestIdStateAction - 1 " + message);
                string requestId = Params["requestId"];
                var state = Params["state"].AsBool;
                var uiData = new JSONObject();
                uiData.Add("requestId", requestId);
                uiData.Add("state", state);
                uiData.Add("rejectMessage", message);

                string leagueId = Params["leagueId"];

                var leagueData = _activeMatchRequest[leagueId];
                var requestData = (Dictionary<string, object>) leagueData[requestId];
//                Debug.Log("onReceiveRequestIdStateAction - 2 " + requestData);
                if (requestData != null)
                {
                    var res = (MatchRequestCallback) requestData["callback"];
                    var data = new JSONObject();
                    data.Add("requestId", requestId);
                    if (state)
                    {
                        res.OnAccept(data);
                    }
                    else
                    {
                        data.Add("rejectMessage", message);
                        res.OnReject(data);
                    }

//                    Debug.Log("onReceiveRequestIdStateAction - 3 " + state);
                    Util.ClearTimeout((string) requestData["timeoutId"]);
                    _currentMatchRequestCount -= 1;
                    leagueData.Remove(requestId);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }
        
        private bool IsGameRun()
        {
            return false;
        }
        
        private JSONObject ReformatMatchObject(JSONObject match)
        {
            var newMatchObj = new JSONObject();

            newMatchObj.Add("endTime", match["EndTimestamp"]);
            newMatchObj.Add("startTime", match["StartTimestamp"]);
            newMatchObj.Add("id", match["ID"].ToString());
            newMatchObj.Add("statusNumber", match["StatusNumber"]);
            newMatchObj.Add("league", ReformatMiniLeagueObject(match["League"].AsObject));

            var users = match["Users"].AsArray;
            var usersList = new JSONArray();

            for (var i = 0; i < users.Count; i++)
            {
                var usr = users[i].AsObject;
                var refUsr = new JSONObject();

                refUsr.Add("id", usr["ID"].ToString());
                refUsr.Add("name", usr["Name"]);
                refUsr.Add("firstName", usr["FirstName"]);
                refUsr.Add("lastName", usr["LastName"]);
                refUsr.Add("nickName", usr["NickName"]);
                refUsr.Add("username", usr["Username"]);
                if (usr.HasKeyNotNull("ProfileImage"))
                {
                    refUsr.Add("imageUrl", usr["ProfileImage"]);
                }
                else
                {
                    refUsr.Add("imageUrl", null);
                }


                usersList.Add(refUsr);
            }

            newMatchObj.Add("users", usersList);

            return newMatchObj;
        }
        
        private void QuickRequest(JSONObject requestData, bool isRepeat, QuickMatchRequestCallback res)
        {
            Debug.Log("quickRequest_0 " + requestData);
            Request(RequestUrls.RequestQuickMatch, requestData, result =>
            {
                try
                {
                    var hasError = result["HasError"].AsBool;
                    if (!isRepeat)
                    {
                        var returnData = new JSONObject();
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);
                        res.OnResult(returnData);
                    }

                    var leaguesQuickData =
                        (Dictionary<string, object>) _quickMatchData["leagues"];
                    var quickData =
                        (Dictionary<string, object>) leaguesQuickData[requestData["leagueId"]];


                    Debug.Log("quickRequest_1 " + quickData);
                    Debug.Log("quickRequest_2 " + requestData["leagueId"]);
                    if (!hasError)
                    {
                        var timeoutId = Util.SetTimeout(() =>
                        {
                            if ((bool) quickData["state"])
                            {
                                QuickRequest(requestData, true, res);
                            }
                        }, ConfigData.Qmt);

                        quickData.Add("timeoutId", timeoutId);
                    }
                    else
                    {
                        if (isRepeat)
                        {
                            var resTemp = (QuickMatchRequestCallback) quickData["callback"];
                            var cancelData = new JSONObject();
                            cancelData.Add("leagueId", requestData["leagueId"]);
                            cancelData.Add("message", _dic["NOTOPPONENTFIND"].AsObject[_lang]);

                            Debug.LogError("something is not implemented here");
                            // todo: think about this later
                            // cancelData.Add("state", quickData["state"]);

                            resTemp.OnCancel(cancelData);
                        }

                        _quickMatchData.Add("requestCount", (int) _quickMatchData["requestCount"] - 1);
                        _quickMatchData.Add("lastLeagueId", null);
                        quickData.Add("state", false);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                    res.OnResult(ExceptionErrorData(e));
                }
            });
        }
        
        private void OnReceiveDataPackAction(JSONObject Params, AsyncResponse res)
        {
            Debug.Log("onReceiveDataPackAction_0 " + Params);
            try
            {
                var game = _activeMatch[Params["matchId"]];

                if (game != null)
                {
                    game._onReceiveData(Params, res);
                }
                else
                {
                    Debug.Log("onReceiveDataPackAction_1 ");
                    res.Call();
                }

                FireEvents("matchReceiveData", Params);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }
    }
}

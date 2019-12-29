using System;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity
{
    public partial class Service {
        
        /**
         * <div style='width: 100%;text-align: right'> دریافت اطلاعات جدول رده بندی </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *       JSONObject reqData = new JSONObject();
         *       reqData.put("leagueId", "3");
         *
         *       service.getTableData(reqData, new RequestCallback() {
         *           {@code @Override}
         *           public void onResult(JSONObject result) {
         *               System.out.println("getTableData method : " + result);
         *           }
         *       });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId - شناسه بازی</li>
         *          <li>{Integer} [rangeType] - زمان جدول
         *              <p>     1 = ۱ ماهه </p>
         *              <p>     3 =    کلی </p>
         *          </li>
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
         *                  <li>{Integer} type
         *                      <ul>
         *                          <li>0 = لیگ دوره ای</li>
         *                          <li>1 = لیگ حذفی</li>
         *                      </ul>
         *                  </li>
         *                  <li>{JSONArray} headerData - نام فیلد های جدول در لیگ دوره ای</li>
         *                  <li>{JSONArray} usersData  - اطلاعات جدول در لیگ دوره ای</li>
         *                  <li>{JSONObject} rounds - اطلاعات مرحله ها در لیگ حذفی</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetTableData(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string leagueId = (Params.HasKey("leagueId") && Params["leagueId"] != null) ? Params["leagueId"] : null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId not exist in Params");
                }

                var requestData = new JSONObject();
                requestData.Add("leagueId", leagueId);

                if (Params.HasKey("rangeType") && Params["rangeType"] != null)
                {
                    requestData.Add("rangeType", Params["rangeType"].AsInt);
                }

                if (Params.HasKey("setting") && Params["setting"] != null)
                {
                    var setting = Params["setting"].AsObject;
                    if (setting != null)
                    {
                        if (setting.HasKey("imageWidth"))
                        {
                        }

                        if (setting.HasKey("imageHeight"))
                        {
                        }
                    }
                }

                Request(RequestUrls.Table, requestData, result =>
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
                            returnData.Add("result",
                                ReformatTableObject(result["Result"].AsObject));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'> دریافت میزان جایزه لیگ </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId", leagueId);
         *
         *      service.getLeagueAwards(reqData, new RequestCallback() {
         *         {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeagueAwards method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId - شناسه لیگ</li>
         *      </ul>
         *
         * @param  callback
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONArray} result -  Array of JSONObject that contain :
         *              <ul>
         *                  <li>{Integer} rank - رتبه دریافت کننده جایزه</li>
         *                  <li>{string} description - توضیحات</li>
         *                  <li>{Long} value - میزان جایزه</li>
         *                  <li>{string} textValue - میزان جایزه به همراه واحد آن</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLeagueAwards(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string leagueId = (Params.HasKey("leagueId") && Params["leagueId"] != null) ? Params["leagueId"] : null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId not exist in Params");
                }

                Request(RequestUrls.LeagueAwards, Params, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        if (!hasError && result.HasKey("Result") && result["Result"] != null)
                        {
                            var retResult = new JSONArray();
                            var awards = result["Result"].AsArray;
                            for (var i = 0; i < awards.Count; i++)
                            {
                                var value = awards[i].AsObject["value"].AsDouble / ConfigData.Cf;
                                var textValue = value + "   " + ConfigData.Cu;
                                var award = new JSONObject();
                                award.Add("rank", i + 1);
                                award.Add("value", value);
                                award.Add("textValue", textValue);
                                award.Add("description", "جایزه نفر " + (i + 1) + (" ") + textValue);
                                retResult.Add(award);
                            }

                            returnData.Add("result", retResult);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         *
         * <div style='width: 100%;text-align: right'> دریافت اطلاعات لیگ </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId", "2");
         *      reqData.put("size", 10);
         *
         *      service.getLeaguesInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeaguesInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} [gameId] - شناسه بازی</li>
         *          <li>{string} [leagueId] - شناسه لیگ</li>
         *          <li>{JSONArray} [leaguesId] -ها شناسه لیگ</li>
         *          <li>{Integer} [size=5]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{string} [name] - نام لیگ</li>
         *          <li>{Integer} [prize=0] - نوع لیگ از نظر جایزه
         *              <p>    0 = تمام موارد</p>
         *              <p>    1 = جایزه دار</p>
         *              <p>    2 = عدم داشتن جایزه</p>
         *          </li>
         *          <li>{Integer} [status=0] - نوع لیگ از نظر وضعیت
         *              <p>    0 = تمام موارد</p>
         *              <p>    1 = شروع نشده</p>
         *              <p>    2 = در حال ثبت نام</p>
         *              <p>    3 = در حال اجرا</p>
         *              <p>    4 = تمام شده</p>
         *              <p>    5 = در حال اجرا و ثبت نام</p>
         *              <p>    6 = حساب رسی شذه</p>
         *              <p>    7 = رد شده</p>
         *              <p>    8 = کنسل شده</p>
         *              <p>    9 =در حال بررسی</p>
         *              <p>    10 = نیاز به تغییرات</p>
         *          </li>
         *          <li>{JSONArray} [statusList] - وضعیت های لیگ </li>
         *          <li>{Integer} [financialType=0] - نوع لیگ بر اساس حق عضویت
         *              <p>    0 = تمام موارد</p>
         *              <p>    1 = رایگان</p>
         *              <p>    2 = پولی</p>
         *          </li>
         *          <li>{Integer} [userState=0] - نوع لیگ بر اساس وضعیت کاربر
         *              <p>     0 = تمام موارد</p>
         *              <p>     1 = عضو شده</p>
         *              <p>     2 =</p>
         *              <p>     3 = عضو نشده</p>
         *          </li>
         *          <li>{bool} [showDefault=true] - وجود یا عدم وجود لیگ پیش فرض</li>
         *          <li>{bool} [newest=false] - دریافت جدیدترین لیگ ها</li>
         *          <li>{string} [lobbyId] - شناسه دسته بازی</li>
         *          <li> {JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر لیگ</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر لیگ</li>
         *              </ul>
         *          </li>
         *      </ul>
         * @param  callback
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONArray} result
         *              <ul>
         *                  <li>{JSONArray} leagues
         *                      <ul>
         *                          <li>{string} id </li>
         *                          <li>{string} enrollUrl</li>
         *                          <li>{bool} isMember </li>
         *                          <li>{bool} isFollower</li>
         *                          <li>{JSONObject} userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{string} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} image
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} gameType</li>
         *                          <li>{Integer} playerType</li>
         *                          <li>{Integer} status</li>
         *                          <li>{Integer} financialType</li>
         *                          <li>{Integer} lobbyId</li>
         *                          <li>{Integer} maxPlayers</li>
         *                          <li>{Integer} minNoOfPlayedGames</li>
         *                          <li>{Integer} minPlayers</li>
         *                          <li>{Integer} memberCount</li>
         *                          <li>{Integer} playerNumberType</li>
         *                          <li>{Integer} timestamp</li>
         *                          <li>{string} creator</li>
         *                          <li>{Integer} memberCount</li>
         *                          <li>{Integer} availableCount</li>
         *                          <li>{Integer} discount</li>
         *                          <li>{Integer} numOfComments</li>
         *                          <li>{Integer} numOfFavorites</li>
         *                          <li>{Integer} numOfLikes</li>
         *                          <li>{Integer} type</li>
         *                          <li>{Integer} [endTime]</li>
         *                          <li>{Integer} [startTime]</li>
         *                          <li>{string} rules</li>
         *                          <li>{string} description</li>
         *                          <li>{string} name</li>
         *                          <li>{string} ThreadId</li>
         *                          <li>{string} timelineId</li>
         *                          <li>{bool} hasPrize</li>
         *                          <li>{bool} quickMatch</li>
         *                          <li>{bool} startTypeCapacityComplete</li>
         *                          <li>{bool} startTypeFromDate</li>
         *                          <li>{bool} startTypePublishDate</li>
         *                          <li>{bool} canComment</li>
         *                          <li>{bool} canLike</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} hide</li>
         *                          <li>{Double} price</li>
         *                          <li>{JSONArray} attributeValues</li>
         *                          <li>{JSONArray} categoryList</li>
         *                          <li>{JSONObject} business</li>
         *                          <li>{JSONObject} rate</li>
         *                          <li>{JSONArray} games</li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLeaguesInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = (Params.HasKey("gameId") && Params["gameId"] != null) ? Params["gameId"] : null;
                string lobbyId = (Params.HasKey("lobbyId") && Params["lobbyId"] != null) ? Params["lobbyId"] : null;

                if (gameId != null)
                {
                    requestData.Add("gameId", gameId);
                }

                var requestUrl = RequestUrls.GetLeague;
                string leagueId = (Params.HasKey("leagueId") && Params["leagueId"] != null) ? Params["leagueId"] : null;
                var leaguesId = (Params.HasKey("leaguesId") && Params["leaguesId"] != null)
                    ? Params["leagueId"].AsArray
                    : null;

                if (lobbyId != null)
                {
                    requestData.Add("lobbyId", lobbyId);
                }

                if (leagueId != null || leaguesId != null)
                {
                    if (leaguesId != null)
                    {
                        requestData.Add("leaguesId", leaguesId);
                    }

                    if (leagueId != null)
                    {
                        requestData.Add("leagueId", leagueId);
                    }
                }
                else
                {
                    int? size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"] : null;
                    int? offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"] : null;

                    if (size != null)
                    {
                        requestData.Add("size", size);
                    }
                    else
                    {
                        requestData.Add("size", ConfigData.Gldc);
                    }

                    if (offset != null)
                    {
                        requestData.Add("offset", offset);
                    }
                    else
                    {
                        requestData.Add("offset", 0);
                    }

                    string nameParam = (Params.HasKey("name") && Params["name"] != null) ? Params["name"] : null;
                    var prize = (Params.HasKey("prize") && Params["prize"] != null)
                        ? Params["prize"].AsInt
                        : (int?) null;
                    var userState = (Params.HasKey("userState") && Params["userState"] != null)
                        ? Params["userState"].AsInt
                        : (int?) null;
                    var status = (Params.HasKey("status") && Params["status"] != null)
                        ? Params["status"].AsInt
                        : (int?) null;
                    var statusList = (Params.HasKey("statusList") && Params["statusList"] != null)
                        ? Params["statusList"].AsArray
                        : null;
                    var financialType = (Params.HasKey("financialType") && Params["financialType"] != null)
                        ? Params["financialType"].AsInt
                        : (int?) null;
                    var showDefault = (Params.HasKey("showDefault") && Params["showDefault"] != null)
                        ? Params["showDefault"].AsBool
                        : (bool?) null;

                    if (nameParam != null)
                    {
                        requestData.Add("filter", nameParam);
                    }

                    if (prize != null)
                    {
                        if (prize == 1)
                        {
                            requestData.Add("hasPrize", true);
                        }
                        else if (prize == 2)
                        {
                            requestData.Add("hasPrize", false);
                        }
                    }

                    if (userState != null)
                    {
                        if (userState == 1)
                        {
                            requestData.Add("mine", true);
                        }
                        else if (userState == 3)
                        {
                            requestData.Add("mine", false);
                        }
                    }

                    if (status != null)
                    {
                        requestData.Add("status", status);
                    }
                    else if (statusList != null)
                    {
                        requestData.Add("statusList", statusList.ToString());
                    }
                    else
                    {
                        requestData.Add("status", 0);
                    }

                    if (showDefault != null)
                    {
                        requestData.Add("showDefault", showDefault);
                    }

                    if (financialType != null)
                    {
                        requestData.Add("financialType", financialType);
                    }
                    else
                    {
                        requestData.Add("financialType", 0);
                    }
                }

                Request(requestUrl, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError",
                            hasError);
                        returnData.Add("errorMessage",
                            result["ErrorMessage"]);
                        returnData.Add("errorCode",
                            result["ErrorCode"].AsInt);

                        if (!hasError)
                        {
                            var leagues = result["Result"].AsArray;
                            var refactorLeagues = new JSONArray();
                            for (var i = 0; i < leagues.Count; i++)
                            {
                                refactorLeagues.Add(ReformatLeagueObject(leagues[i].AsObject));
                            }

                            var retResult = new JSONObject();
                            retResult.Add("leagues", refactorLeagues);
                            if (requestData.HasKey("size"))
                            {
                                retResult.Add("hasNext", requestData["size"].AsInt == leagues.Count);
                                retResult.Add("nextOffset", requestData["offset"].AsInt + leagues.Count);
                            }

                            retResult.Add("count", result["Count"].AsInt);
                            returnData.Add("result", retResult);
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
         *
         * <div style='width: 100%;text-align: right'>دریافت لیگ های مرتبط </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId", "3");
         *      reqData.put("size", 10);
         *      reqData.put("type", 1);
         *
         *      service.getRelatedLeaguesInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeaguesInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId - شناسه لیگ</li>
         *          <li>{Integer} [size=10]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{Integer} [type] - نوع لیگ
         *              <p>     1 = لیگ هایی که در یک لابی می باشند</p>
         *              <p>     2 = لیگ هایی که سازنده آنها یکی است</p>
         *          </li>
         *          <li> {JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر لیگ</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر لیگ</li>
         *              </ul>
         *          </li>
         *          <li></li>
         *       </ul>
         * @param  callback
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} CategoryEquality : $ - لیگ هایی که در یک لابی می باشند</li>
         *                  <li>{JSONArray} CreatorEquality : $ - لیگ هایی که سازنده آنها یکی است
         *                      <ul>
         *                          <li>{string} $.id</li>
         *                          <li>{string} $.enrollUrl</li>
         *                          <li>{bool} $.isMember</li>
         *                          <li>{bool} $.isFollower</li>
         *                          <li>{JSONObject} $.userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{string} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} $.image
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} $.gameType</li>
         *                          <li>{Integer} $.playerType</li>
         *                          <li>{Integer} $.status</li>
         *                          <li>{Integer} $.financialType</li>
         *                          <li>{Integer} $.lobbyId</li>
         *                          <li>{Integer} $.maxPlayers</li>
         *                          <li>{Integer} $.minNoOfPlayedGames</li>
         *                          <li>{Integer} $.minPlayers</li>
         *                          <li>{Integer} $.memberCount</li>
         *                          <li>{Integer} $.playerNumberType</li>
         *                          <li>{Integer} $.timestamp</li>
         *                          <li>{string} $.creator</li>
         *                          <li>{Integer} $.memberCount</li>
         *                          <li>{Integer} $.availableCount</li>
         *                          <li>{Integer} $.discount</li>
         *                          <li>{Integer} $.numOfComments</li>
         *                          <li>{Integer} $.numOfFavorites</li>
         *                          <li>{Integer} $.numOfLikes</li>
         *                          <li>{Integer} $.type</li>
         *                          <li>{Integer} [$.endTime]</li>
         *                          <li>{Integer} [$.startTime]</li>
         *                          <li>{string} $.rules</li>
         *                          <li>{string} $.description</li>
         *                          <li>{string} $.name</li>
         *                          <li>{string} $.ThreadId</li>
         *                          <li>{string} $.timelineId</li>
         *                          <li>{bool} $.hasPrize</li>
         *                          <li>{bool} $.quickMatch</li>
         *                          <li>{bool} $.startTypeCapacityComplete</li>
         *                          <li>{bool} $.startTypeFromDate</li>
         *                          <li>{bool} $.startTypePublishDate</li>
         *                          <li>{bool} $.canComment</li>
         *                          <li>{bool} $.canLike</li>
         *                          <li>{bool} $.enable</li>
         *                          <li>{bool} $.hide</li>
         *                          <li>{Double} $.price</li>
         *                          <li>{JSONArray} $.attributeValues</li>
         *                          <li>{JSONArray} $.categoryList</li>
         *                          <li>{JSONObject} $.business</li>
         *                          <li>{JSONObject} $.rate</li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} result.hasNext</li>
         *                  <li>{Integer} result.nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetRelatedLeaguesInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string leagueId = (Params.HasKey("leagueId") && Params["leagueId"] != null) ? Params["leagueId"] : null;
                var type = (Params.HasKey("type") && Params["type"] != null) ? Params["type"].AsInt : (int?) null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId not exist in Params");
                }

                var requestData = new JSONObject();

                requestData.Add("leagueId", leagueId);
                if (type != null)
                {
                    requestData.Add("type", type);
                }

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : (int?) null;
                var offset = (Params.HasKey("offset") && Params["offset"] != null)
                    ? Params["offset"].AsInt
                    : (int?) null;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetRelatedLeague, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError",
                            hasError);
                        returnData.Add("errorMessage",
                            result["ErrorMessage"]);
                        returnData.Add("errorCode",
                            result["ErrorCode"].AsInt);
                        if (!hasError)
                        {
                            var allResult = result["Result"].AsObject;
                            var retResult = new JSONObject();
                            var hasNext = false;

                            foreach (var key in allResult.Keys)
                            {
                                if (allResult.HasKey(key) && allResult[key] != null)
                                {
                                    var aLeagues = allResult[key].AsArray;
                                    var leagues = new JSONArray();
                                    if (aLeagues != null)
                                    {
                                        for (var j = 0; j < aLeagues.Count; j++)
                                        {
                                            var info = aLeagues[j].AsObject;
                                            leagues.Add(ReformatLeagueObject(info));
                                        }

                                        if (size == leagues.Count)
                                        {
                                            hasNext = true;
                                        }

                                        retResult.Add(key, leagues);
                                    }
                                }
                            }

                            retResult.Add("hasNext", hasNext);
                            retResult.Add("nextOffset", offset + size);
                            returnData.Add("result", retResult);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         *
         * <div style='width: 100%;text-align: right'> دریافت برترین لیگ ها</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("type", type);
         *
         *      service.getTopLeaguesInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getTopLeaguesInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{Integer} [type]
         *              <p>     1 = برترین های گیم سنتر</p>
         *              <p>     8 = پیشنهاد گیم سنتر</p>
         *          </li>
         *          <li>{Integer} [size=5]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{string} [gameId]</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر لیگ</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر لیگ</li>
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
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} GcSuggestion : $ - پیشنهاد گیم سنتر</li>
         *                  <li>{Integer} GcSuggestionCount</li>
         *                  <li>{JSONArray} GcTop : $ - برترین های گیم سنتر</li>
         *                  <li>{JSONArray} GcTopCount</li>
         *                  <li>{JSONArray} TopFollow : $ - برترین دنبال شده ها</li>
         *                  <li>{JSONArray} TopFollowCount</li>
         *                  <li>{JSONArray} TopRateCount
         *                  <li>{JSONArray} TopRate : $ - برترین ها
         *                      <ul>
         *                          <li>{string} $.id</li>
         *                          <li>{string} $.enrollUrl</li>
         *                          <li>{bool} $.isMember</li>
         *                          <li>{bool} $.isFollower</li>
         *                          <li>{JSONObject} $.userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{string} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} $.image
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} $.gameType</li>
         *                          <li>{Integer} $.playerType</li>
         *                          <li>{Integer} $.status</li>
         *                          <li>{Integer} $.financialType</li>
         *                          <li>{Integer} $.lobbyId</li>
         *                          <li>{Integer} $.maxPlayers</li>
         *                          <li>{Integer} $.minNoOfPlayedGames</li>
         *                          <li>{Integer} $.minPlayers</li>
         *                          <li>{Integer} $.memberCount</li>
         *                          <li>{Integer} $.playerNumberType</li>
         *                          <li>{Integer} $.timestamp</li>
         *                          <li>{string} $.creator</li>
         *                          <li>{Integer} $.memberCount</li>
         *                          <li>{Integer} $.availableCount</li>
         *                          <li>{Integer} $.discount</li>
         *                          <li>{Integer} $.numOfComments</li>
         *                          <li>{Integer} $.numOfFavorites</li>
         *                          <li>{Integer} $.numOfLikes</li>
         *                          <li>{Integer} $.type</li>
         *                          <li>{Integer} [$.endTime]</li>
         *                          <li>{Integer} [$.startTime]</li>
         *                          <li>{string} $.rules</li>
         *                          <li>{string} $.description</li>
         *                          <li>{string} $.name</li>
         *                          <li>{string} $.ThreadId</li>
         *                          <li>{string} $.timelineId</li>
         *                          <li>{bool} $.hasPrize</li>
         *                          <li>{bool} $.quickMatch</li>
         *                          <li>{bool} $.startTypeCapacityComplete</li>
         *                          <li>{bool} $.startTypeFromDate</li>
         *                          <li>{bool} $.startTypePublishDate</li>
         *                          <li>{bool} $.canComment</li>
         *                          <li>{bool} $.canLike</li>
         *                          <li>{bool} $.enable</li>
         *                          <li>{bool} $.hide</li>
         *                          <li>{Double} $.price</li>
         *                          <li>{JSONArray} $.attributeValues</li>
         *                          <li>{JSONArray} $.categoryList</li>
         *                          <li>{JSONObject} $.business</li>
         *                          <li>{JSONObject} $.rate</li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetTopLeaguesInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var type = (Params.HasKey("type") && Params["type"] != null) ? Params["type"].AsInt : (int?) null;
                string gameId = (Params.HasKey("gameId") && Params["gameId"] != null) ? Params["gameId"] : null;

                var requestData = new JSONObject();

                if (type != null)
                {
                    requestData.Add("type", type);
                }

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : 10;
                var offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (gameId != null)
                {
                    requestData.Add("gameId", gameId);
                }

                Request(RequestUrls.GetTopLeague, requestData, result =>
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
                            var allResult = result["Result"].AsObject;

                            string[] keys = {"GcSuggestion", "GcTop", "TopFollow", "TopRate"};
                            var refactorLeagues = new JSONObject();
                            for (var i = 0; i < keys.Length; i++)
                            {
                                var key = keys[i];

                                if (allResult.HasKey(key) && allResult[key] != null)
                                {
                                    var leagues = allResult[key].AsArray;
                                    var refLeagues = new JSONArray();

                                    if (leagues != null)
                                    {
                                        for (var j = 0; j < leagues.Count; j++)
                                        {
                                            var info = leagues[j].AsObject;
                                            refLeagues.Add(
                                                ReformatLeagueObject(info));
                                        }

                                        refactorLeagues.Add(key, refLeagues);
                                        var countKeyName = key + "Count";
                                        refactorLeagues.Add(countKeyName, allResult[countKeyName]);
                                    }
                                }
                            }

                            returnData.Add("result", refactorLeagues);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                        returnData = ExceptionErrorData(e);
                    }

                    onResult(returnData);
                });
            }
            catch (Exception e)
            {
                Debug.LogError("exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        /**
         *
         * <div style='width: 100%;text-align: right'> دریافت آخرین لیگ ها</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("size", 10);
         *
         *      service.getLatestLeaguesInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLatestLeaguesInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{Integer} [size=10]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li> {JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر لیگ</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر لیگ</li>
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
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} leagues
         *                      <ul>
         *                          <li>{string} id</li>
         *                          <li>{string} enrollUrl</li>
         *                          <li>{bool} isMember</li>
         *                          <li>{bool} isFollower</li>
         *                          <li>{JSONObject} $.userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{string} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} $.image
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} gameType</li>
         *                          <li>{Integer} playerType</li>
         *                          <li>{Integer} status</li>
         *                          <li>{Integer} financialType</li>
         *                          <li>{Integer} lobbyId</li>
         *                          <li>{Integer} maxPlayers</li>
         *                          <li>{Integer} minNoOfPlayedGames</li>
         *                          <li>{Integer} minPlayers</li>
         *                          <li>{Integer} memberCount</li>
         *                          <li>{Integer} playerNumberType</li>
         *                          <li>{Integer} timestamp</li>
         *                          <li>{string} creator</li>
         *                          <li>{Integer} memberCount</li>
         *                          <li>{Integer} availableCount</li>
         *                          <li>{Integer} discount</li>
         *                          <li>{Integer} numOfComments</li>
         *                          <li>{Integer} numOfFavorites</li>
         *                          <li>{Integer} numOfLikes</li>
         *                          <li>{Integer} type</li>
         *                          <li>{Integer} [endTime]</li>
         *                          <li>{Integer} [startTime]</li>
         *                          <li>{string} rules</li>
         *                          <li>{string} description</li>
         *                          <li>{string} name</li>
         *                          <li>{string} ThreadId</li>
         *                          <li>{string} timelineId</li>
         *                          <li>{bool} hasPrize</li>
         *                          <li>{bool} quickMatch</li>
         *                          <li>{bool} startTypeCapacityComplete</li>
         *                          <li>{bool} startTypeFromDate</li>
         *                          <li>{bool} startTypePublishDate</li>
         *                          <li>{bool} canComment</li>
         *                          <li>{bool} canLike</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} hide</li>
         *                          <li>{Double} price</li>
         *                          <li>{JSONArray} attributeValues</li>
         *                          <li>{JSONArray} categoryList</li>
         *                          <li>{JSONObject} business</li>
         *                          <li>{JSONObject} rate</li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLatestLeaguesInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : 10;
                var offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetLatestLeague, requestData, result =>
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
                            var leagues = result["Result"].AsArray;
                            var refactorLeagues = new JSONArray();

                            for (var i = 0; i < leagues.Count; i++)
                            {
                                refactorLeagues.Add(ReformatLeagueObject(leagues[i].AsObject));
                            }

                            var retResult = new JSONObject();
                            retResult.Add("leagues", refactorLeagues);
                            if (requestData.HasKey("size"))
                            {
                                retResult.Add("hasNext", size == leagues.Count);
                                retResult.Add("nextOffset", offset + leagues.Count);
                            }


                            returnData.Add("result", retResult);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'> دریافت اعضای لیگ</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId", "3");
         *      reqData.put("offset", 0);
         *      reqData.put("size",10 );
         *
         *      service.getLeagueMembers(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeagueMembers method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId</li>
         *          <li>{Integer} [size=20]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{Integer} [userState=0] - وضعیت کاربر
         *              <p>     0 = تمامی کاربران</p>
         *              <p>     1 = کاربران آنلاین</p>
         *              <p>     2 = کاربران آفلاین</p>
         *          </li>
         *          <li>{string} [name]</li>
         *          <li> {JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر لیگ</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر لیگ</li>
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
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONObject} users - کلید های این آبجکت شناسه کاربر می باشد
         *                      <ul>
         *                          <li>{string} name</li>
         *                          <li>{bool} isOnline</li>
         *                          <li>{JSONObject} [image]
         *                              <ul>
         *                                  <li>{string}  id</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                                  <li>{string}  url</li>
         *                              </ul>
         *                          </li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLeagueMembers(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string leagueId = (Params.HasKey("leagueId") && Params["leagueId"] != null) ? Params["leagueId"] : null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId not exist in Params");
                }

                var requestData = new JSONObject();
                requestData.Add("leagueId", leagueId);

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : 10;
                var offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

//                int? imageWidth = null;
//                int? imageHeight = null;

                if (Params.HasKey("setting") && Params["setting"] != null)
                {
                    var setting = Params["setting"].AsObject;
                    if (setting != null)
                    {
                        if (setting.HasKey("imageWidth"))
                        {
//                            imageWidth = setting["imageWidth"].AsInt;
                        }

                        if (setting.HasKey("imageHeight"))
                        {
//                            imageHeight = setting["imageHeight"].AsInt;
                        }
                    }
                }

                //            int? finalImageWidth = imageWidth;
                //            int? finalImageHeight = imageHeight;


                if (Params.HasKey("userState") && Params["userState"] != null)
                {
                    requestData.Add("online", Params["userState"].AsInt == 1);
                }


                if (Params.HasKey("name") && Params["name"] != null)
                {
                    requestData["filter"] = Params["name"];
                }

                Request(RequestUrls.LeagueMembers, requestData, result =>
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
                            var members = result["Result"].AsArray;
                            var users = new JSONObject();

                            if (members != null)
                            {
                                //                                string ownId = userData.get("id").ToString();
                                for (var i = 0; i < members.Count; i++)
                                {
                                    var member = members[i].AsObject;
                                    var memberId = member["UserID"].ToString();
                                    //                                    if (memberId.equals(ownId) ) {
                                    //                                        continue;
                                    //                                    }
                                    var memberData = new JSONObject();
                                    memberData.Add("name", member["Name"]);
                                    memberData.Add("isOnline", member["IsOnline"].AsBool);

                                    if (member.HasKey("Image") && member["Image"] != null)
                                    {
                                        var image = member["Image"].AsObject;
                                        image.Add("id", image["id"].ToString());
                                        memberData.Add("image", image);
                                    }


                                    if (member.HasKey("ProfileImage") && member["ProfileImage"] != null)
                                    {
                                        memberData.Add("imageUrl", member["ProfileImage"]);
                                    }

                                    users.Add(memberId, memberData);
                                }
                            }


                            var retResult = new JSONObject();
                            retResult.Add("users", users);
                            retResult.Add("hasNext", members != null && requestData["size"].AsInt == members.Count);
                            if (members != null)
                                retResult.Add("nextOffset", requestData["offset"].AsInt + members.Count);
                            retResult.Add("count", result["Count"].AsInt);
                            returnData.Add("result", retResult);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
     * <div style='width: 100%;text-align: right'> درخواست عضویت در لیگ</div>
     * <pre>
     *  <code style='float:right'>نمونه کد</code>
     *  <code>
     *      JSONObject reqData = new JSONObject();
     *      reqData.put("gameId", "2");
     *      reqData.put("leagueId", "3");
     *      service.getLeaguesInfo(reqData, new RequestCallback() {
     *          {@code @Override}
     *          public void onResult(JSONObject result) {
     *              try {
     *                  if (!result.getBoolean("hasError")) {
     *                      JSONObject leagueData = result.getJSONObject("result").getJSONArray("leagues").getJSONObject(0);
     *                      String enrollUrl = leagueData.getString("enrollUrl");
     *                      JSONObject reqData = new JSONObject();
     *                      reqData.put("leagueId", leagueData.getString("id"));
     *                      reqData.put("enrollUrl", enrollUrl);
     *                      service.subscribeLeagueRequest(reqData, new RequestCallback() {
     *                          {@code @Override}
     *                          public void onResult(JSONObject data) {
     *                              System.out.println("subscribeLeagueRequest method : " + data);
     *                          }
     *                      });
     *                  }
     *              } catch (JSONException|ServiceException e) {
     *                  e.printStackTrace();
     *              }
     *          }
     *      });
     *  </code>
     * </pre>
     * @param  Params
     *      <ul>
     *          <li>{String} leagueId - شناسه لیگ</li>
     *          <li>{String} enrollUrl - لینک عضویت</li>
     *          <li>{String} [voucherHash] - کد تخفیف</li>
     *      </ul>
     *
     * @param  callback
     *      <p>onResult method Params is JSONObject that has</p>
     *      <ul>
     *          <li>{Boolean} hasError</li>
     *          <li>{String} errorMessage</li>
     *          <li>{Integer} errorCode</li>
     *      </ul>
     *
     * @throws ServiceException خطای پارامتر های ورودی
     * */
        public void SubscribeLeagueRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                String enrollUrl = (Params.HasKeyNotNull("enrollUrl")) ? Params["enrollUrl"] : null;
                String leagueId = (Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;
                String voucherHash = (Params.HasKey("voucherHash")) ? Params["voucherHash"] : null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    throw new ServiceException("leagueId not exist in Params");
                }

                if (enrollUrl == null)
                {
                    Debug.LogError("enrollUrl not exist in Params");
                    throw new ServiceException("enrollUrl not exist in Params");
                }


                var requestData = new JSONObject();
                requestData.Add("leagueId", leagueId);

                if (voucherHash != null && voucherHash.Length > 0)
                {
                    requestData.Add("voucherHash", voucherHash);
                }


                var queryData = Util.GetQueryStringData(enrollUrl);

                var setting = new JSONObject();

                setting.Add("uri", enrollUrl.Substring(0, enrollUrl.IndexOf("?", StringComparison.Ordinal)));
                setting.Add("url", enrollUrl);
                setting.Add("parameters", queryData);

                Request(RequestUrls.SubscribeLeague, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);
                    }
                    catch (Exception e)
                    {
                        returnData = ExceptionErrorData(e);
                    }

                    onResult(returnData);
                }, setting);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }
        
        /**
         *
         * <div style='width: 100%;text-align: right'> دریافت برترین بازیکنان لیگ</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      service.getLeagueTopPlayers(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeagueTopPlayers method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId - شناسه لیگ</li>
         *          <li>{Integer} [size=50]</li>
         *          <li>{Integer} [offset=0]</li>
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
         *                  <li>{string} columnName</li>
         *                  <li>{JSONArray} players
         *                      <ul>
         *                          <li>{Double} score - امتیاز</li>
         *                          <li>{string} id - شناسه بازیکن</li>
         *                          <li>{string} name - نام بازیکن</li>
         *                          <li>{JSONObject} [image]
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} imageUrl</li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLeagueTopPlayers(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string leagueId = Params.HasKeyNotNull("leagueId") ? Params["leagueId"] : null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId not exist in Params");
                }

                requestData.Add("leagueId", leagueId);

                var size = Params.HasKeyNotNull("size") ? Params["size"].AsInt : 10;
                var offset = Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetLeagueTopPlayers, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        if (!hasError)
                        {
                            var players = new JSONArray();
                            if (result.HasKeyNotNull("Result"))
                            {
                                var res = result["Result"].AsObject;
                                var topPlayers = res["TopPlayers"].AsArray;

                                for (var i = 0; i < topPlayers.Count; i++)
                                {
                                    var playerData = topPlayers[i].AsObject;
                                    var playerInfo = playerData["UserInfo"].AsObject;
                                    if (playerInfo.HasKeyNotNull("image"))
                                    {
                                        var imageData = playerInfo["image"].AsObject;
                                        imageData.Add("id", imageData["id"].ToString());
                                        playerInfo.Add("image", imageData);
                                    }

                                    if (playerInfo.HasKeyNotNull("profileImage"))
                                    {
                                        playerInfo.Add("imageUrl", playerInfo["profileImage"]);
                                    }

                                    playerInfo.Add("score", playerData["Score"].AsDouble);
                                    players.Add(playerInfo);
                                }

                                retResult.Add("columnName", res["ColumnName"]);
                                retResult.Add("hasNext", size == topPlayers.Count);
                                retResult.Add("nextOffset", offset + topPlayers.Count);
                                retResult.Add("count", result["Count"]);
                            }
                            else
                            {
                                retResult.Add("columnName", "");
                                retResult.Add("hasNext", false);
                                retResult.Add("nextOffset", 0);
                                retResult.Add("count", 0);
                            }

                            retResult.Add("players", players);
                        }

                        returnData.Add("result", retResult);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         *
         * <div style='width: 100%;text-align: right'> دریافت برترین بازیکنان گیم سنتر و یا لیگ</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      service.getTopPlayers(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getTopPlayers method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} [gameId] - شناسه بازی</li>
         *          <li>{Integer} [size=50]</li>
         *          <li>{Integer} [offset=0]</li>
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
         *                  <li>{Integer} count</li>
         *                  <li>{JSONArray} players
         *                      <ul>
         *                          <li>{Double} score - امتیاز</li>
         *                          <li>{string} id - شناسه بازیکن</li>
         *                          <li>{string} name - نام بازیکن</li>
         *                          <li>{JSONObject} [image]
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} imageUrl</li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetTopPlayers(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = Params.HasKeyNotNull("gameId") ? Params["gameId"] : null;

                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    return;
                    //throw new ServiceException("gameId not exist in Params");
                }

                requestData.Add("gameId", gameId);

                var size = Params.HasKeyNotNull("size") ? Params["size"].AsInt : 50;
                var offset = Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetTopPlayers, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        if (!hasError)
                        {
                            var players = new JSONArray();
                            if (result.HasKeyNotNull("Result"))
                            {
                                var topPlayers = result["Result"].AsArray;

                                for (var i = 0; i < topPlayers.Count; i++)
                                {
                                    var playerData = topPlayers[i].AsObject;
                                    var playerInfo = playerData["UserInfo"].AsObject;
                                    if (playerInfo.HasKeyNotNull("image"))
                                    {
                                        var imageData = playerInfo["image"].AsObject;
                                        imageData.Add("id", imageData["id"].ToString());
                                        playerInfo.Add("image", imageData);
                                    }

                                    if (playerInfo.HasKeyNotNull("profileImage"))
                                    {
                                        playerInfo.Add("imageUrl", playerInfo["profileImage"]);
                                        playerInfo.Remove("profileImage");
                                    }

                                    playerInfo.Add("score", playerData["Score"].AsDouble);
                                    players.Add(playerInfo);
                                }

                                retResult.Add("count", result["Count"]);
                                retResult.Add("hasNext", size == topPlayers.Count);
                                retResult.Add("nextOffset", offset + topPlayers.Count);
                            }
                            else
                            {
                                retResult.Add("count", 0);
                                retResult.Add("hasNext", false);
                                retResult.Add("nextOffset", 0);
                            }

                            retResult.Add("players", players);
                        }

                        returnData.Add("result", retResult);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         *
         * <div style='width: 100%;text-align: right'> دریافت لیست لیگ های عضو شده توسط یک کاربر </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("userId", "userId");
         *      service.getEnrolledLeagues(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getEnrolledLeagues method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} [userId] - شناسه کاربر
         *              <p>در صورتی که این فیلد پر شود, لیگ های کاربر مورد نظر برگردانده می شود و در غیر اینصورت کاربر کنونی</p>
         *          </li>
         *          <li>{Integer} [size=20]</li>
         *          <li>{Integer} [offset=0]</li>
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
         *                  <li>{JSONArray} leagues - شامل اطلاعات لیگ ها</li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetEnrolledLeagues(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string userId = Params.HasKeyNotNull("userId") ? Params["userId"] : null;

                if (userId == null)
                {
                    Debug.LogError("userId not exist in Params");
                    return;
                    //throw new ServiceException("userId not exist in Params");
                }

                requestData.Add("userId", userId);

                var size = Params.HasKeyNotNull("size") ? Params["size"].AsInt : 10;
                var offset = Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetEnrolledLeagues, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        if (!hasError)
                        {
                            var leagues = new JSONArray();
                            if (result.HasKeyNotNull("Result"))
                            {
                                var res = result["Result"].AsArray;
                                for (var i = 0; i < res.Count; i++)
                                {
                                    leagues.Add(
                                        ReformatLeagueObject(res[i].AsObject));
                                }

                                retResult.Add("hasNext", size == res.Count);
                                retResult.Add("nextOffset", offset + res.Count);
                                retResult.Add("count", result["Count"]);
                            }
                            else
                            {
                                retResult.Add("hasNext", false);
                                retResult.Add("nextOffset", 0);
                            }

                            retResult.Add("leagues", leagues);
                        }

                        returnData.Add("result", retResult);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'>تعیین اینکه آیا کاربر می تواند عضو لیگ شود یا خیر</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId","26");
         *      service.getEnrollAccess(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getEnrollAccess method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} businessId شناسه لیگ</li>
         *      </ul>
         *
         * @param  callback
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{bool} result</li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetEnrollAccess(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string leagueId = Params != null && Params.HasKeyNotNull("leagueId") ? Params["leagueId"] : null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId not exist in Params");
                }

                requestData.Add("leagueId", leagueId);

                Request(RequestUrls.GetLeagueEnrollAccess, requestData, result =>
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
                            returnData.Add("result", result["Result"]);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'>اعلام امتیاز کاربر به لیگ</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId","26");
         *      service.sendLeagueRateRequest(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("sendLeagueRateRequest method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId شناسه لیگ</li>
         *          <li>{Integer} rate امتیاز کاربر</li>
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
         *                  <li>{float} myRate</li>
         *                  <li>{rate} rate</li>
         *                  <li>{long} rateCount</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void SendLeagueRateRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string leagueId = Params != null && Params.HasKeyNotNull("leagueId") ? Params["leagueId"] : null;

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("");
                }

                requestData.Add("entityId", leagueId);

                var rate = Params != null && Params.HasKeyNotNull("rate") ? Params["rate"].AsInt : (int?) null;

                if (rate == null)
                {
                    Debug.LogError("rate not exist in Params");
                    return;
                    //throw new ServiceException("");
                }

                if (rate < 0)
                {
                    Debug.LogError("rate should be greater than 0");
                    return;
                    //throw new ServiceException("");
                }

                if (rate > 5)
                {
                    Debug.LogError("rate shouldn`t be greater than 5");
                    return;
                    //throw new ServiceException("");
                }

                requestData.Add("rate", rate);

                Request(RequestUrls.LeagueRate, requestData, result =>
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
                            returnData.Add("result", result["Result"]);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'> افزودن نظر بر روی یک لیگ </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("id","56");
         *      reqData.put("text","text");
         *      service.addLeagueCommentRequest(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("addLeagueCommentRequest method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} id شناسه لیگ </li>
         *          <li>{string} text </li>
         *      </ul>
         *
         * @param  callback
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONArray} result
         *              <ul>
         *                  <li>{string} id</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void AddLeagueCommentRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string id = (Params != null && Params.HasKeyNotNull("id")) ? Params["id"] : null;

                if (id == null)
                {
                    Debug.LogError("id not exist in Params");
                    return;
                    //throw new ServiceException("");
                }

                string text = (Params != null && Params.HasKeyNotNull("text")) ? Params["text"] : null;

                if (text == null)
                {
                    Debug.LogError("text not exist in Params");
                    return;
                    //throw new ServiceException("");
                }

                var requestData = new JSONObject();

                requestData.Add("postId", id);
                requestData.Add("comment", text);

                Request(RequestUrls.AddLeagueComment, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        if (!hasError && result.HasKeyNotNull("Result"))
                        {
                            var resultId = result["Result"].ToString();
                            retResult.Add("id", resultId);
                        }

                        returnData.Add("result", retResult);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'>عضویت در لیگ پیش فرض یک بازی</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId","2");
         *      service.subscribeDefaultLeagueRequest(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("subscribeDefaultLeagueRequest method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} gameId - شناسه بازی
         *          </li>
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
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void SubscribeDefaultLeagueRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string gameId = (Params != null && Params.HasKeyNotNull("gameId")) ? Params["gameId"] : null;
                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    return;
                    //throw new ServiceException("");
                }

                var requestData = new JSONObject();
                requestData.Add("gameId", gameId);

                Request(RequestUrls.DefaultLeagueSubscribe, requestData, result =>
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
                            returnData.Add("result", ReformatLeagueObject(result["Result"].AsObject));
                            var data = new JSONObject();
                            data.Add("gameId", gameId);
                            FireEvents("defaultLeagueSubscribe", data);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         *
         * <div style='width: 100%;text-align: right'>دریافت لیست لیگ ها بر اساس لابی </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      JSONArray lobbyIds = new JSONArray();
         *      lobbyIds.put("22");
         *      lobbyIds.put("44");
         *      service.getLobbiesLeagues(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLobbiesLeagues method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{JSONArray} [lobbyIds] - شناسه لابی</li>
         *          <li>{Integer} [size=20]</li>
         *          <li>{Integer} [offset=0]</li>
         *      </ul>
         *
         * @param  res
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONArray} result
         *              <ul>
         *                  <li>{bool} lobbyId</li>
         *                  <li>{JSONArray} leagues
         *                      <ul>
         *                          <li>{string} id </li>
         *                          <li>{string} enrollUrl</li>
         *                          <li>{bool} isMember </li>
         *                          <li>{bool} isFollower</li>
         *                          <li>{JSONObject} userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{string} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} image
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} gameType</li>
         *                          <li>{Integer} playerType</li>
         *                          <li>{Integer} status</li>
         *                          <li>{Integer} financialType</li>
         *                          <li>{Integer} lobbyId</li>
         *                          <li>{Integer} maxPlayers</li>
         *                          <li>{Integer} minNoOfPlayedGames</li>
         *                          <li>{Integer} minPlayers</li>
         *                          <li>{Integer} memberCount</li>
         *                          <li>{Integer} playerNumberType</li>
         *                          <li>{Integer} timestamp</li>
         *                          <li>{string} creator</li>
         *                          <li>{Integer} memberCount</li>
         *                          <li>{Integer} availableCount</li>
         *                          <li>{Integer} discount</li>
         *                          <li>{Integer} numOfComments</li>
         *                          <li>{Integer} numOfFavorites</li>
         *                          <li>{Integer} numOfLikes</li>
         *                          <li>{Integer} type</li>
         *                          <li>{Integer} [endTime]</li>
         *                          <li>{Integer} [startTime]</li>
         *                          <li>{string} rules</li>
         *                          <li>{string} description</li>
         *                          <li>{string} name</li>
         *                          <li>{string} ThreadId</li>
         *                          <li>{string} timelineId</li>
         *                          <li>{bool} hasPrize</li>
         *                          <li>{bool} quickMatch</li>
         *                          <li>{bool} startTypeCapacityComplete</li>
         *                          <li>{bool} startTypeFromDate</li>
         *                          <li>{bool} startTypePublishDate</li>
         *                          <li>{bool} canComment</li>
         *                          <li>{bool} canLike</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} hide</li>
         *                          <li>{Double} price</li>
         *                          <li>{JSONArray} attributeValues</li>
         *                          <li>{JSONArray} categoryList</li>
         *                          <li>{JSONObject} business</li>
         *                          <li>{JSONObject} rate</li>
         *                          <li>{JSONArray} games</li>
         *                      </ul>
         *                  </li>
         *              </ul>
         *          </li>
         *      </ul>
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLobbiesLeagues(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();
                var lobbyIds = (Params.HasKeyNotNull("lobbyIds")) ? Params["lobbyIds"].AsArray : null;

                if (lobbyIds == null)
                {
                    Debug.LogError("lobbyIds not exist in Params");
                    return;
                    //throw new ServiceException(" not defined");
                }

                var size = (Params.HasKeyNotNull("size")) ? Params["size"].AsInt : 50;
                var offset = (Params.HasKeyNotNull("offset")) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);
                requestData.Add("lobbyIds", lobbyIds);

                Request(RequestUrls.GetLobbyLeagues, requestData, result =>
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
                            var allResult = result["Result"].AsObject;

                            var keys = new JSONArray();

                            foreach (var key in allResult.Keys)
                            {
                                keys.Add(key);
                            }

                            var refactorData = new JSONArray();

                            for (var i = 0; i < keys.Count; i++)
                            {
                                string key = keys[i];

                                if (allResult.HasKeyNotNull(key))
                                {
                                    var games = allResult[key].AsArray;
                                    var refGames = new JSONArray();


                                    if (games != null)
                                    {
                                        var lobbiesGames = new JSONObject();

                                        for (var j = 0; j < games.Count; j++)
                                        {
                                            var info = games[j].AsObject;
                                            refGames.Add(ReformatLeagueObject(info));
                                        }

                                        lobbiesGames.Add("lobbyId", key);
                                        lobbiesGames.Add("leagues", refGames);
                                        refactorData.Add(lobbiesGames);
                                    }
                                }
                            }

                            returnData.Add("result", refactorData);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'>دریافت لیست نتیایح بازی هی یک لیگ</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId","3");
         *      reqData.put("size",10);
         *      reqData.put("offset",0);
         *      service.getLeagueMatchesResult(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeagueMatchesResult method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId</li>
         *          <li>{Integer} [size=20]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{string} [userId]</li>
         *          <li>{string} [matchId]</li>
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
         *                  <li>{JSONArray} matches
         *                      <ul>
         *                          <li>{Number} startTime</li>
         *                          <li>{Number} endTime</li>
         *                          <li>{string} id</li>
         *                          <li>{string} leagueId</li>
         *                          <li>{JSONArray} users
         *                              <ul>
         *                                  <li>{JSONObject}  info</li>
         *                                  <li>{JSONArray}  scores</li>
         *                              </ul>
         *                          </li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLeagueMatchesResult(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string leagueId = Params != null && Params.HasKeyNotNull("leagueId") ? Params["leagueId"] : null;
                string userId = Params != null && Params.HasKeyNotNull("userId") ? Params["userId"] : null;
                string matchId = Params != null && Params.HasKeyNotNull("matchId") ? Params["matchId"] : null;
                var size = Params != null && Params.HasKeyNotNull("size") ? Params["size"].AsInt : 20;
                var offset = Params != null && Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId is not defined in Params");
                }

                requestData.Add("leagueId", leagueId);

                if (userId != null)
                {
                    requestData.Add("userId", userId);
                }

                if (matchId != null)
                {
                    requestData.Add("matchId", userId);
                }

                Request(RequestUrls.LeagueMatchesResult, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        if (!hasError)
                        {
                            //JSONArray matches = new JSONArray();
                            if (result.HasKeyNotNull("Result"))
                            {
                                var matchesData = result["Result"].AsArray;

                                var newResultData = new JSONArray();
                                for (var i = 0; i < matchesData.Count; i++)
                                {
                                    var match = matchesData[i].AsObject;
                                    var usersData = match["Users"].AsArray;
                                    var matchData = match["Match"].AsObject;
                                    var leagueData = matchData["League"].AsObject;


                                    var newLeagueData = new JSONObject();
                                    newLeagueData.Add("expireTimestamp", leagueData["ExpireTimestamp"]);
                                    newLeagueData.Add("financialType", leagueData["FinancialType"]);
                                    newLeagueData.Add("fromDateTimestamp", leagueData["FromDateTimestamp"]);
                                    newLeagueData.Add("hasPrize", leagueData["HasPrize"]);
                                    newLeagueData.Add("id", leagueData["ID"].ToString());
                                    newLeagueData.Add("maxPlayers", leagueData["MaxPlayers"]);
                                    newLeagueData.Add("name", leagueData["Name"]);
                                    newLeagueData.Add("statusNumber", leagueData["StatusNumber"]);

                                    var newMatchData = new JSONObject();

                                    newMatchData.Add("endTime", matchData["EndTimestamp"]);
                                    newMatchData.Add("startTime", matchData["StartTimestamp"]);
                                    newMatchData.Add("id", matchData["ID"].ToString());
                                    newMatchData.Add("statusNumber", matchData["StatusNumber"]);
                                    newMatchData.Add("league", newLeagueData);


                                    var newUsersData = new JSONArray();

                                    for (var j = 0; j < usersData.Count; j++)
                                    {
                                        var user = usersData[j].AsObject;
                                        var userInfo = user["UserInfo"].AsObject;
                                        var scoreData = user["Scores"].AsArray;

                                        var newUserData = new JSONObject();
                                        newUserData.Add("id", userInfo["ID"].ToString());
                                        newUserData.Add("name", userInfo["Name"]);

                                        if (userInfo.HasKeyNotNull("Image"))
                                        {
                                            newUserData.Add("image", userInfo["Image"]);
                                        }

                                        if (userInfo.HasKeyNotNull("ProfileImage"))
                                        {
                                            newUserData.Add("imageUrl", userInfo["ProfileImage"]);
                                        }

                                        var newScoresData = new JSONArray();

                                        for (var k = 0; k < scoreData.Count; k++)
                                        {
                                            var score = scoreData[k].AsObject;

                                            var newScoreData = new JSONObject();
                                            newScoreData.Add("name", score["name"]);
                                            newScoreData.Add("value", score["value"]);

                                            newScoresData.Add(newScoreData);
                                        }

                                        newUserData.Add("scores", newScoresData);
                                        newUsersData.Add(newUserData);
                                    }

                                    newMatchData.Add("users", newUsersData);

                                    newResultData.Add(newMatchData);
                                }

                                retResult.Add("matches", newResultData);
                                retResult.Add("nextOffset", offset + matchesData.Count);
                                retResult.Add("hasNext", size == matchesData.Count);
                                retResult.Add("count", result["Count"]);
                            }
                        }

                        returnData.Add("result", retResult);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'>دریافت لیست آخرین نتایج لیگ</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId","3");
         *      reqData.put("size",10);
         *      reqData.put("offset",0);
         *      service.getLeagueLatestMatchesResult(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeagueLatestMatchesResult method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId</li>
         *          <li>{string} [username]</li>
         *          <li>{Integer} [size=20]</li>
         *          <li>{Integer} [offset=0]</li>
         *      </ul>
         *
         * @param  callback
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} matches
         *                      <ul>
         *                          <li>{Number} startTime</li>
         *                          <li>{Number} endTime</li>
         *                          <li>{string} id</li>
         *                          <li>{string} leagueId</li>
         *                          <li>{JSONArray} users
         *                              <ul>
         *                                  <li>{JSONObject}  info</li>
         *                                  <li>{JSONArray}  scores</li>
         *                              </ul>
         *                          </li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLeagueLatestMatchesResult(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string leagueId = Params != null && Params.HasKeyNotNull("leagueId") ? Params["leagueId"] : null;
                string username = Params != null && Params.HasKeyNotNull("username") ? Params["username"] : null;
                var size = Params != null && Params.HasKeyNotNull("size") ? Params["size"].AsInt : 20;
                var offset = Params != null && Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId is not defined in Params");
                }

                requestData.Add("leagueId", leagueId);

                if (username != null)
                {
                    requestData.Add("username", username);
                }

                Request(RequestUrls.LeagueLatestMatchesResult, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        if (!hasError)
                        {
                            //JSONArray matches = new JSONArray();
                            if (result.HasKeyNotNull("Result"))
                            {
                                var matchesData = result["Result"].AsArray;

                                var newResultData = new JSONArray();
                                for (var i = 0; i < matchesData.Count; i++)
                                {
                                    var match = matchesData[i].AsObject;
                                    var usersData = match["Users"].AsArray;
                                    var matchData = match["Match"].AsObject;
                                    var leagueData = matchData["League"].AsObject;


                                    var newLeagueData = new JSONObject();
                                    newLeagueData.Add("expireTimestamp", leagueData["ExpireTimestamp"]);
                                    newLeagueData.Add("financialType", leagueData["FinancialType"]);
                                    newLeagueData.Add("fromDateTimestamp", leagueData["FromDateTimestamp"]);
                                    newLeagueData.Add("hasPrize", leagueData["HasPrize"]);
                                    newLeagueData.Add("id", leagueData["ID"].ToString());
                                    newLeagueData.Add("maxPlayers", leagueData["MaxPlayers"]);
                                    newLeagueData.Add("name", leagueData["Name"]);
                                    newLeagueData.Add("statusNumber", leagueData["StatusNumber"]);

                                    var newMatchData = new JSONObject();

                                    newMatchData.Add("endTime", matchData["EndTimestamp"]);
                                    newMatchData.Add("startTime", matchData["StartTimestamp"]);
                                    newMatchData.Add("id", matchData["ID"].ToString());
                                    newMatchData.Add("statusNumber", matchData["StatusNumber"]);
                                    newMatchData.Add("league", newLeagueData);


                                    var newUsersData = new JSONArray();

                                    for (var j = 0; j < usersData.Count; j++)
                                    {
                                        var user = usersData[j].AsObject;
                                        var userInfo = user["UserInfo"].AsObject;
                                        var scoreData = user["Scores"].AsArray;

                                        var newUserData = new JSONObject();
                                        newUserData.Add("id", userInfo["ID"].ToString());
                                        newUserData.Add("name", userInfo["Name"]);


                                        if (userInfo.HasKeyNotNull("Image"))
                                        {
                                            newUserData.Add("image", userInfo["Image"]);
                                        }

                                        if (userInfo.HasKeyNotNull("ProfileImage"))
                                        {
                                            newUserData.Add("imageUrl", userInfo["ProfileImage"]);
                                        }

                                        var newScoresData = new JSONArray();

                                        for (var k = 0; k < scoreData.Count; k++)
                                        {
                                            var score = scoreData[k].AsObject;

                                            var newScoreData = new JSONObject();
                                            newScoreData.Add("name", score["name"]);
                                            newScoreData.Add("value", score["value"]);

                                            newScoresData.Add(newScoreData);
                                        }

                                        newUserData.Add("scores", newScoresData);
                                        newUsersData.Add(newUserData);
                                    }

                                    newMatchData.Add("users", newUsersData);

                                    newResultData.Add(newMatchData);
                                }

                                retResult.Add("matches", newResultData);
                                retResult.Add("nextOffset", offset + matchesData.Count);
                                retResult.Add("hasNext", size == matchesData.Count);
                            }
                        }

                        returnData.Add("result", retResult);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'> آخرین مسابقات لیگ </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId","3");
         *      reqData.put("size",10);
         *      reqData.put("offset",0);
         *      service.getLeagueLatestMatches(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeagueLatestMatches method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} name</li>
         *          <li>{string} leagueId</li>
         *          <li>{Integer} [size=20]</li>
         *          <li>{Integer} [offset=0]</li>
         *      </ul>
         *
         * @param  callback
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} matches
         *                      <ul>
         *                          <li>{Number} startTime</li>
         *                          <li>{Number} endTime</li>
         *                          <li>{string} id</li>
         *                          <li>{string} leagueId</li>
         *                          <li>{JSONArray} users
         *                              <ul>
         *                                  <li>{string}  id</li>
         *                                  <li>{string}  name</li>
         *                                  <li>{JSONObject}  [image]</li>
         *                                  <li>{string}  [imageUrl]</li>
         *                              </ul>
         *                          </li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLeagueLatestMatches(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string leagueId = (Params != null && Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;
                string nameParam = (Params != null && Params.HasKeyNotNull("name")) ? Params["name"] : null;
                var size = (Params != null && Params.HasKeyNotNull("size")) ? Params["size"].AsInt : 20;
                var offset = (Params != null && Params.HasKeyNotNull("offset")) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (leagueId == null)
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId is not defined in Params");
                }

                requestData.Add("leagueId", leagueId);

                if (nameParam != null)
                {
                    requestData.Add("query", nameParam);
                }

                Request(RequestUrls.LeagueLatestMatches, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        if (!hasError)
                        {
                            //JSONArray matches = new JSONArray();
                            if (result.HasKeyNotNull("Result"))
                            {
                                var matchesData = result["Result"].AsArray;

                                var newResultData = new JSONArray();
                                for (var i = 0; i < matchesData.Count; i++)
                                {
                                    var match = matchesData[i].AsObject;
                                    var usersData = match["Users"].AsArray;
                                    var matchData = match["Match"].AsObject;
                                    var leagueData = matchData["League"].AsObject;


                                    var newLeagueData = new JSONObject();
                                    newLeagueData.Add("expireTimestamp", leagueData["ExpireTimestamp"]);
                                    newLeagueData.Add("financialType", leagueData["FinancialType"]);
                                    newLeagueData.Add("fromDateTimestamp", leagueData["FromDateTimestamp"]);
                                    newLeagueData.Add("hasPrize", leagueData["HasPrize"]);
                                    newLeagueData.Add("id", leagueData["ID"].ToString());
                                    newLeagueData.Add("maxPlayers", leagueData["MaxPlayers"]);
                                    newLeagueData.Add("name", leagueData["Name"]);
                                    newLeagueData.Add("statusNumber", leagueData["StatusNumber"]);

                                    var newMatchData = new JSONObject();

                                    newMatchData.Add("endTime", matchData["EndTimestamp"]);
                                    newMatchData.Add("startTime", matchData["StartTimestamp"]);
                                    newMatchData.Add("id", matchData["ID"].ToString());
                                    newMatchData.Add("statusNumber", matchData["StatusNumber"]);
                                    newMatchData.Add("league", newLeagueData);


                                    var newUsersData = new JSONArray();

                                    for (var j = 0; j < usersData.Count; j++)
                                    {
                                        var user = usersData[j].AsObject;

                                        var newUserData = new JSONObject();
                                        newUserData.Add("id", user["ID"].ToString());
                                        newUserData.Add("name", user["Username"]);


                                        if (user.HasKeyNotNull("Image"))
                                        {
                                            var img = user["Image"].AsObject;
                                            img.Add("id", img["id"].ToString());
                                            newUserData.Add("image", img);
                                        }

                                        if (user.HasKeyNotNull("ProfileImage"))
                                        {
                                            newUserData.Add("imageUrl", user["ProfileImage"]);
                                        }


                                        newUsersData.Add(newUserData);
                                    }

                                    newMatchData.Add("users", newUsersData);

                                    newResultData.Add(newMatchData);
                                }

                                retResult.Add("matches", newResultData);
                                retResult.Add("nextOffset", offset + matchesData.Count);
                                retResult.Add("hasNext", size == matchesData.Count);
                            }
                        }

                        returnData.Add("result", retResult);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'>دریافت لیست مسابقات کاربر</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("leagueId","3");
         *      reqData.put("size",10);
         *      reqData.put("offset",0);
         *      service.getLeagueMatches(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLeagueMatches method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} leagueId</li>
         *          <li>{Integer} status
         *              NotStarted = 1,
         *              Loading = 2,
         *              Running = 3,
         *              Failed = 4,
         *              Finished = 5,
         *              Cancelled = 6,
         *              NotValidResult = 7
         *          </li>
         *          <li>{Integer} [size=20]</li>
         *          <li>{Integer} [offset=0]</li>
         *      </ul>
         *
         * @param  callback
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} matches
         *                      <ul>
         *                          <li>{Number} startTime</li>
         *                          <li>{Number} endTime</li>
         *                          <li>{string} id</li>
         *                          <li>{string} leagueId</li>
         *                          <li>{JSONArray} users
         *                              <ul>
         *                                  <li>{JSONObject}  info</li>
         *                                  <li>{JSONArray}  scores</li>
         *                              </ul>
         *                          </li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLeagueMatches(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string leagueId = (Params != null && Params.HasKeyNotNull("leagueId")) ? Params["leagueId"] : null;
                var status = (Params != null && Params.HasKeyNotNull("status")) ? Params["status"].AsInt : (int?) null;
                var size = (Params != null && Params.HasKeyNotNull("size")) ? Params["size"].AsInt : 20;
                var offset = (Params != null && Params.HasKeyNotNull("offset")) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (leagueId != null)
                {
                    requestData.Add("leagueId", leagueId);
                }

                if (status != null)
                {
                    requestData.Add("status", status);
                }

                Request(RequestUrls.LeagueMatches, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        if (!hasError)
                        {
                            //JSONArray matches = new JSONArray();
                            if (result.HasKeyNotNull("Result"))
                            {
                                var matchesData = result["Result"].AsArray;

                                var newResultData = new JSONArray();
                                for (var i = 0; i < matchesData.Count; i++)
                                {
                                    var match = matchesData[i].AsObject;
                                    var usersData = match["Users"].AsArray;
                                    var leagueData = match["League"].AsObject;


                                    var newLeagueData = new JSONObject();
                                    newLeagueData.Add("expireTimestamp", leagueData["ExpireTimestamp"]);
                                    newLeagueData.Add("financialType", leagueData["FinancialType"]);
                                    newLeagueData.Add("fromDateTimestamp", leagueData["FromDateTimestamp"]);
                                    newLeagueData.Add("hasPrize", leagueData["HasPrize"]);
                                    newLeagueData.Add("id", leagueData["ID"].ToString());
                                    newLeagueData.Add("maxPlayers", leagueData["MaxPlayers"]);
                                    newLeagueData.Add("name", leagueData["Name"]);
                                    newLeagueData.Add("statusNumber", leagueData["StatusNumber"]);

                                    var newMatchData = new JSONObject();

                                    newMatchData.Add("endTime", match["EndTimestamp"]);
                                    newMatchData.Add("startTime", match["StartTimestamp"]);
                                    newMatchData.Add("id", match["ID"].ToString());
                                    newMatchData.Add("statusNumber", match["StatusNumber"]);
                                    newMatchData.Add("league", newLeagueData);


                                    var newUsersData = new JSONArray();

                                    for (var j = 0; j < usersData.Count; j++)
                                    {
                                        var user = usersData[j].AsObject;

                                        var newUserData = new JSONObject();
                                        newUserData.Add("id", user["ID"].ToString());
                                        newUserData.Add("name", user["Name"]);

                                        if (user.HasKeyNotNull("Image"))
                                        {
                                            newUserData.Add("image", user["Image"]);
                                        }

                                        if (user.HasKeyNotNull("ProfileImage"))
                                        {
                                            newUserData.Add("imageUrl", user["ProfileImage"]);
                                        }

                                        newUsersData.Add(newUserData);
                                    }

                                    newMatchData.Add("users", newUsersData);

                                    newResultData.Add(newMatchData);
                                }

                                retResult.Add("matches", newResultData);
                                retResult.Add("nextOffset", offset + matchesData.Count);
                                retResult.Add("hasNext", size == matchesData.Count);
                            }
                        }

                        returnData.Add("result", retResult);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
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
        
        private void SubscribeDefaultLeague(string gameId)
        {
//            Debug.Log("subscribeDefaultLeague...\n\ngameId: " + gameId);
            var requestData = new JSONObject();
            requestData.Add("gameId", gameId);
            //Debug.Log("subscribeDefaultLeague_1 " + gameId);

            Request(RequestUrls.DefaultLeagueSubscribe, requestData, result =>
            {
//                Debug.Log("subscribeDefaultLeague.result: \n\n" + PrettyJson(result));
                try
                {
                    var hasError = result["HasError"].AsBool;
                    if (!hasError)
                    {
                        var leagueData = result["Result"].AsObject;
                        var data = ReformatLeagueObject(leagueData);
                        FireEvents("defaultLeagueSubscribe", data);
                    }
                    else
                    {
                        Util.SetTimeout(() => { SubscribeDefaultLeague(gameId); }, ConfigData.Smit);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                    //throw new ServiceException(e);
                }
            });
        }
        
        private JSONObject ReformatLeagueObject(JSONObject leagueData)
        {
            var league = new JSONObject();

            try
            {
                string leagueId = leagueData["entityId"];
                var userPostInfo = leagueData["userPostInfo"].AsObject;

                var businessData = leagueData["business"].AsObject;

                if (businessData.HasKey("imageInfo") && businessData["imageInfo"] != null)
                {
                    var businessImageData = businessData["imageInfo"].AsObject;
                    businessImageData.Add("id", businessImageData["id"]);
                    businessData.Add("image", businessImageData);
                    businessData.Remove("imageInfo");
                }


                var leagueGames = leagueData["Games"].AsArray;
                var games = new JSONArray();
                for (var i = 0; i < leagueGames.Count; i++)
                {
                    games.Add(ReformatGameObject(leagueGames[i].AsObject));
                }

                league.Add("games", games);


                league.Add("id", leagueId);
                league.Add("enrollUrl", leagueData["EnrollUrl"]);
                league.Add("isMember", leagueData["IsUserMember"]);
                league.Add("isFollower", userPostInfo["favorite"]);
                league.Add("userPostInfo", userPostInfo);

                league.Add("gameType", leagueData["LeagueGameType"]);
                league.Add("playerType", leagueData["LeaguePlayerType"]);
                league.Add("status", leagueData["LeagueStatus"]);
                league.Add("financialType", leagueData["LeagueFinancialType"]);
                //            league.put("lobbyId", leagueData.get("LobbyID"));
                league.Add("maxPlayers", leagueData["MaxPlayers"]);
                league.Add("minNoOfPlayedGames", leagueData["MinNoOfPlayedGames"]);
                league.Add("minPlayers", leagueData["MinPlayers"]);
                if (leagueData.HasKey("ExpireTimestamp") && leagueData["ExpireTimestamp"] != null)
                {
                    league.Add("endTime", leagueData["ExpireTimestamp"]);
                }

                if (leagueData.HasKey("FromDateTimestamp") && leagueData["FromDateTimestamp"] != null)
                {
                    league.Add("startTime", leagueData["FromDateTimestamp"]);
                }

                league.Add("rules", leagueData["Rules"]);
                league.Add("description", leagueData["description"]);
                league.Add("hasPrize", leagueData["HasPrize"]);
                league.Add("price", leagueData["price"].AsDouble / ConfigData.Cf);
                league.Add("name", leagueData["Name"]);
                league.Add("playerNumberType", leagueData["PlayerNumberType"]);
                league.Add("timestamp", leagueData["timestamp"]);
                league.Add("type", leagueData["NTimeKnockout"]);
                league.Add("accessType", leagueData["LeagueAccessType"]);
                league.Add("quickMatch", leagueData["QuickMatch"]);
                league.Add("creator", leagueData["Creator"]);
                league.Add("memberCount", leagueData["MemberCount"]);
                league.Add("startTypeCapacityComplete", leagueData["StartTypeCapacityComplete"]);
                league.Add("startTypeFromDate", leagueData["StartTypeFromDate"]);
                league.Add("startTypePublishDate", leagueData["StartTypePublishDate"]);
                league.Add("ThreadId", leagueData["ThreadId"].ToString());
                league.Add("availableCount", leagueData["availableCount"]);
                league.Add("attributeValues", leagueData["attributeValues"]);
                league.Add("categoryList", leagueData["categoryList"]);
                league.Add("business", businessData);
                league.Add("canComment", leagueData["canComment"]);
                league.Add("canLike", leagueData["canLike"]);
                league.Add("enable", leagueData["enable"]);
                league.Add("hide", leagueData["hide"]);
                league.Add("discount", leagueData["discount"]);
                league.Add("numOfComments", leagueData["numOfComments"]);
                league.Add("numOfFavorites", leagueData["numOfFavorites"]);
                league.Add("numOfLikes", leagueData["numOfLikes"]);
                league.Add("rate", leagueData["rate"]);
                league.Add("trailer", leagueData["Trailer"]);
                league.Add("timelineId", leagueData["timelineId"].ToString());
                //            GET_LEAGUE.put("offlineRequestState", info.getBoolean("offlineRequestState"));

                if (leagueData.HasKey("previewInfo") && leagueData["previewInfo"] != null)
                {
                    var imageData = leagueData["previewInfo"].AsObject;
                    imageData.Add("id", imageData["id"].ToString());
                    league.Add("image", imageData);
                }

                if (leagueData.HasKey("preview") && leagueData["preview"] != null)
                {
                    league.Add("imageUrl", leagueData["preview"]);
                }

                if (leagueData.HasKey("metadata") && leagueData["metadata"] != null)
                {
                    try
                    {
                        var metaObj = JSON.Parse(leagueData["metadata"]).AsObject;

                        if (metaObj.HasKey("bannerImage") && metaObj["bannerImage"] != null)
                        {
                            league.Add("bannerImageUrl", metaObj["bannerImage"]);
                        }
                        else
                        {
                            league.Add("bannerImageUrl", null);
                        }
                    }
                    catch (Exception e)
                    {
                        league.Add("bannerImageUrl", null);
                        Debug.LogError("Exception: " + e.Message);
                    }
                }
                else
                {
                    league.Add("bannerImageUrl", null);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            return league;
        }
        
        private JSONObject ReformatMiniLeagueObject(JSONObject leagueData)
        {
            var newLeagueData = new JSONObject();

            try
            {
                var leagueGames = leagueData["Games"].AsArray;
                var games = new JSONArray();
                for (var i = 0; i < leagueGames.Count; i++)
                {
                    games.Add(ReformatMiniGameObject(leagueGames[i].AsObject));
                }

                newLeagueData.Add("games", games);


                newLeagueData.Add("endTime", leagueData["ExpireTimestamp"]);
                newLeagueData.Add("startTime", leagueData["FromDateTimestamp"]);
                newLeagueData.Add("financialType", leagueData["FinancialType"]);
                newLeagueData.Add("hasPrize", leagueData["HasPrize"]);
                newLeagueData.Add("id", leagueData["ID"].ToString());
                newLeagueData.Add("maxPlayers", leagueData["MaxPlayers"]);
                newLeagueData.Add("name", leagueData["Name"]);
                newLeagueData.Add("statusNumber", leagueData["StatusNumber"]);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            return newLeagueData;
        }
    }
}

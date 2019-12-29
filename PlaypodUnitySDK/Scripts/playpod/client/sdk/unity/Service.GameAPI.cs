using System;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity
{
	public partial class Service {
        
		/**
         * <div style='width: 100%;text-align: right'>دریافت اطلاعات بازی ها </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      JSONArray gamesId = new JSONArray();
         *      gamesId.put("gameId");
         *      reqData.put("gamesId",gamesId);
         *      service.getGamesInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getGamesInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{JSONArray} [gamesId] - شناسه بازی</li>
         *          <li>{string} [lobbyId] - شناسه دسته بازی</li>
         *          <li>{JSONArray} [name] - نام بازی که میخواهید اظلاعات آن را دریافت کنید</li>
         *          <li>{Integer} [size=30]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر بازی</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر بازی</li>
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
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *                  <li>{JSONArray} games
         *                      <ul>
         *                          <li>{string} id</li>
         *                          <li>{string} name</li>
         *                          <li>{string} description</li>
         *                          <li>{string} creator</li>
         *                          <li>{string} physicalUrl</li>
         *                          <li>{string} timelineId</li>
         *                          <li>{string} packageName</li>
         *                          <li>{string} mobileVersion</li>
         *                          <li>{string} mobileVersionCode</li>
         *                          <li>{string} supporterId</li>
         *                          <li>{string} defaultLeagueId</li>
         *                          <li>{string} downloadLink</li>
         *                          <li>{string} gamePlayDescription</li>
         *                          <li>{string} score</li>
         *                          <li>{string} webVersion</li>
         *                          <li>{JSONArray} attributeValues</li>
         *                          <li>{JSONArray} categoryList</li>
         *                          <li>{JSONObject} business</li>
         *                          <li>{JSONObject} userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{string} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} rate
         *                              <ul>
         *                                  <li>{Integer} rate.rate</li>
         *                                  <li>{Integer} rate.rateCount</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} image
         *                              <ul>
         *                                  <li>{string} image.id</li>
         *                                  <li>{string} image.url</li>
         *                                  <li>{string} image.hashCode</li>
         *                                  <li>{Integer} image.width</li>
         *                                  <li>{Integer} image.height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} playerNumbersType</li>
         *                          <li>{Integer} platformType</li>
         *                          <li>{Integer} availableCount</li>
         *                          <li>{Integer} discount</li>
         *                          <li>{Integer} numOfComments</li>
         *                          <li>{Integer} numOfFavorites</li>
         *                          <li>{Integer} numOfLikes</li>
         *                          <li>{bool} canComment</li>
         *                          <li>{bool} canLike</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} hide</li>
         *                          <li>{Double} latitude</li>
         *                          <li>{Double} longitude</li>
         *                          <li>{Double} publishedDate</li>
         *                          <li>{Double} price</li>
         *                          <li>{Double} timestamp</li>
         *                      </ul>
         *                  </li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceExcwخطای پارامتر های ورودی
         * */
        public void GetGamesInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            //        Debug.Log("Service.GetGamesInfo");
            try
            {
                var requestData = new JSONObject();
                var gamesId = Params.HasKey("gamesId") && Params["gamesId"] != null
                    ? Params["gamesId"].AsArray
                    : null;
                string nameParam = Params.HasKey("name") && Params["name"] != null ? Params["name"] : null;
                string lobbyId = Params.HasKey("lobbyId") && Params["lobbyId"] != null ? Params["lobbyId"] : null;

                if (gamesId != null)
                {
                    requestData.Add("gameId", gamesId);
                }

                if (nameParam != null)
                {
                    requestData.Add("filter", nameParam);
                }

                if (lobbyId != null)
                {
                    requestData.Add("lobbyId", lobbyId);
                }

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : 5;
                var offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GameInfo, requestData, result =>
                {
                    //                Debug.Log("Service.GetGamesInfo.request.onResult.result: " + result.ToString());
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"].ToString());
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        if (!hasError)
                        {
                            var retResult = new JSONObject();
                            var games = new JSONArray();
                            if (result.HasKey("Result") && result["Result"] != null)
                            {
                                var allInfo = result["Result"].AsArray;
                                if (allInfo != null)
                                {
                                    for (var i = 0; i < allInfo.Count; i++)
                                    {
                                        var info = allInfo.AsArray[i].AsObject;
                                        games.Add(ReformatGameObject(info));
                                    }
                                }
                            }

                            retResult.Add("games", games);
                            retResult.Add("hasNext", size == games.Count);
                            retResult.Add("nextOffset", offset + games.Count);
                            retResult.Add("count", result["Count"]);
                            returnData.Add("result", retResult);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("exception: " + e.Message);
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
         * <div style='width: 100%;text-align: right'> دریافت برترین باز ها </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("type", 1);
         *      service.getTopGamesInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getTopGamesInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{Integer} [type]
         *              <p>     1 = برترین های گیم سنتر</p>
         *              <p>     2 = برترین بازی ها</p>
         *              <p>     4 =برترین دنبال شده ها</p>
         *              <p>     8 = پیشنهاد گیم سنتر</p>
         *          </li>
         *          <li>{Integer} [size=5]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر بازی</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر بازی</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @param  res
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
         *                          <li>{string} $.name</li>
         *                          <li>{string} $.description</li>
         *                          <li>{string} $.creator</li>
         *                          <li>{string} $.physicalUrl</li>
         *                          <li>{string} $.timelineId</li>
         *                          <li>{string} $.packageName</li>
         *                          <li>{string} $.mobileVersion</li>
         *                          <li>{string} $.mobileVersionCode</li>
         *                          <li>{string} $.supporterId</li>
         *                          <li>{string} $.defaultLeagueId</li>
         *                          <li>{string} $.downloadLink</li>
         *                          <li>{string} $.gamePlayDescription</li>
         *                          <li>{string} $.score</li>
         *                          <li>{string} $.webVersion</li>
         *                          <li>{JSONArray} $.attributeValues</li>
         *                          <li>{JSONArray} $.categoryList</li>
         *                          <li>{JSONObject} $.business</li>
         *                          <li>{JSONObject} $.userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{Integer} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} $.rate
         *                              <ul>
         *                                  <li>{Integer} rate</li>
         *                                  <li>{Integer} rateCount</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} $.image
         *                              <ul>
         *                                  <li>{string}  id</li>
         *                                  <li>{string}  url</li>
         *                                  <li>{string}  hashCode</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} $.playerNumbersType</li>
         *                          <li>{Integer} $.platformType</li>
         *                          <li>{Integer} $.availableCount</li>
         *                          <li>{Integer} $.discount</li>
         *                          <li>{Integer} $.numOfComments</li>
         *                          <li>{Integer} $.numOfFavorites</li>
         *                          <li>{Integer} $.numOfLikes</li>
         *                          <li>{bool} $.canComment</li>
         *                          <li>{bool} $.canLike</li>
         *                          <li>{bool} $.enable</li>
         *                          <li>{bool} $.hide</li>
         *                          <li>{Double} $.latitude</li>
         *                          <li>{Double} $.longitude</li>
         *                          <li>{Double} $.publishedDate</li>
         *                          <li>{Double} $.price</li>
         *                          <li>{Double} $.timestamp</li>
         *                      </ul>
         *                  </li>
         *              </ul>
         *          </li>
         *      </ul>
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetTopGamesInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                var type = (Params.HasKey("type") && Params["type"] != null) ? Params["type"].AsInt : (int?) null;

                if (type != null)
                {
                    requestData.Add("type", type);
                }

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : 10;
                var offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetTopGame, requestData, result =>
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
                            string[] keys =
                            {
                                "GcSuggestion",
                                "GcTop",
                                "TopFollow",
                                "TopRate"
                            };

                            var refactorLeagues = new JSONObject();
                            for (var i = 0; i < keys.Length; i++)
                            {
                                var key = keys[i];

                                if (allResult.HasKey(key) && allResult[key] != null)
                                {
                                    var games = allResult[key].AsArray;
                                    var refGames = new JSONArray();

                                    if (games != null)
                                    {
                                        for (var j = 0; j < games.Count; j++)
                                        {
                                            var info = games[j].AsObject;
                                            refGames.Add(ReformatGameObject(info));
                                        }

                                        refactorLeagues.Add(key, refGames);
                                        var countKeyName = key + "Count";
                                        refactorLeagues.Add(countKeyName, allResult[countKeyName]);
                                    }
                                }
                            }

                            returnData.Add("result", refactorLeagues);
                        }

                        onResult(returnData);
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
         * <div style='width: 100%;text-align: right'>دریافت آخرین بازی ها</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("size", size);
         *      service.getLatestGamesInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLatestGamesInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{Integer} [size=30]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر بازی</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر بازی</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @param  res
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *                  <li>{JSONArray} games - array of JSONObject that each object contain :
         *                      <ul>
         *                          <li>{string} id</li>
         *                          <li>{string} name</li>
         *                          <li>{string} description</li>
         *                          <li>{string} creator</li>
         *                          <li>{string} physicalUrl</li>
         *                          <li>{string} timelineId</li>
         *                          <li>{string} packageName</li>
         *                          <li>{string} mobileVersion</li>
         *                          <li>{string} mobileVersionCode</li>
         *                          <li>{string} supporterId</li>
         *                          <li>{string} defaultLeagueId</li>
         *                          <li>{string} downloadLink</li>
         *                          <li>{string} gamePlayDescription</li>
         *                          <li>{string} score</li>
         *                          <li>{string} webVersion</li>
         *                          <li>{JSONArray} attributeValues</li>
         *                          <li>{JSONArray} categoryList</li>
         *                          <li>{JSONObject} business</li>
         *                          <li>{JSONObject} userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{Integer} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} rate
         *                              <ul>
         *                                  <li>{Integer} rate</li>
         *                                  <li>{Integer} rateCount</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} image
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{string} hashCode</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} playerNumbersType</li>
         *                          <li>{Integer} platformType</li>
         *                          <li>{Integer} availableCount</li>
         *                          <li>{Integer} discount</li>
         *                          <li>{Integer} numOfComments</li>
         *                          <li>{Integer} numOfFavorites</li>
         *                          <li>{Integer} numOfLikes</li>
         *                          <li>{bool} canComment</li>
         *                          <li>{bool} canLike</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} hide</li>
         *                          <li>{Double} latitude</li>
         *                          <li>{Double} longitude</li>
         *                          <li>{Double} publishedDate</li>
         *                          <li>{Double} price</li>
         *                          <li>{Double} timestamp</li>
         *                      </ul>
         *                  </li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLatestGamesInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : 30;
                var offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetLatestGame, requestData, result =>
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
                            var allGames = result["Result"].AsArray;

                            var games = new JSONArray();
                            var retResult = new JSONObject();
                            if (allGames != null)
                            {
                                for (var i = 0; i < allGames.Count; i++)
                                {
                                    var info = allGames[i].AsObject;
                                    games.Add(ReformatGameObject(info));
                                }
                            }

                            retResult.Add("games", games);
                            retResult.Add("hasNext", size == games.Count);
                            retResult.Add("nextOffset", offset + games.Count);
                            returnData.Add("result", retResult);
                        }

                        onResult(returnData);
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
         * <div style='width: 100%;text-align: right'> دریافت بازی های مرتبط </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId", gameId);
         *      service.getRelatedGamesInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getRelatedGamesInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} gameId شناسه بازی</li>
         *          <li>{Integer} [type]
         *              <p>     1 = بازی هایی که در یک لابی می باشند</p>
         *              <p>     2 = بازی هایی که سازنده آنها یکی است</p>
         *          </li>
         *          <li>{Integer} [size=30]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر بازی</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر بازی</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @param  res
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>    {bool} hasNext</li>
         *                  <li>    {Integer} nextOffset</li>
         *                  <li>    {JSONArray} CategoryEquality : $ - بازی هایی که در یک لابی می باشند</li>
         *                  <li>    {JSONArray} CreatorEquality : $ - بازی هایی که سازنده آنها یکی است
         *                      <ul>
         *                          <li>{string} id</li>
         *                          <li>{string} name</li>
         *                          <li>{string} description</li>
         *                          <li>{string} creator</li>
         *                          <li>{string} physicalUrl</li>
         *                          <li>{string} timelineId</li>
         *                          <li>{string} packageName</li>
         *                          <li>{string} mobileVersion</li>
         *                          <li>{string} mobileVersionCode</li>
         *                          <li>{string} supporterId</li>
         *                          <li>{string} defaultLeagueId</li>
         *                          <li>{string} downloadLink</li>
         *                          <li>{string} gamePlayDescription</li>
         *                          <li>{string} score</li>
         *                          <li>{string} webVersion</li>
         *                          <li>{JSONArray} attributeValues</li>
         *                          <li>{JSONArray} categoryList</li>
         *                          <li>{JSONObject} business</li>
         *                          <li>{JSONObject} userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{Integer} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} rate
         *                              <ul>
         *                                  <li>{Integer} rate</li>
         *                                  <li>{Integer} rateCount</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} image
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{string} hashCode</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} playerNumbersType</li>
         *                          <li>{Integer} platformType</li>
         *                          <li>{Integer} availableCount</li>
         *                          <li>{Integer} discount</li>
         *                          <li>{Integer} numOfComments</li>
         *                          <li>{Integer} numOfFavorites</li>
         *                          <li>{Integer} numOfLikes</li>
         *                          <li>{bool} canComment</li>
         *                          <li>{bool} canLike</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} hide</li>
         *                          <li>{Double} latitude</li>
         *                          <li>{Double} longitude</li>
         *                          <li>{Double} publishedDate</li>
         *                          <li>{Double} price</li>
         *                          <li>{Double} timestamp</li>
         *                      </ul>
         *                  </li>
         *               </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetRelatedGamesInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = (Params.HasKey("gameId") && Params["gameId"] != null) ? Params["gameId"] : null;

                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    return;
                    //throw new ServiceException("gameId not exist in Params");
                }

                var type = (Params.HasKey("type") && Params["type"] != null) ? Params["type"].AsInt : (int?) null;

                if (type != null)
                {
                    requestData.Add("type", type);
                }

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : 5;
                var offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                requestData.Add("gameId", gameId);

                Request(RequestUrls.GetRelatedGame, requestData, result =>
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
                            var retResult = new JSONObject();
                            var hasNext = false;

                            foreach (var nameParams in allResult.Keys)
                            {
                                if (allResult.HasKey(nameParams) && allResult[nameParams] != null)
                                {
                                    var allGames = allResult[nameParams].AsArray;
                                    var games = new JSONArray();

                                    if (allGames != null)
                                    {
                                        for (var j = 0; j < allGames.Count; j++)
                                        {
                                            var info = allGames[j].AsObject;
                                            games.Add(ReformatGameObject(info));
                                        }

                                        if (size == games.Count)
                                        {
                                            hasNext = true;
                                        }

                                        retResult.Add(nameParams, games);
                                    }
                                }
                            }

                            retResult.Add("hasNext", hasNext);
                            retResult.Add("nextOffset", offset + size);
                            returnData.Add("result", retResult);
                        }

                        onResult(returnData);
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
         * <div style='width: 100%;text-align: right'> دریافت آیتم های یک بازی </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId", "2");
         *      service.getGameItems(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getGameItems method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} gameId - شناسه بازی</li>
         *          <li>{Integer} [size=5]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{string} [itemId] - شناسه آیتم
         *              <p> در صورت پر کردن این فیلد , آیتم مشخص شده باز گردانده می شود و در غیر اینصورت کلیه آیتم های آن بازی</p>
         *          </li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر آیتم</li>
         *                  <li>{Integer} [imageHeight]  رزولیشن عمودی تصویر آیتم</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @param  callback
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} packs - Array Of JSONObject that contain :
         *                      <ul>
         *                          <li>{string} id</li>
         *                          <li>{string} name</li>
         *                          <li>{string} description</li>
         *                          <li>{bool} visible</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} allowedTimesToBuy</li>
         *                          <li>{JSONObject} [image]
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
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
        public void GetGameItems(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string gameId = Params.HasKeyNotNull("gameId") ? Params["gameId"] : null;
                string itemId = Params.HasKeyNotNull("itemId") ? Params["itemId"] : null;

                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    return;
                    //throw new ServiceException("gameId not exist in Params");
                }

                var requestData = new JSONObject();
                requestData.Add("entityId", gameId);

                var size = Params.HasKeyNotNull("size") ? Params["size"].AsInt : 10;
                var offset = Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (itemId != null)
                {
                    requestData.Add("itemId", itemId);
                }

                Request(RequestUrls.GetGameItems, requestData, result =>
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
                            var items = new JSONArray();
                            var rawItems = result["Result"].AsArray;
                            for (var i = 0; i < rawItems.Count; i++)
                            {
                                items.Add(ReformatGameItem(rawItems[i].AsObject));
                            }

                            var retResult = new JSONObject();
                            retResult.Add("items", items);
                            retResult.Add("hasNext", requestData["size"].AsInt == items.Count);
                            retResult.Add("nextOffset", requestData["offset"].AsInt + items.Count);

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
         * <div style='width: 100%;text-align: right'>اعلام امتیاز کاربر به بازی</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId","22");
         *      reqData.put("rate",4);
         *      service.sendGameRateRequest(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("sendGameRateRequest method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} gameId شناسه بازی</li>
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
        public void SendGameRateRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = Params != null && Params.HasKeyNotNull("gameId") ? Params["gameId"] : null;

                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    return;
                    //throw new ServiceException(" not exist in Params");
                }

                requestData.Add("entityId", gameId);

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

                Request(RequestUrls.GameRate, requestData, result =>
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
         * <div style='width: 100%;text-align: right'> افزودن نظر بر روی یک بازی </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("id","56");
         *      reqData.put("text","text");
         *      service.addGameCommentRequest(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("addGameCommentRequest method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} id شناسه بازی </li>
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
        public void AddGameCommentRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string id = (Params != null && Params.HasKeyNotNull("id")) ? Params["id"] : null;

                if (id == null)
                {
                    Debug.LogError("id not exist in Params");
                    return;
                    //throw new ServiceException("id not exist in Params");
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

                Request(RequestUrls.AddGameComment, requestData, result =>
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
         * <div style='width: 100%;text-align: right'> دریافت لیست نظر های بازی و یا لیگ</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("id","56");
         *      service.getCommentList(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getCommentList method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{Integer} [size=20]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{string} id شناسه پست بازی و یا لیگ</li>
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
         *                  <li>{JSONArray} comments - Array Of JSONObject that contain :
         *                      <ul>
         *                          <li>{bool} confirmed</li>
         *                          <li>{string} id</li>
         *                          <li>{string} text</li>
         *                          <li>{Double} timestamp</li>
         *                          <li>{JSONObject} user
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string} name</li>
         *                                  <li>{JSONObject} image
         *                                      <ul>
         *                                          <li>{string} id</li>
         *                                          <li>{Integer} actualWidth</li>
         *                                          <li>{Integer} actualHeight</li>
         *                                          <li>{Integer} width</li>
         *                                          <li>{Integer} height</li>
         *                                      </ul>
         *                                  </li>
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
        public void GetCommentList(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string id = (Params != null && Params.HasKeyNotNull("id")) ? Params["id"] : null;

                if (id == null)
                {
                    Debug.LogError("id not exist in Params");
                    return;
                    //throw new ServiceException("id not exist in Params");
                }


                var requestData = new JSONObject();

                requestData.Add("postId", id);

                var size = (Params != null && Params.HasKeyNotNull("size")) ? Params["size"].AsInt : 20;
                var offset = (Params != null && Params.HasKeyNotNull("offset")) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetCommentList, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        var comments = new JSONArray();
                        if (!hasError)
                        {
                            if (result.HasKeyNotNull("Result"))
                            {
                                var res = result["Result"].AsArray;
                                for (var i = 0; i < res.Count; i++)
                                {
                                    var comment = res[i].AsObject;
                                    comment.Add("id", comment["id"].ToString());
                                    

                                    var user = comment["user"].AsObject;
                                    user.Add("id", user["id"].ToString());
                                    

                                    if (user.HasKeyNotNull("image"))
                                    {
                                        var image = user["image"].AsObject;
                                        image.Add("id", image["id"].ToString());
                                        
                                    }

                                    if (user.HasKeyNotNull("profileImage"))
                                    {
                                        user.Add("imageUrl", user["profileImage"]);
                                        
                                        user.Remove("profileImage");
                                    }

                                    comments.Add(comment);
                                }
                            }

                            retResult.Add("comments", comments);
                        }

                        retResult.Add("hasNext", size == comments.Count);
                        retResult.Add("nextOffset", offset + comments.Count);
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
         * <div style='width: 100%;text-align: right'> دریافت گالری تصاویر یک بازی </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId","122")
         *      reqData.put("businessId","44")
         *      service.getGallery(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void getGallery(JSONObject result) {
         *              System.out.println("getGallery method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} gameId</li>
         *          <li>{string} businessId</li>
         *      </ul>
         *
         * @param  callback
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONArray} result
         *              <ul>
         *                  <li>{string} imageUrl </li>
         *                  <li>{string} title </li>
         *                  <li>{string} description </li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetGallery(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = (Params != null && Params.HasKeyNotNull("gameId")) ? Params["gameId"] : null;
                string businessId = (Params != null && Params.HasKeyNotNull("businessId"))
                    ? Params["businessId"]
                    : null;


                if (gameId == null)
                {
                    Debug.LogError("gameId not exist in Params");
                    return;
                    //throw new ServiceException(" is not defined in Params");
                }

                if (businessId == null)
                {
                    Debug.LogError("businessId not exist in Params");
                    return;
                    //throw new ServiceException(" is not defined in Params");
                }

                requestData.Add("productId", gameId);
                requestData.Add("businessId", businessId);

                Request(RequestUrls.GetGallery, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var refResult = new JSONArray();
                        if (!hasError)
                        {
                            if (result.HasKeyNotNull("Result"))
                            {
                                var results = result["Result"].AsArray;
                                for (var i = 0; i < results.Count; i++)
                                {
                                    var data = results[i].AsObject;
                                    var refData = new JSONObject();

                                    refData.Add("imageUrl", data["previewImage"].ToString());
                                    refData.Add("title", data["title"].ToString());
                                    refData.Add("description", data["description"].ToString());

                                    refResult.Add(refData);
                                }
                            }
                        }

                        returnData.Add("result", refResult);
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
         * <div style='width: 100%;text-align: right'>دریافت لیست بازی ها بر اساس لابی </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      JSONArray lobbyIds = new JSONArray();
         *      lobbyIds.put("22");
         *      lobbyIds.put("44");
         *      service.getLobbiesGames(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLobbiesGames method : " + result);
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
         *                  <li>{JSONArray} games
         *                      <ul>
         *                          <li>{string} id</li>
         *                          <li>{string} name</li>
         *                          <li>{string} description</li>
         *                          <li>{string} creator</li>
         *                          <li>{string} physicalUrl</li>
         *                          <li>{string} timelineId</li>
         *                          <li>{string} packageName</li>
         *                          <li>{string} mobileVersion</li>
         *                          <li>{string} mobileVersionCode</li>
         *                          <li>{string} supporterId</li>
         *                          <li>{string} defaultLeagueId</li>
         *                          <li>{string} downloadLink</li>
         *                          <li>{string} gamePlayDescription</li>
         *                          <li>{string} score</li>
         *                          <li>{string} webVersion</li>
         *                          <li>{JSONArray} attributeValues</li>
         *                          <li>{JSONArray} categoryList</li>
         *                          <li>{JSONObject} business</li>
         *                          <li>{JSONObject} userPostInfo
         *                              <ul>
         *                                  <li>{bool} favorite</li>
         *                                  <li>{bool} liked</li>
         *                                  <li>{string} postId</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} rate
         *                              <ul>
         *                                  <li>{Integer} rate.rate</li>
         *                                  <li>{Integer} rate.rateCount</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} image
         *                              <ul>
         *                                  <li>{string} image.id</li>
         *                                  <li>{string} image.url</li>
         *                                  <li>{string} image.hashCode</li>
         *                                  <li>{Integer} image.width</li>
         *                                  <li>{Integer} image.height</li>
         *                              </ul>
         *                          </li>
         *                          <li>{Integer} playerNumbersType</li>
         *                          <li>{Integer} platformType</li>
         *                          <li>{Integer} availableCount</li>
         *                          <li>{Integer} discount</li>
         *                          <li>{Integer} numOfComments</li>
         *                          <li>{Integer} numOfFavorites</li>
         *                          <li>{Integer} numOfLikes</li>
         *                          <li>{bool} canComment</li>
         *                          <li>{bool} canLike</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} hide</li>
         *                          <li>{Double} latitude</li>
         *                          <li>{Double} longitude</li>
         *                          <li>{Double} publishedDate</li>
         *                          <li>{Double} price</li>
         *                          <li>{Double} timestamp</li>
         *                      </ul>
         *                  </li>
         *              </ul>
         *          </li>
         *      </ul>
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetLobbiesGames(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();
                var lobbyIds = (Params.HasKeyNotNull("lobbyIds")) ? Params["lobbyIds"].AsArray : null;

                if (lobbyIds == null || lobbyIds.Count < 1)
                {
                    Debug.LogError("lobbyIds not exist in Params");
                    return;
                    //throw new ServiceException(" not defined");
                }

                var size = (Params.HasKeyNotNull("size")) ? Params["size"].AsInt : 5;
                var offset = (Params.HasKeyNotNull("offset")) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);
                requestData.Add("lobbyIds", lobbyIds);

                Request(RequestUrls.GetLobbyGames, requestData, result =>
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
                                            refGames.Add(ReformatGameObject(info));
                                        }

                                        lobbiesGames.Add("lobbyId", key);
                                        lobbiesGames.Add("games", refGames);
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
         *  <div style='width: 100%;text-align: right'>دریافت لیست لایو ها </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      service.getLives(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLives method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
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
         *                  <li>{JSONArray} lives - Array Of JSONObject that contain :
         *                      <ul>
         *                          <li>{JSONObject} match  match object</li>
         *                          <li>{string} url</li>
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
        public void GetLives(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                //            final Integer size = (Params != null && Params.HasKeyNotNull("size") && !Params.isNull("size")) ? Params.getInt("size") : 20;
                //            final Integer offset = (Params != null && Params.HasKeyNotNull("offset") && !Params.isNull("offset")) ? Params.getInt("offset") : 0;
                //            requestData.Add("size", size);
                //            requestData.Add("offset", offset);

                Request(RequestUrls.GetLive, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var retResult = new JSONObject();
                        var lives = new JSONArray();
                        if (!hasError)
                        {
                            if (result.HasKeyNotNull("Result"))
                            {
                                var res = result["Result"].AsArray;
                                for (var i = 0; i < res.Count; i++)
                                {
                                    lives.Add(ReformatLiveObject(res[i].AsObject));
                                }
                            }

                            retResult.Add("lives", lives);
                        }

                        retResult.Add("hasNext", false);
                        retResult.Add("nextOffset", 0);
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
         * <div style='width: 100%;text-align: right'> دریافت دسته بندی های بازی</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      service.getLobby(new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getLobby method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  callback
         *      <p>onResult method Params is JSONObject that has</p>
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONArray} result - Array Of JSONObject that contain :
         *              <ul>
         *                  <li>{string} id</li>
         *                  <li>{string} name</li>
         *                  <li>{JSONObject} [image]
         *                      <ul>
         *                          <li>{string} id</li>
         *                          <li>{string} url</li>
         *                          <li>{Integer} width</li>
         *                          <li>{Integer} height</li>
         *                      </ul>
         *                  </li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای سرویس
         * */
        public void GetLobby(Action<JSONObject> onResult)
        {
            Request(RequestUrls.GetLobbies, new JSONObject(), result =>
            {
                var returnData = new JSONObject();
                try
                {
                    var hasError = result["HasError"].AsBool;
                    returnData.Add("hasError", hasError);
                    returnData.Add("errorMessage", result["ErrorMessage"]);
                    returnData.Add("errorCode", result["ErrorCode"].AsInt);


                    if (!hasError && result.HasKeyNotNull("Result"))
                    {
                        var retRes = result["Result"].AsArray;
                        var retResult = new JSONArray();

                        for (var i = 0; i < retRes.Count; i++)
                        {
                            var lobby = retRes[i].AsObject;
                            var data = new JSONObject();

                            data.Add("id", lobby["ID"].ToString());
                            data.Add("name", lobby["Name"]);
                            if (lobby.HasKeyNotNull("Image"))
                            {
                                var image = lobby["Image"].AsObject;
                                image.Add("id", image["id"].ToString());
                                data.Add("image", image);
                            }

                            retResult.Add(data);
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
        
        private JSONObject ReformatGameObject(JSONObject info)
        {
            try
            {
                JSONObject businessData = null;

                if (info.HasKey("business") && info["business"] != null)
                {
                    businessData = info["business"].AsObject;

                    if (businessData.HasKey("imageInfo") && businessData["imageInfo"] != null)
                    {
                        var businessImageData = businessData["imageInfo"].AsObject;

                        businessImageData.Add("image", businessImageData["id"].ToString());
                        businessData.Add("image", businessImageData);
                        businessData.Remove("imageInfo");
                    }
                }

                var refLobby = new JSONObject();
                var lobby = info["Lobby"].AsObject;
                refLobby.Add("id", lobby["ID"].ToString());
                refLobby.Add("name", lobby["Name"].ToString());
                refLobby.Add("timestamp", lobby["Timestamp"]);
                refLobby.Add("hashCode", lobby["HashCode"]);

                if (lobby.HasKey("Image") && lobby["Image"] != null)
                {
                    var lobbyImageData = lobby["Image"].AsObject;

                    lobbyImageData.Add("image", lobbyImageData["id"].ToString());
                    refLobby.Add("image", lobbyImageData);
                }


                var gameData = new JSONObject();
                gameData.Add("id", info["entityId"].ToString());
                gameData.Add("lobby", refLobby);
                gameData.Add("name", info["Name"]);
                gameData.Add("description", info["description"]);
                gameData.Add("creator", info["Creator"]);
                gameData.Add("playerNumbersType", info["GamePlayerNumbersType"]);
                gameData.Add("status", info["GameStatus"]);
                gameData.Add("physicalUrl", info["PhysicalUrl"]);
                gameData.Add("timelineId", info["timelineId"].ToString());
                gameData.Add("packageName", info["PackageName"]);
                gameData.Add("mobileVersion", info["MobileVersion"]);
                gameData.Add("mobileVersionCode", info["MobileVersionCode"]);
                gameData.Add("supporterId", info["SupporterID"].ToString());
                gameData.Add("defaultLeagueId", info["DefaultLeague"].ToString());
                gameData.Add("downloadLink", info["DownloadLink"]);
                gameData.Add("gamePlayDescription", info["GamePlayDesc"]);
                gameData.Add("platformType", info["Platform"].AsInt);
                gameData.Add("score", info["Score"]);
                gameData.Add("webVersion", info["WebVersion"]);
                gameData.Add("attributeValues", info["attributeValues"]);
                gameData.Add("categoryList", info["categoryList"]);
                gameData.Add("availableCount", info["availableCount"].AsInt);
                gameData.Add("discount", info["discount"].AsInt);
                gameData.Add("numOfComments", info["numOfComments"]);
                gameData.Add("numOfFavorites", info["numOfFavorites"]);
                gameData.Add("numOfLikes", info["numOfLikes"]);
                gameData.Add("business", businessData);
                gameData.Add("canComment", info["canComment"]);
                gameData.Add("canLike", info["canLike"]);
                gameData.Add("enable", info["enable"]);
                gameData.Add("infrastructure", info["Infrastructure"]);
                gameData.Add("hide", info["hide"]);
                gameData.Add("latitude", info["latitude"]);
                gameData.Add("longitude", info["longitude"]);
                gameData.Add("publishedDate", info["PublishedDate"]);
                gameData.Add("price", info["price"]);
                gameData.Add("timestamp", info["timestamp"]);
                gameData.Add("rate", info["rate"]);
                gameData.Add("userPostInfo", info["userPostInfo"]);
                gameData.Add("hasLeague", info["HasLeague"]);
                gameData.Add("hasSdk", info["HasSdk"]);
                gameData.Add("apkSize", info["ApkSize"]);
                gameData.Add("esrb", info["Esrb"]);
                gameData.Add("trailer", info["Trailer"]);
                gameData.Add("bannerImageUrl", info["Banner"]);

                if (info.HasKey("Changelog") && info["Changelog"] != null)
                {
                    gameData.Add("changelog", info["Changelog"]);
                }
                else
                {
                    gameData.Add("changelog", "");
                }

                if (info.HasKey("previewInfo") && info["previewInfo"] != null)
                {
                    var imageData = info["previewInfo"].AsObject;
                    imageData.Add("id", imageData["id"].ToString());
                    gameData.Add("image", imageData);
                }

                if (info.HasKey("preview") && info["preview"] != null)
                {
                    gameData.Add("imageUrl", info["preview"]);
                }


                return gameData;
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                return null;
            }
        }
        
        private JSONObject ReformatMiniGameObject(JSONObject info)
        {
            try
            {
                var gameData = new JSONObject();
                gameData.Add("id", info["entityId"].ToString());
                gameData.Add("name", info["Name"]);
                gameData.Add("timelineId", info["timelineId"].ToString());
                gameData.Add("downloadLink", info["DownloadLink"]);
                gameData.Add("description", info["Description"]);

                if (info.HasKeyNotNull("Preview"))
                {
                    gameData.Add("imageUrl", info["Preview"]);
                }
                else
                {
                    gameData.Add("imageUrl", null);
                }

                if (info.HasKeyNotNull("Banner"))
                {
                    gameData.Add("bannerImageUrl", info["Banner"]);
                }
                else
                {
                    gameData.Add("bannerImageUrl", null);
                }


                return gameData;
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                return null;
            }
        }
        
        private JSONObject ReformatGameItem(JSONObject item)
        {
            var returnData = new JSONObject();

            try
            {
                var image = item["Image"].AsObject;

                returnData.Add("id", item["ID"].ToString());
                returnData.Add("name", item["Name"]);
                returnData.Add("description", item["Desc"]);
                returnData.Add("visible", item["Visible"]);
                returnData.Add("Enable", item["Enable"]);
                returnData.Add("allowedTimesToBuy", item["AllowedTimesToBuy"]);
                returnData.Add("image", image);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }

            return returnData;
        }
	}
}

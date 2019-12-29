using System;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity
{
    public partial class Service {
        
        /**
         * <div style='width: 100%;text-align: right'>دریافت کاربران آنلاین یک لیگ </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *       JSONObject reqData = new JSONObject();
         *       reqData.put("size", 10);
         *       reqData.put("gameId", "gameId");
         *       reqData.put("leagueId", "leagueId");
         *       service.getOnlineUser(reqData, new RequestCallback() {
         *           {@code @Override}
         *           public void onResult(JSONObject data) {
         *               System.out.println("getOnlineUser method : " + data);
         *           }
         *       });
         *  </code>
         * </pre>
         *  @param  Params
         *      <ul>
         *          <li>{string} gameId شناسه بازی</li>
         *          <li>{string} leagueId شناسه لیگ</li>
         *          <li>{string} [offset=0]</li>
         *          <li>{string} [size]</li>
         *          <li>{string} [filter] نام کاربر</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر کاربر</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر کاربر</li>
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
         *                  <li>{JSONObject} users - مقدار کلید های این آبجکت شناسه کاربران می باشد و مقدار آن نیز اطلاعات کاربر :
         *                      <ul>
         *                          <li>{string} name</li>
         *                          <li>{JSONObject} [image]
         *                              <ul>
         *                                  <li>{string} id</li>
         *                                  <li>{string}  url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
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
        public void GetOnlineUser(JSONObject Params, Action<JSONObject> onResult)
        {
//            Debug.Log("Service.GetOnlineUser: " + PrettyJson(Params));
            try
            {
                string gameId = Params.HasKey("gameId") ? Params["gameId"] : null;
                string leagueId = Params.HasKey("leagueId") ? Params["leagueId"] : null;

                if (gameId == null || gameId.Equals(string.Empty))
                {
                    Debug.LogError("gameId not exist in Params");
                    return;
                    //throw new ServiceException("gameId not exist in Params");
                }

                if (leagueId == null || leagueId.Equals(string.Empty))
                {
                    Debug.LogError("leagueId not exist in Params");
                    return;
                    //throw new ServiceException("leagueId not exist in Params");
                }

                int? size = Params.HasKey("size") ? Params["size"].AsInt : ConfigData.Gous;
                int? offset = Params.HasKey("offset") ? Params["offset"].AsInt : 0;
                string filter = Params.HasKey("filter") ? Params["filter"] : null;

                var requestData = new JSONObject();
                requestData.Add("gameId", gameId);
                requestData.Add("leagueId", leagueId);

                requestData.Add("index", offset);
                requestData.Add("count", size);

                if (filter != null)
                {
                    requestData.Add("filter", filter);
                }

                _getOnlineUser(requestData, onResult);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }
        
        /**
         * <div style='width: 100%;text-align: right'>اعلام ورود کاربر</div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("id","56");
         *      reqData.put("name","ali");
         *      reqData.put("token","************");
         *      reqData.put("tokenIssuer",1);
         *      reqData.put("tokenIssuer",1);
         *      service.initLogin(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("initLogin method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} id</li>
         *          <li>{string} name</li>
         *          <li>{string} token</li>
         *          <li>{string} tokenIssuer</li>
         *          <li>{JSONObject} [image]
         *              <ul>
         *                  <li>{string} id</li>
         *                  <li>{Integer} width</li>
         *                  <li>{Integer} width</li>
         *                  <li>{Integer} height</li>
         *                  <li>{Integer} actualWidth</li>
         *                  <li>{Integer} actualHeight</li>
         *                  <li>{string} [hashCode]</li>
         *                  <li>{string} [name]</li>
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
         *          <li>{JSONObject} result</li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void InitLogin(JSONObject Params, Action<JSONObject> onResult)
        {
//            Debug.Log("initLogin" + Params);
            try
            {
                var token = (Params != null && Params.HasKey("token") && Params["token"] != null)
                    ? Params["token"].ToString()
                    : null;
                var tokenIssuer = (Params != null && Params.HasKey("tokenIssuer") && Params["tokenIssuer"] != null)
                    ? Params["tokenIssuer"].AsInt
                    : -5;
                var nameParams = (Params != null && Params.HasKey("name") && Params["name"] != null)
                    ? Params["name"].ToString()
                    : null;
                var id = (Params != null && Params.HasKey("id") && Params["id"] != null)
                    ? Params["id"].ToString()
                    : null;
                var image = (Params != null && Params.HasKey("image") && Params["image"] != null)
                    ? Params["image"].AsObject
                    : null;
                var imageUrl = (Params != null && Params.HasKey("imageUrl") && Params["imageUrl"] != null)
                    ? Params["imageUrl"].ToString()
                    : null;

                if (token == null)
                {
                    Debug.LogError("No token!!!");
                    return;
                    //throw new ServiceException("token not exist in Params");
                }

                if (tokenIssuer == -5)
                {
                    Debug.LogError("NO tokenIssuer!!!");
                    return;
                    //throw new ServiceException("tokenIssuer not exist in Params");
                }

                if (nameParams == null)
                {
                    Debug.LogError("NO name!!!");
                    return;
                    //throw new ServiceException("name not exist in Params");
                }

                if (id == null)
                {
                    Debug.LogError("NO id!!!");
                    return;
                    //throw new ServiceException("id not exist in Params");
                }


                var uData = new JSONObject();
                uData.Add("id", id);
                uData.Add("name", nameParams);
                uData.Add("image", image);
                uData.Add("imageUrl", imageUrl);
                uData.Add("token", token);
                uData.Add("tokenIssuer", tokenIssuer);
                uData.Add("guest", false);
                LoginAction(uData);
                //            loginAction(id, name, token, image, false, tokenIssuer);


                onResult(Util.CreateReturnData(false, "", 0, new JSONObject()));
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }
        
        
        /**
         *
         * <div style='width: 100%;text-align: right'>دریافت اطلاعات پروفایل کاربر </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *            JSONObject reqData = new JSONObject();
         *            reqData.put("userId", "5");
         *            service.getUserProfile(reqData, new RequestCallback() {
         *                {@code @Override}
         *                public void onResult(JSONObject result) {
         *                    System.out.println("getUserProfile method : " + result);
         *                }
         *            });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} [userId] - در صورت پر نکردن این فیلد , اطلاعات خود کاربر برگردانده می شود</li>
         *          <li>{bool} [refetch=false] - دریافت اطلاعات بروزرسانی شده</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر کاربر</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر کاربر</li>
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
         *                  <li>{Double} birthDate</li>
         *                  <li>{Double} joinDate</li>
         *                  <li>{Double} score</li>
         *                  <li>{Double} sheba</li>
         *                  <li>{string} [cellphoneNumber]</li>
         *                  <li>{string} [email]</li>
         *                  <li>{string} [firstName]</li>
         *                  <li>{string} [lastName]</li>
         *                  <li>{string} [name]</li>
         *                  <li>{string} nickName</li>
         *                  <li>{string} [gender]</li>
         *                  <li>{Integer} followingCount</li>
         *                  <li>{JSONObject} [image]
         *                      <ul>
         *                          <li>{string}  id</li>
         *                          <li>{string}  url</li>
         *                          <li>{Integer} width</li>
         *                          <li>{Integer} height</li>
         *                      </ul>
         *                  </li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetUserProfile(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                if (Params.HasKey("userId") && Params["userId"] != null)
                {
                    requestData.Add("userId", Params["userId"]);
                }

                if (Params.HasKey("refetch") && Params["refetch"] != null)
                {
                    requestData.Add("refetch", Params["userId"].AsBool);
                }

                int? imageWidth = null;
                int? imageHeight = null;

                if (Params.HasKey("setting") && Params["setting"] != null)
                {
                    var setting = Params["setting"].AsObject;
                    if (setting != null)
                    {
                        if (setting.HasKey("imageWidth"))
                        {
                            imageWidth = setting["imageWidth"].AsInt;
                        }

                        if (setting.HasKey("imageHeight"))
                        {
                            imageHeight = setting["imageHeight"].AsInt;
                        }
                    }
                }

                //            int? finalImageWidth = imageWidth;
                //            int? finalImageHeight = imageHeight;

                Request(RequestUrls.GetUserProfile, requestData, result =>
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
                            var user = result["Result"].AsObject;

                            if (user.HasKey("gender") && user["gender"] != null)
                            {
                                if (user["gender"].ToString().Equals("مرد"))
                                {
                                    user.Add("gender", "MAN_GENDER");
                                }
                                else
                                {
                                    user.Add("gender", "WOMAN_GENDER");
                                }
                            }

                            if (user.HasKey("userId") && user["userId"] != null)
                            {
                                user.Add("userId", user["userId"].ToString());
                            }


                            if (user.HasKey("GcUserId") && user["GcUserId"] != null)
                            {
                                user.Add("gcUserId", user["GcUserId"].ToString());
                                user.Remove("GcUserId");
                            }

                            if (user.HasKeyNotNull("imageInfo"))
                            {
                                var image = user["imageInfo"].AsObject;
                                image.Add("id", image["id"].ToString());
                                user.Add("image", image);
                                user.Remove("imageInfo");
                            }

                            if (user.HasKeyNotNull("profileImage"))
                            {
                                user.Add("imageUrl", user["profileImage"]);
                                user.Remove("profileImage");
                            }

                            returnData.Add("result", user);
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
        * <div style='width: 100%;text-align: right'>دریافت اطلاعات کاربر</div>
        * <pre>
        *     <code style='float:right'>نمونه کد</code>
        *  <code>
        *        JSONObject userData = service.getUserData();
        *  </code>
        * </pre>
        *
        * @return
        *  <p>return data is JSONObject that has</p>
        *  <ul>
        *      <li>{string} id</li>
        *      <li>{string} name</li>
        *      <li>{string} token</li>
        *      <li>{string} peerId</li>
        *      <li>{bool} guest</li>
        *      <li>{JSONObject} [image]
        *          <ul>
        *              <li>{string} id</li>
        *              <li>{string} url</li>
        *              <li>{Integer} width</li>
        *              <li>{Integer} height</li>
        *          </ul>
        *      </li>
        * </ul>
        * */
        public JSONObject GetUserData()
        {
            return _userData;
        }
        
        /**
         * <div style='width: 100%;text-align: right'> جست و جو در بین کاربران گیم سنتر </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("name", "ali");
         *      reqData.put("size", 10);
         *      reqData.put("offset", 0);
         *      service.searchUserRequest(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("searchUserRequest method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} name - نام مستعار</li>
         *          <li>{Integer} [size=5]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر کاربر</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر کاربر</li>
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
         *                  <li>{JSONObject} users - JSONObject(key is userId) of JSONObject that  :
         *                      <ul>
         *                          <li>{string} name</li>
         *                          <li>{JSONObject} [image]
         *                              <ul>
         *                                  <li>{string}  id</li>
         *                                  <li>{string}  url</li>
         *                                  <li>{Integer} width</li>
         *                                  <li>{Integer} height</li>
         *                              </ul>
         *                          </li>
         *                      </ul>
         *                  </li>
         *                  <li>{bool} hasNext </li>
         *                  <li>{Integer} nextOffset  </li>
         *                  <li>{Integer} count  </li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void SearchUserRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string name = (Params.HasKey("name") && Params["name"] != null) ? Params["name"] : null;

                if (name == null)
                {
                    Debug.LogError("name not exist in Params");
                    return;
                    //throw new ServiceException("name not exist in Params");
                }

                var requestData = new JSONObject();
                requestData.Add("query", name);

                var size = (Params.HasKey("size") && Params["size"] != null) ? Params["size"].AsInt : 5;
                var offset = (Params.HasKey("offset") && Params["offset"] != null) ? Params["offset"].AsInt : 0;
                requestData.Add("size", size);
                requestData.Add("offset", offset);

                int? imageWidth = null;
                int? imageHeight = null;

                if (Params.HasKey("setting") && Params["setting"] != null)
                {
                    var setting = Params["setting"].AsObject;
                    if (setting != null)
                    {
                        if (setting.HasKey("imageWidth"))
                        {
                            imageWidth = setting["imageWidth"].AsInt;
                        }

                        if (setting.HasKey("imageHeight"))
                        {
                            imageHeight = setting["imageHeight"].AsInt;
                        }
                    }
                }

                //            int? finalImageWidth = imageWidth;
                //            int? finalImageHeight = imageHeight;

                Request(RequestUrls.SearchUser, requestData, result =>
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
                            var users = result["Result"].AsArray;
                            var usr = new JSONObject();

                            if (users != null)
                            {
                                var ownId = _userData["id"].ToString();
                                for (var i = 0; i < users.Count; i++)
                                {
                                    var user = users[i].AsObject;
                                    var userId = user["GcUserId"].ToString();


                                    if (userId.Equals(ownId))
                                    {
                                        continue;
                                    }

                                    var otherUserData = new JSONObject();
                                    if (user.HasKey("nickName"))
                                    {
                                        otherUserData.Add("name", user["nickName"].ToString());
                                    }
                                    else
                                    {
                                        otherUserData.Add("name", user["firstName"].ToString());
                                    }

                                    if (user.HasKey("imageInfo") && user["imageInfo"] != null)
                                    {
                                        var image = user["imageInfo"].AsObject;
                                        image.Add("id", image["id"].ToString());
                                        otherUserData.Add("image", image);
                                    }

                                    if (user.HasKey("profileImage") && user["profileImage"] != null)
                                    {
                                        otherUserData.Add("imageUrl", user["profileImage"]);
                                    }

                                    if (user.HasKey("score") && user["score"] != null)
                                    {
                                        otherUserData.Add("score", user["score"]);
                                    }

                                    if (user.HasKey("username") && user["username"] != null)
                                    {
                                        otherUserData.Add("username", user["username"]);
                                    }

                                    usr.Add(userId, otherUserData);
                                }
                            }


                            var retResult = new JSONObject();
                            retResult.Add("users", usr);
                            retResult.Add("hasNext", requestData["size"].AsInt == users.Count);
                            retResult.Add("nextOffset", requestData["offset"].AsInt + users.Count);
                            retResult.Add("count", result["Count"]);

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
         * <div style='width: 100%;text-align: right'> دریافت آیتم های کاربر </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId", "3");
         *      service.getUserItems(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getUserItems method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} gameId - شناسه بازی</li>
         *          <li>{Integer} [size=5]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{string} [itemId] - شناسه آیتم</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر آیتم</li>
         *                  <li>{Integer} [imageHeight]  رزولیشن عمودی تصویر آیتم</li>
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
         *                  <li>{JSONArray} items  - Array Of JSONObject that contain :
         *                      <ul>
         *                          <li>{string} id</li>
         *                          <li>{Integer} count</li>
         *                          <li>{JSONObject} item</li>
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
        public void GetUserItems(JSONObject Params, Action<JSONObject> onResult)
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

                int? imageWidth = null;
                int? imageHeight = null;

                if (Params.HasKey("setting") && Params["setting"] != null)
                {
                    var setting = Params["setting"].AsObject;
                    if (setting != null)
                    {
                        if (setting.HasKey("imageWidth"))
                        {
                            imageWidth = setting["imageWidth"].AsInt;
                        }

                        if (setting.HasKey("imageHeight"))
                        {
                            imageHeight = setting["imageHeight"].AsInt;
                        }
                    }
                }

                Request(RequestUrls.GetUserItems, requestData, result =>
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
                                items.Add(ReformatUserItem(rawItems[i].AsObject));
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
         *
         * <div style='width: 100%;text-align: right'>دریافت اطلاعات نفرات آنلاین </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId","gameId");
         *      service.getOnlineInfo(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getOnlineInfo method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{Integer} [gameId] - شناسه بازی
         *              <p> در صورت پر بودن این فیلد تعداد نفرات آنلاین بازی مشخص شده برگردانده می شود</p>
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
         *                  <li>{Integer} onlinePlayersCount  - تعداد کاربران آنلاین</li>
         *                  <li>{Integer} playersCount - تعداد کاربران</li>
         *                  <li>{Integer} score - امتیاز</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetOnlineInfo(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = Params != null && Params.HasKeyNotNull("gameId") ? Params["gameId"] : null;

                if (gameId != null)
                {
                    requestData.Add("gameId", gameId);
                }

                Request(RequestUrls.GetOnlineInfo, requestData, result =>
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
                            var res = result["Result"].AsObject;
                            retResult.Add("onlinePlayersCount", res["OnlinePlayersCount"].AsInt);
                            retResult.Add("playersCount", res["PlayersCount"].AsInt);
                            retResult.Add("score", res["Score"].AsInt);
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
         * <div style='width: 100%;text-align: right'> دریافت لیست دستاورد های کاربر </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      service.getUserAchievements(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getUserAchievements method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} [gameId]</li>
         *          <li>{Integer} [type]</li>
         *          <li>{string} [userId]</li>
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
         *                  <li>{string} count  </li>
         *                  <li>{string} name  </li>
         *                  <li>{string} imageUrl</li>
         *                  <li>{Integer} rank</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetUserAchievements(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = Params != null && Params.HasKeyNotNull("gameId") ? Params["gameId"] : null;
                string userId = Params != null && Params.HasKeyNotNull("userId") ? Params["userId"] : null;
                var type = Params != null && Params.HasKeyNotNull("type") ? Params["type"].AsInt : (int?) null;

                if (gameId != null)
                {
                    requestData.Add("gameId", gameId);
                }

                if (type != null)
                {
                    requestData.Add("type", type);
                }

                if (userId != null)
                {
                    requestData.Add("userId", userId);
                }

                Request(RequestUrls.UserAchievements, requestData, result =>
                {
                    var returnData = new JSONObject();
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorMessage", result["ErrorMessage"]);
                        returnData.Add("errorCode", result["ErrorCode"].AsInt);

                        var achivementsData = new JSONArray();
                        if (!hasError)
                        {
                            if (result.HasKeyNotNull("Result"))
                            {
                                var achivements = result["Result"].AsArray;

                                for (var i = 0; i < achivements.Count; i++)
                                {
                                    var achive = achivements[i].AsObject;
                                    var refAchive = new JSONObject();

                                    refAchive.Add("count", achive["Count"]);
                                    refAchive.Add("imageLink", achive["ImageLink"]);
                                    refAchive.Add("name", achive["Name"]);
                                    refAchive.Add("rank", achive["Rank"]);
                                    achivementsData.Add(refAchive);
                                }
                            }
                        }

                        returnData.Add("result", achivementsData);
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
         * <div style='width: 100%;text-align: right'> دریافت جزئیات یک دستاورد </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("rankType",1);
         *      service.getUserAchievementDetail(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getUserAchievementDetail method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{Integer} [rankType]</li>
         *          <li>{string} [gameId]</li>
         *          <li>{string} [size=50]</li>
         *          <li>{string} [offset]</li>
         *      </ul>
         *
         * @param  callback
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} achivements
         *                      <ul>
         *                          <li>{string} name  </li>
         *                          <li>{string} imageUrl</li>
         *                          <li>{Integer} rank</li>
         *                          <li>{JSONObject} leagueInfo
         *                              <ul>
         *                                  <li>{Double} expireTimestamp</li>
         *                                  <li>{Double} fromDateTimestamp</li>
         *                                  <li>{Integer} financialType</li>
         *                                  <li>{Integer} maxPlayers</li>
         *                                  <li>{Integer} statusNumber</li>
         *                                  <li>{bool} hasPrize</li>
         *                                  <li>{string} id</li>
         *                                  <li>{string} name</li>
         *                              </ul>
         *                          </li>
         *                      </ul>
         *                  </li>
         *                  <li>{Integer} count</li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetUserAchievementDetail(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = Params != null && Params.HasKeyNotNull("gameId") ? Params["gameId"] : null;
                var rankType = Params != null && Params.HasKeyNotNull("rankType")
                    ? Params["rankType"].AsInt
                    : (int?) null;
                var size = Params != null && Params.HasKeyNotNull("size") ? Params["size"].AsInt : 50;
                var offset = Params != null && Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (gameId != null)
                {
                    requestData.Add("gameId", gameId);
                }

                if (rankType != null)
                {
                    requestData.Add("rankType", rankType);
                }

                Request(RequestUrls.UserAchievementDetails, requestData, result =>
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
                                var achivements = result["Result"].AsArray;
                                var achivementsData = new JSONArray();

                                for (var i = 0; i < achivements.Count; i++)
                                {
                                    var achive = achivements[i].AsObject;
                                    var leagueInfo = achive["LeagueInfo"].AsObject;


                                    var newLeagueData = new JSONObject();
                                    newLeagueData.Add("expireTimestamp", leagueInfo["ExpireTimestamp"]);
                                    newLeagueData.Add("financialType", leagueInfo["FinancialType"]);
                                    newLeagueData.Add("fromDateTimestamp", leagueInfo["FromDateTimestamp"]);
                                    newLeagueData.Add("hasPrize", leagueInfo["HasPrize"]);
                                    newLeagueData.Add("id", leagueInfo["ID"].ToString());
                                    newLeagueData.Add("maxPlayers", leagueInfo["MaxPlayers"]);
                                    newLeagueData.Add("name", leagueInfo["Name"]);
                                    newLeagueData.Add("statusNumber", leagueInfo["StatusNumber"]);

                                    var refAchieveData = new JSONObject();

                                    refAchieveData.Add("imageUrl", achive["ImageLink"]);
                                    refAchieveData.Add("name", achive["Name"]);
                                    refAchieveData.Add("rank", achive["Rank"]);
                                    refAchieveData.Add("leagueInfo", newLeagueData);

                                    achivementsData.Add(refAchieveData);
                                }

                                retResult.Add("achivements", achivementsData);
                                retResult.Add("nextOffset", offset + achivements.Count);
                                retResult.Add("hasNext", size == achivements.Count);
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
         * <div style='width: 100%;text-align: right'> دریافت امتیاز کاربر در بازی ها </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      service.getUserGamePoints(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getUserGamePoints method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} [gameId]</li>
         *          <li>{string} [size=50]</li>
         *          <li>{string} [offset]</li>
         *      </ul>
         *
         * @param  callback
         *      <ul>
         *          <li>{bool} hasError</li>
         *          <li>{string} errorMessage</li>
         *          <li>{Integer} errorCode</li>
         *          <li>{JSONObject} result
         *              <ul>
         *                  <li>{JSONArray} points
         *                      <ul>
         *                          <li>{JSONObject} point
         *                              <ul>
         *                                  <li>{Long} amount</li>
         *                                  <li>{Long} creationDate</li>
         *                              </ul>
         *                          </li>
         *                          <li>{JSONObject} gameInfo
         *                              <ul>
         *                                  <li>{string} downloadLink</li>
         *                                  <li>{string} id</li>
         *                                  <li>{JSONObject} image</li>
         *                                  <li>{string} imageUrl</li>
         *                                  <li>{string} name</li>
         *                                  <li>{string} postId</li>
         *                                  <li>{string} timelineId</li>
         *                              </ul>
         *                          </li>
         *                      </ul>
         *                  </li>
         *                  <li>{Integer} count</li>
         *                  <li>{bool} hasNext</li>
         *                  <li>{Integer} nextOffset</li>
         *              </ul>
         *          </li>
         *      </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        public void GetUserGamePoints(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                var requestData = new JSONObject();

                string gameId = Params != null && Params.HasKeyNotNull("gameId") ? Params["gameId"] : null;
                var size = Params != null && Params.HasKeyNotNull("size") ? Params["size"].AsInt : 50;
                var offset = Params != null && Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (gameId != null)
                {
                    requestData.Add("gameId", gameId);
                }

                Request(RequestUrls.UserGamePoints, requestData, result =>
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
                                var points = result["Result"].AsArray;
                                var pointsData = new JSONArray();

                                for (var i = 0; i < points.Count; i++)
                                {
                                    var pointData = points[i].AsObject;
                                    var gameInfo = pointData["GameInfo"].AsObject;
                                    var point = pointData["Point"].AsObject;


                                    var newGameData = new JSONObject();
                                    var newPointData = new JSONObject();

                                    newGameData.Add("downloadLink", gameInfo["DownloadLink"]);
                                    newGameData.Add("id", gameInfo["EntityId"].ToString());
                                    newGameData.Add("postId", gameInfo["ID"].ToString());
                                    newGameData.Add("timelineId", gameInfo["TimelineId"].ToString());
                                    newGameData.Add("name", gameInfo["Name"]);

                                    if (gameInfo.HasKeyNotNull("Preview"))
                                    {
                                        newGameData.Add("imageUrl", gameInfo["Preview"]);
                                    }

                                    if (gameInfo.HasKey("PreviewInfo"))
                                    {
                                        if (gameInfo["PreviewInfo"] == null)
                                        {
                                            newGameData.Add("image", gameInfo["PreviewInfo"]);
                                        }
                                        else
                                        {
                                            var image = gameInfo["PreviewInfo"].AsObject;
                                            image.Add("id", image["id"].ToString());
                                            newGameData.Add("image", image);
                                        }
                                    }


                                    newPointData.Add("amount", point["Amount"]);
                                    newPointData.Add("creationDate", point["CreationDate"]);

                                    var refPointData = new JSONObject();

                                    refPointData.Add("point", newPointData);
                                    refPointData.Add("gameInfo", newGameData);

                                    pointsData.Add(refPointData);
                                }

                                retResult.Add("points", pointsData);
                                retResult.Add("nextOffset", offset + points.Count);
                                retResult.Add("hasNext", size == points.Count);
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
        
        private void _getOnlineUser(JSONObject requestData, Action<JSONObject> onResult)
        {
            Request(RequestUrls.OnlineUser, requestData, result =>
            {
                Debug.Log("Service.UserApi._getOnlineUer.Result:\n\n" + PrettyJson(result));
                var resData = new JSONObject();
                try
                {
                    var hasError = result["HasError"].AsBool;
                    resData.Add("hasError", hasError);
                    if (hasError)
                    {
                        resData.Add("errorMessage", result["ErrorMessage"].ToString());
                        resData.Add("errorCode", result["ErrorCode"].ToString());
                        onResult(resData);
                    }
                    else
                    {
                        var innerRes = result["Result"].AsObject;
                        var users = innerRes["users"].AsArray;
                        var usr = new JSONObject();

                        for (var i = 0; i < users.Count; i++)
                        {
                            var user = users[i].AsObject;
                            var userId = user["UserID"].ToString();
                            var otherUsersData = new JSONObject();
                            otherUsersData.Add("name", user["Name"].ToString());

                            if (user.HasKey("Image") && user["Image"] != null)
                            {
                                var image = user["Image"].AsObject;
                                image.Add("id", image["id"].ToString());
                                otherUsersData.Add("image", image);
                            }


                            if (user.HasKey("ProfileImage") && user["ProfileImage"] != null)
                            {
                                otherUsersData.Add("imageUrl", user["ProfileImage"]);
                            }


                            usr.Add(userId, otherUsersData);
                        }

                        var resultData = new JSONObject();
                        var index = requestData["index"].AsInt;
                        resultData.Add("users", usr);
                        resultData.Add("count", innerRes["count"]);
                        resultData.Add("hasNext", (index + 1) * requestData["count"].AsInt < users.Count);
                        resultData.Add("nextOffset", index + 1);
                        resData.Add("result", resultData);

                        onResult(resData);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }
            });
        }
        
        private JSONObject ReformatUserItem(JSONObject item)
        {
            var returnData = new JSONObject();
            try
            {
                returnData.Add("id", item["ID"].ToString());
                returnData.Add("count", item["Count"]);
                returnData.Add("item", ReformatGameItem(item["Item"].AsObject));
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }

            return returnData;
        }
        
        private JSONObject ReformatLiveObject(JSONObject live)
        {
            var newLiveObj = new JSONObject();

            newLiveObj.Add("url", live["Url"].ToString());

            if (live.HasKeyNotNull("Match"))
            {
                newLiveObj.Add("match", ReformatMatchObject(live["Match"].AsObject));
            }
            else
            {
                newLiveObj.Add("match", null);
            }

            return newLiveObj;
        }
    
    }
}

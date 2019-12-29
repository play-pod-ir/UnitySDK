using System;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity
{
    public partial class Service {
        
        
    
    /**
         * <div style='width: 100%;text-align: right'> دریافت پک های بازی </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId", "2");
         *      service.getInAppPurchasePack(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getInAppPurchasePack method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} gameId - شناسه بازی </li>
         *          <li>{Integer} [size=5]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{string} [packId] - شناسه پک</li>
         *          <li>{string} [itemId] - شناسه آیتم</li>
         *          <li>{string} [nameFilter] - فیلتر بر اساس نام پک</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر پک</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر پک</li>
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
         *                  <li>{JSONArray} packs - آرایه ای از آبجک  :
         *                      <ul>
         *                          <li>{string} id - شناسه پک</li>
         *                          <li>{string} name - نام پک</li>
         *                          <li>{string} description - توضیحات</li>
         *                          <li>{Integer} count - تعداد</li>
         *                          <li>{bool} visible</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} allowedTimesToBuy</li>
         *                          <li>{JSONObject} item</li>
         *                          <li>{JSONObject} plan</li>
         *                              <li>{string} id</li>
         *                              <li>{Integer} type</li>
         *                              <li>{Integer} cycle</li>
         *                              <li>{Double} fromDate</li>
         *                              <li>{Double} ToDate</li>
         *                          <li>{Double} price - قیمت</li>
         *                          <li>{string} priceText - قیمت به همراه واحد آن</li>
         *                          <li>{string} priceUnit - واحد قیمت</li>
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
        public void GetInAppPurchasePack(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string gameId = Params.HasKeyNotNull("gameId") ? Params["gameId"] : null;
                string itemId = Params.HasKeyNotNull("itemId") ? Params["itemId"] : null;

                if (gameId == null && itemId == null)
                {
                    Debug.LogError("either gameId or itemId key must br in Params");
                    return;
                    //throw new ServiceException("either gameId or itemId key must br in Params");
                }

                var requestData = new JSONObject();
                if (gameId != null)
                {
                    requestData.Add("entityId", gameId);
                }

                string packId = Params.HasKeyNotNull("packId") ? Params["packId"] : null;
                string nameFilter = Params.HasKeyNotNull("nameFilter") ? Params["nameFilter"] : null;

                var size = Params.HasKeyNotNull("size") ? Params["size"].AsInt : 10;
                var offset = Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                if (packId != null)
                {
                    requestData.Add("packId", packId);
                }

                if (itemId != null)
                {
                    requestData.Add("itemId", itemId);
                }

                if (nameFilter != null)
                {
                    requestData.Add("query", nameFilter);
                }

                Request(RequestUrls.GetInAppPurchasePack, requestData, result =>
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
                            var packs = new JSONArray();
                            var rawPacks = result["Result"].AsArray;
                            for (var i = 0; i < rawPacks.Count; i++)
                            {
                                packs.Add(ReformatInAppPack(rawPacks[i].AsObject));
                            }

                            var retResult = new JSONObject();
                            retResult.Add("packs", packs);
                            retResult.Add("hasNext", requestData["size"].AsInt == packs.Count);
                            retResult.Add("nextOffset", requestData["offset"].AsInt + packs.Count);

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
         * <div style='width: 100%;text-align: right'> دریافت پک های گیم سنتری </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("itemId", "4455");
         *      service.getGlobalInAppPurchasePack(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getGlobalInAppPurchasePack method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{Integer} [size=5]</li>
         *          <li>{Integer} [offset=0]</li>
         *          <li>{string} [itemId] - شناسه آیتم</li>
         *          <li>{JSONObject} [setting]
         *              <ul>
         *                  <li>{Integer} [imageWidth] اندازه رزولیشن افقی تصویر پک</li>
         *                  <li>{Integer} [imageHeight] اندازه رزولیشن عمودی تصویر پک</li>
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
         *                  <li>{JSONArray} packs - آرایه ای از آبجک  :
         *                      <ul>
         *                          <li>{string} id - شناسه پک</li>
         *                          <li>{string} name - نام پک</li>
         *                          <li>{string} description - توضیحات</li>
         *                          <li>{Integer} count - تعداد</li>
         *                          <li>{bool} visible</li>
         *                          <li>{bool} enable</li>
         *                          <li>{bool} allowedTimesToBuy</li>
         *                          <li>{JSONObject} item</li>
         *                          <li>{JSONObject} plan</li>
         *                              <li>{string} id</li>
         *                              <li>{Integer} type</li>
         *                              <li>{Integer} cycle</li>
         *                              <li>{Double} fromDate</li>
         *                              <li>{Double} ToDate</li>
         *                          <li>{Double} price - قیمت</li>
         *                          <li>{string} priceText - قیمت به همراه واحد آن</li>
         *                          <li>{string} priceUnit - واحد قیمت</li>
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
        public void GetGlobalInAppPurchasePack(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string itemId = Params.HasKeyNotNull("itemId") ? Params["itemId"] : null;

                if (itemId == null)
                {
                    Debug.LogError("itemId not exist in Params");
                    return;
                    //throw new ServiceException("itemId not exist in Params");
                }

                var requestData = new JSONObject();
                requestData.Add("itemId", itemId);

                var size = Params.HasKeyNotNull("size") ? Params["size"].AsInt : 10;
                var offset = Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                Request(RequestUrls.GetGlobalInAppPurchasePack, requestData, result =>
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
                            var packs = new JSONArray();
                            var rawPacks = result["Result"].AsArray;
                            for (var i = 0; i < rawPacks.Count; i++)
                            {
                                packs.Add(ReformatInAppPack(rawPacks[i].AsObject));
                            }

                            var retResult = new JSONObject();
                            retResult.Add("packs", packs);
                            retResult.Add("hasNext", requestData["size"].AsInt == packs.Count);
                            retResult.Add("nextOffset", requestData["offset"].AsInt + packs.Count);

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
         * <div style='width: 100%;text-align: right'> مصرف آیتم کاربر </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *            JSONObject reqData = new JSONObject();
         *            reqData.put("itemId", "1061");
         *            reqData.put("count", 10);
         *            service.consumeItem(reqData, new RequestCallback() {
         *                {@code @Override}
         *                public void onResult(JSONObject result) {
         *                    System.out.println("consumeItem method : " + result);
         *                }
         *            });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
         *          <li>{string} itemId - شناسه آیتم</li>
         *          <li>{Integer} count - تعداد مصرف شده</li>
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
        public void ConsumeItemRequest(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string itemId = Params.HasKeyNotNull("itemId") ? Params["itemId"] : null;
                var count = Params.HasKeyNotNull("count") ? Params["count"].AsInt : (int?) null;

                if (itemId == null)
                {
                    Debug.LogError("itemId not exist in Params");
                    return;
                    //throw new ServiceException("itemId not exist in Params");
                }

                if (count == null)
                {
                    Debug.LogError("count not exist in Params or must be greater than 0");
                    return;
                    //throw new ServiceException("count not exist in Params or must be greater than 0");
                }

                Request(RequestUrls.ConsumeItem, Params, result =>
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
         * <div style='width: 100%;text-align: right'> دریافت آیتم های مربوط به گیم سنتر </div>
         * <pre>
         *  <code style='float:right'>نمونه کد</code>
         *  <code>
         *      JSONObject reqData = new JSONObject();
         *      reqData.put("gameId", "3");
         *      service.getUserGameCenterItem(reqData, new RequestCallback() {
         *          {@code @Override}
         *          public void onResult(JSONObject result) {
         *              System.out.println("getUserGameCenterItem method : " + result);
         *          }
         *      });
         *  </code>
         * </pre>
         * @param  Params
         *      <ul>
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
        public void GetUserGameCenterItem(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string itemId = Params.HasKeyNotNull("itemId") ? Params["itemId"] : null;

                if (itemId == null)
                {
                    Debug.LogError("itemId not exist in Params");
                    return;
                    //throw new ServiceException("itemId not exist in Params");
                }

                var requestData = new JSONObject();

                var size = Params.HasKeyNotNull("size") ? Params["size"].AsInt : 10;
                var offset = Params.HasKeyNotNull("offset") ? Params["offset"].AsInt : 0;

                requestData.Add("size", size);
                requestData.Add("offset", offset);

                requestData.Add("itemId", itemId);

                Request(RequestUrls.GetUserGcItems, requestData, result =>
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
    }
}

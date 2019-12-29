using System;
using SimpleJSON;

namespace playpod.client.sdk.unity.Base
{
    /// <summary>
    /// اعلام نتیجه درخواست مسابقه
    /// </summary>
    public class MatchRequestCallback
    {
        private readonly Action<JSONObject> _onResult;
        private readonly Action<JSONObject> _onAccept;
        private readonly Action<JSONObject> _onReject;
        private readonly Action<JSONObject> _onCancel;

        public MatchRequestCallback(Action<JSONObject> onResult, Action<JSONObject> onAccept,
            Action<JSONObject> onReject, Action<JSONObject> onCancel)
        {
            this._onResult = onResult;
            this._onAccept = onAccept;
            this._onReject = onReject;
            this._onCancel = onCancel;
        }

        /**
         *
         *<div style='width: 100%;text-align: right'> بعد از اعلام درخواست به سرور این متد فراخوانی می شود</div>
         * @param  result
         *  <ul>
         *      <li>{Boolean} hasError</li>
         *      <li>{string} errorMessage</li>
         *      <li>{Integer} errorCode</li>
         *      <li>{JSONObject} result
         *              <ul>
         *                <li>{string} requestId شناسه درخواست</li>
         *              </ul>
         *       </li>
         *  </ul>
         */
        public void OnResult(JSONObject result)
        {
            _onResult(result);
        }

        /**
         *<div style='width: 100%;text-align: right'> در صورت پذیرش درخواست مسابقه این متد فراخوانی می شود </div>
         *
         * @param  data
         *  <ul>
         *     <li>{string} requestId شناسه درخواست</li>
         *  </ul>
         */
        public void OnAccept(JSONObject data)
        {
            _onAccept(data);
        }

        /**
         *<div style='width: 100%;text-align: right'>  در صورت رد درخواست مسابقه این متد فراخوانی می شود </div>
         * @param  data
         *  <ul>
         *     <li>{string} requestId شناسه درخواست</li>
         *     <li>{string} rejectMessage پیام رد درخواست</li>
         *  </ul>
         */
        public void OnReject(JSONObject data)
        {
            _onReject(data);
        }

        /**
         *<div style='width: 100%;text-align: right'> در صورت کنسل شدن درخواست مسابقه این متد فراخوانی می شود </div>
         * @param  data
         *  <ul>
         *     <li>{string} requestId شناسه درخواست</li>
         *  </ul>
         */
        public void OnCancel(JSONObject data)
        {
            _onCancel(data);
        }
    }
}
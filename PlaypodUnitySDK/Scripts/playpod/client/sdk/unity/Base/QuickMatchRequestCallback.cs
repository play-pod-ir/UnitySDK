using System;
using SimpleJSON;

namespace playpod.client.sdk.unity.Base
{
    /**
 *<div style='width: 100%;text-align: right'>اعلام نتیجه حریف می طلبم</div>
 * */
    public class QuickMatchRequestCallback
    {
        private readonly Action<JSONObject> _onResult;
        private readonly Action<JSONObject> _onCancel;
        private readonly Action<JSONObject> _onAccept;

        public QuickMatchRequestCallback(
            Action<JSONObject> onResult,
            Action<JSONObject> onCancel,
            Action<JSONObject> onAccept
        )
        {
            this._onResult = onResult;
            this._onCancel = onCancel;
            this._onAccept = onAccept;
        }

        /**
         *<div style='width: 100%;text-align: right'>بعد از اعلام درخواست به سرور و گرفتن نتیجه درخواست این متد فراخوانی می شود </div>
         * @param  result
         *  <ul>
         *      <li>{Boolean} hasError</li>
         *      <li>{String} errorMessage</li>
         *  </ul>
         *
         */
        public void OnResult(JSONObject result)
        {
            _onResult(result);
        }

        /**
         *<div style='width: 100%;text-align: right'>بعد از ایجاد مسابقه ای از طریق حریف می طلبم این متد فراخوانی می شود</div>
         * @param  data
         *  <ul>
         *     <li>{String} leagueId شناسه لیگ</li>
         *  </ul>
         */
        public void OnCancel(JSONObject data)
        {
            _onCancel(data);
        }

        /**
         *<div style='width: 100%;text-align: right'>در صورتی که درخواست حریف می طلبم کنسل شود این متد فراخوانی می شود</div>
         * @param  data
         *  <ul>
         *     <li>{String} leagueId شناسه لیگ</li>
         *     <li>{String} message  پیام مربوط به کنسل شدن درخواست</li>
         *  </ul>
         */
        public void OnAccept(JSONObject data)
        {
            _onAccept(data);
        }
    }
}
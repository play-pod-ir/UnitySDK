using System;
using SimpleJSON;

namespace playpod.client.sdk.unity.Base
{
    public class EventCallback
    {
        private readonly Action<JSONObject> _onFireAction;
        private readonly Action<JSONObject, AsyncResponse> _onFireAsyncAction;

        public EventCallback(Action<JSONObject> onFire)
        {
            this._onFireAction = onFire;
        }

        public EventCallback(Action<JSONObject, AsyncResponse> onFire)
        {
            this._onFireAsyncAction = onFire;
        }

        public EventCallback(Action<JSONObject> onFire, Action<JSONObject, AsyncResponse> onFireAsync)
        {
            this._onFireAction = onFire;
            this._onFireAsyncAction = onFireAsync;
        }

        public void OnFire(JSONObject msg)
        {
            _onFireAction(msg);
        }

        public void OnFire(JSONObject msg, AsyncResponse res)
        {
            _onFireAsyncAction(msg, res);
        }
    }
}
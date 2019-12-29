using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using playpod.client.sdk.unity.Base;
using playpod.client.sdk.unity.Base.Game;
using playpod.client.sdk.unity.Network;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace playpod.client.sdk.unity
{
    /**
     *
     *کلاس اصلی سرویس
     *
     * @see <a href="{@docRoot}/TISIntro/index.html" style='font-size: large;'>شروع کار با سرویس</a>
      */
    public partial class Service : Network.Network.IParent
    {
        AndroidJavaObject _javaPluginObject;

        public Logger _logger;

        public const string ServiceEventReady = "ready";
        public const string ServiceEventLogin = "login";
        public const string ServiceEventConnect = "connect";

        public const string ServiceEventDefaultLeagueSubscribe = "defaultLeagueSubscribe";

        #region ConfStr

        private const string CONF_STR = "{" +
                                        "\"CONNECTINGTOPUSH\":{" +
                                        "\"EN\":\"connecting to push\"," +
                                        "\"FA\":\"در حال ارتباط با سرور لحظاتی دیگر امتحان کنید\"" +
                                        "}," +
                                        "\"GAME\":{" +
                                        "\"EAndroid SQLiteN\":\"game\"," +
                                        "\"FA\":\"بازی\"" +
                                        "}," +
                                        "\"CONNECTIONERROR\":{" +
                                        "\"EN\":\" connection error\"," +
                                        "\"FA\":\"خطا در برقراری ارتباط با سرور\"" +
                                        "}," +
                                        "\"NOTONLINE\":{" +
                                        "\"EN\":\"you are not online.\"," +
                                        "\"FA\":\"ارتباط شما با اینترنت برقرار نیست\"" +
                                        "}," +
                                        "\"NOTAUTHENTICATE\":{" +
                                        "\"EN\":\"cannot authenticate user\"," +
                                        "\"FA\":\"شماره تماس یا رمز عبور اشتباه می باشد\"" +
                                        "}," +
                                        "\"NOTIFREQUESTMATCH\":{" +
                                        "\"EN\":\"request match.\"," +
                                        "\"FA\":\" به شما درخواست بازی داده است. \"" +
                                        "}," +
                                        "\"MATCHREQUESTCANCELD\":{" +
                                        "\"EN\":\"request match.\"," +
                                        "\"FA\":\" به شما درخواست بازی داده است. \"" +
                                        "}," +
                                        "\"MATCHREQUESTVERSIONFAIL\":{" +
                                        "\"EN\":\"request match.\"," +
                                        "\"FA\":\"  به شما درخواست بازی داده است,ولی به دلیل عدم بروزرسانی برنامه شما ,درخواست کنسل گردید. \"" +
                                        "}," +
                                        "\"MATCHREQUEST\":{" +
                                        "\"EN\":\"request match.\"," +
                                        "\"FA\":\"درخواست بازی\"" +
                                        "}," +
                                        "\"MATCHREQUESTIN\":{" +
                                        "\"EN\":\"request match in.\"," +
                                        "\"FA\":\"درخواست بازی در \"" +
                                        "}," +
                                        "\"HASMAJORCONFLICT\":{" +
                                        "\"EN\":\"major conflict.\"," +
                                        "\"FA\":\"نسخه کنونی شما بروز نمی باشد. لطفا بازی خود را بروزرسانی کنید\"" +
                                        "}," +
                                        "\"UPDATE\":{" +
                                        "\"EN\":\"update\"," +
                                        "\"FA\":\"بروزرسانی\"" +
                                        "}," +
                                        "\"MATCHREQUESTS\":{" +
                                        "\"EN\":\"match requests\"," +
                                        "\"FA\":\"درخواست های بازی\"" +
                                        "}," +
                                        "\"MATCHREQUESTFROM\":{" +
                                        "\"EN\":\"match requests from \"," +
                                        "\"FA\":\"درخواست بازی از \"" +
                                        "}," +
                                        "\"HAVE\":{" +
                                        "\"EN\":\"have\"," +
                                        "\"FA\":\"داشته اید.\"" +
                                        "}," +
                                        "\"NEWVERSION\":{" +
                                        "\"EN\":\"new version\"," +
                                        "\"FA\":\"نسخه جدید\"" +
                                        "}," +
                                        "\"GAMEISRUN\":{" +
                                        "\"EN\":\"you are plying game\"," +
                                        "\"FA\":\"تا پایان مسابقه نمی توانید درخواست حریف بدهید.\"" +
                                        "}," +
                                        "\"MAXCONCURRENTREQUEST\":{" +
                                        "\"EN\":\"request result to : \"," +
                                        "\"FA\":\"حداکثر درخواست همزمان $VAR نفر میباشد\"" +
                                        "}," +
                                        "\"SHAREGAMEMESSAGE\":{" +
                                        "\"EN\":\"hi,visit this link,$VAR\"," +
                                        "\"FA\":\"از این بازی خیلی خوشم اومده! نصبش کن بیا بازی کنیم. منتظرتم.$VAR\"" +
                                        "}," +
                                        "\"SHARELEAGUEMESSAGE\":{" +
                                        "\"EN\":\"hi,visit this link,$VAR\"," +
                                        "\"FA\":\" این لیگ خیلی باحاله , یه نیگاه بهش بنداز\n$VAR\"" +
                                        "}," +
                                        "\"WAITFORPREVIOUSREQUEST\":{" +
                                        "\"EN\":\"wait for previous request\"," +
                                        "\"FA\":\"منتظر نتیجه درخواست قبلی بمانید\"" +
                                        "}," +
                                        "\"CAN_ACCEPT_MATCH_REQUEST_AFTER_MATCH\":{" +
                                        "\"EN\":\"you can accept request after match\"," +
                                        "\"FA\":\"بعداز مسابقه می توانید درخواست جدید را بپذیرید\"" +
                                        "}," +
                                        "\"CANTNOTREQUESTINPLAING\":{" +
                                        "\"EN\":\"wait for previous request\"," +
                                        "\"FA\":\"درخواست جدید در حین بازی مجاز نمی باشد\"" +
                                        "}," +
                                        "\"NOTOPPONENTFIND\":{" +
                                        "\"EN\":\"not opponent find\"," +
                                        "\"FA\":\"حریفی پیدا نشد\"" +
                                        "}," +
                                        "\"ADDMIN1PHONENUMBER\":{" +
                                        "\"EN\":\"pleas add minimum one number\"," +
                                        "\"FA\":\"حداقل یک شماره تماس را وارد نمایید\"" +
                                        "}," +
                                        "\"ERRORINPROCESS\":{" +
                                        "\"EN\":\"error in operation\"," +
                                        "\"FA\":\"خطایی در اجرای درخواست شما رخ داد\"" +
                                        "}," +
                                        "\"MATCHSTART\":{" +
                                        "\"EN\":\"match started.\"," +
                                        "\"FA\":\"شروع مسابقه\"" +
                                        "}," +
                                        "\"MATCHSTARTMESSAGE\":{" +
                                        "\"EN\":\"your match started.\"," +
                                        "\"FA\":\"مسابقه شما با $VAR شروع شده است\"" +
                                        "}," +
                                        "\"DOWNLOADINGNEWVERSION\":{" +
                                        "\"EN\":\"downloading .\"," +
                                        "\"FA\":\"در حال دریافت نسخه جدید \"}" +
                                        "}";

        #endregion

        private string _lang = "FA";
        private string _appId = "GC_ANDROID";

        private string _deviceId;

        /**
         * @deprecated
         * */
        private static string Version = "0.1.6";

        private int _type = 2;
        private Network.Network _network;
        private JSONObject _userData;
        private JSONObject _loginData;
        private JSONObject _games;
        private JSONObject _dic;

        private Dictionary<string, Dictionary<string, EventCallback>> _eventCallback =
            new Dictionary<string, Dictionary<string, EventCallback>>();

        private Dictionary<string, MultiPlayer> _activeMatch = new Dictionary<string, MultiPlayer>();

        private Dictionary<string, bool?> _gameCenterMessagesId = new Dictionary<string, bool?>();
        private Dictionary<string, Dictionary<string, object>> _activeMatchRequest;

        private Dictionary<string, string> _requestIdLeague = new Dictionary<string, string>();

        //    private HashMap<string, MultiPlayer> activeMatches = new HashMap<string, MultiPlayer>();
        private Dictionary<string, object> _quickMatchData = new Dictionary<string, object>();
        //private Context context;

        private bool _isMultiTab = true;

        private bool _isCheckLoginActionWithPeer;
        private readonly bool _autoMatchRequestAccept = false;

        //    private bool chatServiceEnable = false;

        //private Assets.Scripts.Database.Database database;
        //private string dbName = "TIS_SERVICE";

        //    private EncryptHandshakeData encryptHandshakeData = new EncryptHandshakeData();
        private int _currentMatchRequestCount;

        private bool _syncPeerWithToken = false;
        private bool _syncTokenWithPeer;

        private bool _peerAndTokenSync;
        private bool _isReady;

        private PlaypodService _mono;
        private GameObject TisService;
        private StateMachine _stateMachine;

        //    private int? tokenIssuer = null;
        private JSONObject _temporaryPeerData;

        //    static Logger log = Logger.getLogger(Service.class);
        private static Service _instance;

        public Service()
        {
            
        }

        /**
         * <div style='width: 100%;text-align: right'>کلاس اصلی برای استفاده از سرویس </div>
         *  <pre>
         *      <code>

         *        JSONObject Params = new JSONObject();
         *        try {
         *
         *            JSONObject games = new JSONObject();
         *            JSONObject game = new JSONObject();
         *            game.put("callback", "com.myGame");
         *            games.put(gameId, game);
         *
         *            Params.put("context", this.getBaseContext());// android application main activity Context
         *            Params.put("version", gameVersion);
         *            Params.put("games", games);// not necessary for game center application
         *
         *
         *            final Service tis = new Service(Params);
         *
         *        } catch (JSONException e) {
         *            e.printStackTrace();
         *        }
         *      </code>
         *  </pre>
         * @param Params
         * <ul>
         *     <li>{Context} context</li>
         * </ul>
         *
         * @throws ServiceException خطای پارامتر های ورودی
         * */
        private Service(JSONObject Params)
        {
            if (_instance != null)
            {
                Debug.LogError("service is init,get instance of it");
                return;
                //throw new ServiceException("service is init,get instance of it");
            }

            _instance = this;

            Initialize(Params);
        }

        // add above here

        /// <summary>
        /// دریافت یک اینستنس از سرویس
        /// </summary>
        /// <param name="Params">
        /// پارامترهای ورودی
        /// </param>
        /// <returns></returns>
        public Service GetInstance(JSONObject Params, PlaypodService mono, StateMachine stateMachine)
        {
            if (_instance == null)
            {
                CreateService(Params, mono, stateMachine);
            }
            
            return _instance;
        }

        private void CreateService(JSONObject Params, PlaypodService mono, StateMachine stateMachine)
        {
//            Debug.Log("Service.createService\n\nParams: " + PrettyJson(Params));
            if (_instance != null)
            {
                Debug.LogError("service is init,get instance of it");
                return;
                //throw new ServiceException("service is init,get instance of it");
            }
            
            if (mono == null || stateMachine == null)
            {
                Debug.LogError("Invalid Service Params");
                return;
                //throw new ServiceException("service is init,get instance of it");
            }
            
            _mono = mono;
            _stateMachine = stateMachine;
            
            _userData = new JSONObject();
            _games = new JSONObject();

            _lang = "FA";
            _appId = "default_appId";

            _network = new Network.Network();

            _isMultiTab = true;

            _requestIdLeague = new Dictionary<string, string>();

            _activeMatchRequest = new Dictionary<string, Dictionary<string, object>>();

            _eventCallback =
                new Dictionary<string, Dictionary<string, EventCallback>>();

            _activeMatch = new Dictionary<string, MultiPlayer>();

            _gameCenterMessagesId = new Dictionary<string, bool?>();


            _instance = this;

            Initialize(Params);
        }

        public static Service GetInstance()
        {
            if (_instance == null)
            {
                Debug.LogError("get instance with parameters");
                throw new ServiceException("get instance with parameters");
            }

            return _instance;
        }

        private void Initialize(JSONObject Params)
        {
//            Debug.Log("Initializing Service...\n\nParams: " + PrettyJson(Params) + "\n");

            try
            {
                _temporaryPeerData = new JSONObject();
                _temporaryPeerData.Add("peerId", null);
                _temporaryPeerData.Add("lastTime", 0);
                _temporaryPeerData.Add("deviceId", Guid.NewGuid().ToString());
                _temporaryPeerData.Add("isLoading", false);

                //Debug.Log("START_OF_SERVICE_1 " + Params);

                _dic = JSON.Parse(CONF_STR).AsObject;

                if (Params.HasKey("deviceId") && Params["deviceId"] != null)
                {
                    _deviceId = Params["deviceId"].ToString();
                }

                if (Params.HasKey("serviceMode") && Params["serviceMode"] != null)
                {
                    var mode = (string) Params["serviceMode"];
                    if (mode.Equals(ServiceModeTypes.DevelopmodeLocal) ||
                        mode.Equals(ServiceModeTypes.DevelopmodeOnline))
                    {
                        ConfigData.ServiceMode = mode;
                        ConfigData.IsLocal = true;
                        ConfigData.Init(null);
                    }
                }
                else
                {
                    if (Params.HasKey("isLocal") && Params["isLocal"] != null)
                    {
                        ConfigData.IsLocal = (bool) Params["isLocal"];
                        ConfigData.Init(null);
                    }
                }

                RegisterGame(Params);

#if UNITY_ANDROID && !UNITY_EDITOR

                string[] gameID;
                string gameVersion = "";

                List<string> gamesId = new List<string>();

                foreach (var key in _games.Keys)
                {
                    gamesId.Add(key);
                }

                gameID = gamesId.ToArray();

                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
                _javaPluginObject = new AndroidJavaObject("com.fanap.gameCenter.unity.PlaypodServiceWrapper", 
                    context,
                    gameID, 
                    gameVersion,
                    true,
                    _serviceMode);

#endif                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

                if (Params.HasKey("loginData") && Params["loginData"] != null)
                {
                    _loginData = Params["loginData"].AsObject;
//                    Debug.Log("loginData -- " + _loginData.ToString());
                    Init();
                }
                else
                {
                    getUserDataInit(_mono.getPlayPodServiceObject(), _mono.OnGotUserDataInit);
                }
            }

            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        private void RegisterGame(JSONObject Params)
        {
//            Debug.Log("registerGame...\n\nParams:\n" + PrettyJson(Params));
            try
            {
                var gamesParams = (Params.HasKeyNotNull("games")) ? Params["games"].AsObject : null;

                if (gamesParams != null)
                {
                    var gamesId = new JSONArray();

                    foreach (var key in gamesParams.Keys)
                    {
                        gamesId.Add(key);
                    }

                    for (var i = 0; i < gamesId.Count; i++)
                    {
                        string gameId = gamesId[i];

                        var gameParams = gamesParams[gameId].AsObject;
                        JSONObject setting;
                        JSONObject info;

                        if (!gameParams.HasKey("info") || gameParams["info"] == null)
                        {
                            info = new JSONObject();
                            info.Add("name", _dic["GAME"].AsObject[_lang]);
                            gameParams.Add("info", info);
                        }
                        else
                        {
                            info = gameParams["info"].AsObject;
                            if (!info.HasKey("name") || info["name"] == null)
                            {
                                info.Add("name", _dic["GAME"].AsObject[_lang]);
                            }
                        }

                        if (!gameParams.HasKey("setting") || gameParams["setting"] == null)
                        {
                            setting = new JSONObject();
                            gameParams.Add("setting", setting);
                        }
                        else
                        {
                            setting = gameParams["setting"].AsObject;
                        }


                        var gameData = new JSONObject();
                        gameData.Add("info", info);
                        gameData.Add("setting", setting);
                        gameData.Add("isRun", false);
                        gameData.Add("version", gameParams["version"]);

                        if (gameParams.HasKeyNotNull("callback"))
                        {
                            gameData.Add("callback", gameParams["callback"]);
                        }

                        _games.Add(gameId, gameData);

                        this._appId = gameId;
                        this._type = 1;
                    }

//                    Debug.Log("games:\n" + PrettyJson(_games));
                }
                else
                {
                    Debug.LogError("Please enter the information (game id and game version) for at least 1 game");
                    //throw new ServiceException("");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        public void InitWithUserData(JSONObject Params)
        {
//            LOG("InitWithUserData: " + Util.PrettyJson(Params));
            _loginData = Params;
            Init();
        }

        private void Init()
        {
            //Debug.Log("INIT_SERVICE ");
            if (_deviceId == null)
            {
                _deviceId = SystemInfo.deviceUniqueIdentifier;
            }

            if (_eventCallback == null)
            {
                _eventCallback = new Dictionary<string, Dictionary<string, EventCallback>>();
            }

            _eventCallback.Add("ready", new Dictionary<string, EventCallback>());
            _eventCallback.Add("login", new Dictionary<string, EventCallback>());
            _eventCallback.Add("guestLogin", new Dictionary<string, EventCallback>());
            _eventCallback.Add("logout", new Dictionary<string, EventCallback>());
            _eventCallback.Add("connect", new Dictionary<string, EventCallback>());
            _eventCallback.Add("disconnect", new Dictionary<string, EventCallback>());
            _eventCallback.Add("reconnect", new Dictionary<string, EventCallback>());
            _eventCallback.Add("message", new Dictionary<string, EventCallback>());
            _eventCallback.Add("defaultLeagueSubscribe", new Dictionary<string, EventCallback>());

            _eventCallback.Add("buyPack", new Dictionary<string, EventCallback>());
            _eventCallback.Add("creditChange", new Dictionary<string, EventCallback>());
            _eventCallback.Add("matchRequestResponse", new Dictionary<string, EventCallback>());
            _eventCallback.Add("profileChange", new Dictionary<string, EventCallback>());
            
            _eventCallback.Add("newMatch", new Dictionary<string, EventCallback>());
            _eventCallback.Add("matchReceiveData", new Dictionary<string, EventCallback>());
            _eventCallback.Add("matchStart", new Dictionary<string, EventCallback>());
            _eventCallback.Add("matchPause", new Dictionary<string, EventCallback>());
            _eventCallback.Add("matchResume", new Dictionary<string, EventCallback>());
            _eventCallback.Add("matchEnd", new Dictionary<string, EventCallback>());
            _eventCallback.Add("matchLeave", new Dictionary<string, EventCallback>());
            _eventCallback.Add("matchRequest", new Dictionary<string, EventCallback>());

            var leaguesQuickData = new Dictionary<string, object>();

            if (_quickMatchData == null)
            {
                _quickMatchData = new Dictionary<string, object>();
            }

            _quickMatchData.Add("leagues", leaguesQuickData);
            _quickMatchData.Add("requestCount", 0);

            InitDatabase(Params =>
            {
                //Debug.Log("Database_Init");

                try
                {
                    InitNetwork();
                    ApplyConfigData();
                }
                catch (ServiceException e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }
            });
        }

        private void ApplyConfigData()
        {
            //Debug.Log("applyConfigData");
            if (ConfigData.ServiceMode.Equals(ServiceModeTypes.DevelopmodeLocal))
            {
                ConfigData.Init(null);
                ReadyService();
            }
            else
            {
                _getConfigFromServer(res =>
                {
//                    LOG("_getConfigFromServer.result: " + Util.PrettyJson(res));
                    try
                    {
                        if (!res["HasError"].AsBool)
                        {
                            ConfigData.Init(res);
                            ReadyService();
                        }
                        else
                        {
                            LOG("Error while getting config from server. Make sure you are connected to Internet!!!\nretrying...");
                            Util.SetTimeout(ApplyConfigData, 1000);
                        }

                        //
                        //                        if (!res.getBoolean("HasError")) {
                        //                            JSONObject result = res.getJSONObject("Result");
                        //
                        //                            ConfigData.init(result);
                        //                            readyService();
                        //
                        //
                        //                            final JSONObject reqData = new JSONObject();
                        //                            reqData.put("tableName", "config");
                        //                            JSONObject data = new JSONObject();
                        //                            data.put("id", "\"SERVER_CONFIG\"");
                        //                            data.put("value", "'" + ConfigData.serialize() + "'");
                        //                            reqData.put("data", data);
                        //
                        //                            database.insert(reqData, new RequestCallback() {
                        //                                @Override
                        //                                public void onResult(JSONObject result){
                        //                                    log.info("INSERT_CONFIG_TO_DB " + result);
                        //                                }
                        //                            });
                        //
                        //                        } else {
                        //
                        //                           getConfigFromDB(new RequestCallback() {
                        //                                @Override
                        //                                public void onResult(JSONObject result) {
                        //                                    try {
                        //                                        JSONObject config = result.getJSONObject("config");
                        //                                        if (config != null) {
                        //                                            ConfigData.initWithDBConfig(config);
                        //                                            readyService();
                        //                                        } else {
                        //                                            Util.setTimeout(new Util.SetTimeoutCallback() {
                        //                                                @Override
                        //                                                public void onDone() {
                        //                                                    applyConfigData();
                        //                                                }
                        //
                        //                                            }, ConfigData.smit);
                        //                                        }
                        //                                    } catch (Exception e) {
                        //                                        e.printStackTrace();
                        //                                    }
                        //
                        //                                }
                        //                            });
                        //
                        //
                        //
                        //                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                        //                        getConfigFromDB(new RequestCallback() {
                        //                            @Override
                        //                            public void onResult(JSONObject result) {
                        //                                try {
                        //                                    JSONObject config = result.getJSONObject("config");
                        //                                    if (config != null) {
                        //                                        ConfigData.init(config);
                        //                                        readyService();
                        //                                    } else {
                        //                                        Util.setTimeout(new Util.SetTimeoutCallback() {
                        //                                            @Override
                        //                                            public void onDone() {
                        //                                                applyConfigData();
                        //                                            }
                        //
                        //                                        }, ConfigData.smit);
                        //                                    }
                        //                                } catch (Exception ex) {
                        //                                    ex.printStackTrace();
                        //                                }
                        //                            }
                        //                        });
                    }
                });
            }
        }

        /**
         *<div style='width: 100%;text-align: right'> دریافت رخداد های موجود در سرویس</div>
         *
         * <pre>
         *     <code style='float:right'>نمونه کد</code>
         *     <code>
         *     //fire after service initialize
         *     string id = tis.on("ready", new EventCallback(){
         *        {@code @Override}
         *         public void onFire() {
         *             System.out.println("READY");
         *             functionTest(tis);
         *         }
         *     });
         *     System.out.println("UUID EVENT " + id);
         *     // fire after  login
         *     tis.on("login", new EventCallback(){
         *        {@code @Override}
         *         public void onFire() {
         *             Util.setTimeout(new Util.SetTimeoutCallback(){
         *                 {@code @Override}
         *                 public void onDone() {
         *                     afterLoginFunctionTest(tis);
         *                 }
         *             }, 4000);
         *         }
         *     });
         *     </code>
         * </pre>
         * @param eventName  نام رخداد
         * <ul>
         *      <li>"ready" - بعد از آماده شدن سرویس این رخداد اتفاق می افتد, و می توانید از توابع سرویس استفاده کنید</li>
         *      <li>"login" - بعد از ورود به حساب کاربری این رخداد اتفاق می افتد</li>
         *      <li>"guestLogin" - بهد از ثبت نام کاربر مهمان و تبدیل شدن به کاربر معمولی اتفاق می افتد</li>
         *      <li>"defaultLeagueSubscribe" - بعد از عضویت در لیگ پیشفرض بازی, این رخداد اتفاق می افتد</li>
         *      <li>"logout" - بعد از خروج به حساب کاربری این رخداد اتفاق می افتد</li>
         *      <li>"connect" -  بعد از وصل شدن به سرور ایسینک این رخداد اتفاق می افتد</li>
         *      <li>"disconnect" - بعد از قطع شدن به سرور ایسینک این رخداد اتفاق می افتد</li>
         *      <li>"reconnect" - بعد از وصل مجدد به سرور ایسینک این رخداد اتفاق می افتد</li>
         *      <li>
         *           "message" -  بعد از دریافت پیام جدید این رویداد اتفاق می افتد
         *            <ul>
         *                <li>{Integer} type نوع پیام
         *                    <pre>
         *                        1 = پیام مربوط به درخواست آنلاین
         *                        2 = پیام مربوط به درخواست افلاین
         *                        3 = پیام مربوط به نوتیفیکیشن
         *                    </pre>
         *                </li>
         *                <li>{JSONObject} content محتویات مربوط به پیام که متناسب با نوع پیام متفاوت می باشد
         *                    <ul>
         *                        <li> در درخواست آنلاین کلید های آن عبارتند از
         *                            <ul>
         *                                <li>{string} name نام درخواست دهنده مسابقه</li>
         *                                <li>{string} leagueName شناسه درخواست دهنده مسابقه</li>
         *                                <li>{string} gameName نام بازی که در آن درخواست داده شده است</li>
         *                                <li>{string} gameId شناسه بازی که در آن درخواست داده شده است</li>
         *                                <li>{string} leagueName نام لیگی که در آن درخواست داده شده است</li>
         *                                <li>{string} leagueId شناسه لیگی که در آن درخواست داده شده است</li>
         *                                <li>{string} requestId شناسه درخواست کنونی</li>
         *                                <li>{string} packageName نام پکیج بازی که در آن درخواست داده شده است</li>
         *                                <li>{string} version نسخه بازی در گیم سنتر</li>
         *                                <li>{JSONObject} [image] اطلاعات مربوط به تصویر درخواست کننده بازی
         *                                    <ul>
         *                                        <li>{string} id</li>
         *                                        <li>{string} url</li>
         *                                        <li>{Integer} width</li>
         *                                        <li>{Integer} height</li>
         *                                    </ul>
         *                                </li>
         *                            </ul>
         *                        </li>
         *                    </ul>
         *                </li>
         *            </ul>
         *      </li>
         *      <li>"profileChange" -  بعد از ثبت تغییر پروفایل کاربری اتفاق می افتد</li>
         *      <li>"newMatch" -  اعلان شروع مسابقه جدید از سمت گیم سنتر</li>
         *      <li>"matchReceiveData" -  دریافت پیام یک مسابقه از سمت حربف</li>
         *      <li>"matchStart" -  اعلام شروع بازی از سوی گیم سنتر</li>
         *      <li>"matchPause" - اعلام توفق بازی از سوی گیم سنتر</li>
         *      <li>"matchResume" - اعلام شروع مجدد مسابقه از سوی گیم سنتر</li>
         *      <li>"matchEnd" - بعد از اعلام نتیجه از سوی افراذ حاضر در مسابقه این رویداد اتفاق می افتد</li>
         *      <li>"matchLeave" - با ترک مسابقه از سوی یک نفر این رخداد اتفاق می افتد</li>
         * </ul>
         *
         * @param callback متد اجرا شونده بعد از اتفاق افتادن رخداد مورد نظر
         *
         * @return  شناسه متد رخداد
         * */
        public string On(string eventName, EventCallback callback)
        {
            //Debug.Log("service.on: " + eventName);
            var events = _eventCallback[eventName];
            string id = null;
            if (events != null)
            {
                id = Guid.NewGuid().ToString();

                events.Add(id, callback);

                if (eventName.Equals("ready") && this._isReady)
                {
                    var readyData = new JSONObject();
                    readyData.Add("config", GetConfig());
                    callback.OnFire(readyData);
                }

                if (eventName.Equals("login") && _userData.HasKeyNotNull("loginState") &&
                    _userData["loginState"].AsBool)
                {
                    callback.OnFire(_userData);
                }

                if (eventName.Equals("connect") && _userData.HasKey("peerId") && _userData["peerId"] != null)
                {
                    JSONObject readyData = new JSONObject();
                    readyData.Add("peerId", _userData["peerId"]);
                    callback.OnFire(readyData);
                }

                //try
                //{

                //}
                //catch (Exception e)
                //{
                //    Debug.LogError("Exception: " + e.Message);
                //}
            }

            //        Debug.Log("ON_EVENT " + id + " " + eventName + " " + this.isReady);
            return id;
        }

        private void ReadyService()
        {
//            LOG("readyService");
            try
            {
                _network.Init(false);

                var Params = new JSONObject();
                //            Params.put("appId", appId + UUID.randomUUID().ToString());
                Params.Add("appId", _appId);
                Params.Add("deviceId", _deviceId);
                Params.Add("dic", _dic);
                Params.Add("lang", _lang);
                //Params.Add("service", this);

                //            if (chatServiceEnable) {
                //                chatService = new ChatService(Params);
                //            }


                this._isReady = true;
                var readyData = new JSONObject();
                readyData.Add("config", GetConfig());
                FireEvents(ServiceEventReady, readyData);
                if (_loginData != null &&
                    _loginData.HasKey("token") && _loginData["token"] != null &&
                    _loginData.HasKey("id") && _loginData["id"] != null &&
                    _loginData.HasKey("name") && _loginData["name"] != null)
                {
                    var guest = false;

                    JSONObject image = null;
                    string imageUrl = null;
                    int? tokenIssuer = null;

                    if (_loginData.HasKey("image") && _loginData["image"] != null)
                    {
                        image = _loginData["image"].AsObject;
                    }

                    if (_loginData.HasKey("imageUrl") && _loginData["imageUrl"] != null)
                    {
                        imageUrl = _loginData["imageUrl"].ToString();
                    }

                    if (_loginData.HasKey("tokenIssuer") && _loginData["tokenIssuer"] != null)
                    {
                        tokenIssuer = _loginData["tokenIssuer"].AsInt;
                    }

                    if (_loginData.HasKey("guest") && _loginData["guest"] != null)
                    {
                        guest = _loginData["guest"].AsBool;
                    }

                    var uData = new JSONObject();
                    uData.Add("id", _loginData["id"]);
                    uData.Add("name", _loginData["name"]);

                    if (image != null)
                    {
                        uData.Add("image", image);
                    }

                    if (imageUrl != null)
                    {
                        uData.Add("imageUrl", imageUrl);
                    }

                    uData.Add("token", _loginData["token"]);
                    uData.Add("tokenIssuer", tokenIssuer);
                    uData.Add("guest", guest);

                    LoginAction(uData);
                    //                loginAction(loginData.get("id").ToString(),loginData.getString("name"),loginData.getString("token"),image,guest,tokenIssuer);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void LoginAction(JSONObject Params)
        {
//            LOG("User login...\n\nParams: " + PrettyJson(Params) + "\n");
            try
            {
                string userId = Params["id"];
                string token = Params["token"];
                var tokenIssuer = Params["tokenIssuer"].AsInt;
                var guest = Params["guest"].AsBool;

                //            JSONObject image = (Params.HasKey("image") && Params["image"] != null) ? Params["image"].AsObject : null;
                //            string imageUrl = (Params.HasKey("imageUrl") && Params["imageUrl"] != null)
                //                ? Params["imageUrl"].ToString()
                //                : null;

                var loginState = false;

                if (_userData.HasKey("loginState") && _userData["loginState"] != null)
                {
                    loginState = _userData["loginState"].AsBool;
                }


                if (loginState)
                {
                    if (!_userData["id"].ToString().Equals(userId))
                    {
                        LogoutAction();
                        LoginAct(Params);
                        //                    loginAct(userId, name, token,guest,image,tokenIssuer);
                    }
                    else
                    {
                        _userData.Add("token", token);
                        _userData.Add("tokenIssuer", tokenIssuer);
                        if (guest)
                        {
                            FireEvents("guestLogin", _userData);
                            FireEvents("profileChange", _userData);
                        }
                    }
                }
                else
                {
                    LoginAct(Params);
                    //                loginAct(userId, name, token,guest,image,tokenIssuer);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        private void LoginAct(JSONObject Params)
        {
//            Debug.Log("loginAct: \n\nParams: \n" + PrettyJson(Params));
            try
            {
                if (_userData == null)
                {
                    _userData = JSON.Parse(Params.ToString()).AsObject;
                }
                else
                {
                    _userData.Add("id", Params["id"]);
                    _userData.Add("name", Params["name"]);
                    _userData.Add("token", Params["token"]);
                    _userData.Add("tokenIssuer", Params["tokenIssuer"].AsInt);

                    _userData.Add("guest", Params.HasKeyNotNull("guest") && Params["guest"].AsBool);
                }

                _userData.Add("loginState", true);

                LoginActionWithPeer();
                CheckPeerAndSocketSync();

                foreach (var key in _games.Keys)
                {
                    SubscribeDefaultLeague(key);
                }

                FireEvents("login", _userData);

                CheckPeerAndSocketSync();
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        private JSONObject GetConfig()
        {
            //Debug.Log("Service.getConfig");
            var config = new JSONObject();
            try
            {
                config.Add("isa", ConfigData.Isa);
                config.Add("mrt", ConfigData.Mrt);
                config.Add("mmrc", ConfigData.Mmrc);
                config.Add("rvt", ConfigData.Rvt);
                config.Add("getChatHistoryCount", ConfigData.Gchc);
                config.Add("maxTextMessageLength", ConfigData.Mtml);
                config.Add("maxEmojiInMessage", ConfigData.Meim);
                config.Add("maxOfflineRequestMatchDay", ConfigData.Mormd);
                config.Add("offlineRequestMinMinute", ConfigData.Mormd);
                config.Add("offlineRequestMinMinute", ConfigData.Ormm);
                config.Add("quickMatchTimeout", ConfigData.Qmt);
                config.Add("increaseCashUrl", ConfigData.Icu);
                config.Add("creditUnit", ConfigData.Cu);
                config.Add("localEditProfile", ConfigData.Lep);
                config.Add("gameCenterLoginPageUrl", ConfigData.Gclpu);
                config.Add("gameCenterUserPageUrl",
                    "http://www." + ConfigData.GcDomainName +
                    (_userData.HasKey("token") ? "/?_token=" + _userData["token"] : "/") + ConfigData.Gcuph);
                config.Add("gameCenterRulesUrl", ConfigData.Gcru);
                config.Add("chatItemId", ConfigData.Ciid);
                config.Add("businessId", ConfigData.BusinessId);
                config.Add("bannerCustomPostId", ConfigData.Bcpid);
                config.Add("emojiCustomPosName", ConfigData.Ecpn);
                config.Add("gameCenterId", ConfigData.Gcid);
                config.Add("gameCenterVersion", ConfigData.Gcv);
                config.Add("gameCenterAPKUrl", ConfigData.Gcau);
                config.Add("supporterId", ConfigData.Gcsid);
                config.Add("ssoLoginUrl", ConfigData.Ssolu);
                config.Add("ssoSignupUrl", ConfigData.Ssosu);
                config.Add("ssoLogoutUrl", ConfigData.Ssolou);
                config.Add("ssoLeagueEnrollUrl", ConfigData.Ssoleu);
                config.Add("ssoInAppPurchaseUrl", ConfigData.Ssoiau);
                config.Add("ssoGameBuyUrl", ConfigData.Ssogbu);
                config.Add("gameCenterAddress", ConfigData.Gca);
                config.Add("lastAndroidAppVersion", ConfigData.Malv);
                config.Add("lastAndroidAppChangeLog", ConfigData.Malvcl);
                config.Add("androidAppMinimumVersion", ConfigData.Mamv);
                config.Add("lastAndroidAppDownloadLink", ConfigData.Malvdl);
                config.Add("lastAndroidAppForceUpdate", ConfigData.Malvfu);
                config.Add("gamesConfigData", ConfigData.Gcd);
                config.Add("leaguesConfigData", ConfigData.Lcd);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }

            return config;
        }

        private void InitDatabase(Action<JSONObject> onFire)
        {
            //Debug.Log("Database_Init_Started");
            onFire(new JSONObject());


            // todo: fully implement later
            //try
            //{
            //    JSONObject Params = new JSONObject();
            //    Params.Add("name", dbName);
            //    Params.Add("version", 1);
            //    //Params.Add("context", context);
            //    database = new Assets.Scripts.Database.Database(Params,
            //        // onCreate
            //        delegate (Assets.Scripts.Database.Database database)
            //        {
            //            //Debug.Log("Database_Init_Started_onCreate_1" + database);
            //            try
            //            {
            //                //database.execSQL("CREATE TABLE config(id TEXT PRIMARY KEY,value TEXT)");
            //            }
            //            catch (ServiceException e)
            //            {
            //                Debug.Log("Database_Init_Started_onCreate_2");
            //                Debug.Log("Exception: " + e.Message);
            //            }

            //            //Debug.Log("Database_Init_Started_onCreate_3");
            //        },

            //        // onUpgrade
            //        delegate (Assets.Scripts.Database.Database database)
            //        {
            //            //Debug.Log("Database_Init_Started_onUpgrade" + database);
            //        },

            //        // onOpen
            //        delegate (Assets.Scripts.Database.Database database)
            //        {
            //            //Debug.Log("Database_Init_Started_onOpen_1 " + database);
            //            // onFire(new JSONObject());
            //        });
            //}
            //catch (Exception e)
            //{
            //    throw new ServiceException(e);
            //}
        }

        private bool HasMajorConflict(string gameId, string version)
        {
            try
            {
                if (ConfigData.Cmc)
                {
                    Debug.Log("hasMajorConflict " + gameId + " " + _games[gameId].AsObject["version"] + " " + version);
                    return Util.HasMajorConflict(_games[gameId].AsObject["version"], version);
                }

                return false;
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                return false;
            }
        }

        private void LoginActionWithPeer()
        {
            try
            {
//                Debug.Log(string.Format("loginActionWithPeer_0 {0}  {1}  {2}",
// _isCheckLoginActionWithPeer,
// _userData.HasKeyNotNull("peerId"),
// (_userData.HasKeyNotNull("loginState") && _userData["loginState"].AsBool)));

                if (!_isCheckLoginActionWithPeer && _userData.HasKeyNotNull("peerId") &&
                    (_userData.HasKeyNotNull("loginState") && _userData["loginState"].AsBool))
                {
//                    Debug.Log("loginActionWithPeer_1 ");
                    _network.OnLogin(_userData);

                    _isCheckLoginActionWithPeer = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void CheckPeerAndSocketSync()
        {
//            Debug.Log(string.Format("checkPeerAndSocketSync_0 : " +
//                                    "\n_peerAndTokenSync: {0}" +
//                                    "\n_syncTokenWithPeer: {1}" +
//                                    "\n_syncPeerWithToken: {2}",
//                _peerAndTokenSync, 
//                _syncTokenWithPeer, 
//                _syncPeerWithToken));
            try
            {
                if (!_peerAndTokenSync && !_syncTokenWithPeer && !_syncPeerWithToken)
                {
//                    Debug.Log("checkPeerAndSocketSync_1 " + _userData.HasKeyNotNull("token") + " " + _userData.HasKeyNotNull("peerId"));
                    if (_userData.HasKeyNotNull("token") && _userData.HasKeyNotNull("peerId"))
                    {
                        JSONObject requestData = new JSONObject();
                        requestData.Add("peerId", _userData["peerId"]);
                        Debug.Log("checkPeerAndSocketSync_2 ");
                        _peerAndTokenSync = true;
                        Request(RequestUrls.Ping, requestData,
                            result => { Debug.Log("checkPeerAndSocketSync_3 " + result); });
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void _getConfigFromServer(Action<JSONObject> onResult)
        {
            //Debug.Log("_getConfigFromServer");
            var requestData = new JSONObject();

            try
            {
                requestData.Add("url", ConfigData.ConfigUrl);
                requestData.Add("data", new JSONObject());

                var headers = new JSONObject();
                headers.Add("Content-Type", "application/json; charset=utf-8");
                requestData.Add("headers", headers);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
            
            Request(RequestUrls.GetConfig, new JSONObject(), res =>
            {
//                Debug.Log("network.postRequest.result");
                var returnData = res;
                try
                {
                    if (!res["HasError"].AsBool)
                    {
                        returnData = res["Result"].AsObject;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                }

                onResult(returnData);
            });
            

//            _network.PostRequest(requestData, res =>
//            {
//                //Debug.Log("network.postRequest.result");
//                var returnData = res;
//                try
//                {
//                    if (!res["HasError"].AsBool)
//                    {
//                        returnData = res["Result"].AsObject;
//                    }
//                }
//                catch (Exception e)
//                {
//                    Debug.LogError("Exception: " + e.Message);
//                }
//
//                onResult(returnData);
//            });
        }

        private JSONObject ExceptionErrorData(Exception e)
        {
            return Util.ExceptionErrorData(e);
        }

        private JSONObject ReformatTableObject(JSONObject data)
        {
            var returnData = new JSONObject();

            try
            {
                var leagueType = (data.HasKey("ColumnNames")) ? 0 : 1;
                returnData.Add("type", leagueType);
                switch (leagueType)
                {
                    case 0:

                        returnData.Add("headerData", data["ColumnNames"].AsArray);

                        var tableRawData = data["Table"].AsArray;
                        var usersData = new JSONArray();
                        for (var i = 0; i < tableRawData.Count; i++)
                        {
                            var uData = tableRawData[i].AsObject;
                            var userData = new JSONObject();
                            var scores = new JSONArray();
                            userData.Add("matchCount", uData["played"]);
                            userData.Add("userId", uData["playerID"].ToString());
                            userData.Add("nickName", uData["playerName"]);
                            userData.Add("rank", uData["rank"]);

                            if (uData.HasKey("profilePreviewImage") && uData["profilePreviewImage"] != null)
                            {
                                userData.Add("image", uData["profilePreviewImage"]);
                            }

                            if (uData.HasKey("profileImage") && uData["profileImage"] != null)
                            {
                                userData.Add("imageUrl", uData["profileImage"]);
                            }

                            var scoreRawData = uData["scores"].AsArray;
                            for (var j = 0; j < scoreRawData.Count; j++)
                            {
                                var score = scoreRawData[j].AsObject;
                                var scoreData = new JSONObject();
                                scoreData.Add("name", score["Name"]);
                                scoreData.Add("value", score["Value"]);

                                scores.Add(scoreData);
                            }

                            userData.Add("scores", scores);

                            usersData.Add(userData);
                        }

                        returnData.Add("usersData", usersData);

                        break;

                    case 1:
                        var currentLevelMatchCount = data["1"].AsObject.Count * 2;
                        var roundData = new JSONObject();
                        var indexes = new JSONObject();
                        var levels = new JSONArray();

                        foreach (var key in data.Keys)
                        {
                            levels.Add(key);
                        }

                        for (var i = 0; i < levels.Count; i++)
                        {
                            var levelId = levels[i].AsInt;
                            currentLevelMatchCount /= 2;
                            var levelIdStr = levelId.ToString();

                            if (!roundData.HasKey(levelIdStr))
                            {
                                roundData.Add(levelIdStr, new JSONObject());
                            }

                            var levelData = data[levelIdStr].AsObject;
                            var index = 0;
                            if (!indexes.HasKey(levelIdStr))
                            {
                                indexes.Add(levelIdStr, new JSONObject());
                            }


                            for (var j = 1; j <= currentLevelMatchCount; j++)
                            {
                                var matchDataObj = new JSONObject();

                                var user1Data = new JSONObject();
                                user1Data.Add("id", null);
                                user1Data.Add("name", null);
                                user1Data.Add("isWinner", null);

                                var user2Data = new JSONObject();
                                user2Data.Add("id", null);
                                user2Data.Add("name", null);
                                user2Data.Add("isWinner", null);

                                matchDataObj.Add("user1", user1Data);
                                matchDataObj.Add("user2", user2Data);
                                var prevLevelId = levelId - 1;
                                string validMatchNodeId = null;

                                if (levelId == 1)
                                {
                                    validMatchNodeId = j.ToString();
                                }
                                else
                                {
                                    var upMatchIndex = ((j - 1) * 2) + 1;
                                    var upMatchData = roundData[prevLevelId.ToString()]
                                        .AsObject[upMatchIndex.ToString()].AsObject;
                                    var downMatchData = roundData[prevLevelId.ToString()]
                                        .AsObject[(upMatchIndex + 1).ToString()].AsObject;

                                    var nodeNames = new JSONArray();

                                    foreach (var key in levelData.Keys)
                                    {
                                        nodeNames.Add(key);
                                    }

                                    for (var k = 0; k < nodeNames.Count; k++)
                                    {
                                        string matchNodeId = nodeNames[k];

                                        var u1Id = levelData[matchNodeId].AsObject["UserId1"].ToString();
                                        var u2Id = levelData[matchNodeId].AsObject["UserId2"].ToString();

                                        if (
                                            u1Id.Equals(upMatchData["user1"].AsObject["id"].ToString()) ||
                                            u1Id.Equals(upMatchData["user2"].AsObject["id"].ToString()) ||
                                            u2Id.Equals(upMatchData["user1"].AsObject["id"].ToString()) ||
                                            u2Id.Equals(upMatchData["user2"].AsObject["id"].ToString()) ||
                                            u1Id.Equals(downMatchData["user1"].AsObject["id"].ToString()) ||
                                            u1Id.Equals(downMatchData["user2"].AsObject["id"].ToString()) ||
                                            u2Id.Equals(downMatchData["user1"].AsObject["id"].ToString()) ||
                                            u2Id.Equals(downMatchData["user2"].AsObject["id"].ToString())
                                        )
                                        {
                                            validMatchNodeId = matchNodeId;
                                            break;
                                        }
                                    }
                                }

                                if (validMatchNodeId != null)
                                {
                                    var matchData = levelData[validMatchNodeId].AsObject;

                                    if (matchData.HasKey("UserId1") && matchData["UserId1"] != null)
                                    {
                                        var userId = matchData["UserId1"].ToString();
                                        indexes[levelIdStr].AsObject.Add(userId, ++index);
                                    }

                                    if (matchData.HasKey("UserId2") && matchData["UserId2"] != null)
                                    {
                                        var userId = matchData["UserId2"].ToString();
                                        indexes[levelIdStr].AsObject.Add(userId, ++index);
                                    }

                                    var user1DataObj = matchDataObj["user1"].AsObject;
                                    user1DataObj.Add("id", matchData["UserId1"].ToString());
                                    user1DataObj.Add("name", matchData["UserId1Name"]);
                                    if (matchData.HasKey("Winner") && matchData["Winner"] != null)
                                    {
                                        user1DataObj.Add("isWinner",
                                            matchData["Winner"].ToString().Equals(matchData["UserId1"].ToString()));
                                    }

                                    if (matchData.HasKey("User1Image") && matchData["User1Image"] != null)
                                    {
                                        var imageData = matchData["User1Image"].AsObject;
                                        imageData.Add("id", imageData["id"].ToString());
                                        user1DataObj.Add("image", imageData);
                                    }

                                    if (matchData.HasKey("User1ImageUrl") && matchData["User1ImageUrl"] != null)
                                    {
                                        user1DataObj.Add("imageUrl", matchData["User1ImageUrl"]);
                                    }


                                    var user2DataObj = matchDataObj["user2"].AsObject;
                                    user2DataObj.Add("id", matchData["UserId2"].ToString());
                                    user2DataObj.Add("name", matchData["UserId2Name"]);
                                    if (matchData.HasKey("Winner") && matchData["Winner"] != null)
                                    {
                                        user2DataObj.Add("isWinner",
                                            matchData["Winner"].ToString().Equals(matchData["UserId2"].ToString()));
                                    }

                                    if (matchData.HasKey("User2Image") && matchData["User2Image"] != null)
                                    {
                                        var imageData = matchData["User2Image"].AsObject;
                                        imageData.Add("id", imageData["id"].ToString());
                                        user2DataObj.Add("image", imageData);
                                    }

                                    if (matchData.HasKey("User2ImageUrl") && matchData["User2ImageUrl"] != null)
                                    {
                                        user2DataObj.Add("imageUrl", matchData["User2ImageUrl"]);
                                    }


                                    if (levelId != 1 && indexes.HasKey("prevLevelId"))
                                    {
                                        matchDataObj["user1"].AsObject.Add("prevIndex",
                                            indexes["prevLevelId"].AsObject
                                                [matchData["UserId1"].ToString()]);
                                        matchDataObj["user2"].AsObject.Add("prevIndex",
                                            indexes["prevLevelId"].AsObject
                                                [matchData["UserId2"].ToString()]);
                                    }
                                }

                                roundData[levelIdStr].AsObject.Add(j.ToString(), matchDataObj);
                            }

                            returnData.Add("rounds", roundData);
                        }

                        break;
                }

                return returnData;
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            return returnData;
        }

        private JSONObject ReformatInAppPack(JSONObject pack)
        {
            var returnData = new JSONObject();

            try
            {
                var Base = pack["Base"].AsObject;
                var plan = pack["Plan"].AsObject;
                var price = Base["price"].AsDouble / ConfigData.Cf;


                returnData.Add("id", pack["ID"].ToString());
                returnData.Add("count", pack["Count"]);
                returnData.Add("name", Base["Name"]);
                returnData.Add("visible", pack["Visible"]);
                returnData.Add("enable", pack["Enable"]);
                returnData.Add("allowedTimesToBuy", pack["AllowedTimesToBuy"]);
                returnData.Add("description", Base["description"]);

                returnData.Add("item", ReformatGameItem(pack["Item"].AsObject));

                var planData = new JSONObject();
                planData.Add("cycle", plan["Cycle"]);
                planData.Add("id", plan["ID"].ToString());
                planData.Add("type", plan["Type"]);
                planData.Add("fromDate", plan.HasKeyNotNull("FromDate") ? plan["FromDate"] : null);
                planData.Add("toDate", plan.HasKeyNotNull("ToDate") ? plan["ToDate"] : null);
                returnData.Add("plan", planData);


                returnData.Add("price", price);
                returnData.Add("priceText", price + " " + ConfigData.Cu);
                returnData.Add("priceUnit", ConfigData.Cu);


                if (Base.HasKeyNotNull("previewInfo"))
                {
                    var image = Base["previewInfo"].AsObject;
                    image.Add("id", image["id"].ToString());
                    returnData.Add("image", image);
                }

                if (Base.HasKeyNotNull("preview"))
                {
                    returnData.Add("imageUrl", Base["preview"]);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            return returnData;
        }

        private void LogoutAction()
        {
            try
            {
                _userData.Remove("id");
                _userData.Remove("name");
                _userData.Remove("token");
                _userData.Remove("tokenIssuer");
                _userData.Remove("image");
                _userData.Remove("imageUrl");
                _userData.Add("loginState", false);

                _network.OnLogout();

                _currentMatchRequestCount = 0;
                Debug.Log("currentMatchRequestCount: " + _currentMatchRequestCount);
                FireEvents("logout", new JSONObject());
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        private void FireEvents(string categoryName, JSONObject msg)
        {
            //Debug.Log("fireEvents: " + categoryName);
            var events = _eventCallback[categoryName];

            if (events != null)
            {
                foreach (var item in events.Values)
                {
                    item.OnFire(msg);
                }
            }
        }

        private void FireEvents(string categoryName, JSONObject msg, AsyncResponse res)
        {
            var events = _eventCallback[categoryName];

            if (events != null)
            {
                foreach (var item in events.Values)
                {
                    item.OnFire(msg, res);
                }
            }
        }

        public static string PrettyJson(JSONObject uglyJson)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(uglyJson.ToString()), Formatting.Indented);
        }

        private void _sendResult(JSONObject Params, Action<JSONObject> onResult)
        {
            try
            {
                string result = Params["result"];
                var forceAddToTable = Params.HasKeyNotNull("forceAddToTable") && Params["forceAddToTable"].AsBool;


                var sendData = new JSONObject();
                sendData.Add("matchResult", result);
                if (Params.HasKeyNotNull("matchId"))
                {
                    sendData.Add("matchId", Params["matchId"]);
                }

                if (Params.HasKeyNotNull("gameId"))
                {
                    sendData.Add("gameId", Params["gameId"]);
                }


                if (_userData["loginState"].AsBool)
                {
                    string userId = Params.HasKeyNotNull("userId") ? Params["userId"] : _userData["id"];
                    sendData.Add("userId", userId);

                    Request(RequestUrls.MatchResult, sendData, reqResult =>
                    {
                        try
                        {
                            var hasError = reqResult["HasError"].AsBool;
                            var returnData = new JSONObject();
                            returnData.Add("hasError", hasError);
                            returnData.Add("errorMessage", reqResult["ErrorMessage"]);
                            if (hasError)
                            {
                                var errorCode = reqResult["ErrorCode"].AsInt;
                                returnData.Add("errorCode", errorCode);

                                if (errorCode == ErrorCodes.Runtime || errorCode == ErrorCodes.RequestFailed)
                                {
                                    if (forceAddToTable)
                                    {
                                        // TODO : database
                                    }
                                }
                                else
                                {
                                    if (!forceAddToTable)
                                    {
                                        // TODO : database
                                    }
                                }
                            }
                            else
                            {
                                if (!forceAddToTable)
                                {
                                    // TODO : database
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
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        private void MajorConflictAction()
        {
        }

        // interface function
        public void RegisterPeerId(long peerId, Action<JSONObject> onResult)
        {
            //Debug.Log("Service.registerPeerId");
            try
            {
                var sendData = new JSONObject();
                sendData.Add("peerId", peerId.ToString());
                sendData.Add("serviceVersion", Version);
                sendData.Add("deviceId", _deviceId);
                sendData.Add("type", _type);

                var gamesId = new JSONArray();

                foreach (var key in _games.Keys)
                {
                    gamesId.Add(key);
                }

                if (gamesId.Count > 0)
                {
                    var gameVersion = new JSONArray();
                    for (var i = 0; i < gamesId.Count; i++)
                    {
                        string gameId = gamesId[i];
                        var gameData = new JSONObject();
                        gameData.Add("gameId", gameId);
                        gameData.Add("version", _games[gameId].AsObject["version"]);
                        gameVersion.Add(gameData);
                    }
                    
                    sendData.Add("gameVersion", gameVersion.ToString());
                    //Debug.Log("registerPeerId_1 " + gameVersion);
                }

                if (_userData.HasKeyNotNull("token"))
                {
                    _syncTokenWithPeer = true;
                    Console.WriteLine(_syncTokenWithPeer);
                }
                //Debug.Log("registerPeerId_2 " + sendData);

                var setting = new JSONObject();
                setting.Add("fromSocket", false);

                Request(RequestUrls.AsyncRegister, sendData, result =>
                {
                    //Debug.Log("registerPeerId_3 " + PrettyJson( result));
                    try
                    {
                        var hasError = result["HasError"].AsBool;
                        var errorCode = result["ErrorCode"].AsInt;


                        var returnData = new JSONObject();
                        returnData.Add("hasError", hasError);
                        returnData.Add("errorCode", errorCode);
                        returnData.Add("errorMessage", result["ErrorMessage"].ToString());

                        if (hasError)
                        {
                            switch (errorCode)
                            {
                                case ErrorCodes.MajorConflict:
                                    MajorConflictAction();
                                    break;

                                case ErrorCodes.PeerClear:
                                    _network.OnLogout();
                                    break;

                                default:
                                    onResult(returnData);
                                    break;
                            }
                        }
                        else
                        {
                            returnData.Add("result", result["Result"]);
                            onResult(returnData);
                        }
                    }
                    catch (Exception e)
                    {
                        onResult(ExceptionErrorData(e));
                    }
                }, setting);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        // interface function
        public void ActivatePeerId(long peerId, Action<JSONObject> onResult)
        {
            var requestData = new JSONObject();
            try
            {
                requestData.Add("peerId", peerId);
                var setting = new JSONObject();
                setting.Add("fromSocket", false);

                Request(RequestUrls.ActivePeer, requestData, result =>
                {
                    try
                    {
                        JSONObject data;
                        var hasError = result["HasError"].AsBool;
                        if (hasError)
                        {
                            switch (result["ErrorCode"].AsInt)
                            {
                                case ErrorCodes.PeerClear:
                                    _network.OnLogout();
                                    break;
                                default:
                                    data = new JSONObject();
                                    data.Add("hasError", result["HasError"].AsBool);
                                    data.Add("errorMessage", result["ErrorMessage"].ToString());
                                    data.Add("errorCode", result["ErrorCode"].AsInt);
                                    onResult(data);
                                    break;
                            }
                        }
                        else
                        {
                            data = new JSONObject();
                            data.Add("hasError", false);
                            onResult(data);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception: " + e.Message);
                    }
                }, setting);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }
        }

        /// <summary>
        /// نمایش پنجره ورود به حساب کاربری
        /// </summary>
        /// <param name="reciever">نام ابجت دریافت کننده پاسخ سرور</param>
        /// <param name="callback">نام متد دریافت کننده پاسخ سرور
        ///     <ul>
        ///         <li>{Boolean} hasError</li>
        ///         <li>{String}  errorMessage</li>
        ///         <li>{Integer} errorCode</li>
        ///         <li> {JSONObject} result
        ///             <ul>
        ///                 <li>{String} id</li>
        ///                 <li>{String} name</li>
        ///                 <li>{String} token</li>
        ///                 <li>{String} imageUrl</li>
        ///             </ul>
        ///         </li>
        ///     </ul>
        ///</param>
        public void loginUI(GameObject reciever = null, Action<JSONObject> callback = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("loginUI", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "");
        }

        /// <summary>
        /// نمایش پنجره ثبت نام در حساب کاربری
        /// </summary>
        /// <param name="reciever">نام ابجت دریافت کننده پاسخ سرور</param>
        /// <param name="callback">نام متد دریافت کننده پاسخ سرور
        ///     <ul>
        ///         <li>{Boolean} hasError</li>
        ///         <li>{String}  errorMessage</li>
        ///         <li>{Integer} errorCode</li>
        ///         <li> {JSONObject} result
        ///             <ul>
        ///                 <li>{String} id</li>
        ///                 <li>{String} name</li>
        ///                 <li>{String} token</li>
        ///                 <li>{String} imageUrl</li>
        ///             </ul>
        ///         </li>
        ///     </ul>
        ///</param>
        public void signupUI(GameObject reciever = null, Action<JSONObject> callback = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("signupUI", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "");
        }

        /// <summary>
        /// نمایش پنجره جدول رده بندی
        /// </summary>
        /// <param name="gameId">شناسه بازی</param>
        /// <param name="leagueId">شناسه لیگ</param>
        public void leaderBoardUI(String gameId = null, String leagueId = null)
        {
            LOG("Get leader board ...");
            if (gameId == null)
            {
                if (_games.Count <= 0)
                {
                    LOG("Please Insert a GameId in: TisService (GameObject) -> Sdk Setup (Script) -> Games Info\n");
                    return;
                }
                else
                {
                    foreach (var id in _games.Keys)
                    {
                        gameId = id;
                        break;
                    }
                }
            }
            
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("leaderBoardUI", gameId, leagueId);
        }

        /// <summary>
        /// نمایش پنجره بازی
        /// </summary>
        /// <param name="gameId">شناسه بازی</param>
        public void gameUI(String gameId = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("gameUI", gameId);
        }

        /// <summary>
        /// نمایش پنجره لیگ و یا لیگ های یک بازی
        /// </summary>
        /// <param name="gameId">شناسه بازی</param>
        /// <param name="leagueId">شناسه لیگ</param>
        public void leagueUI(String gameId = null, String leagueId = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("leagueUI", gameId, leagueId);
        }


        /// <summary>
        /// نمایش پنجره درخواست مسابقه
        /// </summary>
        /// <param name="gameId">شناسه بازی</param>
        /// <param name="leagueId">شناسه لیگ</param>
        public void matchRequestUI(String gameId = null, String leagueId = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("matchRequestUI", gameId, leagueId);
        }


        /// <summary>
        /// نمایش پنجره ویرایش پروفایل
        /// </summary>
        /// <param name="reciever">نام ابجت دریافت کننده پاسخ سرور</param>
        /// <param name="callback">نام متد دریافت کننده پاسخ سرور</param>
        ///
        public void editProfileUI(GameObject reciever = null, Action<JSONObject> callback = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("editProfileUI", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "");
        }


        /// <summary>
        /// نمایش پنجره ویرایش پروفایل
        /// </summary>
        /// <param name="reciever">نام ابجت دریافت کننده پاسخ سرور</param>
        /// <param name="callback">نام متد دریافت کننده پاسخ سرور</param>
        /// <param name="gameId">شناسه بازی</param>
        /// <param name="itemId">شناسه آیتم</param>
        ///
        public void inAppPurchaseUI(String gameId = null, String itemId = null, GameObject reciever = null,
            Action<JSONObject> callback = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("inAppPurchaseUI", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "", gameId, itemId);
        }

        /// <summary>
        /// نمایش پنجره افزایش اعتبار
        /// </summary>
        /// <param name="reciever">نام ابجت دریافت کننده پاسخ سرور</param>
        /// <param name="callback">نام متد دریافت کننده پاسخ سرور</param>
        ///
        public void increaseCreditUI(GameObject reciever = null, Action<JSONObject> callback = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("increaseCreditUI", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "");
        }

        /// <summary>
        /// نمایش پنجره عضویت در لیگ
        /// </summary>
        /// <param name="reciever">نام ابجت دریافت کننده پاسخ سرور</param>
        /// <param name="callback">نام متد دریافت کننده پاسخ سرور</param>
        /// <param name="leagueId">شناسه لیگ</param>
        ///
        public void subscribeLeagueUI(String leagueId = null, GameObject reciever = null,
            Action<JSONObject> callback = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("subscribeLeagueUI", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "", leagueId);
        }

        /// <summary>
        /// نمایش پنجره خرید یک پک
        /// </summary>
        /// <param name="reciever">نام ابجت دریافت کننده پاسخ سرور</param>
        /// <param name="callback">نام متد دریافت کننده پاسخ سرور</param>
        /// <param name="packId">شناسه پک </param>
        ///
        public void buyPackUI(String packId = null, GameObject reciever = null, Action<JSONObject> callback = null)
        {
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("buyPackUI", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "", packId);
        }


        /// <summary>
        /// دریافت اطلاعات کاربر لاگین شده
        /// </summary>
        /// <param name="reciever">نام ابجت دریافت کننده پاسخ سرور</param>
        /// <param name="callback">نام متد دریافت کننده پاسخ سرور
        ///     <ul>
        ///         <li>{Boolean} hasError</li>
        ///         <li>{String}  errorMessage</li>
        ///         <li>{Integer} errorCode</li>
        ///         <li> {JSONObject} result
        ///             <ul>
        ///                 <li>{String} id</li>
        ///                 <li>{String} name</li>
        ///                 <li>{String} token</li>
        ///                 <li>{String} imageUrl</li>
        ///             </ul>
        ///         </li>
        ///     </ul>
        ///</param>
        public void getUserData(GameObject reciever = null, Action<JSONObject> callback = null)
        {
            LOG("Getting user data...\n");
            if (_javaPluginObject != null && _isReady)
                _javaPluginObject.Call("getUserData", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "");
        }
        
        private void getUserDataInit(GameObject reciever = null, Action<JSONObject> callback = null)
        {
//            LOG("Getting user data...\nreciever.name: " + reciever.name + "\ncallback.Method.Name: " + callback.Method.Name);
            if (_javaPluginObject != null)
                _javaPluginObject.Call("getUserData", reciever != null ? reciever.name : "",
                    callback != null ? callback.Method.Name : "");
        }

        /// <summary>
        /// تابعی برای لاگ کردن اطلاعات مورد نیاز
        /// </summary>
        /// <param name="message"></param>
        public void LOG(string message)
        {
            Debug.Log(message);
            if (_logger != null)
            {
                _logger.Log(message);
            }
        }
    }
}
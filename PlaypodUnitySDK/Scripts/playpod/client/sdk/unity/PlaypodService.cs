using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using playpod.client.sdk.unity.Base;
using playpod.client.sdk.unity.Network;
using playpod.client.sdk.unity.Share;
using SimpleJSON;
using UnityEditor;
using UnityEngine;

namespace playpod.client.sdk.unity
{
    public class PlaypodService : MonoBehaviour
    {
        private static Service _service;

        public static string GameObjectName = "PlayPodService";

        private StateMachine _stateMachine;

        [Header("PlayPodService Config")] public bool UseTestServer;
        public string ServiceToken = "";

        [Space(10)] public string gameVersion;

        public GameInfo[] gamesInfo;

        [Header("In-Game Logger")] public GameObject loggerPanel;
        public static Logger _logger;

        private Action<JSONObject> onGotUserData;

        #region serviceParams

        public struct DefaultParams
        {
            public int size;
            public int offset;
            public string filter;
        }

        private DefaultParams defaultParams;

        [Serializable]
        public class GameInfo
        {
            public string gameId;
        }

        public struct LoginInfoParams
        {
            public string testId;
            public string testToken;
            public string id;
            public string name;
            public string imageUrl;
            public string token;
            public int tokenIssuer;
            public bool guest;
        }

        public LoginInfoParams loginParams;

        public struct GetOnlineUserParams
        {
            public string gameId;
            public string leagueId;
        }

        public struct GetLeagueInfoParams
        {
            public string gameId;
            public string leagueId;
            public string[] leaguesId;
            public string name;
            public int prize;
            public int status;
            public int[] statusList;
            public int financialType;
            public int userState;
            public bool showDefault;
            public string lobbyId;
        }

        public struct GetRelatedLeaguesInfoParams
        {
            public string leagueId;
            public int type;
        }

        public struct GetTopLeaguesInfoParams
        {
            public string gameId;
            public int type;
        }

        // getLatestLeaguesInfo only needs default params

        public struct GetLeagueMembersParams
        {
            public string leagueId;
            public int userState;
            public string name;
        }

        public struct GetGamesInfoParams
        {
            public string[] gamesId;
            public string lobbyId;
            public string name;
        }

        public struct GetTopGamesInfoParams
        {
            public int type;
        }

        // getLatestLeaguesInfo only needs default params

        public struct GetRelatedGamesInfoParams
        {
            public string gameId;
            public int type;
        }

        public struct GetTableDataParams
        {
            public string leagueId;
            public int rangeType;
        }

        public struct SearchUserRequestParams
        {
            public string name;
        }

        public struct GetLeagueAwardsParams
        {
            public string leagueId;
        }

        public struct GetUserProfileParams
        {
            public string userId;
            public bool refetch;
        }

        public struct GetInAppPurchasePackParams
        {
            public string gameId;
            public string packId;
            public string itemId;
            public string nameFilter;
        }

        public struct GetGlobalInAppPurchasePackParams
        {
            public string itemId;
        }

        public struct GetGameItemsParams
        {
            public string gameId;
            public string itemId;
        }

        public struct GetUserGameCenterItemParams
        {
            public string itemId;
        }

        public struct GetUserItemsParams
        {
            public string gameId;
            public string itemId;
        }

        public struct ConsumeItemRequestParams
        {
            public string itemId;
            public int count;
        }

        // getLobby requires no params (not even default ones)

        public struct GetLeagueTopPlayersParams
        {
            public string leagueId;
        }

        public struct GetTopPlayersParams
        {
            public string gameId;
        }

        public struct GetOnlineInfoParams
        {
            public string gameId;
        }

        public struct GetEnrolledLeaguesParams
        {
            public string userId;
        }

        public struct GetEnrollAccessParams
        {
            public string leagueId;
        }

        public struct SendLeagueRateRequestParams
        {
            public string leagueId;
            public int rate;
        }

        public struct SendGameRateRequestParams
        {
            public string gameId;
            public int rate;
        }

        public struct GetUserAchievementsParams
        {
            public string gameId;
            public string userId;
            public int type;
        }

        public struct GetUserAchievementDetailParams
        {
            public string gameId;
            public int rankType;
        }

        public struct GetUserGamePointsParams
        {
            public string gameId;
        }

        public struct GetLeagueMatchesResultParams
        {
            public string leagueId;
            public string userId;
            public string matchId;
        }

        public struct GetLeagueLatestMatchesResultParams
        {
            public string leagueId;
            public string username;
        }

        public struct GetLeagueLatestMatchesParams
        {
            public string leagueId;
            public string name;
        }

        public struct GetLeagueMatchesParams
        {
            public string leagueId;
            public int status;
        }

        public struct GetCommentListParams
        {
            public string id;
        }

        public struct AddGameCommentRequestParams
        {
            public string id;
            public string text;
        }

        public struct AddLeagueCommentRequestParams
        {
            public string id;
            public string text;
        }

        public struct SubscribeDefaultLeagueRequestParams
        {
            public string gameId;
        }

        public struct GetGalleryParams
        {
            public string gameId;
            public string businessId;
        }

        public struct GetLobbiesGamesParams
        {
            public string[] lobbyIds;
        }

        public struct GetLobbiesLeaguesParams
        {
            public string[] lobbyIds;
        }

        // getLives has no parameters

        public struct MatchRequestParams
        {
            public string opponentId;
            public string gameId;
            public string leagueId;
        }
        
        public struct CancelMatchRequestParams
        {
            public string requestId;
        }
        
        public struct MatchIdRequestParams
        {
            public string gameId;
            public string leagueId;
        }
        
        public struct QuickMatchRequestParams
        {
            public string gameId;
            public string leagueId;
        }
        
        public struct StreamMatchIdRequestParams
        {
            public string gameId;
            public string leagueId;
            public int clientType;
        }
        
        public struct MatchRequestResponseParams
        {
            public string requestId;
            public int rejectReasonType;
            public string rejectMessage;
        }
        
        public struct CancelQuickMatchRequestParams
        {
            public string leagueId;
        }
        
        public struct SubscribeLeagueRequestParams
        {
            public string leagueId;
            public string enrollUrl;
            public string voucherHash;
        }

        #endregion

        void Awake()
        {
            if (name != GameObjectName)
            {
                name = GameObjectName;
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Util.Init(this);
            InitLogger();
            _stateMachine = new StateMachine();
            SetDefaultParams(5, 0, "");
        }

        private void InitLogger()
        {
            if (loggerPanel == null)
            {
                _logger = new Logger();
                return;
            }

            if (!loggerPanel.activeSelf)
            {
                loggerPanel.SetActive(true);
            }

            _logger = loggerPanel.GetComponent<Logger>();
            _logger.isInit = true;

            LOG("Logger OK ...");
        }

        public void GetServiceInstance(Action<JSONObject> onResult)
        {
            if (_service != null)
            {
                JSONObject serviceInitResult = new JSONObject();
                serviceInitResult.Add("hasError", false);
                serviceInitResult.Add("Desc", "Service is Initialized");
                onResult(serviceInitResult);
                return;
            }

            LOG("Initializing service, please wait ...");

            if (UseTestServer && ServiceToken == String.Empty)
            {
                JSONObject serviceInitResult = new JSONObject();
                serviceInitResult.Add("hasError", true);
                serviceInitResult.Add("errorMessage",
                    "Cannot init in TEST MODE without SERVICE TOKEN. Either disable USE TEST SERVER option, or provide a SERVICE TOKEN.");
                serviceInitResult.Add("errorCode", 1001);
                onResult(serviceInitResult);
                return;
            }

            if (!UseTestServer && Application.isEditor)
            {
                JSONObject serviceInitResult = new JSONObject();
                serviceInitResult.Add("hasError", true);
                serviceInitResult.Add("errorMessage",
                    "Cannot init NON-TEST MODE in EDITOR. Please build the game and run it on a device.");
                serviceInitResult.Add("errorCode", 1002);
                onResult(serviceInitResult);
                return;
            }

            if (gameVersion == String.Empty)
            {
                JSONObject serviceInitResult = new JSONObject();
                serviceInitResult.Add("hasError", true);
                serviceInitResult.Add("errorMessage",
                    "GAME VERSION cannot be empty");
                serviceInitResult.Add("errorCode", 1004);
                onResult(serviceInitResult);
                return;
            }

            var serviceInitParams = new JSONObject();

            if (ServiceToken != String.Empty)
            {
                loginParams = new LoginInfoParams();
                loginParams.token = ServiceToken;

                var loginData = new JSONObject();
                loginData.Add("token", ServiceToken);
                serviceInitParams.Add("loginData", loginData);
            }

            if (UseTestServer)
            {
                serviceInitParams.Add("serviceMode", ServiceModeTypes.DevelopmodeOnline);
            }

            if (gamesInfo.Length < 1)
            {
                JSONObject serviceInitResult = new JSONObject();
                serviceInitResult.Add("hasError", true);
                serviceInitResult.Add("errorMessage",
                    "Please enter the GAME INFO for at least 1 game");
                serviceInitResult.Add("errorCode", 1003);
                onResult(serviceInitResult);
                return;
            }

            if (gamesInfo.Length >= 1)
            {
                foreach (var gameInfo in gamesInfo)
                {
                    if (gameInfo.gameId == string.Empty)
                    {
                        JSONObject serviceInitResult = new JSONObject();
                        serviceInitResult.Add("hasError", true);
                        serviceInitResult.Add("errorMessage",
                            "Game Id in GAMES INFO cannot be empty");
                        serviceInitResult.Add("errorCode", 1005);
                        onResult(serviceInitResult);
                        return;
                    }
                }
            }

            var games = new JSONObject();

            foreach (var gameInfo in gamesInfo)
            {
                var game = new JSONObject();
                game.Add("version", gameVersion);
                games.Add(gameInfo.gameId, game);
            }

            serviceInitParams.Add("games", games);

            Service init = new Service();
            init._logger = _logger;
            _service = init.GetInstance(serviceInitParams, this, _stateMachine);

            //fire after service initialize
            _service.On(Service.ServiceEventReady, new EventCallback(
                // onFire
                delegate(JSONObject message)
                {
                    //LOG("Service Successfully Initialized.\n\nResponse: " + Service.PrettyJson(message) + "\n");
                    onResult(message);
                }
            ));
//
//            // fires after successful login
//            _service.On(Service.ServiceEventLogin, new EventCallback(
//                // onFire
//                delegate(JSONObject message)
//                {
//                    LOG("Login Successful...\n\nResponse: " + Service.PrettyJson(message) + "\n");
//                }
//            ));
//
            _service.On(Service.ServiceEventConnect, new EventCallback(
                // onFire
                delegate(JSONObject message)
                {
                    //LOG("Peer Activation Successful...\n\nResponse: " + Service.PrettyJson(message) + "\n");
//                    onResult(message);
                }
            ));
//
//            _service.On(Service.ServiceEventDefaultLeagueSubscribe, new EventCallback(
//                // onFire
//                delegate(JSONObject message)
//                {
//                    LOG("Default League Subscription Successful...\n\nResponse: " +
//                                Service.PrettyJson(message) + "\n");
//                }));
        }

        public GameObject getPlayPodServiceObject()
        {
            return gameObject;
        }

        public void GetUserData(Action<JSONObject> onGotUserData)
        {
            this.onGotUserData = onGotUserData;
#if UNITY_ANDROID && !UNITY_EDITOR
            this.onGotUserData = onGotUserData;
            _service.getUserData(gameObject, OnGotUserData);
#else
            JSONObject result = new JSONObject();
            result.Add("hasError", true);
            result.Add("ErrorCode", 1111);
            result.Add("ErrorMessage",
                "Cannot get user data in editor. Please build and run the game on a device with GAME CENTER installed and logged in.");
            onGotUserData(result);
#endif
        }

        public void OnGotUserData(JSONObject result)
        {
            onGotUserData(result);
        }

        public void OnGotUserDataInit(JSONObject result)
        {
            if (result["hasError"].AsBool)
            {
                LOG(Util.PrettyJson(result));
            }
            else
            {
                try
                {
                    var data = result["result"].AsObject;

                    loginParams = new LoginInfoParams();

                    loginParams.token = data["token"];
                    loginParams.name = data["name"];
                    loginParams.id = data["id"];
                    loginParams.tokenIssuer = data["tokenIssuer"].AsInt;

                    var loginData = new JSONObject();
                    loginData.Add("token", loginParams.token);
                    loginData.Add("tokenIssuer", loginParams.tokenIssuer);
                    loginData.Add("name", loginParams.name);
                    loginData.Add("id", loginParams.id);

                    _service.InitWithUserData(loginData);
                }
                catch (Exception e)
                {
                    LOG("exception: " + e.Message);
                    throw;
                }
            }
        }

        public void getLeaderBoard(Action<JSONObject> onResult)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            _service.leaderBoardUI();
#else
            JSONObject result = new JSONObject();
            result.Add("hasError", true);
            result.Add("ErrorCode", 1112);
            result.Add("ErrorMessage",
                "Cannot get leader board in editor. Please build and run the game on a device with GAME CENTER installed and logged in.");
            onResult(result);
#endif
        }

        public void Update()
        {
            if (_stateMachine != null)
            {
                _stateMachine.Update();
            }
        }

        // add here

        public void SubscribeLeagueRequest(SubscribeLeagueRequestParams subscribeLeagueRequestParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();
            
            Params.Add("leagueId", subscribeLeagueRequestParams.leagueId);
            Params.Add("leagueId", subscribeLeagueRequestParams.enrollUrl);
            Params.Add("leagueId", subscribeLeagueRequestParams.voucherHash);
            
            _service.SubscribeLeagueRequest(Params, result =>
            {
                LOG("SubscribeLeagueRequest result: " + Service.PrettyJson(result));
                onResult(result);
            });
        }

        public void CancelQuickMatchRequest(CancelQuickMatchRequestParams cancelQuickMatchRequestParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();
            
            Params.Add("leagueId", cancelQuickMatchRequestParams.leagueId);
            
            _service.CancelQuickMatchRequest(Params, result =>
            {
                LOG("CancelQuickMatchRequest result: " + Service.PrettyJson(result));
                onResult(result);
            });
        }

        public void MatchRequestResponse(MatchRequestResponseParams matchRequestResponseParams,
            Action<JSONObject> onResult)
        {
            var Params = new JSONObject();
            
            Params.Add("requestId", matchRequestResponseParams.requestId);
            Params.Add("rejectReasonType", matchRequestResponseParams.rejectReasonType);
            Params.Add("rejectMessage", matchRequestResponseParams.rejectMessage);
            
            _service.MatchRequestResponse(Params, result =>
            {
                LOG("MatchRequestResponse result: " + Service.PrettyJson(result));
                onResult(result);
            });
        }

        public void StreamMatchIdRequest(StreamMatchIdRequestParams streamMatchIdRequestParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();
            
            Params.Add("gameId", streamMatchIdRequestParams.gameId);
            Params.Add("leagueId", streamMatchIdRequestParams.leagueId);
            Params.Add("clientType", streamMatchIdRequestParams.clientType);
            
            _service.StreamMatchIdRequest(Params, result =>
            {
                LOG("StreamMatchIdRequest result: " + Service.PrettyJson(result));
                onResult(result);
            });
        }
        
        public void QuickMatchRequest(QuickMatchRequestParams quickMatchRequestParams, QuickMatchRequestCallback quickMatchRequestCallback)
        {
            var Params = new JSONObject();

            Params.Add("gameId", quickMatchRequestParams.gameId);
            Params.Add("leagueId", quickMatchRequestParams.leagueId);

            _service.QuickMatchRequest(Params, new QuickMatchRequestCallback(
                onResult: delegate(JSONObject result)
                {
                    LOG("QuickMatchRequest result.\n\n" + Service.PrettyJson(result) + "\n");
                    quickMatchRequestCallback.OnResult(result);
                },
                onAccept: delegate(JSONObject data)
                {
                    LOG("QuickMatchRequest-onAccept.\n\n" + Service.PrettyJson(data) + "\n");
                    quickMatchRequestCallback.OnAccept(data);
                },
                onCancel: delegate(JSONObject data)
                {
                    LOG("QuickMatchRequest-onCancel.\n\n" + Service.PrettyJson(data) + "\n");
                    quickMatchRequestCallback.OnCancel(data);
                }
            ));
        }
        
        public void MatchIdRequest(MatchIdRequestParams matchIdRequestParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();
            
            Params.Add("gameId", matchIdRequestParams.gameId);
            Params.Add("leagueId", matchIdRequestParams.leagueId);
            
            _service.MatchIdRequest(Params, result =>
            {
                LOG("MatchIdRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLives", result);
            });
        }

        public void CancelMatchRequest(CancelMatchRequestParams cancelMatchRequestParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();
            
            Params.Add("requestId", cancelMatchRequestParams.requestId);
            _service.CancelMatchRequest(Params, result =>
            {
                LOG("CancelMatchRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLives", result);
            });
        }

        public void MatchRequest(MatchRequestParams matchRequestParams, MatchRequestCallback matchRequestCallback)
        {
            var Params = new JSONObject();

            Params.Add("opponentId", matchRequestParams.opponentId);
            Params.Add("gameId", matchRequestParams.gameId);
            Params.Add("leagueId", matchRequestParams.leagueId);

            LOG("matchRequest...\n\nParams: " + Service.PrettyJson(Params) + "\n");

            _service.MatchRequest(Params, new MatchRequestCallback(
                onResult: delegate(JSONObject result)
                {
                    LOG("matchRequest result.\n\n" + Service.PrettyJson(result) + "\n");
                    matchRequestCallback.OnResult(result);
                    //WriteToFile("matchRequest", result);
                },
                onAccept: delegate(JSONObject data)
                {
                    LOG("matchRequest-onAccept.\n\n" + Service.PrettyJson(data) + "\n");
                    matchRequestCallback.OnAccept(data);
                    //WriteToFile("matchRequest-onAccept", data);
                },
                onReject: delegate(JSONObject data)
                {
                    LOG("matchRequest-onReject.\n\n" + Service.PrettyJson(data) + "\n");
                    matchRequestCallback.OnReject(data);
                    //WriteToFile("matchRequest-onReject", data);
                },
                onCancel: delegate(JSONObject data)
                {
                    LOG("matchRequest-onCancel.\n\n" + Service.PrettyJson(data) + "\n");
                    matchRequestCallback.OnCancel(data);
                    //WriteToFile("matchRequest-onCancel", data);
                }
            ));
        }

        public void GetLives(Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            LOG("getLives...\n\n" + Service.PrettyJson(Params));

            _service.GetLives(Params, result =>
            {
                LOG("getLives result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLives", result);
            });
        }

        public void GetLobbiesLeagues(GetLobbiesLeaguesParams getLobbiesLeaguesParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (getLobbiesLeaguesParams.lobbyIds != null && getLobbiesLeaguesParams.lobbyIds.Length > 0)
            {
                var lobbyIds = new JSONArray();

                for (var i = 0; i < getLobbiesLeaguesParams.lobbyIds.Length; i++)
                {
                    lobbyIds.Add(getLobbiesLeaguesParams.lobbyIds[i]);
                }

                Params.Add("lobbyIds", lobbyIds);
            }

            LOG("getLobbiesLeagues...\n\n" + Service.PrettyJson(Params));
            _service.GetLobbiesLeagues(Params, result =>
            {
                LOG("getLobbiesLeagues result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLobbiesLeagues", result);
            });
        }

        public void GetLobbiesGames(GetLobbiesGamesParams getLobbiesGamesParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (getLobbiesGamesParams.lobbyIds != null && getLobbiesGamesParams.lobbyIds.Length > 0)
            {
                var lobbyIds = new JSONArray();

                for (var i = 0; i < getLobbiesGamesParams.lobbyIds.Length; i++)
                {
                    lobbyIds.Add(getLobbiesGamesParams.lobbyIds[i]);
                }

                Params.Add("lobbyIds", lobbyIds);
            }

            LOG("getLobbiesGames...\n\n" + Service.PrettyJson(Params));
            _service.GetLobbiesGames(Params, result =>
            {
                LOG("getLobbiesGames result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLobbiesGames", result);
            });
        }

        public void GetGallery(GetGalleryParams getGalleryParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!getGalleryParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getGalleryParams.gameId);
            }

            if (!getGalleryParams.businessId.Equals(string.Empty))
            {
                Params.Add("businessId", getGalleryParams.businessId);
            }


            LOG("getGallery...\n\n" + Service.PrettyJson(Params));
            _service.GetGallery(Params, result =>
            {
                LOG("getGallery result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getGallery", result);
            });
        }

        public void SubscribeDefaultLeagueRequest(SubscribeDefaultLeagueRequestParams subscribeDefaultLeagueRequestParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!subscribeDefaultLeagueRequestParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", subscribeDefaultLeagueRequestParams.gameId);
            }


            LOG("subscribeDefaultLeagueRequest...\n\n" + Service.PrettyJson(Params));
            _service.SubscribeDefaultLeagueRequest(Params, result =>
            {
                LOG("subscribeDefaultLeagueRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("subscribeDefaultLeagueRequest", result);
            });
        }

        public void AddLeagueCommentRequest(AddLeagueCommentRequestParams addLeagueCommentRequestParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!addLeagueCommentRequestParams.id.Equals(string.Empty))
            {
                Params.Add("id", addLeagueCommentRequestParams.id);
            }

            if (!addLeagueCommentRequestParams.text.Equals(string.Empty))
            {
                Params.Add("text", addLeagueCommentRequestParams.text);
            }

            LOG("addLeagueCommentRequest...\n\n" + Service.PrettyJson(Params));
            _service.AddLeagueCommentRequest(Params, result =>
            {
                LOG("addLeagueCommentRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("addLeagueCommentRequest", result);
            });
        }

        public void AddGameCommentRequest(AddGameCommentRequestParams addGameCommentRequestParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!addGameCommentRequestParams.id.Equals(string.Empty))
            {
                Params.Add("id", addGameCommentRequestParams.id);
            }

            if (!addGameCommentRequestParams.text.Equals(string.Empty))
            {
                Params.Add("text", addGameCommentRequestParams.text);
            }

            LOG("addGameCommentRequest...\n\n" + Service.PrettyJson(Params));
            _service.AddGameCommentRequest(Params, result =>
            {
                LOG("addGameCommentRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("addGameCommentRequest", result);
            });
        }

        public void GetCommentList(GetCommentListParams getCommentListParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getCommentListParams.id.Equals(string.Empty))
            {
                Params.Add("id", getCommentListParams.id);
            }

            LOG("getCommentList...\n\n" + Service.PrettyJson(Params));
            _service.GetCommentList(Params, result =>
            {
                LOG("getCommentList result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getCommentList", result);
            });
        }

        public void GetLeagueMatches(GetLeagueMatchesParams getLeagueMatchesParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getLeagueMatchesParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getLeagueMatchesParams.leagueId);
            }

            if (getLeagueMatchesParams.status > 0)
            {
                Params.Add("status", getLeagueMatchesParams.status);
            }

            LOG("getLeagueMatches...\n\n" + Service.PrettyJson(Params));
            _service.GetLeagueMatches(Params, result =>
            {
                LOG("getLeagueMatches result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLeagueMatches", result);
            });
        }

        public void GetLeagueLatestMatches(GetLeagueLatestMatchesParams getLeagueLatestMatchesParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getLeagueLatestMatchesParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getLeagueLatestMatchesParams.leagueId);
            }

            if (!getLeagueLatestMatchesParams.name.Equals(string.Empty))
            {
                Params.Add("name", getLeagueLatestMatchesParams.name);
            }

            LOG("getLeagueLatestMatches...\n\n" + Service.PrettyJson(Params));
            _service.GetLeagueLatestMatches(Params, result =>
            {
                LOG("getLeagueLatestMatches result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLeagueLatestMatches", result);
            });
        }

        public void GetLeagueLatestMatchesResult(GetLeagueLatestMatchesResultParams getLeagueLatestMatchesResultParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getLeagueLatestMatchesResultParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getLeagueLatestMatchesResultParams.leagueId);
            }

            if (!getLeagueLatestMatchesResultParams.username.Equals(string.Empty))
            {
                Params.Add("username", getLeagueLatestMatchesResultParams.username);
            }

            LOG("getLeagueLatestMatchesResult...\n\n" + Service.PrettyJson(Params));
            _service.GetLeagueLatestMatchesResult(Params, result =>
            {
                LOG("getLeagueLatestMatchesResult result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLeagueLatestMatchesResult", result);
            });
        }

        public void GetLeagueMatchesResult(GetLeagueMatchesResultParams getLeagueMatchesResultParams,Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getLeagueMatchesResultParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getLeagueMatchesResultParams.leagueId);
            }

            if (!getLeagueMatchesResultParams.userId.Equals(string.Empty))
            {
                Params.Add("userId", getLeagueMatchesResultParams.userId);
            }

            if (!getLeagueMatchesResultParams.matchId.Equals(string.Empty))
            {
                Params.Add("matchId", getLeagueMatchesResultParams.matchId);
            }

            LOG("getLeagueMatchesResult...\n\n" + Service.PrettyJson(Params));
            _service.GetLeagueMatchesResult(Params, result =>
            {
                LOG("getLeagueMatchesResult result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLeagueMatchesResult", result);
            });
        }

        public void GetUserGamePoints(GetUserGamePointsParams getUserGamePointsParams,Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getUserGamePointsParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getUserGamePointsParams.gameId);
            }

            LOG("getUserGamePoints...\n\n" + Service.PrettyJson(Params));
            _service.GetUserGamePoints(Params, result =>
            {
                LOG("getUserGamePoints result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getUserGamePoints", result);
            });
        }

        public void GetUserAchievementDetail(GetUserAchievementDetailParams getUserAchievementDetailParams ,Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getUserAchievementDetailParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getUserAchievementDetailParams.gameId);
            }

            Params.Add("rankType", getUserAchievementDetailParams.rankType);

            LOG("getUserAchievementDetail...\n\n" + Service.PrettyJson(Params));
            _service.GetUserAchievementDetail(Params, result =>
            {
                LOG("getUserAchievementDetail result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getUserAchievementDetail", result);
            });
        }

        public void GetUserAchievements(GetUserAchievementsParams getUserAchievementsParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!getUserAchievementsParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getUserAchievementsParams.gameId);
            }

            if (!getUserAchievementsParams.userId.Equals(string.Empty))
            {
                Params.Add("userId", getUserAchievementsParams.userId);
            }

            Params.Add("type", getUserAchievementsParams.type);

            LOG("getUserAchievements...\n\n" + Service.PrettyJson(Params));
            _service.GetUserAchievements(Params, result =>
            {
                LOG("getUserAchievements result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getUserAchievements", result);
            });
        }

        public void SendGameRateRequest(SendGameRateRequestParams sendGameRateRequestParams,Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!sendGameRateRequestParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", sendGameRateRequestParams.gameId);
            }

            if (sendGameRateRequestParams.rate != -1)
            {
                Params.Add("rate", sendGameRateRequestParams.rate);
            }

            LOG("sendGameRateRequest...\n\n" + Service.PrettyJson(Params));
            _service.SendGameRateRequest(Params, result =>
            {
                LOG("sendGameRateRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("sendGameRateRequest", result);
            });
        }

        public void SendLeagueRateRequest(SendLeagueRateRequestParams sendLeagueRateRequestParams,Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!sendLeagueRateRequestParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", sendLeagueRateRequestParams.leagueId);
            }

            if (sendLeagueRateRequestParams.rate != -1)
            {
                Params.Add("rate", sendLeagueRateRequestParams.rate);
            }

            LOG("sendLeagueRateRequest...\n\n" + Service.PrettyJson(Params));
            _service.SendLeagueRateRequest(Params, result =>
            {
                LOG("sendLeagueRateRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("sendLeagueRateRequest", result);
            });
        }

        public void GetEnrollAccess(GetEnrollAccessParams getEnrollAccessParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!getEnrollAccessParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getEnrollAccessParams.leagueId);
            }

            LOG("getEnrollAccess...\n\n" + Service.PrettyJson(Params));
            _service.GetEnrollAccess(Params, result =>
            {
                LOG("getEnrollAccess result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getEnrollAccess", result);
            });
        }

        public void GetEnrolledLeagues(GetEnrolledLeaguesParams getEnrolledLeaguesParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getEnrolledLeaguesParams.userId.Equals(string.Empty))
            {
                Params.Add("userId", getEnrolledLeaguesParams.userId);
            }

            LOG("getEnrolledLeagues...\n\n" + Service.PrettyJson(Params));
            _service.GetEnrolledLeagues(Params, result =>
            {
                LOG("getEnrolledLeagues result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getEnrolledLeagues", result);
            });
        }

        public void GetOnlineInfo(GetOnlineInfoParams getOnlineInfoParams, Action<JSONObject> onResult)
        {
            var Params = new JSONObject();

            if (!getOnlineInfoParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getOnlineInfoParams.gameId);
            }

            LOG("getOnlineInfo...\n\n" + Service.PrettyJson(Params));
            _service.GetOnlineInfo(Params, result =>
            {
                LOG("getOnlineInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getOnlineInfo", result);
            });
        }

        public void GetTopPlayers(GetTopPlayersParams getTopPlayersParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getTopPlayersParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getTopPlayersParams.gameId);
            }

            LOG("getTopPlayers...\n\n" + Service.PrettyJson(Params));
            _service.GetTopPlayers(Params, result =>
            {
                LOG("getTopPlayers result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getTopPlayers", result);
            });
        }

        public void GetLeagueTopPlayers(GetLeagueTopPlayersParams getLeagueTopPlayersParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getLeagueTopPlayersParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getLeagueTopPlayersParams.leagueId);
            }

            LOG("getLeagueTopPlayers...\n\n" + Service.PrettyJson(Params));
            _service.GetLeagueTopPlayers(Params, result =>
            {
                LOG("getLeagueTopPlayers result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLeagueTopPlayers", result);
            });
        }

        public void GetLobby(Action<JSONObject> onResult)
        {
            LOG("getLobby...\n\n");
            _service.GetLobby(result =>
            {
                LOG("getLobby result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLobby", result);
            });
        }

        public void ConsumeItemRequest(ConsumeItemRequestParams consumeItemRequestParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!consumeItemRequestParams.itemId.Equals(string.Empty))
            {
                Params.Add("itemId", consumeItemRequestParams.itemId);
            }

            if (consumeItemRequestParams.count > 0)
            {
                Params.Add("count", consumeItemRequestParams.count);
            }

            LOG("consumeItemRequest...\n\n" + Service.PrettyJson(Params));
            _service.ConsumeItemRequest(Params, result =>
            {
                LOG("consumeItemRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("consumeItemRequest", result);
            });
        }

        public void GetUserItems(GetUserItemsParams getUserItemsParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getUserItemsParams.itemId.Equals(string.Empty))
            {
                Params.Add("itemId", getUserItemsParams.itemId);
            }

            if (!getUserItemsParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getUserItemsParams.gameId);
            }

            LOG("getUserItems...\n\n" + Service.PrettyJson(Params));
            _service.GetUserItems(Params, result =>
            {
                LOG("getUserItems result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getUserItems", result);
            });
        }

        public void GetUserGameCenterItem(GetUserGameCenterItemParams getUserGameCenterItemParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getUserGameCenterItemParams.itemId.Equals(string.Empty))
            {
                Params.Add("itemId", getUserGameCenterItemParams.itemId);
            }

            LOG("getUserGameCenterItem...\n\n" + Service.PrettyJson(Params));
            _service.GetUserGameCenterItem(Params, result =>
            {
                LOG("getUserGameCenterItem result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getUserGameCenterItem", result);
            });
        }

        public void GetGameItems(GetGameItemsParams getGameItemsParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getGameItemsParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getGameItemsParams.gameId);
            }

            if (!getGameItemsParams.itemId.Equals(string.Empty))
            {
                Params.Add("itemId", getGameItemsParams.itemId);
            }

            LOG("getGameItems...\n\n" + Service.PrettyJson(Params));
            _service.GetGameItems(Params, result =>
            {
                LOG("getGameItems result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getGameItems", result);
            });
        }

        public void GetGlobalInAppPurchasePack(GetGlobalInAppPurchasePackParams getGlobalInAppPurchasePackParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getGlobalInAppPurchasePackParams.itemId.Equals(string.Empty))
            {
                Params.Add("itemId", getGlobalInAppPurchasePackParams.itemId);
            }

            LOG("getGlobalInAppPurchasePack...\n\n" + Service.PrettyJson(Params));
            _service.GetGlobalInAppPurchasePack(Params, result =>
            {
                LOG("getGlobalInAppPurchasePack result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getGlobalInAppPurchasePack", result);
            });
        }

        public void GetInAppPurchasePack(GetInAppPurchasePackParams getInAppPurchasePackParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getInAppPurchasePackParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getInAppPurchasePackParams.gameId);
            }

            if (!getInAppPurchasePackParams.packId.Equals(string.Empty))
            {
                Params.Add("packId", getInAppPurchasePackParams.packId);
            }

            if (!getInAppPurchasePackParams.itemId.Equals(string.Empty))
            {
                Params.Add("itemId", getInAppPurchasePackParams.itemId);
            }

            if (!getInAppPurchasePackParams.nameFilter.Equals(string.Empty))
            {
                Params.Add("nameFilter", getInAppPurchasePackParams.nameFilter);
            }

            LOG("getInAppPurchasePack...\n\n" + Service.PrettyJson(Params));
            _service.GetInAppPurchasePack(Params, result =>
            {
                LOG("getInAppPurchasePack result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getInAppPurchasePack", result);
            });
        }

        public void GetUserProfile(GetUserProfileParams getUserProfileParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getUserProfileParams.userId.Equals(string.Empty))
            {
                Params.Add("userId", getUserProfileParams.userId);
            }


            LOG("getUserProfile...\n\n" + Service.PrettyJson(Params));
            _service.GetUserProfile(Params, result =>
            {
                LOG("getUserProfile result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getUserProfile", result);
            });
        }

        public void GetLeagueAwards(GetLeagueAwardsParams getLeagueAwardsParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getLeagueAwardsParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getLeagueAwardsParams.leagueId);
            }


            LOG("getLeagueAwards...\n\n" + Service.PrettyJson(Params));
            _service.GetLeagueAwards(Params, result =>
            {
                LOG("getLeagueAwards result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLeagueAwards", result);
            });
        }

        public void SearchUserRequest(SearchUserRequestParams searchUserRequestParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!searchUserRequestParams.name.Equals(string.Empty))
            {
                Params.Add("name", searchUserRequestParams.name);
            }

            LOG("searchUserRequest...\n\n" + Service.PrettyJson(Params));
            _service.SearchUserRequest(Params, result =>
            {
                LOG("searchUserRequest result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("searchUserRequest", result);
            });
        }

        public void GetTableData(GetTableDataParams getTableDataParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getTableDataParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getTableDataParams.leagueId);
            }

            Params.Add("rangeType", getTableDataParams.rangeType);

            LOG("getTableData...\n\n" + Service.PrettyJson(Params));
            _service.GetTableData(Params, result =>
            {
                LOG("getTableData result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getTableData", result);
            });
        }

        public void GetRelatedGamesInfo(GetRelatedGamesInfoParams getRelatedGamesInfoParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getRelatedGamesInfoParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getRelatedGamesInfoParams.gameId);
            }

            Params.Add("type", getRelatedGamesInfoParams.type);

            LOG("getRelatedGamesInfo...\n\n" + Service.PrettyJson(Params));
            _service.GetRelatedGamesInfo(Params, result =>
            {
                LOG("getRelatedGamesInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getRelatedGamesInfo", result);
            });
        }

        public void GetLatestGamesInfo(Action<JSONObject> onResult)
        {
            LOG("getLatestGamesInfo...\n\n" + Service.PrettyJson(GetDefaultParams()));
            _service.GetLatestGamesInfo(GetDefaultParams(), result =>
            {
                LOG("getLatestGamesInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLatestGamesInfo", result);
            });
        }

        public void GetTopGamesInfo(GetTopGamesInfoParams getTopGamesInfoParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();
            Params.Add("type", getTopGamesInfoParams.type);

            LOG("getTopGamesInfo...\n\n" + Service.PrettyJson(Params));
            _service.GetTopGamesInfo(Params, result =>
            {
                LOG("getTopGamesInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getTopGamesInfo", result);
            });
        }

        public void GetGamesInfo(GetGamesInfoParams getGamesInfoParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            var gamesId = new JSONArray();

            for (var i = 0; i < getGamesInfoParams.gamesId.Length; i++)
            {
                gamesId.Add(getGamesInfoParams.gamesId[i]);
            }

            if (gamesId.Count > 0)
            {
                Params.Add("gamesId", gamesId);
            }

            if (!getGamesInfoParams.lobbyId.Equals(string.Empty))
            {
                Params.Add("lobbyId", getGamesInfoParams.lobbyId);
            }

            if (!getGamesInfoParams.name.Equals(string.Empty))
            {
                Params.Add("name", getGamesInfoParams.name);
            }

            LOG("getGamesInfo...\n\n" + Service.PrettyJson(Params));
            _service.GetGamesInfo(Params, result =>
            {
                LOG("getGamesInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getGamesInfo", result);
            });
        }

        public void GetLeagueMembers(GetLeagueMembersParams getLeagueMembersParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getLeagueMembersParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getLeagueMembersParams.leagueId);
            }

            Params.Add("userState", getLeagueMembersParams.userState);

            if (!getLeagueMembersParams.name.Equals(string.Empty))
            {
                Params.Add("name", getLeagueMembersParams.name);
            }

            LOG("getLeagueMembers...\n\n" + Service.PrettyJson(Params));
            _service.GetLeagueMembers(Params, result =>
            {
                LOG("getLeagueMembers result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLeagueMembers", result);
            });
        }

        public void GetLatestLeaguesInfo(Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();
            LOG("getLatestLeaguesInfo...\n\n" + Service.PrettyJson(GetDefaultParams()));
            _service.GetLatestLeaguesInfo(GetDefaultParams(), result =>
            {
                LOG("getLatestLeaguesInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getLatestLeaguesInfo", result);
            });
        }

        public void GetTopLeaguesInfo(GetTopLeaguesInfoParams getTopLeaguesInfoParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getTopLeaguesInfoParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getTopLeaguesInfoParams.gameId);
            }

            Params.Add("type", getTopLeaguesInfoParams.type);

            LOG("getTopLeaguesInfo...\n\n" + Service.PrettyJson(Params));

            _service.GetTopLeaguesInfo(Params, result =>
            {
                LOG("getTopLeaguesInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getTopLeaguesInfo", result);
            });
        }

        public void GetRelatedLeaguesInfo(GetRelatedLeaguesInfoParams getRelatedLeaguesInfoParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getRelatedLeaguesInfoParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getRelatedLeaguesInfoParams.leagueId);
            }

            Params.Add("type", getRelatedLeaguesInfoParams.type);

            LOG("getRelatedLeaguesInfo...\n\n" + Service.PrettyJson(Params));

            _service.GetRelatedLeaguesInfo(Params, result =>
            {
                LOG("getRelatedLeaguesInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getRelatedLeaguesInfo", result);
            });
        }

        public void GetLeaguesInfo(GetLeagueInfoParams getLeagueInfoParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            if (!getLeagueInfoParams.gameId.Equals(string.Empty))
            {
                Params.Add("gameId", getLeagueInfoParams.gameId);
            }

            if (!getLeagueInfoParams.leagueId.Equals(string.Empty))
            {
                Params.Add("leagueId", getLeagueInfoParams.leagueId);
            }

            var leaguesId = new JSONArray();
            for (var i = 0; i < getLeagueInfoParams.leaguesId.Length; i++)
            {
                leaguesId.Add(getLeagueInfoParams.leaguesId[i]);
            }

            if (leaguesId.Count > 0)
            {
                Params.Add("leaguesId", leaguesId);
            }

            if (!getLeagueInfoParams.name.Equals(string.Empty))
            {
                Params["filter"] = getLeagueInfoParams.name;
            }

            Params.Add("prize", getLeagueInfoParams.prize);
            Params.Add("status", getLeagueInfoParams.status);

            var statusList = new JSONArray();
            for (var i = 0; i < getLeagueInfoParams.statusList.Length; i++)
            {
                statusList.Add(getLeagueInfoParams.statusList[i]);
            }

            if (statusList.Count > 0)
            {
                Params.Add("statusList", statusList);
            }


            Params.Add("financialType", getLeagueInfoParams.financialType);
            Params.Add("userState", getLeagueInfoParams.userState);
            Params.Add("showDefault", getLeagueInfoParams.showDefault);

            if (!getLeagueInfoParams.lobbyId.Equals(string.Empty))
            {
                Params.Add("lobbyId", getLeagueInfoParams.lobbyId);
            }

            LOG("getleaguesInfo...\n\n" + Service.PrettyJson(Params));

            _service.GetLeaguesInfo(Params, result =>
            {
                LOG("getleaguesInfo result: " + Service.PrettyJson(result));
                onResult(result);
                //WriteToFile("getleaguesInfo", result);
            });
        }

        public void GetOnlineUser(GetOnlineUserParams getOnlineUserParams, Action<JSONObject> onResult)
        {
            var Params = GetDefaultParams();

            Params.Add("gameId", getOnlineUserParams.gameId);
            Params.Add("leagueId", getOnlineUserParams.leagueId);

            LOG("getOnlineUser...\n\nParams: " + Service.PrettyJson(Params));

            _service.GetOnlineUser(Params, result =>
            {
                LOG("getOnlineUser result.\n\n" + Service.PrettyJson(result) + "\n");

                onResult(result);
                //WriteToFile("getOnlineUser", result);
            });
        }
        
        private JSONObject GetDefaultParams()
        {
            var defaultParamsJson = new JSONObject();
            defaultParamsJson.Add("size", defaultParams.size);
            defaultParamsJson.Add("offset", defaultParams.offset);

            if (!defaultParams.filter.Equals(string.Empty))
            {
                defaultParamsJson.Add("filter", defaultParams.filter);
            }

            return defaultParamsJson;
        }

        public void SetDefaultParams(int size = 5, int offset = 0, string filter = "")
        {
            defaultParams.size = size;
            defaultParams.offset = offset;
            defaultParams.filter = filter;
        }

        // should finish here

        # region helper functions

        /// <summary>
        ///  متد دریافت کننده اطلاعات از پلاگین جاوا و فراخوانی متد گیرنده 
        /// </summary>
        /// <param name="data">رشته بازگشتی از سمت جاوا</param>
        public void ReceiveCallback(string data)
        {
//            LOG("ReceiveCallback, data: " + data);
            try
            {
                ReceiveData rd2 = new ReceiveData(data);
                if (rd2 != null && !string.IsNullOrEmpty(rd2.Receiver) && !string.IsNullOrEmpty(rd2.Method))
                {
                    GameObject.Find(rd2.Receiver).BroadcastMessage(rd2.Method, rd2.Data);
                }
            }
            catch (Exception exp)
            {
                LOG("Exception : " + exp.Message);
            }
        }

        void WriteToFile(string path, string functionName, JSONObject result)
        {
            File.WriteAllText(path + functionName + ".json",
                Service.PrettyJson(result));
//            LOG("Results were written to Desktop/testResults/" + functionName + ".json\n");
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

        public static void ClearConsole()
        {
# if UNITY_EDITOR
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            if (method != null) method.Invoke(new object(), null);
#endif
        }

        # endregion
    }
}
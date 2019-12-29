package com.fanap.gameCenter.unity;

import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.content.pm.PackageManager;
import android.os.IBinder;
import android.os.RemoteException;
import android.util.Log;
import land.pod.play.ipc.LauncherInterface;
import com.unity3d.player.UnityPlayer;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.Arrays;


/**
 * Created by saeed on 12/31/2016.
 */
public class PlaypodServiceWrapper {

    private static final String TAG = "PlayPodServiceWrapper";
    
    private final static String UNITY_CALLBACK_GAMEOBJECT_NAME = "PlayPodService";
    private final static String UNITY_CALLBACK_METHOD_NAME = "ReceiveCallback";

    private final static String Callback_loginRequest = "Callback_loginRequest";
    private final static String Callback_getOnlineUser = "Callback_getOnlineUrser";
    private final static String Callback_getGamesInfo = "Callback_getGamesInfo";
    private final static String Callback_verifyWithCompleteProfileRequest = "Callback_verifyWithCompleteProfileRequest";
    private final static String Callback_matchRequest = "Callback_matchRequest";
    private final static String OnCancel_Match = "OnCancel_Match";
    private final static String OnAccept_Match = "OnAccept_Match";

    private final static String Callback_OnMessage = "Callback_OnMessage";
    private final static String Callback_MatchRequestResponse = "Callback_MatchRequestResponse";
    private final static String Callback_signupRequest = "Callback_signupRequest";
    private final static String Callback_OnNewMatch = "Callback_OnNewMatch";
    private final static String Callback_sendMatchReadyRequest = "Callback_sendMatchReadyRequest";
    private final static String Callback_OnReady = "Callback_OnReady";
    private final static String Callback_OnMatchStart = "Callback_OnMatchStart";
    private final static String Callback_OnMatchPause = "Callback_OnMatchPause";
    private final static String Callback_OnMatchResume = "Callback_OnMatchResume";
    private final static String Callback_OnMatchEnd = "Callback_OnMatchEnd";
    private final static String Callback_OnMatchLeave = "Callback_OnMatchLeave";
    private final static String Callback_sendMatchResultRequest = "Callback_sendMatchResultRequest";
    private final static String Callback_logoutRequest = "Callback_logoutRequest";
    private final static String Callback_sendMatchDataRequest = "Callback_sendMatchDataRequest";
    private final static String Callback_OnConnect = "Callback_OnConnect";
    private final static String MatchReceiveData = "MatchReceiveData";
    private final static String Callback_getGameItems = "Callback_getGameItems";
    private final static String Callback_getInAppPurchasePack = "Callback_getInAppPurchasePack";
    private final static String Callback_buyInAppPurchasePackRequest = "Callback_buyInAppPurchasePackRequest";
    private final static String Callback_getUserItems = "Callback_getUserItems";
    private final static String Callback_consumeItemRequest = "Callback_consumeItemRequest";
    private final static String Callback_sendScoreRequest = "Callback_sendScoreRequest";
    private final static String Callback_getTableData = "Callback_getTableData";
    private final static String Callback_changePasswordRequest = "Callback_changePasswordRequest";
    private final static String Callback_increaseCreditWithVoucherRequest = "Callback_increaseCreditWithVoucherRequest";
    private final static String Callback_getCredit = "Callback_getCredit";
    private final static String Callback_getLeagueAwards = "Callback_getLeagueAwards";
    private final static String Callback_searchUserRequest = "Callback_searchUserRequest";
    private final static String Callback_forgetPasswordRequest = "Callback_forgetPasswordRequest";
    private final static String Callback_getLeagueMembers = "Callback_getLeagueMembers";
    private final static String Callback_matchIdRequest = "Callback_matchIdRequest";
    private final static String Callback_changeVisibilityRequest = "Callback_changeVisibilityRequest";
    private final static String Callback_getOnlineInfo = "Callback_getOnlineInfo";
    private final static String Callback_getTopGamesInfo = "Callback_getTopGamesInfo";
    private final static String Callback_getNews = "Callback_getNews";
    private final static String Callback_followRequest = "Callback_followRequest";
    private final static String Callback_getTopPlayers = "Callback_getTopPlayers";
    private final static String Callback_suggestionRequest = "Callback_suggestionRequest";
    private final static String Callback_quickMatchRequest = "Callback_quickMatchRequest";
    private final static String OnCancel_QuickMatch = "OnCancel_QuickMatch";
    private final static String OnAccept_QuickMatch = "OnAccept_QuickMatch";
    private final static String Callback_cancelQuickMatchRequest = "Callback_cancelQuickMatchRequest";
    private final static String Callback_getCustomPost = "Callback_getCustomPost";
    private final static String Callback_sendMatchLeaveRequest = "Callback_sendMatchLeaveRequest";
    private final static String Callback_sendMatchCancelRequest = "Callback_sendMatchCancelRequest";
    private final static String Callback_getRelatedLeaguesInfo = "Callback_getRelatedLeaguesInfo";
    private final static String Callback_getCreditPackList = "Callback_getCreditPackList";
    private final static String Callback_subscribeLeagueRequest = "Callback_subscribeLeagueRequest";
    private final static String Callback_shareRequest = "Callback_shareRequest";
    private final static String Callback_getEnrollAccess = "Callback_getEnrollAccess";
    private final static String Callback_getFileInfo = "Callback_getFileInfo";
    private final static String Callback_editProfileImageRequest = "Callback_editProfileImageRequest";
    private final static String Callback_editProfileRequest = "Callback_editProfileRequest";
    private final static String OnLogin = "OnLogin";
    private final static String OnLogout = "OnLogout";
    private final static String Callback_verifyRequest = "Callback_verifyRequest";
    private final static String Callback_cancelMatchRequest = "Callback_cancelMatchRequest";
    private final static String Callback_getUserProfile = "Callback_getUserProfile";
    private final static String Callback_getLeaguesInfo = "Callback_getLeaguesInfo";
    private final static String Callback_getUserGameCenterItem = "Callback_getUserGameCenterItem";
    private final static String Callback_registerGuestReuest = "Callback_registerGuestReuest";

    private final static String Callback_launcher_loginUI = "Callback_loginUI";
    private final static String Callback_launcher_signupUI = "Callback_signupUI";
    private final static String Callback_launcher_editProfileUI = "Callback_editProfileUI";
    private final static String Callback_launcher_inAppPurchaseUI = "Callback_inAppPurchaseUI";
    private final static String Callback_launcher_increaseCreditUI = "Callback_increaseCreditUI";
    private final static String Callback_launcher_subscribeLeagueUI = "Callback_subscribeLeagueUI";
    private final static String Callback_launcher_buyPackUI = "Callback_buyPackUI";
    private final static String Callback_launcher_getUserData = "Callback_getUserData";


    private Context context;
    private static boolean showLogs;



    private String launcherPackageName = "land.pod.play";
    private String launcherInterfaceAction = "land.pod.play.ipc.LauncherInterface";
    private LauncherInterface launcherInterface = null;
    private ServiceConnection launcherConnection = null;

    public PlaypodServiceWrapper(Context contx, String[] GameID, String version, boolean log, String serviceMode) {
        context = contx;
        JSONObject jo = new JSONObject();
        try {
//            chatService = null;
            if (!serviceMode.isEmpty())
                jo.put("serviceMode", serviceMode);
            jo.put("context", contx);
            jo.put("version", version);
            JSONObject games = new JSONObject();
            for (int i = 0; i < GameID.length; i++)
                games.put(GameID[i], new JSONObject());
            jo.put("games", games);

            showLogs = log;
//            _service = new Service(jo);
//
//            _service.on("message", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnMessage, returnData);
//                    LOG("on message : " + returnData.toString());
//                }
//            });
//
//            _service.on("connect", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnConnect, returnData);
//                    LOG("on connect : " + returnData.toString());
//                }
//            });
//
//            _service.on("newMatch", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnNewMatch, returnData);
//                    LOG("on newMatch : " + returnData.toString());
//                }
//            });
//            _service.on("login", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(OnLogin, returnData);
//                    LOG("on login : " + returnData.toString());
//                }
//            });
//
//            _service.on("logout", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(OnLogout, returnData);
//                    LOG("on login : " + returnData.toString());
//                }
//            });
//
//
//            _service.on("ready", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnReady, returnData);
//                    LOG("on ready : " + returnData.toString());
//
////                    if (_service.getChatService() != null) {
////                        LOG("on getChatSeicerv ready 1 : " + returnData.toString());
////                        chatService = new ChatServiceWrapper(_service.getChatService(), showLogs);
////                    }
//
//                }
//            });
//
//            _service.on("matchStart", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnMatchStart, returnData);
//                    LOG("on matchStart : " + returnData.toString());
//                }
//            });
//
//            _service.on("matchPause", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnMatchPause, returnData);
//                    LOG("on matchPause : " + returnData.toString());
//                }
//            });
//
//            _service.on("matchResume", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnMatchResume, returnData);
//                    LOG("on ready : " + returnData.toString());
//                }
//            });
//
//            _service.on("matchEnd", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnMatchEnd, returnData);
//                    LOG("on matchEnd : " + returnData.toString());
//                }
//            });
//
//            _service.on("matchLeave", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_OnMatchLeave, returnData);
//                    LOG("on matchLeave : " + returnData.toString());
//                }
//            });
//
//            _service.on("matchRequestResponse", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(Callback_MatchRequestResponse, returnData);
//                    LOG("on matchRequestResponse : " + returnData.toString());
//                }
//            });
//
//            _service.on("matchReceiveData", new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    SendResultToUnity(MatchReceiveData, returnData);
//                    LOG("on matchReceiveData : " + returnData.toString());
//                }
//            });


        } catch (Exception e) {
            LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());

        }
    }

    private void initLauncherConnection(final LauncherConnection connection){

        Log.i(TAG, "initLauncherConnection: ");

        if (launcherInterface != null) {
            Log.i(TAG, "initLauncherConnection: (launcherInterface != null");
            connection.onConnect(launcherInterface);
            return;
        }
        Intent intent = new Intent(launcherInterfaceAction);
        intent.setPackage(launcherPackageName);

        launcherConnection = new ServiceConnection() {
            @Override
            public void onServiceConnected(ComponentName name, IBinder binder) {
                Log.i(TAG, "onServiceConnected: ");
                launcherInterface = LauncherInterface.Stub.asInterface(binder);

                connection.onConnect(launcherInterface);
            }

            @Override
            public void onServiceDisconnected(ComponentName name) {
                System.out.println("onServiceDisconnected_ " + name);
                launcherInterface = null;
            }
        };
        context.bindService(intent, launcherConnection, Context.BIND_AUTO_CREATE);
        Log.i(TAG, "initLauncherConnection: after");
    }


    public static interface LauncherConnection {
        void onConnect(LauncherInterface launcherInterface);
    }


    public void addEventListener(final String method, final String callbackReceiver, final String callbackMethod) {
        LOG("1 addEventListener : " + method);
//        if (_service != null) {
//            LOG("2 addEventListener : " + method + " reciever " + callbackReceiver + " callback " + callbackMethod);
//            _service.on(method, new EventCallback() {
//                @Override
//                public void onFire(JSONObject returnData) {
//                    LOG(" on - " + method + " : " + returnData.toString());
//                    SendResultToUnity(callbackReceiver, callbackMethod, "", returnData);
//                }
//            });
//        }
    }

    public void signupRequest(final String callbackReceiver, final String callbackMethod, String cellphoneNumber, boolean resend) {
//        if (_service != null) {
//
//            try {
//                JSONObject jo2 = new JSONObject();
//                jo2.put("cellphoneNumber", cellphoneNumber);
//                jo2.put("resend", resend);
//                _signupRequest(callbackReceiver, callbackMethod, jo2);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void signupRequest(final String callbackReceiver, final String callbackMethod, String cellphoneNumber) {
//        if (_service != null) {
//            try {
//                JSONObject jo2 = new JSONObject();
//                jo2.put("cellphoneNumber", cellphoneNumber);
//                _signupRequest(callbackReceiver, callbackMethod, jo2);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    private void _signupRequest(final String callbackReceiver, final String callbackMethod, JSONObject jo) {
//        if (_service != null) {
//            try {
//                _service.signupRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_signupRequest, returnData);
//                        LOG("2 signupRequest " + returnData.toString());
//                    }
//                });
//                LOG("3  signupRequest ");
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void loginRequest(final String callbackReceiver, final String callbackMethod, String userName, String code) {
//        if (_service != null) {
//            try {
//                LOG("1  login callbackReciever " + callbackReceiver + " callbackMethod " + callbackMethod);
//                JSONObject jo2 = new JSONObject();
//                jo2.put("userName", userName);
//                jo2.put("code", code);
//                _service.loginRequest(jo2, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_loginRequest, returnData);
//                        LOG("2 login " + returnData.toString());
//                    }
//                });
//                LOG("3  login ");
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        } else {
//        }
    }

    public void SendResultToUnity(String callbackReceiver, String callbackMethod, String defaultCallbackMethod, JSONObject Data) {
        Log.i(TAG, "SendResultToUnity: ");
        final String reciever;
        if (callbackReceiver.isEmpty())
            reciever = UNITY_CALLBACK_GAMEOBJECT_NAME;
        else
            reciever = callbackReceiver;
        final String method;
        if (callbackMethod.isEmpty())
            method = defaultCallbackMethod;
        else
            method = callbackMethod;

        JSONObject result = new JSONObject();
        try {
            result.put("Receiver", reciever);
            result.put("Method", method);
            result.put("Data", Data);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage(UNITY_CALLBACK_GAMEOBJECT_NAME, UNITY_CALLBACK_METHOD_NAME, result.toString());
    }

    public void SendResultToUnity(String defaultCallbackMethod, JSONObject Data) {
        JSONObject result = new JSONObject();
        try {
            result.put("Receiver", UNITY_CALLBACK_GAMEOBJECT_NAME);
            result.put("Method", defaultCallbackMethod);
            result.put("Data", Data);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage(UNITY_CALLBACK_GAMEOBJECT_NAME, UNITY_CALLBACK_METHOD_NAME, result.toString());
    }

    public void getOnlineUser(final String callbackReceiver, final String callbackMethod, String gameId, String leagueId) {
        LOG("1 getOnlineUserRequest ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("leagueId", leagueId);
//                _getOnlineUser(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//
//            }
//        }
    }

    public void getOnlineUser(final String callbackReceiver, final String callbackMethod, String gameId, String leagueId, int size, int offset, String filter) {
        LOG("1 getOnlineUserRequest ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("leagueId", leagueId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                jo.put("filter", filter);
//                _getOnlineUser(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//
//            }
//        }
    }

    private void _getOnlineUser(final String callbackReceiver, final String callbackMethod, JSONObject jo) {
        LOG("1 getOnlineUserRequest ");
//        if (_service != null) {
//            try {
//                LOG("2 getOnlineUserRequest   ");
//                _service.getOnlineUser(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getOnlineUser, returnData);
//                        LOG("3 getOnlineUserRequest " + returnData.toString());
//                    }
//                });
//
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//
//            }
//        }
    }

    public void getGamesInfo(final String callbackReceiver, final String callbackMethod, String[] gamesId) {
        LOG("1  getGamesInfo " + gamesId);
//        if (_service != null) {
//            try {
//                JSONObject jo2 = new JSONObject();
//                jo2.put("gamesId", new JSONArray(Arrays.asList(gamesId)));
//                _getGamesInfo(callbackReceiver, callbackMethod, jo2);
//
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void getGamesInfo(final String callbackReceiver, final String callbackMethod, String[] gamesId, String name) {
        LOG("1  getGamesInfo " + gamesId);
//        if (_service != null) {
//            try {
//                JSONObject jo2 = new JSONObject();
//                jo2.put("gamesId", new JSONArray(Arrays.asList(gamesId)));
//                jo2.put("name", name);
//                _getGamesInfo(callbackReceiver, callbackMethod, jo2);
//
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void getGamesInfo(final String callbackReceiver, final String callbackMethod, String[] gamesId, String name, int size, int offset) {
        LOG("1  getGamesInfo ");
//        if (_service != null) {
//            try {
//                JSONObject jo2 = new JSONObject();
//                jo2.put("gamesId", new JSONArray(Arrays.asList(gamesId)));
//                jo2.put("name", name);
//                jo2.put("size", size);
//                jo2.put("offset", offset);
//                _getGamesInfo(callbackReceiver, callbackMethod, jo2);
//
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    private void _getGamesInfo(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getGamesInfo(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getGamesInfo, returnData);
//                LOG("2 login " + returnData.toString());
//            }
//        });
    }

    public void verifyWithCompleteProfileRequest(final String callbackReciever, final String callbackMethod, String cellphoneNumber, String code, String nickName) {
        LOG("1  verifyWithCompleteProfileRequest ");
//        if (_service != null) {
//            try {
//                JSONObject jo2 = new JSONObject();
//                jo2.put("cellphoneNumber", cellphoneNumber);
//                jo2.put("code", code);
//                jo2.put("nickName", nickName);
//                _service.verifyWithCompleteProfileRequest(jo2, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReciever, callbackMethod, Callback_verifyWithCompleteProfileRequest, returnData);
//                        LOG("2 verifyWithCompleteProfileRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void verifyWithCompleteProfileRequest(final String callbackReceiver, final String callbackMethod, String cellphoneNumber, String code, String nickName, String newCode) {
        LOG("1  verifyWithCompleteProfileRequest ");
//        if (_service != null) {
//            try {
//                JSONObject jo2 = new JSONObject();
//                jo2.put("cellphoneNumber", cellphoneNumber);
//                jo2.put("code", code);
//                jo2.put("nickName", nickName);
//                jo2.put("newCode", newCode);
//                _service.verifyWithCompleteProfileRequest(jo2, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_verifyWithCompleteProfileRequest, returnData);
//                        LOG("2 verifyWithCompleteProfileRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void matchRequest(final String callbackReciever, final String callbackMethod, String gameId, String leagueId, String opponentId,
                             final String callbackCancelMethod, final String callbackAcceptMethod) {
        LOG("1  matchRequest ");
//        if (_service != null) {
//            try {
//                LOG("2  matchRequest ");
//                JSONObject jo2 = new JSONObject();
//                jo2.put("gameId", gameId);
//                jo2.put("leagueId", leagueId);
//                jo2.put("opponentId", opponentId);
//                _service.matchRequest(jo2, new MatchRequestCallback() {
//                    @Override
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReciever, callbackMethod, Callback_matchRequest, returnData);
//                        LOG("3 matchRequest " + returnData.toString());
//                    }
//
//                    @Override
//                    public void onCancel(JSONObject returnData) {
//                        SendResultToUnity(callbackReciever, callbackCancelMethod, OnCancel_Match, returnData);
//                        LOG("3 matchRequest " + returnData.toString());
//                    }
//
//                    @Override
//                    public void onAccept(JSONObject returnData) {
//                        SendResultToUnity(callbackReciever, callbackAcceptMethod, OnAccept_Match, returnData);
//                        LOG("3 matchRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 matchRequest Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void matchRequestResponse(final String callbackReceiver, final String callbackMethod, String requestId, boolean reject) {
        LOG("1  MatchRequestResponse ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("requestId", requestId);
//                if (reject) {
//                    jo.put("rejectReasonType", 0);
//                }
//                _matchRequestResponse(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void matchRequestResponse(final String callbackReceiver, final String callbackMethod, String requestId, boolean reject, String rejectMessage) {
        LOG("1  MatchRequestResponse ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("requestId", requestId);
//                if (reject) {
//                    jo.put("rejectReasonType", MatchRequestRejectTypes.USER_NOT_ACCEPT);
//                    jo.put("rejectMessage", rejectMessage);
//                }
//                _matchRequestResponse(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void _matchRequestResponse(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.matchRequestResponse(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_MatchRequestResponse, returnData);
//                LOG("2 MatchRequestResponse " + returnData.toString());
//            }
//        });
    }

    public void sendMatchReadyRequest(final String callbackReceiver, final String callbackMethod, String matchId) {
        LOG("1  SendMatchReadyRequest ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("matchId", matchId);
//                _service.sendMatchReadyRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_sendMatchReadyRequest, returnData);
//                        LOG("2 SendMatchReadyRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void sendMatchResultRequest(final String callbackReceiver, final String callbackMethod, String gameId, String result) {
        LOG("1  SendMatchResultRequest ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("result", new JSONArray(result));
//                jo.put("gameId", gameId);
//
//                _service.sendMatchResultRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_sendMatchResultRequest, returnData);
//                        LOG("2 SendMatchResultRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void logoutRequest(final String callbackReceiver, final String callbackMethod) {
        LOG("1  LogoutRequest ");
//        if (_service != null) {
//            try {
//                LOG("2 LogoutRequest ");
//                _service.logoutRequest(new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_logoutRequest, returnData);
//                        LOG("3 LogoutRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void sendMatchDataRequest(final String callbackReceiver, final String callbackMethod, String matchId, String sendData) {
//        if (_service != null) {
//            try {
//                LOG("1  SendMatchData ");
//                JSONObject jo = new JSONObject();
//                jo.put("matchId", matchId);
//                jo.put("sendData", sendData);
//                _sendMatchDataRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void sendMatchDataRequest(final String callbackReceiver, final String callbackMethod, String matchId, String sendData,String dataId) {
//        if (_service != null) {
//            try {
//                LOG("1  SendMatchData ");
//                JSONObject jo = new JSONObject();
//                jo.put("matchId", matchId);
//                jo.put("sendData", sendData);
//                jo.put("dataId", dataId);
//                _sendMatchDataRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    private void _sendMatchDataRequest(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception{
//        _service.sendMatchDataRequest(params, new SendDataCallback() {
//            public void onReceive(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_sendMatchDataRequest, returnData);
//                LOG("2 SendMatchData " + returnData.toString());
//            }
//        });
    }

    public void getGameItems(final String callbackReceiver, final String callbackMethod, String gameId) {
        LOG("1  GetGameItems ");
//        if (_service != null) {
//            try {
//
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//
//                _getGameItems(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void getGameItems(final String callbackReceiver, final String callbackMethod, String gameId, String itemId, int size, int offset) {
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("itemId", itemId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _getGameItems(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    private void _getGameItems(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getGameItems(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getGameItems, returnData);
//                LOG("2 GetGameItems " + returnData.toString());
//            }
//        });
    }

    public void getInAppPurchasePack(final String callbackReceiver, final String callbackMethod, String gameId) {
        LOG("1  getInAppPurchasePack ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                _getInAppPurchasePack(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void getInAppPurchasePack(final String callbackReceiver, final String callbackMethod, String gameId, String itemId) {
        LOG("1  getInAppPurchasePack ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("itemId", itemId);
//                _getInAppPurchasePack(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void getInAppPurchasePack(final String callbackReceiver, final String callbackMethod, String gameId, String itemId, String packId) {
        LOG("1  getInAppPurchasePack ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("itemId", itemId);
//                jo.put("packId", packId);
//                _getInAppPurchasePack(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void getInAppPurchasePack(final String callbackReceiver, final String callbackMethod, String gameId, String itemId, String packId, int size, int offset, String nameFilter) {
        LOG("1  getInAppPurchasePack ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("itemId", itemId);
//                jo.put("packId", packId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                jo.put("nameFilter", nameFilter);
//                _getInAppPurchasePack(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    private void _getInAppPurchasePack(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getInAppPurchasePack(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getInAppPurchasePack, returnData);
//                LOG("2 getInAppPurchasePack " + returnData.toString());
//            }
//        });
    }

    public void buyInAppPurchasePackRequest(final String callbackReceiver, final String callbackMethod, String packId) {
        LOG("1  BuyInAppPurchasePackRequest ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("packId", packId);
//
//                _buyInAppPurchasePackRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void buyInAppPurchasePackRequest(final String callbackReceiver, final String callbackMethod, String packId, String voucherHash) {
        LOG("1  BuyInAppPurchasePackRequest ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("packId", packId);
//                jo.put("voucherHash", voucherHash);
//
//                _buyInAppPurchasePackRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void _buyInAppPurchasePackRequest(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.buyInAppPurchasePackRequest(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_buyInAppPurchasePackRequest, returnData);
//                LOG("2 BuyInAppPurchasePackRequest " + returnData.toString());
//            }
//        });
    }

    public void getUserItems(final String callbackReciever, final String callbackMethod, String gameId) {
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                _getUserItems(callbackReciever, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void getUserItems(final String callbackReciever, final String callbackMethod, String gameId, String itemId, int size, int offset) {
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("itemId", itemId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _getUserItems(callbackReciever, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    private void _getUserItems(final String callbackReciever, final String callbackMethod, JSONObject jo) {
        LOG("1  GetUserItems ");
//        if (_service != null) {
//            try {
//                _service.getUserItems(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReciever, callbackMethod, Callback_getUserItems, returnData);
//                        LOG("2 GetUserItems " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void consumeItemRequest(final String callbackReciever, final String callbackMethod, String itemId) {
        LOG("1  ConsumeItem ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("itemId", itemId);
//                jo.put("count", 1);
//
//                _service.consumeItemRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReciever, callbackMethod, Callback_consumeItemRequest, returnData);
//                        LOG("2 ConsumeItemRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void consumeItemRequest(final String callbackReciever, final String callbackMethod, String itemId, int count) {
        LOG("1  ConsumeItem ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("itemId", itemId);
//                jo.put("count", count);
//
//                _service.consumeItemRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReciever, callbackMethod, Callback_consumeItemRequest, returnData);
//                        LOG("2 ConsumeItemRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }

    public void sendScoreRequest(final String callbackReceiver, final String callbackMethod, String gameId, int score) {
        LOG("1  SubmitScore ");
//        if (_service != null) {
//            try {
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("score", score);
//
//                _service.sendScoreRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_sendScoreRequest, returnData);
//                        LOG("2 sendScoreRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
//            }
//        }
    }


    public void getTableData(final String callbackReceiver, final String callbackMethod, String leagueId) {
        try {
            JSONObject jo = new JSONObject();
            jo.put("leagueId", leagueId);
            _getTableData(callbackReceiver, callbackMethod, jo);
        } catch (Exception e) {
            LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
        }
    }

    public void getTableData(final String callbackReceiver, final String callbackMethod, String leagueId, int rangeType) {
        try {

            JSONObject jo = new JSONObject();
            jo.put("leagueId", leagueId);
            jo.put("rangeType", rangeType);
            _getTableData(callbackReceiver, callbackMethod, jo);
        } catch (Exception e) {
            LOG("JSONException NozhacoWraper " + e.getMessage() + " getStackTrace " + e.getStackTrace().toString());
        }
    }

    private void _getTableData(final String callbackReceiver, final String callbackMethod, JSONObject params) {
        LOG("1  _GetLeaderboard " + params.toString());
//        if (_service != null) {
//            try {
//                _service.getTableData(params, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getTableData, returnData);
//                        LOG("2 _GetLeaderboard " + returnData.toString());
//                    }
//                });
//            } catch (ServiceException e) {
//                e.printStackTrace();
//            }
//        }
    }

    public void changePasswordRequest(final String callbackReceiver, final String callbackMethod, String oldPass, String newPass, String confirmPass) {
        LOG("1  ChangePassword ");
//        if (_service != null) {
//            try {
//                LOG("2  ChangePassword ");
//                JSONObject jo = new JSONObject();
//                jo.put("oldPass", oldPass);
//                jo.put("newPass", newPass);
//                jo.put("confirmPass", confirmPass);
//                _service.changePasswordRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_changePasswordRequest, returnData);
//                        LOG("3 ChangePassword " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 ServiceException getMessage " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void increaseCreditWithVoucherRequest(final String callbackReceiver, final String callbackMethod, String voucherHash) {
        LOG("1  increaseCreditWithVoucherRequest ");
//        if (_service != null) {
//            try {
//                LOG("2  IncreaseCreditWithVoucher ");
//                JSONObject jo = new JSONObject();
//                jo.put("voucherHash", voucherHash);
//                _service.increaseCreditWithVoucherRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_increaseCreditWithVoucherRequest, returnData);
//                        LOG("3 IncreaseCreditWithVoucher " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 Exception IncreaseCreditWithVoucher " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getCredit(final String callbackReceiver, final String callbackMethod) {
        LOG("1  GetCredit ");
//        if (_service != null) {
//            try {
//                LOG("2  GetCredit ");
//                _service.getCredit(new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getCredit, returnData);
//                        LOG("3 GetCredit " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 Exception GetCredit " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getLeagueAwards(final String callbackReceiver, final String callbackMethod, String leagueId) {
        LOG("1  GetLeagueAwards ");
//        if (_service != null) {
//            try {
//                LOG("2  GetLeagueAwards ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                _service.getLeagueAwards(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getLeagueAwards, returnData);
//                        LOG("3 GetLeagueAwards " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 GetLeagueAwards Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void searchUserRequest(final String callbackReceiver, final String callbackMethod, String name) {
        LOG("1  SearchUser ");
//        if (_service != null) {
//            try {
//                LOG("2  SearchUser ");
//                JSONObject jo = new JSONObject();
//                jo.put("name", name);
//                _searchUserRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 SearchUser Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void searchUserRequest(final String callbackReceiver, final String callbackMethod, String name, int size, int offset) {
        LOG("1  SearchUser ");
//        if (_service != null) {
//            try {
//                LOG("2  SearchUser ");
//                JSONObject jo = new JSONObject();
//                jo.put("name", name);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _searchUserRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 SearchUser Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }

    }

    private void _searchUserRequest(final String callbackReceiver, final String callbackMethod, JSONObject jsonObject) throws Exception {
//        _service.searchUserRequest(jsonObject, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_searchUserRequest, returnData);
//                LOG("3 _SearchUser " + returnData.toString());
//            }
//        });
    }

    public void forgetPasswordRequest(final String callbackReceiver, final String callbackMethod, String cellphoneNumber) {
        LOG("1  ForgetPassword ");
//        if (_service != null) {
//            try {
//                LOG("2  ForgetPassword ");
//                JSONObject jo = new JSONObject();
//                jo.put("cellphoneNumber", cellphoneNumber);
//                _service.forgetPasswordRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_forgetPasswordRequest, returnData);
//                        LOG("3 ForgetPassword " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 ForgetPassword Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getLeagueMembers(final String callbackReceiver, final String callbackMethod, String leagueId, int size, int offset, int userState, String name) {
        LOG("1  getLeagueMembers ");
//        if (_service != null) {
//            try {
//
//                LOG("2  GetLeagueMembers ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                jo.put("userState", userState);
//                jo.put("name", name);
//                _getLeagueMembers(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetLeagueMembers Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getLeagueMembers(final String callbackReceiver, final String callbackMethod, String leagueId, int size, int offset, int userState) {
        LOG("1  getLeagueMembers ");
//        if (_service != null) {
//            try {
//
//                LOG("2  GetLeagueMembers ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                jo.put("userState", userState);
//                _getLeagueMembers(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetLeagueMembers Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getLeagueMembers(final String callbackReceiver, final String callbackMethod, String leagueId) {
        LOG("1  GetLeagueMembers ");
//        if (_service != null) {
//            try {
//
//                LOG("2  GetLeagueMembers ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                _getLeagueMembers(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetLeagueMembers Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getLeagueMembers(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getLeagueMembers(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getLeagueMembers, returnData);
//                LOG("3 GetLeagueMembers " + returnData.toString());
//            }
//        });
    }

    public void matchIdRequest(final String callbackReceiver, final String callbackMethod, String gameId, String leagueId) {
        LOG("1  MatchIdRequest ");
//        if (_service != null) {
//            try {
//                LOG("2  MatchIdRequest ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("leagueId", leagueId);
//                _service.matchIdRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_matchIdRequest, returnData);
//                        LOG("3 MatchIdRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 MatchIdRequest Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void changeVisibilityRequest(final String callbackReceiver, final String callbackMethod) {
        LOG("1  change visibility ");
//        if (_service != null) {
//            try {
//                LOG("2  ChangeVisibility ");
//                _service.changeVisibilityRequest(new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_changeVisibilityRequest, returnData);
//                        LOG("3 ChangeVisibility " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 ChangeVisibility Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getOnlineInfo(final String callbackReceiver, final String callbackMethod, String gameId) {
        LOG("1  GetOnlineInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  GetOnlineInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                _service.getOnlineInfo(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getOnlineInfo, returnData);
//                        LOG("3 GetOnlineInfo " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 GetOnlineInfo Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getTopGamesInfo(final String callbackReceiver, final String callbackMethod, int type) {
        LOG("1  GetTopGamesInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  GetTopGamesInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", type);
//                _getTopGamesInfo(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetTopGamesInfo Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void GetTopGamesInfo(final String callbackReceiver, final String callbackMethod, int type, int size, int offset) {
        LOG("1  GetTopGamesInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  GetTopGamesInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", type);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _getTopGamesInfo(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetTopGamesInfo Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getTopGamesInfo(final String callbackReceiver, final String callbackMethod, JSONObject params) {
        LOG("1  GetTopGamesInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  GetTopGamesInfo ");
//                _service.getTopGamesInfo(params, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getTopGamesInfo, returnData);
//                        LOG("3 GetTopGamesInfo " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 GetTopGamesInfo Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getNews(final String callbackReceiver, String callbackMethod, int businessId) {
        LOG("1  GetNews ");
//        if (_service != null) {
//            try {
//                LOG("2  GetNews ");
//                JSONObject jo = new JSONObject();
//                jo.put("businessId", businessId);
//                _getNews(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetNews Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getNews(final String callbackReceiver, String callbackMethod, int businessId, int size, int offset) {
        LOG("1  GetNews ");
//        if (_service != null) {
//            try {
//                LOG("2  GetNews ");
//                JSONObject jo = new JSONObject();
//                jo.put("businessId", businessId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _getNews(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetNews Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getNews(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getNews(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getNews, returnData);
//                LOG("3 GetNews " + returnData.toString());
//            }
//        });
    }

    public void followRequest(final String callbackReceiver, final String callbackMethod, int businessId, int postId) {
        LOG("1  Follow ");
//        if (_service != null) {
//            try {
//                LOG("2  Follow ");
//                JSONObject jo = new JSONObject();
//                jo.put("businessId", businessId);
//                jo.put("postId", postId);
//                _followRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 Follow Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void followRequest(final String callbackReceiver, final String callbackMethod, int businessId, int postId, Boolean state) {
        LOG("1  Follow ");
//        if (_service != null) {
//            try {
//                LOG("2  Follow ");
//                JSONObject jo = new JSONObject();
//                jo.put("businessId", businessId);
//                jo.put("postId", postId);
//                jo.put("state", state);
//                _followRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 Follow Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _followRequest(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.followRequest(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_followRequest, returnData);
//                LOG("3 _Follow " + returnData.toString());
//            }
//        });
    }

    public void getTopPlayers(final String callbackReceiver, final String callbackMethod, String gameId, String leagueId) {
        LOG("1  GetTopPlayers ");
//        if (_service != null) {
//            try {
//                LOG("2  GetTopPlayers ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("leagueId", leagueId);
//                _getTopPlayers(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetTopPlayers Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getTopPlayers(final String callbackReceiver, final String callbackMethod, String gameId, String leagueId, int size, int offset) {
        LOG("1  GetTopPlayers ");
//        if (_service != null) {
//            try {
//                LOG("2  GetTopPlayers ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("leagueId", leagueId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _getTopPlayers(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetTopPlayers Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getTopPlayers(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getTopPlayers(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getTopPlayers, returnData);
//                LOG("3 _GetTopPlayers " + returnData.toString());
//            }
//        });
    }

    public void suggestionRequest(final String callbackReceiver, final String callbackMethod, String gameId, String suggestion, int type) {
        LOG("1  Suggestion ");
//        if (_service != null) {
//            try {
//                LOG("2  Suggestion ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("suggestion", suggestion);
//                jo.put("type", type);
//                _suggestionRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 Suggestion Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void suggestionRequest(final String callbackReceiver, final String callbackMethod, String gameId, String suggestion, int type, String name, String email, String metaData) {
        LOG("1  Suggestion ");
//        if (_service != null) {
//            try {
//                LOG("2  Suggestion ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("suggestion", suggestion);
//                jo.put("type", type);
//                jo.put("name", name);
//                jo.put("email", email);
//                jo.put("metaData", metaData);
//                _suggestionRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 Suggestion Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _suggestionRequest(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.suggestionRequest(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_suggestionRequest, returnData);
//                LOG("3 Suggestion " + returnData.toString());
//            }
//        });
    }

    public void quickMatchRequest(final String callbackReceiver, final String callbackMethod,
                                  final String callbackCancelMethod, final String callbackAcceptMethod,
                                  String gameId, String leagueId) {
        LOG("1  QuickMatch ");
//        if (_service != null) {
//            try {
//                LOG("2  QuickMatch ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("leagueId", leagueId);
//                _service.quickMatchRequest(jo, new QuickMatchRequestCallback() {
//                    @Override
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_quickMatchRequest, returnData);
//                        LOG("3 QuickMatch " + returnData.toString());
//                    }
//
//                    @Override
//                    public void onCancel(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackCancelMethod, OnCancel_QuickMatch, returnData);
//                        LOG("3 QuickMatch " + returnData.toString());
//                    }
//
//                    @Override
//                    public void onAccept(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackAcceptMethod, OnAccept_QuickMatch, returnData);
//                        LOG("3 QuickMatch " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 QuickMatch Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void cancelQuickMatchRequest(final String callbackReceiver, final String callbackMethod, String leagueId) {
        LOG("1  CancelQuickMatch ");
//        if (_service != null) {
//            try {
//
//                LOG("2  CancelQuickMatch ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                _service.cancelQuickMatchRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_cancelQuickMatchRequest, returnData);
//                        LOG("3 CancelQuickMatch " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 CancelQuickMatch Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getCustomPost(final String callbackReceiver, final String callbackMethod, String businessId, String metadata) {
        LOG("1  GetCustomPost ");
//        if (_service != null) {
//            try {
//                LOG("2  GetCustomPost ");
//                JSONObject jo = new JSONObject();
//                jo.put("businessId", businessId);
//                jo.put("metadata", metadata);
//                _service.getCustomPost(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getCustomPost, returnData);
//                        LOG("3 GetCustomPost " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 GetCustomPost Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void sendMatchLeaveRequest(final String callbackReceiver, final String callbackMethod, String matchId) {
        LOG("1  LeavedMatch ");
//        if (_service != null) {
//            try {
//                LOG("2  LeavedMatch ");
//                JSONObject jo = new JSONObject();
//                jo.put("matchId", matchId);
//                _service.sendMatchLeaveRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_sendMatchLeaveRequest, returnData);
//                        LOG("3 LeavedMatch " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 LeavedMatch Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void sendMatchCancelRequest(final String callbackReceiver, final String callbackMethod, String matchId) {
        LOG("1  CancelMatch ");
//        if (_service != null) {
//            try {
//                LOG("2  CancelMatch ");
//                JSONObject jo = new JSONObject();
//                jo.put("matchId", matchId);
//                _service.sendMatchCancelRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_sendMatchCancelRequest, returnData);
//                        LOG("3 CancelMatch " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 CancelMatch Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getRelatedLeaguesInfo(final String callbackReceiver, final String callbackMethod, String leagueId, int type) {
        LOG("1  GetRelatedLeaguesInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  GetRelatedLeaguesInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                jo.put("type", type);
//                _getRelatedLeaguesInfo(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetRelatedLeaguesInfo Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getRelatedLeaguesInfo(final String callbackReceiver, final String callbackMethod, String leagueId, int type, int size, int offset) {
        LOG("1  GetRelatedLeaguesInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  GetRelatedLeaguesInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                jo.put("type", type);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _getRelatedLeaguesInfo(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetRelatedLeaguesInfo Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getRelatedLeaguesInfo(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getRelatedLeaguesInfo(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getRelatedLeaguesInfo, returnData);
//                LOG("3 _GetRelatedLeaguesInfo " + returnData.toString());
//            }
//        });
    }

    public void getCreditPackList(final String callbackReceiver, final String callbackMethod) {
        LOG("1  GetCreditPackList ");
//        if (_service != null) {
//            try {
//                LOG("2  GetCreditPackList ");
//                JSONObject jo = new JSONObject();
//                _getCreditPackList(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetCreditPackList Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getCreditPackList(final String callbackReceiver, final String callbackMethod, int fromAmount) {
        LOG("1  GetCreditPackList ");
//        if (_service != null) {
//            try {
//                LOG("2  GetCreditPackList ");
//                JSONObject jo = new JSONObject();
//                jo.put("fromAmount", fromAmount);
//                _getCreditPackList(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetCreditPackList Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getCreditPackList(final String callbackReceiver, final String callbackMethod, String name) {
        LOG("1  GetCreditPackList ");
//        if (_service != null) {
//            try {
//                LOG("2  GetCreditPackList ");
//                JSONObject jo = new JSONObject();
//                jo.put("name", name);
//                _getCreditPackList(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetCreditPackList Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getCreditPackList(final String callbackReceiver, final String callbackMethod, int size, int offset) {
        LOG("1  GetCreditPackList ");
//        if (_service != null) {
//            try {
//                LOG("2  GetCreditPackList ");
//                JSONObject jo = new JSONObject();
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _getCreditPackList(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetCreditPackList Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getCreditPackList(final String callbackReceiver, final String callbackMethod, String name, int size, int offset) {
        LOG("1  GetCreditPackList ");
//        if (_service != null) {
//            try {
//                LOG("2  GetCreditPackList ");
//                JSONObject jo = new JSONObject();
//                jo.put("size", size);
//                jo.put("offset", offset);
//                jo.put("name", name);
//                _getCreditPackList(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 GetCreditPackList Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getCreditPackList(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getCreditPackList(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getCreditPackList, returnData);
//                LOG("3 _GetCreditPackList " + returnData.toString());
//            }
//        });
    }

    public void subscribeLeagueRequest(final String callbackReceiver, final String callbackMethod, String enrollUrl, String leagueId) {
        LOG("1  SubscribeLeague ");
//        if (_service != null) {
//            try {
//                LOG("2  SubscribeLeague ");
//                JSONObject jo = new JSONObject();
//                jo.put("enrollUrl", enrollUrl);
//                jo.put("leagueId", leagueId);
//                _subscribeLeagueRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 SubscribeLeague Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void subscribeLeagueRequest(final String callbackReceiver, final String callbackMethod, String enrollUrl, String leagueId, String voucherHash) {
        LOG("1  SubscribeLeague ");
//        if (_service != null) {
//            try {
//                LOG("2  SubscribeLeague ");
//                JSONObject jo = new JSONObject();
//                jo.put("enrollUrl", enrollUrl);
//                jo.put("leagueId", leagueId);
//                jo.put("voucherHash", voucherHash);
//                _subscribeLeagueRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 SubscribeLeague Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _subscribeLeagueRequest(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.subscribeLeagueRequest(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_subscribeLeagueRequest, returnData);
//                LOG("3 _SubscribeLeague " + returnData.toString());
//            }
//        });
    }

    public void ShareByGameIdAndCellphone(final String callbackReceiver, final String callbackMethod, String gameId, String cellphoneNumbers) {
        LOG("1  Share ");
//        if (_service != null) {
//            try {
//                LOG("2  Share ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("cellphoneNumbers", new JSONArray(cellphoneNumbers));
//                _shareRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 Share Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void ShareByGameIdAndUserIds(final String callbackReceiver, final String callbackMethod, String gameId, String userIds) {
        LOG("1  Share ");
//        if (_service != null) {
//            try {
//                LOG("2  Share ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("userIds", new JSONArray(userIds));
//                _shareRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 Share Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void ShareByLeagueIdAndCellphone(final String callbackReceiver, final String callbackMethod, String leagueId, String cellphoneNumbers) {
        LOG("1  Share ");
//        if (_service != null) {
//            try {
//                LOG("2  Share ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                jo.put("cellphoneNumbers", new JSONArray(cellphoneNumbers));
//                _shareRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 Share Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void ShareByLeagueIdAndUserIds(final String callbackReceiver, final String callbackMethod, String leagueId, String userIds) {
        LOG("1  Share ");
//        if (_service != null) {
//            try {
//                LOG("2  Share ");
//                JSONObject jo = new JSONObject();
//                jo.put("leagueId", leagueId);
//                jo.put("userIds", new JSONArray(userIds));
//                _shareRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 Share Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _shareRequest(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.shareRequest(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_shareRequest, returnData);
//                LOG("3 _Share " + returnData.toString());
//            }
//        });
    }

    public void getEnrollAccess(final String callbackReceiver, final String callbackMethod, String leagueId) {
        LOG("1  GetEnrollAccess ");
//        if (_service != null) {
//            try {
//                LOG("2  GetEnrollAccess ");
//                JSONObject jo = new JSONObject();
//                jo.put("businessId", leagueId);
//                _service.getEnrollAccess(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getEnrollAccess, returnData);
//                        LOG("3 GetEnrollAccess " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 GetEnrollAccess Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getFileInfo(final String callbackReceiver, final String callbackMethod, String gameId) {
        LOG("1  GetFileInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  GetFileInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                _service.getFileInfo(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_getFileInfo, returnData);
//                        LOG("3 GetFileInfo " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 GetFileInfo Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public String generateImageSrc(String imageId, int imageWidth, int imageHeight) {
        LOG("1  GenerateImageSrc ");
//        if (_service != null) {
//            try {
//                LOG("2  GenerateImageSrc ");
//                return _service.generateImageSrc(imageId, imageWidth, imageHeight);
//            } catch (Exception e) {
//                LOG("4 GenerateImageSrc Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
        return "";
    }

    public void editProfileImageRequest(final String callbackReceiver, final String callbackMethod, String base64Image) {
        LOG("1 EditProfileImage ");
//        if (_service != null) {
//            try {
//                LOG("2  EditProfileImage ");
//                JSONObject jo = new JSONObject();
//                jo.put("base64Image", base64Image);
//                _service.editProfileImageRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_editProfileImageRequest, returnData);
//                        LOG("3 EditProfileImage " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4 EditProfileImage Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void editProfileRequest(final String callbackReceiver, final String callbackMethod, String nickName) {
        LOG("1  EditProfile ");
//        if (_service != null) {
//            try {
//                LOG("2  EditProfile ");
//                JSONObject jo = new JSONObject();
//                jo.put("nickName", nickName);
//                _editProfileRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 EditProfile Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void editProfileRequest(final String callbackReceiver, final String callbackMethod, String nickName, String firstName, String lastName, String email, Boolean genderMale) {
        LOG("1  EditProfile ");
//        if (_service != null) {
//            try {
//                LOG("2  EditProfile ");
//                JSONObject jo = new JSONObject();
//                jo.put("nickName", nickName);
//                jo.put("firstName", firstName);
//                jo.put("lastName", lastName);
//                jo.put("email", email);
//                if (genderMale)
//                    jo.put("gender", "MAN_GENDER");
//                else
//                    jo.put("gender", "WOMAN_GENDER");
//                _editProfileRequest(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4 EditProfile Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _editProfileRequest(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.editProfileRequest(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_editProfileRequest, returnData);
//                LOG("3 EditProfileImage " + returnData.toString());
//            }
//        });
    }

    public String generateIncreaseCreditUrl(String creditPackId, String leagueId, String packId) {
        LOG("1  generateIncreaseCreditUrl ");
//        if (_service != null) {
//            try {
//                LOG("2  generateIncreaseCreditUrl ");
//                JSONObject jo = new JSONObject();
//                jo.put("creditPackId", creditPackId);
//
//                if (leagueId != null) {
//                    jo.put("leagueId", leagueId);
//                }
//
//                if (packId != null) {
//                    jo.put("packId", packId);
//                }
//
//                return _generateIncreaseCreditUrl(jo);
//            } catch (Exception e) {
//                LOG("4 generateIncreaseCreditUrl Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
        return "";
    }

    public String generateIncreaseCreditUrl(String creditPackId) {
        LOG("1  generateIncreaseCreditUrl ");
//        if (_service != null) {
//            try {
//                LOG("2  generateIncreaseCreditUrl ");
//                JSONObject jo = new JSONObject();
//                jo.put("creditPackId", creditPackId);
//                _generateIncreaseCreditUrl(jo);
//            } catch (Exception e) {
//                LOG("4 generateIncreaseCreditUrl Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
        return "";
    }

    private String _generateIncreaseCreditUrl(JSONObject params) {
        LOG("1  generateIncreaseCreditUrl ");
//        if (_service != null) {
//            try {
//                LOG("2  generateIncreaseCreditUrl ");
//                return _service.generateIncreaseCreditUrl(params);
//            } catch (Exception e) {
//                LOG("4 generateIncreaseCreditUrl Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
        return "";
    }

//    ChatServiceWrapper chatService;

//    public ChatServiceWrapper getChatService() {
//        return chatService;
//    }

    public void verifyRequest(final String callbackReceiver, final String callbackMethod, String cellphoneNumber, String code, String newCode) {
        LOG("1  verifyRequest ");
//        if (_service != null) {
//            try {
//                LOG("2  verifyRequest ");
//                JSONObject jo = new JSONObject();
//                jo.put("cellphoneNumber", cellphoneNumber);
//                jo.put("code", code);
//                jo.put("newCode", newCode);
//                _service.verifyRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_verifyRequest, returnData);
//                        LOG("3 verifyRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void cancelMatchRequest(final String callbackReceiver, final String callbackMethod, String requestId) {
        LOG("1  cancelMatchRequest ");
//        if (_service != null) {
//            try {
//                LOG("2  cancelMatchRequest ");
//                JSONObject jo = new JSONObject();
//                jo.put("requestId", requestId);
//                _service.cancelMatchRequest(jo, new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_cancelMatchRequest, returnData);
//                        LOG("3 cancelMatchRequest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getLeaguesInfo(final String callbackReceiver, final String callbackMethod, String gameId) {
        LOG("1  cancelMatchRequest ");
//        if (_service != null) {
//            try {
//                LOG("2  cancelMatchRequest ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                _getLeaguesInfo(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getLeaguesInfo(final String callbackReceiver, final String callbackMethod, String gameId, String leagueId) {
        LOG("1  getLeaguesInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  getLeaguesInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//                jo.put("leagueId", leagueId);
//                _getLeaguesInfo(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getLeaguesInfo(final String callbackReceiver, final String callbackMethod, String gameId, String name, int prize, int status, int financialType, int userState, boolean showDefault, boolean newest) {
        LOG("1  getLeaguesInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  getLeaguesInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//
//                if (name != null) {
//                    jo.put("name", name);
//                }
//
//                jo.put("prize", prize);
//                jo.put("status", status);
//                jo.put("financialType", financialType);
//                jo.put("userState", userState);
//                jo.put("showDefault", showDefault);
//                jo.put("newest", newest);
//
//                _getLeaguesInfo(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getLeaguesInfo(final String callbackReceiver, final String callbackMethod, String gameId, String name, int prize, int[] statusList, int financialType, int userState, boolean showDefault, boolean newest) {
        LOG("1  getLeaguesInfo ");
//        if (_service != null) {
//            try {
//                LOG("2  getLeaguesInfo ");
//                JSONObject jo = new JSONObject();
//                jo.put("gameId", gameId);
//
//                if (name != null) {
//                    jo.put("name", name);
//                }
//
//                jo.put("prize", prize);
//                jo.put("statusList", new JSONArray(Arrays.asList(statusList)));
//                jo.put("financialType", financialType);
//                jo.put("userState", userState);
//                jo.put("showDefault", showDefault);
//                jo.put("newest", newest);
//
//                _getLeaguesInfo(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getLeaguesInfo(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getLeaguesInfo(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getLeaguesInfo, returnData);
//                LOG("3 _getLeaguesInfo " + returnData.toString());
//            }
//        });
    }

    public void getUserProfile(final String callbackReceiver, final String callbackMethod, String userId) {
        LOG("1  getUserProfile ");
//        if (_service != null) {
//            try {
//                LOG("2  getUserProfile ");
//                JSONObject jo = new JSONObject();
//                jo.put("userId", userId);
//                _getUserProfile(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getUserProfile(final String callbackReceiver, final String callbackMethod, String userId, Boolean refetch) {
        LOG("1  getUserProfile ");
//        if (_service != null) {
//            try {
//                LOG("2  getUserProfile ");
//                JSONObject jo = new JSONObject();
//                jo.put("userId", userId);
//                jo.put("refetch", refetch);
//                _getUserProfile(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getUserProfile(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getUserProfile(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getUserProfile, returnData);
//                LOG("3 getUserProfile " + returnData.toString());
//            }
//        });
    }

    public void getUserGameCenterItem(final String callbackReceiver, final String callbackMethod, String itemId) {
        LOG("1  getUserGameCenterItem ");
//        if (_service != null) {
//            try {
//                LOG("2  getUserGameCenterItem ");
//                JSONObject jo = new JSONObject();
//                jo.put("itemId", itemId);
//                _getUserGameCenterItem(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    public void getUserGameCenterItem(final String callbackReceiver, final String callbackMethod, String itemId, int size, int offset) {
        LOG("1  getUserGameCenterItem ");
//        if (_service != null) {
//            try {
//                LOG("2  getUserGameCenterItem ");
//                JSONObject jo = new JSONObject();
//                jo.put("itemId", itemId);
//                jo.put("size", size);
//                jo.put("offset", offset);
//                _getUserGameCenterItem(callbackReceiver, callbackMethod, jo);
//            } catch (Exception e) {
//                LOG("4   Exception " + e.getMessage().toString() + " getStackTrace " + e.getStackTrace());
//                e.printStackTrace();
//            }
//        }
    }

    private void _getUserGameCenterItem(final String callbackReceiver, final String callbackMethod, JSONObject params) throws Exception {
//        _service.getUserGameCenterItem(params, new RequestCallback() {
//            public void onResult(JSONObject returnData) {
//                SendResultToUnity(callbackReceiver, callbackMethod, Callback_getUserGameCenterItem, returnData);
//                LOG("3 getUserGameCenterItem " + returnData.toString());
//            }
//        });
    }

    public void registerGuestRequest(final String callbackReceiver, final String callbackMethod) {
//        if (_service != null) {
//            try {
//                _service.registerGuestRequest(new RequestCallback() {
//                    public void onResult(JSONObject returnData) {
//                        SendResultToUnity(callbackReceiver, callbackMethod, Callback_registerGuestReuest, returnData);
//                        LOG("2 _registerGuestReuest " + returnData.toString());
//                    }
//                });
//            } catch (Exception e) {
//                e.printStackTrace();
//            }
//        }
    }


    public void loginUI(final String callbackReceiver, final String callbackMethod){
        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {
                    launcherInterface.loginUI(new JSONObject().toString(), new land.pod.play.ipc.RequestCallback.Stub() {
                        @Override
                        public void onResult(String params) throws RemoteException {

                            try {

                                JSONObject result = new JSONObject(params);
                                Boolean hasError = result.getBoolean("HasError");
                                JSONObject returnData = new JSONObject();
                                returnData.put("hasError", hasError);
                                returnData.put("errorMessage", result.get("ErrorMessage"));
                                returnData.put("errorCode", result.get("ErrorCode"));


                                if (!hasError && result.has("Result") && !result.isNull("Result")) {
                                    JSONObject data = result.getJSONObject("Result");
                                    returnData.put("result", data);

//                                    _service.initLogin(data, new RequestCallback() {
//                                        @Override
//                                        public void onResult(JSONObject result) {
//                                            super.onResult(result);
//                                        }
//                                    });
                                } else {
                                    returnData.put("result", null);
                                }
                                SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_loginUI, returnData);
                            }catch (Exception e){
                                e.printStackTrace();
                            }

                        }
                    });
                } catch (RemoteException e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void signupUI(final String callbackReceiver, final String callbackMethod){
        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {
                    launcherInterface.signupUI(new JSONObject().toString(), new land.pod.play.ipc.RequestCallback.Stub() {
                        @Override
                        public void onResult(String params) throws RemoteException {

                            try {

                                JSONObject result = new JSONObject(params);
                                Boolean hasError = result.getBoolean("HasError");
                                JSONObject returnData = new JSONObject();
                                returnData.put("hasError", hasError);
                                returnData.put("errorMessage", result.get("ErrorMessage"));
                                returnData.put("errorCode", result.get("ErrorCode"));


                                if (!hasError && result.has("Result") && !result.isNull("Result")) {
                                    JSONObject data = result.getJSONObject("Result");
                                    returnData.put("result", data);

//                                    _service.initLogin(data, new RequestCallback() {
//                                        @Override
//                                        public void onResult(JSONObject result) {
//                                            super.onResult(result);
//                                        }
//                                    });
                                } else {
                                    returnData.put("result", null);
                                }
                                SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_signupUI, returnData);
                            }catch (Exception e){
                                e.printStackTrace();
                            }

                        }
                    });
                } catch (RemoteException e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void leaderBoardUI(final String gameId,final String leagueId){
        Log.d(TAG, "leaderBoardUI() called with: gameId = [" + gameId + "], leagueId = [" + leagueId + "]");
        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {

                    JSONObject reqData = new JSONObject();

                    if (gameId != null) {
                        reqData.put("gameId", gameId);
                    }

                    if (leagueId != null) {
                        reqData.put("leagueId", leagueId);
                    }

                    launcherInterface.leaderBoardUI(reqData.toString());
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void gameUI(final String gameId){

        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {

                    JSONObject reqData = new JSONObject();

                    if (gameId != null) {
                        reqData.put("gameId", gameId);
                    }

                    launcherInterface.gameUI(reqData.toString());
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void leagueUI(final String gameId,final String leagueId){

        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {

                    JSONObject reqData = new JSONObject();

                    if (gameId != null) {
                        reqData.put("gameId", gameId);
                    }

                    if (leagueId != null) {
                        reqData.put("leagueId", leagueId);
                    }

                    launcherInterface.leagueUI(reqData.toString());
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void matchRequestUI(final String gameId,final String leagueId){

        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {

                    JSONObject reqData = new JSONObject();

                    if (gameId != null) {
                        reqData.put("gameId", gameId);
                    }

                    if (leagueId != null) {
                        reqData.put("leagueId", leagueId);
                    }

                    launcherInterface.matchRequestUI(reqData.toString());
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void editProfileUI(final String callbackReceiver, final String callbackMethod){
        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {
                    launcherInterface.editProfileUI(new JSONObject().toString(), new land.pod.play.ipc.RequestCallback.Stub() {
                        @Override
                        public void onResult(String params) throws RemoteException {

                            try {

                                JSONObject result = new JSONObject(params);
                                Boolean hasError = result.getBoolean("HasError");
                                JSONObject returnData = new JSONObject();
                                returnData.put("hasError", hasError);
                                returnData.put("errorMessage", result.get("ErrorMessage"));
                                returnData.put("errorCode", result.get("ErrorCode"));
                                returnData.put("result", result.get("Result"));

                                SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_editProfileUI, returnData);
                            }catch (Exception e){
                                e.printStackTrace();
                            }

                        }
                    });
                } catch (RemoteException e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void inAppPurchaseUI(final String callbackReceiver, final String callbackMethod,final String gameId, final String itemId){
        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {

                    JSONObject reqData = new JSONObject();

                    if (gameId != null) {
                        reqData.put("gameId", gameId);
                    }

                    if (itemId != null) {
                        reqData.put("leagueId", itemId);
                    }

                    launcherInterface.inAppPurchaseUI(reqData.toString(), new land.pod.play.ipc.RequestCallback.Stub() {
                        @Override
                        public void onResult(String params) throws RemoteException {

                            try {

                                JSONObject result = new JSONObject(params);
                                Boolean hasError = result.getBoolean("HasError");
                                JSONObject returnData = new JSONObject();
                                returnData.put("hasError", hasError);
                                returnData.put("errorMessage", result.get("ErrorMessage"));
                                returnData.put("errorCode", result.get("ErrorCode"));
                                returnData.put("result", result.get("Result"));

                                SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_inAppPurchaseUI, returnData);
                            }catch (Exception e){
                                e.printStackTrace();
                            }

                        }
                    });
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void increaseCreditUI(final String callbackReceiver, final String callbackMethod){
        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {

                    JSONObject reqData = new JSONObject();

                    launcherInterface.increaseCreditUI(reqData.toString(), new land.pod.play.ipc.RequestCallback.Stub() {
                        @Override
                        public void onResult(String params) throws RemoteException {

                            try {

                                JSONObject result = new JSONObject(params);
                                Boolean hasError = result.getBoolean("HasError");
                                JSONObject returnData = new JSONObject();
                                returnData.put("hasError", hasError);
                                returnData.put("errorMessage", result.get("ErrorMessage"));
                                returnData.put("errorCode", result.get("ErrorCode"));
                                returnData.put("result", result.get("Result"));

                                SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_increaseCreditUI, returnData);
                            }catch (Exception e){
                                e.printStackTrace();
                            }

                        }
                    });
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void subscribeLeagueUI(final String callbackReceiver, final String callbackMethod,final String leagueId){
        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {

                    JSONObject reqData = new JSONObject();

                    if (leagueId != null) {
                        reqData.put("leagueId", leagueId);
                    }

                    launcherInterface.subscribeLeagueUI(reqData.toString(), new land.pod.play.ipc.RequestCallback.Stub() {
                        @Override
                        public void onResult(String params) throws RemoteException {

                            try {

                                JSONObject result = new JSONObject(params);
                                Boolean hasError = result.getBoolean("HasError");
                                JSONObject returnData = new JSONObject();
                                returnData.put("hasError", hasError);
                                returnData.put("errorMessage", result.get("ErrorMessage"));
                                returnData.put("errorCode", result.get("ErrorCode"));
                                returnData.put("result", result.get("Result"));

                                SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_subscribeLeagueUI, returnData);
                            }catch (Exception e){
                                e.printStackTrace();
                            }

                        }
                    });
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void buyPackUI(final String callbackReceiver, final String callbackMethod,final String packId){
        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                try {

                    JSONObject reqData = new JSONObject();

                    if (packId != null) {
                        reqData.put("packId", packId);
                    }

                    launcherInterface.buyPackUI(reqData.toString(), new land.pod.play.ipc.RequestCallback.Stub() {
                        @Override
                        public void onResult(String params) throws RemoteException {

                            try {

                                JSONObject result = new JSONObject(params);
                                Boolean hasError = result.getBoolean("HasError");
                                JSONObject returnData = new JSONObject();
                                returnData.put("hasError", hasError);
                                returnData.put("errorMessage", result.get("ErrorMessage"));
                                returnData.put("errorCode", result.get("ErrorCode"));
                                returnData.put("result", result.get("Result"));

                                SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_buyPackUI, returnData);
                            }catch (Exception e){
                                e.printStackTrace();
                            }

                        }
                    });
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });



    }

    public void getUserData(final String callbackReceiver, final String callbackMethod){
        Log.i(TAG, "getUserData: ");
        if (!isPackageInstalled(launcherPackageName, context.getPackageManager())) {
            JSONObject returnData = new JSONObject();
            try {
                returnData.put("hasError", true);
                returnData.put("errorMessage", "GameCenter app (" + launcherPackageName + ") NOT installed");
                returnData.put("errorCode", 2001);
                returnData.put("result", null);
            } catch (JSONException e) {
                e.printStackTrace();
            }

            SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_getUserData, returnData);
            return;
        }

        initLauncherConnection(new LauncherConnection() {
            @Override
            public void onConnect(LauncherInterface launcherInterface) {
                Log.i(TAG, "onConnect: ");
                try {

                    JSONObject reqData = new JSONObject();

                    launcherInterface.getUserData(reqData.toString(), new land.pod.play.ipc.RequestCallback.Stub() {
                        @Override
                        public void onResult(String params) throws RemoteException {
                            Log.i(TAG, "onResult: " + params);
                            try {
                                JSONObject result = new JSONObject(params);
                                Boolean hasError = result.getBoolean("HasError");
                                JSONObject returnData = new JSONObject();
                                returnData.put("hasError", hasError);

                                if (!result.has("Result") || result.get("Result") == null) {
                                    returnData.put("hasError", true);
                                    returnData.put("errorMessage", "User not logged in. Please LOGIN at game center first");
                                    returnData.put("errorCode", 2002);
                                    returnData.put("result", null);
                                } else {
                                    returnData.put("errorMessage", result.get("ErrorMessage"));
                                    returnData.put("errorCode", result.get("ErrorCode"));
                                    returnData.put("result", result.get("Result"));
                                }

                                SendResultToUnity(callbackReceiver, callbackMethod, Callback_launcher_getUserData, returnData);
                            }catch (Exception e){
                                e.printStackTrace();
                            }

                        }
                    });
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    private boolean isPackageInstalled(String packageName, PackageManager packageManager) {
        try {
            packageManager.getPackageInfo(packageName, 0);
            return true;
        } catch (PackageManager.NameNotFoundException e) {
            return false;
        }
    }


    public static void LOG(String message) {
        try {
            if (showLogs)
                Log.i("Unity", "@ " + message);

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}

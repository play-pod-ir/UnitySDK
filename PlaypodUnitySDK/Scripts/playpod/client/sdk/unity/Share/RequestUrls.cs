namespace playpod.client.sdk.unity.Share
{
    public class RequestUrls
    {
        public static UrlData Sugnup = new UrlData("/srv/user/signup", "BAZITECH");
        public static UrlData Verify = new UrlData("/srv/user/verify", "BAZITECH");
        public static UrlData Login = new UrlData("/srv/user/login", "BAZITECH");
        public static UrlData RegisterGuest = new UrlData("/srv/user/registerGuest", "BAZITECH");
        public static UrlData Logout = new UrlData("/srv/user/logout", "BAZITECH");
        public static UrlData Ping = new UrlData("/srv/user/ping", "BAZITECH");
        public static UrlData OnlineUser = new UrlData("/srv/user/getOnlineUsers", "BAZITECH");
        public static UrlData CompleteProfile = new UrlData("/srv/user/completeProfile", "BAZITECH");
        public static UrlData VerifyAndCompleteProfile = new UrlData("/srv/user/verifyandcompleteProfile", "BAZITECH");
        public static UrlData ForgetPassword = new UrlData("/srv/user/forgotpass", "BAZITECH");
        public static UrlData EditProfileImage = new UrlData("/srv/user/editprofileimage", "BAZITECH");
        public static UrlData GameInfo = new UrlData("/srv/game/get", "BAZITECH");
        public static UrlData GetLeague = new UrlData("/srv/league/get", "BAZITECH");
        public static UrlData GetTopLeague = new UrlData("/srv/league/top", "BAZITECH");
        public static UrlData GetLatestLeague = new UrlData("/srv/league/latest", "BAZITECH");
        //    public static UrlData TABLE = new UrlData("/srv/league/getTable","BAZITECH");
        public static UrlData Table = new UrlData("/srv/league/table", "BAZITECH");
        //    public static UrlData REQUEST_MATCH = new UrlData("/srv/match/request","BAZITECH",true);
        public static UrlData RequestMatch = new UrlData("/srv/match/matchrequest", "BAZITECH", true);
        public static UrlData MatchReady = new UrlData("/srv/match/ready", "BAZITECH", true);
        public static UrlData MatchRequestResponse = new UrlData("/srv/match/requestresult", "BAZITECH", true);
        public static UrlData CancelMatchRequest = new UrlData("/srv/match/cancelrequest", "BAZITECH");
        public static UrlData OfflineMatchRequest = new UrlData("/srv/match/offlinerequest", "BAZITECH");
        public static UrlData LeagueMembers = new UrlData("/srv/league/members", "BAZITECH");
        public static UrlData GetLeagueEnrollAccess = new UrlData("/srv/league/enrollaccess", "BAZITECH");
        public static UrlData LeagueRate = new UrlData("/srv/league/rate", "BAZITECH");
        public static UrlData GameRate = new UrlData("/srv/game/rate", "BAZITECH");
        public static UrlData RequestMatchId = new UrlData("/srv/match/add", "BAZITECH", true);
        public static UrlData RequestStreamMatchId = new UrlData("/srv/stream/addmatch", "BAZITECH");
        public static UrlData MatchResult = new UrlData("/srv/match/result", "BAZITECH", true);
        public static UrlData MatchValidate = new UrlData("/srv/match/validate", "BAZITECH", true);
        public static UrlData MatchCancel = new UrlData("/srv/match/cancel", "BAZITECH", true);
        public static UrlData Suggestion = new UrlData("/srv/user/bugReport", "BAZITECH");
        public static UrlData TopScore = new UrlData("/srv/user/getTopScore", "BAZITECH");
        public static UrlData EditScore = new UrlData("/srv/user/editScore", "BAZITECH");
        public static UrlData Reconnect = new UrlData("/srv/match/reconnect", "BAZITECH");
        public static UrlData AsyncRegister = new UrlData("/srv/user/asyncRegister", "BAZITECH");
        public static UrlData ChatAsyncRegister = new UrlData("/srv/chat/register", "BAZITECH");
        public static UrlData ActivePeer = new UrlData("/srv/user/activatePeer", "BAZITECH");
        public static UrlData ChatActivePeer = new UrlData("/srv/chat/activate", "BAZITECH");
        public static UrlData SetChatId = new UrlData("/srv/chat/setChatId", "BAZITECH");
        public static UrlData ChatRequest = new UrlData("/srv/chat/request", "BAZITECH");
        public static UrlData DefaultLeagueSubscribe = new UrlData("/srv/league/enrollDefault", "BAZITECH");
        public static UrlData Image = new UrlData("/handlers/imageHandler.ashx?imgid=", "BAZITECH");
        public static UrlData CustomPost = new UrlData("/srv/custompost/get", "BAZITECH");
        public static UrlData Follow = new UrlData("/srv/league/follow", "BAZITECH");
        public static UrlData Invisible = new UrlData("/srv/user/invisible", "BAZITECH");
        public static UrlData RequestQuickMatch = new UrlData("/srv/match/addquick", "BAZITECH", true);
        public static UrlData CancelQuickMatch = new UrlData("/srv/match/removequick", "BAZITECH", true);
        public static UrlData SearchUser = new UrlData("/srv/user/search", "BAZITECH");
        public static UrlData Share = new UrlData("/srv/user/share", "BAZITECH");
        public static UrlData Credit = new UrlData("/srv/user/getcredit", "BAZITECH");
        public static UrlData FileInfo = new UrlData("/srv/game/fileinfo", "BAZITECH");
        public static UrlData GetCreditPackList = new UrlData("/srv/getcreditpacklist", "BAZITECH");
        public static UrlData GetCommentList = new UrlData("/srv/commentList/", "BAZITECH");
        public static UrlData GetLive = new UrlData("/srv/match/getlive", "BAZITECH");
        public static UrlData AddGameComment = new UrlData("/srv/game/addcomment", "BAZITECH");
        public static UrlData AddLeagueComment = new UrlData("/srv/league/addcomment", "BAZITECH");
        public static UrlData ValidChatThreadId = new UrlData("/srv/league/threads", "BAZITECH");
        public static UrlData LeagueAwards = new UrlData("/srv/league/awards", "BAZITECH");
        public static UrlData GetUserProfile = new UrlData("/srv/user/getProfile", "BAZITECH");
        public static UrlData ChangePassword = new UrlData("/srv/user/changepass", "BAZITECH");
        public static UrlData EditProfile = new UrlData("/srv/user/editProfile", "BAZITECH");
        public static UrlData GetLobby = new UrlData("/srv/lobby/get", "BAZITECH");
        public static UrlData GetLobbyGames = new UrlData("/srv/game/getbylobby", "BAZITECH");
        public static UrlData MigrateBazitechToken = new UrlData("/srv/user/migrate", "BAZITECH");
        public static UrlData GetLobbyLeagues = new UrlData("/srv/league/getbylobby", "BAZITECH");
        public static UrlData GetNews = new UrlData("/srv/news/get", "BAZITECH");
        public static UrlData FollowGame = new UrlData("/srv/game/follow", "BAZITECH");
        public static UrlData FollowLeague = new UrlData("/srv/league/follow", "BAZITECH");
        public static UrlData FollowPost = new UrlData("/srv/user/follow", "BAZITECH");
        public static UrlData LikePost = new UrlData("/srv/user/like", "BAZITECH");
        public static UrlData GetTopGame = new UrlData("/srv/game/top", "BAZITECH");
        public static UrlData GetLatestGame = new UrlData("/srv/game/latest", "BAZITECH");
        public static UrlData GetRelatedGame = new UrlData("/srv/game/related", "BAZITECH");
        public static UrlData GetRelatedLeague = new UrlData("/srv/league/related", "BAZITECH");
        public static UrlData GetLeagueTopPlayers = new UrlData("/srv/user/topplayers", "BAZITECH");
        public static UrlData GetTopPlayers = new UrlData("/srv/user/gettopplayers", "BAZITECH");
        public static UrlData GetTimeLine = new UrlData("/srv/timeline", "BAZITECH");
        public static UrlData GetOnlineInfo = new UrlData("/srv/game/onlineinfo", "BAZITECH");
        public static UrlData GetGameFollowning = new UrlData("/srv/game/following", "BAZITECH");
        public static UrlData GetLeagueFollowing = new UrlData("/srv/league/following", "BAZITECH");
        public static UrlData GetEnrolledLeagues = new UrlData("/srv/user/enrolledleagues", "BAZITECH");
        public static UrlData IncreaseCreditByVoucher = new UrlData("/srv/user/increasecreditbyvoucher", "BAZITECH");
        public static UrlData EncryptHandShake = new UrlData("/srv/aut/handshake", "BAZITECH");
        public static UrlData SubscribeLeague = new UrlData(null, "BAZITECH");
        public static UrlData LeagueMatchesResult = new UrlData("/srv/user/matchresult", "BAZITECH");
        public static UrlData LeagueLatestMatchesResult = new UrlData("/srv/league/latestmatchresult", "BAZITECH");
        public static UrlData LeagueLatestMatches = new UrlData("/srv/league/latestmatches", "BAZITECH");
        public static UrlData LeagueMatches = new UrlData("/srv/user/matches", "BAZITECH");
        public static UrlData GameCenterStatus = new UrlData("/srv/manage/gcstatus", "BAZITECH");
        public static UrlData UserAchievements = new UrlData("/srv/user/achievements", "BAZITECH");
        public static UrlData UserAchievementDetails = new UrlData("/srv/user/achievementdetails", "BAZITECH");
        public static UrlData UserGamePoints = new UrlData("/srv/user/gamepoints", "BAZITECH");
        public static UrlData RecievedFriendshipRequest = new UrlData("/srv/user/receivedfriendshiprequestlist", "BAZITECH");
        public static UrlData SentFriendshipRequest = new UrlData("/srv/user/sentfriendshiprequestlist", "BAZITECH");
        public static UrlData FriendshipRequest = new UrlData("/srv/user/friendshiprequest", "BAZITECH");
        public static UrlData ReplyFriendshipRequest = new UrlData("/srv/user/replyfriendshiprequest", "BAZITECH");
        public static UrlData CancelFriendshipRequest = new UrlData("/srv/user/cancelfriendshiprequest", "BAZITECH");
        public static UrlData RemoveFriendRequest = new UrlData("/srv/user/cancelfriendship", "BAZITECH");
        public static UrlData UserFriends = new UrlData("/srv/user/friends", "BAZITECH");
        public static UrlData GetLobbies = new UrlData("/srv/lobby/get", "BAZITECH");
        public static UrlData GetGallery = new UrlData("/srv/product/gallery", "BAZITECH");

        public static UrlData GetInAppPurchasePack = new UrlData("/srv/iap/getgamepacks", "BAZITECH");
        public static UrlData GetGlobalInAppPurchasePack = new UrlData("/srv/iap/getgcpacks", "BAZITECH");
        public static UrlData ByyInAppPurchasePack = new UrlData("/srv/iap/buy", "BAZITECH");
        public static UrlData GetGameItems = new UrlData("/srv/iap/searchgameitems", "BAZITECH");
        public static UrlData GetUserGcItems = new UrlData("/srv/iap/getgcitems", "BAZITECH");
        public static UrlData GetUserItems = new UrlData("/srv/iap/getgameitems", "BAZITECH");
        public static UrlData ConsumeItem = new UrlData("/srv/iap/consume", "BAZITECH");
        public static UrlData GetConfig = new UrlData("/srv/serviceApi/getConfig", "BAZITECH");

        public static UrlData SetLocation = new UrlData("/srv/objectPool/geoLocation/add", "OBJECT_POOL");
        public static UrlData MetaData = new UrlData("/srv/objectPool/metaData/add", "OBJECT_POOL");
        public static UrlData CustomData = new UrlData("/srv/wl/customData/get", "OBJECT_POOL");

        public class UrlData
        {
            public string Uri;
            public string HostName;
            public bool Encrypt;

            public UrlData(string uri, string hostName)
            {
                this.Uri = uri;
                this.HostName = hostName;
            }

            public UrlData(string uri, string hostName, bool encrypt)
            {
                this.Uri = uri;
                this.HostName = hostName;
                this.Encrypt = encrypt;
            }

            public UrlData Copy()
            {
                return new UrlData(Uri, HostName, Encrypt);
            }
        }
    }
}

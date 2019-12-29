using UnityEngine;

namespace playpod.client.sdk.unity.Share
{
    public class PushMessageContentTypes : MonoBehaviour {

        public const int DataPack = 1;
        public const int NewUser = 2;
        public const int LogoutUser = 3;
        public const int RequestIdState = 4;
        public const int MatchNew = 5;
        public const int MatchStart = 6;
        public const int MatchResume = 7;
        public const int MatchPause = 8;
        public const int MatchRequest = 9;
        public const int MatchResult = 10;
        public const int Message = 11;
        public const int MatchReconnect = 12;
        public const int MatchLeave = 13;

    }
}

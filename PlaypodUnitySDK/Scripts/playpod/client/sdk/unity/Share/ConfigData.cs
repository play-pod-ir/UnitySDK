using System;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity.Share
{
    public class ConfigData : MonoBehaviour
    {
        
        public static bool IsLocal = false;
        public static string GcDomainName = IsLocal ? "176.221.69.209:1036" : "pod.ir";
        public static string ServiceMode = ServiceModeTypes.Releasemode;

        public static string Gca = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "https://sandbox.pod.ir:9090"
                : "http://172.16.106.43:8082")
            : "https://service-play.pod.ir";

        public static string Aha = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "https://sandbox.pod.ir:8043"
                : "http://172.16.110.235:8003")
            : "https://playpod-bus.pod.ir";

        public static string Psa = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "ws://sandbox.pod.ir:8003/ws"
                : "ws://172.16.110.235:8003/ws")
            : "wss://playpod-bus.pod.ir/ws";

        public static string Psat = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "sandbox.pod.ir:8002"
                : "172.16.110.235:8002")
            : "https://playpod-bus.pod.ir:8002";

        public static string Csa = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "ws://sandbox.pod.ir:8003/ws"
                : "ws://172.16.110.235:8003/ws")
            : "wss://playpod-bus.pod.ir/ws";

        public static string Csat = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "sandbox.pod.ir:8002"
                : "172.16.110.235:8002")
            : "sandbox.pod.ir:8002";

        public static string Isa = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "http://sandbox.pod.ir:8080"
                : "http://172.16.110.76:8080")
            : "https://core.pod.ir:8080";

        public static string Opsa = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "http://176.221.69.213:8084"
                : "http://172.16.106.45:8084")
            : "http://84.241.60.231:8084";

        public static string Gcid = IsLocal ? "44" : "3"; // game center id
        public static string Gcsid = IsLocal ? "157" : "333"; // game center supporter id
        public static string Gcv = "1.0.0"; // game center id

        public static string Gcau = ""; // game center android app url

        //    public static string configUrl = "http://service." + gcDomainName + "/srv/serviceApi/getConfig";
        public static string ConfigUri = "/srv/serviceApi/getConfig";

        public static string ConfigUrl =
            IsLocal ? (Gca + ConfigUri) : ("https://service-play." + GcDomainName + ConfigUri);

        public static string DomainName = "https://service-play." + GcDomainName;
        public static string Gcpn = "land.pod.play";
        public static JSONObject Gcd = new JSONObject();
        public static JSONObject Lcd = new JSONObject();
        public static string Malv = "1.0.0"; // mobile app last version
        public static bool Malvfu; // mobile app last version force update
        public static string Mamv = "1.0.0"; // mobile app minimum versub
        public static string Malvcl = ""; //mobile app last change log
        public static string Malvdl = ""; //mobile app last version download link
        public static string Sba = "com.fanap.gameCenter.actions.gameToGC";
        public static string Rba = "com.fanap.gameCenter.actions.GCToGame";
        public static string Cu = "ریال"; //CREDIT unit
        public static string Ecpn = "TIS_CHAT_EMOJI_CONFIG";
        public static string Uppu = "market://details?id="; // update package prefix url
        public static string Sucpn = "TIS_SERVICE_UI"; // service ui custom post name
        public static string Icu = "http://service." + GcDomainName + "/payByGateway/"; // increase CREDIT link
        public static string Bglp = "http://www." + GcDomainName + "/#game/"; // bazitech game link prefix
        public static string Bllp = "http://www." + GcDomainName + "/#GET_LEAGUE/"; // bazitech GET_LEAGUE link prefix
        public static string Bvl = "http://www." + GcDomainName + "/about.html"; // view bazitech in iFrame
        public static string Qpl = "http://www." + GcDomainName + "/Hushang"; // quiz panel link
        public static string Gclpu = "http://www." + GcDomainName + "/index.html#login"; // game center LOGIN page url
        public static string Gcru = "http://www." + GcDomainName + "/?#rules"; // game center rules url
        public static string Gcuph = "#profile/user"; // game center user page hash
        public static string Ehd = "##$##"; // encryption hash delimiter

        public static string Ssoleu = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "https://sandbox.pod.ir:9090/Pages/League/Enroll/Default.aspx"
                : "http://172.16.106.43:8082/Pages/League/Enroll/Default.aspx")
            : "https://service-play.pod.ir/Pages/League/Enroll/Default.aspx"; // sso league enroll url

        public static string Ssolu = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "https://accounts.pod.ir/oauth2/authorize/?client_id=39105edd466f819c057b3c937374&response_type=code&redirect_uri=https://sandbox.pod.ir:9090/Pages/Auth/SSOCallback/Default.aspx&scope=phone profile"
                : "http://172.16.110.76/oauth2/authorize/?client_id=59cbd42cc8f29e2ced10858d2&response_type=code&redirect_uri=http://172.16.106.43:8082/Pages/Auth/SSOCallback/Default.aspx&scope=phone profile"
            )
            : "https://accounts.pod.ir/oauth2/authorize/?client_id=16807y864b4ab6a05a80d602f5b6d7&response_type=code&redirect_uri=https://service-play.pod.land:443/Pages/Auth/SSOCallback/Default.aspx&scope=phone profile"; // sso login url

        public static string Ssosu = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "https://accounts.pod.ir/oauth2/authorize/?client_id=39105edd466f819c057b3c937374&response_type=code&redirect_uri=https://sandbox.pod.ir:9090/Pages/Auth/SSOCallback/Default.aspx&prompt=signup&scope=phone profile"
                : "http://172.16.110.76/oauth2/authorize/?client_id=59cbd42cc8f29e2ced10858d2&response_type=code&redirect_uri=http://172.16.106.43:8082/Pages/Auth/SSOCallback/Default.aspx&prompt=signup&scope=phone profile"
            )
            : "https://accounts.pod.ir/oauth2/authorize/?client_id=16807y864b4ab6a05a80d602f5b6d7&response_type=code&redirect_uri=https://service-play.pod.land:443/Pages/Auth/SSOCallback/Default.aspx&prompt=signup&scope=phone profile"; // sso signup  url

        public static string Ssoiau = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "https://sandbox.pod.ir:9090/pages/iap/buy/default.aspx"
                : "http://172.16.106.43:8082/pages/iap/buy/default.aspx")
            : "https://service-play.pod.ir/pages/iap/buy/default.aspx"; //sso inapppurchase  url

        public static string Ssolou = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "https://sandbox.pod.ir:9090/Pages/Logout/Default.aspx"
                : "http://172.16.106.43:8082/Pages/Logout/Default.aspx")
            : "https://service-play.pod.ir/Pages/Logout/Default.aspx"; //sso inapppurchase  url

        public static string Ssogbu = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                ? "https://sandbox.pod.ir:9090/pages/game/buy/default.aspx"
                : "http://172.16.106.43:8082/pages/game/buy/default.aspx")
            : "https://service-play.pod.ir/Pages/game/buy/Default.aspx"; //sso game buy  url

        public static string Ahrrn = IsLocal
            ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline) ? "bp.gc.sandbox" : "vhd.ow")
            : "bp.gc.sandbox";


        public static string Adurl;

        public static int BusinessId = IsLocal ? 43 : 1788;
        public static long Bcpid = IsLocal ? 342 : 2541; //banner custom post id
        public static int Pcct = 20000; //push connection check timeout
        public static int Pcctt = 400; //push connection check timeout threshold
        public static int Wsto = 4000;
        public static int Searchto = 300;
        public static int GetConfigTimeout = 10000;
        public static int Pmto = 15000;
        public static int Smit = 5000;
        public static int Pmttl = 7000;
        public static int Wscwti = 500; //webSocket connection wait time interval
        public static int Mrt = 60000;
        public static int Mmrc = 4;
        public static int Pcrit = 5000; //push Connection Retry Interval Time
        public static int Gcrt = 180; //game center register timeout
        public static int Rvt = 120000;
        public static int Dmt = 20; //default max try
        public static int Gchc = 10; // get chat history count
        public static int Cevid = 10; // check emoji version interval day
        public static int Gldc = 5; //get GET_LEAGUE data count
        public static int Mtml = 200; //max text message length
        public static int Meim = 20; //max emoji in message
        public static int Glms = 20; // get GET_LEAGUE members size
        public static int Gous = 10; // get online user size
        public static int Gsus = 5; // get search user size
        public static int Ormm = 5; //offline Request Minimum  Minute
        public static int Mormd = 10; // max offline request match day,
        public static int Gtdt = 1000; // get thread data timeout
        public static int Qmt = 55000; // quick match timeout
        public static int Glma = 50000; // geo location maximum age
        public static int Glt = 60000; // geo location timeout
        public static int Glit = 600000; // geo location interval timeout
        public static int Siadi = 30; // send installed apps d interval
        public static int Glrdd = 2; //geo location request distance difrence
        public static int Cmp = 5; // chat message priority
        public static int Cf = 1; //CREDIT fraction
        public static int Hrt = 20000; //http request timeout
        public static int Msdtc = 10; //max sent data try count
        public static int Ciid = 1061; //chat item id
        public static int Ehet = 5 * 60 * 1000; //encription handshake expire time
        public static int Pt = 4 * 60 * 1000; //peer timeout


        public static int Gctmhp; // game center max history page

        public static bool Gciv; // game center is Viewable
        public static bool Rctam = true; // remove chat thread after match
        public static bool Suml; // send unvalid match log
        public static bool Cmc = true; //check major conflict
        public static bool Vbif = true; // view bazitech in iFrame
        public static bool Nsu = true; //need service update
        public static bool Lep; //edit profile local
        public static bool Dlor = true; //default GET_LEAGUE offline request
        public static bool Ufs = true; // update from server
        public static bool Har = true; // http async request
        public static bool Harfs; // http async request from socket
        public static bool Ure; // use request encryption
        public static bool Utc; // use tcp connection

        private static void CheckLocalState()
        {
            //Debug.Log("checkLocalState");
            Gca = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "https://sandbox.pod.ir:9090"
                    : "http://172.16.106.43:8082")
                : "https://service-play.pod.ir";
            Aha = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "https://sandbox.pod.ir:8043"
                    : "http://172.16.110.235:8003")
                : "https://playpod-bus.pod.ir";
            Psa = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "ws://sandbox.pod.ir:8003/ws"
                    : "ws://172.16.110.235:8003/ws")
                : "wss://playpod-bus.pod.ir/ws";
            Psat = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "sandbox.pod.ir:8002"
                    : "172.16.110.235:8002")
                : "https://playpod-bus.pod.ir:8002";
            Csa = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "ws://sandbox.pod.ir:8003/ws"
                    : "ws://172.16.110.235:8003/ws")
                : "wss://playpod-bus.pod.ir/ws";
            Csat = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "sandbox.pod.ir:8002"
                    : "172.16.110.235:8002")
                : "https://playpod-bus.pod.ir:8002";
            Isa = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "http://sandbox.pod.ir:8080"
                    : "http://172.16.110.76:8080")
                : "https://core.pod.ir:8080";
            Opsa = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "http://84.241.60.231:8084"
                    : "http://172.16.106.45:8084")
                : "http://84.241.60.231:8084";

            Ssoleu = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "https://sandbox.pod.ir:9090/Pages/League/Enroll/Default.aspx"
                    : "http://172.16.106.43:8082/Pages/League/Enroll/Default.aspx")
                : "https://service-play.pod.ir/Pages/League/Enroll/Default.aspx"; // sso league enroll url

            Ssolu = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "https://accounts.pod.ir/oauth2/authorize/?client_id=39105edd466f819c057b3c937374&response_type=code&redirect_uri=https://sandbox.pod.ir:9090/Pages/Auth/SSOCallback/Default.aspx&scope=phone profile"
                    : "http://172.16.110.76/oauth2/authorize/?client_id=59cbd42cc8f29e2ced10858d2&response_type=code&redirect_uri=http://172.16.106.43:8082/Pages/Auth/SSOCallback/Default.aspx&scope=phone profile"
                )
                : "https://accounts.pod.ir/oauth2/authorize/?client_id=16807y864b4ab6a05a80d602f5b6d7&response_type=code&redirect_uri=https://service-play.pod.land:443/Pages/Auth/SSOCallback/Default.aspx&scope=phone profile"; // sso league enroll url
            Ssosu = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "https://accounts.pod.ir/oauth2/authorize/?client_id=39105edd466f819c057b3c937374&response_type=code&redirect_uri=https://sandbox.pod.ir:9090/Pages/Auth/SSOCallback/Default.aspx&prompt=signup&scope=phone profile"
                    : "http://172.16.110.76/oauth2/authorize/?client_id=59cbd42cc8f29e2ced10858d2&response_type=code&redirect_uri=http://172.16.106.43:8082/Pages/Auth/SSOCallback/Default.aspx&prompt=signup&scope=phone profile"
                )
                : "https://accounts.pod.ir/oauth2/authorize/?client_id=16807y864b4ab6a05a80d602f5b6d7&response_type=code&redirect_uri=https://service-play.pod.land:443/Pages/Auth/SSOCallback/Default.aspx&prompt=signup&scope=phone profile"; // sso league enroll url


            Ssoiau = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "https://sandbox.pod.ir:9090/pages/iap/buy/default.aspx"
                    : "http://172.16.106.43:8082/pages/iap/buy/default.aspx")
                : "https://service-play.pod.ir/pages/iap/buy/default.aspx"; //sso inapppurchase  url

            Ssolou = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "https://sandbox.pod.ir:9090/Pages/Logout/Default.aspx"
                    : "http://172.16.106.43:8082/Pages/Logout/Default.aspx")
                : "https://service-play.pod.ir/Pages/Logout/Default.aspx"; //sso inapppurchase  url

            Ssogbu = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline)
                    ? "https://sandbox.pod.ir:9090/pages/game/buy/default.aspx"
                    : "http://172.16.106.43:8082/pages/game/buy/default.aspx")
                : "https://service-play.pod.ir/Pages/game/buy/Default.aspx"; //sso game buy  url

            Ahrrn = IsLocal
                ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline) ? "bp.gc.sandbox" : "vhd.ow")
                : "playpod.service";

            Gcid = IsLocal ? "44" : "3"; // game center id
            Gcsid = IsLocal ? "157" : "333"; // game center id
            Gcv = "1.0.0"; // game center id
            Gcau = ""; // game center android app url
            ConfigUrl = IsLocal ? (Gca + ConfigUri) : ("https://service-play." + GcDomainName + ConfigUri);
            BusinessId = IsLocal ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline) ? 692 : 43) : 692;
            Bcpid = IsLocal ? 342 : 2541;
            Bcpid = IsLocal ? (ServiceMode.Equals(ServiceModeTypes.DevelopmodeOnline) ? 2541 : 342) : 2541;
        }

        public static void Init(JSONObject config)
        {
            //Debug.Log("ConfigData.init: config: " + (config == null ? "null" : config.ToString()));
            CheckLocalState();

            if (config != null)
            {
                try
                {
                    var conf = config["config"].AsObject;

                    string ip = config["ip"];
                    string gameCenterAddress;
                    if (ip.StartsWith("http"))
                    {
                        gameCenterAddress = ip;
                    }
                    else
                    {
                        gameCenterAddress = "http://" + ip + ":" + config["port"];
                    }


                    Gca = gameCenterAddress;
                    Aha = (conf.HasKey("aha") && conf["aha"] != null)
                        ? (string) conf["aha"]
                        : Aha;
                    Isa = (conf.HasKey("isa") && conf["isa"] != null)
                        ? (string) conf["isa"]
                        : Isa;
                    Psa = (conf.HasKey("psa") && conf["psa"] != null)
                        ? (string) conf["psa"]
                        : "ws://84.241.60.231:8002/ws";
                    Psat = (conf.HasKey("psat") && conf["psat"] != null)
                        ? (string) conf["psat"]
                        : "sandbox.pod.ir:8002";
                    Csa = (conf.HasKey("csa") && conf["csa"] != null)
                        ? (string) conf["csa"]
                        : "ws://84.241.60.231:8002/ws";
                    Csat = (conf.HasKey("csat") && conf["csat"] != null)
                        ? (string) conf["csat"]
                        : "sandbox.pod.ir:8002";

                    Uppu = conf.HasKey("uppu") && conf["uppu"] != null ? (string) conf["uppu"] : Uppu;
                    Sucpn = conf.HasKey("sucpn") && conf["sucpn"] != null ? (string) conf["sucpn"] : Sucpn;
                    Gcpn = conf.HasKey("gcpn") && conf["gcpn"] != null ? (string) conf["gcpn"] : Gcpn;
                    Malv = conf.HasKey("malv") && conf["malv"] != null ? (string) conf["malv"] : Malv;
                    Mamv = conf.HasKey("mamv") && conf["mamv"] != null ? (string) conf["mamv"] : Mamv;
                    Malvcl = conf.HasKey("malvcl") && conf["malvcl"] != null ? (string) conf["malvcl"] : Malvcl;
                    Malvdl = conf.HasKey("malvdl") && conf["malvdl"] != null ? (string) conf["malvdl"] : Malvdl;
                    Sba = conf.HasKey("sba") && conf["sba"] != null ? (string) conf["sba"] : Sba;
                    Rba = conf.HasKey("rba") && conf["rba"] != null ? (string) conf["rba"] : Rba;

                    Icu = (conf.HasKey("icu") && conf["icu"] != null)
                        ? (string) conf["icu"]
                        : ("http://service." + GcDomainName + "/payByGateway/");
                    Bglp = (conf.HasKey("bglp") && conf["bglp"] != null)
                        ? (string) conf["bglp"]
                        : ("http://www." + GcDomainName + "/#game/");
                    Bllp = (conf.HasKey("bllp") && conf["bllp"] != null)
                        ? (string) conf["bllp"]
                        : ("http://www." + GcDomainName + "/#GET_LEAGUE/");
                    Bvl = (conf.HasKey("bvl") && conf["bvl"] != null)
                        ? (string) conf["bvl"]
                        : ("http://www." + GcDomainName + "/about.html");
                    Bvl = (conf.HasKey("qpl") && conf["qpl"] != null)
                        ? (string) conf["qpl"]
                        : ("http://www." + GcDomainName + "/Hushang");
                    Bvl = (conf.HasKey("gclpu") && conf["gclpu"] != null)
                        ? (string) conf["gclpu"]
                        : ("http://www." + GcDomainName + "/index.html#login");
                    Bvl = (conf.HasKey("gcru") && conf["gcru"] != null)
                        ? (string) conf["gcru"]
                        : ("http://www." + GcDomainName + "/?#rules");

                    // from here
                    Cu = conf.HasKey("cu") && conf["cu"] != null ? (string) conf["cu"] : Cu;
                    Ecpn = conf.HasKey("ecpn") && conf["ecpn"] != null ? (string) conf["ecpn"] : Ecpn;
                    Gcuph = conf.HasKey("gcuph") && conf["gcuph"] != null ? (string) conf["gcuph"] : Gcuph;
                    Ehd = conf.HasKey("ehd") && conf["ehd"] != null ? (string) conf["ehd"] : Ehd;
                    Ssoleu = conf.HasKey("ssoleu") && conf["ssoleu"] != null ? (string) conf["ssoleu"] : Ssoleu;
                    Ssolu = conf.HasKey("ssolu") && conf["ssolu"] != null ? (string) conf["ssolu"] : Ssolu;
                    Ssosu = conf.HasKey("ssosu") && conf["ssosu"] != null ? (string) conf["ssosu"] : Ssosu;
                    Ssoiau = conf.HasKey("ssoiau") && conf["ssoiau"] != null ? (string) conf["ssoiau"] : Ssoiau;
                    Ssolou = conf.HasKey("ssolou") && conf["ssolou"] != null ? (string) conf["ssolou"] : Ssolou;
                    Ssogbu = conf.HasKey("ssogbu") && conf["ssogbu"] != null ? (string) conf["ssogbu"] : Ssogbu;
                    Ahrrn = conf.HasKey("ahrrn") && conf["ahrrn"] != null ? (string) conf["ahrrn"] : Ahrrn;

                    Wsto = conf.HasKey("wsto") && conf["wsto"] != null ? Convert.ToInt32(conf["wsto"].AsInt) : Wsto;
                    Searchto = conf.HasKey("searchto") && conf["searchto"] != null
                        ? Convert.ToInt32(conf["searchto"].AsInt)
                        : Searchto;
                    Pmto = conf.HasKey("pmto") && conf["pmto"] != null ? Convert.ToInt32(conf["pmto"].AsInt) : Pmto;
                    Smit = conf.HasKey("smit") && conf["smit"] != null ? Convert.ToInt32(conf["smit"].AsInt) : Smit;
                    Pmttl = conf.HasKey("pmttl") && conf["pmttl"] != null
                        ? Convert.ToInt32(conf["pmttl"].AsInt)
                        : Pmttl;
                    Mrt = conf.HasKey("mrt") && conf["mrt"] != null ? Convert.ToInt32(conf["mrt"].AsInt) : Mrt;
                    Mmrc = conf.HasKey("mmrc") && conf["mmrc"] != null ? Convert.ToInt32(conf["mmrc"].AsInt) : Mmrc;
                    Rvt = conf.HasKey("rvt") && conf["rvt"] != null ? Convert.ToInt32(conf["rvt"].AsInt) : Rvt;
                    BusinessId = conf.HasKey("businessId") && conf["businessId"] != null
                        ? Convert.ToInt32(conf["businessId"].AsInt)
                        : BusinessId;
                    Bcpid = conf.HasKey("bcpid") && conf["bcpid"] != null ? (long) conf["bcpid"] : Bcpid;
                    Pcct = conf.HasKey("PCCT") && conf["PCCT"] != null ? (int) conf["PCCT"] : Pcct;
                    Pcctt = conf.HasKey("PCCTT") && conf["PCCTT"] != null ? (int) conf["PCCTT"] : Pcctt;
                    Wscwti = conf.HasKey("WSCWTI") && conf["WSCWTI"] != null ? (int) conf["WSCWTI"] : Wscwti;
                    Pcrit = conf.HasKey("pcrit") && conf["pcrit"] != null
                        ? Convert.ToInt32(conf["pcrit"].AsInt)
                        : Pcrit;
                    Gcrt = conf.HasKey("gcrt") && conf["gcrt"] != null ? Convert.ToInt32(conf["gcrt"].AsInt) : Gcrt;
                    Gctmhp = conf.HasKey("gctmhp") && conf["gctmhp"] != null
                        ? Convert.ToInt32(conf["gctmhp"].AsInt)
                        : Gctmhp;
                    Gldc = conf.HasKey("gldc") && conf["gldc"] != null ? Convert.ToInt32(conf["gldc"].AsInt) : Gldc;
                    Gsus = conf.HasKey("gsus") && conf["gsus"] != null ? Convert.ToInt32(conf["gsus"].AsInt) : Gsus;
                    Glms = conf.HasKey("glms") && conf["glms"] != null ? Convert.ToInt32(conf["glms"].AsInt) : Glms;
                    Cf = conf.HasKey("cf") && conf["cf"] != null ? Convert.ToInt32(conf["cf"].AsInt) : Cf;
                    Gous = conf.HasKey("gous") && conf["gous"] != null ? Convert.ToInt32(conf["gous"].AsInt) : Gous;
                    Cmp = conf.HasKey("cmp") && conf["cmp"] != null ? Convert.ToInt32(conf["cmp"].AsInt) : Cmp;
                    Gtdt = conf.HasKey("gtdt") && conf["gtdt"] != null ? Convert.ToInt32(conf["gtdt"].AsInt) : Gtdt;
                    Ciid = conf.HasKey("ciid") && conf["ciid"] != null ? Convert.ToInt32(conf["ciid"].AsInt) : Ciid;
                    Ehet = conf.HasKey("ehet") && conf["ehet"] != null ? Convert.ToInt32(conf["ehet"].AsInt) : Ehet;
                    Pt = conf.HasKey("pt") && conf["pt"] != null ? Convert.ToInt32(conf["pt"].AsInt) : Pt;
                    Hrt = conf.HasKey("hrt") && conf["hrt"] != null ? Convert.ToInt32(conf["hrt"].AsInt) : Hrt;

                    Gcid = conf.HasKey("gcid") && conf["gcid"] != null ? conf["gcid"].ToString() : Gcid;
                    Gcsid = conf.HasKey("gcsid") && conf["gcsid"] != null ? (string) conf["gcsid"] : Gcsid;
                    Gcv = conf.HasKey("gcv") && conf["gcv"] != null ? (string) conf["gcv"] : Gcv;
                    Gcau = conf.HasKey("gcau") && conf["gcau"] != null ? (string) conf["gcau"] : Gcau;

                    Gciv = conf.HasKey("gciv") && conf["gciv"] != null ? (bool) conf["gciv"] : Gciv;
                    Malvfu = conf.HasKey("malvfu") && conf["malvfu"] != null ? (bool) conf["malvfu"] : Malvfu;
                    Rctam = conf.HasKey("rctam") && conf["rctam"] != null ? (bool) conf["rctam"] : Rctam;
                    Suml = conf.HasKey("suml") && conf["suml"] != null ? (bool) conf["suml"] : Suml;
                    Cmc = conf.HasKey("cmc") && conf["cmc"] != null ? (bool) conf["cmc"] : Cmc;
                    Vbif = conf.HasKey("vbif") && conf["vbif"] != null ? (bool) conf["vbif"] : Vbif;
                    Nsu = conf.HasKey("nsu") && conf["nsu"] != null ? (bool) conf["nsu"] : Nsu;
                    Lep = conf.HasKey("lep") && conf["lep"] != null ? (bool) conf["lep"] : Lep;
                    Dlor = conf.HasKey("dlor") && conf["dlor"] != null ? (bool) conf["dlor"] : Dlor;
                    Ufs = conf.HasKey("ufs") && conf["ufs"] != null ? (bool) conf["ufs"] : Ufs;
                    Har = conf.HasKey("har") && conf["har"] != null ? (bool) conf["har"] : Har;
                    Harfs = conf.HasKey("harfs") && conf["harfs"] != null ? (bool) conf["harfs"] : Harfs;
                    Ure = conf.HasKey("ure") && conf["ure"] != null ? (bool) conf["ure"] : Ure;
                    Utc = conf.HasKey("utc") && conf["utc"] != null ? (bool) conf["utc"] : Utc;

                    Gcd = conf.HasKey("gcd") && conf["gcd"] != null ? conf["gcd"].AsObject : Gcd;
                    Lcd = conf.HasKey("lcd") && conf["lcd"] != null ? conf["lcd"].AsObject : Lcd;

                    Opsa = conf.HasKey("opsa") && conf["opsa"] != null ? (string) conf["opsa"] : Opsa;
                    // to here

                    ConfigUrl = IsLocal ? Gca : "https://service-play." + GcDomainName;
                    ConfigUrl += ConfigUri;
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception: " + e.Message);
                    //throw new ServiceException(e);
                }
            }
        }

        public static void InitWithDbConfig(JSONObject config)
        {
            CheckLocalState();
            try
            {
                if (config.HasKey("gca")) Gca = config["gca"];
                if (config.HasKey("psa")) Psa = config["psa"];
                if (config.HasKey("psat")) Psat = config["psat"];
                if (config.HasKey("csa")) Csa = config["csa"];
                if (config.HasKey("csat")) Csat = config["csat"];
                if (config.HasKey("isa")) Isa = config["isa"];
                if (config.HasKey("domainName")) DomainName = config["domainName"];
                if (config.HasKey("gcpn")) Gcpn = config["gcpn"];
                if (config.HasKey("malv")) Malv = config["malv"];
                if (config.HasKey("mamv")) Mamv = config["mamv"];
                if (config.HasKey("malvcl")) Malvcl = config["malvcl"];
                if (config.HasKey("malvdl")) Malvdl = config["malvdl"];
                if (config.HasKey("sba")) Sba = config["sba"];
                if (config.HasKey("rba")) Rba = config["rba"];
                if (config.HasKey("cu")) Cu = config["cu"];
                if (config.HasKey("ecpn")) Ecpn = config["ecpn"];
                if (config.HasKey("uppu")) Uppu = config["uppu"];
                if (config.HasKey("sucpn")) Sucpn = config["sucpn"];
                if (config.HasKey("icu")) Icu = config["icu"];
                if (config.HasKey("bglp")) Bglp = config["bglp"];
                if (config.HasKey("bllp")) Bllp = config["bllp"];
                if (config.HasKey("bvl")) Bvl = config["bvl"];
                if (config.HasKey("qpl")) Qpl = config["qpl"];
                if (config.HasKey("gclpu")) Gclpu = config["gclpu"];
                if (config.HasKey("gcru")) Gcru = config["gcru"];
                if (config.HasKey("gcuph")) Gcuph = config["gcuph"];
                if (config.HasKey("ehd")) Ehd = config["ehd"];
                if (config.HasKey("ssoleu")) Ssoleu = config["ssoleu"];
                if (config.HasKey("ssolu")) Ssolu = config["ssolu"];
                if (config.HasKey("ssosu")) Ssosu = config["ssosu"];
                if (config.HasKey("ssoiau")) Ssoiau = config["ssoiau"];
                if (config.HasKey("ssolou")) Ssolou = config["ssolou"];
                if (config.HasKey("ssogbu")) Ssogbu = config["ssogbu"];
                if (config.HasKey("ahrrn")) Ahrrn = config["ahrrn"];
                if (config.HasKey("adurl")) Adurl = config["adurl"];
                if (config.HasKey("gcid")) Gcid = config["gcid"];
                if (config.HasKey("gcsid")) Gcsid = config["gcsid"];
                if (config.HasKey("gcv")) Gcv = config["gcv"];
                if (config.HasKey("gcau")) Gcau = config["gcau"];
                if (config.HasKey("PCCT")) Pcct = (int) config["PCCT"];
                if (config.HasKey("PCCTT")) Pcctt = (int) config["PCCTT"];
                if (config.HasKey("wsto")) Wsto = (int) config["wsto"];
                if (config.HasKey("searchto")) Searchto = (int) config["searchto"];
                if (config.HasKey("getConfigTimeout")) GetConfigTimeout = (int) config["getConfigTimeout"];
                if (config.HasKey("pmto")) Pmto = (int) config["pmto"];
                if (config.HasKey("smit")) Smit = (int) config["smit"];
                if (config.HasKey("pmttl")) Pmttl = (int) config["pmttl"];
                if (config.HasKey("WSCWTI")) Wscwti = (int) config["WSCWTI"];
                if (config.HasKey("mrt")) Mrt = (int) config["mrt"];
                if (config.HasKey("mmrc")) Mmrc = (int) config["mmrc"];
                if (config.HasKey("pcrit")) Pcrit = (int) config["pcrit"];
                if (config.HasKey("gcrt")) Gcrt = (int) config["gcrt"];
                if (config.HasKey("rvt")) Rvt = (int) config["rvt"];
                if (config.HasKey("businessId")) BusinessId = (int) config["businessId"];
                if (config.HasKey("bcpid")) Bcpid = (long) config["bcpid"];
                if (config.HasKey("dmt")) Dmt = (int) config["dmt"];
                if (config.HasKey("gchc")) Gchc = (int) config["gchc"];
                if (config.HasKey("cevid")) Cevid = (int) config["cevid"];
                if (config.HasKey("gldc")) Gldc = (int) config["gldc"];
                if (config.HasKey("ormm")) Ormm = (int) config["ormm"];
                if (config.HasKey("meim")) Meim = (int) config["meim"];
                if (config.HasKey("glms")) Glms = (int) config["glms"];
                if (config.HasKey("gous")) Gous = (int) config["gous"];
                if (config.HasKey("gsus")) Gsus = (int) config["gsus"];
                if (config.HasKey("mormd")) Mormd = (int) config["mormd"];
                if (config.HasKey("gtdt")) Gtdt = (int) config["gtdt"];
                if (config.HasKey("qmt")) Qmt = (int) config["qmt"];
                if (config.HasKey("glma")) Glma = (int) config["glma"];
                if (config.HasKey("glt")) Glt = (int) config["glt"];
                if (config.HasKey("glit")) Glit = (int) config["glit"];
                if (config.HasKey("siadi")) Siadi = (int) config["siadi"];
                if (config.HasKey("glrdd")) Glrdd = (int) config["glrdd"];
                if (config.HasKey("cmp")) Cmp = (int) config["cmp"];
                if (config.HasKey("cf")) Cf = (int) config["cf"];
                if (config.HasKey("hrt")) Hrt = (int) config["hrt"];
                if (config.HasKey("msdtc")) Msdtc = (int) config["msdtc"];
                if (config.HasKey("gctmhp")) Gctmhp = (int) config["gctmhp"];
                if (config.HasKey("ciid")) Ciid = (int) config["ciid"];
                if (config.HasKey("ehet")) Ehet = (int) config["ehet"];
                if (config.HasKey("pt")) Pt = (int) config["pt"];

                if (config.HasKey("gciv")) Gciv = (bool) config["gciv"];
                if (config.HasKey("malvfu")) Malvfu = (bool) config["malvfu"];
                if (config.HasKey("rctam")) Rctam = (bool) config["rctam"];
                if (config.HasKey("suml")) Suml = (bool) config["suml"];
                if (config.HasKey("cmc")) Cmc = (bool) config["cmc"];
                if (config.HasKey("vbif")) Vbif = (bool) config["vbif"];
                if (config.HasKey("nsu")) Nsu = (bool) config["nsu"];
                if (config.HasKey("lep")) Lep = (bool) config["lep"];
                if (config.HasKey("dlor")) Dlor = (bool) config["dlor"];
                if (config.HasKey("ufs")) Ufs = (bool) config["ufs"];
                if (config.HasKey("har")) Har = (bool) config["har"];
                if (config.HasKey("harfs")) Harfs = (bool) config["harfs"];
                if (config.HasKey("ure")) Ure = (bool) config["ure"];
                if (config.HasKey("utc")) Utc = (bool) config["utc"];
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }
        }

        public static string Serialize()
        {
            var config = new JSONObject();
            try
            {
                config.Add("gca", Gca);
                config.Add("psa", Psa);
                config.Add("psat", Psat);
                config.Add("csa", Csa);
                config.Add("csat", Csat);
                config.Add("isa", Isa);
                config.Add("domainName", DomainName);
                config.Add("gcpn", Gcpn);
                config.Add("malv", Malv);
                config.Add("mamv", Mamv);
                config.Add("malvcl", Malvcl);
                config.Add("malvdl", Malvdl);
                config.Add("sba", Sba);
                config.Add("rba", Rba);
                config.Add("cu", Cu);
                config.Add("ecpn", Ecpn);
                config.Add("uppu", Uppu);
                config.Add("sucpn", Sucpn);
                config.Add("icu", Icu);
                config.Add("bglp", Bglp);
                config.Add("bllp", Bllp);
                config.Add("bvl", Bvl);
                config.Add("qpl", Qpl);
                config.Add("gclpu", Gclpu);
                config.Add("gcru", Gcru);
                config.Add("gcuph", Gcuph);
                config.Add("ehd", Ehd);
                config.Add("ssoleu", Ssoleu);
                config.Add("ssolu", Ssolu);
                config.Add("ssosu", Ssosu);
                config.Add("ssoiau", Ssoiau);
                config.Add("ssolou", Ssolou);
                config.Add("ssogbu", Ssogbu);
                config.Add("ahrrn", Ahrrn);
                config.Add("adurl", Adurl);
                config.Add("gcid", Gcid);
                config.Add("gcsid", Gcsid);
                config.Add("gcv", Gcv);
                config.Add("gcau", Gcau);
                config.Add("PCCT", Pcct);
                config.Add("PCCTT", Pcctt);
                config.Add("wsto", Wsto);
                config.Add("searchto", Searchto);
                config.Add("getConfigTimeout", GetConfigTimeout);
                config.Add("pmto", Pmto);
                config.Add("smit", Smit);
                config.Add("pmttl", Pmttl);
                config.Add("WSCWTI", Wscwti);
                config.Add("mrt", Mrt);
                config.Add("mmrc", Mmrc);
                config.Add("pcrit", Pcrit);
                config.Add("gcrt", Gcrt);
                config.Add("rvt", Rvt);
                config.Add("businessId", BusinessId);
                config.Add("bcpid", Bcpid);
                config.Add("dmt", Dmt);
                config.Add("gchc", Gchc);
                config.Add("cevid", Cevid);
                config.Add("gldc", Gldc);
                config.Add("mtml", Mtml);
                config.Add("meim", Meim);
                config.Add("glms", Glms);
                config.Add("gous", Gous);
                config.Add("gsus", Gsus);
                config.Add("ormm", Ormm);
                config.Add("mormd", Mormd);
                config.Add("gtdt", Gtdt);
                config.Add("qmt", Qmt);
                config.Add("glma", Glma);
                config.Add("glt", Glt);
                config.Add("glit", Glit);
                config.Add("siadi", Siadi);
                config.Add("glrdd", Glrdd);
                config.Add("cmp", Cmp);
                config.Add("cf", Cf);
                config.Add("hrt", Hrt);
                config.Add("msdtc", Msdtc);
                config.Add("gctmhp", Gctmhp);
                config.Add("gciv", Gciv);
                config.Add("malvfu", Malvfu);
                config.Add("rctam", Rctam);
                config.Add("suml", Suml);
                config.Add("cmc", Cmc);
                config.Add("vbif", Vbif);
                config.Add("nsu", Nsu);
                config.Add("lep", Lep);
                config.Add("dlor", Dlor);
                config.Add("ufs", Ufs);
                config.Add("har", Har);
                config.Add("harfs", Harfs);
                config.Add("ure", Ure);
                config.Add("utc", Utc);
                config.Add("ciid", Ciid);
                config.Add("ehet", Ehet);
                config.Add("pt", Pt);
                config.Add("gcd", Gcd);
                config.Add("lcd", Lcd);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }

            return config.ToString();
        }
    }
}
package land.pod.play.ipc;

import land.pod.play.ipc.RequestCallback;

interface LauncherInterface {

    void getUserData(String params,RequestCallback callback);


    void loginUI(String params,RequestCallback callback);
    void signupUI(String params,RequestCallback callback);
    void leaderBoardUI(String params);
    void gameUI(String params);
    void leagueUI(String params);
    void matchRequestUI(String params);
    void editProfileUI(String params,RequestCallback callback);
    void inAppPurchaseUI(String params,RequestCallback callback);
    void increaseCreditUI(String params,RequestCallback callback);
    void subscribeLeagueUI(String params,RequestCallback callback);
    void buyPackUI(String params,RequestCallback callback);

}

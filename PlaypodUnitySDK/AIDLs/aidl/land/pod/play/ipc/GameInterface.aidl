package land.pod.play.ipc;

import land.pod.play.ipc.RequestCallback;

interface GameInterface {

    void newMatch(String params);
    void canAcceptMatch(String params,RequestCallback callback);
    void startApp(String params);

}

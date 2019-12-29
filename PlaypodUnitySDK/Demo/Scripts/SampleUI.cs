using System.Collections;
using System.Collections.Generic;
using playpod.client.sdk.unity;
using playpod.client.sdk.unity.Share;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SampleUI : MonoBehaviour
{
    public PlaypodService playpod;

    public Button initServiceButton;
    public Button getUserDataButton;
    public Button showLeaderBoardButton;
    // Start is called before the first frame update
    void Start()
    {
        initServiceButton.onClick.AddListener(() =>
        {
            playpod.GetServiceInstance(message =>
            {
                if (message["hasError"].AsBool)
                {
                    playpod.LOG(Util.PrettyJson(message));
                    return;
                }
                
                // message is a JSONObject which will have error details
                // should something go wrong
                
                playpod.LOG("Service initialized successfully.");
                playpod.SetDefaultParams(size:5, offset:0, filter:"");
                
                getUserDataButton.gameObject.SetActive(true);
                showLeaderBoardButton.gameObject.SetActive(true);
            });
        });
        
        getUserDataButton.onClick.AddListener(() =>
        {
            playpod.GetUserData(result =>
            {
                playpod.LOG("User Data:" + Util.PrettyJson(result));
            });
        });
        
        showLeaderBoardButton.onClick.AddListener(() =>
        {
            playpod.getLeaderBoard(result =>
            {
                playpod.LOG("GetLeaderBoard:" + Util.PrettyJson(result));
            });
        });
    }
}

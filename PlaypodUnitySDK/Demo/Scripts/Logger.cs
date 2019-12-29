using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Text _loggerText;
#pragma warning restore 0649
    
    public static Logger Instance;

    public bool isInit = false;
    
    public ScrollRect scroll;
    
    private List<KeyValuePair<string, int>> _loggerMessages;

    private void Awake()
    {
        Instance = this;
    }

    public void Log(string msg)
    {
        if (!isInit) return;
        
        if (Instance == null) Instance = this;
        
        if (_loggerMessages == null) _loggerMessages = new List<KeyValuePair<string, int>>();
        
        if (_loggerMessages.Exists(pair => pair.Key == msg))
        {
            var message = _loggerMessages.Find(pair => pair.Key == msg);
            var currentCount = message.Value;
            _loggerMessages.Remove(message);
            message = new KeyValuePair<string, int>(msg, currentCount + 1);
            _loggerMessages.Add(message);
        }
        else
        {
            _loggerMessages.Add(new KeyValuePair<string, int>(msg, 1));
        }

        _loggerText.text = "";

        foreach (var message in _loggerMessages)
        {
            _loggerText.text += message.Key + "\n------------------------------------------- (" + message.Value + ")\n\n";
        }
        
        scroll.verticalNormalizedPosition = 0;
    }
    
    public void Clear()
    {
        _loggerMessages.Clear();
        _loggerText.text = "";
    }
}

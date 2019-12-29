using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoggerScollToBottom : MonoBehaviour
{
    public ScrollRect scroll;

    public void scrollToBottom()
    {
        scroll.verticalNormalizedPosition = 0;
    }
}

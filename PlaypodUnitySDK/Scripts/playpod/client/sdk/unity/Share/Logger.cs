using System.Collections.Generic;
using UnityEngine;

namespace playpod.client.sdk.unity.Share
{
    public class Logger
    {
        private static bool _enable = true;
        private static  Dictionary<string , Logger> _logs = new Dictionary<string, Logger>();
        readonly string _className = null;

        Logger(string className) {
            this._className = className;
        }

        public void Info(string data) {
            if (_enable) {
//            Thread one = new Thread() {
//                public void run() {
//                    System.out.println(data);
//                }
//            };
//            one.start();
                string printData = "";
                if (_className != null) {
                    printData += _className + "  ";
                }
                printData += data;
                Debug.Log(printData);
            }
        }

        static void Enable() {
            _enable = true;
        }

        static void Disable() {
            _enable = false;
        }

        public static Logger GetLogger(string className) {
            Logger log = _logs[className];
            if (log == null) {
                log = new Logger(className);
            }
            return  log;
        }
    }
}
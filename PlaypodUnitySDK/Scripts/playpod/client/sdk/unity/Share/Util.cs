using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity.Share
{
    public class Util
    {
        private static MonoBehaviour _mono;
        
        public static void Init (MonoBehaviour mono)
        {
            _mono = mono;
        }
        

        // todo: incomplete. implement c# version later. currently returns the passed string
        public static string Md5(string s)
        {
            try
            {
                //            MessageDigest digest = java.security.MessageDigest.getInstance("MD5");
                //            digest.update(s.getBytes("UTF-8"));
                //            byte messageDigest[] = digest.digest();
                //
                //            // Create Hex string
                //            StringBuffer hexString = new StringBuffer();
                //            for (int i=0; i<messageDigest.length; i++)
                //                hexString.append(Integer.toHexString(0xFF & messageDigest[i]));
                //
                //            return hexString.toString();


                Debug.Log("11111111111 " + s);

                //			MessageDigest md = MessageDigest.getInstance("MD5");
                //			byte[] messageDigest = md.digest(s.getBytes("UTF-8"));
                //			BigInteger number = new BigInteger(1, messageDigest);
                //			string hashtext = number.toString(16);
                // Now we need to zero pad it if you actually want the full 32 chars.
                //			while (hashtext.length() < 32) {
                //				hashtext = "0" + hashtext;
                //			}

                //			System.out.println("22222222222 " + hashtext);

                //			return hashtext;
                return s;
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            return "";
        }

        public static string AesEncrypt(string data, string key, string iv)
        {
            try
            {
                //            Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
                //            int blockSize = 128;
                ////            int blockSize = cipher.getBlockSize();
                //
                //            byte[] dataBytes = data.getBytes();
                //            int plaintextLength = dataBytes.length;
                ////            if (plaintextLength % blockSize != 0) {
                ////                plaintextLength = plaintextLength + (blockSize - (plaintextLength % blockSize));
                ////            }
                //
                //            byte[] plaintext = new byte[plaintextLength];
                //            System.arraycopy(dataBytes, 0, plaintext, 0, dataBytes.length);
                //
                //            SecretKeySpec keyspec = new SecretKeySpec(key.getBytes(Charset.forName("US-ASCII")), "AES");
                ////            IvParameterSpec ivspec = new IvParameterSpec(iv.getBytes(Charset.forName("US-ASCII")));
                //            IvParameterSpec ivspec = new IvParameterSpec(Base64.decode(iv, 0));
                //
                //            cipher.init(Cipher.ENCRYPT_MODE, keyspec, ivspec);
                //            byte[] encrypted = cipher.doFinal(plaintext);
                //            string str = Base64.encodeToString(encrypted, Base64.DEFAULT);
                ////            string str = new sun.misc.BASE64Encoder().encode(encrypted);
                //
                //            return str;

                return Encrypt(data, key, iv, "AES");
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                return null;
                //throw new ServiceException(e);
            }
        }

        public static string DesEncrypt(string data, string key, string iv)
        {
            try
            {
                //            Cipher cipher = Cipher.getInstance("DES/CBC/PKCS5Padding");
                //            int blockSize = 128;
                ////            int blockSize = cipher.getBlockSize();
                //
                //            byte[] dataBytes = data.getBytes();
                //            int plaintextLength = dataBytes.length;
                ////            if (plaintextLength % blockSize != 0) {
                ////                plaintextLength = plaintextLength + (blockSize - (plaintextLength % blockSize));
                ////            }
                //
                //            byte[] plaintext = new byte[plaintextLength];
                //            System.arraycopy(dataBytes, 0, plaintext, 0, dataBytes.length);
                //
                //            SecretKeySpec keyspec = new SecretKeySpec(key.getBytes(Charset.forName("US-ASCII")), "DES");
                //            System.out.println("AAAAAAAAAAAAAAA " + data);
                //            System.out.println("BBBBBBBBBBBBBBB " + iv + " " + key);
                //            System.out.println("CCCCCCCCCCCCCCC " + Arrays.toString(iv.getBytes(Charset.forName("US-ASCII"))));
                //            System.out.println("DDDDDDDDDDDDDDD " + Arrays.toString(plaintext));
                //            System.out.println("FFFFFFFFFFFFFFFFF " + Arrays.toString(Base64.decode(iv, 0)));
                //
                ////            IvParameterSpec ivspec = new IvParameterSpec(iv.getBytes(Charset.forName("US-ASCII")));
                ////            IvParameterSpec ivspec = new IvParameterSpec(Base64.decode(iv, 0));
                //            IvParameterSpec ivspec = new IvParameterSpec(hexStringToByteArray(iv));
                //
                //
                //            cipher.init(Cipher.ENCRYPT_MODE, keyspec, ivspec);
                //            byte[] encrypted = cipher.doFinal(plaintext);
                //            string str = Base64.encodeToString(encrypted, Base64.DEFAULT);
                ////            string str = new sun.misc.BASE64Encoder().encode(encrypted);
                //
                //            System.out.println("EEEEEEEEEEEEEEE " + str);
                //
                //            return str;
                return Encrypt(data, key, iv, "DES");
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                return null;
                //throw new ServiceException(e);
            }
        }



        // todo: incomplete. implement c# version later. currently returns the passed string
        private static string Encrypt(string data, string key, string iv, string algorithm)
        {
            try
            {
                //            Cipher cipher = Cipher.getInstance(algorithm + "/CBC/PKCS5Padding");
                //            int blockSize = 128;
                ////            int blockSize = cipher.getBlockSize();
                //
                //            byte[] dataBytes = data.getBytes();
                //            int plaintextLength = dataBytes.length;
                ////            if (plaintextLength % blockSize != 0) {
                ////                plaintextLength = plaintextLength + (blockSize - (plaintextLength % blockSize));
                ////            }
                //
                //            byte[] plaintext = new byte[plaintextLength];
                //            System.arraycopy(dataBytes, 0, plaintext, 0, dataBytes.length);
                //
                //            SecretKeySpec keyspec = new SecretKeySpec(key.getBytes(Charset.forName("US-ASCII")), algorithm);
                //            System.out.println("AAAAAAAAAAAAAAA " + data);
                //            System.out.println("BBBBBBBBBBBBBBB " + algorithm + " " + iv + " " + key);
                //
                //
                ////            IvParameterSpec ivspec = new IvParameterSpec(iv.getBytes(Charset.forName("US-ASCII")));
                //            IvParameterSpec ivspec = new IvParameterSpec(iv.getBytes("UTF-8"));
                ////            IvParameterSpec ivspec = new IvParameterSpec(Base64.decode(iv, Base64.DEFAULT));
                ////            IvParameterSpec ivspec = new IvParameterSpec(hexStringToByteArray(iv));
                //
                //
                //            cipher.init(Cipher.ENCRYPT_MODE, keyspec, ivspec);
                //            byte[] encrypted = cipher.doFinal(plaintext);
                //            string str = Base64.encodeToString(encrypted, Base64.DEFAULT);
                ////            string str = new string(encrypted);
                ////            str =  URLEncoder.encode(str, "UTF-8");
                ////            string str = new sun.misc.BASE64Encoder().encode(encrypted);
                //
                ////            System.out.println("DDDDDDDDDDDDDDDDDDD " + str);
                //
                //            return str;
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                return null;
                //throw new ServiceException(e);
            }
        }
        
        public static string PrettyJson(JSONObject uglyJson)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(uglyJson.ToString()), Formatting.Indented);
        }
        
        public static JSONObject CreateServerReturnData(bool hasError, String message, int errorCode, System.Object result) {
            JSONObject retData = new JSONObject();

            try {
                retData.Add("HasError", hasError);
                retData.Add("ErrorMessage", message!= null ? message :"");
                retData.Add("ErrorCode", errorCode);
                retData.Add("Result", result.ToString());
            } catch (Exception e) {
                Debug.LogError("Exception: " + e.Message);
            }

            return retData;
        }

        public static JSONObject CreateReturnData(bool hasError, string message, int? errorCode, JSONObject result)
        {
            var retData = new JSONObject();

            try
            {
                retData.Add("hasError", hasError);
                retData.Add("errorMessage", message != null ? message : "");
                retData.Add("errorCode", errorCode != null ? errorCode : 0);
                retData.Add("result", result);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }

            return retData;
        }

        public static JSONObject CreateReturnData(bool hasError, string message, int? errorCode, JSONArray result)
        {
            var retData = new JSONObject();

            try
            {
                retData.Add("hasError", hasError);
                retData.Add("errorMessage", message != null ? message : "");
                retData.Add("errorCode", errorCode != null ? errorCode : 0);
                retData.Add("result", result);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
                //throw new ServiceException(e);
            }

            return retData;
        }

        public static JSONObject ExceptionErrorData(Exception e)
        {
            var retData = new JSONObject();
            try
            {
                retData.Add("hasError", true);
                retData.Add("errorMessage", e.Message);
                retData.Add("errorCode", ErrorCodes.Exception);
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception: " + ex);
            }

            return retData;
        }

        private static readonly Dictionary<string, IEnumerator> Timeouts = new Dictionary<string, IEnumerator>();

        public static string SetTimeout(Action onDone, long delayMiliSeconds)
        {
            var timeoutId = Guid.NewGuid().ToString();
            var coroutine = _setTimeout(timeoutId, onDone, delayMiliSeconds / 1000);
            Timeouts.Add(timeoutId, coroutine);
            _mono.StartCoroutine(coroutine);
            return timeoutId;
        }

        public static string SetTimeout(Action onDone, int delayMiliSeconds)
        {
            //Debug.Log("Before to string");
            var timeoutId = Guid.NewGuid().ToString();
            //Debug.Log("After to string");
            var coroutine = _setTimeout(timeoutId, onDone, delayMiliSeconds / 1000);
            Timeouts.Add(timeoutId, coroutine);
            //Debug.Log("timeoutId: " + timeoutId);
            //Debug.Log("instance: " + (instance == null));
            _mono.StartCoroutine(coroutine);
            //Debug.Log("timeoutId: " + timeoutId);
            return timeoutId;
        }

        private static IEnumerator _setTimeout(string timeoutId, Action onDone, long delaySeconds)
        {
            //Debug.Log("_setTimeout: ");
            yield return new WaitForSeconds(delaySeconds);
            onDone();
            ClearTimeout(timeoutId);
        }

        public static void ClearTimeout(string timeoutId)
        {
            //Debug.Log("clear time out start");

            //Debug.Log("timeouts: " + timeouts[timeoutId].GetType());
            if (Timeouts.ContainsKey(timeoutId) && Timeouts[timeoutId] != null)
            {
                _mono.StopCoroutine(Timeouts[timeoutId]);
                Timeouts.Remove(timeoutId);
            }

            //Debug.Log("clear time out finish");
        }

        public static bool IsValidInJson(JSONObject json, string key)
        {
            return json.HasKey(key) && json[key] != null;
        }

        public static bool HasMajorConflict(String currentVersion, String lastVersion)
        {
            if (currentVersion != null && lastVersion != null)
            {
                // original
                //return !(currentVersion.Split('\\.')[0].Equals(lastVersion.Split("\\.")[0]));

                // mine (may cause error)
                return !(currentVersion.Split('.')[0].Equals(lastVersion.Split('.')[0]));
            }
            else
            {
                return false;
            }
        }

        public static JSONArray GetQueryStringData(String url)
        {

            // todo: implement later

            return new JSONArray();

            //    try {
            //            JSONArray queryData = new JSONArray();
            //            string regex = "\\?([^#]*)";

            //            Pattern pattern = Pattern.compile(regex, Pattern.CASE_INSENSITIVE);
            //            Matcher matcher = pattern.matcher(url);

            //            if (matcher.find())
            //            {
            //                String data = matcher.group(1);

            //                String[] urlData = data.split("&");

            //                for (int i = 0; i < urlData.length; i++)
            //                {
            //                    if (urlData[i].length() > 0)
            //                    {
            //                        String query = urlData[i];

            //                        if (query != null && query.length() > 0)
            //                        {
            //                            String[] qData = query.split("=");

            //                            if (qData.length == 2)
            //                            {
            //                                JSONObject paramData = new JSONObject();
            //                                paramData.put("name", qData[0]);
            //                                paramData.put("value", qData[1]);
            //                            }
            //                        }
            //                    }
            //                }

            //            }
            //            return queryData;
            //        }catch (Exception e){
            //        throw new ServiceException(e);
            //}

        }
    }
}
using Newtonsoft.Json.Linq;
using SimpleJSON;
public class ReceiveData
{
    public string Receiver;
    public string Method;
    public JSONObject Data;

    public ReceiveData(string json)
    {
        JSONObject jo = JSON.Parse(json).AsObject;
        
        if (jo.HasKeyNotNull("Receiver"))
        {
            Receiver = jo["Receiver"];
        }
        
        if (jo.HasKeyNotNull("Method"))
        {
            Method = jo["Method"];
        }
        
        if (jo.HasKeyNotNull("Data"))
        {
            Data = jo["Data"].AsObject;
        }
    }
}
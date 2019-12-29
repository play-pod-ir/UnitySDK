using System;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity.Database
{
    public class Database
    {
        //todo: implement fully later

        //static Logger log = Logger.getLogger(Database.class);
        //private SQLiteDatabase db;
        //private DBCallback dbCallback;

        public Database(JSONObject Params, Action<Database> onCreate, Action<Database> onUpgrade,
            Action<Database> onOpen)
        {
            //super((Context)params.get("context"),params.getString("name"), null,params.getInt("version"));
            //dbCallback = callback;
            try
            {
                //Context context = (Context) params.get("context");

                //string name = Params["name"];
                //int version = Params["version"].AsInt;
                var self = this;

                onCreate(self);
                onUpgrade(self);
                onOpen(self);


                //    SQLiteOpenHelper sqLiteOpenHelper = new SQLiteOpenHelper(context, name, null, version)
                //    {
                //        @Override
                //        public void onCreate(SQLiteDatabase sqLiteDatabase)
                //    {
                //        db = sqLiteDatabase;
                //        callback.onCreate(self);
                //    }

                //    @Override

                //    public void onUpgrade(SQLiteDatabase sqLiteDatabase, int i, int i1)
                //    {
                //        log.info("database_onUpgrade");
                //        db = sqLiteDatabase;
                //        callback.onUpgrade(self);
                //    }

                //    @Override

                //    public void onOpen(SQLiteDatabase sqLiteDatabase)
                //    {
                //        log.info("database_onOpen " + sqLiteDatabase.getVersion());
                //        db = sqLiteDatabase;
                //        callback.onOpen(self);
                //    }

                //};
                //// onCreate call if this method call
                //sqLiteOpenHelper.getReadableDatabase();
            }
            catch (Exception e)
            {
                Debug.LogError("Database_Exception " + e.Message);
                //throw new ServiceException(e);
            }

            //Debug.Log("database_INIT");
        }

        //    public void execSQL(string sql) throws ServiceException
        //    {
        //        try {
        //            db.execSQL(sql);
        //        } catch (Exception e) {
        //            e.printStackTrace();
        //            throw new ServiceException(e);
        //}

        //    }

        //    public Cursor rawQuery(string sql, string[] selectionArgs)
        //{
        //    return db.rawQuery(sql, selectionArgs);
        //}


        //public void createTable(JSONObject params, RequestCallback callback) throws ServiceException
        //{
        //        try {

        //        string tableName = params.getString("name");
        //        string sqlStr = "CREATE TABLE " + tableName;
        //        if (params.has("attr") && !params.isNull("attr")) {
        //            sqlStr += "(";
        //            JSONArray attrs = params.getJSONArray("attr");
        //            for (int i = 0; i < attrs.length(); i++)
        //            {
        //                JSONObject attr = attrs.getJSONObject(i);
        //                string attrName = attr.getString("name");
        //                string attrType = attr.getString("type");

        //                sqlStr += attrName + " ";
        //                sqlStr += attrType + "";
        //                if (attr.has("primaryKey") && !params.isNull("primaryKey") && params.getBoolean("PRIMARY KEY")) {
        //                sqlStr += "PRIMARY KEY ";
        //            }
        //            if (i < attrs.length() - 1)
        //            {
        //                sqlStr += ",";
        //            }
        //        }

        //        sqlStr += ")";
        //    }
        //    self.execSQL(sqlStr);
        //    callback.onResult(Util.createReturnData(false, null, null, new JSONObject()));

        //        } catch (Exception e) {
        //            throw new ServiceException(e);
        //        }
        //    }

        //    public void find(JSONObject params, RequestCallback callback) throws ServiceException
        //{
        //        try {
        //        string tableName = params.getString("tableName");
        //        string sqlStr = "SELECT * FROM " + tableName;

        //        if (params.has("where") && !params.isNull("where")) {
        //            JSONObject where = params.getJSONObject("where");
        //            JSONArray keys = where.names();
        //            if (keys != null && keys.length() > 0)
        //            {
        //                string key = keys.getString(0);
        //                sqlStr += " WHERE ";
        //                sqlStr += (key + "=\"" + where.get(key) + "\"");
        //            }
        //        }
        //        Cursor cursor = self.rawQuery(sqlStr, null);

        //        JSONArray result = new JSONArray();
        //        cursor.moveToFirst();
        //        while (!cursor.isAfterLast())
        //        {
        //            string[] columnNames = cursor.getColumnNames();
        //            JSONObject data = new JSONObject();
        //            for (int i = 0; i < columnNames.length; i++)
        //            {
        //                string columnName = columnNames[i];
        //                data.put(columnName, cursor.getString(cursor.getColumnIndex(columnName)));
        //            }
        //            result.put(data);
        //            cursor.moveToNext();
        //        }
        //        callback.onResult(Util.createReturnData(false, null, null, result));
        //    } catch (Exception e) {
        //        throw new ServiceException(e);
        //    }
        //}

        //public void insert(JSONObject params, RequestCallback callback) throws ServiceException
        //{
        //        try {
        //        string tableName = params.getString("tableName");
        //        string sqlStr = "INSERT OR REPLACE INTO " + tableName;

        //        if (params.has("data") && !params.isNull("data")) {
        //            JSONObject data = params.getJSONObject("data");
        //            JSONArray keys = data.names();
        //            if (keys != null && keys.length() > 0)
        //            {

        //                string valuesStr = "(";
        //                string keysStr = "(";
        //                for (int i = 0; i < keys.length(); i++)
        //                {
        //                    string key = keys.getString(i);
        //                    valuesStr += (data.get(key));
        //                    keysStr += key;

        //                    if (i < keys.length() - 1)
        //                    {
        //                        valuesStr += ",";
        //                        keysStr += ",";
        //                    }
        //                }

        //                valuesStr += ")";
        //                keysStr += ")";

        //                sqlStr += (keysStr + " VALUES " + valuesStr + ";");
        //            }
        //            else
        //            {
        //                throw new ServiceException("data dos not any key and value");
        //            }
        //        } else {
        //            throw new ServiceException("data key is not defined in params");
        //        }
        //        self.execSQL(sqlStr);
        //        if (callback != null)
        //        {
        //            callback.onResult(Util.createReturnData(false, null, null, new JSONObject()));
        //        }

        //    } catch (Exception e) {
        //        throw new ServiceException(e);
        //    }
        //}
    }
}
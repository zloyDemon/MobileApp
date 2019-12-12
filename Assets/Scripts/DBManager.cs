using System;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Text;

class DBManager
{
    private static readonly string DBName = "adviceDB.bytes";
    private static readonly string AdviceTableName = "Advice";

    public static DBManager Instance { get; } = new DBManager();

    public event Action DatabaseUpdated = () => { };
    public event Action<Advice> AddedToDB = a => { };
    public event Action<Advice> DeletedFromDB = a => { };
    public List<Advice> CurrentAdvices { get; private set; } = new List<Advice>();

    private SqliteConnection dbConnection;
    private SqliteCommand cmd;
    private SqliteDataReader reader;
    private string dbPath;

    public void Init()
    {
        dbPath = GetDBPath();
        LoadAdvicesFromDB();
    }

    public void AddNewAdvice(Advice advice)
    {
        if(CheckAdviceExists(advice.AdviceId))
        {
            Debug.Log("This advice already exists in DB");
            return;
        }

        string text = advice.AdviceText.Replace("'", "''");
        string query = $"INSERT INTO {AdviceTableName} (id,advice) VALUES ('{advice.AdviceId}','{text}')";
        CurrentAdvices.Add(advice);
        ExecuteQuery(query);
        AddedToDB(advice);
    }

    public void DeleteAllData()
    {
        string query = $"DELETE  FROM {AdviceTableName}";
        CurrentAdvices.Clear();
        ExecuteQuery(query);
    }

    public void DeleteAdviceById(int id)
    {
        if(!CheckAdviceExists(id))
        {
            Debug.Log($"Advice with id = {id} does not exists in DB");
            return;
        }

        string query = $"DELETE  FROM {AdviceTableName} WHERE id = {id}";
        var advice = CurrentAdvices.Find(a => a.AdviceId == id);
        CurrentAdvices.Remove(advice);
        ExecuteQuery(query);
        DeletedFromDB(advice);
    }

    public Advice GetAdviceById(int id)
    {
        string query = $"SELECT * FROM {AdviceTableName} WHERE id = {id}";
        var table = GetTable(query);
        if (table.Rows.Count > 1)
        {
            var row = table.Rows[0];
            int adviceid = int.Parse(row[0].ToString());
            string adviceText = row[1].ToString();
            return new Advice(adviceid, adviceText);
        }
        else
        {
            return null;
        }
    }

    private bool CheckAdviceExists(int id)
    {
      return CurrentAdvices.Find(a => a.AdviceId == id) != null;
    }

    public void LoadAdvicesFromDB()
    {
        List<Advice> list = new List<Advice>();
        string query = $"SELECT * FROM {AdviceTableName}";
        var table = GetTable(query);

        var rows = table.Rows;

        for (int i = 0; i < rows.Count; i++)
        {
            int id = int.Parse(rows[i][0].ToString());
            string advice = rows[i][1].ToString();
            var adviceObj = new Advice(id, advice);
            CurrentAdvices.Add(adviceObj);
        }
    }

    private DataTable GetTable(string query)
    {
        OpenConnection();
        SqliteDataAdapter adapter = new SqliteDataAdapter(query, dbConnection);
        DataSet dataSet = new DataSet();
        adapter.Fill(dataSet);
        adapter.Dispose();
        CloseConnection();
        return dataSet.Tables[0];
    }

    private void ExecuteQuery(string query)
    {
        OpenConnection();
        cmd.CommandText = query;
        cmd.ExecuteNonQuery();
        CloseConnection();
        DatabaseUpdated();
    }

    private void OpenConnection()
    {
        dbConnection = new SqliteConnection("Data Source=" + dbPath);
        cmd = new SqliteCommand(dbConnection);
        dbConnection.Open();
    }

    private void CloseConnection()
    {
        dbConnection.Close();
        dbConnection.Dispose();
        dbConnection = null;
        cmd.Dispose();
        cmd = null;
    }

    private void UnpackDB(string toPath)
    {
        string path = Path.Combine(Application.streamingAssetsPath, DBName);
        WWW reader = new WWW(path);
        while (!reader.isDone) { }
        File.WriteAllBytes(toPath, reader.bytes);
    }
    private string GetDBPath()
    {
#if UNITY_EDITOR
        return Path.Combine(Application.streamingAssetsPath, DBName);
#endif

#if UNITY_ANDROID
        string filePath = Path.Combine(Application.persistentDataPath, DBName);
        if (!File.Exists(filePath)) UnpackDB(filePath);
        return filePath;
#endif

#if UNITY_STANDALONE
        string filePath = Path.Combine(Application.dataPath, DBName);
        if(!File.Exists(filePath)) 
            UnpackDB(filePath);
        return filePath;
#endif
    }
}
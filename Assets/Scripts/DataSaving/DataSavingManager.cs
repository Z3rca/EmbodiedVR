using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;  
using UnityEngine;

public class DataSavingManager : MonoBehaviour
{
    public static DataSavingManager Instance { get ; private set; }
    [SerializeField] private String SavePath;

    private string _participantId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        if (SavePath == "")
        {
            SavePath = Application.persistentDataPath;
        }
    }

    // Write arbitrary data to disk 
    public void WriteSerializableDataToDisk(object data, string filePath, bool prettyPrint=true, bool saveAsync=true)
    {
        // Check data is existent
        if (data is null)
        {
            Debug.Log("[FileIO] Not writing data at " + filePath + " to disk. No data.");
            return;
        }
    
        // Make sure directory for file exists 
        FileInfo fileInfo = new FileInfo(filePath);
        fileInfo.Directory.Create();
        
        // Serialize data 
        string serialized = JsonUtility.ToJson(data,prettyPrint);
        
        // Write data
        if (!saveAsync)
        {
            // Save waiting for finish 
            Debug.Log("[FileIO] Writing data non-async at " + filePath + " to disk.");
            File.WriteAllText(filePath, serialized);
            Debug.Log("[FileIO] Writing data finished.");
        }
        else
        {
            // Save Async 
            SaveData saveData = new SaveData();
            saveData.data = serialized;
            saveData.path = filePath;
            Thread saverThread = new Thread(SavingWorker);  
            saverThread.Start(saveData);
        }
        
        Debug.Log("hello from writer");
        
    }

    // Capsule for data to write to disk 
    private class SaveData
    {
        public string data;
        public string path;
    }
    
    // Used for threaded writing to disk 
    private void SavingWorker(object payload)
    {
        // Extract data 
        string data = ((SaveData) payload).data;
        string path = ((SaveData) payload).path;
        
        Debug.Log("[FileIO] Writing data async at " + path + " to disk.");
        File.WriteAllText(path, data);
        Debug.Log("[FileIO] Writing data finished.");
    }


    private List <string> ConvertToJson<T>(List<T> genericList)
    {
        List<string> list = new List<string>();
        //list.Add("[");
        foreach (var g in genericList)
        {
           // Debug.Log(g.ToString());
            string jsonString = JsonUtility.ToJson(g);
            list.Add(jsonString);
        }
        
        //list.Add("]");

        return list;
    }
    
    private string ConvertToJson<T>(T generic)
    {
        string json= JsonUtility.ToJson(generic);
    
        return json;
    }

    

    public List<T> LoadFileList<T>(string FileName)
    {
        string path = GetPathForSaveFile(FileName);
        List<T> genericList=new List<T>();

        if (File.Exists(path))
        {
            string[] data = File.ReadAllLines(path);
            foreach (var line in data)
            {
                T tmp= JsonUtility.FromJson<T>(line);
                genericList.Add(tmp);
            }
            return genericList;
        }
        else
        {
            throw new Exception("file not found " + path);
        }
    }
    
    public T LoadFile<T>(string DataName)
    {
        string path = GetPathForSaveFile(DataName);
        if (File.Exists(path))
        {
            string[] data = File.ReadAllLines(path);
            T tmp= JsonUtility.FromJson<T>(data[0]);
            return tmp;
        }
        else
        {
            throw new Exception("file not found");
        }
    }
    
    
    public void Save<T>(T file, string  fileName)
    {
        var data = ConvertToJson(file);

        string path = GetPathForSaveFile(fileName);
        
        FileStream fileStream= new FileStream(path, FileMode.Create);
        using (var fileWriter= new StreamWriter(fileStream))
        {
            fileWriter.WriteLine(data);
        }
        
        
        Debug.Log("saved  " +fileName + " to : " + SavePath );
    }
    
    //this prints data in a "pretty" format, so csv reader can interpret this better for single data frames. unfortunately, in this approach, data cant be load again, into unity since it corrupts the json format. Keep this in mind using this function
    //TODO potentially introduce another method for this
    public void SaveList<T>(List<T> file, string  fileName)         
    {
        var stringList = ConvertToJson(file);

        string path = GetPathForSaveFile(fileName);
        
        // I implemented the LoopAR Data saving, this time I got Access Violation.  I dont get why,  I needed a new File Stream Implementation 
        FileStream fileStream= new FileStream(path, FileMode.Create);
        using (var fileWriter= new StreamWriter(fileStream))
        {
            foreach (var line in stringList)
            {
                fileWriter.Write(line);
                fileWriter.Write(",");
                fileWriter.WriteLine();
            }
        }
        
        
        Debug.Log("saved  " +fileName + " to : " + SavePath );
    }
    
    private string GetPathForSaveFile(string fileName, string format=".json")
    {
        string name = fileName + format;
        // return Path.Combine(Application.persistentDataPath, name);
        return Path.Combine(SavePath, name);
    }

    public string GetSavePath()
    {
        return SavePath;
    }
    
}

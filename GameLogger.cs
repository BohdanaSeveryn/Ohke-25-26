namespace Ohke_25_26;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class GameLogger
{
    private readonly string folderPath = "28082025";
    private readonly string filePath;

    public GameLogger()
    {
        Directory.CreateDirectory(folderPath);
        filePath = Path.Combine(folderPath, "tulokset.json");
    }

    public void LogResult(string result)
    {
        var newEntry = new Tulos
        {
            Datestamp = DateTime.Today,
            GamesResult = result
        };

        List<Tulos> existingResults = new();

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            existingResults = JsonConvert.DeserializeObject<List<Tulos>>(json) ?? new List<Tulos>();
        }

        existingResults.Add(newEntry);

        string updatedJson = JsonConvert.SerializeObject(existingResults, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);
    }
}

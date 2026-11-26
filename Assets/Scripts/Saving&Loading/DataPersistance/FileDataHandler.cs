using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

internal enum SaveLoadResult
{
    Missing,
    Loaded,
    Failed
}

public class FileDataHandler
{
    readonly string dataDirPath;
    readonly string dataFileName;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    internal SaveLoadResult Load(out GameData loadedData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        loadedData = null;

        try
        {
            string json;
            try
            {
                json = File.ReadAllText(fullPath);
            }
            catch (FileNotFoundException)
            {
                return SaveLoadResult.Missing;
            }
            catch (DirectoryNotFoundException)
            {
                return SaveLoadResult.Missing;
            }

            ValidateSaveJson(json);
            GameData data = JsonUtility.FromJson<GameData>(json);
            if (data?.playerStats_Save == null || data.mapInfo_SaveList == null)
                throw new InvalidDataException("Required save data could not be deserialized.");

            loadedData = data;
            return SaveLoadResult.Loaded;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not read the save.\n" + fullPath + "\n" + e);
            return SaveLoadResult.Failed;
        }
    }

    private static void ValidateSaveJson(string json)
    {
        using var reader = new JsonTextReader(new StringReader(json)) { DateParseHandling = DateParseHandling.None };
        if (!reader.Read() || reader.TokenType != JsonToken.StartObject)
            throw new InvalidDataException("Expected a JSON save object.");

        // Check the original save fields so an empty object is never treated as a fresh save.
        bool hasPlayerStats = false;
        bool hasMapInfo = false;
        while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
        {
            string field = (string)reader.Value;
            if (!reader.Read())
                throw new InvalidDataException("The save ends before a field's value.");

            if (field == nameof(GameData.playerStats_Save))
                hasPlayerStats = reader.TokenType == JsonToken.StartObject;
            else if (field == nameof(GameData.mapInfo_SaveList))
                hasMapInfo = reader.TokenType == JsonToken.StartObject;

            reader.Skip();
        }

        if (reader.TokenType != JsonToken.EndObject || reader.Read() || !hasPlayerStats || !hasMapInfo)
            throw new InvalidDataException("The save is incomplete or has an unexpected format.");
    }

    internal bool Save(GameData data, bool replaceExisting)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        string temporaryPath = null;

        try
        {
            if (data?.playerStats_Save == null || data.mapInfo_SaveList == null)
                throw new InvalidDataException("Required save data is missing.");

            string dataToStore = JsonUtility.ToJson(data);
            if (string.IsNullOrEmpty(dataToStore))
                throw new InvalidDataException("Save data could not be serialized.");

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            temporaryPath = fullPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, leaveOpen: true))
                    writer.Write(dataToStore);

                stream.Flush(flushToDisk: true);
            }

            if (replaceExisting && File.Exists(fullPath))
                File.Replace(temporaryPath, fullPath, null);
            else
                File.Move(temporaryPath, fullPath);

            temporaryPath = null;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not write the save.\n" + fullPath + "\n" + e);
            return false;
        }
        finally
        {
            if (temporaryPath != null)
            {
                try
                {
                    File.Delete(temporaryPath);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Could not remove the temporary save file.\n" + temporaryPath + "\n" + e);
                }
            }
        }
    }
}

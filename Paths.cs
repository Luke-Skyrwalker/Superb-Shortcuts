using System.Reflection;
using System.Text.Json;
using System.IO;


public class Paths
{
    public class TilePaths // ToDo: Besserer Name
    {
        // public string PictureBoxName { get; set; }
        public string PicturePath { get; set; }
        public string ApplicationPath { get; set; }
    }

    private readonly string configDirectory;
    private readonly string configFilePath;

    public Paths()
	{
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        configDirectory = Path.Combine(documentsPath, "SuperbShortcuts");
        configFilePath = Path.Combine(configDirectory, "paths.json");
        EnsureDirectoryExists();
        ExtractBasicPic();
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists(configDirectory))
        {
            Directory.CreateDirectory(configDirectory);
        }
    }

    private void EnsureFileExists()
    {
        if (!File.Exists(configFilePath))
        {
            SaveTilePaths(new Dictionary<String, TilePaths>());
        }
    }

    // ToDo: Rückgabe Dict PbName -> AppEntry? So auch in Json darstellen?
    // Dictionary<PictureBox, AppEntry> ...
    public Dictionary<String, TilePaths> LoadTilePaths()
    {
        EnsureFileExists();

        try
        {
            string jsonContent = File.ReadAllText(configFilePath);
            var tps = JsonSerializer.Deserialize<Dictionary<String, TilePaths>>(jsonContent);
            return tps ?? new Dictionary<String, TilePaths>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Laden: {ex.Message}");
            return new Dictionary<String, TilePaths>();
        }
    }

    public void SaveTilePaths(Dictionary<String, TilePaths> tps)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            File.WriteAllText(configFilePath, JsonSerializer.Serialize(tps, options));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Speichern: {ex.Message}");
            throw;
        }
    }

    public void AddOrUpdateTilePath(string pbName, string picturePath, string applicationPath)
    {
        var tps = LoadTilePaths();
        TilePaths tp = new TilePaths
        {
            PicturePath = picturePath,
            ApplicationPath = applicationPath
        };
        if (tps.ContainsKey(pbName))
            tps[pbName] = tp;
        else tps.Add(pbName, tp);
        SaveTilePaths(tps);
    }

    public void UpdatePicturePath(string pbName, string picturePath)
    {
        var tps = LoadTilePaths();
        TilePaths? tp;
        if (tps.TryGetValue(pbName, out tp))
        {
            tp.PicturePath = picturePath;
            SaveTilePaths(tps);
        }
    }

    public void SwitchPbs(string pb1Name, string pb2Name)
    {
        var tps = LoadTilePaths();
        if (!(tps.ContainsKey(pb1Name) && tps.ContainsKey(pb1Name))) return;
        TilePaths tp1 = tps[pb1Name];
        tps[pb1Name] = tps[pb2Name];
        tps[pb2Name] = tp1;
        SaveTilePaths(tps);

    }

    public string GetBasicPicPath()
    {
        return configDirectory + "\\BasicPic.png";
    }

    public void ExtractBasicPic()
    {
        string resourceName = "BasicPic.png";
        string targetPath = Path.Combine(configDirectory, resourceName);

        if (File.Exists(targetPath))
        {
            return;
        }

        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullResourceName = $"Superb_Shortcuts.{resourceName}"; 

            using (Stream resourceStream = assembly.GetManifestResourceStream(fullResourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception($"Couldn't load resource: {fullResourceName}");
                }

                using (FileStream fileStream = File.Create(targetPath))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }

            Console.WriteLine($"Ressource erfolgreich extrahiert: {targetPath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Fehler beim Extrahieren der Ressource: {ex.Message}", ex);
        }
    }
}

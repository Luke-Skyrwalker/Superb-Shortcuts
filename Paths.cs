using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;


public class Paths
{

    private readonly string configDirectory;
    private readonly string configFilePath;

    public Paths()
	{
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        configDirectory = Path.Combine(documentsPath, "SuperbShortcuts");
        configFilePath = Path.Combine(configDirectory, "paths.json");
        if (!Directory.Exists(configDirectory)) Directory.CreateDirectory(configDirectory);
        ExtractBasicPic();
    }

    public Dictionary<String, String> LoadAppPaths()
    {
        if (!File.Exists(configFilePath)) SaveAppPaths(new Dictionary<String, String>());

        try
        {
            string jsonContent = File.ReadAllText(configFilePath);
            var tps = JsonSerializer.Deserialize<Dictionary<String, String>>(jsonContent);
            return tps ?? new Dictionary<String, String>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Laden: {ex.Message}");
            return new Dictionary<String, String>();
        }
    }

    public void SaveAppPaths(Dictionary<String, String> tps)
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

    // ToDo: Better name
    public void AddOrUpdateTile(string pbName, Bitmap pic, string applicationPath)
    {
        SavePicture(pbName, pic);
        // Add/Update path in json file
        var tps = LoadAppPaths();
        if (tps.ContainsKey(pbName))
            tps[pbName] = applicationPath;
        else tps.Add(pbName, applicationPath);
        SaveAppPaths(tps);
    }

    public void SavePicture(string name, Bitmap pic)
    {
        // Save picture to config Folder
        string picPath = GetPicPath(name);
        pic.Save(picPath, ImageFormat.Bmp);
        pic.Dispose();
    }

    public Image LoadPicture(string name)
    {
        string picPath = GetPicPath(name);
        if (!File.Exists(picPath)) picPath = GetPicPath("BasicPic");
        using (FileStream fs = new FileStream(picPath, FileMode.Open))
        {
            return Image.FromStream(fs);
        }
    }

    public void SwitchTiles(string pb1Name, string pb2Name)
    {
        var tps = LoadAppPaths();
        if (!(tps.ContainsKey(pb1Name) && tps.ContainsKey(pb1Name))) return;
        // switch application path
        string tp1 = tps[pb1Name];
        tps[pb1Name] = tps[pb2Name];
        tps[pb2Name] = tp1;
        SaveAppPaths(tps);
        // switch pictures
        File.Move(GetPicPath(pb1Name), GetPicPath("temp"));
        File.Move(GetPicPath(pb2Name), GetPicPath(pb1Name));
        File.Move(GetPicPath("temp"), GetPicPath(pb2Name));
    }

    private string GetPicPath(string name)
    {
        return Path.Combine(configDirectory, name + ".bmp");
    }

    private void ExtractBasicPic()
    {
        string resourceName = "BasicPic.bmp";
        string targetPath = GetPicPath("BasicPic");

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

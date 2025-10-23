
using IWshRuntimeLibrary;
using static Superb_Shortcuts.Shortcuts;


namespace Superb_Shortcuts
{
    // ToDo: better name
    internal class FileHandling
    {

        public static DropType GetDropTypeAndPath(DragEventArgs e, out string? dropFilepath)
        {
            dropFilepath = null;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 1)
            {
                dropFilepath = files[0];
                string ext = Path.GetExtension(files[0]).ToLower();
                return ext switch
                {
                    ".jpg" or ".jpeg" or ".png" or ".bmp" => DropType.Picture,
                    ".lnk" or ".exe" or ".url" => DropType.App,
                    _ => DropType.Invalid
                };                
            }
            return DropType.Invalid;
        }

        public static bool GetPicAndPath(out Bitmap? pic, out string? targetPath, string appPath, string? picturePath = null)
        {
            DropType dropType;
            pic = null;
            targetPath = null;
            try
            {
                targetPath = Path.GetExtension(appPath).ToLower() switch
                {
                    ".exe" => appPath,
                    ".lnk" => GetTargetPath(appPath),
                    ".url" => GetUrl(appPath)
                };

                if (targetPath == "")
                {
                    MessageBox.Show(
                        "This file isn't compatible",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return false;
                }

                if (picturePath == null)
                {
                    Icon? icon = Icon.ExtractAssociatedIcon(appPath);
                    if (icon == null) icon = Icon.ExtractAssociatedIcon(targetPath);
                    if (icon != null) pic = icon.ToBitmap();
                }
                else pic = new Bitmap(picturePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error handling file:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            return false;
        }

        private static string GetTargetPath(string lnkPath)
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(lnkPath);
            return shortcut.TargetPath;
        }

        private static string GetUrl(string urlPath)
        {
            var lines = System.IO.File.ReadAllLines(urlPath);

            foreach (var line in lines)
            {
                if (line.StartsWith("URL"))
                {
                    return line.Substring(4).Trim();
                }
            }

            return null; // ToDo: Is this case possible? Maybe work with returning bool / out argument
        }
    }
}

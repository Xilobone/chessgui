using System.Text.Json;

namespace gui
{
    public class GUISettings
    {
        public static GUISettings SETTINGS { get => GetSettings(); private set => SETTINGS = value; }
        private static GUISettings? _SETTINGS;

        private static string basePath = $"{AppDomain.CurrentDomain.BaseDirectory}/../../..";


        public string lightColor { get; set; }
        public string darkColor { get; set; }
        public bool showFilesAndRanks { get; set; }
        public GUISettings()
        {
            lightColor = "";
            darkColor = "";
            showFilesAndRanks = false;
        }

        private static GUISettings GetSettings()
        {
            if (_SETTINGS != null) return _SETTINGS;

            string[] str = File.ReadAllLines($"{basePath}/lib/gui_settings.json");

            string json = "";

            foreach (string s in str)
            {
                json += s;
            }

            _SETTINGS = JsonSerializer.Deserialize<GUISettings>(json)!;

            return _SETTINGS!;
        }

        public static void writeAsDefault(GUISettings settings)
        {
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllLines($"{basePath}/lib/gui_settings.json", [json]);
        }
    }
}
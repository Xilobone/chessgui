using System.Text.Json;

namespace gui
{   
    /// <summary>
    /// Class containing all settings of the gui window, all settings are stored in the /lib/gui_settings.json file
    /// </summary>
    public class GUISettings
    {   
        /// <summary>
        /// The global settings object
        /// </summary>
        public static GUISettings SETTINGS { get => GetSettings(); private set => SETTINGS = value; }
        private static GUISettings? _SETTINGS;

        private static string basePath = $"{AppDomain.CurrentDomain.BaseDirectory}/../../..";

        /// <summary>
        /// The hex value of the light colored squares of the chess board
        /// </summary>
        public string lightColor { get; set; }

        /// <summary>
        /// The hex value of the dark colored squares of the chess board
        /// </summary>
        public string darkColor { get; set; }

        /// <summary>
        /// True if the files and ranks are shown, false if they are not shown
        /// </summary>
        public bool showFilesAndRanks { get; set; }

        /// <summary>
        /// Creates a new gui settings object
        /// </summary>
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

        /// <summary>
        /// Writes the given settings to the /lib/gui_settings.json file
        /// </summary>
        /// <param name="settings">The settings to write</param>
        public static void writeAsDefault(GUISettings settings)
        {
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllLines($"{basePath}/lib/gui_settings.json", [json]);
        }
    }
}
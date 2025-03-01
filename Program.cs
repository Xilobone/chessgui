using gui;

/// <summary>
/// Entrypoint of the application
/// </summary>
public class Progam
{   
    /// <summary>
    /// Starts the program, creates a new gui window
    /// </summary>
    /// <param name="args">Arguments passed to the application, unused</param>
    [STAThread]
    public static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new GUI());

    }
}
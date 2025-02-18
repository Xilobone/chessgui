using System.Diagnostics;
using gui;
public class Progam
{
    [STAThread]
    public static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new GUI());

        // new OpenFileDialog().ShowDialog();
    }
}
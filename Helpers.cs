using System;
using System.Diagnostics;
using System.IO;
using static batman.Constants;

namespace batman;

public static class Helpers
{
    public static bool IsInstalled()
    {
        return File.Exists(SERVICE_FILE);
    }

    public static bool IsRoot()
    {        
        ProcessStartInfo psi = new ProcessStartInfo("id", "-u"); // Return 0 if root and any other number if not root
        psi.RedirectStandardOutput = true;
        psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        psi.UseShellExecute = false;
        Process reg;
        reg = System.Diagnostics.Process.Start(psi) ?? throw new Exception("Unable to check if running as root!");
        using (StreamReader stream = reg.StandardOutput)
        {
            string output = stream.ReadToEnd();
            if(output.Trim() == "0")
                return true;
        }
        return false;       
    }
    
    public enum BatMode
    {
        Full = 100,
        Half = 60,
        Balanced = 80
    }
}
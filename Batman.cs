using System.Diagnostics;
using Cocona;

public class Batman
{

    const string BAT_CHARGE_END_THRESHOLD_FILE = "/sys/class/power_supply/BAT0/charge_control_end_threshold";

    const string BAT_END_THRESHOLD_CONFIG_FILE = "/etc/max_charge";

    [Command("cap")]
    public void ShowCurrentCapacity()
    {
        int current;
        int persistent;

        current = int.Parse(File.ReadAllText(BAT_CHARGE_END_THRESHOLD_FILE));

        if(File.Exists(BAT_END_THRESHOLD_CONFIG_FILE))
        {
            persistent = int.Parse(File.ReadAllText(BAT_END_THRESHOLD_CONFIG_FILE));

            if(persistent != current)
            {
                System.Console.WriteLine($"For this session the battery will charge upto {current}% \n" + 
                $"After a reboot it will stop charging when it reaches {persistent}%");
                return;
            }
        }

        System.Console.WriteLine($"Battery will stop charging when it reaches {current}%");
    }



    [Command("set-mode")]
    public void SetMode([Argument]BatMode mode,
    [Option(Description = "Forget the newly set mode after a reboot")]bool noPersist)
    {
        int newcap = (int)mode;

        if(!IsRoot())
        {
            System.Console.WriteLine("This program should be run as root to chage current battery charge mode");
            return;
        }
        
        if(!noPersist)
        {
            File.WriteAllText(BAT_END_THRESHOLD_CONFIG_FILE, newcap + "\n");            
        }

        File.WriteAllText(BAT_CHARGE_END_THRESHOLD_FILE, newcap + "\n");
    }

    public bool IsRoot()
    {        
        ProcessStartInfo psi = new ProcessStartInfo("id", "-u"); // Return 0 if root and any other number if not root

        psi.RedirectStandardOutput = true;
        psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        psi.UseShellExecute = false;
        System.Diagnostics.Process reg;
        reg = System.Diagnostics.Process.Start(psi) ?? throw new Exception("Unable to check if running as root!");



        using (System.IO.StreamReader stream = reg.StandardOutput)
        {
            string output = stream.ReadToEnd();

            if(output.Trim() == "0")
            return true;
        }

        return false;       

    }

    public enum BatMode  :int
    {
        Full = 100,
        Half = 60,
        Balanced = 80
    }
}



using System;
using System.Diagnostics;
using System.IO;
using Cocona;
using static batman.Helpers;
using static batman.Constants;

public class Batman
{
    [Command("cap")]
    public void ShowCurrentCapacity()
    {
        var current = int.Parse(File.ReadAllText(BAT_CHARGE_END_THRESHOLD_FILE));
        if(File.Exists(BAT_END_THRESHOLD_CONFIG_FILE))
        {
            var persistent = int.Parse(File.ReadAllText(BAT_END_THRESHOLD_CONFIG_FILE));
            if(persistent != current)
            {
                Console.WriteLine($"For this session the battery will charge upto {current}% \n" + 
                $"After a reboot it will stop charging when it reaches {persistent}%");
                return;
            }
        }
        Console.WriteLine($"Battery will stop charging when it reaches {current}%");
    }

    [Command("set-mode")]
    public void SetMode([Argument]BatMode mode,
    [Option(Description = "Forget the newly set mode after a reboot")]bool noPersist)
    {
        int newcap = (int)mode;
        if(!IsRoot())
        {
            Console.WriteLine("This program should be run as root to change current battery charge mode");
            return;
        }
        if(!noPersist)
        {
            if(!IsInstalled())
            {
                Console.WriteLine("Service not installed. Settings will not persist across reboots");
                Console.WriteLine("To install the service, use 'batman install'");
            }
            else
                File.WriteAllText(BAT_END_THRESHOLD_CONFIG_FILE, newcap + "\n");            
        }
        File.WriteAllText(BAT_CHARGE_END_THRESHOLD_FILE, newcap + "\n");
    }


    [Command("install")]
    public void Install(bool noConfirm = false)
    {
        if (!IsRoot())
        {
            Console.WriteLine("This program should be run as root to install the systemd service");
            return;
        }
        
        if(IsInstalled())
        {
            Console.WriteLine("Service already installed. To reinstall, first uninstall the service.");
            return;
        }
        
        if(!noConfirm)
        {
            Console.WriteLine("This will install a systemd service that will set the battery charge limit at boot. Do you want to continue? [y/N]");
            var response = Console.ReadLine();
            if(response?.ToLower() != "y")
            {
                Console.WriteLine("Aborted");
                return;
            }
        }
        
        Console.WriteLine($"Installing service file to {SERVICE_FILE}");
        File.WriteAllText(SERVICE_FILE, SERVICE_FILE_CONTENT);
        Console.WriteLine("Service file installed");
        Console.WriteLine("Use 'systemctl enable batman --now' to enable and run the service");
    }
    
    [Command("uninstall")]
    public void Uninstall()
    {
        if (!IsRoot())
        {
            Console.WriteLine("This program should be run as root to uninstall the systemd service");
            return;
        }
        if(!IsInstalled())
        {
            Console.WriteLine("Service not installed. Nothing to do.");
            return;
        }
        
        Console.WriteLine("Disabling service");
        Process.Start("systemctl", "disable batman").WaitForExit(TimeSpan.FromMinutes(1));
        Console.WriteLine("Removing service file");
        File.Delete(SERVICE_FILE);
    }
}



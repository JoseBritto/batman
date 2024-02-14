namespace batman;

public static class Constants
{
    public const string BAT_CHARGE_END_THRESHOLD_FILE = "/sys/class/power_supply/BAT0/charge_control_end_threshold";
    public const string BAT_END_THRESHOLD_CONFIG_FILE = "/etc/max_charge";
    public const string SERVICE_FILE = "/etc/systemd/system/batman.service";
    
    public const string SERVICE_FILE_CONTENT = 
        """
        [Unit]
        Description=Set the battery charge threshold
        After=multi-user.target suspend.target hibernate.target hybrid-sleep.target suspend-then-hibernate.target
        StartLimitBurst=0
        
        [Service]
        Type=oneshot
        Restart=on-failure
        ExecStart=/bin/bash -c 'cat /etc/max_charge > /sys/class/power_supply/BAT0/charge_control_end_threshold'
        
        [Install]
        WantedBy=multi-user.target suspend.target hibernate.target hybrid-sleep.target suspend-then-hibernate.target
        """;
}
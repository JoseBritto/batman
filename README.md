# Batman
## Simple Battery Manager application for linux
This was orginally written for ASUS TUF FX505GD laptops, 
but it should work on any similar linux laptops.

### Check if your laptop is supported
```bash
$ cat /sys/class/power_supply/BAT0/charge_control_end_threshold
```
If this command returns a value, then your laptop is probably supported.

> ⚠️ Warning: This application is not tested on other laptops.

**Note:** This was written for personal use, so it may not work on your laptop. **Please use it at your own risk.**

### Build Instructions

#### Prerequisites
- .NET 8.0.x SDK

```bash
$ git clone 'https://github.com/JoseBritto/batman.git'
$ cd batman
$ dotnet publish -o build
```

### Usage

#### To install the systemd service
```bash
$ cd build
$ sudo ./batman install
```

#### To uninstall the systemd service
```bash
$ cd build
$ sudo ./batman uninstall
```
I recommend moving the `batman` binary to a folder in your `$PATH` so that you can run it from anywhere.


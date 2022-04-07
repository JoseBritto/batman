using Cocona;

CoconaApp.Run<Batman>(args, options =>
{
    // If the option value is `false`, All command methods require `CommandAttribute`.
    options.TreatPublicMethodsAsCommands = false;
});


// App complete! Now document and build into a single executable. (Also update the systemd service script)
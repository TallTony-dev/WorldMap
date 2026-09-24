using System;
using System.Text;

public struct DeviceCommand
{
    public DeviceCommandType CommandName { get; set; }
    public string[] Args {  get; set; }

    public DeviceCommand(DeviceCommandType commandName, string[] args)
    { 
        CommandName = commandName;
        Args = args;
    }
}

public enum DeviceCommandType
{
    GetImageFromDirection,
    MoveInDirectionUntilStopped,
    StopMovement,
    MoveInDirectionForDuration,
    MoveInDirectionUntilSense
}
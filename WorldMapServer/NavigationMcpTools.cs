using DriverLib;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace WorldMapServer;

[McpServerToolType]
public class NavigationMcpTools(IRobot robot)
{
    private IRobot _robot = robot;


    [McpServerTool, Description("Reasons about navigating to an area as given by a name by calling an agent framework.")]
    public void NavigateToAreaByName(string areaName)
    {
        
    }

}

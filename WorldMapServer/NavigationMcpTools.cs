using DriverLib;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace WorldMapServer;

[McpServerToolType]
public class NavigationMcpTools(IRobot robot)
{
    private IRobot _robot = robot;


    [McpServerTool, Description("t")]
    public void Thing()
    {
        
    }

}

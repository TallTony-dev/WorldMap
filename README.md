Structure:

Overarching Hermes agent has the RobotServer mcp server api exposed to it, DriverLib contains MAF agents using Gemma 4 models that preform certain actions:

- Preform certain open form actions called from mcp server using tools (move to area by name, etc) 
- Maintain the WorldGraph structure and keep it clean

The MAF agents will preform those actions using tools to interact with the WorldGraph and the robot drivers
using RoboFactory.Models;
using RoboFactory.Utils;

namespace RoboFactory.Factories;

public class IndustrialFactory : IRobotFactory
{
    public Robot CreateRobot(Core core, Generator generator, Arms arms, Legs legs)
    {
        return new Robot(ECategory.Industrial, core, generator, arms, legs);
    }
}
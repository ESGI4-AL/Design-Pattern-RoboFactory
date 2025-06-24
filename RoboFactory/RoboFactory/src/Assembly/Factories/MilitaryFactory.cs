using RoboFactory.Models;
using RoboFactory.Utils;

namespace RoboFactory.Factories;

public class MilitaryFactory : IRobotFactory
{
    public Robot CreateRobot(Core core, Generator generator, Arms arms, Legs legs)
    {
        return new Robot(ECategory.Military, core, generator, arms, legs);
    }
}
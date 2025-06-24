using RoboFactory.Models;
using RoboFactory.Utils;

namespace RoboFactory.Factories;

public class DomesticFactory: IRobotFactory
{
    public Robot CreateRobot(Core core, Generator generator, Arms arms, Legs legs)
    {
        return new Robot(ECategory.Domestic, core, generator, arms, legs);
    }
}
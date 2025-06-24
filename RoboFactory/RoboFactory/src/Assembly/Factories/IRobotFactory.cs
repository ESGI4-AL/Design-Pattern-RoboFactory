using RoboFactory.Models;

namespace RoboFactory.Factories;

public interface IRobotFactory
{
    public Robot CreateRobot(Core core, Generator generator, Arms arms, Legs legs);
}
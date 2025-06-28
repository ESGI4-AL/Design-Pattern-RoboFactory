using RoboFactory.Models;

namespace RoboFactory;

public class RobotBuilder
{
    private Robot _robot;
    private ECategory Category;
    
    public RobotBuilder(ECategory category)
    {
        Category = category;
        Reset();
    }

    private void Reset()
    {
        _robot = new Robot(Category);
    }

    public void SetCore(Core core)
    {
        _robot.core = core;
    }

    public void SetGenerator(Generator generator)
    {
        _robot.generator = generator;
    }

    public void SetArms(Arms arms)
    {
        _robot.arms = arms;
    }

    public void SetLegs(Legs legs)
    {
        _robot.legs = legs;
    }

    public Robot Build()
    {
        return _robot;
    }
}
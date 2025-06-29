namespace RoboFactory;

public record RobotTemplate(
        string name,
        ECategory robotCategory,
        ECategory systemCategory,
        ECategory coreCategory,
        ECategory generatorCategory,
        ECategory armsCategory,
        ECategory legsCategory
    ) { }
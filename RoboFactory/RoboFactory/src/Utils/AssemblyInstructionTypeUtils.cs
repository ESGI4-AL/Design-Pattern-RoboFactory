namespace RoboFactory.Utils;

public static class AssemblyInstructionTypeUtils
{
    public static AssemblyInstructionType FromStringToEnum(string instructionType)
    {
        if (string.IsNullOrWhiteSpace(instructionType))
            throw new ArgumentException("Instruction type cannot be empty");

        return instructionType.ToUpper() switch
        {
            "PRODUCING" => AssemblyInstructionType.Producing,
            "GET_OUT_STOCK" => AssemblyInstructionType.GetOutStock,
            "INSTALL" => AssemblyInstructionType.Install,
            "ASSEMBLE" => AssemblyInstructionType.Assemble,
            "FINISHED" => AssemblyInstructionType.Finished,
            _ => throw new ArgumentException($"Unknown instruction type: {instructionType}")
        };
    }
    
    public static string ToString(AssemblyInstructionType instructionType)
    {
        return instructionType switch
        {
            AssemblyInstructionType.Producing => "PRODUCING",
            AssemblyInstructionType.GetOutStock => "GET_OUT_STOCK",
            AssemblyInstructionType.Install => "INSTALL",
            AssemblyInstructionType.Assemble => "ASSEMBLE",
            AssemblyInstructionType.Finished => "FINISHED",
            _ => throw new ArgumentOutOfRangeException(nameof(instructionType))
        };
    }
}

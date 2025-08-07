using System.Reflection;

namespace WarehouseManagement.OutcomeProcessing.Contracts;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
using System.Reflection;

namespace WarehouseManagement.IncomeProcessing.Contracts;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
using System.Diagnostics.CodeAnalysis;

namespace LBHTenancyAPI.Models
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    public class Test
    {
        public int Id { get; }
        public string Name { get; }
    }
}

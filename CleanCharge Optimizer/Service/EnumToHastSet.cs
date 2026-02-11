using CleanCharge_Optimizer.Enum;
using System.Collections.Generic;
using System.Linq;

namespace CleanCharge_Optimizer.Service
{
    public static class CleanEnergyHelper
    {
        public static HashSet<string> GetCleanEnergySources()
        {
            return System.Enum.GetNames(typeof(CleanEnergySource))
                              .Select(name => name.ToLower())
                              .ToHashSet();
        }
    }
}
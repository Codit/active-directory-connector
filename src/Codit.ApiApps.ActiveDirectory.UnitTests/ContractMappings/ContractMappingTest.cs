using Codit.ApiApps.ActiveDirectory.Contracts;
using NUnit.Framework;

namespace Codit.ApiApps.ActiveDirectory.UnitTests.ContractMappings
{
    [Category("Unit")]
    public class ContractMappingTest
    {
        public ContractMappingTest()
        {
            ContractMapping.Setup();
        }
    }
}
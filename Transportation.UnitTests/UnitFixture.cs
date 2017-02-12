using Conjurer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.UnitTests
{
    [TestFixture(Category = "Unit Test")]
    public abstract class UnitFixture
    {
        static UnitFixture()
        {
            ConjurerDefinitions.Intialize();
        }
    }
}

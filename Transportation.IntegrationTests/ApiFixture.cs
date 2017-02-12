using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.IntegrationTests
{
    public abstract class ApiFixture : IntegrationFixture
    {
        protected ApiHelper Odata;
        protected string Url;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Odata = new ApiHelper();
            Url = "http://localhost.:9090/odata/";
        }
    }
}

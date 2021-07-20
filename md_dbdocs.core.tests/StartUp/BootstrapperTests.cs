using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace md_dbdocs.core.tests.StartUp
{
    public class BootstrapperTests
    {
        [Fact]
        public void ShouldReturnContainer()
        {
            var b = new md_dbdocs.core.StartUp.Bootstrapper();

            var c = b.BootStrap();

            Assert.NotNull(c);
        }
    }
}

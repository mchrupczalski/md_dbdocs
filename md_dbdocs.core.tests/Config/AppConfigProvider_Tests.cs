using md_dbdocs.core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace md_dbdocs.core.tests.Config
{
    public class AppConfigProvider_Tests
    {
        [Fact]
        public void SaveFile_ShouldCreateFile()
        {
            // arrange
            string expected = "222";

            // act
            AppConfigProvider.LoadFromFile();

            // assert
            //Assert.Equal(expected, AppConfigProvider.LoadFromFile());
        }
    }
}

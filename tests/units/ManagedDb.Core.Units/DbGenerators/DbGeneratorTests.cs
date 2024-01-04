using ManagedDb.Core.Features.GenerateDbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Core.Units.DbGenerators
{
    [TestClass]
    public class DbGeneratorTests
    {
        [TestMethod]
        public void GenerateDb_Should_Work() 
        {
            var pathToDataFolder = @"D:\Repositories\ManagedDb\Core\data";
            var dbExportPath = @"D:\Repositories\ManagedDb\Core\data\mdb.db";

            var dbGenerator = new DbGenerator();
            dbGenerator.Generate(pathToDataFolder, dbExportPath);
        }
    }
}

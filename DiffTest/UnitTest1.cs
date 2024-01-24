using DifferentiatingApplication.Domain;
using DifferentiatingApplication.DTO;
using DifferentiatingApplication.Persistence;
using DifferentiatingApplication.Services;
using Microsoft.EntityFrameworkCore;

namespace DiffUnitTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void GetDiff_DifferentData_ReturnsContentDoNotMatch()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DiffDbContext>()
            .UseInMemoryDatabase(databaseName: "DiffDbForTesting" + Guid.NewGuid())
            .Options;

        using (var context = new DiffDbContext(options))
        {
            context.DataRecords.Add(new DataRecord { Id = 1, Side = "left", Data = Convert.FromBase64String("AAAAAA==") });
            context.DataRecords.Add(new DataRecord { Id = 1, Side = "right", Data = Convert.FromBase64String("AQABAQ==") });
            context.SaveChanges();
        }

        using (var context = new DiffDbContext(options))
        {
            var service = new DiffService(context);

            // Act
            var result = service.GetDiff(1);

            // Assert
            Assert.IsNotNull(result);
            var diffResultTypeProperty = result.GetType().GetProperty("DiffResultType").GetValue(result, null);
            Assert.AreEqual("ContentDoNotMatch", diffResultTypeProperty);

            var diffsProperty = result.GetType().GetProperty("Diffs").GetValue(result, null) as List<DiffDetail>;
            Assert.IsNotNull(diffsProperty);
            Assert.IsTrue(diffsProperty.Count > 0);
        }
    }

}

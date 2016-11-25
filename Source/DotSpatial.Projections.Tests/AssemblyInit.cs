using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    [SetUpFixture]
    public class AssemblyInit
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            GridShift.InitializeExternalGrids("GeogTransformGrids", false);
        }
    }
}


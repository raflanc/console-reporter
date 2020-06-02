using Importer.Models;
using System;
using Xunit;

namespace ImporterTests.Models
{
    public class WarehouseTests
    {
        [Fact]
        public void NewWarehouseIsEmpty()
        {
            var war = new Warehouse("test");
            Assert.Equal(0, war.Total);
        }

        [Fact]
        public void AddMaterialToWarehouse()
        {
            var war = new Warehouse("test");
            var id = Guid.NewGuid().ToString();
            var mat = new Material(id, "Sample material", 25);

            war.AddMaterial(mat);
            Assert.Equal(25, war.Total);
        }

        [Fact]
        public void AddMaterialZeroQuantityToWarehouse()
        {
            var war = new Warehouse("test");
            var id = Guid.NewGuid().ToString();
            var mat = new Material(id, "Sample material", -1);

            Assert.Throws<ArgumentException>(() => war.AddMaterial(mat));
        }

        [Fact]
        public void AddMultipleMaterialToWarehouse()
        {
            var war = new Warehouse("test");
            var id = Guid.NewGuid().ToString();
            var mat = new Material(id, "Sample material", 25);
            var id2 = Guid.NewGuid().ToString();
            var mat2 = new Material(id2, "Sample material 2", 75);

            war.AddMaterial(mat);
            war.AddMaterial(mat2);
            Assert.Equal(100, war.Total);
        }
    }
}

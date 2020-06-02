namespace Importer.Models
{
    public class Material
    {
        public Material(string materialId, string materialName, int quantity)
        {
            MaterialId = materialId;
            MaterialName = materialName;
            Quantity = quantity;
        }

        public string MaterialId { get; private set; }

        public string MaterialName { get; private set; }

        public int Quantity { get; private set; }
    }
}

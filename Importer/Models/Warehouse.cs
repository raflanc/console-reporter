using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Importer.Models
{
    public class Warehouse
    {
        public string WarehouseId { get; private set; }

        public int Total { get; private set; }

        public ReadOnlyCollection<Material> Materials
        {
            get
            {
                return _materials.AsReadOnly();
            }
        }

        private List<Material> _materials { get; set; }

        public Warehouse(string warehouseId)
        {
            WarehouseId = warehouseId;
            _materials = new List<Material>();
            Total = 0;
        }
               

        public void AddMaterial(Material mat)
        {
            Debug.Assert(mat != null, "Cannot add null material");
            if (mat.Quantity < 1)
            {
                throw new ArgumentException("Cannot add material with 0 or negative quantity", "mat");
            }

            Total += mat.Quantity;
            _materials.Add(mat);
        }
    }
}

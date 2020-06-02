using Importer.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.Controllers
{
    public class ReportGenerator
    {
        private InputFileParser _parser;

        public ReportGenerator(InputFileParser parser)
        {
            _parser = parser;
        }

        public void SampleReport(Stream outputStream)
        {
            using (StreamWriter sw = new StreamWriter(outputStream))
            {
                var warehouses = _parser.Inventory
                    .OrderByDescending(war => war.Total)
                    .ThenByDescending(war => war.WarehouseId);

                foreach (var warehouse in warehouses)
                {
                    sw.WriteLine($"{warehouse.WarehouseId} (total {warehouse.Total})");

                    var materials = warehouse.Materials.OrderBy(mat => mat.MaterialId);
                    foreach (var material in materials)
                    {
                        sw.WriteLine($"{material.MaterialId}: {material.Quantity}");
                    }

                    sw.WriteLine();
                }

            }
        }
    }
}

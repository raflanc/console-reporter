using Importer.Models;
using System.Collections.Generic;
using System.IO;

namespace Importer.Controller
{
    public class InputFileParser
    {
        private Stream _inputStream { get; set; }

        private Dictionary<string, Warehouse> _inventory = new Dictionary<string, Warehouse>();

        private long _lineNumber;

        public InputFileParser(Stream stream)
        {
            _inputStream = stream;
            ParseErrors = new List<string>();
        }

        public bool HasErrors 
        { 
            get
            {
                return ParseErrors.Count > 0;
            }
        }

        public List<string> ParseErrors { get; private set; }

        public IEnumerable<Warehouse> Inventory 
        { 
            get 
            {
                return _inventory.Values;
            }
        }

        public void ParseFile()
        {
            StreamReader sr = new StreamReader(_inputStream);

            string line;
            _lineNumber = 1;
            while((line = sr.ReadLine()) != null)
            {
                ParseLine(line.Trim());
                _lineNumber++;
            }
        }

        private void ParseLine(string line)
        {
            if (line.StartsWith("#") || string.IsNullOrEmpty(line.Trim()))
            {
                return;
            }

            var fields = line.Split(';');

            if (fields.Length != 3)
            {
                ParseErrors.Add($"ParseError line {_lineNumber} missing or too many [;] ");
                return;
            }

            var materialName = fields[0];
            var materialId = fields[1];

            ParseMaterial(materialId, materialName, fields[2]);
        }

        private void ParseMaterial(string materialId, string materialName, string warehouseStock)
        {
            var warehouses = warehouseStock.Split('|');

            foreach(var warehouse in warehouses)
            {
                ParseSingleWarehouse(materialId, materialName, warehouse);
            }
        }

        private void ParseSingleWarehouse(string materialId, string materialName, string warehouse)
        {
            var stock = warehouse.Split(',');

            if (stock.Length != 2)
            {
                ParseErrors.Add($"ParseError line {_lineNumber} malformed warehouse stock (not in format [Name,Quantity]");
                return;
            }

            var warehouseId = stock[0];
            int quantity;
            if (!int.TryParse(stock[1], out quantity))
            {
                ParseErrors.Add($"ParseError line {_lineNumber} quantity is not a number");
                return;
            }

            if (quantity < 1)
            {
                ParseErrors.Add($"ParseError line {_lineNumber} quantity cannot be zero");
                return;
            }

            AddStock(warehouseId, materialId, materialName, quantity);
        }

        private void AddStock(string warehouseId, string materialId, string materialName, int quantity)
        {
            Warehouse warehouse;
            if(!_inventory.TryGetValue(warehouseId, out warehouse))
            {
                warehouse = new Warehouse(warehouseId);
                _inventory[warehouseId] = warehouse;
            }

            warehouse.AddMaterial(new Material(materialId, materialName, quantity));
        }
    }
}

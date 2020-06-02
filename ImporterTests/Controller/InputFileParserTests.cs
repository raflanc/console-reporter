using Importer.Controller;
using Importer.Models;
using System.IO;
using Xunit;

namespace ImporterTests.Controller
{
    public class InputFileParserTests
    {
        public InputFileParserTests()
        {
        }

        [Fact]
        public void SimpleScenario()
        {
            string sample = @"
                    # Test comment
                    Test material 1;TST001;W01,10|W02,25|W03,99
                ";

            InputFileParser parser = new InputFileParser(GenerateStreamFromString(sample));
            parser.ParseFile();
            Assert.False(parser.HasErrors);
            
            Assert.Collection<Warehouse>(parser.Inventory, item => Assert.Equal(10,item.Total),
                                                           item => Assert.Equal(25, item.Total),
                                                           item => Assert.Equal(99, item.Total));
        }

        [Fact]
        public void ManyLines()
        {
            string sample = @"
                    # Test comment
                    Test material 1;TST001;W01,10|W02,25|W03,99
                    Test material 2;TST001;W01,20|W02,50|W03,198
                ";

            InputFileParser parser = new InputFileParser(GenerateStreamFromString(sample));
            parser.ParseFile();
            Assert.False(parser.HasErrors);

            Assert.Collection<Warehouse>(parser.Inventory, item => Assert.Equal(30, item.Total),
                                                           item => Assert.Equal(75, item.Total),
                                                           item => Assert.Equal(297, item.Total));
        }

        [Fact]
        public void DifferentLengths()
        {
            string sample = @"
                    # Test comment
                    Test material 1;TST001;W01,10
                    Test material 2;TST001;W02,50|W03,198
                ";

            InputFileParser parser = new InputFileParser(GenerateStreamFromString(sample));
            parser.ParseFile();
            Assert.False(parser.HasErrors);

            Assert.Collection<Warehouse>(parser.Inventory, item => Assert.Equal(10, item.Total),
                                                           item => Assert.Equal(50, item.Total),
                                                           item => Assert.Equal(198, item.Total));
        }

        [Fact]
        public void BrokenFileFormatMissingWarehouse()
        {
            string sample = @"
                    # Test comment
                    Test material 1;TST001W01,10
                ";

            InputFileParser parser = new InputFileParser(GenerateStreamFromString(sample));
            parser.ParseFile();
            Assert.True(parser.HasErrors);
        }

        [Fact]
        public void BrokenFileFormatMissingQuantity()
        {
            string sample = @"
                    # Test comment
                    Test material 1;TST001;W01
                ";

            InputFileParser parser = new InputFileParser(GenerateStreamFromString(sample));
            parser.ParseFile();
            Assert.True(parser.HasErrors);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}

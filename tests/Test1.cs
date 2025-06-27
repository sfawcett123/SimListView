using System.Diagnostics;

namespace tests
{
    
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestLoadTestFile()
        {
            SimListView.SimListView listView = new SimListView.SimListView();
            listView.load( Path.Combine( "." , "TestData", "TestData1.yaml"));

            Assert.IsNotNull(listView);

            Assert.IsTrue(listView.Columns.Count > 0, "Expected at least one column to be created.");

            Assert.IsTrue(listView.Items.Count > 0, "Expected at least one item to be created.");

            foreach (System.Windows.Forms.ColumnHeader column in listView.Columns)
            {
                Assert.IsFalse(string.IsNullOrEmpty(column.Text), "Column header text should not be empty.");
            }

            var Values =listView.to_string();
            Assert.IsNotNull(Values, "Expected to_string() to return a non-null list.");

            Assert.IsTrue(Values.FirstOrDefault()?.Contains("ENG N1 RPM") ?? false, $"Expected the first item to contain 'Id'. {Values.FirstOrDefault<string>()}");
        }
    }
}

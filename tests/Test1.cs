using System.Diagnostics;

namespace tests
{
    
    [TestClass]
    public sealed class TestListView
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
        }



        [DataTestMethod]
        [DataRow("ENG N1 RPM")]
        public void TestSetValue( string measure )
        {
            var value = "1000";

            SimListView.SimListView listView = new SimListView.SimListView();
            listView.load(Path.Combine(".", "TestData", "TestData1.yaml"));

            Assert.IsTrue( listView.setValue( measure , value ), $"Able to set {measure} to {value}" );
        }

        [DataTestMethod]
        [DataRow("ENG N1 RPM" ,1)]
        [DataRow("ENG N1 RPM", 2)]
        public void TestSetIndexValue(string measure , int index)
        {
            var value = "1000";
            var value2 = "2000";
  
            SimListView.SimListView listView = new SimListView.SimListView();
            listView.load(Path.Combine(".", "TestData", "TestData1.yaml"));

            Assert.IsTrue(listView.setValue(measure, index, value), $"Able to set {measure}[{index}] to {value}");

            Assert.IsTrue(listView.setValue(measure, index, value2), $"Able to set {measure}[{index}] to {value2}");

            value = listView.getValue(measure, index).ToString();

            Assert.AreEqual(value2, value, $"Expected {measure}[{index}] to be {value2}, but got {value}.");
        }
    }
}

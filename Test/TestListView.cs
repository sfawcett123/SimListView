using SimListView;

namespace SimListView.Test
{
    [TestClass]
    public class TestSimListView
    {
        [TestMethod]
        public void InitializeControl()
        {
            SimListView listView = new SimListView();
            Assert.IsNotNull(listView, "SimListView should not be null after instantiation.");
            Assert.AreEqual( listView.Name == "SimulatorData", true, "Name should be 'SimulatorData'.");
        }

        [TestMethod]
        public void LoadYamlFile()
        {
            SimListView listView = new SimListView();
            string filePath = "testdata/test_1.yaml"; // Adjust the path to your test YAML file
            listView.load(filePath);
            Assert.IsTrue(listView.Columns.Count > 0, "Columns should be created from the YAML file.");
            Assert.IsTrue(listView.Items.Count > 0, "Rows should be created from the YAML file.");
        }
    }
}

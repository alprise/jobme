using System.Windows;
using TestReference;

namespace Test1.Content
{
    class TestClass
    {
        public void TestMb(string message)
        {
            MessageBox.Show(message);
            var s = new TestClassRef();
            s.TestRefMethod(message + "hand");
        }
    }
}

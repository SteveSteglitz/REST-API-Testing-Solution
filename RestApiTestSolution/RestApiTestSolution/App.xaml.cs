using System.Windows;

namespace RestApiTestSolution
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            using (var stream = new System.IO.MemoryStream(RestApiTestSolution.Properties.Resources.jsonSyntaxHighligt))
            {
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.RegisterHighlighting("JSON", new string[0],
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                            ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance));
                }
            }
        }
    }
}

using System.IO;
using System.Windows;

namespace DailyJounal
{
    /// <summary>
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class Read : Window
    {
        public Read()
        {
            InitializeComponent();
        }

        public void LoadDocument(string _FilePath, string Title = "")
        {
            if (File.Exists(_FilePath))
            {
                DateTextBlock.Text = Title;
                string text = File.ReadAllText(_FilePath);
                MainTextBox.Text = text;
            }
            else
            {
                DateTextBlock.Text = "File not found!";
            }


        }
    }
}

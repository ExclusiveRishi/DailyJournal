using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace DailyJounal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string CurrentDate;
        private string DirPath;
        private DispatcherTimer? _saveTimer;

        public MainWindow()
        {
            CurrentDate = DateTime.Now.ToString("dd");
            var CurrentMonth = DateTime.Now.ToString("MMMM");
            var CurrentYear = DateTime.Now.ToString("yyyy");
            String MyDocumentFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            DirPath = Path.Combine(MyDocumentFolderPath, "DailyJournal", CurrentYear, CurrentMonth);
            Directory.CreateDirectory(DirPath);

            InitializeComponent();
            DateTextBlock.Text = DateTime.Now.ToString("D");
            LoadDocument(CurrentDate);

            FileTree fileTree = new FileTree();
            fileTree.Show();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            _saveTimer?.Stop();

            _saveTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2),
            };
            _saveTimer.Tick += (s, e) =>
            {
                _saveTimer.Stop();
                SaveDocument();
            };
            _saveTimer.Start();
        }

        private void SaveDocument()
        {
            TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (var fstream = new FileStream(Path.Combine(DirPath, CurrentDate), FileMode.Create))
            {
                range.Save(fstream, DataFormats.Text);
            }
        }

        private void LoadDocument(string _Date)
        {
            string filePath = Path.Combine(DirPath, _Date);

            if (!File.Exists(filePath)) return;

            try
            {
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                using (var fstream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    range.Load(fstream, DataFormats.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Loading document: {ex.Message}");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit? \nfiles are saved automatically!", "Confirm Exit?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
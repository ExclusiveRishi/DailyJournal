using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DailyJounal
{
    /// <summary>
    /// Interaction logic for FileTree.xaml
    /// </summary>
    public partial class FileTree : Window
    {
        private Dictionary<String, Dictionary<String, List<String>>> Directories;

        public FileTree()
        {
            InitializeComponent();
            LoadDirectories();
            PopulateTree();
        }

        private void LoadDirectories()
        {
            String MyDocumentFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String RootDirPath = Path.Combine(MyDocumentFolderPath, "DailyJournal");

            Directories = new();

            foreach (var yearPath in Directory.GetDirectories(RootDirPath))
            {
                var year = Path.GetFileName(yearPath);
                Directories[year] = new();
                foreach (var monthPath in Directory.GetDirectories(yearPath))
                {
                    var month = Path.GetFileName(monthPath);
                    Directories[year][month] = new();
                    foreach (var dayPath in Directory.GetFiles(monthPath))
                    {
                        var day = Path.GetFileName(dayPath);
                        Directories[year][month].Add(day);
                    }
                }
            }
        }

        private void PageViewItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue as TreeViewItem;
            if (selectedItem != null)
            {
                if (selectedItem.Items.Count == 0)
                {
                    List<string> parentChain = GetParentChain(selectedItem);
                    string RootDirPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DailyJournal");
                    string fullPath = System.IO.Path.Combine(parentChain.ToArray());
                    string FinalPath = System.IO.Path.Combine(RootDirPath, fullPath);
                    string TitleText = $"{parentChain[2]} {parentChain[1]} {parentChain[0]}";
                    Read read = new Read();
                    read.LoadDocument(FinalPath, TitleText);
                    read.Show();
                }

            }
        }

        private List<string> GetParentChain(TreeViewItem item)
        {
            var chain = new List<string>();
            DependencyObject current = item;

            while (current != null)
            {
                if (current is TreeViewItem treeItem)
                {
                    chain.Insert(0, treeItem.Header.ToString()!);
                }
                current = VisualTreeHelper.GetParent(current);
            }

            return chain;
        }

        private void PopulateTree()
        {
            PageView.Items.Clear();

            foreach (var year in Directories.Keys)
            {
                var yearItem = new TreeViewItem { Header = year };

                foreach (var month in Directories[year].Keys)
                {
                    var monthItem = new TreeViewItem { Header = month };

                    foreach (var day in Directories[year][month])
                    {
                        monthItem.Items.Add(new TreeViewItem { Header = day});
                    }
                    yearItem.Items.Add(monthItem);
                }
                PageView.Items.Add(yearItem);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}

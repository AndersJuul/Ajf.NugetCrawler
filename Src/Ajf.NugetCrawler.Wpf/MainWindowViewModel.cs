using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using Ajf.NugetCrawler.Wpf.Annotations;

namespace Ajf.NugetCrawler.Wpf
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private string _text;

        public MainWindowViewModel()
        {
            var backgroundWorker = new BackgroundWorker
            {

            };
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Text = "Looking in c:\\projects" + Environment.NewLine;
            var enumerateFiles = Directory.EnumerateFiles(@"c:\projects\", "packages.config", SearchOption.AllDirectories)
                .ToArray();

            Text += enumerateFiles.Length + "Files" + Environment.NewLine;

            foreach (var enumerateFile in enumerateFiles)
            {
                var doc= new XmlDocument();
                doc.Load(enumerateFile);

                var path = Path.GetDirectoryName(enumerateFile);

                var packagesNode = doc.ChildNodes[1];
                foreach (XmlNode nugetNode in packagesNode.ChildNodes)
                {
                    var id = nugetNode.Attributes[0].Value;
                    var version = nugetNode.Attributes[1].Value;
                }
                Text += "*";
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value; 
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
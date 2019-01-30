using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using Ajf.NugetCrawler.Wpf.Annotations;

namespace Ajf.NugetCrawler.Wpf
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _text;
        private BackgroundWorker _backgroundWorker;

        public MainWindowViewModel()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
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

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var findings = new List<Finner>();
            Text = "Looking in c:\\projects" + Environment.NewLine;
            var enumerateFiles = Directory
                .EnumerateFiles(@"c:\projects\", "packages.config", SearchOption.AllDirectories)
                .ToArray();

            Text += enumerateFiles.Length + "Files" + Environment.NewLine;

            foreach (var enumerateFile in enumerateFiles)
            {
                var doc = new XmlDocument();
                doc.Load(enumerateFile);

                var path = Path.GetDirectoryName(enumerateFile);

                var packagesNode = doc.ChildNodes[1];
                foreach (XmlNode nugetNode in packagesNode.ChildNodes)
                {
                    var id = nugetNode.Attributes[0].Value;
                    var version = nugetNode.Attributes[1].Value;

                    if (id.ToLower().Contains("jci")) findings.Add(new Finner(path, id, version));
                }

                Text += "*";
            }

            var queryLastNames =
                from finner in findings
                group finner by finner.Id
                into newGroup
                orderby newGroup.Key
                select newGroup;

            foreach (var queryLastName in queryLastNames)
            {
                Text +=
                    $"{queryLastName.Key} {Environment.NewLine}";

                var finnners = queryLastName.ToArray();
                var queryLastNames1 =
                    from finner in finnners
                    group finner by finner.Version
                    into newGroup
                    orderby newGroup.Key
                    select newGroup;


                var groupings = queryLastNames1.ToArray();

                if (groupings.Length > 1)
                    foreach (var grouping in groupings)
                    {
                        foreach (var finner in grouping)
                            Text +=
                                $" -- {grouping.Key} {finner.Path} {Environment.NewLine}";
                        Text += $"{Environment.NewLine}";
                    }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Activated()
        {
            if(!_backgroundWorker.IsBusy)
            _backgroundWorker.RunWorkerAsync();
        }
    }

    public class Finner
    {
        public Finner(string path, string id, string version)
        {
            Path = path;
            Id = id;
            Version = version;
        }

        public string Path { get; }
        public string Id { get; }
        public string Version { get; }
    }
}
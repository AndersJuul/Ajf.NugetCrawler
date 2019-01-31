using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using Ajf.NugetCrawler.Wpf.Annotations;
using System.Windows.Input;

namespace Ajf.NugetCrawler.Wpf
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _text;
        private string _projectpath = "c:\\projects";
        private readonly BackgroundWorker _backgroundWorker;

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

        public string ProjectPath
        {
            get => _projectpath;
            set
            {
                _projectpath = value;
                OnPropertyChanged();
            }
        }

        private ICommand _saveCommand;
        public ICommand ActivatedCmd
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => Activated()
                    );
                }
                return _saveCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var nugetReferences = new List<NugetReference>();
            Text = "Looking in \"" + ProjectPath + "\"" + Environment.NewLine;

            IEnumerable<string> packageConfigFiles = null;
            try
            {
                packageConfigFiles = Directory
                .EnumerateFiles(ProjectPath, "packages.config", SearchOption.AllDirectories);
            }
            catch(Exception ex)
            {
                Text += "(" + ex.GetType().Name + ") " + ex.Message + Environment.NewLine;
                return;
            }


            Text += packageConfigFiles.Count() + " Files" + Environment.NewLine;

            foreach (var packageConfigFile in packageConfigFiles)
            {
                var doc = new XmlDocument();
                doc.Load(packageConfigFile);

                var path = Path.GetDirectoryName(packageConfigFile);

                var packagesNode = doc.ChildNodes[1];
                foreach (XmlNode nugetNode in packagesNode.ChildNodes)
                {
                    var id = nugetNode.Attributes[0].Value;
                    var version = nugetNode.Attributes[1].Value;

                    if (id.ToLower().Contains("jci")) nugetReferences.Add(new NugetReference(path, id, version));
                }

                Text += "*";
            }

            Text +=$"{Environment.NewLine}";

            var idGroups =
                (from nugetReference in nugetReferences
                group nugetReference by nugetReference.Id
                into newGroup
                orderby newGroup.Key
                select newGroup).ToArray();

            foreach (var idGroup in idGroups)
            {
                Text +=$"{idGroup.Key} {Environment.NewLine}";

                var packagesInIdGroup = idGroup.ToArray();

                var packagesGroupedByVersion =
                    (from nugetReference in packagesInIdGroup
                    group nugetReference by nugetReference.Version
                    into newGroup
                    orderby newGroup.Key
                    select newGroup).ToArray();


                var versionGroups = packagesGroupedByVersion.ToArray();

                if (versionGroups.Length > 1)
                    foreach (var grouping in versionGroups)
                    {
                        foreach (var finner in grouping)
                            Text +=
                                $" -- {grouping.Key} {finner.Path} {Environment.NewLine}";
                        Text += $"{Environment.NewLine}";
                    }
            }

            var lineCount = Text.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Length;
            Text = "Total lines " + lineCount + Environment.NewLine + Text;
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

    public class NugetReference
    {
        public NugetReference(string path, string id, string version)
        {
            Path = path;
            Id = id;
            Version = version;

            FolderName = Path.Split('\\').Last();
        }

        public string FolderName { get; set; }

        public string Path { get; }
        public string Id { get; }
        public string Version { get; }

        public int NumberOfOwnedNugets { get; set; }
    }
}
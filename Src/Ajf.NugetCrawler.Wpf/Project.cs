using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ajf.NugetCrawler.Wpf
{
    public class Project : IEnumerable<NugetReference>
    {
        private readonly NugetReferenceList _list;

        public Project(string path)
        {
            _list = new NugetReferenceList();

            Path = path;
            Name = path
                .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Last();
        }

        public int NCount => _list.Count();
        public string Path { get; }
        public string Name { get; }

        public IEnumerator<NugetReference> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddNuget(NugetReference nugetReference)
        {
            _list.GetByNugetReference(nugetReference);
        }

        public bool HasNugets()
        {
            return _list.Any();
        }
    }
}
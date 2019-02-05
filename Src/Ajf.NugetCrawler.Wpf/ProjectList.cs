using System.Collections;
using System.Collections.Generic;

namespace Ajf.NugetCrawler.Wpf
{
    public class ProjectList : IEnumerable<Project>
    {
        private readonly List<Project> _list;

        public ProjectList()
        {
            _list = new List<Project>();
        }

        public IEnumerator<Project> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Project project)
        {
            _list.Add(project);
        }
    }
}
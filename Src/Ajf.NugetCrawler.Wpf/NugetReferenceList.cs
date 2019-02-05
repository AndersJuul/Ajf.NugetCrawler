using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ajf.NugetCrawler.Wpf
{
    public class NugetReferenceList:IEnumerable<NugetReference>
    {
        private readonly List<NugetReference> _list;

        public NugetReferenceList()
        {
            _list = new List<NugetReference>();
        }

        public NugetReference GetByNugetReference(NugetReference nugetReference)
        {
            var existing = _list.SingleOrDefault(x => x == nugetReference);
            if (existing != null) return existing;

            _list.Add(nugetReference);
            return nugetReference;
        }

        public bool Any()
        {
            return _list.Any();
        }

        public IEnumerator<NugetReference> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
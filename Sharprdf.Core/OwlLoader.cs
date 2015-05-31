using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace Sharprdf.Core
{
    public class OwlLoader
    {
        public void LoadOwl()
        {
            IGraph g = new Graph();
            g.LoadFromFile("scro.owl");
        }
    }
}

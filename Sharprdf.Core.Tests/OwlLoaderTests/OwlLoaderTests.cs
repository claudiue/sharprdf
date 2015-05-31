using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharprdf.Core.Tests.OwlLoaderTests
{
    [TestFixture]
    public class OwlLoaderTests
    {
        [Test]
        public void Should_load_owl_file()
        {
            var sut = new OwlLoader();
            sut.LoadOwl();
        }
    }
}

using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Bodoconsult.Core.Test.MetaData.Handler;
using JobExecuterCore.Dal.Model;
using NUnit.Framework;

namespace JobExecuterCore.Test
{
    [TestFixture]
    public class UnitTestMetaDataHandler
    {
        [Test]
        public void TestAddType()
        {

            var type = typeof(ActiveDirectoryJobTask);

            var mh = new MetaDataHandler();
            mh.LoadReferencedAssemblies(Assembly.GetExecutingAssembly());

            mh.AddType(type);

            var item = mh.TypeMetaDatas[0];


            Assert.IsTrue(item != null);
            Assert.IsFalse(string.IsNullOrEmpty(item.Summary));

            var prop = item.TypeItemMetaDatas.FirstOrDefault(x => x.Name == "Timeout");

            Assert.IsTrue(prop != null);
            Assert.IsFalse(string.IsNullOrEmpty(prop.Summary));

            Debug.Print(prop.Name + ": " + prop.Summary);

        }


        [Test]
        public void TestAddTypeSqlDocuAllDatabasesJobTask()
        {

            var type = typeof(SqlDocuAllDatabasesJobTask);

            var mh = new MetaDataHandler();
            mh.LoadReferencedAssemblies(Assembly.GetExecutingAssembly());
            mh.AddType(type);

            var item = mh.TypeMetaDatas[0];


            Assert.IsTrue(item != null);
            Assert.IsFalse(string.IsNullOrEmpty(item.Summary));


            //var sr = new StructuredText();

            //sr.AddParagraph(item.Summary);

            Debug.Print(item.Summary);


            var prop = item.TypeItemMetaDatas.FirstOrDefault(x => x.Name == "Timeout");

            Assert.IsTrue(prop != null);
            Assert.IsFalse(string.IsNullOrEmpty(prop.Summary));

            Debug.Print(prop.Name + ": " + prop.Summary);

        }


    }
}
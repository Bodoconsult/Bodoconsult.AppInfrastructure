//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System.IO;
//using System.Threading;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Media;
//using Bodoconsult.App.Wpf.Converters;
//using NUnit.Framework;

//namespace Bodoconsult.App.Wpf.Test
//{
//    [TestFixture]
//    public class UnitTestWpfUtility
//    {
//        [Test]
//        public void TestFindResource_OnlyRessourceName()
//        {

//            var brush = (Brush)WpfUtility.FindResource("BackgroundBrush02");

//            Assert.IsNotNull(brush);
//        }

//        [Test]
//        public void TestSaveElementAsXamlFile()
//        {

//            const string xamlFile = @"C:\temp\XamlTestFile.xaml";

//            if (File.Exists(xamlFile)) File.Delete(xamlFile);

//            var button = new Button { Content = "Hallo" };

//            WpfUtility.SaveElementAsXamlFile(button, xamlFile);

//            Assert.That(File.Exists(xamlFile));

//        }


//        [Test]
//        public void TestLoadElementFromXamlFile()
//        {

//            const string xamlFile = @"C:\temp\XamlTestFile.xaml";

//            if (File.Exists(xamlFile)) File.Delete(xamlFile);

//            var button = new Button { Content = "Hallo" };

//            WpfUtility.SaveElementAsXamlFile(button, xamlFile);

//            Assert.That(File.Exists(xamlFile));

//            var buttonErg = (Button)WpfUtility.LoadElementFromXamlFile(xamlFile);

//            Assert.That(buttonErg != null);
//            Assert.That(buttonErg.Content.ToString() == "Hallo");
//        }

//    }

//    [TestFixture]
//    public class UnitTestFlowDocumentContentToXamlConverter
//    {


//        [Test]
//        public void TestConvertAndConvertBack()
//        {
//            //Arrange
//            //const string xaml = "<Paragraph>Lorem <Run FontStyle='italic'>ipsum</Run> dolor sit amet, <Run FontWeight='bold'>consetetur sadipscing elitr</Run>, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo <Run FontWeight='bold'>duo dolores et ea rebum</Run>. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.</Paragraph>";
//            const string xaml = "<Paragraph>Lorem ipsum  dolor sit amet, consetetur sadipscing elitr</Paragraph>";

//            // Act
//            var conv = new FlowDocumentContentToXamlConverter();

//            var doc = (FlowDocument)conv.Convert(xaml, typeof(FlowDocument), null, Thread.CurrentThread.CurrentUICulture);


//            //Assert
//            Assert.That(doc != null);

//            var erg = (string)conv.ConvertBack(doc, typeof(string), null, Thread.CurrentThread.CurrentUICulture);

//            Assert.That(erg == xaml);
//        }
//    }
//}

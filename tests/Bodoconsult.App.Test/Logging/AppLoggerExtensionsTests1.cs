//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Bodoconsult.App.Logging;

//namespace Bodoconsult.App.Test.Logging
//{
//    [TestFixture]
//    // ReSharper disable once InconsistentNaming
//    public class AppLoggerExtensionsTests
//    {
//        [Test]
//        public void TestLoadDebugLog4Net()
//        {
//            // Arrange
//            var config = new LoggingConfig();

//            // Act
//            var factory = AppLoggerExtensions.GetDefaultLogger(config);

//            // Assert
//            Assert.That(factory, Is.Not.Null);
//        }

//        [Test]
//        public void TestLoadDebug()
//        {
//            // Arrange
//            var config = new LoggingConfig();

//            // Act
//            var factory = AppLoggerExtensions.GetDefaultLogger(config);

//            // Assert
//            Assert.That(factory, Is.Not.Null);

//        }

//        [Test]
//        public void TestLoadLog4Net()
//        {
//            // Arrange
//            var config = new LoggingConfig();

//            // Act
//            var factory = AppLoggerExtensions.GetDefaultLogger(config);

//            // Assert
//            Assert.That(factory, Is.Not.Null);
//        }


//        [Test]
//        public void TestLoadConsole()
//        {
//            // Arrange
//            var config = new LoggingConfig();

//            // Act
//            var factory = AppLoggerExtensions.GetDefaultLogger(config);

//            // Assert
//            Assert.That(factory, Is.Not.Null);
//            Assert.That(config.UseConsoleProvider, Is.True);
//            Assert.That(config.UseLog4NetProvider, Is.False);
//            Assert.That(config.UseDebugProvider, Is.False);

//        }


//        //[Test]
//        //public void TestLoadFromAppSettings()
//        //{
//        //    // Arrange
//        //    var config = Globals.LoadLoggingConfig();

//        //    // Act
//        //    var factory = AppLoggerExtensions.GetDefaultLogger(config);

//        //    // Assert
//        //    Assert.That(factory));

//        //}
//    }
//}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bodoconsult.App.Wpf.Models;
using Bodoconsult.App.Wpf.Test.Converters;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Test.Models
{
    [TestFixture]
    internal class SharedResourceDictionaryTests
    {
        [Test]
        public void Ctor_ValidPath_InstanceCreated()
        {
            // Arrange 

            // Act  
            var p = new SharedResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Bodoconsult.App.Wpf.Test;component/Locales/culture.de.xaml", UriKind.RelativeOrAbsolute)
            };

            // Assert
            Assert.That(p, Is.Not.Null);
        }
    }
}

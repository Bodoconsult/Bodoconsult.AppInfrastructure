// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Tests.App;
using Bodoconsult.App.ReactiveUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodoconsult.App.ReactiveUI.Tests.ViewModels
{
    [TestFixture]
    internal class CopyrightViewModelTests
    {
        [Test]
        public void Ctor_ValidSetup_PropsSetCorrectly()
        {
            // Arrange 
            var appGlobals = Globals.Instance;

            // Act  
            var vm = new CopyrightViewModel(appGlobals);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(string.IsNullOrEmpty(vm.AppTitle), Is.False);
                Assert.That(string.IsNullOrEmpty(vm.ModulesInfo), Is.False);
            }
        }

        [Test]
        public void LoadModule_ValidModuleInfo_PropsSetCorrectly()
        {
            // Arrange 
            var appGlobals = Globals.Instance;

            var vm = new CopyrightViewModel(appGlobals);

            // Act  
            vm.LoadModule("Backend 1.0.0");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(string.IsNullOrEmpty(vm.AppTitle), Is.False);
                Assert.That(string.IsNullOrEmpty(vm.ModulesInfo), Is.False);
            }

            Debug.Print(vm.ModulesInfo);
        }

        [Test]
        public void LoadLicenseInfo_ValidDefaultFile_PropsSetCorrectly()
        {
            // Arrange 
            var appGlobals = Globals.Instance;

            var vm = new CopyrightViewModel(appGlobals);

            // Act  
            vm.LoadLicenseInfo();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(string.IsNullOrEmpty(vm.AppTitle), Is.False);
                Assert.That(string.IsNullOrEmpty(vm.LicenseInfo), Is.False);
            }

            Debug.Print(vm.LicenseInfo);
        }

        [Test]
        public void LoadToolInfo_ValidDefaultFile_PropsSetCorrectly()
        {
            // Arrange 
            var appGlobals = Globals.Instance;

            var vm = new CopyrightViewModel(appGlobals);

            // Act  
            vm.LoadToolInfo();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(string.IsNullOrEmpty(vm.AppTitle), Is.False);
                Assert.That(string.IsNullOrEmpty(vm.ToolInfo), Is.False);
            }

            Debug.Print(vm.ToolInfo);
        }
    }
}

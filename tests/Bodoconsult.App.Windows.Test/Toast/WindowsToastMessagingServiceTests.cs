// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Windows.Toast;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.Toast;

[TestFixture]
internal class WindowsToastMessagingServiceTests
{

    [Test]
    public void SendSimpleToastMessage_NotifyRequestRecord_NotificationIsShown()
    {
        // Arrange 
        var request = new NotifyRequestRecord
        {
            Text = "Das ist eine Message",
            Title = "Title"
        };

        var s = new WindowsToastMessagingService();

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            s.SendSimpleToastMessage(request);
            s.Dispose();
        });

        
    }

    [Test]
    public void SendSimpleToastMessage_NotifyRequestRecordLongerMessage_NotificationIsShown()
    {
        // Arrange 
        var request = new NotifyRequestRecord
        {
            Text = "Das ist eine Message\r\nDas ist eine 2. Message",
            Title = "Title"
        };

        var s = new WindowsToastMessagingService();

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            s.SendSimpleToastMessage(request);
            s.Dispose();
        });
    }
}
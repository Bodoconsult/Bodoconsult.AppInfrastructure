// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.WinForms.Toast;

namespace Bodoconsult.App.Test.Toast;

[TestFixture]
internal class WinFormsToastMessagingServiceTests
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

        var s = new WinFormsToastMessagingService();

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            s.SendSimpleToastMessage(request);
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

        var s = new WinFormsToastMessagingService();

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            s.SendSimpleToastMessage(request);
        });

    }

}
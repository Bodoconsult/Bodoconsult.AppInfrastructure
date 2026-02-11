// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Menus;
using Bodoconsult.App.ReactiveUI.Ui;
using System.Diagnostics;
using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Tests.Helpers
{
    public static class MenuBuilderHelper
    {
        public static void LoadMenuItems(IUiMenuBuilder builder)
        {
            var groupItem = new GroupUiMenuItem("File");

            var commandItem = new CommandUiMenuItem("Exit")
            {
                CommandDefinition = new UiCommandDefinition(DoSomethingAsync(), null)
            };

            groupItem.AddChild(commandItem);

            var groupItem2 = new GroupUiMenuItem("Options");
            groupItem.AddChild(groupItem2);

            var commandItem2 = new CommandUiMenuItem("Test")
            {
                CommandDefinition = new UiCommandDefinition(DoSomethingAsync2(), null)
            };

            groupItem2.AddChild(commandItem2);

            var separatorItem = new SeparatorUiMenuItem("Dummy");
            groupItem2.AddChild(separatorItem);


            var commandItem3 = new CommandUiMenuItem("Test")
            {
                CommandDefinition = new UiCommandDefinition(DoSomethingAsync2(), null)
            };
            groupItem2.AddChild(commandItem3);

            builder.Add(groupItem);
        }

        private static async Task DoSomethingAsync()
        {
            Debug.Print("Hello World 1!");
            await Task.Delay(TimeSpan.FromSeconds(3));
        }

        private static async Task DoSomethingAsync2()
        {
            Debug.Print("Hello World 1!");
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}

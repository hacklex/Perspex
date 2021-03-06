﻿// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Perspex.Collections;
using Perspex.Controls.Presenters;
using Perspex.Controls.Primitives;
using Perspex.Controls.Templates;
using Xunit;

namespace Perspex.Controls.UnitTests.Primitives
{
    public class SelectingItemsControlTests_AutoSelect
    {
        [Fact]
        public void First_Item_Should_Be_Selected()
        {
            var target = new SelectingItemsControl
            {
                AutoSelect = true,
                Items = new[] { "foo", "bar" },
                Template = Template(),
            };

            target.ApplyTemplate();

            Assert.Equal(0, target.SelectedIndex);
            Assert.Equal("foo", target.SelectedItem);
        }

        [Fact]
        public void First_Item_Should_Be_Selected_When_Added()
        {
            var items = new PerspexList<string>();
            var target = new SelectingItemsControl
            {
                AutoSelect = true,
                Items = items,
                Template = Template(),
            };

            target.ApplyTemplate();
            items.Add("foo");

            Assert.Equal(0, target.SelectedIndex);
            Assert.Equal("foo", target.SelectedItem);
        }

        [Fact]
        public void Item_Should_Be_Selected_When_Selection_Removed()
        {
            var items = new PerspexList<string>(new[] { "foo", "bar", "baz", "qux" });

            var target = new SelectingItemsControl
            {
                AutoSelect = true,
                Items = items,
                Template = Template(),
            };

            target.ApplyTemplate();
            target.SelectedIndex = 2;
            items.RemoveAt(2);

            Assert.Equal(2, target.SelectedIndex);
            Assert.Equal("qux", target.SelectedItem);
        }

        [Fact]
        public void Selection_Should_Be_Cleared_When_No_Items_Left()
        {
            var items = new PerspexList<string>(new[] { "foo", "bar" });

            var target = new SelectingItemsControl
            {
                AutoSelect = true,
                Items = items,
                Template = Template(),
            };

            target.ApplyTemplate();
            target.SelectedIndex = 1;
            items.RemoveAt(1);
            items.RemoveAt(0);

            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        private ControlTemplate Template()
        {
            return new ControlTemplate<SelectingItemsControl>(control =>
                new ItemsPresenter
                {
                    Name = "itemsPresenter",
                    [~ItemsPresenter.ItemsProperty] = control[~ItemsControl.ItemsProperty],
                    [~ItemsPresenter.ItemsPanelProperty] = control[~ItemsControl.ItemsPanelProperty],
                });
        }

        private class Item : Control, ISelectable
        {
            public bool IsSelected { get; set; }
        }
    }
}

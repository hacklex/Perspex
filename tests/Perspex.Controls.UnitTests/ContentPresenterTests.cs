﻿// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Specialized;
using System.Linq;
using Perspex.Controls.Presenters;
using Perspex.LogicalTree;
using Xunit;

namespace Perspex.Controls.UnitTests
{
    public class ContentPresenterTests
    {
        [Fact]
        public void Setting_Content_Should_Make_Control_Appear_In_LogicalChildren()
        {
            var target = new ContentPresenter();
            var child = new Control();

            target.Content = child;
            target.ApplyTemplate();

            Assert.Equal(new[] { child }, ((ILogical)target).LogicalChildren.ToList());
        }

        [Fact]
        public void Clearing_Content_Should_Remove_From_LogicalChildren()
        {
            var target = new ContentPresenter();
            var child = new Control();

            target.Content = child;
            target.ApplyTemplate();
            target.Content = null;
            target.ApplyTemplate();

            Assert.Equal(new ILogical[0], ((ILogical)target).LogicalChildren.ToList());
        }

        [Fact]
        public void Clearing_Content_Clear_Childs_Parent()
        {
            var target = new ContentPresenter();
            var child = new Control();

            target.Content = child;
            target.ApplyTemplate();
            target.Content = null;
            target.ApplyTemplate();

            Assert.Null(child.Parent);
            Assert.Null(child.GetLogicalParent());
        }

        [Fact]
        public void Changing_Content_Should_Fire_LogicalChildren_CollectionChanged()
        {
            var target = new ContentPresenter();
            var child = new Control();
            var called = false;

            ((ILogical)target).LogicalChildren.CollectionChanged += (s, e) =>
                called = e.Action == NotifyCollectionChangedAction.Add;

            target.Content = child;
            target.ApplyTemplate();

            Assert.True(called);
        }

        [Fact]
        public void Clearing_Content_Should_Fire_LogicalChildren_CollectionChanged()
        {
            var target = new ContentPresenter();
            var child = new Control();
            var called = false;

            target.Content = child;
            target.ApplyTemplate();

            ((ILogical)target).LogicalChildren.CollectionChanged += (s, e) => called = true;

            target.Content = null;
            target.ApplyTemplate();

            Assert.True(called);
        }
    }
}

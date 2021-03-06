﻿// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Generic;

namespace Perspex.Collections
{
    /// <summary>
    /// A notiftying list.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    public interface IPerspexList<T> : IList<T>, IPerspexReadOnlyList<T>
    {
        /// <summary>
        /// Adds multiple items to the collection.
        /// </summary>
        /// <param name="items">The items.</param>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Inserts multiple items at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="items">The items.</param>
        void InsertRange(int index, IEnumerable<T> items);

        /// <summary>
        /// Removes multiple items from the collection.
        /// </summary>
        /// <param name="items">The items.</param>
        void RemoveAll(IEnumerable<T> items);
    }
}
#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common.Utilities
{
    /// <summary>
    /// Synchronizes the state of one collection based on the state of another collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The term "synchronization" here has nothing to do with threads, but refers to updating the elements of
    /// one collection based on the elements contained in another collection.  The two collections need not 
    /// have the same element type.  
    /// </para>
    /// <para>
    /// Call the <see cref="Synchronize"/> method to update the "destination" collection so that it matches
    /// the contents of the "source" collection.  The <see cref="CompareItems"/> callback will be used to determine
    /// if an item in the source collection represents the same item in the destination collection.  For items
    /// that appear in the source but not the destination, the <see cref="AddItem"/> callback will be called.
    /// For items that appear in the destination collection but not the source collection, the <see cref="RemoveItem"/>
    /// callback will be called.  For items that appear in both collections, the <see cref="UpdateItem"/> callback
    /// will be called in order to update the element in the destination collection based on the item in the source
    /// collection.
    /// </para>
    /// <para>
    /// There are two ways to use this class.  Either instantiate it directly, providing a set of delegates
    /// to implement the callbacks, or create a subclass and override the protected callback methods.
    /// </para>
    /// <para>
    /// 
    /// </para>
    /// </remarks>
    /// <typeparam name="TDestItem"></typeparam>
    /// <typeparam name="TSourceItem"></typeparam>
    public class CollectionSynchronizeHelper<TDestItem, TSourceItem>
        where TDestItem : class
    {
        /// <summary>
        /// Delegate to compare identities of items in source and destination collections.
        /// </summary>
        public delegate bool CompareItemsDelegate(TDestItem destItem, TSourceItem sourceItem);

        /// <summary>
        /// Delegate to add an item to the destination collection based on a source item.
        /// </summary>
        public delegate void AddItemDelegate(TSourceItem item, ICollection<TDestItem> destList);

        /// <summary>
        /// Delegate to update an item in the destination collection based on a source item.
        /// </summary>
        public delegate void UpdateItemDelegate(TDestItem destItem, TSourceItem sourceItem, ICollection<TDestItem> destList);

        /// <summary>
        /// Delegate to remove an item from the destination collection.
        /// </summary>
        public delegate void RemoveItemDelegate(TDestItem destItem, ICollection<TDestItem> destList);

        private readonly CompareItemsDelegate _compareItemsCallback;
        private readonly AddItemDelegate _addItemCallback;
        private readonly UpdateItemDelegate _updateItemCallback;
        private readonly RemoveItemDelegate _removeItemCallback;

        private readonly bool _allowUpdate = false;
        private readonly bool _allowRemove = false;
        
        /// <summary>
        /// Protected constructor for creating subclasses.
        /// </summary>
        /// <param name="allowUpdate">Indicates whether items in the destination collection can be updated.</param>
        /// <param name="allowRemove">Indicates whether items can be removed from the destination collection.</param>
        protected CollectionSynchronizeHelper(bool allowUpdate, bool allowRemove)
        {
            _allowUpdate = allowUpdate;
            _allowRemove = allowRemove;
        }

        /// <summary>
        /// Public constructor for direct use of this class.
        /// </summary>
        /// <param name="compareItemsCallback">Delegate for comparing identity of items in the source and destination collections.</param>
        /// <param name="addItemCallback">Delegate for adding items to the destination collection.</param>
        /// <param name="updateItemCallback">Delegate for updating items in the destination collection, or null if items should not be updated.</param>
        /// <param name="removeCallback">Delegate for removing items from the destination collection, or null if items should not be removed.</param>
        public CollectionSynchronizeHelper(
            CompareItemsDelegate compareItemsCallback,
            AddItemDelegate addItemCallback,
            UpdateItemDelegate updateItemCallback,
            RemoveItemDelegate removeCallback)
        {
            _compareItemsCallback = compareItemsCallback;
            _addItemCallback = addItemCallback;
            _updateItemCallback = updateItemCallback;
            _removeItemCallback = removeCallback;

            _allowUpdate = _updateItemCallback != null;
            _allowRemove = _removeItemCallback != null;
        }

        /// <summary>
        /// Synchronize the destination collection to match the source collection.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        public void Synchronize(ICollection<TDestItem> dest, ICollection<TSourceItem> source)
        {
			List<TDestItem> existing = new List<TDestItem>(dest);
			List<TDestItem> unProcessed = new List<TDestItem>(dest);

            CollectionUtils.ForEach(source,
                    delegate(TSourceItem sourceItem)
                    {
                        // Find a pre-existing item that matches the source item
						TDestItem existingItem = CollectionUtils.SelectFirst(existing,
                            delegate(TDestItem destItem) { return CompareItems(destItem, sourceItem); });

                        if (existingItem == null)
                        {
                            // Add a new dest item
                            AddItem(sourceItem, dest);
                        }
                        else
                        {
                            // Update the existing attachment
                            if (_allowUpdate)
                                UpdateItem(existingItem, sourceItem, dest);

                            // and remove from un-processed list
                            unProcessed.Remove(existingItem);
                        }
                    });

            // Any items in the dest list that are not in the source list are considered "deleted"
            if (unProcessed.Count > 0)
            {
                if (_allowRemove)
                {
                    CollectionUtils.ForEach(unProcessed,
                                            delegate(TDestItem destItem)
                                            {
                                                RemoveItem(destItem, dest);
                                            });
                }
            }
        }

        /// <summary>
        /// Compare items in the source and destination collections to determine if they have the same identity.
        /// </summary>
        /// <param name="destItem"></param>
        /// <param name="sourceItem"></param>
        /// <returns>True if the item in the source collection represents the same item in the destination collection.</returns>
        protected virtual bool CompareItems(TDestItem destItem, TSourceItem sourceItem)
        {
            if (_compareItemsCallback == null)
                throw new NotImplementedException("Method must be overridden or a delegate supplied.");

            return _compareItemsCallback(destItem, sourceItem);
        }

        /// <summary>
        /// Called to add an item to the destination collection representing the specified source item.
        /// </summary>
        /// <param name="sourceItem"></param>
        /// <param name="dest"></param>
        protected virtual void AddItem(TSourceItem sourceItem, ICollection<TDestItem> dest)
        {
            if (_addItemCallback == null)
                throw new NotImplementedException("Method must be overridden or a delegate supplied.");

            _addItemCallback(sourceItem, dest);
        }

        /// <summary>
        /// Called to update the specified destination item with the specified source item.
        /// </summary>
        /// <param name="destItem">The item to be updated.</param>
        /// <param name="sourceItem">The item that is the source of the update.</param>
        /// <param name="dest">The destination collection, typically not used here.</param>
        protected virtual void UpdateItem(TDestItem destItem, TSourceItem sourceItem, ICollection<TDestItem> dest)
        {
            if (_updateItemCallback == null)
                throw new NotImplementedException("Method must be overridden or a delegate supplied.");

            _updateItemCallback(destItem, sourceItem, dest);
        }

        /// <summary>
        /// Called to remove the specified item from the destination collection.
        /// </summary>
        /// <param name="destItem">The item to remove.</param>
        /// <param name="dest">The destination collection.</param>
        protected virtual void RemoveItem(TDestItem destItem, ICollection<TDestItem> dest)
        {
            if (_removeItemCallback == null)
                throw new NotImplementedException("Method must be overridden or a delegate supplied.");

            _removeItemCallback(destItem, dest);
        }
    }
}
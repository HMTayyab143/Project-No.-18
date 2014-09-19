// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachableCollection.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   The attachable collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using UniversalTriggers.Interfaces;

    using Windows.ApplicationModel;
    using Windows.UI.Xaml;

    /// <summary>
    /// An attachable collection that can be attached to an element.
    /// </summary>
    /// <typeparam name="T">
    /// The type to store in the collection.
    /// </typeparam>
    public class AttachableCollection<T> : FrameworkElement, IList<T>, INotifyCollectionChanged, IAssociatableElement
        where T : FrameworkElement, IAssociatableElement
    {
        /// <summary>
        /// The lazy collection of item.
        /// </summary>
        private readonly Lazy<ObservableCollection<T>> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachableCollection{T}"/> class.
        /// </summary>
        public AttachableCollection()
        {
            this.items = new Lazy<ObservableCollection<T>>(this.InitializeItemsCollection);
        }

        /// <summary>
        /// The collection changed event handler.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets the associated element.
        /// </summary>
        public FrameworkElement AssociatedElement { get; private set; }

        /// <summary>
        /// Gets the count of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the items of the collection.
        /// </summary>
        private ObservableCollection<T> Items
        {
            get
            {
                return this.items.Value;
            }
        }

        /// <summary>
        /// Gets or sets an item in the collection at an index.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="T"/> to add.
        /// </returns>
        public T this[int index]
        {
            get
            {
                return this.Items[index];
            }

            set
            {
                var oldItem = this.Items[index];
                if (oldItem != null)
                {
                    this.ItemRemoved(oldItem);
                    oldItem.Detach();
                }

                this.Items[index] = value;

                if (value != null)
                {
                    this.ItemAdded(value);
                }
            }
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">
        /// The item to add.
        /// </param>
        public void Add(T item)
        {
            this.ItemAdded(item);
            this.Items.Add(item);
        }

        /// <summary>
        /// Clears the items in the collection.
        /// </summary>
        public void Clear()
        {
            foreach (var item in this.Items)
            {
                this.ItemRemoved(item);
            }

            this.Items.Clear();
        }

        /// <summary>
        /// Checks whether the collection contains an item.
        /// </summary>
        /// <param name="item">
        /// The item to check.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> result.
        /// </returns>
        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        /// <summary>
        /// Copies the collection to an array at a starting index.
        /// </summary>
        /// <param name="array">
        /// The array to copy to.
        /// </param>
        /// <param name="index">
        /// The index to start from.
        /// </param>
        public void CopyTo(T[] array, int index)
        {
            this.Items.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the index of an item.
        /// </summary>
        /// <param name="item">
        /// The item to check.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> result.
        /// </returns>
        public int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item at a given index from the collection.
        /// </summary>
        /// <param name="index">
        /// The index to insert at.
        /// </param>
        /// <param name="item">
        /// The item to insert.
        /// </param>
        public void Insert(int index, T item)
        {
            this.ItemAdded(item);
            this.Items.Insert(index, item);
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">
        /// The item to remove.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> result.
        /// </returns>
        public bool Remove(T item)
        {
            this.ItemRemoved(item);
            return this.Items.Remove(item);
        }

        /// <summary>
        /// Removes an item at an index from the collection.
        /// </summary>
        /// <param name="index">
        /// The index to remove at.
        /// </param>
        public void RemoveAt(int index)
        {
            var item = this.Items[index];
            this.ItemRemoved(item);
            this.Items.RemoveAt(index);
        }

        /// <summary>
        /// Attaches an element to this.
        /// </summary>
        /// <param name="element">
        /// The element to attach.
        /// </param>
        public void Attach(FrameworkElement element)
        {
            if (element != this.AssociatedElement)
            {
                if (this.AssociatedElement != null)
                {
                    throw new InvalidOperationException("Collection is already attached to another object.");
                }

                if (!DesignMode.DesignModeEnabled)
                {
                    this.AssociatedElement = element;
                }

                this.OnAttached();
            }
        }

        /// <summary>
        /// Detaches the associated element.
        /// </summary>
        public void Detach()
        {
            if (this.AssociatedElement == null)
            {
                return;
            }

            this.OnDetached();
            this.AssociatedElement = null;
        }

        /// <summary>
        /// Gets an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <summary>
        /// Called when an item is added to the collection to attach it to the associated element.
        /// </summary>
        /// <param name="item">
        /// The item to attach.
        /// </param>
        protected virtual void ItemAdded(T item)
        {
            if (this.AssociatedElement != null)
            {
                item.Attach(this.AssociatedElement);
            }
        }

        /// <summary>
        /// Called when an item is removed from the collection to detach it from the associated element.
        /// </summary>
        /// <param name="item">
        /// The item to detach.
        /// </param>
        protected virtual void ItemRemoved(T item)
        {
            if (this.AssociatedElement != null)
            {
                item.Detach();
            }
        }

        /// <summary>
        /// Called when the collection is attached to an element.
        /// </summary>
        protected virtual void OnAttached()
        {
            foreach (var item in this)
            {
                item.Attach(this.AssociatedElement);
            }
        }

        /// <summary>
        /// Called when the collection is detached from an element.
        /// </summary>
        protected virtual void OnDetached()
        {
            foreach (var item in this)
            {
                item.Detach();
            }
        }

        /// <summary>
        /// Handles the collection changed event.
        /// </summary>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Lazily initializes the collection.
        /// </summary>
        /// <returns>
        /// The <see cref="ObservableCollection"/>.
        /// </returns>
        private ObservableCollection<T> InitializeItemsCollection()
        {
            var collection = new ObservableCollection<T>();
            collection.CollectionChanged += this.CollectionItemsChanged;
            return collection;
        }

        /// <summary>
        /// Handles the collection changed event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        private void CollectionItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.OnCollectionChanged(args);
        }
    }
}
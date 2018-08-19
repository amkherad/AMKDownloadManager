using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using AMKDownloadManager.UI.Business.Models.Downloads;

namespace AMKDownloadManager.UI.Business.Services
{
    public class CategoryConfigService : IConfigDataEntryService<DownloadCategoryItem>
    {
        private int _count;
        private bool _isReadOnly;
        private int _count1;
        private int _count2;
        private bool _isReadOnly1;

        public IEnumerable<DownloadCategoryItem> Load()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<DownloadCategoryItem> LoadAndTrack()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<DownloadCategoryItem> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(DownloadCategoryItem item)
        {
            throw new NotImplementedException();
        }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize { get; }

        bool IList.IsReadOnly
        {
            get { return _isReadOnly1; }
        }

        object IList.this[int index]
        {
            get { return null; }
            set{}
        }

        void ICollection<DownloadCategoryItem>.Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(DownloadCategoryItem item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(DownloadCategoryItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(DownloadCategoryItem item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int ICollection.Count
        {
            get { return _count2; }
        }

        public bool IsSynchronized { get; }
        public object SyncRoot { get; }

        int ICollection<DownloadCategoryItem>.Count
        {
            get { return _count; }
        }

        bool ICollection<DownloadCategoryItem>.IsReadOnly
        {
            get { return _isReadOnly; }
        }

        public int IndexOf(DownloadCategoryItem item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, DownloadCategoryItem item)
        {
            throw new NotImplementedException();
        }

        void IList<DownloadCategoryItem>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public DownloadCategoryItem this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        int IReadOnlyCollection<DownloadCategoryItem>.Count
        {
            get { return _count1; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
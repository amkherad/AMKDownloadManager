using System;
using ir.amkdp.gear.core.Collections;
using System.ComponentModel;
using ir.amkdp.gear.core.Patterns.Mvvm;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api.Binders;

namespace AMKDownloadManager.Core.Api.Barriers
{
    public delegate void DownloadItemProtocolSelected(
        DownloadItem di,
        IProtocolProvider protocolProvider
    );

    public delegate void DownloadItemRequestCreated(
        DownloadItem di,
        IRequest request
    );

    public class DownloadItem : ViewModelBase
    {
        public event DownloadItemProtocolSelected ProtocolSelected;
        public event DownloadItemRequestCreated RequestCreated;

        public PropertyBag Properties { get; }

        public DownloadItem()
        {
            Properties = new PropertyBag();
        }

        public void OnProtocolSelected(DownloadItem di, IProtocolProvider protocolProvider)
            => ProtocolSelected?.Invoke(di, protocolProvider);

        public void OnRequestCreated(DownloadItem di, IRequest request)
            => RequestCreated?.Invoke(di, request);


        public Uri Uri
        {
            get
            {
                return Properties[KnownProperties.Uri] as Uri;
            }
            set
            {
                Properties[KnownProperties.Uri] = value;
                OnPropertyChanged(KnownProperties.Uri);
            }
        }

        public IEnumerable<Uri> Mirrors
        {
            get
            {
                return Properties[KnownProperties.Mirrors] as IEnumerable<Uri>;
            }
            set
            {
                Properties[KnownProperties.Mirrors] = value;
                OnPropertyChanged(KnownProperties.Mirrors);
            }
        }

        public string LocalFileName
        {
            get
            {
                return Properties[KnownProperties.LocalFileName] as string;
            }
            set
            {
                Properties[KnownProperties.LocalFileName] = value;
                OnPropertyChanged(KnownProperties.LocalFileName);
            }
        }


        public class KnownProperties
        {
            public const string Uri = "Uri";
            public const string Mirrors = "Mirrors";
            public const string LocalFileName = "LocalFileName";
        }
    }
}
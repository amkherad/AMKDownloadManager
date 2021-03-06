﻿using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.Network;
using AMKsGear.Core.Collections;
using AMKsGear.Core.Patterns.Mvvm;

// ReSharper disable ExplicitCallerInfoArgument

namespace AMKDownloadManager.Core.Api.Transport
{
    /// <summary>
    /// EventHandler for DownloadItem protocol selection signal.
    /// </summary>
    public delegate void DownloadItemProtocolSelected(
        DownloadItem di,
        IProtocolProvider protocolProvider
    );

    /// <summary>
    /// EventHandler for DownloadItem request created signal.
    /// </summary>
    public delegate void DownloadItemRequestCreated(
        DownloadItem di,
        IRequest request
    );

    /// <summary>
    /// DownloadItem stores information about downloading resource.
    /// </summary>
    public class DownloadItem : ViewModelBase
    {
        public event DownloadItemProtocolSelected ProtocolSelected;
        public event DownloadItemRequestCreated RequestCreated;

        /// <summary>
        /// Stores all download resource properties as key-value pairs.
        /// </summary>
        /// <value>The properties.</value>
        public PropertyBag Properties { get; }

        public DownloadItem()
        {
            Properties = new PropertyBag();
        }

        public DownloadItem(Uri uri)
        {
            Properties = new PropertyBag();

            Uri = uri;
            RedirectionStack.Push(uri);
        }

        /// <summary>
        /// Raises the protocol selected event.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="protocolProvider">Protocol provider.</param>
        public void OnProtocolSelected(DownloadItem downloadItem, IProtocolProvider protocolProvider)
            => ProtocolSelected?.Invoke(downloadItem, protocolProvider);

        /// <summary>
        /// Raises the request created event.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="request">Request.</param>
        public void OnRequestCreated(DownloadItem downloadItem, IRequest request)
            => RequestCreated?.Invoke(downloadItem, request);

        /// <summary>
        /// Gets or sets the URI of the dowonload resource.
        /// </summary>
        /// <value>The URI.</value>
        public Uri Uri
        {
            get { return Properties[KnownProperties.Uri] as Uri; }
            set
            {
                Properties[KnownProperties.Uri] = value;
                OnPropertyChanged(KnownProperties.Uri);
            }
        }

        /// <summary>
        /// Gets or sets the URI of the dowonload resource.
        /// </summary>
        /// <value>The URI.</value>
        public Stack<Uri> RedirectionStack
        {
            get
            {
                var stack = Properties[KnownProperties.RedirectionStack] as Stack<Uri>;
                if (stack == null)
                {
                    stack = new Stack<Uri>();
                    Properties[KnownProperties.RedirectionStack] = stack;
                }

                return stack;
            }
        }

        public void Redirect(Uri uri)
        {
            Uri = uri;
            RedirectionStack.Push(uri);
        }

        /// <summary>
        /// Gets or sets the list of download mirrors.
        /// </summary>
        /// <value>The mirrors.</value>
        public IEnumerable<Uri> Mirrors
        {
            get { return Properties[KnownProperties.Mirrors] as IEnumerable<Uri>; }
            set
            {
                Properties[KnownProperties.Mirrors] = value;
                OnPropertyChanged(KnownProperties.Mirrors);
            }
        }

        /// <summary>
        /// Gets or sets the name of the local file on file system.
        /// </summary>
        /// <value>The name of the local file.</value>
        public string LocalFileName
        {
            get { return Properties[KnownProperties.LocalFileName] as string; }
            set
            {
                Properties[KnownProperties.LocalFileName] = value;
                OnPropertyChanged(KnownProperties.LocalFileName);
            }
        }

        /// <summary>
        /// Gets or sets the interface used for this item.
        /// </summary>
        /// <value>The <see cref="NetworkInterfaceContext"/> provides information about the interface used to download the resource.</value>
        public NetworkInterfaceContext Interface
        {
            get { return Properties[KnownProperties.Interface] as NetworkInterfaceContext; }
            set
            {
                Properties[KnownProperties.Interface] = value;
                OnPropertyChanged(KnownProperties.Interface);
            }
        }

        /// <summary>
        /// Gets or sets the http proxy used for this item.
        /// </summary>
        /// <value>The <see cref="HttpProxyDescriptor"/> object.</value>
        public HttpProxyDescriptor HttpProxy
        {
            get { return Properties[KnownProperties.HttpProxy] as HttpProxyDescriptor; }
            set
            {
                Properties[KnownProperties.HttpProxy] = value;
                OnPropertyChanged(KnownProperties.HttpProxy);
            }
        }

#if DEBUG
        public string DebugName { get; set; }
        public string GetDebugName() => DebugName;
#endif

        /// <summary>
        /// DownloadItem known properties.
        /// </summary>
        public class KnownProperties
        {
            public const string OriginalUri = "OriginalUri";
            public const string Uri = "Uri";
            public const string RedirectionStack = "RedirectionStack";
            public const string Method = "Method";
            public const string Mirrors = "Mirrors";
            public const string LocalFileName = "LocalFileName";
            public const string Interface = "Interface";
            public const string HttpProxy = "HttpProxy";
        }
    }
}
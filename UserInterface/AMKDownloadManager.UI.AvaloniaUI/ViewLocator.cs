// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using AMKDownloadManager.UI.Business.ViewModels;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKsGear.Core.Patterns.AppModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AppContext = AMKDownloadManager.Core.AppContext;

namespace AMKDownloadManager.UI.AvaloniaUI
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;
        
        public ITypeResolver TypeResolver { get; }

        public ViewLocator()
        {
            TypeResolver = AppContext.Instance.GetTypeResolver();
        }
        public ViewLocator(ITypeResolver typeResolver)
        {
            TypeResolver = typeResolver;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)TypeResolver.Resolve(type);
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
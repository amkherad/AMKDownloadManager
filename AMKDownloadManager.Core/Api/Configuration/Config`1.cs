using System;
using System.ComponentModel;

namespace AMKDownloadManager.Core.Api.Configuration
{
    public class Config<TValue> : IConfig
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        
        public IConfigProvider ConfigProvider { get; }
        
        public string Name { get; set; }
        public TValue Value { get; set; }

        public Config(IConfigProvider configProvider, string name)
        {
            ConfigProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            
            if (name == null) throw new ArgumentNullException(nameof(name));
        }
        
        public object GetValue() => Value;

        public void SetValue(object value)
        {
            Value = (TValue)value;
        }
    }
}
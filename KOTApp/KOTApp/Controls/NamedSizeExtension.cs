using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KOTApp.Controls
{
    [ContentProperty(nameof(NamedSize))]
    public class NamedSizeExtension : IMarkupExtension
    {
        public NamedSize NamedSize { get; set; }
        public Type TargetType { get; set; } = typeof(Label);
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Device.GetNamedSize(NamedSize, TargetType);
        }
    }
}

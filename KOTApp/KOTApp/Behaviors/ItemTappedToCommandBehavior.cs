using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBehaviors
{
    public class ItemTappedToCommandBehavior : Behavior<ListView>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                propertyName: "Command",
                returnType: typeof(ICommand),
                declaringType: typeof(ItemTappedToCommandBehavior)

                );

        public static readonly BindableProperty CommandParameterProperty =
           BindableProperty.Create(
                propertyName: "CommandParameterProperty",
                returnType: typeof(object),
                declaringType: typeof(ItemTappedToCommandBehavior)
               );

        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }
            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemTapped += Bindable_ItemTapped;
            bindable.BindingContextChanged += Bindable_BindingContextChanged;    
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            var lv = sender as ListView;
            BindingContext = lv?.BindingContext;
        }

        private void Bindable_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Command.Execute(this.CommandParameter !=null? this.CommandParameter:null );
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemTapped -= Bindable_ItemTapped;
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        }
    }
}

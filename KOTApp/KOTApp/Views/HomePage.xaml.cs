﻿using KOTApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KOTApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        public HomePageVM viewModel { get; set; }
		public HomePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new HomePageVM();
		}
	}
}
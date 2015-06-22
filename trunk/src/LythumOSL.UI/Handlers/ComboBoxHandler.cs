using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

using LythumOSL.Core.Metadata;

namespace LythumOSL.UI.Handlers
{
	public class ComboBoxHandler : ILythumBase
	{
		#region Attributes

		IEnumerable _ItemsSource;
		bool _BindingIsInitialized;

		#endregion

		#region Properties

		public IEnumerable ItemsSource
		{
			get
			{
				return _ItemsSource;
			}
			set
			{
				_ItemsSource = value;

				SourceSetup ();
			}
		}

		public ComboBox ComboBox { get; protected set; }
		public Binding Binding { get; protected set; }
		public IValueConverter Converter { get; set; }

		#endregion

		#region Ctor

		public ComboBoxHandler (ComboBox comboBox, IEnumerable itemsSource)
		{
			LythumOSL.Core.Validation.RequireValid (comboBox, "comboBox");

			this.ComboBox = comboBox;
			this.ItemsSource = itemsSource;
			this.Converter = null;
			this.Binding = null;

			_BindingIsInitialized = false;
		}

		public ComboBoxHandler (ComboBox comboBox)
			: this (comboBox, null)
		{
		}

		#endregion

		#region Methods

		void SourceSetup ()
		{
			this.Binding = new Binding ();

			if (Converter != null)
			{
				this.Binding.Converter = Converter;
			}

			this.Binding.Source = _ItemsSource;

			//ComboBox.ItemsSource = _ItemsSource;
			ComboBox.SetBinding (ComboBox.ItemsSourceProperty, Binding);

			_BindingIsInitialized = true;
		}

		#endregion
	}
}

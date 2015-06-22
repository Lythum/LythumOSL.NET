using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;

namespace LythumOSL.UI.Controls
{
	/// <summary>
	/// Interaction logic for PropertyItem.xaml
	/// </summary>
	public partial class LythumPropertyGridItem : UserControl
	{

		public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register ("PropertyName", typeof (string), typeof (LythumPropertyGridItem));
		public static readonly DependencyProperty PropertyValueProperty = DependencyProperty.Register ("PropertyValue", typeof (object), typeof (LythumPropertyGridItem));
		public static readonly DependencyProperty PropertyDescriptionProperty = DependencyProperty.Register ("PropertyDescription", typeof (string), typeof (LythumPropertyGridItem));
		public static readonly DependencyProperty PropertyCategoryProperty = DependencyProperty.Register ("PropertyCategory", typeof (string), typeof (LythumPropertyGridItem));

		public EventHandler<DescriptionEventArgs> DescriptionEventHandler;
		public event EventHandler<DescriptionEventArgs> OnActive;


		public LythumPropertyGridItem ()
		{
			InitializeComponent ();
			PropertyItemGrid.DataContext = this;
		}

		public string PropertyName
		{

			get { return (string)GetValue (LythumPropertyGridItem.PropertyNameProperty); }
			set { SetValue (LythumPropertyGridItem.PropertyNameProperty, value); }
		}

		public object PropertyValue
		{
			get { return (string)GetValue (LythumPropertyGridItem.PropertyValueProperty); }
			set { SetValue (LythumPropertyGridItem.PropertyValueProperty, value); }
		}

		public string PropertyDescription
		{
			get { return (string)GetValue (LythumPropertyGridItem.PropertyDescriptionProperty); }
			set { SetValue (LythumPropertyGridItem.PropertyDescriptionProperty, value); }
		}

		public string PropertyCategory
		{
			get { return (string)GetValue (LythumPropertyGridItem.PropertyCategoryProperty); }
			set { SetValue (LythumPropertyGridItem.PropertyCategoryProperty, value); }
		}

		#region events
		private void TextBox_MouseEnter (object sender, MouseEventArgs e)
		{
			if (OnActive != null)
			{
				OnActive (this, new DescriptionEventArgs (PropertyDescription));
			}
		}
		#endregion
	}

	public class DescriptionEventArgs : EventArgs
	{
		public string Description { get; set; }

		public DescriptionEventArgs (string descr)
		{
			this.Description = descr;
		}
	}
}

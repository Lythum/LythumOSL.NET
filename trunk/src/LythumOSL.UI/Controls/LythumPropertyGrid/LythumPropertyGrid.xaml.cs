using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;

namespace LythumOSL.UI.Controls
{
	public partial class LythumPropertyGrid : HeaderedContentControl
	{
		private object selectedObject = null;

		public LythumPropertyGrid ()
		{
			InitializeComponent ();
			searchTextBox.TextChanged += new TextChangedEventHandler (searchTextBox_TextChanged);
		}

		#region PropertyGrid related stuff
		public object SelectedObject
		{
			get { return selectedObject; }
			set { selectedObject = value; SelectedObjectHelper (selectedObject, null); }
		}

		public void SelectedObjectHelper (object value, EventArgs e)
		{
			if (!Application.Current.Dispatcher.CheckAccess ())
			{
				Application.Current.Dispatcher.BeginInvoke (System.Windows.Threading.DispatcherPriority.Normal,
														   new EventHandler (SelectedObjectHelper), value, e);
			}
			else
			{
				this.PropertyPanel.Children.Clear (); //clear propertypanel

				foreach (PropertyDescriptor property in TypeDescriptor.GetProperties (value))
				{
					if (!property.IsBrowsable) continue; //could also check for browsableattribute, but this one's shorter

					LythumPropertyGridItem currentProperty = new LythumPropertyGridItem ();
					currentProperty.PropertyName = property.Name;
					Binding b = new Binding (property.Name);
					b.Source = selectedObject;
					b.Mode = property.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;

					currentProperty.SetBinding (LythumPropertyGridItem.PropertyValueProperty, b);
					currentProperty.OnActive += new EventHandler<DescriptionEventArgs> (currentProperty_OnActive);

					foreach (Attribute attribute in property.Attributes)
					{
						if (attribute.GetType () == typeof (DescriptionAttribute))
						{
							currentProperty.PropertyDescription = ((DescriptionAttribute)attribute).Description;
						}
						if (attribute.GetType () == typeof (CategoryAttribute))
						{
							currentProperty.PropertyCategory = ((CategoryAttribute)attribute).Category;
						}
					}
					PropertyPanel.Children.Add (currentProperty); //add the propertyitem
				}
			}
		}
		#endregion
		#region events
		void searchTextBox_TextChanged (object sender, TextChangedEventArgs e)
		{
			string filterText = searchTextBox.Text.ToLower (); //we don't want to be case sensitive
			foreach (LythumPropertyGridItem pi in PropertyPanel.Children)
			{   //hide PropertyItem if it does not contain filter value
				pi.Visibility = (pi.PropertyName.ToLower ().Contains (filterText) || filterText.Equals (string.Empty)) ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		void currentProperty_OnActive (object sender, DescriptionEventArgs e)
		{
			if (!Application.Current.Dispatcher.CheckAccess ())
			{
				Application.Current.Dispatcher.BeginInvoke (System.Windows.Threading.DispatcherPriority.Normal,
														   new EventHandler<DescriptionEventArgs> (currentProperty_OnActive), sender, e);
			}
			else
			{
				this.descriptionTextBlock.Text = e.Description;
			}
		}
		#endregion
	}
}

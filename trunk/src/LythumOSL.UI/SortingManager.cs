using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace LythumOSL.UI
{
	public class SortingManager
	{
		object _SortingSource;
		List<SortDescription> _SortDesc;

		public object SortingSource
		{
			get
			{
				return _SortingSource;
			}
			set
			{
				_SortingSource = value;
				_SortDesc.Clear();

				if (_SortingSource != null)
				{
					try
					{
						SortDescriptionCollection col = CollectionViewSource.GetDefaultView(value).SortDescriptions;

						if (col != null)
						{
							foreach (SortDescription desc in col)
							{
								SortDescription sd = new SortDescription(
									desc.PropertyName.Clone().ToString(), desc.Direction);

								_SortDesc.Add(sd);
							}
						}
					}
					catch
					{
						_SortDesc = null;
					}
				}
			}
		}

		public SortingManager()
		{
			_SortDesc = new List<SortDescription>();
		}

		public SortingManager(object o)
			: this()
		{
			SortingSource = o;
		}


		public bool ApplySorting(object o)
		{
			if (_SortDesc != null)
			{
				System.Diagnostics.Debug.Write(">>SortManager>>> Has " + _SortDesc.Count + " sorting descriptions...");

				if (_SortDesc.Count > 0)
				{
					foreach(SortDescription desc in _SortDesc)
					{
						try
						{
							CollectionViewSource.GetDefaultView(o).SortDescriptions.Add(desc);
						}
						catch
						{
							return false;
						}
					}
				}

			}

			return false;
		}


	}
}

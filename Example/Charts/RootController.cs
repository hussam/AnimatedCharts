using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace Charts
{
	public class RootController : UITableViewController
	{
		class RootSource : UITableViewSource
		{
			const string CellId = "C";
			readonly string[] titles;
			readonly RootController controller;

			Dictionary<string, UIViewController> examples = new Dictionary<string, UIViewController> {
				{"Counting Label", new CountingLabelViewController()},
				{"Circle Chart", new CircleChartViewController()},
				{"Bar Chart", new BarChartViewController()},
				{"Line Chart", new LineChartViewController()}
			};

			public RootSource(RootController rc)
			{
				titles = examples.Keys.ToArray();
				controller = rc;
			}
					
			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return examples.Count;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				controller.NavigationController.PushViewController (examples[titles[indexPath.Row]], true);
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (CellId);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, CellId);
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}

				cell.TextLabel.Text = titles[indexPath.Row];

				return cell;
			}
		}

		public RootController () : base (UITableViewStyle.Grouped)
		{
			Title = "Animated Charts Example";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			TableView.Source = new RootSource (this);
		}
	}
}
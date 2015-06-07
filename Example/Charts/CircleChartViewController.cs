using System;

using Foundation;
using UIKit;

using CoreGraphics;

using AnimatedCharts;

namespace Charts
{
	public class CircleChartViewController : UIViewController
	{
		public CircleChartViewController ()
		{
			Title = "Circle Chart";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.BackgroundColor = UIColor.White;

			var chart = new CircleChart (new CGRect (0, 100, UIScreen.MainScreen.Bounds.Width, 100), true, 15);
			chart.BackgroundColor = UIColor.Clear;

			var txt = new UITextView (new CGRect (50, 200, 50, 50));
			txt.Text = "60";

			var btn = new UIButton (new CGRect (100, 200, 200, 100));
			btn.SetTitle ("Change Value", UIControlState.Normal);
			btn.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btn.TouchUpInside += (sender, e) => {
				float newValue;
				if (float.TryParse (txt.Text, out newValue)) {
					chart.ChangeValue (newValue, true);
				}
			};

			View.AddSubviews(chart, txt, btn);
		}
	}
}

using System;

using Foundation;
using UIKit;

using AnimatedCharts;
using CoreGraphics;

namespace Charts
{
	public class CountingLabelViewController : UIViewController
	{
		public CountingLabelViewController ()
		{
			Title = "Counting Label";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.BackgroundColor = UIColor.White;

			var lbl = new CountingLabel (new CGRect (100, 200, 100, 100));
			lbl.Text = "Any Text";
			lbl.CountingMethod = CountingMethod.EaseInOut;
			lbl.Finished += (sender, e) => Console.WriteLine("Finished Counting");

			var btn = new UIButton (new CGRect (100, 400, 200, 100));
			btn.SetTitle ("Start Counting", UIControlState.Normal);
			btn.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btn.TouchUpInside += (sender, e) => lbl.CountFrom (1, 100, 3);

			View.AddSubview (lbl);
			View.AddSubview (btn);
		}
	}
}


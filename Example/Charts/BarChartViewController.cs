using System;

using Foundation;
using UIKit;

using CoreGraphics;
using System.Collections.Generic;
using AnimatedCharts;
 

namespace Charts
{
	public class BarChartViewController : UIViewController
	{
		public BarChartViewController ()
		{
			Title = "Bar Chart!";
		}

		List<string> xLabels = new List<string> { "MON", "TUE", "WED", "THU", "FRI" };
		List<nfloat> yLabels = new List<nfloat> ( new nfloat[] { (nfloat) 40.0, (nfloat) 30.0, (nfloat) 80.0, (nfloat) 45.0, (nfloat) 32.0 });
		List<nfloat> yLabels2 = new List<nfloat>(new nfloat[] { (nfloat) 30.0, (nfloat) 80.0, (nfloat) 55.0, (nfloat) 20.0, (nfloat) 90.0 });

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var lastClick = true;
			View.BackgroundColor = UIColor.White;

			var chart = new BarChart (new CGRect ((nfloat) 0.0, (nfloat) 75.0, UIScreen.MainScreen.Bounds.Width, (nfloat) 0.5 * UIScreen.MainScreen.Bounds.Height),  xLabels, 100);

			var btn = new UIButton (new CGRect (100, 500, 200, 100)); 
			btn.SetTitle ("Change Value", UIControlState.Normal);
			btn.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btn.TouchUpInside += (sender, e) => {
				if (lastClick)  { 
					chart.ChangeValues (yLabels2); 
					lastClick = false;
				}
				else {
					chart.ChangeValues (yLabels); 
					lastClick = true;
				}

			};

			View.AddSubviews( chart, btn );
		}
	}
}


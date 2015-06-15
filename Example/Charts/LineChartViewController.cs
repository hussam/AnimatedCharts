using System;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using AnimatedCharts;

namespace Charts
{
	public class LineChartViewController : UIViewController
	{
		public LineChartViewController ()
		{
			Title = "Line Chart!";
		}
		List<string> xLabels = new List<string> { "MON", "TUE", "WED", "THU", "FRI" };
		List<nfloat> yLabels = new List<nfloat> ( new nfloat[] { (nfloat) 50.0, (nfloat) 30.0, (nfloat) 80.0, (nfloat) 45.0, (nfloat) 32.0 });
		List<nfloat> yLabels2 = new List<nfloat>(new nfloat[] { (nfloat) 25.0, (nfloat) 45.0, (nfloat) 10.0, (nfloat) 30.0, (nfloat) 90.0 });

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.White;

			var chart = new LineChart (new CGRect ((nfloat) 0.0, (nfloat) 75.0, UIScreen.MainScreen.Bounds.Width, (nfloat) 0.5 * UIScreen.MainScreen.Bounds.Height));

			var btn = new UIButton (new CGRect (100, (nfloat) 0.6 * UIScreen.MainScreen.Bounds.Height, 200, 100)); 
			btn.SetTitle ("Draw Background", UIControlState.Normal);
			btn.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btn.TouchUpInside += (sender, e) => {
				chart.DrawBackground(xLabels, 100);
			};

			var btn2 = new UIButton (new CGRect (100, (nfloat) 0.7 * UIScreen.MainScreen.Bounds.Height, 200, 100)); 
			btn2.SetTitle ("Draw Green Line", UIControlState.Normal);
			btn2.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btn2.TouchUpInside += (sender, e) => {
				chart.StrokeColor = UIColor.Green;
				chart.DrawLine(yLabels);
			};
			var btn3 = new UIButton (new CGRect (100, (nfloat) 0.8 * UIScreen.MainScreen.Bounds.Height, 200, 100)); 
			btn3.SetTitle ("Draw Red Line", UIControlState.Normal);
			btn3.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btn3.TouchUpInside += (sender, e) => {
				chart.StrokeColor = UIColor.Red;
				chart.DrawLine(yLabels2);
			};
			View.AddSubviews( chart, btn, btn2, btn3 );
		}

	}
}


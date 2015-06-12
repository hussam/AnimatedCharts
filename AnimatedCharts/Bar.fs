namespace AnimatedCharts

open System
open UIKit
open CoreGraphics
open CoreAnimation
open Foundation
open Extensions


type private Bar(frame : CGRect ) as this = 
    inherit UIView(frame) 

    let mutable barRadius = 6.0

    let mutable value = nfloat 0.0
    let maxValue = nfloat 100.0

    let chartLine = new CAShapeLayer()
    let progressLine = new UIBezierPath()

    let backLine = new CAShapeLayer()
    let backPath = new UIBezierPath()

    let changePathAnimation = CABasicAnimation.FromKeyPath("path")

    do
        // create background
        backLine.LineCap <- CAShapeLayer.CapButt
        backLine.LineWidth <- this.Frame.Size.Width
        backLine.StrokeStart <- nfloat 0.0
        backLine.StrokeEnd <- nfloat 1.0
        backLine.ZPosition <- nfloat -1.0
        backLine.StrokeColor <- UIColor.PNLightGrey.CGColor
        this.Layer.AddSublayer(backLine)
        backPath.MoveTo(new CGPoint(this.Frame.Size.Width / nfloat 2.0, this.Frame.Size.Height))
        backPath.AddLineTo(new CGPoint(this.Frame.Size.Width /nfloat 2.0, nfloat 0.0))
        backLine.Path <- backPath.CGPath

        //create line that shows value
        chartLine.LineCap <- CAShapeLayer.CapButt
        chartLine.FillColor <- UIColor.White.CGColor
        chartLine.LineWidth <- this.Frame.Size.Width
        chartLine.StrokeStart <- nfloat 0.0
        chartLine.StrokeEnd <- nfloat 0.0
        chartLine.ZPosition <- nfloat 1.0
        this.ClipsToBounds <- true
        this.Layer.AddSublayer(chartLine)
        this.Layer.CornerRadius <- nfloat barRadius

        progressLine.MoveTo(new CGPoint(this.Frame.Size.Width / nfloat 2.0, this.Frame.Size.Height))
        progressLine.AddLineTo(new CGPoint(this.Frame.Size.Width / nfloat 2.0, nfloat 0.0))
        chartLine.Path <- progressLine.CGPath

    // Properties
    member this.Value with get() = value
    member this.MaxValue with get() = maxValue
    member this.BarRadius with get() = barRadius and set(value) = barRadius <- value
    member val AnimationDuration = 1.0 with get, set
    member val StrokeColor = UIColor.PNFreshGreen with get, set

    member this.ChangeValue(newValue : nfloat ) =

        let startPct = value / maxValue
        let endPct = newValue / maxValue
        value <- newValue

        chartLine.StrokeColor <- this.StrokeColor.CGColor
        chartLine.StrokeEnd <- endPct

        changePathAnimation.Duration <- this.AnimationDuration
        changePathAnimation.From <- NSNumber.FromNFloat( startPct )
        changePathAnimation.To <- NSNumber.FromNFloat( endPct )
        changePathAnimation.TimingFunction <- CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut)
        chartLine.AddAnimation(changePathAnimation, "animationKey")
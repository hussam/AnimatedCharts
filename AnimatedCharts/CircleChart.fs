namespace AnimatedCharts

open System
open UIKit
open CoreGraphics
open CoreAnimation
open Foundation
open Extensions

type ChartFormat = Percent = 0 | Dollar = 1 | None = 2

type CircleChart(frame : CGRect, clockwise, shadowColor : UIColor, displayCountingLabel, lineWidth) as this = 
    inherit UIView(frame)

    let mutable value = 0.0
    let mutable maxValue = 100.0

    let lineWidth = lineWidth
    let displayCountingLabel = displayCountingLabel

    let startAngle = if clockwise then -90.0 else 270.0
    let endAngle = if clockwise then -90.01 else 270.01

    let arcCenter = new CGPoint(frame.Size.Width / nfloat 2.0, frame.Size.Height / nfloat 2.0)
    let arcRadius = (frame.Size.Height * nfloat 0.5) - nfloat (lineWidth / 2.0)
    let degreesToRadius angle = nfloat (angle / 180.0 * Math.PI)
    let circlePath = UIBezierPath.FromArc(arcCenter, arcRadius, degreesToRadius(startAngle), degreesToRadius(endAngle), clockwise)

    let circle = new CAShapeLayer()
    let circleBackground = new CAShapeLayer()
    let countingLabel = new CountingLabel(new CGRect(0.0, 0.0, 100.0, 50.0))

    let mutable gradientMask : CAShapeLayer = null

    do
        // Initialize properties
        circle.Path <- circlePath.CGPath
        circle.LineCap <- CAShapeLayer.CapRound
        circle.FillColor <- UIColor.Clear.CGColor
        circle.LineWidth <- nfloat lineWidth
        circle.ZPosition <- nfloat 1.0
        circle.StrokeEnd <- nfloat 0.0

        circleBackground.Path <- circlePath.CGPath
        circleBackground.LineCap <- CAShapeLayer.CapRound
        circleBackground.FillColor <- UIColor.Clear.CGColor
        circleBackground.LineWidth <- nfloat lineWidth
        circleBackground.StrokeColor <- shadowColor.CGColor
        circleBackground.StrokeEnd <- nfloat 1.0
        circleBackground.ZPosition <- nfloat -1.0

        this.Layer.AddSublayer(circle)
        this.Layer.AddSublayer(circleBackground)

        countingLabel.TextAlignment <- UITextAlignment.Center
        countingLabel.Font <- UIFont.BoldSystemFontOfSize(nfloat 16.0)
        countingLabel.TextColor <- UIColor.Gray
        countingLabel.BackgroundColor <- UIColor.Clear
        countingLabel.Center <- new CGPoint(this.Frame.Size.Width / nfloat 2.0, this.Frame.Size.Height / nfloat 2.0)
        countingLabel.CountingMethod <- CountingMethod.EaseInOut
        if displayCountingLabel then
            this.AddSubview(countingLabel)


    // Other constructors
    new(frame, clockwise, shadowColor, displayCountingLabel) = new CircleChart(frame, clockwise, shadowColor, displayCountingLabel, 8.0)
    new(frame, clockwise, shadowColor) = new CircleChart(frame, clockwise, shadowColor, true)
    new(frame, clockwise) = new CircleChart(frame, clockwise, UIColor.PNGrey)

    // Properties
    member this.Value with get() = value
    member this.MaxValue with get() = maxValue
    member val AnimationDuration = 1.0 with get, set
    member val ChartType = ChartFormat.Percent with get, set
    member val StrokeColor = UIColor.PNFreshGreen with get, set
    member val StrokeColorGradientStart : UIColor = null with get, set

    // Methods
    member this.ChangeValue(newValue, animate) =
        // Setup
        let startPct = value / maxValue
        let endPct = newValue / maxValue
        value <- newValue

        if displayCountingLabel then
            let formatStr = match this.ChartType with
                            | ChartFormat.Percent -> "{0:P0}"
                            | ChartFormat.Dollar -> "{0:C2}"
                            | _ -> "{0:D}"
            countingLabel.Formatter <- CountFormatter(fun value -> String.Format(formatStr, value))
            this.AddSubview(countingLabel)
        
        // Add circle params
        circle.LineWidth <- nfloat lineWidth
        circle.StrokeColor <- this.StrokeColor.CGColor
        circle.StrokeEnd <- nfloat endPct

        circleBackground.LineWidth <- nfloat lineWidth
        circleBackground.StrokeEnd <- nfloat 1.0

        let pathAnimation = CABasicAnimation.FromKeyPath("strokeEnd")
        if animate then
            // Add animation
            pathAnimation.Duration <- this.AnimationDuration
            pathAnimation.TimingFunction <- CAMediaTimingFunction.FromName( CAMediaTimingFunction.EaseInEaseOut )
            pathAnimation.From <- NSNumber.FromNFloat(nfloat startPct)
            pathAnimation.To <- NSNumber.FromNFloat(nfloat endPct)
            circle.AddAnimation(pathAnimation, "strokeEndAnimation")
            countingLabel.CountFrom(startPct, endPct, this.AnimationDuration)
        else
            countingLabel.Text <- countingLabel.Formatter.Invoke(endPct)

        // Add a gradient from start color to the bar color?
        if this.StrokeColorGradientStart <> null then
            gradientMask <- new CAShapeLayer()
            gradientMask.FillColor <- UIColor.Clear.CGColor
            gradientMask.StrokeColor <- UIColor.Black.CGColor
            gradientMask.LineWidth <- circle.LineWidth
            gradientMask.LineCap <- CAShapeLayer.CapRound
            let gradientFrame = new CGRect(nfloat 0.0, nfloat 0.0, nfloat 2.0 * this.Bounds.Size.Width, nfloat 2.0 * this.Bounds.Size.Height)
            gradientMask.Frame <- gradientFrame
            gradientMask.Path <- circle.Path

            let gradientLayer = new CAGradientLayer()
            gradientLayer.StartPoint <- new CGPoint(0.5, 1.0)
            gradientLayer.EndPoint <- new CGPoint(0.5, 0.0)
            gradientLayer.Frame <- gradientFrame
            gradientLayer.Colors <- [| this.StrokeColor.CGColor ; this.StrokeColorGradientStart.CGColor |]
            gradientLayer.Mask <- gradientMask
            circle.AddSublayer(gradientLayer)

            gradientMask.StrokeEnd <- nfloat endPct
            if animate then gradientMask.AddAnimation(pathAnimation, "strokeEndAnimation")
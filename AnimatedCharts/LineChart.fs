
namespace AnimatedCharts

open System
open UIKit
open CoreGraphics
open CoreAnimation
open Foundation
open Extensions
open System.Collections.Generic


type LineChart (frame : CGRect ) as this =
    inherit UIView(frame)

    let width = this.Frame.Size.Width
    let height = this.Frame.Size.Height
    let sideMargin = nfloat 0.1 * width
    let vertMargin = nfloat 0.05 * height
    let horizFrame = nfloat 0.8 * width
    let vertFrame = nfloat 0.9 * height
    let mutable count = 0
    let mutable classXLabels = List<string>()
    let mutable classYLabels = List<nfloat>()
    let mutable maxValue = nfloat 100.0
    let mutable bottomLabelWidth = nfloat 0.0
    let mutable backgroundDrawn = false
      
    // Properties
    member val StrokeColor = UIColor.PNFreshGreen with get, set
    member val LineWidth = nfloat 5.0 with get, set
    member val ShowAxes = true with get, set
    member val ShowBottomLabels = true with get, set
    member val ShowSideLabels = true with get, set
    member val AnimationTime = 2.0 with get, set

    member this.DrawBackground (xLabels : List<string>, max : int ) = 
        classXLabels <- xLabels
        maxValue <- nfloat ( float ( max ) )
        count <- 0
        bottomLabelWidth <- horizFrame / nfloat ( float ( classXLabels.Count ) )
        backgroundDrawn <- true

        for item in xLabels do
            if this.ShowBottomLabels then
                let bottomLabelYPos = vertFrame + vertMargin * nfloat 1.2
                let bottomLabelsXPos = nfloat(float(count)) * bottomLabelWidth + sideMargin
                let frame1 = new CGRect( bottomLabelsXPos, bottomLabelYPos, bottomLabelWidth, vertMargin )
                let lbl = new UILabel(frame1)
                let text = xLabels.[count]
                lbl.Text <- text
                lbl.TextAlignment <- UITextAlignment.Center
                this.AddSubview (lbl)

            if this.ShowSideLabels && count < 4 then                
                let yPosition = (nfloat(float(count)) * vertFrame / nfloat 4.0) + vertMargin 
                let sideFrame = new CGRect(nfloat 0.0, yPosition, sideMargin, vertMargin) 
                let sideLabel = new UILabel(sideFrame)
                let sideValue : int = (max - count * max / 4)
                let text = sideValue.ToString()
                sideLabel.Text <- text
                sideLabel.TextAlignment <- UITextAlignment.Center
                this.AddSubview(sideLabel)

            if this.ShowAxes then
                let chartBottomLine = new CAShapeLayer()
                let chartSideLine = new CAShapeLayer()
                let bottomPath = new UIBezierPath()
                let sidePath = new UIBezierPath()

                bottomPath.MoveTo( new CGPoint ( sideMargin, vertMargin + vertFrame))
                bottomPath.AddLineTo ( new CGPoint ( sideMargin * nfloat 1.5 + horizFrame, vertMargin + vertFrame)) 

                sidePath.MoveTo( new CGPoint ( sideMargin, vertMargin + vertFrame))
                sidePath.AddLineTo( new CGPoint ( sideMargin, nfloat 0.5 * vertMargin))  

                chartBottomLine.StrokeColor <- UIColor.PNLightGrey.CGColor
                chartSideLine.StrokeColor <- UIColor.PNLightGrey.CGColor

                chartBottomLine.Path <- bottomPath.CGPath
                chartSideLine.Path <- sidePath.CGPath

                this.Layer.AddSublayer( chartBottomLine )
                this.Layer.AddSublayer( chartSideLine )

            count <- count + 1

    member this.DrawLine (yLabels : List<nfloat>) = 
        if backgroundDrawn then 
            count <- 0
            classYLabels <- yLabels
            let line = new CAShapeLayer()
            let path = new UIBezierPath()
               
            for item in yLabels do
                let xPosition = (nfloat(float(count))*bottomLabelWidth) + sideMargin + nfloat 0.5 * bottomLabelWidth
                let yPosition = vertMargin + vertFrame * ( nfloat 1.0 - item /  maxValue) 
                if count = 0 then
                    path.MoveTo ( new CGPoint( xPosition, yPosition ) )
                else 
                    path.AddLineTo ( new CGPoint ( xPosition, yPosition ) )
                count <- count + 1
            
            line.LineCap <- CAShapeLayer.CapRound
            line.FillColor <- UIColor.Clear.CGColor
            this.Layer.AddSublayer(line)
            line.Path <- path.CGPath
            line.LineWidth <- this.LineWidth
            line.StrokeColor <- this.StrokeColor.CGColor
            line.StrokeEnd <- nfloat 1.0

            let animation = CABasicAnimation.FromKeyPath("strokeEnd")
            animation.Duration <- this.AnimationTime
            animation.From <- NSNumber.FromNFloat( nfloat 0.0 )
            animation.To <- NSNumber.FromNFloat( nfloat 1.0 )
            animation.TimingFunction <- CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut)
            line.AddAnimation(animation, "strokeEndAnimation")
          
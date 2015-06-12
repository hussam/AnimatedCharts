

namespace AnimatedCharts

open System
open UIKit
open CoreGraphics
open CoreAnimation
open Foundation
open Extensions
open System.Collections.Generic


type BarChart(frame : CGRect, xLabels : List<string>, max : int ) as this=
    inherit UIView(frame)
     
    //let xList = List.ofSeq(xLabels)
    let sideMargins = nfloat 0.1 * this.Frame.Size.Width
    let vertMargins = nfloat 0.05 * this.Frame.Size.Height
    let numLabels = nfloat( float (xLabels.Count))
    let barSpaceWidth = (nfloat 0.8 * this.Frame.Size.Width) / numLabels
    let barSpaceHeight = nfloat 0.9 * this.Frame.Size.Height
    let actualBarWidth = nfloat 0.5 * barSpaceWidth
    let bars = List<_>()

    let mutable count = 0
    let mutable showAxes = true
    let mutable showBottomLabels = true
    let mutable showSideLabels = true     
   
    do 
        for item in xLabels do

            let xBarPosition = nfloat (float (count)) * barSpaceWidth + sideMargins + nfloat 0.5 * actualBarWidth
            let bar = new Bar ( new CGRect( xBarPosition, vertMargins, actualBarWidth, nfloat 0.9 * this.Frame.Size.Height))
            bars.Add(bar)
            this.AddSubview( bar )

            if showBottomLabels then
                let bottomLabelsHeight = nfloat 2.0 * vertMargins + barSpaceHeight
                let xPosition = nfloat(float(count)) * barSpaceWidth + sideMargins
                let frame1 = new CGRect( xPosition, bottomLabelsHeight, barSpaceWidth, vertMargins )
                let lbl = new UILabel(frame1)
                let text = xLabels.[count]
                lbl.Text <- text
                lbl.TextAlignment <- UITextAlignment.Center 

                this.AddSubview (lbl)
            
            if showSideLabels && count < 4 then                
                // starts making the labels from the top
                let yPosition = (nfloat(float(count)) * barSpaceHeight / nfloat 4.0) + vertMargins
                let sideFrame = new CGRect(nfloat 0.0, yPosition, sideMargins, vertMargins) 
                let sideLabel = new UILabel(sideFrame)
                let sideValue : int = (max - count * max / 4)
                let text = sideValue.ToString()
                sideLabel.Text <- text
                sideLabel.TextAlignment <- UITextAlignment.Center

                this.AddSubview(sideLabel)

            if showAxes then
                let chartBottomLine = new CAShapeLayer()
                let chartSideLine = new CAShapeLayer()
                let bottomPath = new UIBezierPath()
                let sidePath = new UIBezierPath()

                bottomPath.MoveTo( new CGPoint ( sideMargins, vertMargins * nfloat 1.5 + barSpaceHeight))
                bottomPath.AddLineTo ( new CGPoint ( sideMargins * nfloat 1.5 + barSpaceWidth * numLabels, vertMargins * nfloat 1.5 + barSpaceHeight)) 
                
                chartBottomLine.StrokeStart <- nfloat 0.0
                chartBottomLine.StrokeEnd <- nfloat 1.0
                chartBottomLine.StrokeColor <- UIColor.PNLightGrey.CGColor

                sidePath.MoveTo( new CGPoint ( sideMargins, vertMargins * nfloat 1.5 + barSpaceHeight))
                sidePath.AddLineTo( new CGPoint ( sideMargins, nfloat 0.5 * vertMargins))  

                chartSideLine.StrokeStart <- nfloat 0.0
                chartSideLine.StrokeEnd <- nfloat 1.0
                chartSideLine.StrokeColor <- UIColor.PNLightGrey.CGColor

                chartBottomLine.Path <- bottomPath.CGPath
                chartSideLine.Path <- sidePath.CGPath

                this.Layer.AddSublayer( chartBottomLine )
                this.Layer.AddSublayer( chartSideLine )

            count <- count + 1

    //Properties
    member this.ShowXLabels with get() = showBottomLabels and set (value) = showBottomLabels <- value
    member this.ShowYLabels with get() = showSideLabels and set (value) = showSideLabels <- value 
    member val ShowChartAxes = showAxes with get, set

    // Methods
    member this.SetChartAxes ( value ) = showAxes <- value
    member this.ChangeValues ( newYValues : List<nfloat>) =
        count <- 0 
        for item : Bar in bars do
            item.ChangeValue ( newYValues.[count] )
            count <- count + 1
        
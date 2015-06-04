// Adapted from https://github.com/dataxpress/UICountingLabel

namespace AnimatedCharts

open System
open System.Timers
open System.Collections.Generic
open CoreGraphics
open Foundation
open UIKit

type CountingMethod = EaseInOut = 0 | EaseIn = 1 | EaseOut = 2 | Linear = 3

type CountFormatter = delegate of float -> string
type CountAttributedFormatter = delegate of float -> NSAttributedString

type CountingLabel(frame : CGRect) as this =
    inherit UILabel(frame)
    let mutable startingValue = 0.0
    let mutable targetValue = 0.0
    let mutable progress = 0L
    let mutable lastUpdate = 0L
    let mutable duration = 0L

    let easingRate = 3.0
    let timer = new Timer(33.0)
    let finished = new Event<EventHandler, EventArgs>()

    let mutable formatter = fun (value : float) -> ()

    let displayedProgress (timedProgress : float) =
        match this.CountingMethod with
        | CountingMethod.Linear ->
            timedProgress
        | CountingMethod.EaseIn ->
            Math.Pow(timedProgress, easingRate)
        | CountingMethod.EaseOut ->
            1.0 - Math.Pow(1.0 - timedProgress, easingRate)
        | _ ->
            let r = int easingRate
            let sign = if r % 2 = 0 then -1.0 else 1.0
            let t = timedProgress * 2.0
            if t < 1.0 then
                0.5 * Math.Pow(t, easingRate)
            else
                sign * 0.5 * (Math.Pow(t-2.0, easingRate) + (sign * 2.0))

    let updateValue eventArgs =
        let now = DateTime.Now.Ticks
        progress <- progress + now - lastUpdate
        lastUpdate <- now

        let nextValue = if progress >= duration then
                            timer.Stop()
                            finished.Trigger(this, EventArgs.Empty)
                            targetValue
                        else
                            let percent = displayedProgress (float progress / float duration)
                            startingValue + (percent * (targetValue - startingValue))

        this.InvokeOnMainThread(fun() -> formatter(nextValue))

    do
        this.TextColor <- UIColor.Black
        timer.Elapsed.Add(updateValue)

    new() = new CountingLabel(CGRect.Empty)

    member val CountingMethod = CountingMethod.Linear with get, set
    member val IntegerSteps = false with get,set
    member val Formatter = null : CountFormatter with get, set
    member val AttributedFormatter = null : CountAttributedFormatter with get, set

    [<CLIEvent>]
    member this.Finished = finished.Publish

    member this.CountFrom(startValue : float, endValue : float, durationInSeconds : float) =
        startingValue <- startValue
        targetValue <- endValue
        progress <- 0L

        formatter <- if this.AttributedFormatter <> null then
                         fun value -> (this.AttributedText <- this.AttributedFormatter.Invoke(value))
                     elif this.Formatter <> null then
                         fun value -> (this.Text <- this.Formatter.Invoke(value))
                     elif this.IntegerSteps then
                         fun value -> (this.Text <- (int value).ToString())
                     else
                         fun value -> (this.Text <- String.Format("{0:f1}", value))

        if durationInSeconds = 0.0 then
            formatter(endValue)
            finished.Trigger(this, EventArgs.Empty)
        else
            lastUpdate <- DateTime.Now.Ticks
            duration <- DateTime.Now.AddSeconds(durationInSeconds).Ticks - lastUpdate
            timer.Start()
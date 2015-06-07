// Adapted from https://github.com/kevinzhow/PNChart

module Extensions

open System
open UIKit
open CoreGraphics

let rgb(r, g, b) = UIColor.FromRGB(nfloat (float r/255.0), nfloat (float g/255.0), nfloat (float b/255.0))

type UIColor with
    static member PNGrey          = rgb(246, 246, 246)
    static member PNLightBlue     = rgb(94, 147, 196)
    static member PNGreen         = rgb(77, 186, 122)
    static member PNTitleColor    = rgb(0, 189, 113)
    static member PNButtonGrey    = rgb(141, 141, 141)
    static member PNLightGreen    = rgb(77, 216, 122)
    static member PNFreshGreen    = rgb(77, 196, 122)
    static member PNDeepGreen     = rgb(77, 176, 122)
    static member PNRed           = rgb(245, 94, 78)
    static member PNMauve         = rgb(88, 75, 103)
    static member PNBrown         = rgb(119, 107, 95)
    static member PNBlue          = rgb(82, 116, 188)
    static member PNDarkBlue      = rgb(121, 134, 142)
    static member PNYellow        = rgb(242, 197, 117)
    static member PNWhite         = rgb(255, 255, 255)
    static member PNDeepGrey      = rgb(99, 99, 99)
    static member PNPinkGrey      = rgb(200, 193, 193)
    static member PNHealYellow    = rgb(245, 242, 238)
    static member PNLightGrey     = rgb(225, 225, 225)
    static member PNCleanGrey     = rgb(251, 251, 251)
    static member PNLightYellow   = rgb(241, 240, 240)
    static member PNDarkYellow    = rgb(152, 150, 159)
    static member PNPinkDark      = rgb(170, 165, 165)
    static member PNCloudWhite    = rgb(244, 244, 244)
    static member PNBlack         = rgb(45, 45, 45)
    static member PNStarYellow    = rgb(252, 223, 101)
    static member PNTwitterColor  = rgb(0, 171, 243)
    static member PNWeiboColor    = rgb(250, 0, 33)
    static member PNiOSGreenColor = rgb(98, 247, 77)

    static member ImageFromColor(color : UIColor) =
        let rect = new CGRect(0.0, 0.0, 1.0, 1.0)
        UIGraphics.BeginImageContext(rect.Size)
        let context = UIGraphics.GetCurrentContext()
        context.SetFillColor(color.CGColor)
        context.FillRect(rect)
        let img = UIGraphics.GetImageFromCurrentImageContext()
        UIGraphics.EndImageContext()
        img
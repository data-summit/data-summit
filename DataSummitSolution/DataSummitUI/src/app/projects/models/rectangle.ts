import { Point } from "./point"

export class Rectangle {

    Tl: Point
    Tr: Point
    Bl: Point
    Br: Point
    Width: number
    Height: number
    Fill: string
    Type: string 

    constructor(tl?: Point, tr?: Point, bl?: Point, br?: Point,
                width?: number, height?: number, fill?: string, type?: string) {
        this.Tl = tl || new Point()
        this.Tr = tr || new Point()
        this.Bl = bl || new Point()
        this.Br = br || new Point()
        this.Width = width || 0
        this.Height = height || 0
        this.Fill = fill || ""
        this.Type = type || ""
    }
}
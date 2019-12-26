import { Rectangle } from "./rectangle"

export class RectanglePair {

    TitleRectangle: Rectangle
    ValueRectangle: Rectangle

    constructor(titlerectangle?: Rectangle, valuerectangle?: Rectangle) {
        this.TitleRectangle = titlerectangle || null
        this.ValueRectangle = valuerectangle || null
    }
}
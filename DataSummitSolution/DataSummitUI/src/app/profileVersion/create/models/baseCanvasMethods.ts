declare const fabric: any;

export class BaseCanvasMethods {

    canvas: any;
    textEditor: boolean;
    imageEditor: boolean;
    figureEditor: boolean;
    json: string;

    props: any = {
        canvasFill: '#ffffff',
        canvasImage: '',
        id: null,
        opacity: null,
        fill: null,
        fontSize: null,
        lineHeight: null,
        charSpacing: null,
        fontWeight: null,
        fontStyle: null,
        textAlign: null,
        fontFamily: null,
        TextDecoration: ''
    };
    selected: any;
    width: number;
    height: number;

      //initiailise the canvas
  initCanvas() {
    this.canvas = new fabric.Canvas('canvas', {
      hoverCursor: 'pointer',
      selection: true,
      selectionBorderColor: 'blue'
    });

    this.canvas.on({
      'object:moving': () => { },
      'object:modified': () => { },
      'object:selected': (e) => {

        let selectedObject = e.target;
        this.selected = selectedObject
        selectedObject.hasRotatingPoint = true;
        selectedObject.transparentCorners = false;
        // selectedObject.cornerColor = 'rgba(255, 87, 34, 0.7)';

        this.resetPanels();

        if (selectedObject.type !== 'group' && selectedObject) {

          this.getId();
          this.getOpacity();

          switch (selectedObject.type) {
            case 'rect':
            case 'circle':
            case 'triangle':
              this.figureEditor = true;
              this.getFill();
              break;
            case 'i-text':
              this.textEditor = true;
              this.getLineHeight();
              this.getCharSpacing();
              this.getBold();
              this.getFontStyle();
              this.getFill();
              this.getTextDecoration();
              this.getTextAlign();
              this.getFontFamily();
              break;
            case 'image':
              console.log('image');
              break;
          }
        }
      },
      'selection:cleared': () => {
        this.selected = null;
        this.resetPanels();
      }
    });
  }

    /*General Canvas Methods*/

    cleanSelect() {
        // this.canvas.deactivateAllWithDispatch().renderAll();
    }

    selectItemAfterAdded(obj) {
        // this.canvas.deactivateAllWithDispatch().renderAll();
        this.canvas.setActiveObject(obj);
    }

    extend(obj, id) {
        obj.toObject = (function (toObject) {
            return function () {
                return fabric.util.object.extend(toObject.call(this), {
                    id: id
                });
            };
        })(obj.toObject);
    }

    randomId() {
        return Math.floor(Math.random() * 999999) + 1;
    }

    /*------------------------Global actions for element------------------------*/

    getActiveStyle(styleName, object) {
        object = object || this.canvas.getActiveObject();
        if (!object) return '';

        return (object.getSelectionStyles && object.isEditing)
            ? (object.getSelectionStyles()[styleName] || '')
            : (object[styleName] || '');
    }


    setActiveStyle(styleName, value, object) {
        object = object || this.canvas.getActiveObject();
        if (!object) return;

        if (object.setSelectionStyles && object.isEditing) {
            var style = {};
            style[styleName] = value;
            object.setSelectionStyles(style);
            object.setCoords();
        }
        else {
            object.set(styleName, value);
        }

        object.setCoords();
        this.canvas.renderAll();
    }


    getActiveProp(name) {
        var object = this.canvas.getActiveObject();
        if (!object) return '';

        return object[name] || '';
    }

    setActiveProp(name, value) {
        var object = this.canvas.getActiveObject();
        if (!object) return;
        object.set(name, value).setCoords();
        this.canvas.renderAll();
    }

    clone() {
        let activeObject = this.canvas.getActiveObject();

        if (activeObject) {
            let clone;
            switch (activeObject.type) {
                case 'rect':
                    clone = new fabric.Rect(activeObject.toObject());
                    break;
                case 'circle':
                    clone = new fabric.Circle(activeObject.toObject());
                    break;
                case 'triangle':
                    clone = new fabric.Triangle(activeObject.toObject());
                    break;
                case 'i-text':
                    clone = new fabric.IText('', activeObject.toObject());
                    break;
                case 'image':
                    clone = fabric.util.object.clone(activeObject);
                    break;
            }
            if (clone) {
                clone.set({ left: 10, top: 10 });
                this.canvas.add(clone);
                this.selectItemAfterAdded(clone);
            }
        }
    }

    getId() {
        this.props.id = this.canvas.getActiveObject().toObject().id;
    }

    setId() {
        let val = this.props.id;
        let complete = this.canvas.getActiveObject().toObject();
        console.log(complete);
        this.canvas.getActiveObject().toObject = () => {
            complete.id = val;
            return complete;
        };
    }

    getOpacity() {
        this.props.opacity = this.getActiveStyle('opacity', null) * 100;
    }

    setOpacity() {
        this.setActiveStyle('opacity', parseInt(this.props.opacity) / 100, null);
    }

    getFill() {
        this.props.fill = this.getActiveStyle('fill', null);
    }

    setFill() {
        this.setActiveStyle('fill', this.props.fill, null);
    }

    getLineHeight() {
        this.props.lineHeight = this.getActiveStyle('lineHeight', null);
    }

    setLineHeight() {
        this.setActiveStyle('lineHeight', parseFloat(this.props.lineHeight), null);
    }

    getCharSpacing() {
        this.props.charSpacing = this.getActiveStyle('charSpacing', null);
    }

    setCharSpacing() {
        this.setActiveStyle('charSpacing', this.props.charSpacing, null);
    }

    getFontSize() {
        this.props.fontSize = this.getActiveStyle('fontSize', null);
    }

    setFontSize() {
        this.setActiveStyle('fontSize', parseInt(this.props.fontSize), null);
    }

    getBold() {
        this.props.fontWeight = this.getActiveStyle('fontWeight', null);
    }

    setBold() {
        this.props.fontWeight = !this.props.fontWeight;
        this.setActiveStyle('fontWeight', this.props.fontWeight ? 'bold' : '', null);
    }

    getFontStyle() {
        this.props.fontStyle = this.getActiveStyle('fontStyle', null);
    }

    setFontStyle() {
        this.props.fontStyle = !this.props.fontStyle;
        this.setActiveStyle('fontStyle', this.props.fontStyle ? 'italic' : '', null);
    }


    getTextDecoration() {
        this.props.TextDecoration = this.getActiveStyle('textDecoration', null);
    }

    setTextDecoration(value) {
        let iclass = this.props.TextDecoration;
        if (iclass.includes(value)) {
            iclass = iclass.replace(RegExp(value, "g"), "");
        } else {
            iclass += ` ${value}`
        }
        this.props.TextDecoration = iclass;
        this.setActiveStyle('textDecoration', this.props.TextDecoration, null);
    }

    hasTextDecoration(value) {
        return this.props.TextDecoration.includes(value);
    }


    getTextAlign() {
        this.props.textAlign = this.getActiveProp('textAlign');
    }

    setTextAlign(value) {
        this.props.textAlign = value;
        this.setActiveProp('textAlign', this.props.textAlign);
    }

    getFontFamily() {
        this.props.fontFamily = this.getActiveProp('fontFamily');
    }

    setFontFamily() {
        this.setActiveProp('fontFamily', this.props.fontFamily);
    }

    /*System*/


    removeSelected() {
        let activeObject = this.canvas.getActiveObject()
        // activeGroup = this.canvas.getActiveGroup();

        if (activeObject) {
            this.canvas.remove(activeObject);
            // this.textString = '';
        }
        // else if (activeGroup) {
        //   let objectsInGroup = activeGroup.getObjects();
        //   this.canvas.discardActiveGroup();
        //   let self = this;
        //   objectsInGroup.forEach(function (object) {
        //     self.canvas.remove(object);
        //   });
        // }
    }

    bringToFront() {
        let activeObject = this.canvas.getActiveObject(),
            activeGroup = this.canvas.getActiveGroup();

        if (activeObject) {
            activeObject.bringToFront();
            // activeObject.opacity = 1;
        }
        else if (activeGroup) {
            let objectsInGroup = activeGroup.getObjects();
            this.canvas.discardActiveGroup();
            objectsInGroup.forEach((object) => {
                object.bringToFront();
            });
        }
    }

    sendToBack() {
        let activeObject = this.canvas.getActiveObject(),
            activeGroup = this.canvas.getActiveGroup();

        if (activeObject) {
            activeObject.sendToBack();
            // activeObject.opacity = 1;
        }
        else if (activeGroup) {
            let objectsInGroup = activeGroup.getObjects();
            this.canvas.discardActiveGroup();
            objectsInGroup.forEach((object) => {
                object.sendToBack();
            });
        }
    }

    confirmClear() {
        if (confirm('Are you sure?')) {
            this.canvas.clear();
        }
    }

    rasterize() {
        if (!fabric.Canvas.supports('toDataURL')) {
            alert('This browser doesn\'t provide means to serialize canvas to an image');
        }
        else {
            console.log(this.canvas.toDataURL('png'))
            //window.open(this.canvas.toDataURL('png'));
            var image = new Image();
            image.src = this.canvas.toDataURL('png')
            var w = window.open("");
            w.document.write(image.outerHTML);
        }
    }

    rasterizeSVG() {
        console.log(this.canvas.toSVG())
        // window.open(
        //   'data:image/svg+xml;utf8,' +
        //   encodeURIComponent(this.canvas.toSVG()));
        // console.log(this.canvas.toSVG())
        // var image = new Image();
        // image.src = this.canvas.toSVG()
        var w = window.open("");
        w.document.write(this.canvas.toSVG());
    };


    saveCanvasToJSON() {
        let json = JSON.stringify(this.canvas);
        localStorage.setItem('Kanvas', json);
        console.log('json');
        console.log(json);

    }

    loadCanvasFromJSON() {
        let CANVAS = localStorage.getItem('Kanvas');
        console.log('CANVAS');
        console.log(CANVAS);

        // and load everything from the same json
        this.canvas.loadFromJSON(CANVAS, () => {
            console.log('CANVAS untar');
            console.log(CANVAS);

            // making sure to render canvas at the end
            this.canvas.renderAll();

            // and checking if object's "name" is preserved
            console.log('this.canvas.item(0).name');
            console.log(this.canvas);
        });

    };

    rasterizeJSON() {
        this.json = JSON.stringify(this.canvas, null, 2);
    }

    resetPanels() {
        this.textEditor = false;
        this.imageEditor = false;
        this.figureEditor = false;
    }

}
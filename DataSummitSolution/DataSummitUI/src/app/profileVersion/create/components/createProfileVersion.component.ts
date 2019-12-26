import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ApiService } from '../../../shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { RectanglePairs } from '../models/rectanglepairs';
import { RectanglePair } from '../models/rectanglepair';
import 'fabric';
import { NotifyService } from '../../../shared/services/notify.service';
import { ProfileVersion } from '../../../profileVersion/models/profileVersion';
import { ProfileAttribute } from '../../../profileAttributes/models/profileAttribute';
import { Rectangle } from '../models/rectangle';
import { format } from 'url';
import { Point } from '../models/point';
import { ArrayBuffer } from '@angular/http/src/static_request';
import { StandardAttribute } from 'src/app/standardAttribute/models/standardAttribute';

declare const fabric: any;

@Component({
  selector: 'ds-createProfileVersion',
  styleUrls: ['createProfileVersion.component.css'],
  templateUrl: 'createProfileVersion.component.html'
})

export class CreateProfileVersionComponent implements OnInit, AfterViewInit {

  @ViewChild('canvasContainer') canvasContainer
  @ViewChild('templateNameModal') templateNameModal

  @Input() companyId: number;
  template: ProfileVersion = new ProfileVersion();;
  pairs: RectanglePairs;
  templateName: string;
  loading: boolean;
  standardAttributes: StandardAttribute[] = [];
  headers: string[];
  canvas: any;
  
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

  textString: string;
  url: any = '';

  json: any;
  globalEditor: boolean = false;
  textEditor: boolean = false;
  imageEditor: boolean = false;
  figureEditor: boolean = false;
  selected: any;
  width: number;
  height: number;
  toolbarLeft: number | string = 0;
  toolbarRight: number | string = 'auto';
  toolbarAlign: string;
  titleFill: string = 'rgba(214,35,30,0.3)';
  valueFill: string = 'rgba(46,148,181,0.3)';

  saveGroups: RectanglePair[] = [];

  constructor(private router: Router,
    private api: ApiService,
    private route: ActivatedRoute,
    private notifyService: NotifyService) { }

  ngOnInit() {
    //CompanyId
    this.companyId = this.route.snapshot.params['companyId']
    if (typeof this.companyId == 'string')         //Ensure companyId is number if received as a string
    { this.companyId = Number(this.companyId);}
    this.getStandardAttributeNames();
    this.initProfileVersionTable();
  }

  initProfileVersionTable() {
    this.headers = [
        "Original",
        "Standard",
        "Actions"
    ]}

  saveTemplateName()
  {
    //Populate profile attributes
    let pas: ProfileAttribute[] = [];
    this.saveGroups.forEach(rp => {
      let pa: ProfileAttribute = new ProfileAttribute();
      let rN: Rectangle = new Rectangle();
      let rV: Rectangle = new Rectangle();
      rN = rp.TitleRectangle;
      rV = rp.ValueRectangle;
      pa.ProfileAttributeId = pas.length + 1;
      pa.NameHeight = Math.round(rN.Height);
      pa.NameWidth = Math.round(rN.Width);
      pa.NameX = Math.round(rN.Tl.x);
      pa.NameY = Math.round(rN.Tl.y);
      pa.ValueHeight = Math.round(rV.Height);
      pa.ValueWidth = Math.round(rV.Width);
      pa.ValueX = Math.round(rV.Tl.x);
      pa.ValueY = Math.round(rV.Tl.y);
      if (pa.ProfileVersionId == null) pa.ProfileVersionId = 0;
      if (pa.BlockPositionId == null || pa.BlockPositionId == 0) pa.BlockPositionId = 1;
      if (pa.StandardAttributeId == null|| pa.StandardAttributeId == 0) pa.StandardAttributeId = 1;
      pas.push(pa);
    });
    this.template.ProfileAttributes = pas;
    this.templateNameModal.show();
  }

  deleteAttribute(pa: ProfileAttribute)
  {
    this.template.ProfileAttributes.splice(pa.ProfileAttributeId, 1);
  }

  saveDefinition(templateName: string) {
    this.loading = true;
    this.templateNameModal.hide();

    this.template.Name = templateName;
    this.template.CompanyId = this.companyId;
    this.template.HeightOriginal = Math.round(this.height);
    this.template.WidthOriginal = Math.round(this.width);

    let xMax = 0;
    let yMax = 0;
    let xMin = 10000000000000;
    let yMin = 10000000000000;
    
    this.template.ProfileAttributes.forEach(attr => {
      attr.StandardAttributeId = parseInt(attr.StandardAttributeId.toString())
      if ((attr.NameX + attr.NameWidth) > xMax) xMax = attr.NameX + attr.NameWidth;
      if ((attr.NameY + attr.NameHeight) > yMax) yMax = attr.NameY + attr.NameHeight;
      if (attr.NameX < xMin) xMin = attr.NameX;
      if (attr.NameY < yMin) yMin = attr.NameY;
    });
    
    this.template.Width = xMax - xMin;
    this.template.Height = yMax - yMin;
    this.template.X = xMin;
    this.template.Y = yMin;

    if (this.template.Width == null) this.template.Width = 0;
    if (this.template.Height == null) this.template.Height = 0;
    if (this.template.X == null) this.template.X = 0;
    if (this.template.Y == null) this.template.Y = 0;

    this.template.ImageString = this.url;
    //TODO replace default userId with actual user
    this.template.UserId = 1;
    
    //let v: string = JSON.stringify(this.template);

    this.api.post('api/profileversions', this.template)
        .pipe(take(1))
        .subscribe(result => {
                this.router.navigate([`companies/${this.companyId}/profileversions`])
                console.log(location.origin.toString() + this.router.url.toString());
                this.loading = false;
            }, error => {
                console.log(error);
                this.loading = false;
            })
  }

  getStandardAttributeNames() {
    this.standardAttributes.push(new StandardAttribute(1, "Project Number"));
    this.standardAttributes.push(new StandardAttribute(2, "Project Name"));
    this.standardAttributes.push(new StandardAttribute(3, "Project Address"));
    this.standardAttributes.push(new StandardAttribute(4, "Drawing Title"));
    this.standardAttributes.push(new StandardAttribute(5, "Drawing Number"));
    this.standardAttributes.push(new StandardAttribute(6, "Drawing Revision"));
    this.standardAttributes.push(new StandardAttribute(7, "Drawing Date"));
    this.standardAttributes.push(new StandardAttribute(8, "Design Stage"));
    this.standardAttributes.push(new StandardAttribute(9, "Discipline"));
    this.standardAttributes.push(new StandardAttribute(10, "Size"));
    this.standardAttributes.push(new StandardAttribute(11, "Scale"));
    this.standardAttributes.push(new StandardAttribute(12, "Drafter"));
    this.standardAttributes.push(new StandardAttribute(13, "Checker"));
    this.standardAttributes.push(new StandardAttribute(14, "Approver"));
    this.standardAttributes.push(new StandardAttribute(15, "Zone"));
    this.standardAttributes.push(new StandardAttribute(16, "Security"));
    this.standardAttributes.push(new StandardAttribute(17, "Other"));
    this.standardAttributes.push(new StandardAttribute(101, "Drafting Company"));
    this.standardAttributes.push(new StandardAttribute(201, "Client"));
}

  ngAfterViewInit() {
    this.canvas = new fabric.Canvas('canvas', {
      hoverCursor: 'pointer',
      selection: true,
      selectionBorderColor: 'blue'
    });

    this.canvas.on({
      'object:moving': (e) => { },
      'object:modified': (e) => { },
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
      'selection:cleared': (e) => {
        this.selected = null;
        this.resetPanels();
      }
    });

    // this.canvas.setWidth(this.size.width);
    this.width = this.canvasContainer.nativeElement.clientWidth - 30
    this.height = this.width * (1 / 1.4142)
    this.canvas.setWidth(this.width);
    this.canvas.setHeight(this.height);

    // get references to the html canvas element & its context
    // this.canvas.on('mouse:down', (e) => {
    // let canvasElement: any = document.getElementById('canvas');
    // console.log(canvasElement)
    // });
  }


  imageString: string

  onUpload(event) {

  }

  linkRects() {
    if (!this.canvas.getActiveObject()) {
      return;
    }

    if (this.canvas.getActiveObject().type !== 'activeSelection') {
      return;
    }

    let selectedRects = this.canvas.getActiveObjects()
    if (selectedRects.length == 2) {
      let group: RectanglePair = new RectanglePair();
      selectedRects.forEach(rect => {
        if (rect.fill == this.titleFill)
        {
          let titleRect: Rectangle = new Rectangle();
          titleRect.Tl = rect.aCoords.tl;
          titleRect.Tr = rect.aCoords.tr;
          titleRect.Bl = rect.aCoords.bl;
          titleRect.Br = rect.aCoords.br;
          titleRect.Width = titleRect.Tr.x - titleRect.Tl.x;
          titleRect.Height = titleRect.Br.y - titleRect.Tr.y;
          titleRect.Fill = rect.fill;
          titleRect.Type = "title";
          
          group.TitleRectangle = titleRect;
        } 
        else if (rect.fill == this.valueFill) 
        {
          let valueRect: Rectangle = new Rectangle();
          valueRect.Tl = rect.aCoords.tl,
          valueRect.Tr = rect.aCoords.tr,
          valueRect.Bl = rect.aCoords.bl,
          valueRect.Br = rect.aCoords.br,
          valueRect.Width = valueRect.Tr.x - valueRect.Tl.x,
          valueRect.Height = valueRect.Br.y - valueRect.Tr.y,
          valueRect.Fill = rect.fill,
          valueRect.Type = "value"
          
          group.ValueRectangle = valueRect;
        }
      });
      if (group.TitleRectangle && group.ValueRectangle) {
        this.saveGroups.push(group)
      } else {
        this.notifyService.warn('A selected item was not either a title or value block.')
        return;
      }
    } else {
      this.notifyService.warn('Please Select a title and value rectangle only')
    }


    let obj = this.canvas.getActiveObject()

    //add new surrounding rect
    let add = new fabric.Rect({
      left: obj.left,
      top: obj.top,
      width: obj.width,
      height: obj.height,
      fill: 'rgba(92,184,92,0.1)',
      borderColor: '#1F2226',
      hasBorders: true
    });

    // this.canvas.add(add);
    // this.canvas.requestRenderAll();

    this.canvas.getActiveObject().toGroup();
    // g1.set({ fill: 'rgba(92,184,92,0.1)' })
    let g1 = this.canvas.getActiveObject();

    let group = new fabric.Group([g1, add], {
      left: obj.left,
      top: obj.top,
      width: obj.width,
      height: obj.height,
      originX: 'left',
      originY: 'top'
    })
    this.canvas.add(group);
    this.canvas.discardActiveObject();
    this.canvas.requestRenderAll();
    // }
  }

  unlinkRects() {
    if (!this.canvas.getActiveObject()) {
      return;
    }
    if (this.canvas.getActiveObject().type !== 'group') {
      return;
    }
    this.canvas.getActiveObject().toActiveSelection();
    this.canvas.requestRenderAll();
  }

  log(e) {
    console.log(e)
  }

  fileChange(event) {
    let fileList: FileList = event.files;
    if (fileList.length > 0) {
      let file: File = fileList[0];
      let formData: FormData = new FormData();
      formData.append('uploadFile', file, file.name);
    }
  }

  //Block "Size"

  // changeSize(event: any) {
  //   this.canvas.setWidth(this.size.width);
  //   this.canvas.setHeight(this.size.height);
  // }

  //Block "Add text"

  addText() {
    let textString = this.textString;
    let text = new fabric.IText(textString, {
      left: 10,
      top: 10,
      fontFamily: 'helvetica',
      angle: 0,
      fill: '#000000',
      scaleX: 0.5,
      scaleY: 0.5,
      fontWeight: '',
      hasRotatingPoint: true
    });
    this.extend(text, this.randomId());
    this.canvas.add(text);
    this.selectItemAfterAdded(text);
    this.textString = '';
  }

  //Block "Add images"

  getImgPolaroid(event: any) {
    let el = event.target;
    fabric.Image.fromURL(el.src, (image) => {
      image.set({
        left: 10,
        top: 10,
        angle: 0,
        padding: 10,
        cornersize: 10,
        hasRotatingPoint: true,
        peloas: 12
      });
      image.setWidth(150);
      image.setHeight(150);
      this.extend(image, this.randomId());
      this.canvas.add(image);
      this.selectItemAfterAdded(image);
    });
  }

  //Block "Upload Image"

  addImageOnCanvas(url) {
    if (url) {
      fabric.Image.fromURL(url, (image) => {
        image.set({
          left: 10,
          top: 10,
          angle: 0,
          padding: 10,
          cornersize: 10,
          hasRotatingPoint: true,
          width: this.canvasContainer.nativeElement.clientWidth,
          height: 800
        });
        // image.setWidth(200);
        // image.setHeight(200);
        this.extend(image, this.randomId());
        this.canvas.setBackgroundImage(image);
      });
    }
  }

  readUrl(event) {
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();
      reader.onload = (event) => {
        this.url = event.target['result'];
        let image = new Image()
        image.src = event.target['result'] as string;
        this.canvas.setWidth(image.width)
        this.canvas.setHeight(image.height)
        this.height = image.height
        this.width = image.width
        this.canvas.setBackgroundColor({ source: image.src, repeat: 'no-repeat' }, () => {
          // self.props.canvasFill = '';
          this.canvas.renderAll();
        });
        this.canvas.renderAll();
        // this.setCanvasImage(event.target['result']);
      }
      reader.readAsDataURL(event.target.files[0]);
      //this.ConvertImageToString(reader.result);
    }
  }

  // async ConvertImageToString(im: any)
  // {
  //   if (typeof im != "string")
  //   {
  //     ArrayBuffer ab = 
  //   }
  // }

  panScreen(direction: string) {
    switch (direction) {
      case 'left': window.scrollTo({ left: 0, behavior: "smooth" })
        break;
      case 'right': window.scrollTo({ left: 100000, behavior: "smooth" })
        break;
      case 'top': window.scrollTo({ top: 0, behavior: "smooth" })
        break;
      case 'bottom': window.scrollTo({ top: 100000, behavior: "smooth" })
        break;
      default: window.scrollTo({ top: 0, left: 0, behavior: "smooth" })
        break;
    }
  }

  toggleToolbarSide() {
    if (this.toolbarLeft == 0) {
      this.toolbarRight = 0
      this.toolbarLeft = 'auto'
      this.toolbarAlign = 'right'
    } else {
      this.toolbarRight = 'auto'
      this.toolbarLeft = 0
      this.toolbarAlign = 'left'
    }
  }

  removeWhite(url) {
    this.url = '';
    this.canvas.setBackgroundColor({ source: this.url, repeat: 'no-repeat' }, () => {
      // self.props.canvasFill = '';
      this.canvas.renderAll();
    });
  };

  //Block "Add figure"

  addFigure(figure) {
    let add: any;
    console.log(window)
    let x = window.pageXOffset + (window.innerWidth / 2)
    let y = window.pageYOffset + + (window.innerHeight / 2)
    switch (figure) {
      case 'rectangle':
        add = new fabric.Rect({
          width: 200, height: 100, left: 10, top: 10, angle: 0,
          fill: 'rgba(0,0,0,0.25)',
          borderColor: '#3f51b5',
          hasBorders: true
        });
        break;
      case 'title':
        add = new fabric.Rect({
          width: 200, height: 100, left: x, top: y, angle: 0,
          fill: this.titleFill,
          borderColor: '#1F2226',
          hasBorders: true
        });
        break;
      case 'value':
        add = new fabric.Rect({
          width: 200, height: 100, left: x, top: y, angle: 0,
          fill: this.valueFill,
          borderColor: '#1F2226',
          hasBorders: true
        });
        break;
      case 'square':
        add = new fabric.Rect({
          width: 100, height: 100, left: 10, top: 10, angle: 0,
          fill: '#4caf50'
        });
        break;
      case 'triangle':
        add = new fabric.Triangle({
          width: 100, height: 100, left: 10, top: 10, fill: '#2196f3'
        });
        break;
      case 'circle':
        add = new fabric.Circle({
          radius: 50, left: 10, top: 10, fill: '#ff5722'
        });
        break;
    }
    this.extend(add, this.randomId());
    this.canvas.add(add);
    this.selectItemAfterAdded(add);
  }

  /*Canvas*/

  cleanSelect() {
    // this.canvas.deactivateAllWithDispatch().renderAll();
  }

  selectItemAfterAdded(obj) {
    // this.canvas.deactivateAllWithDispatch().renderAll();
    this.canvas.setActiveObject(obj);
  }

  setCanvasFill() {
    if (!this.props.canvasImage) {
      this.canvas.backgroundColor = this.props.canvasFill;
      this.canvas.renderAll();
    }
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

  setCanvasImage(url) {
    let self = this;
    if (url) {
      let image = new Image()
      image.src = url
      this.canvas.setWidth(image.width)
      this.canvas.setHeight(image.height)
      this.canvas.setBackgroundColor({ source: image.src, repeat: 'no-repeat' }, () => {
        // self.props.canvasFill = '';
        this.canvas.renderAll();
      });
      this.width = image.width;
      this.height = image.height;
    }
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
    let activeObject = this.canvas.getActiveObject(),
      activeGroup = this.canvas.getActiveGroup();

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
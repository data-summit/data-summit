import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core';
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

import { StandardAttribute } from 'src/app/standardAttribute/models/standardAttribute';
import { BaseCanvasMethods } from '../models/baseCanvasMethods';

declare const fabric: any;

@Component({
  selector: 'ds-createProfileVersion',
  styleUrls: ['createProfileVersion.component.css'],
  templateUrl: 'createProfileVersion.component.html'
})

export class CreateProfileVersionComponent extends BaseCanvasMethods implements OnInit, AfterViewInit {

  @Input() companyId: number;
  
  @ViewChild('canvasContainer', { static: false }) canvasContainer
  @ViewChild('templateNameModal', { static: false }) templateNameModal


  template: ProfileVersion = new ProfileVersion();;
  pairs: RectanglePairs;
  templateName: string;
  loading: boolean;
  standardAttributes: StandardAttribute[] = [];
  headers: string[];

  imageString: string
  textString: string;
  url: any = '';

  globalEditor: boolean = false;

  toolbarLeft: number | string = 0;
  toolbarRight: number | string = 'auto';
  toolbarAlign: string;
  titleFill: string = 'rgba(214,35,30,0.3)';
  valueFill: string = 'rgba(46,148,181,0.3)';

  saveGroups: RectanglePair[] = [];

  constructor(private router: Router,
    private api: ApiService,
    private route: ActivatedRoute,
    private notifyService: NotifyService) {
    super()
  }

  ngOnInit() {
    //CompanyId
    this.companyId = this.route.snapshot.params['companyId']
    if (typeof this.companyId == 'string')         //Ensure companyId is number if received as a string
    { this.companyId = Number(this.companyId); }
    this.getStandardAttributeNames();
    this.initProfileVersionTable();
  }

  ngAfterViewInit() {
    super.initCanvas()
    this.width = this.canvasContainer.nativeElement.clientWidth - 30
    this.height = this.width * (1 / 1.4142)
    this.canvas.setWidth(this.width);
    this.canvas.setHeight(this.height);
  }

  initProfileVersionTable() {
    this.headers = [
      "Original",
      "Standard",
      "Actions"
    ]
  }

  saveTemplateName() {
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
      if (pa.StandardAttributeId == null || pa.StandardAttributeId == 0) pa.StandardAttributeId = 1;
      pas.push(pa);
    });
    this.template.ProfileAttributes = pas;
    this.templateNameModal.show();
  }

  deleteAttribute(pa: ProfileAttribute) {
    this.template.ProfileAttributes.splice(pa.ProfileAttributeId, 1);
  }


  //saves the template
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
      .subscribe(() => {
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

  onUpload() {

  }

  //Creates group of the selected rectangles
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
        if (rect.fill == this.titleFill) {
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
        else if (rect.fill == this.valueFill) {
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

  //ungroups a selected grouped rectangle pair
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

  //for testing purposes
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

  //Block "Upload Image"

  //Called on selecting an image, sets the canvas background to the selected image and resizes.
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
          this.canvas.renderAll();
        });
        this.canvas.renderAll();
      }
      reader.readAsDataURL(event.target.files[0]);
    }
  }

  //Allows user to move the screen to top/bottom/left/right extremes without the need for scrolling
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

  //swaps the toolbar orientations
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

  //removes image from background.
  removeImage() {
    this.url = '';
    this.canvas.setBackgroundColor({ source: this.url, repeat: 'no-repeat' }, () => {
      // self.props.canvasFill = '';
      this.canvas.renderAll();
    });
  };

  //Adds rectangle to the canvas (title / value rect.)
  addFigure(figure) {
    let add: any;
    console.log(window)
    let x = window.pageXOffset + (window.innerWidth / 2)
    let y = window.pageYOffset + + (window.innerHeight / 2)
    switch (figure) {
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
    }
    this.extend(add, this.randomId());
    this.canvas.add(add);
    this.selectItemAfterAdded(add);
  }

  setCanvasImage(url) {
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
}
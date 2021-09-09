import { Component, Inject, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
  selector: 'blobs',
  templateUrl: './blobs.component.html'
})

export class BlobsComponent extends BaseComponent {
  forceReload: boolean = false;

  @Output() refresh: EventEmitter<boolean> = new EventEmitter();

  @Input() container: string = "";
  @ViewChild('fileInput', { read: null, static: false }) fileInput: any;
  @ViewChild('modal', { read: null, static: false }) modal: any;
  @ViewChild('uploadToFolder', { read: null, static: false }) uploadToFolder: any;

  public showTable: boolean = false;

  public blobs: any[];
  public selected: string;
  public path: string = "";

  public removeContainerFlag: boolean = false;

  constructor(utils: UtilsService) {
    super(utils);
    this.getBlobs();
  }

  ngOnChanges() {
    this.path = "";
    this.getBlobs();
  }

  getBlobs() {

    if (!this.container) {
      this.showTable = false;
      return;
    }

    this.loading = true;
    this.showTable = false;
    this.utilsService.getData('api/Blobs/GetBlobs?container=' + this.container + '&path=' + this.path).subscribe(result => {

      let list = JSON.parse(result);
      this.blobs = new Array();
      for (var i = 0; i < list.length; i++) {
        var b = decodeURI(list[i]);
        if (b.endsWith('/')) {
          var y = b.lastIndexOf('/');
          var x = b.substring(0, y - 1).lastIndexOf('/');
          this.blobs.push({ name: b.substring(x + 1, y + 1), isFolder: true, url: b });
        }
        else {
          var y = b.lastIndexOf('/');
          this.blobs.push({ name: b.substring(y+1), url: b });
        }
      }

      this.loading = false;
      this.showTable = true;
    }, error => { this.setHttpError(error); });
  }

  removeBlob(event: Event) {

    var element = (event.currentTarget as Element); //button
    var blob: string = element.parentElement!.parentElement!.children[3]!.textContent!;

    this.selected = blob;
  }

  deleteBlob() {
    this.utilsService.postData('api/Blobs/DeleteBlob?container=' + this.container + '&blobUri=' + encodeURIComponent(this.selected), null).subscribe(result => {
      this.selected = '';
      this.getBlobs();
    }, error => { this.setError(error); });
  }

  cancelDeleteBlob() {
    this.selected = '';
  }

  removeContainer(event: Event) {
    this.removeContainerFlag = true;
  }

  cancelDeleteContainer() {
    this.removeContainerFlag = false;
  }

  deleteContainer() {
    this.utilsService.postData('api/Containers/DeleteContainer?container=' + this.container, null).subscribe(result => {
      this.container = "";
      this.removeContainerFlag = false;
      this.refresh.emit(true);
    }, error => { this.setError(error); });

  }

  upload() {
    var that = this;
    const fileBrowser = this.fileInput.nativeElement;
    if (fileBrowser.files && fileBrowser.files[0]) {
      const formData = new FormData();
      formData.append('files', fileBrowser.files[0]);

      var fullPath = this.path
      var folder = encodeURIComponent(this.uploadToFolder.nativeElement.value)
      if (folder) {
        fullPath += folder + "/"
      }
      this.utilsService.uploadFile('api/Blobs/UploadBlob?container=' + this.container + '&path=' + fullPath, formData).onload = function () {
        that.getBlobs();
      };
    }
  }

  downloadBlob(event: Event) {
    var element = (event.currentTarget as Element); //button
    var blob: string = element.parentElement!.parentElement!.children[3]!.textContent!;

    this.utilsService.getFile('api/Blobs/GetBlob?container=' + this.container + '&blobUri=' + blob).subscribe(result => {

      var fileName: string = "NONAME";
      var contentDisposition: string = result.headers!.get("content-disposition")!;
      contentDisposition.split(";").forEach(token => {
        token = token.trim();
        if (token.startsWith("filename=")) {
          fileName = token.substr("filename=".length);
          fileName = fileName.replace(/"/g, '');
        }
      });

      var blobUrl = URL.createObjectURL(result.body);
      var link = document.createElement('a');
      link.href = blobUrl;
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);

    }, error => { this.setError(error); });

  }

  enterFolder(event: Event) {
    var element = (event.currentTarget as Element); //button
    var vDir: string = element.parentElement!.parentElement!.children[2]!.textContent!;
    this.path += vDir
    this.getBlobs();
  }

  moveUp(event: Event) {
    var y = this.path.lastIndexOf('/');
    var x = this.path.substring(0, y - 1).lastIndexOf('/');
    if (x == -1)
      this.path = "";
    else
      this.path = this.path.substring(0, x + 1);

    this.getBlobs();
  }
}

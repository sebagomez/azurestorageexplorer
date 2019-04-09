import { Component, Inject, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'blobs',
	templateUrl: './blobs.component.html'
})

export class BlobsComponent extends BaseComponent {
	forceReload: boolean = false;

	@Output() refresh: EventEmitter<boolean> = new EventEmitter();

	@Input() container: string = "";
	@ViewChild('fileInput') fileInput: any;
	@ViewChild('modal') modal: any;

	public showTable: boolean = false;

	public blobs: string[];
	public selected: string;

	public removeContainerFlag: boolean = false;

	constructor(utils: UtilsService) {
		super(utils);
		this.getBlobs();
	}

	ngOnChanges() {
		this.getBlobs();
	}

	getBlobs() {

		if (!this.container) {
			this.showTable = false;
			return;
		}

		this.loading = true;
		this.showTable = false;
		this.utilsService.getData('api/Blobs/GetBlobs?container=' + this.container).subscribe(result => {
			this.blobs = result.json();
			this.loading = false;
			this.showTable = true;
		}, error => { this.setErrorMessage(error.statusText); });
	}

	removeBlob(event: Event) {

		var element = (event.currentTarget as Element); //button
		var blob: string = element.parentElement!.parentElement!.children[2]!.textContent!;

		this.selected = blob;
	}

	deleteBlob() {
		this.utilsService.postData('api/Blobs/DeleteBlob?blobUri=' + encodeURIComponent(this.selected), null).subscribe(result => {
			this.selected = '';
			this.getBlobs();
		}, error => { this.setErrorMessage(error.statusText); });
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
		}, error => { this.setErrorMessage(error.statusText); });

	}

	upload() {
		var that = this;
		const fileBrowser = this.fileInput.nativeElement;
		if (fileBrowser.files && fileBrowser.files[0]) {
			const formData = new FormData();
			formData.append('files', fileBrowser.files[0]);

			this.utilsService.uploadFile('api/Blobs/UploadBlob?container=' + this.container, formData).onload = function () {
				that.getBlobs();
			};
		}
	}

	downloadBlob(event: Event) {
		var element = (event.currentTarget as Element); //button
		var blob: string = element.parentElement!.parentElement!.children[2]!.textContent!;

		this.utilsService.getFile('api/Blobs/GetBlob?blobUri=' + blob).subscribe(result => {

			var fileName: string = "NONAME";
			var contentDisposition: string = result.headers!.get("content-disposition")!;
			contentDisposition.split(";").forEach(token => {
				token = token.trim();
				if (token.startsWith("filename="))
					fileName = token.substr("filename=".length);
			});

			var byteArray = result.arrayBuffer();

			var blobFile = new Blob([byteArray], { type: "application/octet-stream;charset=utf-8", endings: "transparent" });
			var blobUrl = URL.createObjectURL(blobFile);

			var link = document.createElement('a');
			link.href = blobUrl;
			link.setAttribute('download', fileName);
			document.body.appendChild(link);
			link.click();
			document.body.removeChild(link);

		}, error => { this.setErrorMessage(error.statusText); });

	}
}

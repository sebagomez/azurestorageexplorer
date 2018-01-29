import { Component, Inject, Input, ViewChild  } from '@angular/core';
//import { Response, RequestOptions, ResponseContentType } from '@angular/http';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
	selector: 'blobs',
	templateUrl: './blobs.component.html'
})

export class BlobsComponent {
	forceReload: boolean;
	utilsService: UtilsService;

	@Input() container: string = "";
	@ViewChild('fileInput') fileInput: any;

	public loading: boolean = false;

	public blobs: string[];

	constructor(utils: UtilsService) {

		this.utilsService = utils;

		this.getBlobs();
	}

	ngOnChanges() {
		this.getBlobs();
	}

	getBlobs() {

		if (!this.container)
			return;
		this.loading = true;
		this.utilsService.getData('api/Blobs/GetBlobs?container=' + this.container).subscribe(result => {
			this.loading = false;
			this.blobs = result.json();
		}, error => console.error(error));
	}

	removeBlob(event: Event) {
		var element = (event.currentTarget as Element); //button
		var blob : string = element.parentElement!.parentElement!.children[2]!.textContent!;

		this.utilsService.postData('api/Blobs/DeleteBlob?blobUri=' + encodeURIComponent(blob), null).subscribe(result => {
			this.getBlobs();
		}, error => console.error(error));
	}

	removeContainer(event: Event) {
		this.utilsService.postData('api/Containers/DeleteContainer?container=' + this.container, null).subscribe(result => {
			this.container = "";
		}, error => console.error(error));
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

			//const xhr = new XMLHttpRequest();
			//xhr.open('POST', this.baseUrl + 'api/Blobs/UploadBlob?container=' + this.container, true);
			//xhr.onload = function () {
			//	that.getBlobs();
			//};
			//xhr.send(formData);
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

		}, error => console.error(error));
	}
}

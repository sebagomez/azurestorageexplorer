import { Component, Inject, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'files',
	templateUrl: './files.component.html'
})

export class FilesComponent extends BaseComponent {
	forceReload: boolean;

	@Output() refresh: EventEmitter<boolean> = new EventEmitter();

	@Input() share: string = "";
	@ViewChild('fileInput') fileInput: any;
	@ViewChild('modal') modal: any;

	public showTable: boolean = false;

	public files: string[];
	public selected: string;

	public removeContainerFlag: boolean = false;

	constructor(utils: UtilsService) {
		super(utils);
		this.getFiles();
	}

	ngOnChanges() {
		this.getFiles();
	}

	getFiles() {

		if (!this.share) {
			this.showTable = false;
			return;
		}

		this.loading = true;
		this.showTable = false;
		this.utilsService.getData('api/Files/GetFilesAndDirectories?share=' + this.share).subscribe(result => {
			this.files = result.json();
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
		this.utilsService.postData('api/Files/DeleteFile?blobUri=' + encodeURIComponent(this.selected), null).subscribe(result => {
			this.selected = '';
			this.getFiles();
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
		this.utilsService.postData('api/File/DeleteShare?share=' + this.share, null).subscribe(result => {
			this.share = "";
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

			this.utilsService.uploadFile('api/Files/UploadFile?share=' + this.share, formData).onload = function () {
				that.getFiles();
			};
		}
	}
}

import { Component, Inject, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'files',
	templateUrl: './files.component.html'
})

export class FilesComponent extends BaseComponent {
	//forceReload: boolean;

	@Output() refresh: EventEmitter<boolean> = new EventEmitter();

	@Input() share: string = "";
	@ViewChild('fileInput', { read: null, static: false }) fileInput: any;
	@ViewChild('newFolder', { read: null, static: false }) newFolder: any;

	public showTable: boolean = false;
	public folder: string = "";
	public files: any;
	public selected: string;

	public removeContainerFlag: boolean = false;

	constructor(utils: UtilsService) {
		super(utils);
		this.getFiles();
	}

	ngOnChanges() {
		this.getFiles();
	}

	setFolder(event: Event) {

		var element = (event.currentTarget as Element);
		var folder = (element.parentElement!.children[1]!.textContent as string).trim();

		this.internalSetFolder(folder);
	}

	setCurrentFolder(event: Event) {
		var element = (event.currentTarget as Element);
		var folder = (element.textContent as string).trim();

		this.internalSetFolder(folder);
	}

	internalSetFolder(folder: string) {

		if (this.folder)
			this.folder = this.folder + "\\" + folder;
		else
			this.folder = folder;

		this.getFiles();
	}

	levelUp() {

		var slash = this.folder.lastIndexOf("\\");
		if (slash > 0)
			this.folder = this.folder.substr(0, slash);
		else
			this.folder = '';

		this.getFiles();
	}

	getFiles() {

		if (!this.share) {
			this.showTable = false;
			return;
		}

		this.loading = true;
		this.showTable = false;
		this.utilsService.getData('api/Files/GetFilesAndDirectories?share=' + this.share + '&folder=' + this.folder).subscribe(result => {
			this.files = JSON.parse(result);
			this.loading = false;
			this.showTable = true;
		}, error => { this.setError(error); });
	}

	downloadFile(event: Event) {
		var element = (event.currentTarget as Element);
		var file: string = element.children[0]!.textContent!;

		this.utilsService.getFile('api/Files/GetFile?share=' + this.share + '&folder=' + this.folder + '&file=' + file).subscribe(result => {

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

		}, error => {
			this.setError(error);
		});
	}

	removeFile(event: Event) {
		var element = (event.currentTarget as Element); //button
		var file: string = element.parentElement!.parentElement!.children[3]!.textContent!;

		this.selected = file;
	}

	cancelDeleteFile() {
		this.selected = '';
	}

	deleteFile(event: Event) {
		var element = (event.currentTarget as Element);
		var file = (element.children[0].textContent as string).trim();

		this.utilsService.postData('api/Files/DeleteFile?share=' + this.share + '&folder=' + this.folder + '&file=' + file, null).subscribe(result => {
			this.selected = "";
			this.getFiles();
		}, error => { this.setError(error); });
	}

	createSubFolder() {

		if (!this.newFolder.nativeElement.value)
			return;

		this.utilsService.postData('api/Files/CreateSubDir?share=' + this.share + '&folder=' + this.folder + '&subDir=' + this.newFolder.nativeElement.value, null).subscribe(result => {
			this.selected = "";
			this.getFiles();
		}, error => { this.setError(error); });
	}

	upload() {
		var that = this;
		const fileBrowser = this.fileInput.nativeElement;
		if (fileBrowser.files && fileBrowser.files[0]) {
			const formData = new FormData();
			formData.append('files', fileBrowser.files[0]);

			var fileName = encodeURIComponent(fileBrowser.files[0].name)
			this.utilsService.uploadFile('api/Files/UploadFile?share=' + this.share+ '&folder=' + this.folder + '&fileName=' +  fileName, formData).onload = function () {
				that.getFiles();
			};
		}
	}
}

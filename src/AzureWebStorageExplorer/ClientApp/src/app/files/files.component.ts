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

	public showTable: boolean = false;
	public folder: string = "";
	public files: any;
	public selected: any;

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
}

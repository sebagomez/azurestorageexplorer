import { Component, Inject, ViewChild, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'containers',
	templateUrl: './containers.component.html'
})

export class ContainersComponent extends BaseComponent {
	public containers: string[] | undefined;
	public selectedContainer: string | undefined;

  @ViewChild('newContainerName', { read: null, static: false }) newContainerName: any;
  @ViewChild('publicAccess', { read: null, static: false }) publicAccess: any;
  @ViewChild('containersMenu', { read: null, static: false }) containersMenu: any;

	constructor(utilsService: UtilsService) {
		super(utilsService);
		this.getContainers();
	}

	ngOnChanges() {
		this.getContainers();
	}

	getContainers() {
		this.loading = true;
		this.utilsService.getData('api/Containers/GetContainers').subscribe(result => {
			this.loading = false;
      this.containers = JSON.parse(result); 
		}, error => { this.setError(error); });
	}

	containerChanged(event: Event) {
		var element = (event.currentTarget as Element);
		var container = (element.textContent as string).trim();

		var nodes = this.containersMenu.nativeElement.childNodes;
		for (var i = 0; i < nodes.length; i++) {
			if (nodes[i].classList)
				nodes[i].classList.remove("active");
		}

		element.classList.add("active");

		this.selectedContainer = container;
	}

	newContainer(event: Event) {
		let url = 'api/Containers/NewContainer?container=' + this.newContainerName.nativeElement.value + '&publicAccess=' + this.publicAccess.nativeElement.checked
		this.utilsService.postData(url, null).subscribe(result => {
			this.newContainerName.nativeElement.value = "";
			this.publicAccess.nativeElement.checked = false;
			this.getContainers();
		}, error => {
				this.setError(error);
		});

	}

	forceRefresh(force: boolean) {
		if (force) {
			this.getContainers();
			this.selectedContainer = '';
		}
	}
}

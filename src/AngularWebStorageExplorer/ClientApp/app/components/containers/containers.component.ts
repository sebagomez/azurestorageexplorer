import { Component, Inject, ViewChild } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
	selector: 'containers',
	templateUrl: './containers.component.html'
})

export class ContainersComponent {
	public containers: string[];
	public selectedContainer: string;

	utilsService: UtilsService;

	@ViewChild('newContainerName') newContainerName: any;
	@ViewChild('publicAccess') publicAccess: any;
	@ViewChild('containersMenu') containersMenu: any;

	constructor(utils: UtilsService) {

		this.utilsService = utils;

		this.getContainers();
	}

	ngOnChanges() {
		this.getContainers();
	}

	getContainers() {
		this.utilsService.getData('api/Containers/GetContainers').subscribe(result => {
			this.containers = result.json();
		}, error => console.error(error));
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
			debugger;
			this.publicAccess.nativeElement.checked = false;
			this.getContainers();
		}, error => console.error(error));
	}

	forceRefresh(force: boolean) {
		if (force) {
			this.getContainers();
			this.selectedContainer = '';
		}
	}
}

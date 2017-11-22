import { Component, Inject, ViewChild } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	selector: 'containers',
	templateUrl: './containers.component.html',
	styleUrls: ['./containers.component.css']
})
export class ContainersComponent {
	public containers: string[];
	public selectedContainer: string;

	http: Http;
	baseUrl: string;

	@ViewChild('newContainerName') newContainerName: any;
	@ViewChild('publicAccess') publicAccess: any;
	@ViewChild('containersMenu') containersMenu: any;

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

		this.http = http;
		this.baseUrl = baseUrl;

		this.getContainers();
	}

	ngOnChanges() {
		this.getContainers();
	}

	getContainers() {
		this.http.get(this.baseUrl + 'api/Containers/GetContainers').subscribe(result => {
			this.containers = result.json();
		}, error => console.error(error));
	}

	containerChanged(event: Event) {
		var element = (event.currentTarget as Element);
		var container = (element.textContent as string).trim();

		var nodes = this.containersMenu.nativeElement.childNodes;
		debugger;
		for (var i = 0; i < nodes.length; i++) {
			if (nodes[i].classList)
				nodes[i].classList.remove("active");
		}

		element.classList.add("active");

		this.selectedContainer = container;
	}

	newContainer(event: Event) {
		debugger
		this.http.post(this.baseUrl + 'api/Containers/NewContainer?container=' + this.newContainerName.nativeElement.value + '&publicAccess=' + (this.publicAccess.nativeElement.value === "on"), null).subscribe(result => {
			this.newContainerName.nativeElement.value = "";
			this.publicAccess.nativeElement.value = "off";
			this.getContainers();
		}, error => console.error(error));
	}
}

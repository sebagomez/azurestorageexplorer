import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	selector: 'containers',
	templateUrl: './containers.component.html',
	styleUrls: ['./containers.component.css']
})
export class ContainersComponent {
	public containers: string[];
	public selectedContainer: string;

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
		http.get(baseUrl + 'api/Containers/GetContainers').subscribe(result => {
			this.containers = result.json();
		}, error => console.error(error));
	}

	containerChanged(event: Event) {
		var element = (event.currentTarget as Element);
		var container = (element.textContent as string).trim();

		this.selectedContainer = container;

		console.log("you clicked on " + container);
	
	}
}

import { Component, Inject, Input, ViewChild } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	selector: 'tabledata',
	templateUrl: './tabledata.component.html'
})

export class TabledataComponent {
	public data: string[];

	public selectedTable: string;

	@Input() table: string = "";

	http: Http;
	baseUrl: string;

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

		this.http = http;
		this.baseUrl = baseUrl;

		this.getData();
	}

	ngOnChanges() {
		this.getData();
	}

	getData() {
		this.http.get(this.baseUrl + 'api/Queues/GetMessages?queue=' + this.table).subscribe(result => {
			this.data = result.json();
		}, error => console.error(error));
	}
}
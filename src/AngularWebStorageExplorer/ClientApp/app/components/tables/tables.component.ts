import { Component, Inject, ViewChild } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	selector: 'tables',
	templateUrl: './tables.component.html'
})

export class TablesComponent {
	public tables: string[];

	public selectedTable: string;

	http: Http;
	baseUrl: string;

	@ViewChild('newTableName') newTableName: any;
	@ViewChild('tablesMenu') tablesMenu: any;

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

		this.http = http;
		this.baseUrl = baseUrl;

		this.getTables();
	}

	ngOnChanges() {
		this.getTables();
	}

	getTables() {
		this.http.get(this.baseUrl + 'api/Tables/GetTables').subscribe(result => {
			this.tables = result.json();
		}, error => console.error(error));
	}

	queueChanged(event: Event) {
		var element = (event.currentTarget as Element);
		var table = (element.textContent as string).trim();

		var nodes = this.tablesMenu.nativeElement.childNodes;
		debugger;
		for (var i = 0; i < nodes.length; i++) {
			if (nodes[i].classList)
				nodes[i].classList.remove("active");
		}

		element.classList.add("active");

		this.selectedTable = table;
	}

	newTable(event: Event) {
		this.http.post(this.baseUrl + 'api/Tables/NewTable?table=' + this.newTableName.nativeElement.value, null).subscribe(result => {
			this.newTableName.nativeElement.value = "";
			this.getTables();
		}, error => console.error(error));
	}
}
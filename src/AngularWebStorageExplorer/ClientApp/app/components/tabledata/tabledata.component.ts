import { Component, Inject, Input, ViewChild } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	selector: 'tabledata',
	templateUrl: './tabledata.component.html'
})

export class TabledataComponent {
	forceReload: boolean;
	http: Http;
	baseUrl: string;

	@Input() storageTable: string = "";
	@ViewChild('inputQuery') inputQuery: any;

	public data: any;
	public loading: boolean = false;

	public headers: string[] = [];
	public rows: object[] = []; 

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

		this.http = http;
		this.baseUrl = baseUrl;

		this.getData();
	}

	ngOnChanges() {
		this.getData();
	}

	getData() {
		if (!this.storageTable)
			return;

		this.data = null;
		this.loading = true;
		this.http.get(this.baseUrl + 'api/Tables/QueryTable?table=' + this.storageTable + '&query=').subscribe(result => {
			this.data = result.json();
			this.processData();
		}, error => console.error(error));
	}

	processData() {
		this.headers.push("Partition Key");
		this.headers.push("Row Key");
		for (let entry of this.data) {
			let values: string[] = entry.values.split(";");
			var obj: any = {};
			obj["Partition Key"] = entry.partitionKey;
			obj["Row Key"] = entry.rowKey;
			for (let v of values) {
				if (!v) continue;
				let pair: string[] = v.split("=");
				if (!this.headers.find(h => h === pair[0]))
					this.headers.push(pair[0]);
				obj[pair[0]] = pair[1];
			}
			this.rows.push(obj);
		}
		this.loading = false;
	}
}
import { Component, Inject, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'tabledata',
	templateUrl: './tabledata.component.html'
})

export class TabledataComponent extends BaseComponent {
	forceReload: boolean;

	@Input() storageTable: string = "";
  @ViewChild('inputQuery', { read: null, static: false }) inputQuery: any;
  @ViewChild('mode', { read: null, static: false }) mode: any;
	@Output() refresh: EventEmitter<boolean> = new EventEmitter();

	public data: any;
	public showTable: boolean = false;
	public removeTableFlag: boolean = false;

	public headers: string[] = [];
	public rows: object[] = [];

	constructor(utils: UtilsService) {
		super(utils);
	}

	ngOnChanges() {
		this.data = null;
		this.showTable = false;
	}

	getData() {
		if (!this.storageTable)
			return;

		this.showTable = false;
		this.data = null;
		this.loading = true;
		this.utilsService.getData('api/Tables/QueryTable?table=' + this.storageTable + '&query=' + this.inputQuery.nativeElement.value).subscribe(result => {
			this.data = JSON.parse(result);
			this.processData();
		}, error => { this.setError(error); });
	}

	insertData() {
		if (!this.storageTable)
			return;

		this.showTable = false;
		this.loading = true;
		this.utilsService.putData('api/Tables/InsertData?table=' + this.storageTable + '&data=' + this.inputQuery.nativeElement.value, null).subscribe(result => {
			this.inputQuery.nativeElement.value = '';
			this.loading = false;
		}, error => { this.setError(error); });
	}

	processData() {
		//https://stackoverflow.com/questions/1232040/how-do-i-empty-an-array-in-javascript
		this.headers.length = 0;
		this.rows.length = 0;

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
		this.showTable = true;
	}

	queryData() {

		if (!this.storageTable) {
			var m = this.mode.nativeElement.value === 'q'?  "query" : "insert";
			this.setErrorMessage('You must first select a table to ' + m  + ' to');
			return;
		}

		if (this.mode.nativeElement.value === 'q')
			this.getData();
		else
			this.insertData();
	}

	modeChanged() {
		if (this.mode.nativeElement.value === 'q')
			this.inputQuery.nativeElement.placeholder = "Search pattern...";
		else
			this.inputQuery.nativeElement.placeholder = "Insert statement...";
	}

	removeRow(event: Event) {
		var element = (event.currentTarget as Element); //button
		var partitionKey: string = element.parentElement!.parentElement!.children[1]!.textContent!.trim();
		var rowKey: string = element.parentElement!.parentElement!.children[2]!.textContent!.trim();

		this.utilsService.deleteData('api/Tables/DeleteData?table=' + this.storageTable + '&partitionKey=' + partitionKey + '&rowKey=' + rowKey).subscribe(result => {
			this.getData();
		}, error => { this.setError(error); }); 
	}

	removeTable() {
		this.removeTableFlag = true;
	}

	cancelDeleteTable() {
		this.removeTableFlag = false;
	}

	deleteTable() {
		this.utilsService.deleteData('api/Tables/DeleteTable?table=' + this.storageTable).subscribe(result => {
			this.refresh.emit(true);
			this.removeTableFlag = false;
		}, error => { this.setError(error); });
	}
}

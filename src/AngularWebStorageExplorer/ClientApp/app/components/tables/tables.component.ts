import { Component, Inject, ViewChild } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
	selector: 'tables',
	templateUrl: './tables.component.html'
})

export class TablesComponent {
	public tables: string[];

	public selectedTable: string;
	utilsService: UtilsService;

	@ViewChild('newTableName') newTableName: any;
	@ViewChild('tablesMenu') tablesMenu: any;

	constructor(utils: UtilsService) {

		this.utilsService = utils;

		this.getTables();
	}

	ngOnChanges() {
		this.getTables();
	}

	getTables() {
		this.utilsService.getData('api/Tables/GetTables').subscribe(result => {
			this.tables = result.json();
		}, error => console.error(error));
	}

	tableChanged(event: Event) {
		var element = (event.currentTarget as Element);
		var table = (element.textContent as string).trim();

		var nodes = this.tablesMenu.nativeElement.childNodes;
		for (var i = 0; i < nodes.length; i++) {
			if (nodes[i].classList)
				nodes[i].classList.remove("active");
		}

		element.classList.add("active");

		this.selectedTable = table;
	}

	newTable(event: Event) {
		this.utilsService.postData('api/Tables/NewTable?table=' + this.newTableName.nativeElement.value, null).subscribe(result => {
			this.newTableName.nativeElement.value = "";
			this.getTables();
		}, error => console.error(error));
	}
}
import { Component, Inject, ViewChild } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'tables',
	templateUrl: './tables.component.html'
})

export class TablesComponent extends BaseComponent {
	public tables: string[];

	public selectedTable: string;

	@ViewChild('newTableName') newTableName: any;
	@ViewChild('tablesMenu') tablesMenu: any;

	constructor(utils: UtilsService) {
		super(utils);
		this.getTables();
	}

	ngOnChanges() {
		this.getTables();
	}

	getTables() {
		this.utilsService.getData('api/Tables/GetTables').subscribe(result => {
			this.tables = result.json();
		}, error => { this.setErrorMessage(error.statusText); });
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
		}, error => { this.setErrorMessage(error.statusText); });
	}
}
import { Component } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {

	public currentVersion: string;

	constructor(private utilsService: UtilsService) {
		this.getversion();
	}

	ngOnChanges() {
		this.getversion();
	}

	getversion() {
		this.utilsService.getData('api/Util/GetVersion').subscribe(result => {
			this.currentVersion = result.text();
		}, error => { console.error(error) });
	}
}

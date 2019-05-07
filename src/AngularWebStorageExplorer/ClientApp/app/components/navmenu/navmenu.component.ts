import { Component, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
	selector: 'nav-menu',
	templateUrl: './navmenu.component.html',
	styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {

	public account: string | null;

	constructor(private utilsService: UtilsService) {
		this.getAccount();
	}

	ngOnChanges() {
		this.getAccount();
	}

	getAccount() {
		this.account = this.utilsService.getAccount();
	}

	logOut(event: Event) {

		this.utilsService.clearCredentials();
		location.reload(true);
	}
}

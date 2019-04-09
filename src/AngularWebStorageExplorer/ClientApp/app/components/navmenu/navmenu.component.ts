import { Component, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {

	public currentVersion: string | undefined;

	//https://yakovfain.com/2016/10/31/angular-2-component-communication-with-events-vs-callbacks/
	@Output() signedIn: EventEmitter<boolean> = new EventEmitter();

	constructor(private utilsService: UtilsService) { }


	logOut(event: Event) {

		this.utilsService.logOut();
		this.signedIn.emit(false);
	}
}

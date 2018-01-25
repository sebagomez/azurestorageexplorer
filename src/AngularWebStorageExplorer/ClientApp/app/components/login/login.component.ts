import { Component, Inject, ViewChild, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
	selector: 'login',
	templateUrl: './login.component.html'
})

export class LoginComponent {

	@ViewChild('azureAccount') azureAccount: any;
	@ViewChild('azureKey') azureKey: any;

	utilsService: UtilsService;

	//https://yakovfain.com/2016/10/31/angular-2-component-communication-with-events-vs-callbacks/
	@Output() signedIn: EventEmitter<boolean> = new EventEmitter();

	constructor(utils: UtilsService) {

		this.utilsService = utils;
	}

	signIn(event: Event) {
		let account = encodeURIComponent(this.azureAccount.nativeElement.value);
		let key = encodeURIComponent(this.azureKey.nativeElement.value);

		this.utilsService.signIn(account, key).subscribe(result => {
			localStorage.setItem('account', account);
			localStorage.setItem('key', key);

			this.signedIn.emit(true);

		}, error => {

			localStorage.clear();

			this.signedIn.emit(false);

			console.error(error)
		});
	}
}

import { Component, Inject, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	selector: 'login',
	templateUrl: './login.component.html'
})

export class LoginComponent {

	@ViewChild('azureAccount') azureAccount: any;
	@ViewChild('azureKey') azureKey: any;

	http: Http;
	baseUrl: string;

	//https://yakovfain.com/2016/10/31/angular-2-component-communication-with-events-vs-callbacks/
	@Output() signedIn: EventEmitter<boolean> = new EventEmitter();

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

		this.http = http;
		this.baseUrl = baseUrl;

	}

	signIn(event: Event) {
		let account = encodeURIComponent(this.azureAccount.nativeElement.value);
		let key = encodeURIComponent(this.azureKey.nativeElement.value);

		this.http.get(this.baseUrl + 'api/Queues/GetQueues?account=' + account + '&key=' + key).subscribe(result => {
			debugger;
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

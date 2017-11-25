import { Component, Inject, Input, ViewChild } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	selector: 'qmessages',
	templateUrl: './qmessages.component.html'
})

export class QmessagesComponent {
	public messages: string[];

	public selectedQueue: string;

	@Input() queue: string = "";

	http: Http;
	baseUrl: string;

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

		this.http = http;
		this.baseUrl = baseUrl;

		this.getMessages();
	}

	ngOnChanges() {
		this.getMessages();
	}

	getMessages() {
		this.http.get(this.baseUrl + 'api/Queues/GetMessages?queue=' + this.queue).subscribe(result => {
			this.messages = result.json();
		}, error => console.error(error));
	}
}
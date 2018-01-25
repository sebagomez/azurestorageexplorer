import { Component, Inject, Input, ViewChild } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
	selector: 'qmessages',
	templateUrl: './qmessages.component.html'
})

export class QmessagesComponent {
	public messages: string[];

	public selectedQueue: string;

	@Input() queue: string = "";

	utilsService: UtilsService;

	constructor(utils: UtilsService) {

		this.utilsService = utils;

		this.getMessages();
	}

	ngOnChanges() {
		this.getMessages();
	}

	getMessages() {
		this.utilsService.getData('api/Queues/GetMessages?queue=' + this.queue).subscribe(result => {
			this.messages = result.json();
		}, error => console.error(error));
	}
}
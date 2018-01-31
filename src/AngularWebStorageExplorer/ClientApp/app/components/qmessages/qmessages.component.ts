import { Component, Inject, Input, ViewChild } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
	selector: 'qmessages',
	templateUrl: './qmessages.component.html'
})

export class QmessagesComponent {
	public messages: string[];

	public selectedQueue: string;
	public loading: boolean = false;
	public showTable: boolean = false;

	@Input() queue: string = "";
	@ViewChild('newMessage') newMessage: any;

	utilsService: UtilsService;

	constructor(utils: UtilsService) {

		this.utilsService = utils;

		this.getMessages();
	}

	ngOnChanges() {
		this.getMessages();
	}

	getMessages() {

		if (!this.queue)
			return;

		this.showTable = false;
		this.messages = [];
		this.loading = true;
		this.utilsService.getData('api/Queues/GetMessages?queue=' + this.queue).subscribe(result => {
			this.loading = false;
			this.messages = result.json();
			this.showTable = true;
		}, error => console.error(error));
	}

	addMessage() {
		this.utilsService.postData('api/Queues/NewQueueMessage?queue=' + this.queue + '&message=' + this.newMessage.nativeElement.value, null).subscribe(result => {
			this.getMessages();
			this.newMessage.nativeElement.value = '';
		}, error => console.error(error));
	}

	typingMessage(event: KeyboardEvent) {
		if (event.key == 'Enter')
			this.addMessage();
	}
}

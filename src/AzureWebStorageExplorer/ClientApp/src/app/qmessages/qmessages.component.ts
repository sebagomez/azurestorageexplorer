import { Component, Inject, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'qmessages',
	templateUrl: './qmessages.component.html'
})

export class QmessagesComponent extends BaseComponent {
	public messages: any;

	public selectedQueue: string;
	public showTable: boolean = false;
	public removeQueueFlag: boolean = false;
	public selected: string;

	@Output() refresh: EventEmitter<boolean> = new EventEmitter();
	@Input() queue: string = "";
  @ViewChild('newMessage', { read: null, static: false }) newMessage: any;

	constructor(utils: UtilsService) {
		super(utils);
		this.getMessages();
	}

	ngOnChanges() {
		this.getMessages();
	}

	getMessages() {

		if (!this.queue) {
			this.showTable = false;
			return;
		}

		this.showTable = false;
		this.loading = true;
		this.utilsService.getData('api/Queues/GetMessages?queue=' + this.queue).subscribe(result => {
			this.loading = false;
			this.messages = JSON.parse(result);
			this.showTable = true;
		}, error => { this.setError(error); });
	}

	addMessage() {
		this.utilsService.postData('api/Queues/NewQueueMessage?queue=' + encodeURIComponent(this.queue) + '&message=' + encodeURIComponent(this.newMessage.nativeElement.value), null).subscribe(result => {
			this.getMessages();
			this.newMessage.nativeElement.value = '';
		}, error => { this.setError(error); });
	}

	removeMessage(event: Event) {
		var element = (event.currentTarget as Element); //button
		var messageId: string = element.parentElement!.parentElement!.children[2]!.textContent!;	

		this.selected = messageId;
	}

	deleteMessage() {
		this.utilsService.postData('api/Queues/DeleteMessage?queue=' + encodeURIComponent(this.queue) + '&messageId=' + encodeURIComponent(this.selected), null).subscribe(result => {
      this.selected = '';
      setTimeout(() => {
        this.getMessages(); //for some reason it taks a while to update the queue metadata after message deletion
      }, 500);
			
		}, error => { this.setError(error); });
	}

	cancelDeleteMessage() {
		this.selected = '';
	}

	removeQueue(event: Event) {
		this.removeQueueFlag = true;
	}

	cancelDeleteQueue() {
		this.removeQueueFlag = false;
	}

	deleteQueue() {
		this.utilsService.postData('api/Queues/DeleteQueue?queue=' + this.queue, null).subscribe(result => {
			this.queue = "";
			this.removeQueueFlag = false;
			this.refresh.emit(true);
		}, error => { this.setError(error); });
	}

	typingMessage(event: KeyboardEvent) {
		if (event.key == 'Enter')
			this.addMessage();
	}
}

import { Component, Inject, ViewChild } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'queues',
	templateUrl: './queues.component.html'
})

export class QueuesComponent extends BaseComponent {
	public queues: string[] | undefined;

	public selectedQueue: string = '';

  @ViewChild('newQueueName', { read: null, static: false }) newQueueName: any;
  @ViewChild('queuesMenu', { read: null, static: false }) queuesMenu: any;

	constructor(utils: UtilsService) {
		super(utils);
		this.getQueues();
	}

	ngOnChanges() {
		this.getQueues();
	}

	getQueues() {
		this.loading = true;
		this.utilsService.getData('api/Queues/GetQueues').subscribe(result => {
			this.loading = false;
			this.queues = JSON.parse(result);
		}, error => { this.setErrorMessage(error.statusText); });

	}

	queueChanged(event: Event) {
		var element = (event.currentTarget as Element);
		var queue = (element.textContent as string).trim();

		var nodes = this.queuesMenu.nativeElement.childNodes;
		for (var i = 0; i < nodes.length; i++) {
			if (nodes[i].classList)
				nodes[i].classList.remove("active");
		}

		element.classList.add("active");

		this.selectedQueue = queue;
	}

	newQueue(event: Event) {
		this.utilsService.postData('api/Queues/NewQueue?queue=' + this.newQueueName.nativeElement.value, null).subscribe(result => {
			this.newQueueName.nativeElement.value = "";
			this.getQueues();
		}, error => { this.setErrorMessage(error.statusText); });

	}

	forceRefresh(force: boolean) {
		if (force) {
			this.getQueues();
			this.selectedQueue = '';
		}
	}
}

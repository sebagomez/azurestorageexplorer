import { Component, Injectable } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';

@Component({
	selector: 'base-component',
	template: '',
})

@Injectable()
export class BaseComponent {

	public errorMessage: string = '';
	public hasErrors: boolean = false;
	public loading: boolean = false;

	constructor(public utilsService: UtilsService) {
		this.loading = false;
	}

	setErrorMessage(message: string) {
		this.errorMessage = message;
		this.hasErrors = true;
		this.loading = false;

		setTimeout(() => {
			this.errorMessage = '';
			this.hasErrors = false;
		}, 5000);
	}
}

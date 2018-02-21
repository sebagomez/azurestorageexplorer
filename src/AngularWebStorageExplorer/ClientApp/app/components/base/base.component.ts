import { Component, Injectable } from '@angular/core';
import { UtilsService } from '../../services/utils/utils.service';

@Component({
	selector: 'base-component',
	template: '',
	//templateUrl: './base.component.html'
})

@Injectable()
export class BaseComponent {

	public errorMessage: string;
	public hasErrors: boolean = false;

	constructor(public utilsService: UtilsService) { }

	setErrorMessage(message: string) {
		this.errorMessage = message;
		this.hasErrors = true;

		setTimeout(() => {
			this.errorMessage = '';
			this.hasErrors = false;
		}, 5000);
	}
}
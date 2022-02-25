import { Component, Injectable } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';
import { HttpErrorResponse } from '@angular/common/http';

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

	setHttpError(error: HttpErrorResponse) {
		var desc = JSON.parse(error.error);
		this.setErrorMessage(desc.title);
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

	setError(error: any) {
		if (error.error && error.error.statusText) {
			this.setErrorMessage(error.error.statusText);
			return;
		}
		if (error.message) {
			this.setErrorMessage(error.message);
			return;
		}
		this.setErrorMessage(error.statusText);
	}
}

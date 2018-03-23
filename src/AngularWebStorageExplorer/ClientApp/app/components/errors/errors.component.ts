import { Component, Inject, Injector, Injectable, ViewChild, ErrorHandler } from '@angular/core';

@Component({
	selector: 'errors',
	//template: ''
	templateUrl: './errors.component.html'
})

@Injectable()
export class MyErrorsHandler implements ErrorHandler {

	public errorMessage: string;
	public showErrors: boolean = false;

	handleError(error: any) {

		this.showErrors = true;
		console.error(error);

		if (error.statusText)
			alert(error.statusText)
		else if (error.message)
			alert(error.message)
		else
			alert(error);
	}
}
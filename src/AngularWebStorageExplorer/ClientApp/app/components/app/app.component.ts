import { Component, ViewChild, Renderer2, Inject } from '@angular/core';

@Component({
	selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
	public loggedIn: boolean = false;

	loggedInHandler(logged: boolean) {
		this.loggedIn = logged;
	}
}

import { Component, ViewChild, Renderer2, Inject } from '@angular/core';
import { Router } from '@angular/router';


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

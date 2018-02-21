import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { ContainersComponent } from './components/containers/containers.component';
import { BlobsComponent } from './components/blobs/blobs.component';
import { QueuesComponent } from './components/queues/queues.component';
import { QmessagesComponent } from './components/qmessages/qmessages.component';
import { TablesComponent } from './components/tables/tables.component';
import { TabledataComponent } from './components/tabledata/tabledata.component';
import { LoginComponent } from './components/login/login.component';
import { MyErrorsHandler } from './components/errors/errors.component';
import { BaseComponent } from './components/base/base.component';

@NgModule({
	declarations: [
		AppComponent,
		NavMenuComponent,
		HomeComponent,
		ContainersComponent,
		BlobsComponent,
		QueuesComponent,
		QmessagesComponent,
		TablesComponent,
		TabledataComponent,
		LoginComponent,
		MyErrorsHandler,
		BaseComponent
	],
	imports: [
		CommonModule,
		HttpModule,
		FormsModule,
		RouterModule.forRoot([
			{ path: '', redirectTo: 'home', pathMatch: 'full' },
			{ path: 'home', component: HomeComponent },
			{ path: 'containers', component: ContainersComponent },
			{ path: 'queues', component: QueuesComponent },
			{ path: 'tables', component: TablesComponent },
			{ path: '**', redirectTo: 'home' }
		])
	],
	providers: [
		{
			provide: ErrorHandler,
			useClass: MyErrorsHandler
		}
	]
})
export class AppModuleShared {
}

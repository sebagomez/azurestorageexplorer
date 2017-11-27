import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { ContainersComponent } from './components/containers/containers.component';
import { BlobsComponent } from './components/blobs/blobs.component';
import { QueuesComponent } from './components/queues/queues.component';
import { QmessagesComponent } from './components/qmessages/qmessages.component';
import { TablesComponent } from './components/tables/tables.component';
import { TabledataComponent } from './components/tabledata/tabledata.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
		HomeComponent,
		ContainersComponent,
		BlobsComponent,
		QueuesComponent,
		QmessagesComponent,
		TablesComponent,
		TabledataComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
			{ path: 'fetch-data', component: FetchDataComponent },
			{ path: 'containers', component: ContainersComponent },
			{ path: 'queues', component: QueuesComponent },
			{ path: 'tables', component: TablesComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { LoginComponent } from './login/login.component';
import { VersionComponent } from './version/version.component';
import { ContainersComponent } from './containers/containers.component'
import { BlobsComponent } from './blobs/blobs.component'
import { QmessagesComponent } from './qmessages/qmessages.component'
import { QueuesComponent } from './queues/queues.component'
import { TablesComponent } from './tables/tables.component'
import { TabledataComponent } from './tabledata/tabledata.component'
import { SharesComponent } from './shares/shares.component';
import { FilesComponent } from './files/files.component'
import { BaseComponent } from './base/base.component'

import { UtilsService } from './services/utils/utils.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavMenuComponent,
    LoginComponent,
    VersionComponent,
    ContainersComponent,
    BlobsComponent,
    QmessagesComponent,
    QueuesComponent,
    TablesComponent,
    TabledataComponent,
    SharesComponent,
    FilesComponent,
    BaseComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'containers', component: ContainersComponent },
      { path: 'queues', component: QueuesComponent },
      { path: 'tables', component: TablesComponent },
      { path: 'files', component: SharesComponent }
    ])
  ],
  providers: [UtilsService],
  bootstrap: [AppComponent]
})
export class AppModule { }

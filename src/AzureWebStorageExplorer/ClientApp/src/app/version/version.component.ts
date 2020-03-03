import { Component } from '@angular/core';
import { UtilsService } from '../services/utils/utils.service';

@Component({
	selector: 'version',
	templateUrl: './version.component.html'
})
export class VersionComponent {

	public currentVersion: string | undefined;

	constructor(private utilsService: UtilsService) {
		this.getversion();
	}

	ngOnChanges() {
		this.getversion();
	}

	getversion() {
    this.utilsService.getData('api/Util/GetVersion')
      .subscribe(result => {
        this.currentVersion = result.toString()
      });
	}
}

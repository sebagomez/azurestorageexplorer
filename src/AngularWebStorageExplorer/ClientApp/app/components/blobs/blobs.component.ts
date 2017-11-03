import { Component, Inject, Input } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	selector: 'blobs',
	templateUrl: './blobs.component.html'
})
export class BlobsComponent {
	@Input() container: string = "";

	public blobs: string[];

	forceReload: boolean;
	http: Http;
	baseUrl: string;

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

		this.http = http;
		this.baseUrl = baseUrl;

		this.getBlobs();
	}

	ngOnChanges() {
		this.getBlobs();
	}

	getBlobs() {

		if (!this.container)
			return;

		this.http.get(this.baseUrl + 'api/Blobs/GetBlobs?container=' + this.container).subscribe(result => {
			this.blobs = result.json();
		}, error => console.error(error));
	}

	removeBlob(event: Event) {
		var element = (event.currentTarget as Element); //button
		var blob : string = element.parentElement!.parentElement!.children[1]!.textContent!;

		this.http.post(this.baseUrl + 'api/Blobs/DeleteBlob?blobUri=' + encodeURIComponent(blob), null).subscribe(result => {
			this.getBlobs();
		}, error => console.error(error));
	}

}

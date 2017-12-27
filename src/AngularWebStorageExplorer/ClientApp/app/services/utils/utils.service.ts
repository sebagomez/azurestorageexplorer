import { Directive, Component, ViewChild, Renderer2, Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';

@Injectable()
export class UtilsService {

	http: Http;
	baseUrl: string;

	account: string;
	key: string;

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

		this.http = http;
		this.baseUrl = baseUrl;

	}

	loadCredentials(url: string) {

		if (!this.account || !this.key) {
			this.account = localStorage.getItem('account')!;
			this.key = localStorage.getItem('key')!;
		}

		let credentials = '?account=' + this.account + '&key=' + this.key;

		if (url.lastIndexOf('?') > 0)
			credentials = credentials.replace('?', '&');

		return credentials;
	}

	getData(url: string) {
		let credentials = this.loadCredentials(url);
		return this.http.get(this.baseUrl + url + credentials);
	}

	postData(url: string, body: any) {
		let credentials = this.loadCredentials(url);
		return this.http.post(this.baseUrl + url + credentials, body);
	}
}

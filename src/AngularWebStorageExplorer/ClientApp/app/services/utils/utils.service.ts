import { Directive, Component, ViewChild, Renderer2, Inject, Injectable } from '@angular/core';
import { Http, ResponseContentType } from '@angular/http';

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

	private loadCredentials(url: string) {

		if (!this.account || !this.key) {
			this.account = localStorage.getItem('account')!;
			this.key = localStorage.getItem('key')!;
		}

		let credentials = '?account=' + this.account + '&key=' + this.key;

		if (url.lastIndexOf('?') > 0)
			credentials = credentials.replace('?', '&');

		return credentials;
	}

	signIn(account: string, key: string) {
		return this.http.get(this.baseUrl + 'api/Queues/GetQueues?account=' + account + '&key=' + key);
	}

	getData(url: string) {
		let credentials = this.loadCredentials(url);
		return this.http.get(this.baseUrl + url + credentials);
	}

	getFile(url: string) {
		let credentials = this.loadCredentials(url);
		return this.http.get(this.baseUrl + url + credentials, { responseType: ResponseContentType.ArrayBuffer });
	}

	postData(url: string, body: any) {
		let credentials = this.loadCredentials(url);
		return this.http.post(this.baseUrl + url + credentials, body);
	}

	putData(url: string, body: any) {
		let credentials = this.loadCredentials(url);
		return this.http.put(this.baseUrl + url + credentials, body);
	}

	deleteData(url: string) {
		let credentials = this.loadCredentials(url);
		return this.http.delete(this.baseUrl + url + credentials);
	}

	uploadFile(url: string, data: FormData) {
		const xhr = new XMLHttpRequest();
		let credentials = this.loadCredentials(url);
		xhr.open('POST', this.baseUrl + url + credentials, true);
		xhr.send(data);
		return xhr;
	}
}

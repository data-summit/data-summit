
import {throwError as observableThrowError,  Observable ,  Subject } from 'rxjs';

import {finalize, catchError} from 'rxjs/operators';
import { Injectable } from "@angular/core";

import { HttpClient } from "@angular/common/http";

import { environment } from "../../../environments/environment";

import { URLSearchParams } from '@angular/http';

export class ApiOptions {
  method: string;
  url: string;
  headers: any = {};
  params = {};
  data = {};
  tenantGid: string;
}

@Injectable()
export class ApiService {
  private baseUrl: string = "";

  // define the internal subject we"ll use to push the command count
  private pendingCommandsSubject = new Subject<number>();
  private pendingCommandCount = 0;
  // private _version = GuidUtils.generateGUID();

  // provide the *public* observable that clients can subscribe to
  private pendingCommands$: Observable<number>;

  constructor(
    private http: HttpClient
  ) {
    this.pendingCommands$ = this.pendingCommandsSubject.asObservable();

    // get remoteservice url location from the environment
    this.baseUrl = environment.remoteServiceBaseUrl;
  }

  // http overrides
  get(url: string, params?: any): Observable<any> {
    let options = new ApiOptions();
    this.addBearerToken(options);
    this.addContentType(options);
    this.addAcceptType(options);
    this.addCors(options);

    let url_ = this.Url(url);
    return this.http.get(url_, options);
  }

  post(url: string, data?: any, params?: any): Observable<any> {
    if (!data) {
      data = params;
      params = {};
    }
    let url_ = this.Url(url);

    let options = new ApiOptions();
    options.method = 'POST';
    options.url = url_;
    options.params = params;
    options.data = data;
    return this.request(options);
  }

  postImage(url: string, data?: any, params?: any): Observable<any> {
    if (!data) {
      data = params;
      params = {};
    }
    let url_ = this.Url(url);

    let options = new ApiOptions();
    options.method = 'POST';
    options.url = url_;

    this.interpolateUrl(options);
    this.addXsrfToken(options);
    this.addBearerToken(options);
    this.addCors(options);

    return this.http.post(url_, data, options);
    //return this.request(options, true);
  }

  put(url: string, data?: any, params?: any): Observable<Response> {
    if (!data) {
      data = params;
      params = {};
    }
    let url_ = this.Url(url);

    let options = new ApiOptions();
    options.method = 'PUT';
    options.url = url_;
    options.params = params;
    options.data = data;
    return this.request(options);
  }

  delete(url: string, params?: any): Observable<Response> {
    let url_ = this.Url(url);
    let options = new ApiOptions();
    options.method = 'DELETE';
    options.url = url_;
    options.params = params;

    return this.request(options);
  }

  Url(url: string): string {
    return this.baseUrl ? this.baseUrl + url : url;
  }

  // internal methods

  private request(options: ApiOptions): Observable<any> {
    options.method = options.method || 'GET';
    options.url = options.url || "";
    options.headers = options.headers || {};
    options.params = options.params || {};
    options.data = options.data || {};

    this.interpolateUrl(options);
    this.addXsrfToken(options);
    this.addContentType(options);
    this.addBearerToken(options);
    this.addCors(options);
   
    options['body'] =  JSON.stringify(options.data);
    options.params = this.buildUrlSearchParams(options.params)

    let isCommand = options.method !== 'GET';

    if (isCommand) {
      this.pendingCommandsSubject.next(++this.pendingCommandCount);
    }

    let stream = this.http
      .request(options.method, options.url, options).pipe(
      catchError((error: any) => {
      this.handleError(error);
        return observableThrowError(error);
      }),
      catchError((error: any) => {
        return observableThrowError(this.unwrapHttpError(error));
      }),
      finalize(() => {
        if (isCommand) {
          this.pendingCommandsSubject.next(--this.pendingCommandCount);
        }
      }),);

    return stream;
  }

  private addContentType(options: ApiOptions): ApiOptions {
    if (options.method !== 'GET') {
      options.headers["Content-Type"] = "application/json; charset=UTF-8";
    }

    options.headers["Cache-Control"] = "no-cache";
    options.headers["Pragma"] = "no-cache";
    options.headers["Expires"] = "-1";

    return options;
  }

  private addAcceptType(options: ApiOptions): ApiOptions {
    if (options.method !== 'GET') {
      options.headers["Accept"] = "application/json; charset=UTF-8";
    }
    return options;
  }

  private addBearerToken(options: ApiOptions): ApiOptions {
    if (sessionStorage.getItem("access_token")) {
       options.headers.Authorization = "Bearer " + sessionStorage.getItem("access_token");
    }
    return options;
  }

  private extractValue(collection: any, key: string): any {
    let value = collection[key];
    delete collection[key];
    return value;
  }

  private addXsrfToken(options: ApiOptions): ApiOptions {
    let xsrfToken = this.getXsrfCookie();
    if (xsrfToken) {
      options.headers["X-XSRF-TOKEN"] = xsrfToken;
    }
    return options;
  }

  private getXsrfCookie(): string {
    let matches = document.cookie.match(/\bXSRF-TOKEN=([^\s;]+)/);
    try {
      return matches && decodeURIComponent(matches[1]);
    } catch (decodeError) {
      return "";
    }
  }

  private addCors(options: ApiOptions): ApiOptions {
    options.headers["Access-Control-Allow-Origin"] = "*";
    //options.headers.append('Access-Control-Allow-Origin', 'http://localhost:45678');
    options.headers['Access-Control-Allow-Credentials'] = 'true';
    return options;
  }

  private buildUrlSearchParams(params: any): URLSearchParams {
    let searchParams = new URLSearchParams();
    for (let key in params) {
      if (params.hasOwnProperty(key)) {
        searchParams.append(key, params[key]);
      }
    }
    return searchParams;
  }

  private interpolateUrl(options: ApiOptions): ApiOptions {
    options.url = options.url.replace(/:([a-zA-Z]+[\w-]*)/g, ($0, token) => {
      // try to move matching token from the params collection.
      if (options.params.hasOwnProperty(token)) {
        return this.extractValue(options.params, token);
      }
      // try to move matching token from the data collection.
      if (options.data.hasOwnProperty(token)) {
        return this.extractValue(options.data, token);
      }
      // if a matching value couldn"t be found, just replace
      // the token with the empty string.
      return "";
    });

    // clean up any repeating slashes.
    //options.url = options.url.replace(/\/{2,}/g, "/");
    // clean up any trailing slashes.
    options.url = options.url.replace(/\/+$/g, "");

    return options;
  }

  private handleError(error: any) {
    if (error.status === 401) {
    //   this.authService.startAuthentication();
    }

    if (!environment.production) {
      console.error("ERROR: ", error);
    }
  }

  private unwrapHttpError(error: any): any {
    try {
      return error.json();
    } catch (jsonError) {
      try {
        if (error.message) {
          return error;
        }
        var errResp = <Response>error;
        if (errResp) {
          var msg = errResp.text();
          return {
            code: errResp.status,
            message: msg.then((res) => res == "" ? errResp.statusText : errResp.text()) 
          };
        }
      } catch (jsonError) {
        return {
          code: -1,
          message: "An unexpected error occurred."
        };
      }
    }

    return {
      code: -1,
      message: "An unexpected error occurred."
    };
  }

  private unwrapHttpValue(value: Response): any {
    return value.json();
  }

}

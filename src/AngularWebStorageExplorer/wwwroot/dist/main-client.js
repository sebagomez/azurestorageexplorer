/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// identity function for calling harmony imports with the correct context
/******/ 	__webpack_require__.i = function(value) { return value; };
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "dist/";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 26);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(3);

/***/ }),
/* 1 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return UtilsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__(6);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};


var UtilsService = (function () {
    function UtilsService(http, baseUrl) {
        this.account = '';
        this.key = '';
        this.http = http;
        this.baseUrl = baseUrl;
    }
    UtilsService.prototype.loadCredentials = function (url) {
        if (!this.account || !this.key) {
            this.account = localStorage.getItem('account');
            this.key = localStorage.getItem('key');
        }
        var credentials = '?account=' + this.account + '&key=' + this.key;
        if (url.lastIndexOf('?') > 0)
            credentials = credentials.replace('?', '&');
        return credentials;
    };
    UtilsService.prototype.signIn = function (account, key) {
        return this.http.get(this.baseUrl + 'api/Queues/GetQueues?account=' + account + '&key=' + key);
    };
    UtilsService.prototype.logOut = function () {
        localStorage.setItem('account', '');
        localStorage.setItem('key', '');
    };
    UtilsService.prototype.getData = function (url) {
        var credentials = this.loadCredentials(url);
        return this.http.get(this.baseUrl + url + credentials);
    };
    UtilsService.prototype.getFile = function (url) {
        var credentials = this.loadCredentials(url);
        return this.http.get(this.baseUrl + url + credentials, { responseType: __WEBPACK_IMPORTED_MODULE_1__angular_http__["ResponseContentType"].ArrayBuffer });
    };
    UtilsService.prototype.postData = function (url, body) {
        var credentials = this.loadCredentials(url);
        return this.http.post(this.baseUrl + url + credentials, body);
    };
    UtilsService.prototype.putData = function (url, body) {
        var credentials = this.loadCredentials(url);
        return this.http.put(this.baseUrl + url + credentials, body);
    };
    UtilsService.prototype.deleteData = function (url) {
        var credentials = this.loadCredentials(url);
        return this.http.delete(this.baseUrl + url + credentials);
    };
    UtilsService.prototype.uploadFile = function (url, data) {
        var xhr = new XMLHttpRequest();
        var credentials = this.loadCredentials(url);
        xhr.open('POST', this.baseUrl + url + credentials, true);
        xhr.send(data);
        return xhr;
    };
    UtilsService = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __param(1, __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])('BASE_URL')),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_http__["Http"], String])
    ], UtilsService);
    return UtilsService;
}());



/***/ }),
/* 2 */
/***/ (function(module, exports) {

module.exports = vendor_8cb63956b4d1b52cb50d;

/***/ }),
/* 3 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return BaseComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var BaseComponent = (function () {
    function BaseComponent(utilsService) {
        this.utilsService = utilsService;
        this.errorMessage = '';
        this.hasErrors = false;
        this.loading = false;
        this.loading = false;
    }
    BaseComponent.prototype.setErrorMessage = function (message) {
        var _this = this;
        this.errorMessage = message;
        this.hasErrors = true;
        this.loading = false;
        setTimeout(function () {
            _this.errorMessage = '';
            _this.hasErrors = false;
        }, 5000);
    };
    BaseComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'base-component',
            template: '',
        }),
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], BaseComponent);
    return BaseComponent;
}());



/***/ }),
/* 4 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var AppComponent = (function () {
    function AppComponent() {
        this.loggedIn = false;
    }
    AppComponent.prototype.loggedInHandler = function (logged) {
        this.loggedIn = logged;
    };
    AppComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app',
            template: __webpack_require__(29),
            styles: [__webpack_require__(43)]
        })
    ], AppComponent);
    return AppComponent;
}());



/***/ }),
/* 5 */
/***/ (function(module, exports) {

/*
	MIT License http://www.opensource.org/licenses/mit-license.php
	Author Tobias Koppers @sokra
*/
// css base code, injected by the css-loader
module.exports = function(useSourceMap) {
	var list = [];

	// return the list of modules as css string
	list.toString = function toString() {
		return this.map(function (item) {
			var content = cssWithMappingToString(item, useSourceMap);
			if(item[2]) {
				return "@media " + item[2] + "{" + content + "}";
			} else {
				return content;
			}
		}).join("");
	};

	// import a list of modules into the list
	list.i = function(modules, mediaQuery) {
		if(typeof modules === "string")
			modules = [[null, modules, ""]];
		var alreadyImportedModules = {};
		for(var i = 0; i < this.length; i++) {
			var id = this[i][0];
			if(typeof id === "number")
				alreadyImportedModules[id] = true;
		}
		for(i = 0; i < modules.length; i++) {
			var item = modules[i];
			// skip already imported module
			// this implementation is not 100% perfect for weird media query combinations
			//  when a module is imported multiple times with different media queries.
			//  I hope this will never occur (Hey this way we have smaller bundles)
			if(typeof item[0] !== "number" || !alreadyImportedModules[item[0]]) {
				if(mediaQuery && !item[2]) {
					item[2] = mediaQuery;
				} else if(mediaQuery) {
					item[2] = "(" + item[2] + ") and (" + mediaQuery + ")";
				}
				list.push(item);
			}
		}
	};
	return list;
};

function cssWithMappingToString(item, useSourceMap) {
	var content = item[1] || '';
	var cssMapping = item[3];
	if (!cssMapping) {
		return content;
	}

	if (useSourceMap && typeof btoa === 'function') {
		var sourceMapping = toComment(cssMapping);
		var sourceURLs = cssMapping.sources.map(function (source) {
			return '/*# sourceURL=' + cssMapping.sourceRoot + source + ' */'
		});

		return [content].concat(sourceURLs).concat([sourceMapping]).join('\n');
	}

	return [content].join('\n');
}

// Adapted from convert-source-map (MIT)
function toComment(sourceMap) {
	// eslint-disable-next-line no-undef
	var base64 = btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap))));
	var data = 'sourceMappingURL=data:application/json;charset=utf-8;base64,' + base64;

	return '/*# ' + data + ' */';
}


/***/ }),
/* 6 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(39);

/***/ }),
/* 7 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* unused harmony export getBaseUrl */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser__ = __webpack_require__(48);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_module_shared__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__components_app_app_component__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_utils_utils_service__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};





var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            bootstrap: [__WEBPACK_IMPORTED_MODULE_3__components_app_app_component__["a" /* AppComponent */]],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser__["BrowserModule"],
                __WEBPACK_IMPORTED_MODULE_2__app_module_shared__["a" /* AppModuleShared */]
            ],
            providers: [
                { provide: 'BASE_URL', useFactory: getBaseUrl }, __WEBPACK_IMPORTED_MODULE_4__services_utils_utils_service__["a" /* UtilsService */]
            ]
        })
    ], AppModule);
    return AppModule;
}());

function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}


/***/ }),
/* 8 */
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function(process, global) {/*! *****************************************************************************
Copyright (C) Microsoft. All rights reserved.
Licensed under the Apache License, Version 2.0 (the "License"); you may not use
this file except in compliance with the License. You may obtain a copy of the
License at http://www.apache.org/licenses/LICENSE-2.0

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
MERCHANTABLITY OR NON-INFRINGEMENT.

See the Apache Version 2.0 License for specific language governing permissions
and limitations under the License.
***************************************************************************** */
var Reflect;
(function (Reflect) {
    "use strict";
    var hasOwn = Object.prototype.hasOwnProperty;
    // feature test for Symbol support
    var supportsSymbol = typeof Symbol === "function";
    var toPrimitiveSymbol = supportsSymbol && typeof Symbol.toPrimitive !== "undefined" ? Symbol.toPrimitive : "@@toPrimitive";
    var iteratorSymbol = supportsSymbol && typeof Symbol.iterator !== "undefined" ? Symbol.iterator : "@@iterator";
    var HashMap;
    (function (HashMap) {
        var supportsCreate = typeof Object.create === "function"; // feature test for Object.create support
        var supportsProto = { __proto__: [] } instanceof Array; // feature test for __proto__ support
        var downLevel = !supportsCreate && !supportsProto;
        // create an object in dictionary mode (a.k.a. "slow" mode in v8)
        HashMap.create = supportsCreate
            ? function () { return MakeDictionary(Object.create(null)); }
            : supportsProto
                ? function () { return MakeDictionary({ __proto__: null }); }
                : function () { return MakeDictionary({}); };
        HashMap.has = downLevel
            ? function (map, key) { return hasOwn.call(map, key); }
            : function (map, key) { return key in map; };
        HashMap.get = downLevel
            ? function (map, key) { return hasOwn.call(map, key) ? map[key] : undefined; }
            : function (map, key) { return map[key]; };
    })(HashMap || (HashMap = {}));
    // Load global or shim versions of Map, Set, and WeakMap
    var functionPrototype = Object.getPrototypeOf(Function);
    var usePolyfill = typeof process === "object" && process.env && process.env["REFLECT_METADATA_USE_MAP_POLYFILL"] === "true";
    var _Map = !usePolyfill && typeof Map === "function" && typeof Map.prototype.entries === "function" ? Map : CreateMapPolyfill();
    var _Set = !usePolyfill && typeof Set === "function" && typeof Set.prototype.entries === "function" ? Set : CreateSetPolyfill();
    var _WeakMap = !usePolyfill && typeof WeakMap === "function" ? WeakMap : CreateWeakMapPolyfill();
    // [[Metadata]] internal slot
    // https://rbuckton.github.io/reflect-metadata/#ordinary-object-internal-methods-and-internal-slots
    var Metadata = new _WeakMap();
    /**
      * Applies a set of decorators to a property of a target object.
      * @param decorators An array of decorators.
      * @param target The target object.
      * @param propertyKey (Optional) The property key to decorate.
      * @param attributes (Optional) The property descriptor for the target key.
      * @remarks Decorators are applied in reverse order.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     Example = Reflect.decorate(decoratorsArray, Example);
      *
      *     // property (on constructor)
      *     Reflect.decorate(decoratorsArray, Example, "staticProperty");
      *
      *     // property (on prototype)
      *     Reflect.decorate(decoratorsArray, Example.prototype, "property");
      *
      *     // method (on constructor)
      *     Object.defineProperty(Example, "staticMethod",
      *         Reflect.decorate(decoratorsArray, Example, "staticMethod",
      *             Object.getOwnPropertyDescriptor(Example, "staticMethod")));
      *
      *     // method (on prototype)
      *     Object.defineProperty(Example.prototype, "method",
      *         Reflect.decorate(decoratorsArray, Example.prototype, "method",
      *             Object.getOwnPropertyDescriptor(Example.prototype, "method")));
      *
      */
    function decorate(decorators, target, propertyKey, attributes) {
        if (!IsUndefined(propertyKey)) {
            if (!IsArray(decorators))
                throw new TypeError();
            if (!IsObject(target))
                throw new TypeError();
            if (!IsObject(attributes) && !IsUndefined(attributes) && !IsNull(attributes))
                throw new TypeError();
            if (IsNull(attributes))
                attributes = undefined;
            propertyKey = ToPropertyKey(propertyKey);
            return DecorateProperty(decorators, target, propertyKey, attributes);
        }
        else {
            if (!IsArray(decorators))
                throw new TypeError();
            if (!IsConstructor(target))
                throw new TypeError();
            return DecorateConstructor(decorators, target);
        }
    }
    Reflect.decorate = decorate;
    // 4.1.2 Reflect.metadata(metadataKey, metadataValue)
    // https://rbuckton.github.io/reflect-metadata/#reflect.metadata
    /**
      * A default metadata decorator factory that can be used on a class, class member, or parameter.
      * @param metadataKey The key for the metadata entry.
      * @param metadataValue The value for the metadata entry.
      * @returns A decorator function.
      * @remarks
      * If `metadataKey` is already defined for the target and target key, the
      * metadataValue for that key will be overwritten.
      * @example
      *
      *     // constructor
      *     @Reflect.metadata(key, value)
      *     class Example {
      *     }
      *
      *     // property (on constructor, TypeScript only)
      *     class Example {
      *         @Reflect.metadata(key, value)
      *         static staticProperty;
      *     }
      *
      *     // property (on prototype, TypeScript only)
      *     class Example {
      *         @Reflect.metadata(key, value)
      *         property;
      *     }
      *
      *     // method (on constructor)
      *     class Example {
      *         @Reflect.metadata(key, value)
      *         static staticMethod() { }
      *     }
      *
      *     // method (on prototype)
      *     class Example {
      *         @Reflect.metadata(key, value)
      *         method() { }
      *     }
      *
      */
    function metadata(metadataKey, metadataValue) {
        function decorator(target, propertyKey) {
            if (!IsObject(target))
                throw new TypeError();
            if (!IsUndefined(propertyKey) && !IsPropertyKey(propertyKey))
                throw new TypeError();
            OrdinaryDefineOwnMetadata(metadataKey, metadataValue, target, propertyKey);
        }
        return decorator;
    }
    Reflect.metadata = metadata;
    /**
      * Define a unique metadata entry on the target.
      * @param metadataKey A key used to store and retrieve metadata.
      * @param metadataValue A value that contains attached metadata.
      * @param target The target object on which to define metadata.
      * @param propertyKey (Optional) The property key for the target.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     Reflect.defineMetadata("custom:annotation", options, Example);
      *
      *     // property (on constructor)
      *     Reflect.defineMetadata("custom:annotation", options, Example, "staticProperty");
      *
      *     // property (on prototype)
      *     Reflect.defineMetadata("custom:annotation", options, Example.prototype, "property");
      *
      *     // method (on constructor)
      *     Reflect.defineMetadata("custom:annotation", options, Example, "staticMethod");
      *
      *     // method (on prototype)
      *     Reflect.defineMetadata("custom:annotation", options, Example.prototype, "method");
      *
      *     // decorator factory as metadata-producing annotation.
      *     function MyAnnotation(options): Decorator {
      *         return (target, key?) => Reflect.defineMetadata("custom:annotation", options, target, key);
      *     }
      *
      */
    function defineMetadata(metadataKey, metadataValue, target, propertyKey) {
        if (!IsObject(target))
            throw new TypeError();
        if (!IsUndefined(propertyKey))
            propertyKey = ToPropertyKey(propertyKey);
        return OrdinaryDefineOwnMetadata(metadataKey, metadataValue, target, propertyKey);
    }
    Reflect.defineMetadata = defineMetadata;
    /**
      * Gets a value indicating whether the target object or its prototype chain has the provided metadata key defined.
      * @param metadataKey A key used to store and retrieve metadata.
      * @param target The target object on which the metadata is defined.
      * @param propertyKey (Optional) The property key for the target.
      * @returns `true` if the metadata key was defined on the target object or its prototype chain; otherwise, `false`.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     result = Reflect.hasMetadata("custom:annotation", Example);
      *
      *     // property (on constructor)
      *     result = Reflect.hasMetadata("custom:annotation", Example, "staticProperty");
      *
      *     // property (on prototype)
      *     result = Reflect.hasMetadata("custom:annotation", Example.prototype, "property");
      *
      *     // method (on constructor)
      *     result = Reflect.hasMetadata("custom:annotation", Example, "staticMethod");
      *
      *     // method (on prototype)
      *     result = Reflect.hasMetadata("custom:annotation", Example.prototype, "method");
      *
      */
    function hasMetadata(metadataKey, target, propertyKey) {
        if (!IsObject(target))
            throw new TypeError();
        if (!IsUndefined(propertyKey))
            propertyKey = ToPropertyKey(propertyKey);
        return OrdinaryHasMetadata(metadataKey, target, propertyKey);
    }
    Reflect.hasMetadata = hasMetadata;
    /**
      * Gets a value indicating whether the target object has the provided metadata key defined.
      * @param metadataKey A key used to store and retrieve metadata.
      * @param target The target object on which the metadata is defined.
      * @param propertyKey (Optional) The property key for the target.
      * @returns `true` if the metadata key was defined on the target object; otherwise, `false`.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     result = Reflect.hasOwnMetadata("custom:annotation", Example);
      *
      *     // property (on constructor)
      *     result = Reflect.hasOwnMetadata("custom:annotation", Example, "staticProperty");
      *
      *     // property (on prototype)
      *     result = Reflect.hasOwnMetadata("custom:annotation", Example.prototype, "property");
      *
      *     // method (on constructor)
      *     result = Reflect.hasOwnMetadata("custom:annotation", Example, "staticMethod");
      *
      *     // method (on prototype)
      *     result = Reflect.hasOwnMetadata("custom:annotation", Example.prototype, "method");
      *
      */
    function hasOwnMetadata(metadataKey, target, propertyKey) {
        if (!IsObject(target))
            throw new TypeError();
        if (!IsUndefined(propertyKey))
            propertyKey = ToPropertyKey(propertyKey);
        return OrdinaryHasOwnMetadata(metadataKey, target, propertyKey);
    }
    Reflect.hasOwnMetadata = hasOwnMetadata;
    /**
      * Gets the metadata value for the provided metadata key on the target object or its prototype chain.
      * @param metadataKey A key used to store and retrieve metadata.
      * @param target The target object on which the metadata is defined.
      * @param propertyKey (Optional) The property key for the target.
      * @returns The metadata value for the metadata key if found; otherwise, `undefined`.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     result = Reflect.getMetadata("custom:annotation", Example);
      *
      *     // property (on constructor)
      *     result = Reflect.getMetadata("custom:annotation", Example, "staticProperty");
      *
      *     // property (on prototype)
      *     result = Reflect.getMetadata("custom:annotation", Example.prototype, "property");
      *
      *     // method (on constructor)
      *     result = Reflect.getMetadata("custom:annotation", Example, "staticMethod");
      *
      *     // method (on prototype)
      *     result = Reflect.getMetadata("custom:annotation", Example.prototype, "method");
      *
      */
    function getMetadata(metadataKey, target, propertyKey) {
        if (!IsObject(target))
            throw new TypeError();
        if (!IsUndefined(propertyKey))
            propertyKey = ToPropertyKey(propertyKey);
        return OrdinaryGetMetadata(metadataKey, target, propertyKey);
    }
    Reflect.getMetadata = getMetadata;
    /**
      * Gets the metadata value for the provided metadata key on the target object.
      * @param metadataKey A key used to store and retrieve metadata.
      * @param target The target object on which the metadata is defined.
      * @param propertyKey (Optional) The property key for the target.
      * @returns The metadata value for the metadata key if found; otherwise, `undefined`.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     result = Reflect.getOwnMetadata("custom:annotation", Example);
      *
      *     // property (on constructor)
      *     result = Reflect.getOwnMetadata("custom:annotation", Example, "staticProperty");
      *
      *     // property (on prototype)
      *     result = Reflect.getOwnMetadata("custom:annotation", Example.prototype, "property");
      *
      *     // method (on constructor)
      *     result = Reflect.getOwnMetadata("custom:annotation", Example, "staticMethod");
      *
      *     // method (on prototype)
      *     result = Reflect.getOwnMetadata("custom:annotation", Example.prototype, "method");
      *
      */
    function getOwnMetadata(metadataKey, target, propertyKey) {
        if (!IsObject(target))
            throw new TypeError();
        if (!IsUndefined(propertyKey))
            propertyKey = ToPropertyKey(propertyKey);
        return OrdinaryGetOwnMetadata(metadataKey, target, propertyKey);
    }
    Reflect.getOwnMetadata = getOwnMetadata;
    /**
      * Gets the metadata keys defined on the target object or its prototype chain.
      * @param target The target object on which the metadata is defined.
      * @param propertyKey (Optional) The property key for the target.
      * @returns An array of unique metadata keys.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     result = Reflect.getMetadataKeys(Example);
      *
      *     // property (on constructor)
      *     result = Reflect.getMetadataKeys(Example, "staticProperty");
      *
      *     // property (on prototype)
      *     result = Reflect.getMetadataKeys(Example.prototype, "property");
      *
      *     // method (on constructor)
      *     result = Reflect.getMetadataKeys(Example, "staticMethod");
      *
      *     // method (on prototype)
      *     result = Reflect.getMetadataKeys(Example.prototype, "method");
      *
      */
    function getMetadataKeys(target, propertyKey) {
        if (!IsObject(target))
            throw new TypeError();
        if (!IsUndefined(propertyKey))
            propertyKey = ToPropertyKey(propertyKey);
        return OrdinaryMetadataKeys(target, propertyKey);
    }
    Reflect.getMetadataKeys = getMetadataKeys;
    /**
      * Gets the unique metadata keys defined on the target object.
      * @param target The target object on which the metadata is defined.
      * @param propertyKey (Optional) The property key for the target.
      * @returns An array of unique metadata keys.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     result = Reflect.getOwnMetadataKeys(Example);
      *
      *     // property (on constructor)
      *     result = Reflect.getOwnMetadataKeys(Example, "staticProperty");
      *
      *     // property (on prototype)
      *     result = Reflect.getOwnMetadataKeys(Example.prototype, "property");
      *
      *     // method (on constructor)
      *     result = Reflect.getOwnMetadataKeys(Example, "staticMethod");
      *
      *     // method (on prototype)
      *     result = Reflect.getOwnMetadataKeys(Example.prototype, "method");
      *
      */
    function getOwnMetadataKeys(target, propertyKey) {
        if (!IsObject(target))
            throw new TypeError();
        if (!IsUndefined(propertyKey))
            propertyKey = ToPropertyKey(propertyKey);
        return OrdinaryOwnMetadataKeys(target, propertyKey);
    }
    Reflect.getOwnMetadataKeys = getOwnMetadataKeys;
    /**
      * Deletes the metadata entry from the target object with the provided key.
      * @param metadataKey A key used to store and retrieve metadata.
      * @param target The target object on which the metadata is defined.
      * @param propertyKey (Optional) The property key for the target.
      * @returns `true` if the metadata entry was found and deleted; otherwise, false.
      * @example
      *
      *     class Example {
      *         // property declarations are not part of ES6, though they are valid in TypeScript:
      *         // static staticProperty;
      *         // property;
      *
      *         constructor(p) { }
      *         static staticMethod(p) { }
      *         method(p) { }
      *     }
      *
      *     // constructor
      *     result = Reflect.deleteMetadata("custom:annotation", Example);
      *
      *     // property (on constructor)
      *     result = Reflect.deleteMetadata("custom:annotation", Example, "staticProperty");
      *
      *     // property (on prototype)
      *     result = Reflect.deleteMetadata("custom:annotation", Example.prototype, "property");
      *
      *     // method (on constructor)
      *     result = Reflect.deleteMetadata("custom:annotation", Example, "staticMethod");
      *
      *     // method (on prototype)
      *     result = Reflect.deleteMetadata("custom:annotation", Example.prototype, "method");
      *
      */
    function deleteMetadata(metadataKey, target, propertyKey) {
        if (!IsObject(target))
            throw new TypeError();
        if (!IsUndefined(propertyKey))
            propertyKey = ToPropertyKey(propertyKey);
        var metadataMap = GetOrCreateMetadataMap(target, propertyKey, /*Create*/ false);
        if (IsUndefined(metadataMap))
            return false;
        if (!metadataMap.delete(metadataKey))
            return false;
        if (metadataMap.size > 0)
            return true;
        var targetMetadata = Metadata.get(target);
        targetMetadata.delete(propertyKey);
        if (targetMetadata.size > 0)
            return true;
        Metadata.delete(target);
        return true;
    }
    Reflect.deleteMetadata = deleteMetadata;
    function DecorateConstructor(decorators, target) {
        for (var i = decorators.length - 1; i >= 0; --i) {
            var decorator = decorators[i];
            var decorated = decorator(target);
            if (!IsUndefined(decorated) && !IsNull(decorated)) {
                if (!IsConstructor(decorated))
                    throw new TypeError();
                target = decorated;
            }
        }
        return target;
    }
    function DecorateProperty(decorators, target, propertyKey, descriptor) {
        for (var i = decorators.length - 1; i >= 0; --i) {
            var decorator = decorators[i];
            var decorated = decorator(target, propertyKey, descriptor);
            if (!IsUndefined(decorated) && !IsNull(decorated)) {
                if (!IsObject(decorated))
                    throw new TypeError();
                descriptor = decorated;
            }
        }
        return descriptor;
    }
    function GetOrCreateMetadataMap(O, P, Create) {
        var targetMetadata = Metadata.get(O);
        if (IsUndefined(targetMetadata)) {
            if (!Create)
                return undefined;
            targetMetadata = new _Map();
            Metadata.set(O, targetMetadata);
        }
        var metadataMap = targetMetadata.get(P);
        if (IsUndefined(metadataMap)) {
            if (!Create)
                return undefined;
            metadataMap = new _Map();
            targetMetadata.set(P, metadataMap);
        }
        return metadataMap;
    }
    // 3.1.1.1 OrdinaryHasMetadata(MetadataKey, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinaryhasmetadata
    function OrdinaryHasMetadata(MetadataKey, O, P) {
        var hasOwn = OrdinaryHasOwnMetadata(MetadataKey, O, P);
        if (hasOwn)
            return true;
        var parent = OrdinaryGetPrototypeOf(O);
        if (!IsNull(parent))
            return OrdinaryHasMetadata(MetadataKey, parent, P);
        return false;
    }
    // 3.1.2.1 OrdinaryHasOwnMetadata(MetadataKey, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinaryhasownmetadata
    function OrdinaryHasOwnMetadata(MetadataKey, O, P) {
        var metadataMap = GetOrCreateMetadataMap(O, P, /*Create*/ false);
        if (IsUndefined(metadataMap))
            return false;
        return ToBoolean(metadataMap.has(MetadataKey));
    }
    // 3.1.3.1 OrdinaryGetMetadata(MetadataKey, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinarygetmetadata
    function OrdinaryGetMetadata(MetadataKey, O, P) {
        var hasOwn = OrdinaryHasOwnMetadata(MetadataKey, O, P);
        if (hasOwn)
            return OrdinaryGetOwnMetadata(MetadataKey, O, P);
        var parent = OrdinaryGetPrototypeOf(O);
        if (!IsNull(parent))
            return OrdinaryGetMetadata(MetadataKey, parent, P);
        return undefined;
    }
    // 3.1.4.1 OrdinaryGetOwnMetadata(MetadataKey, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinarygetownmetadata
    function OrdinaryGetOwnMetadata(MetadataKey, O, P) {
        var metadataMap = GetOrCreateMetadataMap(O, P, /*Create*/ false);
        if (IsUndefined(metadataMap))
            return undefined;
        return metadataMap.get(MetadataKey);
    }
    // 3.1.5.1 OrdinaryDefineOwnMetadata(MetadataKey, MetadataValue, O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinarydefineownmetadata
    function OrdinaryDefineOwnMetadata(MetadataKey, MetadataValue, O, P) {
        var metadataMap = GetOrCreateMetadataMap(O, P, /*Create*/ true);
        metadataMap.set(MetadataKey, MetadataValue);
    }
    // 3.1.6.1 OrdinaryMetadataKeys(O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinarymetadatakeys
    function OrdinaryMetadataKeys(O, P) {
        var ownKeys = OrdinaryOwnMetadataKeys(O, P);
        var parent = OrdinaryGetPrototypeOf(O);
        if (parent === null)
            return ownKeys;
        var parentKeys = OrdinaryMetadataKeys(parent, P);
        if (parentKeys.length <= 0)
            return ownKeys;
        if (ownKeys.length <= 0)
            return parentKeys;
        var set = new _Set();
        var keys = [];
        for (var _i = 0, ownKeys_1 = ownKeys; _i < ownKeys_1.length; _i++) {
            var key = ownKeys_1[_i];
            var hasKey = set.has(key);
            if (!hasKey) {
                set.add(key);
                keys.push(key);
            }
        }
        for (var _a = 0, parentKeys_1 = parentKeys; _a < parentKeys_1.length; _a++) {
            var key = parentKeys_1[_a];
            var hasKey = set.has(key);
            if (!hasKey) {
                set.add(key);
                keys.push(key);
            }
        }
        return keys;
    }
    // 3.1.7.1 OrdinaryOwnMetadataKeys(O, P)
    // https://rbuckton.github.io/reflect-metadata/#ordinaryownmetadatakeys
    function OrdinaryOwnMetadataKeys(O, P) {
        var keys = [];
        var metadataMap = GetOrCreateMetadataMap(O, P, /*Create*/ false);
        if (IsUndefined(metadataMap))
            return keys;
        var keysObj = metadataMap.keys();
        var iterator = GetIterator(keysObj);
        var k = 0;
        while (true) {
            var next = IteratorStep(iterator);
            if (!next) {
                keys.length = k;
                return keys;
            }
            var nextValue = IteratorValue(next);
            try {
                keys[k] = nextValue;
            }
            catch (e) {
                try {
                    IteratorClose(iterator);
                }
                finally {
                    throw e;
                }
            }
            k++;
        }
    }
    // 6 ECMAScript Data Typ0es and Values
    // https://tc39.github.io/ecma262/#sec-ecmascript-data-types-and-values
    function Type(x) {
        if (x === null)
            return 1 /* Null */;
        switch (typeof x) {
            case "undefined": return 0 /* Undefined */;
            case "boolean": return 2 /* Boolean */;
            case "string": return 3 /* String */;
            case "symbol": return 4 /* Symbol */;
            case "number": return 5 /* Number */;
            case "object": return x === null ? 1 /* Null */ : 6 /* Object */;
            default: return 6 /* Object */;
        }
    }
    // 6.1.1 The Undefined Type
    // https://tc39.github.io/ecma262/#sec-ecmascript-language-types-undefined-type
    function IsUndefined(x) {
        return x === undefined;
    }
    // 6.1.2 The Null Type
    // https://tc39.github.io/ecma262/#sec-ecmascript-language-types-null-type
    function IsNull(x) {
        return x === null;
    }
    // 6.1.5 The Symbol Type
    // https://tc39.github.io/ecma262/#sec-ecmascript-language-types-symbol-type
    function IsSymbol(x) {
        return typeof x === "symbol";
    }
    // 6.1.7 The Object Type
    // https://tc39.github.io/ecma262/#sec-object-type
    function IsObject(x) {
        return typeof x === "object" ? x !== null : typeof x === "function";
    }
    // 7.1 Type Conversion
    // https://tc39.github.io/ecma262/#sec-type-conversion
    // 7.1.1 ToPrimitive(input [, PreferredType])
    // https://tc39.github.io/ecma262/#sec-toprimitive
    function ToPrimitive(input, PreferredType) {
        switch (Type(input)) {
            case 0 /* Undefined */: return input;
            case 1 /* Null */: return input;
            case 2 /* Boolean */: return input;
            case 3 /* String */: return input;
            case 4 /* Symbol */: return input;
            case 5 /* Number */: return input;
        }
        var hint = PreferredType === 3 /* String */ ? "string" : PreferredType === 5 /* Number */ ? "number" : "default";
        var exoticToPrim = GetMethod(input, toPrimitiveSymbol);
        if (exoticToPrim !== undefined) {
            var result = exoticToPrim.call(input, hint);
            if (IsObject(result))
                throw new TypeError();
            return result;
        }
        return OrdinaryToPrimitive(input, hint === "default" ? "number" : hint);
    }
    // 7.1.1.1 OrdinaryToPrimitive(O, hint)
    // https://tc39.github.io/ecma262/#sec-ordinarytoprimitive
    function OrdinaryToPrimitive(O, hint) {
        if (hint === "string") {
            var toString_1 = O.toString;
            if (IsCallable(toString_1)) {
                var result = toString_1.call(O);
                if (!IsObject(result))
                    return result;
            }
            var valueOf = O.valueOf;
            if (IsCallable(valueOf)) {
                var result = valueOf.call(O);
                if (!IsObject(result))
                    return result;
            }
        }
        else {
            var valueOf = O.valueOf;
            if (IsCallable(valueOf)) {
                var result = valueOf.call(O);
                if (!IsObject(result))
                    return result;
            }
            var toString_2 = O.toString;
            if (IsCallable(toString_2)) {
                var result = toString_2.call(O);
                if (!IsObject(result))
                    return result;
            }
        }
        throw new TypeError();
    }
    // 7.1.2 ToBoolean(argument)
    // https://tc39.github.io/ecma262/2016/#sec-toboolean
    function ToBoolean(argument) {
        return !!argument;
    }
    // 7.1.12 ToString(argument)
    // https://tc39.github.io/ecma262/#sec-tostring
    function ToString(argument) {
        return "" + argument;
    }
    // 7.1.14 ToPropertyKey(argument)
    // https://tc39.github.io/ecma262/#sec-topropertykey
    function ToPropertyKey(argument) {
        var key = ToPrimitive(argument, 3 /* String */);
        if (IsSymbol(key))
            return key;
        return ToString(key);
    }
    // 7.2 Testing and Comparison Operations
    // https://tc39.github.io/ecma262/#sec-testing-and-comparison-operations
    // 7.2.2 IsArray(argument)
    // https://tc39.github.io/ecma262/#sec-isarray
    function IsArray(argument) {
        return Array.isArray
            ? Array.isArray(argument)
            : argument instanceof Object
                ? argument instanceof Array
                : Object.prototype.toString.call(argument) === "[object Array]";
    }
    // 7.2.3 IsCallable(argument)
    // https://tc39.github.io/ecma262/#sec-iscallable
    function IsCallable(argument) {
        // NOTE: This is an approximation as we cannot check for [[Call]] internal method.
        return typeof argument === "function";
    }
    // 7.2.4 IsConstructor(argument)
    // https://tc39.github.io/ecma262/#sec-isconstructor
    function IsConstructor(argument) {
        // NOTE: This is an approximation as we cannot check for [[Construct]] internal method.
        return typeof argument === "function";
    }
    // 7.2.7 IsPropertyKey(argument)
    // https://tc39.github.io/ecma262/#sec-ispropertykey
    function IsPropertyKey(argument) {
        switch (Type(argument)) {
            case 3 /* String */: return true;
            case 4 /* Symbol */: return true;
            default: return false;
        }
    }
    // 7.3 Operations on Objects
    // https://tc39.github.io/ecma262/#sec-operations-on-objects
    // 7.3.9 GetMethod(V, P)
    // https://tc39.github.io/ecma262/#sec-getmethod
    function GetMethod(V, P) {
        var func = V[P];
        if (func === undefined || func === null)
            return undefined;
        if (!IsCallable(func))
            throw new TypeError();
        return func;
    }
    // 7.4 Operations on Iterator Objects
    // https://tc39.github.io/ecma262/#sec-operations-on-iterator-objects
    function GetIterator(obj) {
        var method = GetMethod(obj, iteratorSymbol);
        if (!IsCallable(method))
            throw new TypeError(); // from Call
        var iterator = method.call(obj);
        if (!IsObject(iterator))
            throw new TypeError();
        return iterator;
    }
    // 7.4.4 IteratorValue(iterResult)
    // https://tc39.github.io/ecma262/2016/#sec-iteratorvalue
    function IteratorValue(iterResult) {
        return iterResult.value;
    }
    // 7.4.5 IteratorStep(iterator)
    // https://tc39.github.io/ecma262/#sec-iteratorstep
    function IteratorStep(iterator) {
        var result = iterator.next();
        return result.done ? false : result;
    }
    // 7.4.6 IteratorClose(iterator, completion)
    // https://tc39.github.io/ecma262/#sec-iteratorclose
    function IteratorClose(iterator) {
        var f = iterator["return"];
        if (f)
            f.call(iterator);
    }
    // 9.1 Ordinary Object Internal Methods and Internal Slots
    // https://tc39.github.io/ecma262/#sec-ordinary-object-internal-methods-and-internal-slots
    // 9.1.1.1 OrdinaryGetPrototypeOf(O)
    // https://tc39.github.io/ecma262/#sec-ordinarygetprototypeof
    function OrdinaryGetPrototypeOf(O) {
        var proto = Object.getPrototypeOf(O);
        if (typeof O !== "function" || O === functionPrototype)
            return proto;
        // TypeScript doesn't set __proto__ in ES5, as it's non-standard.
        // Try to determine the superclass constructor. Compatible implementations
        // must either set __proto__ on a subclass constructor to the superclass constructor,
        // or ensure each class has a valid `constructor` property on its prototype that
        // points back to the constructor.
        // If this is not the same as Function.[[Prototype]], then this is definately inherited.
        // This is the case when in ES6 or when using __proto__ in a compatible browser.
        if (proto !== functionPrototype)
            return proto;
        // If the super prototype is Object.prototype, null, or undefined, then we cannot determine the heritage.
        var prototype = O.prototype;
        var prototypeProto = prototype && Object.getPrototypeOf(prototype);
        if (prototypeProto == null || prototypeProto === Object.prototype)
            return proto;
        // If the constructor was not a function, then we cannot determine the heritage.
        var constructor = prototypeProto.constructor;
        if (typeof constructor !== "function")
            return proto;
        // If we have some kind of self-reference, then we cannot determine the heritage.
        if (constructor === O)
            return proto;
        // we have a pretty good guess at the heritage.
        return constructor;
    }
    // naive Map shim
    function CreateMapPolyfill() {
        var cacheSentinel = {};
        var arraySentinel = [];
        var MapIterator = (function () {
            function MapIterator(keys, values, selector) {
                this._index = 0;
                this._keys = keys;
                this._values = values;
                this._selector = selector;
            }
            MapIterator.prototype["@@iterator"] = function () { return this; };
            MapIterator.prototype[iteratorSymbol] = function () { return this; };
            MapIterator.prototype.next = function () {
                var index = this._index;
                if (index >= 0 && index < this._keys.length) {
                    var result = this._selector(this._keys[index], this._values[index]);
                    if (index + 1 >= this._keys.length) {
                        this._index = -1;
                        this._keys = arraySentinel;
                        this._values = arraySentinel;
                    }
                    else {
                        this._index++;
                    }
                    return { value: result, done: false };
                }
                return { value: undefined, done: true };
            };
            MapIterator.prototype.throw = function (error) {
                if (this._index >= 0) {
                    this._index = -1;
                    this._keys = arraySentinel;
                    this._values = arraySentinel;
                }
                throw error;
            };
            MapIterator.prototype.return = function (value) {
                if (this._index >= 0) {
                    this._index = -1;
                    this._keys = arraySentinel;
                    this._values = arraySentinel;
                }
                return { value: value, done: true };
            };
            return MapIterator;
        }());
        return (function () {
            function Map() {
                this._keys = [];
                this._values = [];
                this._cacheKey = cacheSentinel;
                this._cacheIndex = -2;
            }
            Object.defineProperty(Map.prototype, "size", {
                get: function () { return this._keys.length; },
                enumerable: true,
                configurable: true
            });
            Map.prototype.has = function (key) { return this._find(key, /*insert*/ false) >= 0; };
            Map.prototype.get = function (key) {
                var index = this._find(key, /*insert*/ false);
                return index >= 0 ? this._values[index] : undefined;
            };
            Map.prototype.set = function (key, value) {
                var index = this._find(key, /*insert*/ true);
                this._values[index] = value;
                return this;
            };
            Map.prototype.delete = function (key) {
                var index = this._find(key, /*insert*/ false);
                if (index >= 0) {
                    var size = this._keys.length;
                    for (var i = index + 1; i < size; i++) {
                        this._keys[i - 1] = this._keys[i];
                        this._values[i - 1] = this._values[i];
                    }
                    this._keys.length--;
                    this._values.length--;
                    if (key === this._cacheKey) {
                        this._cacheKey = cacheSentinel;
                        this._cacheIndex = -2;
                    }
                    return true;
                }
                return false;
            };
            Map.prototype.clear = function () {
                this._keys.length = 0;
                this._values.length = 0;
                this._cacheKey = cacheSentinel;
                this._cacheIndex = -2;
            };
            Map.prototype.keys = function () { return new MapIterator(this._keys, this._values, getKey); };
            Map.prototype.values = function () { return new MapIterator(this._keys, this._values, getValue); };
            Map.prototype.entries = function () { return new MapIterator(this._keys, this._values, getEntry); };
            Map.prototype["@@iterator"] = function () { return this.entries(); };
            Map.prototype[iteratorSymbol] = function () { return this.entries(); };
            Map.prototype._find = function (key, insert) {
                if (this._cacheKey !== key) {
                    this._cacheIndex = this._keys.indexOf(this._cacheKey = key);
                }
                if (this._cacheIndex < 0 && insert) {
                    this._cacheIndex = this._keys.length;
                    this._keys.push(key);
                    this._values.push(undefined);
                }
                return this._cacheIndex;
            };
            return Map;
        }());
        function getKey(key, _) {
            return key;
        }
        function getValue(_, value) {
            return value;
        }
        function getEntry(key, value) {
            return [key, value];
        }
    }
    // naive Set shim
    function CreateSetPolyfill() {
        return (function () {
            function Set() {
                this._map = new _Map();
            }
            Object.defineProperty(Set.prototype, "size", {
                get: function () { return this._map.size; },
                enumerable: true,
                configurable: true
            });
            Set.prototype.has = function (value) { return this._map.has(value); };
            Set.prototype.add = function (value) { return this._map.set(value, value), this; };
            Set.prototype.delete = function (value) { return this._map.delete(value); };
            Set.prototype.clear = function () { this._map.clear(); };
            Set.prototype.keys = function () { return this._map.keys(); };
            Set.prototype.values = function () { return this._map.values(); };
            Set.prototype.entries = function () { return this._map.entries(); };
            Set.prototype["@@iterator"] = function () { return this.keys(); };
            Set.prototype[iteratorSymbol] = function () { return this.keys(); };
            return Set;
        }());
    }
    // naive WeakMap shim
    function CreateWeakMapPolyfill() {
        var UUID_SIZE = 16;
        var keys = HashMap.create();
        var rootKey = CreateUniqueKey();
        return (function () {
            function WeakMap() {
                this._key = CreateUniqueKey();
            }
            WeakMap.prototype.has = function (target) {
                var table = GetOrCreateWeakMapTable(target, /*create*/ false);
                return table !== undefined ? HashMap.has(table, this._key) : false;
            };
            WeakMap.prototype.get = function (target) {
                var table = GetOrCreateWeakMapTable(target, /*create*/ false);
                return table !== undefined ? HashMap.get(table, this._key) : undefined;
            };
            WeakMap.prototype.set = function (target, value) {
                var table = GetOrCreateWeakMapTable(target, /*create*/ true);
                table[this._key] = value;
                return this;
            };
            WeakMap.prototype.delete = function (target) {
                var table = GetOrCreateWeakMapTable(target, /*create*/ false);
                return table !== undefined ? delete table[this._key] : false;
            };
            WeakMap.prototype.clear = function () {
                // NOTE: not a real clear, just makes the previous data unreachable
                this._key = CreateUniqueKey();
            };
            return WeakMap;
        }());
        function CreateUniqueKey() {
            var key;
            do
                key = "@@WeakMap@@" + CreateUUID();
            while (HashMap.has(keys, key));
            keys[key] = true;
            return key;
        }
        function GetOrCreateWeakMapTable(target, create) {
            if (!hasOwn.call(target, rootKey)) {
                if (!create)
                    return undefined;
                Object.defineProperty(target, rootKey, { value: HashMap.create() });
            }
            return target[rootKey];
        }
        function FillRandomBytes(buffer, size) {
            for (var i = 0; i < size; ++i)
                buffer[i] = Math.random() * 0xff | 0;
            return buffer;
        }
        function GenRandomBytes(size) {
            if (typeof Uint8Array === "function") {
                if (typeof crypto !== "undefined")
                    return crypto.getRandomValues(new Uint8Array(size));
                if (typeof msCrypto !== "undefined")
                    return msCrypto.getRandomValues(new Uint8Array(size));
                return FillRandomBytes(new Uint8Array(size), size);
            }
            return FillRandomBytes(new Array(size), size);
        }
        function CreateUUID() {
            var data = GenRandomBytes(UUID_SIZE);
            // mark as random - RFC 4122 § 4.4
            data[6] = data[6] & 0x4f | 0x40;
            data[8] = data[8] & 0xbf | 0x80;
            var result = "";
            for (var offset = 0; offset < UUID_SIZE; ++offset) {
                var byte = data[offset];
                if (offset === 4 || offset === 6 || offset === 8)
                    result += "-";
                if (byte < 16)
                    result += "0";
                result += byte.toString(16).toLowerCase();
            }
            return result;
        }
    }
    // uses a heuristic used by v8 and chakra to force an object into dictionary mode.
    function MakeDictionary(obj) {
        obj.__ = undefined;
        delete obj.__;
        return obj;
    }
    // patch global Reflect
    (function (__global) {
        if (typeof __global.Reflect !== "undefined") {
            if (__global.Reflect !== Reflect) {
                for (var p in Reflect) {
                    if (hasOwn.call(Reflect, p)) {
                        __global.Reflect[p] = Reflect[p];
                    }
                }
            }
        }
        else {
            __global.Reflect = Reflect;
        }
    })(typeof global !== "undefined" ? global :
        typeof self !== "undefined" ? self :
            Function("return this;")());
})(Reflect || (Reflect = {}));
//# sourceMappingURL=Reflect.js.map
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(45), __webpack_require__(49)))

/***/ }),
/* 9 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(40);

/***/ }),
/* 10 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(42);

/***/ }),
/* 11 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(47);

/***/ }),
/* 12 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModuleShared; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__(50);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__(46);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_http__ = __webpack_require__(6);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_router__ = __webpack_require__(47);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__components_app_app_component__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__components_navmenu_navmenu_component__ = __webpack_require__(19);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__components_version_version_component__ = __webpack_require__(25);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__components_home_home_component__ = __webpack_require__(17);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__components_containers_containers_component__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__components_blobs_blobs_component__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__components_queues_queues_component__ = __webpack_require__(21);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__components_qmessages_qmessages_component__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__components_tables_tables_component__ = __webpack_require__(24);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__components_tabledata_tabledata_component__ = __webpack_require__(23);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__components_login_login_component__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__components_errors_errors_component__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__components_base_base_component__ = __webpack_require__(3);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__components_shares_shares_component__ = __webpack_require__(22);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__components_files_files_component__ = __webpack_require__(16);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




















var AppModuleShared = (function () {
    function AppModuleShared() {
    }
    AppModuleShared = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_5__components_app_app_component__["a" /* AppComponent */],
                __WEBPACK_IMPORTED_MODULE_6__components_navmenu_navmenu_component__["a" /* NavMenuComponent */],
                __WEBPACK_IMPORTED_MODULE_7__components_version_version_component__["a" /* VersionComponent */],
                __WEBPACK_IMPORTED_MODULE_8__components_home_home_component__["a" /* HomeComponent */],
                __WEBPACK_IMPORTED_MODULE_9__components_containers_containers_component__["a" /* ContainersComponent */],
                __WEBPACK_IMPORTED_MODULE_10__components_blobs_blobs_component__["a" /* BlobsComponent */],
                __WEBPACK_IMPORTED_MODULE_11__components_queues_queues_component__["a" /* QueuesComponent */],
                __WEBPACK_IMPORTED_MODULE_12__components_qmessages_qmessages_component__["a" /* QmessagesComponent */],
                __WEBPACK_IMPORTED_MODULE_13__components_tables_tables_component__["a" /* TablesComponent */],
                __WEBPACK_IMPORTED_MODULE_14__components_tabledata_tabledata_component__["a" /* TabledataComponent */],
                __WEBPACK_IMPORTED_MODULE_15__components_login_login_component__["a" /* LoginComponent */],
                __WEBPACK_IMPORTED_MODULE_16__components_errors_errors_component__["a" /* MyErrorsHandler */],
                __WEBPACK_IMPORTED_MODULE_17__components_base_base_component__["a" /* BaseComponent */],
                __WEBPACK_IMPORTED_MODULE_18__components_shares_shares_component__["a" /* SharesComponent */],
                __WEBPACK_IMPORTED_MODULE_19__components_files_files_component__["a" /* FilesComponent */]
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_3__angular_http__["HttpModule"],
                __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormsModule"],
                __WEBPACK_IMPORTED_MODULE_4__angular_router__["RouterModule"].forRoot([
                    { path: '', redirectTo: 'home', pathMatch: 'full' },
                    { path: 'home', component: __WEBPACK_IMPORTED_MODULE_8__components_home_home_component__["a" /* HomeComponent */] },
                    { path: 'containers', component: __WEBPACK_IMPORTED_MODULE_9__components_containers_containers_component__["a" /* ContainersComponent */] },
                    { path: 'queues', component: __WEBPACK_IMPORTED_MODULE_11__components_queues_queues_component__["a" /* QueuesComponent */] },
                    { path: 'tables', component: __WEBPACK_IMPORTED_MODULE_13__components_tables_tables_component__["a" /* TablesComponent */] },
                    { path: 'files', component: __WEBPACK_IMPORTED_MODULE_18__components_shares_shares_component__["a" /* SharesComponent */] },
                    { path: '**', redirectTo: 'home' }
                ])
            ],
            providers: [
                {
                    provide: __WEBPACK_IMPORTED_MODULE_0__angular_core__["ErrorHandler"],
                    useClass: __WEBPACK_IMPORTED_MODULE_16__components_errors_errors_component__["a" /* MyErrorsHandler */]
                }
            ]
        })
    ], AppModuleShared);
    return AppModuleShared;
}());



/***/ }),
/* 13 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return BlobsComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__base_base_component__ = __webpack_require__(3);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var BlobsComponent = (function (_super) {
    __extends(BlobsComponent, _super);
    function BlobsComponent(utils) {
        var _this = _super.call(this, utils) || this;
        _this.forceReload = false;
        _this.refresh = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
        _this.container = "";
        _this.showTable = false;
        _this.removeContainerFlag = false;
        _this.getBlobs();
        return _this;
    }
    BlobsComponent.prototype.ngOnChanges = function () {
        this.getBlobs();
    };
    BlobsComponent.prototype.getBlobs = function () {
        var _this = this;
        if (!this.container) {
            this.showTable = false;
            return;
        }
        this.loading = true;
        this.showTable = false;
        this.utilsService.getData('api/Blobs/GetBlobs?container=' + this.container).subscribe(function (result) {
            _this.blobs = result.json();
            _this.loading = false;
            _this.showTable = true;
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    BlobsComponent.prototype.removeBlob = function (event) {
        var element = event.currentTarget; //button
        var blob = element.parentElement.parentElement.children[2].textContent;
        this.selected = blob;
    };
    BlobsComponent.prototype.deleteBlob = function () {
        var _this = this;
        this.utilsService.postData('api/Blobs/DeleteBlob?blobUri=' + encodeURIComponent(this.selected), null).subscribe(function (result) {
            _this.selected = '';
            _this.getBlobs();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    BlobsComponent.prototype.cancelDeleteBlob = function () {
        this.selected = '';
    };
    BlobsComponent.prototype.removeContainer = function (event) {
        this.removeContainerFlag = true;
    };
    BlobsComponent.prototype.cancelDeleteContainer = function () {
        this.removeContainerFlag = false;
    };
    BlobsComponent.prototype.deleteContainer = function () {
        var _this = this;
        this.utilsService.postData('api/Containers/DeleteContainer?container=' + this.container, null).subscribe(function (result) {
            _this.container = "";
            _this.removeContainerFlag = false;
            _this.refresh.emit(true);
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    BlobsComponent.prototype.upload = function () {
        var that = this;
        var fileBrowser = this.fileInput.nativeElement;
        if (fileBrowser.files && fileBrowser.files[0]) {
            var formData = new FormData();
            formData.append('files', fileBrowser.files[0]);
            this.utilsService.uploadFile('api/Blobs/UploadBlob?container=' + this.container, formData).onload = function () {
                that.getBlobs();
            };
        }
    };
    BlobsComponent.prototype.downloadBlob = function (event) {
        var _this = this;
        var element = event.currentTarget; //button
        var blob = element.parentElement.parentElement.children[2].textContent;
        this.utilsService.getFile('api/Blobs/GetBlob?blobUri=' + blob).subscribe(function (result) {
            var fileName = "NONAME";
            var contentDisposition = result.headers.get("content-disposition");
            contentDisposition.split(";").forEach(function (token) {
                token = token.trim();
                if (token.startsWith("filename="))
                    fileName = token.substr("filename=".length);
            });
            var byteArray = result.arrayBuffer();
            var blobFile = new Blob([byteArray], { type: "application/octet-stream;charset=utf-8", endings: "transparent" });
            var blobUrl = URL.createObjectURL(blobFile);
            var link = document.createElement('a');
            link.href = blobUrl;
            link.setAttribute('download', fileName);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Output"])(),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"])
    ], BlobsComponent.prototype, "refresh", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", String)
    ], BlobsComponent.prototype, "container", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('fileInput'),
        __metadata("design:type", Object)
    ], BlobsComponent.prototype, "fileInput", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('modal'),
        __metadata("design:type", Object)
    ], BlobsComponent.prototype, "modal", void 0);
    BlobsComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'blobs',
            template: __webpack_require__(30)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], BlobsComponent);
    return BlobsComponent;
}(__WEBPACK_IMPORTED_MODULE_2__base_base_component__["a" /* BaseComponent */]));



/***/ }),
/* 14 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ContainersComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__base_base_component__ = __webpack_require__(3);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var ContainersComponent = (function (_super) {
    __extends(ContainersComponent, _super);
    function ContainersComponent(utilsService) {
        var _this = _super.call(this, utilsService) || this;
        _this.getContainers();
        return _this;
    }
    ContainersComponent.prototype.ngOnChanges = function () {
        this.getContainers();
    };
    ContainersComponent.prototype.getContainers = function () {
        var _this = this;
        this.loading = true;
        this.utilsService.getData('api/Containers/GetContainers').subscribe(function (result) {
            _this.loading = false;
            _this.containers = result.json();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    ContainersComponent.prototype.containerChanged = function (event) {
        var element = event.currentTarget;
        var container = element.textContent.trim();
        var nodes = this.containersMenu.nativeElement.childNodes;
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].classList)
                nodes[i].classList.remove("active");
        }
        element.classList.add("active");
        this.selectedContainer = container;
    };
    ContainersComponent.prototype.newContainer = function (event) {
        var _this = this;
        var url = 'api/Containers/NewContainer?container=' + this.newContainerName.nativeElement.value + '&publicAccess=' + this.publicAccess.nativeElement.checked;
        this.utilsService.postData(url, null).subscribe(function (result) {
            _this.newContainerName.nativeElement.value = "";
            _this.publicAccess.nativeElement.checked = false;
            _this.getContainers();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    ContainersComponent.prototype.forceRefresh = function (force) {
        if (force) {
            this.getContainers();
            this.selectedContainer = '';
        }
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('newContainerName'),
        __metadata("design:type", Object)
    ], ContainersComponent.prototype, "newContainerName", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('publicAccess'),
        __metadata("design:type", Object)
    ], ContainersComponent.prototype, "publicAccess", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('containersMenu'),
        __metadata("design:type", Object)
    ], ContainersComponent.prototype, "containersMenu", void 0);
    ContainersComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'containers',
            template: __webpack_require__(31)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], ContainersComponent);
    return ContainersComponent;
}(__WEBPACK_IMPORTED_MODULE_2__base_base_component__["a" /* BaseComponent */]));



/***/ }),
/* 15 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyErrorsHandler; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var MyErrorsHandler = (function () {
    function MyErrorsHandler() {
        this.showErrors = false;
    }
    MyErrorsHandler.prototype.handleError = function (error) {
        this.showErrors = true;
        console.error(error);
        if (error.statusText)
            alert(error.statusText);
        else if (error.message)
            alert(error.message);
        else
            alert(error);
    };
    MyErrorsHandler = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'errors',
            //template: ''
            template: __webpack_require__(32)
        }),
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])()
    ], MyErrorsHandler);
    return MyErrorsHandler;
}());



/***/ }),
/* 16 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FilesComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__base_base_component__ = __webpack_require__(3);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var FilesComponent = (function (_super) {
    __extends(FilesComponent, _super);
    function FilesComponent(utils) {
        var _this = _super.call(this, utils) || this;
        //forceReload: boolean;
        _this.refresh = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
        _this.share = "";
        _this.showTable = false;
        _this.folder = "";
        _this.removeContainerFlag = false;
        _this.getFiles();
        return _this;
    }
    FilesComponent.prototype.ngOnChanges = function () {
        this.getFiles();
    };
    FilesComponent.prototype.setFolder = function (event) {
        var element = event.currentTarget;
        var folder = element.parentElement.children[1].textContent.trim();
        this.internalSetFolder(folder);
    };
    FilesComponent.prototype.setCurrentFolder = function (event) {
        var element = event.currentTarget;
        var folder = element.textContent.trim();
        this.internalSetFolder(folder);
    };
    FilesComponent.prototype.internalSetFolder = function (folder) {
        if (this.folder)
            this.folder = this.folder + "\\" + folder;
        else
            this.folder = folder;
        this.getFiles();
    };
    FilesComponent.prototype.levelUp = function () {
        var slash = this.folder.lastIndexOf("\\");
        if (slash > 0)
            this.folder = this.folder.substr(0, slash);
        else
            this.folder = '';
        this.getFiles();
    };
    FilesComponent.prototype.getFiles = function () {
        var _this = this;
        if (!this.share) {
            this.showTable = false;
            return;
        }
        this.loading = true;
        this.showTable = false;
        this.utilsService.getData('api/Files/GetFilesAndDirectories?share=' + this.share + '&folder=' + this.folder).subscribe(function (result) {
            _this.files = result.json();
            _this.loading = false;
            _this.showTable = true;
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Output"])(),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"])
    ], FilesComponent.prototype, "refresh", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", String)
    ], FilesComponent.prototype, "share", void 0);
    FilesComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'files',
            template: __webpack_require__(33)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], FilesComponent);
    return FilesComponent;
}(__WEBPACK_IMPORTED_MODULE_2__base_base_component__["a" /* BaseComponent */]));



/***/ }),
/* 17 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HomeComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var HomeComponent = (function () {
    function HomeComponent() {
    }
    HomeComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'home',
            template: __webpack_require__(34)
        })
    ], HomeComponent);
    return HomeComponent;
}());



/***/ }),
/* 18 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LoginComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var LoginComponent = (function () {
    function LoginComponent(utils) {
        //https://yakovfain.com/2016/10/31/angular-2-component-communication-with-events-vs-callbacks/
        this.signedIn = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
        this.loading = false;
        this.showError = false;
        this.utilsService = utils;
    }
    LoginComponent.prototype.signIn = function () {
        var _this = this;
        this.loading = true;
        this.showError = false;
        var account = encodeURIComponent(this.azureAccount.nativeElement.value);
        var key = encodeURIComponent(this.azureKey.nativeElement.value);
        this.utilsService.signIn(account, key).subscribe(function (result) {
            localStorage.setItem('account', account);
            localStorage.setItem('key', key);
            _this.loading = false;
            _this.signedIn.emit(true);
        }, function (error) {
            localStorage.clear();
            _this.loading = false;
            _this.signedIn.emit(false);
            _this.showError = true;
            console.error(error);
        });
    };
    LoginComponent.prototype.typingMessage = function (event) {
        if (this.showError)
            this.showError = false;
        if (event.key == 'Enter')
            this.signIn();
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('azureAccount'),
        __metadata("design:type", Object)
    ], LoginComponent.prototype, "azureAccount", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('azureKey'),
        __metadata("design:type", Object)
    ], LoginComponent.prototype, "azureKey", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Output"])(),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"])
    ], LoginComponent.prototype, "signedIn", void 0);
    LoginComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'login',
            template: __webpack_require__(35)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], LoginComponent);
    return LoginComponent;
}());



/***/ }),
/* 19 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return NavMenuComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var NavMenuComponent = (function () {
    function NavMenuComponent(utilsService) {
        this.utilsService = utilsService;
        //https://yakovfain.com/2016/10/31/angular-2-component-communication-with-events-vs-callbacks/
        this.signedIn = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
    }
    NavMenuComponent.prototype.logOut = function (event) {
        this.utilsService.logOut();
        this.signedIn.emit(false);
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Output"])(),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"])
    ], NavMenuComponent.prototype, "signedIn", void 0);
    NavMenuComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'nav-menu',
            template: __webpack_require__(36),
            styles: [__webpack_require__(44)]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], NavMenuComponent);
    return NavMenuComponent;
}());



/***/ }),
/* 20 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return QmessagesComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__base_base_component__ = __webpack_require__(3);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var QmessagesComponent = (function (_super) {
    __extends(QmessagesComponent, _super);
    function QmessagesComponent(utils) {
        var _this = _super.call(this, utils) || this;
        _this.showTable = false;
        _this.removeQueueFlag = false;
        _this.refresh = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
        _this.queue = "";
        _this.getMessages();
        return _this;
    }
    QmessagesComponent.prototype.ngOnChanges = function () {
        this.getMessages();
    };
    QmessagesComponent.prototype.getMessages = function () {
        var _this = this;
        if (!this.queue) {
            this.showTable = false;
            return;
        }
        this.showTable = false;
        this.loading = true;
        this.utilsService.getData('api/Queues/GetMessages?queue=' + this.queue).subscribe(function (result) {
            _this.loading = false;
            _this.messages = result.json();
            _this.showTable = true;
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    QmessagesComponent.prototype.addMessage = function () {
        var _this = this;
        this.utilsService.postData('api/Queues/NewQueueMessage?queue=' + encodeURIComponent(this.queue) + '&message=' + encodeURIComponent(this.newMessage.nativeElement.value), null).subscribe(function (result) {
            _this.getMessages();
            _this.newMessage.nativeElement.value = '';
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    QmessagesComponent.prototype.removeMessage = function (event) {
        var element = event.currentTarget; //button
        var messageId = element.parentElement.parentElement.children[2].textContent;
        this.selected = messageId;
    };
    QmessagesComponent.prototype.deleteMessage = function () {
        var _this = this;
        this.utilsService.postData('api/Queues/DeleteMessage?queue=' + encodeURIComponent(this.queue) + '&messageId=' + encodeURIComponent(this.selected), null).subscribe(function (result) {
            _this.selected = '';
            _this.getMessages();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    QmessagesComponent.prototype.cancelDeleteMessage = function () {
        this.selected = '';
    };
    QmessagesComponent.prototype.removeQueue = function (event) {
        this.removeQueueFlag = true;
    };
    QmessagesComponent.prototype.cancelDeleteQueue = function () {
        this.removeQueueFlag = false;
    };
    QmessagesComponent.prototype.deleteQueue = function () {
        var _this = this;
        this.utilsService.postData('api/Queues/DeleteQueue?queue=' + this.queue, null).subscribe(function (result) {
            _this.queue = "";
            _this.removeQueueFlag = false;
            _this.refresh.emit(true);
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    QmessagesComponent.prototype.typingMessage = function (event) {
        if (event.key == 'Enter')
            this.addMessage();
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Output"])(),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"])
    ], QmessagesComponent.prototype, "refresh", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", String)
    ], QmessagesComponent.prototype, "queue", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('newMessage'),
        __metadata("design:type", Object)
    ], QmessagesComponent.prototype, "newMessage", void 0);
    QmessagesComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'qmessages',
            template: __webpack_require__(37)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], QmessagesComponent);
    return QmessagesComponent;
}(__WEBPACK_IMPORTED_MODULE_2__base_base_component__["a" /* BaseComponent */]));



/***/ }),
/* 21 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return QueuesComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__base_base_component__ = __webpack_require__(3);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var QueuesComponent = (function (_super) {
    __extends(QueuesComponent, _super);
    function QueuesComponent(utils) {
        var _this = _super.call(this, utils) || this;
        _this.selectedQueue = '';
        _this.getQueues();
        return _this;
    }
    QueuesComponent.prototype.ngOnChanges = function () {
        this.getQueues();
    };
    QueuesComponent.prototype.getQueues = function () {
        var _this = this;
        this.loading = true;
        this.utilsService.getData('api/Queues/GetQueues').subscribe(function (result) {
            _this.loading = false;
            _this.queues = result.json();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    QueuesComponent.prototype.queueChanged = function (event) {
        var element = event.currentTarget;
        var queue = element.textContent.trim();
        var nodes = this.queuesMenu.nativeElement.childNodes;
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].classList)
                nodes[i].classList.remove("active");
        }
        element.classList.add("active");
        this.selectedQueue = queue;
    };
    QueuesComponent.prototype.newQueue = function (event) {
        var _this = this;
        this.utilsService.postData('api/Queues/NewQueue?queue=' + this.newQueueName.nativeElement.value, null).subscribe(function (result) {
            _this.newQueueName.nativeElement.value = "";
            _this.getQueues();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    QueuesComponent.prototype.forceRefresh = function (force) {
        if (force) {
            this.getQueues();
            this.selectedQueue = '';
        }
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('newQueueName'),
        __metadata("design:type", Object)
    ], QueuesComponent.prototype, "newQueueName", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('queuesMenu'),
        __metadata("design:type", Object)
    ], QueuesComponent.prototype, "queuesMenu", void 0);
    QueuesComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'queues',
            template: __webpack_require__(38)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], QueuesComponent);
    return QueuesComponent;
}(__WEBPACK_IMPORTED_MODULE_2__base_base_component__["a" /* BaseComponent */]));



/***/ }),
/* 22 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SharesComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__base_base_component__ = __webpack_require__(3);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var SharesComponent = (function (_super) {
    __extends(SharesComponent, _super);
    function SharesComponent(utilsService) {
        var _this = _super.call(this, utilsService) || this;
        _this.selectedShare = '';
        _this.getShares();
        return _this;
    }
    SharesComponent.prototype.ngOnChanges = function () {
        this.getShares();
    };
    SharesComponent.prototype.getShares = function () {
        var _this = this;
        this.loading = true;
        this.utilsService.getData('api/Files/GetShares').subscribe(function (result) {
            _this.loading = false;
            _this.shares = result.json();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    SharesComponent.prototype.sharedChanged = function (event) {
        var element = event.currentTarget;
        var share = element.textContent.trim();
        var nodes = this.containersMenu.nativeElement.childNodes;
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].classList)
                nodes[i].classList.remove("active");
        }
        element.classList.add("active");
        this.selectedShare = share;
    };
    //newShare(event: Event) {
    //	let url = 'api/Files/NewShare?share=' + this.newShareName.nativeElement.value
    //	this.utilsService.postData(url, null).subscribe(result => {
    //		this.newShareName.nativeElement.value = "";
    //		this.getShares();
    //	}, error => { this.setErrorMessage(error.statusText); });
    //}
    SharesComponent.prototype.forceRefresh = function (force) {
        if (force) {
            this.getShares();
            this.selectedShare = '';
        }
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('newShareName'),
        __metadata("design:type", Object)
    ], SharesComponent.prototype, "newShareName", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('containersMenu'),
        __metadata("design:type", Object)
    ], SharesComponent.prototype, "containersMenu", void 0);
    SharesComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'shares',
            template: __webpack_require__(39)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], SharesComponent);
    return SharesComponent;
}(__WEBPACK_IMPORTED_MODULE_2__base_base_component__["a" /* BaseComponent */]));



/***/ }),
/* 23 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TabledataComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__base_base_component__ = __webpack_require__(3);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var TabledataComponent = (function (_super) {
    __extends(TabledataComponent, _super);
    function TabledataComponent(utils) {
        var _this = _super.call(this, utils) || this;
        _this.storageTable = "";
        _this.refresh = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
        _this.showTable = false;
        _this.removeTableFlag = false;
        _this.headers = [];
        _this.rows = [];
        return _this;
    }
    TabledataComponent.prototype.ngOnChanges = function () {
        this.data = null;
        this.showTable = false;
    };
    TabledataComponent.prototype.getData = function () {
        var _this = this;
        if (!this.storageTable)
            return;
        this.showTable = false;
        this.data = null;
        this.loading = true;
        this.utilsService.getData('api/Tables/QueryTable?table=' + this.storageTable + '&query=' + this.inputQuery.nativeElement.value).subscribe(function (result) {
            _this.data = result.json();
            _this.processData();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    TabledataComponent.prototype.insertData = function () {
        var _this = this;
        if (!this.storageTable)
            return;
        this.showTable = false;
        this.loading = true;
        this.utilsService.putData('api/Tables/InsertData?table=' + this.storageTable + '&data=' + this.inputQuery.nativeElement.value, null).subscribe(function (result) {
            _this.inputQuery.nativeElement.value = '';
            _this.loading = false;
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    TabledataComponent.prototype.processData = function () {
        //https://stackoverflow.com/questions/1232040/how-do-i-empty-an-array-in-javascript
        this.headers.length = 0;
        this.rows.length = 0;
        this.headers.push("Partition Key");
        this.headers.push("Row Key");
        for (var _i = 0, _a = this.data; _i < _a.length; _i++) {
            var entry = _a[_i];
            var values = entry.values.split(";");
            var obj = {};
            obj["Partition Key"] = entry.partitionKey;
            obj["Row Key"] = entry.rowKey;
            var _loop_1 = function (v) {
                if (!v)
                    return "continue";
                var pair = v.split("=");
                if (!this_1.headers.find(function (h) { return h === pair[0]; }))
                    this_1.headers.push(pair[0]);
                obj[pair[0]] = pair[1];
            };
            var this_1 = this;
            for (var _b = 0, values_1 = values; _b < values_1.length; _b++) {
                var v = values_1[_b];
                _loop_1(v);
            }
            this.rows.push(obj);
        }
        this.loading = false;
        this.showTable = true;
    };
    TabledataComponent.prototype.queryData = function () {
        if (!this.storageTable) {
            var m = this.mode.nativeElement.value === 'q' ? "query" : "insert";
            this.setErrorMessage('You must first select a table to ' + m + ' to');
            return;
        }
        if (this.mode.nativeElement.value === 'q')
            this.getData();
        else
            this.insertData();
    };
    TabledataComponent.prototype.modeChanged = function () {
        if (this.mode.nativeElement.value === 'q')
            this.inputQuery.nativeElement.placeholder = "Search pattern...";
        else
            this.inputQuery.nativeElement.placeholder = "Insert statement...";
    };
    TabledataComponent.prototype.removeRow = function (event) {
        var _this = this;
        var element = event.currentTarget; //button
        var partitionKey = element.parentElement.parentElement.children[1].textContent.trim();
        var rowKey = element.parentElement.parentElement.children[2].textContent.trim();
        this.utilsService.deleteData('api/Tables/DeleteData?table=' + this.storageTable + '&partitionKey=' + partitionKey + '&rowKey=' + rowKey).subscribe(function (result) {
            _this.getData();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    TabledataComponent.prototype.removeTable = function () {
        this.removeTableFlag = true;
    };
    TabledataComponent.prototype.cancelDeleteTable = function () {
        this.removeTableFlag = false;
    };
    TabledataComponent.prototype.deleteTable = function () {
        var _this = this;
        this.utilsService.deleteData('api/Tables/DeleteTable?table=' + this.storageTable).subscribe(function (result) {
            _this.refresh.emit(true);
            _this.removeTableFlag = false;
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", String)
    ], TabledataComponent.prototype, "storageTable", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('inputQuery'),
        __metadata("design:type", Object)
    ], TabledataComponent.prototype, "inputQuery", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('mode'),
        __metadata("design:type", Object)
    ], TabledataComponent.prototype, "mode", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Output"])(),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"])
    ], TabledataComponent.prototype, "refresh", void 0);
    TabledataComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'tabledata',
            template: __webpack_require__(40)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], TabledataComponent);
    return TabledataComponent;
}(__WEBPACK_IMPORTED_MODULE_2__base_base_component__["a" /* BaseComponent */]));



/***/ }),
/* 24 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TablesComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__base_base_component__ = __webpack_require__(3);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var TablesComponent = (function (_super) {
    __extends(TablesComponent, _super);
    function TablesComponent(utils) {
        var _this = _super.call(this, utils) || this;
        _this.getTables();
        return _this;
    }
    TablesComponent.prototype.ngOnChanges = function () {
        this.getTables();
    };
    TablesComponent.prototype.getTables = function () {
        var _this = this;
        this.loading = true;
        this.utilsService.getData('api/Tables/GetTables').subscribe(function (result) {
            _this.loading = false;
            _this.tables = result.json();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    TablesComponent.prototype.tableChanged = function (event) {
        var element = event.currentTarget;
        var table = element.textContent.trim();
        var nodes = this.tablesMenu.nativeElement.childNodes;
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].classList)
                nodes[i].classList.remove("active");
        }
        element.classList.add("active");
        this.selectedTable = table;
    };
    TablesComponent.prototype.newTable = function (event) {
        var _this = this;
        this.utilsService.postData('api/Tables/NewTable?table=' + this.newTableName.nativeElement.value, null).subscribe(function (result) {
            _this.newTableName.nativeElement.value = "";
            _this.getTables();
        }, function (error) { _this.setErrorMessage(error.statusText); });
    };
    TablesComponent.prototype.forceRefresh = function (force) {
        if (force) {
            this.getTables();
            this.selectedTable = '';
        }
    };
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('newTableName'),
        __metadata("design:type", Object)
    ], TablesComponent.prototype, "newTableName", void 0);
    __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('tablesMenu'),
        __metadata("design:type", Object)
    ], TablesComponent.prototype, "tablesMenu", void 0);
    TablesComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'tables',
            template: __webpack_require__(41)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], TablesComponent);
    return TablesComponent;
}(__WEBPACK_IMPORTED_MODULE_2__base_base_component__["a" /* BaseComponent */]));



/***/ }),
/* 25 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return VersionComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var VersionComponent = (function () {
    function VersionComponent(utilsService) {
        this.utilsService = utilsService;
        this.getversion();
    }
    VersionComponent.prototype.ngOnChanges = function () {
        this.getversion();
    };
    VersionComponent.prototype.getversion = function () {
        var _this = this;
        this.utilsService.getData('api/Util/GetVersion').subscribe(function (result) {
            _this.currentVersion = result.text();
        }, function (error) { console.error(error); });
    };
    VersionComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'version',
            template: __webpack_require__(42)
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__services_utils_utils_service__["a" /* UtilsService */]])
    ], VersionComponent);
    return VersionComponent;
}());



/***/ }),
/* 26 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_reflect_metadata__ = __webpack_require__(8);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_reflect_metadata___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_reflect_metadata__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_zone_js__ = __webpack_require__(11);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_zone_js___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_zone_js__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_bootstrap__ = __webpack_require__(10);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_bootstrap___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_bootstrap__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_platform_browser_dynamic__ = __webpack_require__(9);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__app_app_module_browser__ = __webpack_require__(7);






if (false) {
    module.hot.accept();
    module.hot.dispose(function () {
        // Before restarting the app, we create a new root element and dispose the old one
        var oldRootElem = document.querySelector('app');
        var newRootElem = document.createElement('app');
        oldRootElem.parentNode.insertBefore(newRootElem, oldRootElem);
        modulePromise.then(function (appModule) { return appModule.destroy(); });
    });
}
else {
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__angular_core__["enableProdMode"])();
}
// Note: @ng-tools/webpack looks for the following expression when performing production
// builds. Don't change how this line looks, otherwise you may break tree-shaking.
var modulePromise = __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_4__angular_platform_browser_dynamic__["platformBrowserDynamic"])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_5__app_app_module_browser__["a" /* AppModule */]);


/***/ }),
/* 27 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(undefined);
// imports


// module
exports.push([module.i, "@media (max-width: 767px) {\r\n    /* On small screens, the nav menu spans the full width of the screen. Leave a space for it. */\r\n    .body-content {\r\n        padding-top: 50px;\r\n    }\r\n}\r\n\r\n", ""]);

// exports


/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(undefined);
// imports


// module
exports.push([module.i, "li .glyphicon {\r\n    margin-right: 10px;\r\n}\r\n\r\n/* Highlighting rules for nav menu items */\r\nli.link-active a,\r\nli.link-active a:hover,\r\nli.link-active a:focus {\r\n    background-color: #4189C7;\r\n    color: white;\r\n}\r\n\r\n/* Keep the nav menu independent of scrolling and on top of other items */\r\n.main-nav {\r\n    position: fixed;\r\n    top: 0;\r\n    left: 0;\r\n    right: 0;\r\n    z-index: 1;\r\n}\r\n\r\n@media (min-width: 768px) {\r\n    /* On small screens, convert the nav menu to a vertical sidebar */\r\n    .main-nav {\r\n        height: 100%;\r\n        width: calc(25% - 20px);\r\n    }\r\n    .navbar {\r\n        border-radius: 0px;\r\n        border-width: 0px;\r\n        height: 100%;\r\n    }\r\n    .navbar-header {\r\n        float: none;\r\n    }\r\n    .navbar-collapse {\r\n        border-top: 1px solid #444;\r\n        padding: 0px;\r\n    }\r\n    .navbar ul {\r\n        float: none;\r\n    }\r\n    .navbar li {\r\n        float: none;\r\n        font-size: 15px;\r\n        margin: 6px;\r\n    }\r\n    .navbar li a {\r\n        padding: 10px 16px;\r\n        border-radius: 4px;\r\n    }\r\n    .navbar a {\r\n        /* If a menu item's text is too long, truncate it */\r\n        width: 100%;\r\n        white-space: nowrap;\r\n        overflow: hidden;\r\n        text-overflow: ellipsis;\r\n    }\r\n}\r\n", ""]);

// exports


/***/ }),
/* 29 */
/***/ (function(module, exports) {

module.exports = "\r\n<div *ngIf=\"loggedIn\" class='fullHeight container-fluid' >\r\n\t<div class=\"fullHeight col\">\r\n\t\t<div class=''>\r\n\t\t\t<nav-menu></nav-menu>\r\n\t\t</div>\r\n\t\t<div class='body-content'>\r\n\t\t\t<router-outlet></router-outlet>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n\r\n<div *ngIf=\"!loggedIn\">\r\n\t<login (signedIn)=\"loggedInHandler($event)\"></login>\r\n</div>";

/***/ }),
/* 30 */
/***/ (function(module, exports) {

module.exports = "\r\n<div *ngIf=\"loading\" class=\"ui active text centered inline loader\">\r\n\t<em>Fetching data from {{ container }}...</em>\r\n</div>\r\n\r\n<table class=\"ui blue table\" *ngIf=\"showTable\">\r\n\t<thead>\r\n\t\t<tr>\r\n\t\t\t<th>\r\n\t\t\t\t<button *ngIf=\"!removeContainerFlag\" class=\"ui icon button\" (click)=\"removeContainer($event)\">\r\n\t\t\t\t\t<i class=\"trash icon\"></i>\r\n\t\t\t\t</button>\r\n\t\t\t\t<div *ngIf=\"removeContainerFlag\" class=\"ui buttons\">\r\n\t\t\t\t\t<button class=\"ui button\" (click)=\"cancelDeleteContainer()\">Cancel</button>\r\n\t\t\t\t\t<div class=\"or\"></div>\r\n\t\t\t\t\t<button class=\"ui negative button\" (click)=\"deleteContainer()\">Delete</button>\r\n\t\t\t\t</div>\r\n\t\t\t</th>\r\n\t\t\t<th></th>\r\n\t\t\t<th></th>\r\n\t\t\t<th>\r\n\t\t\t\t<div style=\"display:flex\">\r\n\t\t\t\t\t<input type=\"file\" #fileInput placeholder=\"Upload file...\" />\r\n\t\t\t\t\t<button type=\"button\" (click)=\"upload()\">Upload</button>\r\n\t\t\t\t</div>\r\n\t\t\t</th>\r\n\t</thead>\r\n\t<tbody>\r\n\t\t<tr *ngFor=\"let blob of blobs\">\r\n\t\t\t<td class=\"collapsing\">\r\n\t\t\t\t<button *ngIf=\"selected != blob\" class=\"ui icon button\" (click)=\"removeBlob($event)\">\r\n\t\t\t\t\t<i class=\"trash icon\"></i>\r\n\t\t\t\t</button>\r\n\t\t\t\t<div *ngIf=\"selected == blob\" class=\"ui buttons\">\r\n\t\t\t\t\t<button class=\"ui button\" (click)=\"cancelDeleteBlob()\">Cancel</button>\r\n\t\t\t\t\t<div class=\"or\"></div>\r\n\t\t\t\t\t<button class=\"ui negative button\" (click)=\"deleteBlob()\">Delete</button>\r\n\t\t\t\t</div>\r\n\t\t\t</td>\r\n\t\t\t<td class=\"collapsing\">\r\n\t\t\t\t<button class=\"ui icon button\" (click)=\"downloadBlob($event)\">\r\n\t\t\t\t\t<i class=\"download icon\"></i>\r\n\t\t\t\t</button>\r\n\t\t\t</td>\r\n\t\t\t<td colspan=\"2\">{{ blob }}</td>\r\n\t\t</tr>\r\n\t</tbody>\r\n\t<tfoot>\r\n\t\t<tr>\r\n\t\t\t<th colspan=\"2\">{{ blobs.length }} blobs found</th>\r\n\t\t\t<th></th>\r\n\t\t\t<th></th>\r\n\t\t</tr>\r\n\t</tfoot>\r\n</table>\r\n\r\n<div *ngIf=\"hasErrors\" class=\"ui negative message errorMessage\">\r\n\t<div class=\"header\">{{ errorMessage }}</div>\r\n</div>";

/***/ }),
/* 31 */
/***/ (function(module, exports) {

module.exports = "\r\n\r\n<p></p>\r\n\r\n<div class=\"ui action input\">\r\n\t<input #newContainerName type=\"text\" placeholder=\"New container...\">\r\n\t<button class=\"ui icon button\" (click)=\"newContainer($event)\">\r\n\t\t<i class=\"add icon\"></i>\r\n\t</button>\r\n</div>\r\n<div class=\"ui checkbox\">\r\n\t<input #publicAccess style=\"margin:0;\" name=\"containerAccess\" type=\"checkbox\" placeholder=\"Public Access?\" />\r\n\t<label>Set as public</label>\r\n</div>\r\n\r\n<p></p>\r\n\r\n<em *ngIf=\"loading\">Loading containers...</em>\r\n\r\n<div *ngIf=\"containers\" class=\"parent\">\r\n\r\n\t<div #containersMenu class=\"child ui vertical pointing menu\">\r\n\r\n\t\t<a class=\"link item elipsed\" *ngFor=\"let container of containers\" (click)=\"containerChanged($event)\">{{container}}</a>\r\n\t</div>\r\n\r\n\t<blobs class=\"child\" style=\"padding-left:20px\" [container]=\"selectedContainer\" (refresh)=\"forceRefresh($event)\"></blobs>\r\n\r\n</div>\r\n\r\n<div *ngIf=\"hasErrors\" class=\"ui negative message errorMessage\">\r\n\t<div class=\"header\">{{ errorMessage }}</div>\r\n</div>";

/***/ }),
/* 32 */
/***/ (function(module, exports) {

module.exports = "<errors></errors>";

/***/ }),
/* 33 */
/***/ (function(module, exports) {

module.exports = "\r\n<div *ngIf=\"loading\" class=\"ui active text centered inline loader\">\r\n\t<em>Fetching data from {{ share }}...</em>\r\n</div>\r\n\r\n<table class=\"ui blue table\" *ngIf=\"showTable\">\r\n\t<thead>\r\n\t\t<tr>\r\n\t\t\t<th colspan=\"2\">\r\n\t\t\t\t<button *ngIf=\"folder\" class=\"ui icon button\" (click)=\"levelUp()\">\r\n\t\t\t\t\t<i class=\"glyphicon glyphicon-level-up\"></i>\r\n\t\t\t\t</button>\r\n\t\t\t\t<span>\\{{ folder }}</span>\r\n\t\t\t</th>\r\n\t\t</tr>\r\n\t</thead>\r\n\t<tbody>\r\n\t\t<tr *ngFor=\"let file of files\">\r\n\t\t\t<td *ngIf=\"file.isDirectory\" (click)=\"setFolder($event)\" class=\"pointer\">\r\n\t\t\t\t<span class='glyphicon glyphicon-folder-close'></span>\r\n\t\t\t</td>\r\n\t\t\t<td *ngIf=\"!file.isDirectory\">\r\n\t\t\t\t<span class='glyphicon glyphicon-file'></span>\r\n\t\t\t</td>\r\n\t\t\t<td>\r\n\t\t\t\t<div *ngIf=\"file.isDirectory\" (click)=\"setCurrentFolder($event)\" class=\"pointer\">{{ file.name }}</div>\r\n\t\t\t\t<div *ngIf=\"!file.isDirectory\">{{ file.name }}</div>\r\n\t\t\t</td>\r\n\t\t</tr>\r\n\t</tbody>\r\n\t<tfoot>\r\n\t\t<tr>\r\n\t\t\t<th colspan=\"2\">{{ files.length }} files found</th>\r\n\t\t</tr>\r\n\t</tfoot>\r\n</table>\r\n\r\n<div *ngIf=\"hasErrors\" class=\"ui negative message errorMessage\">\r\n\t<div class=\"header\">{{ errorMessage }}</div>\r\n</div>";

/***/ }),
/* 34 */
/***/ (function(module, exports) {

module.exports = "<a href=\"https://github.com/sebagomez/azurestorageexplorer\">\r\n\t<img style=\"position: absolute; top: 0; right: 0; border: 0;\" src=\"https://camo.githubusercontent.com/a6677b08c955af8400f44c6298f40e7d19cc5b2d/68747470733a2f2f73332e616d617a6f6e6177732e636f6d2f6769746875622f726962626f6e732f666f726b6d655f72696768745f677261795f3664366436642e706e67\" alt=\"Fork me on GitHub\" data-canonical-src=\"https://s3.amazonaws.com/github/ribbons/forkme_right_gray_6d6d6d.png\">\r\n</a>\r\n\r\n<h1 class=\"azure\">Azure Storage web explorer</h1>\r\n<p>Welcome to the revamped version of Azure Storage web explorer</p>\r\n<p>This stared as a side project (stil is) but it got attention from Microsoft and got features in their documentation for Azure storage management tools. So in 2018 this tool was revamped to .NET Core 2.1 and Angular, out of the good old WebForms</p>\r\n<p>With this site you can manage</p>\r\n<ul>\r\n\t<li><strong>Blobs</strong>: Create public or private Containers and Blobs (only BlockBlobs for now). Download or delete your blobs.</li>\r\n\t<li><strong>Queues</strong>: Create Queues and messages.</li>\r\n\t<li><strong>Tables</strong>: Create table and Entities. To create an Entity you'll have to add one property per line in the form of <code>&lt;PropertyName&gt;=&lt;PropertyValue&gt;</code>. In order to query data you provide a query in the form of <code>&lt;PropertyName&gt; &lt;operator&gt; &lt;PropertyValue&gt;</code>, being <code>&lt;operator&gt;</code> one of the <a href=\"https://docs.microsoft.com/en-us/rest/api/storageservices/querying-tables-and-entities#supported-comparison-operators\">supported comparison operators</a></li>\r\n\t<li><strong>Files Shares</strong>: Navigate thru your file shares and directories. More features coming soon.</li>\r\n</ul>\r\n<version class=\"footer\"></version>\r\n";

/***/ }),
/* 35 */
/***/ (function(module, exports) {

module.exports = "<a href=\"https://github.com/sebagomez/azurestorageexplorer\">\r\n\t<img style=\"position: absolute; top: 0; right: 0; border: 0;\" src=\"https://camo.githubusercontent.com/a6677b08c955af8400f44c6298f40e7d19cc5b2d/68747470733a2f2f73332e616d617a6f6e6177732e636f6d2f6769746875622f726962626f6e732f666f726b6d655f72696768745f677261795f3664366436642e706e67\" alt=\"Fork me on GitHub\" data-canonical-src=\"https://s3.amazonaws.com/github/ribbons/forkme_right_gray_6d6d6d.png\">\r\n</a>\r\n<h1 class=\"azure parent\">Azure Storage web explorer</h1>\r\n<p></p>\r\n<div class=\"parent\">\r\n\t<div class=\"child ui card\">\r\n\t\t<div class=\"content\">\r\n\t\t\t<div class=\"header\">Sign in</div>\r\n\t\t\t<div class=\"meta\">Sign in with your Azure Key or Shared Access Signature</div>\r\n\t\t\t<div class=\"description\">\r\n\t\t\t\t<div class=\"ui left icon input\">\r\n\t\t\t\t\t<input #azureAccount type=\"text\" placeholder=\"Azure account\" (keyup)=\"typingMessage($event)\">\r\n\t\t\t\t\t<i class=\"user icon\"></i>\r\n\t\t\t\t</div>\r\n\t\t\t\t<p></p>\r\n\t\t\t\t<div class=\"ui left icon input\">\r\n\t\t\t\t\t<input #azureKey type=\"password\" placeholder=\"Key or SAS\" (keyup)=\"typingMessage($event)\">\r\n\t\t\t\t\t<i class=\"key icon\"></i>\r\n\t\t\t\t</div>\r\n\t\t\t\t<p></p>\r\n\t\t\t\t<button class=\"ui active button\" (click)=\"signIn()\">\r\n\t\t\t\t\t<i class=\"user icon\"></i>\r\n\t\t\t\t\tLogin\r\n\t\t\t\t</button>\r\n\r\n\t\t\t\t<div *ngIf=\"showError\" class=\"ui left pointing red basic label\">Invalid account or key</div>\r\n\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n<version class=\"parent\"></version>\r\n\r\n<div *ngIf=\"loading\" class=\"ui active text centered inline loader\">\r\n</div>\r\n\r\n";

/***/ }),
/* 36 */
/***/ (function(module, exports) {

module.exports = "<div class=\"ui pointing menu\">\r\n\t<a [routerLinkActive]=\"['active']\" class=\"item\" [routerLink]=\"['/home']\">\r\n\t\tHome\r\n\t</a>\r\n\t<a [routerLinkActive]=\"['active']\" class=\"item\" [routerLink]=\"['/containers']\">\r\n\t\tBlobs\r\n\t</a>\r\n\t<a [routerLinkActive]=\"['active']\" class=\"item\" [routerLink]=\"['/queues']\">\r\n\t\tQueues\r\n\t</a>\r\n\t<a [routerLinkActive]=\"['active']\" class=\"item\" [routerLink]=\"['/tables']\">\r\n\t\tTables\r\n\t</a>\r\n\t<a [routerLinkActive]=\"['active']\" class=\"item\" [routerLink]=\"['/files']\">\r\n\t\tFiles\r\n\t</a>\r\n\t<!-- The logout is still not implemented\r\n\t<div class=\"right menu\">\r\n\t\t<a class=\"item\" (click)=\"logOut($event)\">Logout</a>\r\n\t</div>\r\n\t-->\r\n</div>";

/***/ }),
/* 37 */
/***/ (function(module, exports) {

module.exports = "<div *ngIf=\"loading\" class=\"ui active text centered inline loader\">\r\n\t<em>Fetching data from {{ queue }}...</em>\r\n</div>\r\n\r\n\r\n<table class=\"ui blue table\" *ngIf=\"showTable\">\r\n\t<thead>\r\n\t\t<tr>\r\n\t\t\t<th>\r\n\t\t\t\t<button *ngIf=\"!removeQueueFlag\" class=\"ui icon button\" (click)=\"removeQueue($event)\">\r\n\t\t\t\t\t<i class=\"trash icon\"></i>\r\n\t\t\t\t</button>\r\n\t\t\t\t<div *ngIf=\"removeQueueFlag\" class=\"ui buttons\">\r\n\t\t\t\t\t<button class=\"ui button\" (click)=\"cancelDeleteQueue()\">Cancel</button>\r\n\t\t\t\t\t<div class=\"or\"></div>\r\n\t\t\t\t\t<button class=\"ui negative button\" (click)=\"deleteQueue()\">Delete</button>\r\n\t\t\t\t</div>\r\n\t\t\t</th>\r\n\t\t\t<th>\r\n\t\t\t\t<div style=\"display:flex\" class=\"ui input\">\r\n\t\t\t\t\t<input #newMessage type=\"text\" placeholder=\"New message\" (keyup)=\"typingMessage($event)\">\r\n\t\t\t\t\t<button title=\"Add message\" type=\"button\" (click)=\"addMessage()\">Add Message</button>\r\n\t\t\t\t</div>\r\n\t\t\t</th>\r\n</thead>\r\n\t<tbody>\r\n\t\t<tr *ngFor=\"let message of messages\">\r\n\t\t\t<td class=\"collapsing\">\r\n\t\t\t\t<button *ngIf=\"selected != message.key\" title=\"Remove message\" class=\"ui icon button\" (click)=\"removeMessage($event)\">\r\n\t\t\t\t\t<i class=\"trash icon\"></i>\r\n\t\t\t\t</button>\r\n\t\t\t\t<div *ngIf=\"selected == message.key\" class=\"ui buttons\">\r\n\t\t\t\t\t<button class=\"ui button\" (click)=\"cancelDeleteMessage()\">Cancel</button>\r\n\t\t\t\t\t<div class=\"or\"></div>\r\n\t\t\t\t\t<button class=\"ui negative button\" (click)=\"deleteMessage()\">Delete</button>\r\n\t\t\t\t</div>\r\n\t\t\t</td>\r\n\t\t\t<td colspan=\"2\">{{ message.value }}</td>\r\n\t\t\t<td hidden=\"hidden\">{{ message.key }}</td>\r\n\t\t</tr>\r\n\t</tbody>\r\n\t<tfoot>\r\n\t\t<tr>\r\n\t\t\t<th colspan=\"2\">{{ messages.length }} messages queued</th>\r\n\t\t</tr>\r\n\t</tfoot>\r\n</table>\r\n\r\n<div *ngIf=\"hasErrors\" class=\"ui negative message errorMessage\">\r\n\t<div class=\"header\">{{ errorMessage }}</div>\r\n</div>";

/***/ }),
/* 38 */
/***/ (function(module, exports) {

module.exports = "<p></p>\r\n\r\n\r\n<div class=\"ui action input\">\r\n\t<input #newQueueName type=\"text\" placeholder=\"New queue...\">\r\n\t<button title=\"Add queue\" class=\"ui icon button\" (click)=\"newQueue($event)\">\r\n\t\t<i class=\"add icon\"></i>\r\n\t</button>\r\n</div>\r\n\r\n<p></p>\r\n\r\n<em *ngIf=\"loading\">Loading queues...</em>\r\n\r\n<div *ngIf=\"queues\" class=\"parent\">\r\n\r\n\t<div #queuesMenu class=\"child ui vertical pointing menu\">\r\n\r\n\t\t<a class=\"link item elipsed\" *ngFor=\"let queue of queues\" (click)=\"queueChanged($event)\">{{queue}}</a>\r\n\t</div>\r\n\r\n\t<qmessages class=\"child\" style=\"padding-left:20px\" [queue]=\"selectedQueue\" (refresh)=\"forceRefresh($event)\"></qmessages>\r\n\r\n</div>\r\n\r\n<div *ngIf=\"hasErrors\" class=\"ui negative message errorMessage\">\r\n\t<div class=\"header\">{{ errorMessage }}</div>\r\n</div>";

/***/ }),
/* 39 */
/***/ (function(module, exports) {

module.exports = "\r\n\r\n<p></p>\r\n\r\n<!--<div class=\"ui action input\">\r\n\t<input #newShareName type=\"text\" placeholder=\"New share...\">\r\n\t<button class=\"ui icon button\" (click)=\"newShare($event)\">\r\n\t\t<i class=\"add icon\"></i>\r\n\t</button>\r\n</div>-->\r\n\r\n<p></p>\r\n\r\n<em *ngIf=\"loading\">Loading share files...</em>\r\n\r\n<div *ngIf=\"shares\" class=\"parent\">\r\n\r\n\t<div #containersMenu class=\"child ui vertical pointing menu\">\r\n\r\n\t\t<a class=\"link item elipsed\" *ngFor=\"let share of shares\" (click)=\"sharedChanged($event)\">{{share}}</a>\r\n\t</div>\r\n\r\n\t<files class=\"child\" style=\"padding-left:20px\" [share]=\"selectedShare\" (refresh)=\"forceRefresh($event)\"></files>\r\n\r\n</div>\r\n\r\n<div *ngIf=\"hasErrors\" class=\"ui negative message errorMessage\">\r\n\t<div class=\"header\">{{ errorMessage }}</div>\r\n</div>";

/***/ }),
/* 40 */
/***/ (function(module, exports) {

module.exports = "\r\n<div class=\"ui action input fullWidth\">\r\n\t<input #inputQuery type=\"text\" placeholder=\"Search pattern...\">\r\n\t<select #mode class=\"ui compact selection dropdown\" (change)=\"modeChanged()\">\r\n\t\t<option value=\"q\" selected=\"\">Search</option>\r\n\t\t<option value=\"i\">Insert</option>\r\n\t</select>\r\n\t<div class=\"ui button\" (click)=\"queryData()\">Go</div>\r\n</div>\r\n\r\n<div *ngIf=\"loading\" class=\"ui active text centered inline loader\">\r\n\t<em>Fetching data from {{ storageTable }}...</em>\r\n</div>\r\n\r\n<table class=\"ui blue table\" *ngIf=\"showTable\">\r\n\t<thead>\r\n\t\t<tr>\r\n\t\t\t<th>\r\n\t\t\t\t<button *ngIf=\"!removeTableFlag\" class=\"ui icon button\" (click)=\"removeTable($event)\">\r\n\t\t\t\t\t<i class=\"trash icon\"></i>\r\n\t\t\t\t</button>\r\n\t\t\t\t<div *ngIf=\"removeTableFlag\" class=\"ui buttons\">\r\n\t\t\t\t\t<button class=\"ui button\" (click)=\"cancelDeleteTable()\">Cancel</button>\r\n\t\t\t\t\t<div class=\"or\"></div>\r\n\t\t\t\t\t<button class=\"ui negative button\" (click)=\"deleteTable()\">Delete</button>\r\n\t\t\t\t</div>\r\n\t\t\t</th>\r\n\r\n\t\t\t<th *ngFor=\"let h of headers\">{{ h }}</th>\r\n\t\t</tr>\r\n\t</thead>\r\n\t<tbody>\r\n\t\t<tr *ngFor=\"let row of rows\">\r\n\r\n\t\t\t<td>\r\n\t\t\t\t<button class=\"ui icon button\" (click)=\"removeRow($event)\">\r\n\t\t\t\t\t<i class=\"trash icon\"></i>\r\n\t\t\t\t</button>\r\n\t\t\t</td>\r\n\r\n\t\t\t<td *ngFor=\"let h of headers\">\r\n\t\t\t\t{{ row[h] }}\r\n\t\t\t</td>\r\n\t\t</tr>\r\n\t</tbody>\r\n\t<tfoot>\r\n\t\t<tr>\r\n\t\t\t<th [attr.colspan]=\"headers.length + 1\">{{ rows.length }} rows found</th>\r\n\t\t</tr>\r\n\t</tfoot>\r\n</table>\r\n\r\n<div *ngIf=\"hasErrors\" class=\"ui negative message errorMessage\">\r\n\t<div class=\"header\">{{ errorMessage }}</div>\r\n</div>";

/***/ }),
/* 41 */
/***/ (function(module, exports) {

module.exports = "<p></p>\r\n\r\n\r\n<div class=\"ui action input\">\r\n\t<input #newTableName type=\"text\" placeholder=\"New table...\">\r\n\t<button title=\"Add table\" class=\"ui icon button\" (click)=\"newTable($event)\">\r\n\t\t<i class=\"add icon\"></i>\r\n\t</button>\r\n</div>\r\n\r\n<p></p>\r\n\r\n<em *ngIf=\"loading\">Loading tables...</em>\r\n\r\n<div *ngIf=\"tables\" class=\"parent\">\r\n\r\n\t<div #tablesMenu class=\"child ui vertical pointing menu\">\r\n\t\t<a class=\"link item elipsed\" *ngFor=\"let row of tables\" (click)=\"tableChanged($event)\">{{row}}</a>\r\n\t</div>\r\n\t\r\n\t<tabledata class=\"child\" style=\"padding-left:20px\" [storageTable]=\"selectedTable\" (refresh)=\"forceRefresh($event)\"></tabledata>\r\n\t\t\r\n</div>\r\n\r\n<div *ngIf=\"hasErrors\" class=\"ui negative message errorMessage\">\r\n\t<div class=\"header\">{{ errorMessage }}</div>\r\n</div>";

/***/ }),
/* 42 */
/***/ (function(module, exports) {

module.exports = "<div class=\"version\">v {{currentVersion}} by <a href=\"https://twitter.com/sebagomez\">@sebagomez</a></div>";

/***/ }),
/* 43 */
/***/ (function(module, exports, __webpack_require__) {


        var result = __webpack_require__(27);

        if (typeof result === "string") {
            module.exports = result;
        } else {
            module.exports = result.toString();
        }
    

/***/ }),
/* 44 */
/***/ (function(module, exports, __webpack_require__) {


        var result = __webpack_require__(28);

        if (typeof result === "string") {
            module.exports = result;
        } else {
            module.exports = result.toString();
        }
    

/***/ }),
/* 45 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(23);

/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(38);

/***/ }),
/* 47 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(41);

/***/ }),
/* 48 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(6);

/***/ }),
/* 49 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(8);

/***/ }),
/* 50 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(2))(9);

/***/ })
/******/ ]);
//# sourceMappingURL=main-client.js.map
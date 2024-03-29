import { watch, unref, inject, computed, watchEffect, Vue2, defineComponent, shallowRef, toRefs, getCurrentInstance, onMounted, onBeforeUnmount, h, nextTick } from 'vue-demi';
import { throttle, init } from 'echarts/core';
import { addListener, removeListener } from 'resize-detector';

/******************************************************************************
Copyright (c) Microsoft Corporation.

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR
OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR
PERFORMANCE OF THIS SOFTWARE.
***************************************************************************** */

var __assign = function() {
    __assign = Object.assign || function __assign(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p)) t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};

typeof SuppressedError === "function" ? SuppressedError : function (error, suppressed, message) {
    var e = new Error(message);
    return e.name = "SuppressedError", e.error = error, e.suppressed = suppressed, e;
};

var METHOD_NAMES = [
    "getWidth",
    "getHeight",
    "getDom",
    "getOption",
    "resize",
    "dispatchAction",
    "convertToPixel",
    "convertFromPixel",
    "containPixel",
    "getDataURL",
    "getConnectedDataURL",
    "appendData",
    "clear",
    "isDisposed",
    "dispose"
];
function usePublicAPI(chart) {
    function makePublicMethod(name) {
        return function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            if (!chart.value) {
                throw new Error("ECharts is not initialized yet.");
            }
            return chart.value[name].apply(chart.value, args);
        };
    }
    function makePublicMethods() {
        var methods = Object.create(null);
        METHOD_NAMES.forEach(function (name) {
            methods[name] = makePublicMethod(name);
        });
        return methods;
    }
    return makePublicMethods();
}

function useAutoresize(chart, autoresize, root) {
    var resizeListener = null;
    watch([root, chart, autoresize], function (_a, _, cleanup) {
        var root = _a[0], chart = _a[1], autoresize = _a[2];
        if (root && chart && autoresize) {
            var autoresizeOptions = autoresize === true ? {} : autoresize;
            var _b = autoresizeOptions.throttle, wait = _b === void 0 ? 100 : _b, onResize_1 = autoresizeOptions.onResize;
            var callback = function () {
                chart.resize();
                onResize_1 === null || onResize_1 === void 0 ? void 0 : onResize_1();
            };
            resizeListener = wait ? throttle(callback, wait) : callback;
            addListener(root, resizeListener);
        }
        cleanup(function () {
            if (root && resizeListener) {
                removeListener(root, resizeListener);
            }
        });
    });
}
var autoresizeProps = {
    autoresize: [Boolean, Object]
};

var onRE = /^on[^a-z]/;
var isOn = function (key) { return onRE.test(key); };
function omitOn(attrs) {
    var result = {};
    for (var key in attrs) {
        if (!isOn(key)) {
            result[key] = attrs[key];
        }
    }
    return result;
}
function unwrapInjected(injection, defaultValue) {
    var value = unref(injection);
    if (value && typeof value === "object" && "value" in value) {
        return value.value || defaultValue;
    }
    return value || defaultValue;
}

var LOADING_OPTIONS_KEY = "ecLoadingOptions";
function useLoading(chart, loading, loadingOptions) {
    var defaultLoadingOptions = inject(LOADING_OPTIONS_KEY, {});
    var realLoadingOptions = computed(function () { return (__assign(__assign({}, unwrapInjected(defaultLoadingOptions, {})), loadingOptions === null || loadingOptions === void 0 ? void 0 : loadingOptions.value)); });
    watchEffect(function () {
        var instance = chart.value;
        if (!instance) {
            return;
        }
        if (loading.value) {
            instance.showLoading(realLoadingOptions.value);
        }
        else {
            instance.hideLoading();
        }
    });
}
var loadingProps = {
    loading: Boolean,
    loadingOptions: Object
};

var registered = null;
var TAG_NAME = "x-vue-echarts";
function register() {
    if (registered != null) {
        return registered;
    }
    if (typeof HTMLElement === "undefined" ||
        typeof customElements === "undefined") {
        return (registered = false);
    }
    try {
        var reg = new Function("tag", "class EChartsElement extends HTMLElement {\n  __dispose = null;\n\n  disconnectedCallback() {\n    if (this.__dispose) {\n      this.__dispose();\n      this.__dispose = null;\n    }\n  }\n}\n\nif (customElements.get(tag) == null) {\n  customElements.define(tag, EChartsElement);\n}\n");
        reg(TAG_NAME);
    }
    catch (e) {
        return (registered = false);
    }
    return (registered = true);
}

var e=[],t=[];function n(n,r){if(n&&"undefined"!=typeof document){var a,s=!0===r.prepend?"prepend":"append",d=!0===r.singleTag,i="string"==typeof r.container?document.querySelector(r.container):document.getElementsByTagName("head")[0];if(d){var u=e.indexOf(i);-1===u&&(u=e.push(i)-1,t[u]={}),a=t[u]&&t[u][s]?t[u][s]:t[u][s]=c();}else a=c();65279===n.charCodeAt(0)&&(n=n.substring(1)),a.styleSheet?a.styleSheet.cssText+=n:a.appendChild(document.createTextNode(n));}function c(){var e=document.createElement("style");if(e.setAttribute("type","text/css"),r.attributes)for(var t=Object.keys(r.attributes),n=0;n<t.length;n++)e.setAttribute(t[n],r.attributes[t[n]]);var a="prepend"===s?"afterbegin":"beforeend";return i.insertAdjacentElement(a,e),e}}

var css = "x-vue-echarts{display:block;width:100%;height:100%;min-width:0}x-vue-echarts>div{width:100%;height:100%}\n";
n(css,{});

var wcRegistered = register();
if (Vue2) {
    Vue2.config.ignoredElements.push(TAG_NAME);
}
var THEME_KEY = "ecTheme";
var INIT_OPTIONS_KEY = "ecInitOptions";
var UPDATE_OPTIONS_KEY = "ecUpdateOptions";
var ECharts = defineComponent({
    name: "echarts",
    props: __assign(__assign({ option: Object, theme: {
            type: [Object, String]
        }, initOptions: Object, updateOptions: Object, group: String, manualUpdate: Boolean }, autoresizeProps), loadingProps),
    emits: {},
    inheritAttrs: false,
    setup: function (props, _a) {
        var attrs = _a.attrs;
        var root = shallowRef();
        var inner = shallowRef();
        var chart = shallowRef();
        var manualOption = shallowRef();
        var defaultTheme = inject(THEME_KEY, null);
        var defaultInitOptions = inject(INIT_OPTIONS_KEY, null);
        var defaultUpdateOptions = inject(UPDATE_OPTIONS_KEY, null);
        var _b = toRefs(props), autoresize = _b.autoresize, manualUpdate = _b.manualUpdate, loading = _b.loading, loadingOptions = _b.loadingOptions;
        var realOption = computed(function () { return manualOption.value || props.option || null; });
        var realTheme = computed(function () { return props.theme || unwrapInjected(defaultTheme, {}); });
        var realInitOptions = computed(function () { return props.initOptions || unwrapInjected(defaultInitOptions, {}); });
        var realUpdateOptions = computed(function () { return props.updateOptions || unwrapInjected(defaultUpdateOptions, {}); });
        var nonEventAttrs = computed(function () { return omitOn(attrs); });
        var listeners = getCurrentInstance().proxy.$listeners;
        function init$1(option) {
            if (!inner.value) {
                return;
            }
            var instance = (chart.value = init(inner.value, realTheme.value, realInitOptions.value));
            if (props.group) {
                instance.group = props.group;
            }
            var realListeners = listeners;
            if (!realListeners) {
                realListeners = {};
                Object.keys(attrs)
                    .filter(function (key) { return key.indexOf("on") === 0 && key.length > 2; })
                    .forEach(function (key) {
                    var event = key.charAt(2).toLowerCase() + key.slice(3);
                    if (event.substring(event.length - 4) === "Once") {
                        event = "~".concat(event.substring(0, event.length - 4));
                    }
                    realListeners[event] = attrs[key];
                });
            }
            Object.keys(realListeners).forEach(function (key) {
                var handler = realListeners[key];
                if (!handler) {
                    return;
                }
                var event = key.toLowerCase();
                if (event.charAt(0) === "~") {
                    event = event.substring(1);
                    handler.__once__ = true;
                }
                var target = instance;
                if (event.indexOf("zr:") === 0) {
                    target = instance.getZr();
                    event = event.substring(3);
                }
                if (handler.__once__) {
                    delete handler.__once__;
                    var raw_1 = handler;
                    handler = function () {
                        var args = [];
                        for (var _i = 0; _i < arguments.length; _i++) {
                            args[_i] = arguments[_i];
                        }
                        raw_1.apply(void 0, args);
                        target.off(event, handler);
                    };
                }
                target.on(event, handler);
            });
            function resize() {
                if (instance && !instance.isDisposed()) {
                    instance.resize();
                }
            }
            function commit() {
                var opt = option || realOption.value;
                if (opt) {
                    instance.setOption(opt, realUpdateOptions.value);
                }
            }
            if (autoresize.value) {
                nextTick(function () {
                    resize();
                    commit();
                });
            }
            else {
                commit();
            }
        }
        function setOption(option, updateOptions) {
            if (props.manualUpdate) {
                manualOption.value = option;
            }
            if (!chart.value) {
                init$1(option);
            }
            else {
                chart.value.setOption(option, updateOptions || {});
            }
        }
        function cleanup() {
            if (chart.value) {
                chart.value.dispose();
                chart.value = undefined;
            }
        }
        var unwatchOption = null;
        watch(manualUpdate, function (manualUpdate) {
            if (typeof unwatchOption === "function") {
                unwatchOption();
                unwatchOption = null;
            }
            if (!manualUpdate) {
                unwatchOption = watch(function () { return props.option; }, function (option, oldOption) {
                    if (!option) {
                        return;
                    }
                    if (!chart.value) {
                        init$1();
                    }
                    else {
                        chart.value.setOption(option, __assign({ notMerge: option !== oldOption }, realUpdateOptions.value));
                    }
                }, { deep: true });
            }
        }, {
            immediate: true
        });
        watch([realTheme, realInitOptions], function () {
            cleanup();
            init$1();
        }, {
            deep: true
        });
        watchEffect(function () {
            if (props.group && chart.value) {
                chart.value.group = props.group;
            }
        });
        var publicApi = usePublicAPI(chart);
        useLoading(chart, loading, loadingOptions);
        useAutoresize(chart, autoresize, inner);
        onMounted(function () {
            init$1();
        });
        onBeforeUnmount(function () {
            if (wcRegistered && root.value) {
                root.value.__dispose = cleanup;
            }
            else {
                cleanup();
            }
        });
        return __assign({ chart: chart, root: root, inner: inner, setOption: setOption, nonEventAttrs: nonEventAttrs }, publicApi);
    },
    render: function () {
        var attrs = (Vue2 ? { attrs: this.nonEventAttrs } : __assign({}, this.nonEventAttrs));
        attrs.ref = "root";
        attrs["class"] = attrs["class"] ? ["echarts"].concat(attrs["class"]) : "echarts";
        return h(TAG_NAME, attrs, [h("div", { ref: "inner" })]);
    }
});

export { INIT_OPTIONS_KEY, LOADING_OPTIONS_KEY, THEME_KEY, UPDATE_OPTIONS_KEY, ECharts as default };
//# sourceMappingURL=index.esm.js.map

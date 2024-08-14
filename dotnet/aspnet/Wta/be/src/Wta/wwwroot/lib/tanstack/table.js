/**
 * Bundled by jsDelivr using Rollup v2.79.1 and Terser v5.19.2.
 * Original file: /npm/@tanstack/vue-table@8.20.4/build/lib/index.mjs
 *
 * Do NOT use SRI with dynamically generated files! More information: https://www.jsdelivr.com/using-sri-with-dynamic-files
 */
import{createTable as e}from"./table-core.js";export*from"./table-core.js";import{defineComponent as t,h as n,isRef as r,shallowRef as o,watch as a,ref as s,watchEffect as u,unref as l}from"vue";
/**
   * vue-table
   *
   * Copyright (c) TanStack
   *
   * This source code is licensed under the MIT license found in the
   * LICENSE.md file in the root directory of this source tree.
   *
   * @license MIT
   */function c(){return!0}const i=Symbol("merge-proxy"),p={get:(e,t,n)=>t===i?n:e.get(t),has:(e,t)=>e.has(t),set:c,deleteProperty:c,getOwnPropertyDescriptor:(e,t)=>({configurable:!0,enumerable:!0,get:()=>e.get(t),set:c,deleteProperty:c}),ownKeys:e=>e.keys()};function f(e){return"value"in e?e.value:e}function g(){for(var e=arguments.length,t=new Array(e),n=0;n<e;n++)t[n]=arguments[n];return new Proxy({get(e){for(let n=t.length-1;n>=0;n--){const r=f(t[n])[e];if(void 0!==r)return r}},has(e){for(let n=t.length-1;n>=0;n--)if(e in f(t[n]))return!0;return!1},keys(){const e=[];for(let n=0;n<t.length;n++)e.push(...Object.keys(f(t[n])));return[...Array.from(new Set(e))]}},p)}const m=t({props:["render","props"],setup:e=>()=>"function"==typeof e.render||"object"==typeof e.render?n(e.render,e.props):e.render});function d(e){return g(e,{data:l(e.data)})}function y(t){const n=r(t.data),l=g({state:{},onStateChange:()=>{},renderFallbackValue:null,mergeOptions:(e,t)=>n?{...e,...t}:g(e,t)},n?d(t):t),c=e(l);if(n){const e=o(t.data);a(e,(()=>{c.setState((t=>({...t,data:e.value})))}),{immediate:!0})}const i=s(c.initialState);return u((()=>{c.setOptions((e=>{var r;const o=new Proxy({},{get:(e,t)=>i.value[t]});return g(e,n?d(t):t,{state:g(o,null!=(r=t.state)?r:{}),onStateChange:e=>{i.value=e instanceof Function?e(i.value):e,null==t.onStateChange||t.onStateChange(e)}})}))})),c}export{m as FlexRender,y as useVueTable};export default null;
//# sourceMappingURL=/sm/05f0b64ffa2ee5d11f28e4dc9381c843fdd72f925acda8942cd410e8646271a2.map

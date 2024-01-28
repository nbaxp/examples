# vue

<https://cn.vuejs.org/>

1. 组件
    1. 生命周期
    1. ESM组件
    1. SFC组件
1. 模板
    1. 基础语法 
1. 响应式

```mermaid
graph LR
vue-->v1(基于标准 HTML、CSS 和 JavaScript 构建)
vue-->v2(用于构建用户界面的 JavaScript 框架)
vue-->声明式渲染
vue-->响应式
vue-->组件化
```

## 组件

ESM 组件可以直接使用，单文件组件（SFC）必须构建

### 生命周期

```mermaid
graph LR
生命周期-->beforeCreate
生命周期-->created
生命周期-->beforeMount
生命周期-->mounted
生命周期-->beforeUnmount
生命周期-->unmounted
```

### ESM 组件

```js
import { ref } from 'vue';

export default {
    components:{},
    template: `<p class="greeting">{{ greeting }}</p>`,
    props:[],
    emit:[]
    setup(props,context){
        const greeting = ref('Hello World!');
        return {
            greeting
        };
    }
}
```

### 单文件组件

```html
<script setup>
    import { ref } from "vue";
    const greeting = ref("Hello World!");
</script>

<template>
    <p class="greeting">{{ greeting }}</p>
</template>

<style>
    .greeting {
        color: red;
        font-weight: bold;
    }
</style>
```

## 模板

### 语法

```mermaid
graph LR
模板语法-->文本插值
模板语法-->原始HTML
模板语法-->Attribute绑定
模板语法-->JavaScript表达式
模板语法-->函数调用
模板语法-->受限的全局访问
模板语法-->指令Directives
模板语法-->修饰符Modifiers
```

### 模板引用

```mermaid
graph LR
模板引用-->在组件挂载后才能访问模板引用
模板引用-->声明-->单个-->声明和模板ref属性同名变量
声明-->v-for-->声明ref属性同名数组
v-for-->ref数组并不保证与源数组相同的顺序
模板引用-->函数模板引用
模板引用-->组件上的ref
```

### 插槽

```mermaid
graph LR
插槽-->使用slot定义
插槽-->插槽内容由父元素提供
插槽-->具名插槽
插槽-->动态插槽名
插槽-->作用域插槽-->使用attributes传递
作用域插槽-->接收-->默认插槽-->v-slot定义接收对象或解构的变量-->#default
接收-->剧名插槽-->v-slot:name-->v("#name")
```

## 响应式

推荐使用 ref() 函数来声明响应式状态

```mermaid
graph LR
声明响应式状态-->ref
声明响应式状态-->reactive
reactive-->只能用于对象类型
reactive-->不能替换整个对象
reactive-->对解构操作不友好
```

```mermaid
graph LR
计算属性-->使用computed定义
计算属性-->计算属性值会基于其响应式依赖被缓存
计算属性-->可写计算属性-->同时提供getter和setter来创建
同时提供getter和setter来创建-->Getter不应有副作用
同时提供getter和setter来创建-->避免直接修改计算属性值
```


```mermaid
graph LR
侦听器-->响应式状态发生变化时触发
响应式状态发生变化时触发-->watch-->只追踪参数指定的数据源
watch-->immediate-->创建侦听器时立即执行回调
响应式状态发生变化时触发-->watchEffect-->自动追踪回调中所有能访问到的响应式属性
watchEffect-->创建时立即执行回调
watchEffect-->flush-->回调中能访问被Vue更新之后的DOM
响应式状态发生变化时触发-->watchPostEffect-->watchEffect+flush
侦听器-->停止侦听器-->同步语句创建的会自动停止
停止侦听器-->异步语句创建的必须手动调用返回的函数停止-->不推荐异步方式
```

```mermaid
graph LR
事件处理-->监听事件
监听事件-->内联事件处理器-->在内联处理器中调用方法-->传递参数-->原生DOM事件$event
监听事件-->方法事件处理器-->使用方法名
事件处理-->事件修饰符-->.stop
事件修饰符-->.prevent
事件修饰符-->.self
事件修饰符-->.capture
事件修饰符-->.once
事件修饰符-->.passive
```

<template>
    <view class="flex flex-direction-column page">
        <scroll-view class="flex-1 overflow-y-auto" scroll-y>
            <view class="scroll-content">
                <view v-for="i in 3" :key="i" class="item">地址 {{ i }}</view>
            </view>
        </scroll-view>
        <view class="flex-shrink-0">
            <navigator class="text-align-center" url="/pages/address/add">
                <uni-list-item title="新增地址" />
            </navigator>
        </view>
    </view>
</template>

<script setup>
    import {
        onMounted
    } from 'vue';
    onMounted(() => {
        uni.$on('city', o => {
            console.log(o);
        })
    });
</script>

<style>

</style>
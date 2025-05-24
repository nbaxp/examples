<template>
    <view class="flex flex-direction-column h-page">
        <view class="flex-shrink-0 flex justify-content-between padding-15"><text>全部：</text></view>
        <scroll-view class="flex-1 overflow-y-auto" scroll-y>
            <view class="scroll-content">
                <view v-for="i in 50" :key="i" class="item">内容 {{ i }}</view>
            </view>
        </scroll-view>
        <view class="flex-shrink-0 flex justify-content-between padding-15">
            <label class="radio">
                <radio />全选
            </label>
            <text>合计：</text>
            <text><button size="mini">结算</button></text>
        </view>
    </view>
</template>

<script setup>

</script>

<style>

</style>
<template>
    <view class="flex flex-direction-column page">
        <view class="flex-shrink-0 flex justify-content-between padding-15"><text>全部：</text></view>
        <scroll-view class="flex-1 overflow-y-auto" scroll-y>
            <view class="scroll-content">
                <view v-for="i in 50" :key="i" class="item">内容 {{ i }}</view>
            </view>
        </scroll-view>
        <view class="flex-shrink-0 inline-flex align-items-center justify-content-between padding-15">
            <label class="radio">
                <radio />全选
            </label>
            <text>合计：</text>
            <navigator url="/pages/payment/payment"><button class="margin-0" type="primary" size="mini">结算</button>
            </navigator>
        </view>
    </view>
</template>

<script setup>

</script>

<style>
</style>
<template>
    <view>
        <uni-row>
            <view class="swiper-wrap">
                <swiper class="swiper" circular autoplay :interval="3000">
                    <swiper-item>
                        <view class="swiper-item" style="background-color: green;">A</view>
                    </swiper-item>
                    <swiper-item>
                        <view class="swiper-item" style="background-color: lightblue;">B</view>
                    </swiper-item>
                    <swiper-item>
                        <view class="swiper-item" style="background-color: yellow;">C</view>
                    </swiper-item>
                </swiper>
            </view>
        </uni-row>
        <uni-grid :show-border="false" :column="5" @change="change">
            <uni-grid-item v-for="(item ,index) in list" :index="index" :key="index">
                <view class="w-full flex-1 flex flex-direction-column align-items-center justify-content-center">
                    <uni-icons type="image" :size="30" />
                    <text class="text">分类名称</text>
                </view>
            </uni-grid-item>
        </uni-grid>
        <uni-row>
            <uni-grid :column="2" :show-border="false" :square="false" @change="change">
                <uni-grid-item v-for="(item ,index) in list" :index="index" :key="index">
                    <view class="w-full flex-1 flex align-items-center padding-15">
                        <uni-icons type="image" :size="60" />
                        <text class="text">商品</text>
                    </view>
                </uni-grid-item>
            </uni-grid>
        </uni-row>
    </view>
</template>

<script setup>
    import {
        ref
    } from 'vue';

    const list = ref([]);
    for (let i = 0; i < 12; i++) {
        list.value.push({
            url: '/static/logo.png',
            text: 'Grid 1',
            badge: '0',
            type: "primary"
        });
    }
</script>

<style>
    .swiper-wrap {
        width: 690rpx;
        width: 100%;
    }

    .swiper {
        height: 300rpx;
    }

    .swiper-item {
        display: block;
        height: 300rpx;
        line-height: 300rpx;
        text-align: center;
    }


    .image {
        width: 25px;
        height: 25px;
    }

    .text {
        font-size: 14px;
        margin-top: 5px;
    }
</style>
<template>
    <view>
        <uni-card>
            <view class="flex align-items-center justify-content-between">
                <image class="avatar-image" mode="aspectFit" :src="avatar" />
                <button class="margin-right-0" type="primary" size="mini" v-if="token" @click="logout">退出</button>
                <button class="margin-right-0" type="primary" size="mini" v-else @click="login">登录</button>
            </view>
        </uni-card>
        <uni-card padding="0">
            <uni-list>
                <uni-list-item title="我的订单" showArrow />
                <uni-list-item title="收货地址" showArrow />
                <uni-list-item title="个人资料" showArrow />
                <uni-list-item title="我的收藏" showArrow />
                <uni-list-item title="我的足迹" showArrow />
                <uni-list-item title="积分、优惠券、卡券" showArrow />
            </uni-list>
        </uni-card>
    </view>
</template>

<script setup>
    import {
        onShow
    } from "@dcloudio/uni-app";
    import {
        ref
    } from 'vue';

    const tokenKey = 'token';

    const token = ref(null);

    const avatar = '/static/images/avatar.png';

    const login = () => {
        if (!token.value) {
            uni.navigateTo({
                url: '/pages/login/login'
            })
        }
    };

    const logout = () => {
        uni.clearStorageSync(tokenKey)
        token.value = null;
    };

    onShow(() => {
        console.log('test');
        token.value = uni.getStorageSync(tokenKey);
    });
</script>

<style>
    .avatar-image {
        width: 40px;
        height: 40px;
        border-radius: 50%;
    }

    .avatar-button {
        margin-left: .5rem;
    }
</style>
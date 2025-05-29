<template>
    <view>
        <view class="flex align-items-center justify-content-between padding-15">
            <image class="avatar-image" mode="aspectFit" :src="avatar" />
            <button class="margin-right-0" type="primary" size="mini" v-if="token" @click="logout">退出</button>
            <button class="margin-right-0" type="primary" size="mini" v-else @click="login">登录</button>
        </view>
        <uni-list>
            <navigator url="/pages/address/address"><uni-list-item showExtraIcon :extraIcon="{type:'location'}"
                    title="我的地址" showArrow />
            </navigator>
        </uni-list>
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
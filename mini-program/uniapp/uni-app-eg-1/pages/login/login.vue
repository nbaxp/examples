<template>
	<view>
		<text class="title">{{uniPlatform}}登录</text>
		<button @click="login">登录</button>
	</view>
</template>

<script setup>
	const uniPlatform = uni.getSystemInfoSync().uniPlatform;

	const login = async () => {
		uni.setStorageSync('token', 'test-token-value');
		uni.navigateBack();
	};

	if (uniPlatform === 'mp-weixin') {

	}
</script>

<style>
</style>
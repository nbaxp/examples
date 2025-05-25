<template>
    <view class="padding-15">
        <uni-forms ref="formRef" :model="model" :rules="rules" validate-trigger="bind" err-show-type="undertext">
            <uni-forms-item name="phoneNumber">
                <uni-easyinput type="number" trim="all" placeholder="输入手机号" v-model="model.phoneNumber" />
            </uni-forms-item>
            <uni-forms-item name="authCode">
                <uni-easyinput type="number" trim="all" placeholder="输入验证码" :clearable="false"
                    v-model="model.authCode" />
                <button class="absolute" size="mini" plain :disabled="!/^\d{11}$/.test(model.phoneNumber)" @click="send"
                    style="top:50%;right:0;transform:translateY(-50%);border:none;">获取验证码</button>
            </uni-forms-item>
            <uni-forms-item>
                <button class="w-full" type="primary" :disabled="!model.agreeWith" @click="submit">登录</button>
            </uni-forms-item>
            <view class="inline-flex w-full align-items-center justify-content-center">
                <radio :checked="model.agreeWith" @click="model.agreeWith=!model.agreeWith" />
                <text>我已阅读并同意</text>
                <navigator class="inline-block" url="/pages/tos/tos">《协议》
                </navigator>
            </view>
        </uni-forms>
    </view>
</template>

<script setup>
    import {
        ref
    } from 'vue';

    const model = ref({
        phoneNumber: null,
        authCode: null,
        platform: uni.getSystemInfoSync().uniPlatform,
        provider: null,
        code: null
    });
    console.log(model.value);
    const rules = ref({
        phoneNumber: {
            rules: [{
                required: true,
                errorMessage: '手机号不能为空'
            }, {
                pattern: /^\d{11}$/,
                errorMessage: '手机号由11位数字组成',
            }]
        },
        authCode: {
            rules: [{
                required: true,
                errorMessage: '验证码不能为空'
            }]
        }
    });

    const formRef = ref(null);

    const submit = async () => {
        console.log(model.value);
        try {
            await formRef.value.validate();
        } catch (error) {
            console.log(error);
        }
    };

    const getPhoneNumber = (e) => {
        console.log(e);
        code.value = e.detail.errMsg;
    };

    const login = async () => {
        // uni.setStorageSync('token', 'test-token-value');
        // uni.navigateBack();
    };

    if (model.value.platform === 'web') {

    } else { //App + 小程序
        uni.getProvider({
            service: 'oauth',
            success(o) {
                model.value.provider = o.provider[0];
                uni.login({
                    provider: model.value.provider,
                    success(o) {
                        model.value.code = o.vaue;
                        if (model.value.platform === 'mp-weixin') {
                            // uni.showLoading({
                            //     mask: true,
                            //     success() {
                            //         //发送 code 到服务端换取 token
                            //         //etc...
                            //         setTimeout(() => {
                            //             //存储token到本地
                            //             uni.setStorageSync('token', code);
                            //             uni.navigateBack();
                            //         }, 2000)
                            //     }
                            // })
                        }
                    }
                });
            }
        });
    }
</script>

<style>
</style>
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
                <button class="w-full" type="primary" :disabled="!agreeWith" @click="submit">登录</button>
            </uni-forms-item>
            <view class="inline-flex w-full align-items-center justify-content-center">
                <checkbox :checked="agreeWith" @click="agree" />
                <text>我已阅读并同意</text>
                <navigator class="inline-block" url="/pages/tos/tos">《协议》1
                </navigator>
                <text v-if="model.value">{{model.value.code}}</text>
            </view>
        </uni-forms>
    </view>
</template>

<script setup>
    import {
        ref,
        onMounted
    } from 'vue';

    const agreeWith = ref(false);

    const model = ref({
        phoneNumber: null,
        authCode: null,
        platform: uni.getSystemInfoSync().uniPlatform,
        provider: null,
        code: null
    });

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

    const agree = () => {
        agreeWith.value = !agreeWith.value;
    }

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

    const load = async () => {
        if (model.value.platform === 'web') {
            console.log(model.vlaue.platform);
        } else { //App + 小程序
            const providerInfo = await uni.getProvider({
                service: 'oauth'
            });
            model.value.provider = providerInfo.provider[0];
            const loginInfo = await uni.login({
                provider: model.value.provider,
            });
            console.log(loginInfo);
            model.value.code = loginInfo.code;
            //在前端测试取得openid
            // if (model.value.platform === 'mp-weixin') {
            //     const url = 'https://api.weixin.qq.com/sns/jscode2session';
            //     const method = 'GET';
            //     const appid = 'wx4296d072ff802b17';
            //     const secret = 'b9123ceb46151b6e5d3cc351b52e6fd0';
            //     const js_code = model.value.code;
            //     const grant_type = 'authorization_code';
            //     const data = {
            //         appid,
            //         secret,
            //         js_code,
            //         grant_type
            //     };
            //     const res = await uni.request({
            //         url,
            //         method,
            //         data
            //     });
            //     console.log(res);
            //     const ipenId = res.data.openid;
            //     console.log(ipenId);
            // }
        }
    };

    onMounted(async () => {
        await load();
    });
    // if (model.value.platform === 'web') {

    // } else { //App + 小程序

    //     uni.getProvider({
    //         service: 'oauth',
    //         success(res) {
    //             model.value.provider = res.provider[0];
    //             uni.login({
    //                 provider: model.value.provider,
    //                 success(o) {
    //                     model.value.code = o.vaue;
    //                     console.log(model.value.code);
    //                     if (model.value.platform === 'mp-weixin') {
    //                         uni.request({
    //                             url: 'https://api.weixin.qq.com/sns/jscode2session',
    //                             method: 'GET',
    //                             data: {
    //                                 appid: 'wx4296d072ff802b17',
    //                                 secret: 'b9123ceb46151b6e5d3cc351b52e6fd0',
    //                                 js_code: model.value.code,
    //                                 grant_type: 'authorization_code'
    //                             },
    //                             success(res) {
    //                                 console.log(res);
    //                             },
    //                             fail(err) {
    //                                 console.error('请求失败!', err);
    //                             }
    //                         })
    //                         // uni.showLoading({
    //                         //     mask: true,
    //                         //     success() {
    //                         //         //发送 code 到服务端换取 token
    //                         //         //etc...
    //                         //         setTimeout(() => {
    //                         //             //存储token到本地
    //                         //             uni.setStorageSync('token', code);
    //                         //             uni.navigateBack();
    //                         //         }, 2000)
    //                         //     }
    //                         // })
    //                     }
    //                 }
    //             });
    //         }
    //     });
    // }
</script>

<style>
</style>
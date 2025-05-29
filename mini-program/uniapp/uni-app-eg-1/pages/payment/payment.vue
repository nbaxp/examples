<template>
    <view>
        支付
    </view>
</template>

<script setup>
    import {
        ref,
        onMounted
    } from 'vue';

    const platform = uni.getSystemInfoSync().uniPlatform;

    const load = async () => {
        if (platform === 'web') {

        } else {
            const providerInfo = await uni.getProvider({
                service: 'payment'
            });
            const provider = providerInfo.provider[0];
            if (provider === 'wxpay') {
                //服务端返回客户端支付需要的参数
                uni.requestPayment({
                    provider,
                    timeStamp: String(Date.now()),
                    nonceStr: 'A1B2C3D4E5',
                    package: 'prepay_id=wx20180101abcdefg',
                    signType: 'MD5',
                    paySign: '',
                    success: function(res) {
                        console.log('支付成功:' + JSON.stringify(res));
                    },
                    fail: function(err) {
                        console.log('支付失败', err);
                        if (err.errMsg.indexOf('cancel') !== -1) {
                            console.log('用户取消支付');
                        } else {
                            console.log('支付失败：', err.errMsg);
                        }
                    }
                })
            } else if (platform === 'alipay') {

            }
        }
    };

    onMounted(async () => {
        await load();
    });
</script>

<style>

</style>